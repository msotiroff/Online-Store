namespace MSToolKit.Core.Mapping.AutoMapping
{
    using AutoMapper;

    /// <summary>
    /// Provides an abstraction for configuring a custom mapping for the inheritor.
    /// </summary>
    public interface IHaveCustomMapping
    {
        /// <summary>
        /// Configures custom mapping rules between the source and the destination.
        /// </summary>
        /// <param name="mapperProfile">The inheritor instance of the AutoMapper.Profile class.</param>
        void ConfigureMapping(Profile mapperProfile);
    }
}
