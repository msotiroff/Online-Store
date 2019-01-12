using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MSToolKit.Core.Mapping.CustomMapping
{
    /// <summary>
    /// Provides a default mapping between two type of objects.
    /// </summary>
    internal class Mapping<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> convertion;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Mapping.CustomMapping.Mapping.
        /// </summary>
        /// <param name="convertion">
        /// Delegate that accepts a source object and returns a new destination form of it.
        /// </param>
        public Mapping(Func<TSource, TDestination> convertion)
        {
            this.convertion = convertion;
        }

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Mapping.CustomMapping.Mapping.
        /// </summary>
        public Mapping()
        {
            this.convertion = (src) => this.CreateDefaultInstance(src);
        }

        /// <summary>
        /// Make a convertion from a source object to a destination object.
        /// </summary>
        /// <param name="source">
        /// The source object that should be transformed.
        /// </param>
        /// <returns>
        /// The transformed destination object.
        /// </returns>
        public TDestination MapFrom(TSource source)
        {
            return this.convertion(source);
        }

        private TDestination CreateDefaultInstance(TSource source)
        {
            var destInstance = (TDestination)FormatterServices
                .GetUninitializedObject(typeof(TDestination));

            if (source == null)
            {
                return destInstance;
            }

            var srcProperties = typeof(TSource).GetProperties();

            var initializeableDestProperties = typeof(TDestination)
                .GetProperties()
                .Where(pi => pi.SetMethod != null)
                .ToList();

            foreach (var destProperty in initializeableDestProperties)
            {
                // TODO: Think about nullable types to map.
                var srcPropertyToMap = srcProperties
                    .FirstOrDefault(pi => pi.Name
                        .Equals(destProperty.Name, StringComparison.InvariantCultureIgnoreCase)
                            && pi.PropertyType == destProperty.PropertyType);

                if (srcPropertyToMap != null)
                {
                    destProperty
                        .SetValue(destInstance, srcPropertyToMap.GetValue(source));

                    continue;
                }

                var propertiesThatBeginsWith = srcProperties
                    .Where(pi => destProperty.Name.StartsWith(pi.Name))
                    .ToList();

                var outerProperty = propertiesThatBeginsWith
                    .FirstOrDefault(prop => prop.PropertyType
                        .GetProperties()
                        .Any(innerProp => innerProp.PropertyType == destProperty.PropertyType
                            && destProperty.Name.EndsWith(innerProp.Name)));

                if (outerProperty == null)
                {
                    continue;
                }

                var innerProperty = outerProperty
                        .PropertyType
                        .GetProperties()
                        .First(pi => pi.PropertyType == destProperty.PropertyType
                            && destProperty.Name.EndsWith(pi.Name));

                if (!destProperty.Name.Equals(
                    $"{outerProperty.Name}{innerProperty.Name}", 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var innerSource = outerProperty.GetValue(source);
                
                destProperty
                    .SetValue(destInstance, innerProperty.GetValue(innerSource));
            }

            return destInstance;
        }
    }
}
