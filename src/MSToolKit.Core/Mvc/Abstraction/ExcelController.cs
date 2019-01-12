using Microsoft.AspNetCore.Mvc;
using MSToolKit.Core.Mvc.Enums;
using System.Collections.Generic;

namespace MSToolKit.Core.Mvc.Abstraction
{
    /// <summary>
    /// Extends Microsoft.AspNetCore.Mvc.Controller 
    /// and provides methods that return MSToolKit.Core.Mvc.ExcelResult.
    /// </summary>
    public abstract class ExcelController : Controller
    {
        /// <summary>
        /// Returns new instance of MSToolKit.Core.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <returns>New instance of MSToolKit.Core.Mvc.ExcelResult.</returns>
        protected ExcelResult Excel(IEnumerable<byte> data) 
            => new ExcelResult(data);

        /// <summary>
        /// Returns new instance of MSToolKit.Core.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <param name="fileName">
        /// The name, which should be used for the attached file.
        /// </param>
        /// <returns>
        /// New instance of MSToolKit.Core.Mvc.ExcelResult.
        /// </returns>
        protected ExcelResult Excel(IEnumerable<byte> data, string fileName) 
            => new ExcelResult(data, fileName);

        /// <summary>
        /// Returns new instance of MSToolKit.Core.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <param name="fileName">
        /// The name, which should be used for the attached file.
        /// </param>
        /// <param name="fileExtension">
        /// The extension, that should be used for the attached file.
        /// </param>
        /// <returns>
        /// New instance of MSToolKit.Core.Mvc.ExcelResult.
        /// </returns>
        protected ExcelResult Excel(
            IEnumerable<byte> data, string fileName, ExcelFileExtension fileExtension)
                => new ExcelResult(data, fileName, fileExtension);
    }
}
