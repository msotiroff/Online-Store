namespace MSToolKit.Core.Mvc.Enums
{
    /// <summary>
    /// Enumeration, containing the allowed excel extensions.
    /// </summary>
    public enum ExcelFileExtension
    {
        /// <summary>
        /// XLSX files are the default spreadsheet output documents of newer versions of Microsoft Excel, starting with Microsoft Office 2007. 
        /// These XLSX files can also be opened with older versions of Microsoft Excel, 
        /// though compatibility support must first be downloaded from the Microsoft website and installed in the system.
        /// </summary>
        xlsx,
        /// <summary>
        /// The XLS file format is implemented by older versions of 
        /// Microsoft Excel for its output spreadsheet documents.
        /// </summary>
        xls
    }
}
