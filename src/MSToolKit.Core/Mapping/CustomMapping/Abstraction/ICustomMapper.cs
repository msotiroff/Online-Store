namespace MSToolKit.Core.Mapping.CustomMapping.Abstraction
{
    /// <summary>
    /// Provides an abstraction for custom mapping between objects.
    /// </summary>
    public interface ICustomMapper
    {
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
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
