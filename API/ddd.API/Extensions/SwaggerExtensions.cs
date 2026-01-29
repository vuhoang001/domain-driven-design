using Microsoft.OpenApi.Models;

namespace ddd.API.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v0", new OpenApiInfo
            {
                Title       = "MyMeetings API",
                Version     = "v0",
                Description = "MyMeetings API for modular monolith .NET application."
            });

            options.CustomSchemaIds(t => t.ToString());

            var baseDirectory    = AppDomain.CurrentDomain.BaseDirectory;
            // var commentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var commentsFile     = Path.Combine(baseDirectory, commentsFileName);
            // options.IncludeXmlComments(commentsFile);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name   = "Authorization",
                In     = ParameterLocation.Header,
                Type   = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v0/swagger.json", "MyMeetings API V0"); });
        return app;
    }
}