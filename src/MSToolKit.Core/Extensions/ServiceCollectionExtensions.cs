using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MSToolKit.Core.Authentication;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Authentication.Options;
using MSToolKit.Core.DataAccess.Abstraction;
using MSToolKit.Core.DataAccess.Wrappers.DynamoDb;
using MSToolKit.Core.DataAccess.Wrappers.DynamoDb.Abstraction;
using MSToolKit.Core.DataAccess.Wrappers.MongoDb;
using MSToolKit.Core.DataAccess.Wrappers.MongoDb.Abstraction;
using MSToolKit.Core.EmailServices;
using MSToolKit.Core.EmailServices.Abstraction;
using MSToolKit.Core.IO;
using MSToolKit.Core.IO.Abstraction;
using MSToolKit.Core.IO.AmazonFileManagment;
using MSToolKit.Core.IO.AmazonFileManagment.Abstraction;
using MSToolKit.Core.IO.Excel;
using MSToolKit.Core.IO.Excel.Abstraction;
using MSToolKit.Core.Mapping.CustomMapping;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using over Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static HashSet<Type> registeredUserStores = new HashSet<Type>();

        /// <summary>
        /// Adds Amazon S3 servicesas well as default implementation 
        /// of MSToolKit.Core.IO.AmazonFileManagment.Abstraction.IAmazonS3FileManager.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddAmazonS3FileManagement(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IAmazonS3FileManager, AmazonS3FileManager>();
            services.AddTransient<ITransferUtility>(
                sp => new TransferUtility(sp.GetRequiredService<IAmazonS3>()));

            return services;
        }

        /// <summary>
        /// Add a default implementation of MSToolKit.Core.Mapping.CustomMapping.Abstraction.ICustomMapper 
        /// that provides a functionality for default mapping between objects.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddTransient<ICustomMapper, CustomMapper>();
            services.AddTransient(typeof(Mapping<,>), typeof(Mapping<,>));

            return services;
        }

        /// <summary>
        /// Add a default mapping between two type of objects.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <param name="convertion">
        /// A delegate that accepts a source object and returns transformed destination object.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddCustomMapping<TSource, TDestination>(
            this IServiceCollection services,
            Func<TSource, TDestination> convertion)
        {
            services.AddTransient(sp => new Mapping<TSource, TDestination>(convertion));

            return services;
        }

        /// <summary>
        /// Adds authentication services with specified(or default, if missing) options to the service provider.
        /// </summary>
        /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="options">Instance of MSToolKit.Core.Authentication.Options.AuthenticationOptions(optional parameter).</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddAuthenticationServices<TUser>(
            this IServiceCollection services, Action<AuthenticationOptions> options)
            where TUser : AuthenticationUser
        {
            var authOptions = new AuthenticationOptions();
            options?.Invoke(authOptions);

            services.AddSingleton(authOptions);
            services.AddSingleton(authOptions.Password);
            services.AddSingleton(authOptions.User);
            services.AddSingleton(authOptions.SignIn);
            services.AddTransient<ISignInManager<TUser>, SignInManager<TUser>>();
            services.AddTransient<IUserManager<TUser>, UserManager<TUser>>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IUserValidator<TUser>, UserValidator<TUser>>();

            return services;
        }

        /// <summary>
        /// Adds a specified user store to the service provider. Only one user store per user type is allowed.
        /// </summary>
        /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
        /// <typeparam name="TStore">The type encapsulating a user store.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <exception cref="InvalidOperationException">
        /// System.InvalidOperationException will be thrown, if method's already invoked with the same type of TUser.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled user store services.
        /// </returns>
        public static IServiceCollection AddUserStore<TUser, TStore>(this IServiceCollection services)
            where TStore : IDbContext<TUser, string> where TUser : AuthenticationUser
        {
            if (registeredUserStores.Contains(typeof(TUser)))
            {
                throw new InvalidOperationException(
                    $"You have already added a user store for type: {typeof(TUser).FullName}.");
            }

            services.AddTransient<IUserStore<TUser>>(sp =>
            {
                var store = sp.GetRequiredService<TStore>();
                return new UserStore<TUser>(store);
            });

            registeredUserStores.Add(typeof(TUser));
            return services;
        }

        /// <summary>
        /// Adds a default instance of MSToolKit.Core.Wrappers.MongoDb.Abstraction.IMongoDbWrapper
        /// for a specified entity with specified primary key.
        /// </summary>
        /// <typeparam name="TEntity">The type encapsulating the entity.</typeparam>
        /// <typeparam name="TKey">The primary key's type for the given entity.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="tableName">The name of the table, that contains the entities of the given type.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled MongoDbWrapper services.
        /// </returns>
        public static IServiceCollection AddMongoDbWrapper<TEntity, TKey>(this IServiceCollection services, string tableName)
             where TEntity : IEntity<TKey>
        {
            services.AddTransient<IMongoDbWrapper<TEntity, TKey>>(
                sp => new MongoDbWrapper<TEntity, TKey>(
                    sp.GetService<IMongoDatabase>(), tableName));

            return services;
        }

        /// <summary>
        /// Adds a default instance for MongoDB.Driver.IMongoDatabase with specified options.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="mongoDbOptions">
        /// MSToolKit.Core.Wrappers.MongoDb.MongoDbOptions, that configure the IMongoDatabase instance.
        /// </param>
        /// <exception cref="ArgumentException">
        /// System.ArgumentException will be thrown, if some of the required options are not specified or invalid.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled mongo db services.
        /// </returns>
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, Action<MongoDbOptions> mongoDbOptions)
        {
            var options = new MongoDbOptions();
            mongoDbOptions?.Invoke(options);

            var optionsValidationResult = options.GetValidationResult();
            if (!optionsValidationResult.Success)
            {
                var errorMsg = "MongoDbOptions validation failed. Errors: "
                    + string.Join(" ", optionsValidationResult.Errors);

                throw new ArgumentException(errorMsg);
            }

            services.AddTransient<IMongoDatabase>(sp =>
            {
                var client = new MongoClient(options.ConnectionString);
                return client.GetDatabase(options.DatabaseName);
            });

            return services;
        }

        /// <summary>
        /// Adds a default instance for MSToolKit.Core.Wrappers.DynamoDb.Abstraction.IDynamoDbWrapper with specified options.
        /// </summary>
        /// <typeparam name="TEntity">The type encapsulating the entity.</typeparam>
        /// <typeparam name="TKey">The primary key's type for the given entity.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="tableOptions">
        /// MSToolKit.Core.Wrappers.DynamoDb.DynamoDbTable options, that confifure the DynamoDbWrapper instance.
        /// </param>
        /// <exception cref="ArgumentException">
        /// System.ArgumentException will be thrown, if some of the required options are not specified or invalid.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled DynamoDbWrapper services.
        /// </returns>
        public static IServiceCollection AddDynamoDbWrapper<TEntity, TKey>(
            this IServiceCollection services, 
            Action<DynamoDbTableOptions<TEntity, TKey>> tableOptions) where TEntity : IEntity<TKey>
        {
            var tableOpt = new DynamoDbTableOptions<TEntity, TKey>();
            tableOptions?.Invoke(tableOpt);

            var tableValidationResult = tableOpt.GetValidationResult();
            if (!tableValidationResult.Success)
            {
                var errorMsg = "DynamoDbTable options validation failed. Errors: "
                    + string.Join(" ", tableValidationResult.Errors);

                throw new ArgumentException(errorMsg);
            }

            services.AddTransient<IDynamoDbWrapper<TEntity, TKey>>(
                sp => new DynamoDbWrapper<TEntity, TKey>(
                    sp.GetService<IAmazonDynamoDB>(), tableOpt.TableName));

            services.AddTransient<IDbContext<TEntity, TKey>>(
                sp => sp.GetRequiredService<IDynamoDbWrapper<TEntity, TKey>>());

            return services;
        }

        /// <summary>
        /// Adds a default instance for Amazon.DynamoDBv2.DataModel.IDynamoDBContext with specified options.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="dynamoDbOptions">
        /// MSToolKit.Core.Wrappers.DynamoDb.DynamoDbOptions, that configure the default instance for IDynanoDbContext.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with added DynamoDbContext services.
        /// </returns>
        public static IServiceCollection AddDynamoDbContext(
            this IServiceCollection services, Action<DynamoDbOptions> dynamoDbOptions = null)
        {
            var options = new DynamoDbOptions();
            dynamoDbOptions?.Invoke(options);

            if (options.LocalMode == true)
            {
                var clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = options.ServiceUrl ?? null
                };

                if (clientConfig.ServiceURL == null)
                {
                    clientConfig.RegionEndpoint = options.RegionEndpoint;
                }

                services.AddTransient<IAmazonDynamoDB>(sp => 
                {
                    if (options.AccessKeyId != null && options.SecretAccessKey != null)
                    {
                        return new AmazonDynamoDBClient(
                            options.AccessKeyId,
                            options.SecretAccessKey,
                            clientConfig);
                    }

                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddTransient<IDynamoDBContext>(
                sp => new DynamoDBContext(sp.GetService<IAmazonDynamoDB>()));
            
            return services;
        }

        /// <summary>
        /// Adds a default instance for MSToolKit.Core.EmailServices.Abstraction.IEmailSender to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled email services.
        /// </returns>
        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            return services;
        }

        /// <summary>
        /// Adds a default instance for MSToolKit.Core.IO.Excel.Abstraction.IExcelGenerator to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled excel services.
        /// </returns>
        public static IServiceCollection AddExcelServices(this IServiceCollection services)
        {
            services.AddTransient<IExcelBuilder, ExcelBuilder>();

            return services;
        }

        /// <summary>
        /// Adds a default instance for MSToolKit.Core.IO.Abstraction.IFileProcessor to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled file services.
        /// </returns>
        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            services.AddTransient<IFileProcessor, FileProcessor>();

            return services;
        }

        /// <summary>
        /// Adds all services that match the following name convention: 
        /// [Interface name = I{serviceName}, Implementation name = {serviceName}] to the service provider.
        /// </summary>
        /// <param name="assembly">The assembly, that services are located.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddConvetionallyNamedServices(
            this IServiceCollection services, 
            Assembly assembly)
        {
            var ass = Assembly.GetExecutingAssembly().FullName;
            /// Prevents adding services for MSToolKit.Core.dll
            if (assembly.FullName == Assembly.GetExecutingAssembly().FullName)
            {
                return services;
            }

            assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && !t.IsGenericType
                    && t.GetInterfaces()
                        .Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => services.AddTransient(s.Interface, s.Implementation));

            return services;
        }
    }
}
