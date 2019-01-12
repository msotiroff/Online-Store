using System.Collections.Generic;

namespace MSToolKit.Core.IO.Excel.Abstraction
{
    /// <summary>
    /// Provides an abstraction for building excel worksheets.
    /// </summary>
    public interface IExcelBuilder
    {
        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <typeparam name="TSource">The type, that should be represented in the worksheet.</typeparam>
        /// <param name="source">The collection of elements, to be inserted in the worksheet.</param>
        /// <param name="sheetName">(optional) The name of the sheet to build.</param>
        /// <returns>The built worksheet's bytes.</returns>
        IEnumerable<byte> BuildExcelWorksheet<TSource>(
            IEnumerable<TSource> source, string sheetName = default(string)) where TSource : class;

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
        IEnumerable<byte> BuildComplexExcelWorksheet(
            IEnumerable<IEnumerable<object>> multiCollections, string sheetName = default(string));
    }
}
