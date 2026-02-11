namespace ddd.API.Extensions;

internal static class SwaggerExtensions
{
    internal static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title       = "DDD API",
                Version     = "v1",
                Description = "API Documentation"
            });

            options.CustomSchemaIds(t => t.FullName);
        });
    }

    internal static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DDD API V1");
            options.RoutePrefix = string.Empty;
        });
    }
}