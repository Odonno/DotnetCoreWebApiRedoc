using DotnetCoreWebApiRedoc.Auth;
using DotnetCoreWebApiRedoc.Extensions;
using DotnetCoreWebApiRedoc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotnetCoreWebApiRedoc
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
            services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddApiKeySupport(options => { });

            services.AddSwaggerGen(c =>
            {
                // Internal swagger gen (for the organization itself)
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "API",
                        Version = "v1"
                    }
                );
                // External swagger gen (for partners)
                c.SwaggerDoc(
                    "partners",
                    new OpenApiInfo
                    {
                        Title = "API",
                        Version = "v1"
                    }
                );

                c.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "partners")
                    {
                        return apiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any();
                    }

                    return true;
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var apiKeyScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiKeyAuthenticationOptions.DefaultScheme
                    },
                    Description = "Api Key using the X-Api-Key header. Example: \"X-Api-Key: {key}\"",
                    Name = "X-Api-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticationOptions.DefaultScheme
                };
                c.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, apiKeyScheme);

                var requirement = new OpenApiSecurityRequirement
                {
                    { apiKeyScheme, Enumerable.Empty<string>().ToList() }
                };
                c.AddSecurityRequirement(requirement);
            });

            services.AddSingleton<IGetApiKeyQuery, InMemoryGetApiKeyQuery>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            app.UseReDoc(c =>
            {
                c.SpecUrl = "/swagger/partners/swagger.json";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
