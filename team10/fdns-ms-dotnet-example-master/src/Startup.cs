#pragma warning disable 1591 // disables the warnings about missing Xml code comments
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Foundation.Sdk;
using Foundation.Sdk.Data;
using Foundation.Sdk.Security;
using Foundation.Sdk.Services;
using Foundation.Example.WebUI.Importers;
using Foundation.Example.WebUI.Models;
using Foundation.Example.WebUI.Security;
using Polly;
using Polly.Extensions;
using Polly.Extensions.Http;

namespace Foundation.Example.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services to add</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddResponseCaching();

            /* If you set this microservice up to use an OAuth2 provider, such as Auth0, then 'useAuthorization' will be true and there is extra
             * work involved to configure your scope-based authorization model. That's all been deactivated for the sake of simplicity but this
             * at least shows you how it can be done.*/
            string authorizationDomain = Common.GetConfigurationVariable(Configuration, "OAUTH2_ACCESS_TOKEN_URI", "Auth:Domain", string.Empty);
            bool useAuthorization = !string.IsNullOrEmpty(authorizationDomain);

            string applicationName = Common.GetConfigurationVariable(Configuration, "APP_NAME", "AppName", "Customer");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Example .NET Core 2.1 API",
                    Version = "v1",
                    Description = "A microservice example that shows how to use the FDNS .NET Core SDK.",
                    Contact = new Contact
                    {
                        Name = "Erik Knudsen",
                        Email = string.Empty,
                        Url = "https://gitlab.com/eknudsen"
                    },
                    License = new License
                    {
                        Name = "Apache 2.0",
                        Url = "https://www.apache.org/licenses/LICENSE-2.0"
                    }
                });

                /* If you set this microservice up to use an OAuth2 provider, such as Auth0, and you have passed in the right config vars,
                 * then Swagger is automatically set up to include a login prompt via the code below. */
                if (useAuthorization)
                {
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                        { "Bearer", Enumerable.Empty<string>() },
                    });
                }

                // These two lines are necessary for Swagger to pick up the C# XML comments and show them in the Swagger UI. See https://github.com/domaindrivendev/Swashbuckle.AspNetCore for more details.
                var filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "api.xml");
                c.IncludeXmlComments(filePath);

                c.EnableAnnotations();
            });

            services.AddMvc(options =>
            {
               options.InputFormatters.Add(new TextPlainInputFormatter());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Get the URL to the FDNS Object microservice from the configuration. Note that OBJECT_URL is an environment variable, while ObjectService:Url refers to something in AppSettings.json
            var objectServiceUrl = Sdk.Common.GetConfigurationVariable(Configuration, "OBJECT_URL", "ObjectService:Url", "http://localhost:8083/api/1.0");
            var storageServiceUrl = Sdk.Common.GetConfigurationVariable(Configuration, "STORAGE_URL", "StorageService:Url", "http://localhost:8082/api/1.0");
            var indexingServiceUrl = Sdk.Common.GetConfigurationVariable(Configuration, "INDEXING_URL", "IndexingService:Url", "http://localhost:8084/api/1.0");
            var rulesServiceUrl = Sdk.Common.GetConfigurationVariable(Configuration, "RULES_URL", "RulesService:Url", "http://localhost:8086/api/1.0");

            #region Create HTTP clients with configurable resiliency patterns
            // HTTP client for FDNS Object service: Bookstore database, Customer collection
            services.AddHttpClient($"{applicationName}-{Common.OBJECT_SERVICE_NAME}", client =>
            {
                client.BaseAddress = new Uri($"{objectServiceUrl}/bookstore/customer/");
                client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                /* This example creates a policy which will handle typical transient faults, retrying the
                 * underlying http request up to 3 times if necessary. The policy will apply a delay of 50ms
                 * before the first retry; 250ms before a second retry; and 1 second before the third.
                 * Note: These apply only to 5xx error codes, 408, and System.Net.Http.HttpRequestException.
                 */
                TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(1)
            }))
            .AddPolicyHandler(GetCircuitBreakerPolicy()); // sets a circuit breaker so that after several failed requests, we just stop sending those requests

            // HTTP client for FDNS Storage service
            services.AddHttpClient($"{applicationName}-{Common.STORAGE_SERVICE_NAME}", client =>
            {
                client.BaseAddress = new Uri($"{storageServiceUrl}/");
                client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(1)
            }))
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // HTTP client for FDNS Indexing service
            services.AddHttpClient($"{applicationName}-{Common.INDEXING_SERVICE_NAME}", client =>
            {
                client.BaseAddress = new Uri($"{indexingServiceUrl}/");
                client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(1)
            }))
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // HTTP client for FDNS Rules service
            services.AddHttpClient($"{applicationName}-{Common.RULES_SERVICE_NAME}", client =>
            {
                client.BaseAddress = new Uri($"{rulesServiceUrl}/");
                client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(1)
            }))
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            #endregion

            #region Create services for dependency injection
            services.AddSingleton<IObjectRepository<Customer>>(provider => new HttpObjectRepository<Customer>(
                provider.GetService<IHttpClientFactory>(),
                provider.GetService<ILogger<HttpObjectRepository<Customer>>>(),
                applicationName));

            services.AddSingleton<IStorageRepository>(provider => new HttpStorageRepository(
                provider.GetService<IHttpClientFactory>(),
                provider.GetService<ILogger<HttpStorageRepository>>(),
                applicationName,
                "bookstore-customer"));

            services.AddSingleton<IIndexingService>(provider => new HttpIndexingService(
                provider.GetService<IHttpClientFactory>(),
                provider.GetService<ILogger<HttpIndexingService>>(),
                applicationName));

            services.AddSingleton<IRulesService>(provider => new HttpRulesService(
                provider.GetService<IHttpClientFactory>(),
                provider.GetService<ILogger<HttpRulesService>>(),
                applicationName));

            services.AddSingleton<ICustomerImporter>(provider => new HttpCustomerImporter(
                provider.GetService<IObjectRepository<Customer>>()));
            #endregion // Create services for dependency injection

            #region Health checks
            services.AddHealthChecks(checks =>
            {
                // check the FDNS microservices that this one is dependent on
                checks.AddHealthCheckGroup(
                        "servers",
                        group => group.AddUrlCheck($"{objectServiceUrl}/", TimeSpan.FromMilliseconds(50))
                                      .AddUrlCheck($"{storageServiceUrl}/", TimeSpan.FromMilliseconds(50))
                                      .AddUrlCheck($"{rulesServiceUrl}/", TimeSpan.FromMilliseconds(50))
                    )
                    .AddHealthCheckGroup(
                        "memory",
                        group => group.AddPrivateMemorySizeCheck(1)
                                    .AddVirtualMemorySizeCheck(2)
                                    .AddWorkingSetCheck(1),
                            CheckStatus.Unhealthy
                    );
            });
            #endregion // Health checks

            #region OAuth2 configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = authorizationDomain;
                options.Audience = Common.GetConfigurationVariable(Configuration, "OAUTH2_CLIENT_ID", "Auth:ApiIdentifier", string.Empty);
            });

            /* These policy names match the names in the [Authorize] attribute(s) in the Controller classes.
             * The HasScopeHandler class is used (see below) to pass/fail the authorization check if authorization
             * has been enabled via the microservice's configuration.
             */
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Common.READ_AUTHORIZATION_NAME, policy => policy.Requirements.Add(new HasScopeRequirement(Common.READ_AUTHORIZATION_NAME, authorizationDomain)));
                options.AddPolicy(Common.INSERT_AUTHORIZATION_NAME, policy => policy.Requirements.Add(new HasScopeRequirement(Common.INSERT_AUTHORIZATION_NAME, authorizationDomain)));
                options.AddPolicy(Common.UPDATE_AUTHORIZATION_NAME, policy => policy.Requirements.Add(new HasScopeRequirement(Common.UPDATE_AUTHORIZATION_NAME, authorizationDomain)));
                options.AddPolicy(Common.DELETE_AUTHORIZATION_NAME, policy => policy.Requirements.Add(new HasScopeRequirement(Common.DELETE_AUTHORIZATION_NAME, authorizationDomain)));
            });

            // If the developer has not configured OAuth2, then disable authentication and authorization
            if (useAuthorization)
            {
                services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            }
            else
            {
                services.AddSingleton<IAuthorizationHandler, AlwaysAllowHandler>();
            }
            #endregion // OAuth2 configuration
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // app.UseHttpsRedirection(); // use for production but for R&D and testing, can be a problem
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example .NET Core 2.1 API V1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(15));
        }
    }
}
#pragma warning restore 1591
