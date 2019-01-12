namespace MSToolKit.Core.Mapping.AutoMapping.Profiles
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The default implementation of AutoMapper.Profile.
    /// </summary>
    public class DefaultProfile : Profile
    {
        /// <summary>
        /// Initialize a new instance of MSToolKit.Core.Mapping.AutoMapping.Profiles.DefaultProfile.
        /// </summary>
        public DefaultProfile()
        {
            this.ConfigureProfile();
        }

        private void ConfigureProfile()
        {
            var allTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes());

            this.CreateMappings(allTypes);

            this.CreateCustomMappins(allTypes);
        }
        
        /// <summary>
        /// Creates bidirectional mapping for all types, which extends IAutoMapWith<> interface.
        /// </summary>
        private void CreateMappings(IEnumerable<Type> allTypes)
        {
            var allMappingTypes = allTypes
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Select(i => i.GetGenericTypeDefinition())
                            .Contains(typeof(IAutoMapWith<>)))
                .Select(t => new
                {
                    Destination = t,
                    Sourse = t.GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Select(i => new
                        {
                            Definition = i.GetGenericTypeDefinition(),
                            Arguments = i.GetGenericArguments()
                        })
                        .Where(i => i.Definition == typeof(IAutoMapWith<>))
                        .SelectMany(i => i.Arguments)
                        .First()
                })
                .ToList();

            foreach (var type in allMappingTypes)
            {
                this.CreateMap(type.Destination, type.Sourse);
                this.CreateMap(type.Sourse, type.Destination);
            }
        }

        /// <summary>
        /// Calls ConfigureMapping method of all types, which extends IHaveCustomMapping interface.
        /// </summary>
        private void CreateCustomMappins(IEnumerable<Type> allTypes)
        {
            allTypes
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && typeof(IHaveCustomMapping).IsAssignableFrom(t)
                    && t.GetConstructors()
                        .Any(ci => ci.GetParameters().Count() == 0))
                    .Select(Activator.CreateInstance)
                    .Cast<IHaveCustomMapping>()
                    .ToList()
                    .ForEach(mapping => mapping.ConfigureMapping(this));
        }
    }
}
