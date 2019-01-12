using System;
using System.Linq;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using over System.Type class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if the given Type has a public property with the specified name.
        /// </summary>
        /// <param name="type">The given Type to be checked.</param>
        /// <param name="propertyName">The specified property name to seach for.</param>
        /// <returns>True or false, depending on the invokation result.</returns>
        public static bool HasProperty(this Type type, string propertyName)
        {
            var hasProperty = type
                .GetProperties()
                .Any(pi => pi.Name == propertyName);

            return hasProperty;
        }

        /// <summary>
        /// Checks if the given Type has a public property with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The generic argument, that represents the property type to check for.</typeparam>
        /// <param name="type">The given Type to be checked.</param>
        /// <param name="propertyName">The specified property name to seach for.</param>
        /// <returns>True or false, depending on the invokation result.</returns>
        public static bool HasPropertyOfType<T>(this Type type, string propertyName)
        {
            var propertyInfo = type
                .GetProperties()
                .FirstOrDefault(pi => pi.Name == propertyName);

            if (propertyInfo == null)
            {
                return false;
            }
            
            return propertyInfo.PropertyType == typeof(T);
        }

        /// <summary>
        /// Checks if the given Type has a public property of a specified type with a specified name.
        /// </summary>
        /// <param name="type">The given Type to be checked.</param>
        /// <param name="propertyValueType">The type, that represents the property type to check for.</param>
        /// <param name="propertyName">The specified property name to seach for.</param>
        /// <returns>True or false, depending on the invokation result.</returns>
        public static bool HasPropertyOfType(this Type type, Type propertyValueType, string propertyName)
        {
            var propertyInfo = type
                .GetProperties()
                .FirstOrDefault(pi => pi.Name == propertyName);

            if (propertyInfo == null)
            {
                return false;
            }

            return propertyInfo.PropertyType == propertyValueType;
        }
    }
}
