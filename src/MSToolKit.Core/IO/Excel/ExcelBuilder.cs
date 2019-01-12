using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MSToolKit.Core.IO.Excel.Abstraction;
using MSToolKit.Core.IO.Excel.Attributes;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MSToolKit.Core.IO.Excel
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.IO.Excel.Abstraction.IExcelBuilder.
    /// </summary>
    public class ExcelBuilder : IExcelBuilder
    {
        private const string DefaultSheetName = "Sheet";
        private const string SheetHeaderFontFamily = "Times New Roman";
        private const int SheetHeaderFontSize = 14;
        private const int SheetBodyFontSize = 12;
        private int currentRow = 1;

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <typeparam name="TSource">The type, that should be represented in the worksheet.</typeparam>
        /// <param name="source">The collection of elements, to be inserted in the worksheet.</param>
        /// <param name="sheetName">(optional) The name of the sheet to build.</param>
        /// <returns>The built worksheet's bytes.</returns>
        public IEnumerable<byte> BuildExcelWorksheet<TSource>(
            IEnumerable<TSource> source, string sheetName = DefaultSheetName)
            where TSource : class
        {
            using (var application = new ExcelPackage())
            {
                application.Workbook.Properties.Title = sheetName ?? DefaultSheetName;
                var sheet = application.Workbook.Worksheets.Add(sheetName ?? DefaultSheetName);

                var allowedProperties = typeof(TSource)
                    .GetProperties()
                    .Select(property => new
                    {
                        Property = property,
                        Attributes = property.GetCustomAttributes(false)
                    })
                    .Where(o => o.Attributes
                        .All(attribute => !(attribute is ExcelIgnoreAttribute)))
                    .ToDictionary(a => a.Property, a => a.Attributes.Cast<Attribute>());

                var headerFont = new Font(SheetHeaderFontFamily, SheetHeaderFontSize, FontStyle.Bold);
                this.GenerateExcelHeader(sheet, allowedProperties, headerFont, Color.SkyBlue);

                var bodyFont = new Font(SheetHeaderFontFamily, SheetBodyFontSize);
                this.GenerateExcelBody(sheet, source, allowedProperties, bodyFont);

                this.FormatSheet(sheet);
                this.currentRow = 1;

                return application.GetAsByteArray();
            }
        }

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <param name="multiCollections">
        /// The collections of different elements, to be inserted in the worksheet.
        /// </param>
        /// <param name="sheetName">
        /// (optional) The name of the sheet to build.
        /// </param>
        /// <returns>The built worksheet's bytes.</returns>
        public IEnumerable<byte> BuildComplexExcelWorksheet(
            IEnumerable<IEnumerable<object>> multiCollections, string sheetName = DefaultSheetName)
        {
            using (var application = new ExcelPackage())
            {
                application.Workbook.Properties.Title = sheetName ?? DefaultSheetName;
                var sheet = application.Workbook.Worksheets.Add(sheetName ?? DefaultSheetName);
                var headerFont = new Font(SheetHeaderFontFamily, SheetHeaderFontSize, FontStyle.Bold);
                var bodyFont = new Font(SheetHeaderFontFamily, SheetBodyFontSize);

                foreach (var array in multiCollections)
                {
                    if (!array.Any())
                    {
                        continue;
                    }

                    var allowedProperties = array
                        .First()
                        .GetType()
                    .GetProperties()
                    .Select(property => new
                    {
                        Property = property,
                        Attributes = property.GetCustomAttributes(true)
                    })
                    .Where(o => o.Attributes
                        .All(attribute => !(attribute is ExcelIgnoreAttribute)))
                    .ToDictionary(a => a.Property, a => a.Attributes.Cast<Attribute>());

                    this.GenerateExcelHeader(sheet, allowedProperties, headerFont, Color.SkyBlue);

                    this.GenerateExcelBody(sheet, array, allowedProperties, bodyFont);
                }

                this.FormatSheet(sheet);

                this.currentRow = 1;
                return application.GetAsByteArray();
            }
        }

        private void FormatSheet(ExcelWorksheet sheet)
        {
            sheet.Cells.AutoFitColumns();
        }

        private void GenerateExcelBody<TSource>(
            ExcelWorksheet sheet,
            IEnumerable<TSource> source,
            Dictionary<PropertyInfo, IEnumerable<Attribute>> properties,
            Font font)
        {
            foreach (var item in source)
            {
                var currentColumn = 1;
                foreach (var property in properties)
                {
                    var currentCell = sheet.Cells[this.currentRow, currentColumn];
                    var propertyValue = property.Key.GetValue(item);

                    var formatAttribute = property.Value
                        .FirstOrDefault(attr => typeof(ExcelValueFormatAttribute)
                            .IsAssignableFrom(attr.GetType())) as ExcelValueFormatAttribute;

                    var isPropertyFormattable = typeof(IFormattable).IsAssignableFrom(property.Key.PropertyType);

                    if (formatAttribute != null && isPropertyFormattable)
                    {
                        propertyValue = ((IFormattable)propertyValue)
                            .ToString(formatAttribute.Format, CultureInfo.InvariantCulture);
                    }

                    currentCell.Value = propertyValue;
                    currentCell.Style.Font.SetFromFont(font);
                    currentCell.Style.Indent = 1;
                    currentCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    currentCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    currentCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    currentCell.Style.WrapText = false;

                    currentColumn++;
                }

                this.currentRow++;
            }

            this.currentRow++;
        }

        private void GenerateExcelHeader(
            ExcelWorksheet sheet,
            Dictionary<PropertyInfo, IEnumerable<Attribute>> properties,
            Font font,
            Color color)
        {
            var currentColumn = 1;

            foreach (var property in properties)
            {
                var currentCell = sheet.Cells[this.currentRow, currentColumn];

                var displayName = property.Value.Any(attr => attr.GetType() == typeof(ExcelDisplayNameAttribute))
                    ? ((ExcelDisplayNameAttribute)property.Value.First(attr => attr.GetType() == typeof(ExcelDisplayNameAttribute))).Name
                    : property.Key.Name;

                currentCell.Value = displayName;

                currentCell.Style.Font.SetFromFont(font);
                currentCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                currentCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                currentCell.Style.Fill.BackgroundColor.SetColor(color);
                currentCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                currentColumn++;
            }

            this.currentRow++;
        }
    }
}
