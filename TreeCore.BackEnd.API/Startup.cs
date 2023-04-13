#region USING
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using TreeCore.BackEnd.API.Settings;
using TreeCore.Shared.Data.Db;
using TreeCore.BackEnd.API.Middleware;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AspNetCoreRateLimit;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using TreeCore.BackEnd.WorkerServices;
using TreeCore.APIClient;
#endregion

namespace TreeCore.BackEnd.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            DirectoryMapping.setRuta(configuration);
            Environment = environment;

            APIObjects.rutaAPI = configuration.GetSection("APIRoute").Value;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // the clientId/clientIp resolvers use it.
            services.AddHttpContextAccessor();

            // Register and configure localization services
            services.AddLocalization(options => options.ResourcesPath = "Localization");

            #region Security

            // confiure HTTP Strict Transport Security Protocol (HSTS)
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(1);
            });

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Register and configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost")
                        .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE")
                        .AllowCredentials();
                    });
            });

            #endregion

            #region Swagger

            // Register and Configure API versioning
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            //Register and configure API versioning explorer
            services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });

            // Swagger OpenAPI Configuration
            var swaggerDocOptions = new SwaggerDocOptions();
            Configuration.GetSection(nameof(SwaggerDocOptions)).Bind(swaggerDocOptions);
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });
            services.AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((swagger, service) =>
                {
                    foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
                    {
                        swagger.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = swaggerDocOptions.Title,
                            Version = description.ApiVersion.ToString(),
                            Description = swaggerDocOptions.Description,
                            Contact = new OpenApiContact
                            {
                                Name = swaggerDocOptions.Organization,
                                Email = swaggerDocOptions.Email
                            }
                        });
                    }

                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[0]}
                    };

                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });

                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[]{ }
                        }
                    });

                    swagger.OperationFilter<AuthorizeCheckOperationFilter>();

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    swagger.IncludeXmlComments(xmlPath);
                });
            #endregion


            services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
            services
                .AddScoped<DbConnection>(x => new SqlConnection(DatabaseConnection.BuildConnectionString(Configuration, Convert.ToString(Environment))))
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<TransactionalWrapper>();
            IoC.AddRegistration(services, Configuration, Environment);
            //.AddScoped<DbConnection>(x => new SqlConnection(DatabaseConnection.BuildConnectionString(hostingContext.Configuration)));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable Middelware to serve generated Swager as JSON endpoint/
                var swaggerOptions = new SwaggerOptions();
                Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

                app.UseSwagger(option =>
                {
                    option.RouteTemplate = swaggerOptions.JsonRoute;
                });

                // Enable Middelware to serve Swagger UI (HTML, JavaScript, CSS etc.)
                app.UseSwaggerUI(option =>
                {
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }
            else
            {
                // Enable HTTP Strict Transport Security Protocol (HSTS)
                app.UseHsts();
                app.UseExceptionHandler("/error");
            }

            #region Culture Info

            // List of supported cultures for localization used in RequestLocalizationOptions
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es")
            };

            // Configure RequestLocalizationOptions with supported culture
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),

                // Formatting numbers, date etc.
                SupportedCultures = supportedCultures,

                // UI strings that are localized
                SupportedUICultures = supportedCultures
            };

            // Enable Request Localization
            app.UseRequestLocalization(requestLocalizationOptions);

            #endregion

            #region Security

            // Enable NWebSec Security Headers
            app.UseXContentTypeOptions();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(options => options.NoReferrerWhenDowngrade());

            // Feature-Policy security header
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Feature-Policy", "geolocation 'none'; midi 'none';");
                await next.Invoke();
            });

            // Enable IP Rate Limiting Middleware
            app.UseIpRateLimiting();


            #endregion

            app.UseCustomExceptionHandler();

            //app.UseHttpsRedirection();

            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers()
            //        .RequireAuthorization("ApiScope");
            //});
        }
    }
}
