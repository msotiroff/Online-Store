using System;

namespace MSToolKit.Core.IO.Excel.Attributes
{
    /// <summary>
    /// Adds a metadata to a property, that should be used for building an excel worksheet.
    /// The member, that use this Attribute should implement System.IFormattable interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelValueFormatAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.IO.Excel.Attributes.ExcelValueFormatAttribute.
        /// </summary>
        /// <param name="format">
        /// The format, which will be used for formatting the member value.
        /// </param>
        public ExcelValueFormatAttribute(string format)
        {
            this.Format = format;
        }

        /// <summary>
        /// Gets the specified format for the member, which value should be formatted.
        /// </summary>
        public string Format { get; }
    }
}
