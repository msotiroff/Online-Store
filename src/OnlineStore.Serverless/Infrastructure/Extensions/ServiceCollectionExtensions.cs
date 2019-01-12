using Microsoft.Extensions.DependencyInjection;
using MSToolKit.Core.Extensions;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.Common.ViewModels.User;
using OnlineStore.Models;

namespace OnlineStore.Serverless.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationCustomMappings(this IServiceCollection services)
        {
            /// Mapping between User and UserIndexViewModel
            services
                .AddCustomMapping<User, UserIndexViewModel>(user =>
                new UserIndexViewModel
                {
                    Id = user?.Id,
                    Email = user?.Email,
                    FirstName = user?.FirstName,
                    LastName = user?.LastName,
                    Username = user?.Username,
                    HasAdminPermissions = (user?.IsAdmin ?? false) ? "YES" : "NO",
                    EmailConfirmed = user?.EmailConfirmed ?? false
                });

            /// Mapping between Product and ProductConciseViewModel
            services
                .AddCustomMapping<Product, ProductConciseViewModel>(product =>
                new ProductConciseViewModel
                {
                    Id = product.Id,
                    CategoryName = "Fishing equipment",
                    Name = product.Name,
                    Price = product.Price,
                    ShortDescription = product.Description.Length > 50
                        ? product.Description.Substring(0, 50) + "..."
                        : product.Description
                });

            return services;
        }
    }
}
