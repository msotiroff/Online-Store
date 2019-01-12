using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSToolKit.Core.DataAccess.Wrappers.DynamoDb.Abstraction;
using MSToolKit.Core.EmailServices;
using MSToolKit.Core.Extensions;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Serverless.Infrastructure.Extensions;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Reflection;

namespace OnlineStore.Serverless
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<MSToolKit.Core.Filters.Options.LoggingExceptionHandlerOptions>(
                    this.Configuration.GetSection("ExceptionHandlerOptions"))
                .Configure<SendGridOptions>(this.Configuration.GetSection("SendGridOptions"))
                .Configure<EnvironmentOptions>(this.Configuration.GetSection("EnvironmentOptions"))
                .Configure<AwsFunctionOptions>(this.Configuration.GetSection("AwsFunctionOptions"))
                .Configure<ResourceOptions>(this.Configuration.GetSection("ResourceOptions"))
                .Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/user/account/login";
                        options.LogoutPath = "/user/account/logout";
                    });

            services
                .AddDefaultAWSOptions(this.Configuration.GetAWSOptions())
                .AddAmazonS3FileManagement()
                .AddDynamoDbWrapper<User, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:UsersTable"))
                .AddDynamoDbWrapper<Category, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:CategoriesTable"))
                .AddDynamoDbWrapper<Product, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:ProductsTable"))
                .AddDynamoDbWrapper<Picture, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:PicturesTable"))
                .AddDynamoDbWrapper<Order, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:OrdersTable"))
                .AddDynamoDbWrapper<ShoppingCart, string>(opt => opt.TableName
                    = this.Configuration.GetValue<string>("DynamoDbSettings:ShoppingCartsTable"))
                .AddDynamoDbContext(options =>
                {
                    options.AccessKeyId = this.Configuration.GetValue<string>("AwsCredentials:AccessKeyId");
                    options.SecretAccessKey = this.Configuration.GetValue<string>("AwsCredentials:SecretAccessKey");
                    options.LocalMode = this.Configuration.GetValue<bool>("DynamoDbSettings:LocalMode");
                    options.ServiceUrl = this.Configuration.GetValue<string>("DynamoDbSettings:ServiceUrl");
                });
            
            services
                .AddFileServices()
                .AddExcelServices()
                .AddEmailServices()
                .AddDistributedMemoryCache()
                .AddUserStore<User, IDynamoDbWrapper<User, string>>()
                .AddAuthenticationServices<User>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddRouting(options => options.LowercaseUrls = true)
                .AddConvetionallyNamedServices(Assembly.GetAssembly(typeof(Startup)))
                .AddConvetionallyNamedServices(Assembly.GetAssembly(typeof(IService)))
                .AddConvetionallyNamedServices(Assembly.GetAssembly(typeof(IFactory)))
                .AddConvetionallyNamedServices(Assembly.GetAssembly(typeof(IRepository)));

            services
                .AddCustomMapper()
                .AddApplicationCustomMappings();

            services
                .AddMvc(options => 
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app
            .UseLoggingExceptionHandler(options =>
            {
                options.LocalErrorUrl = "/home/error";
                options.ShouldRethrowException = true;
            })
            .UseAnonymousBrowser()
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseAuthentication()
            .UseHttpsRedirection();

            loggerFactory.AddLambdaLogger(this.Configuration, "Lambda.Logging");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                         name: "areaRoute",
                         template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "defaultRoute",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
