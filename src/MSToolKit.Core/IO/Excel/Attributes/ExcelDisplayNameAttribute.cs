using System;

namespace MSToolKit.Core.IO.Excel.Attributes
{
    /// <summary>
    /// Adds a metadata to a property, that should be used for building an excel worksheet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelDisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.IO.Excel.Attributes.ExcelDisplayNameAttribute.
        /// </summary>
        /// <param name="name">The display name of the property, that has the current attribute.</param>
        public ExcelDisplayNameAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the display name for the property, that should be used in an excel worksheet.
        /// </summary>
        public string Name { get; }
    }
}
