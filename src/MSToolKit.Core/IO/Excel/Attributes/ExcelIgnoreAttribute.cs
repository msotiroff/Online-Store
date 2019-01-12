using System;

namespace MSToolKit.Core.IO.Excel.Attributes
{
    /// <summary>
    /// Adds a metadata to a property, indicating that the current member
    /// should be IGNORED when building an excel worksheet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelIgnoreAttribute : Attribute
    {
    }
}
