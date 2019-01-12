using Microsoft.Extensions.DependencyInjection;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using System;

namespace MSToolKit.Core.Mapping.CustomMapping
{
    /// <summary>
    /// The default implementation of MSToolKit.Core.Mapping.CustomMapping.Abstraction.ICustomMapper.
    /// </summary>
    internal class CustomMapper : ICustomMapper
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Mapping.CustomMapping.CustomMapper.
        /// </summary>
        /// <param name="serviceProvider">
        /// The current instance of System.IServiceProvider.
        /// </param>
        public CustomMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Projects the source object to a new form.
        /// All constructor logic of the destination object will not be executed.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the object to be transformed.
        /// </typeparam>
        /// <typeparam name="TDestination">
        /// The type of the object that the source object should be transformed to.
        /// </typeparam>
        /// <param name="source">
        /// The object to be transformed.
        /// </param>
        /// <returns>
        /// New instance of TDestination. All constructor logic of the destination object will not be executed.
        /// </returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            CoreValidator.ThrowIfNull(source);

            var mapping = this.serviceProvider
                .GetRequiredService<Mapping<TSource, TDestination>>();

            return mapping.MapFrom(source);
        }
    }
}
