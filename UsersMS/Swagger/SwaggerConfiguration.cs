using Microsoft.OpenApi.Models;

namespace UsersMS.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerConfig = configuration.GetSection("Swagger").Get<SwaggerConfig>();

            var location = Enum.TryParse<ParameterLocation>(swaggerConfig?.Authorization?.In, out var loc)
                ? loc
                : ParameterLocation.Header;

            var type = Enum.TryParse<SecuritySchemeType>(swaggerConfig?.Authorization?.Type, out var t)
                ? t
                : SecuritySchemeType.ApiKey;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    swaggerConfig?.Version ?? "v1",
                    new OpenApiInfo
                    {
                        Title = swaggerConfig?.Title ?? "API",
                        Version = swaggerConfig?.Version ?? "v1"
                    });

                c.AddSecurityDefinition(swaggerConfig?.Authorization?.Scheme ?? "Bearer", new OpenApiSecurityScheme
                {
                    Description = swaggerConfig?.Description ?? "JWT Authorization header using the Bearer scheme.",
                    Name = swaggerConfig?.Authorization?.Name ?? "Authorization",
                    In = location,
                    Type = type,
                    Scheme = swaggerConfig?.Authorization?.Scheme ?? "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = swaggerConfig?.Authorization?.Scheme ?? "Bearer"
                        },
                        Scheme = swaggerConfig?.Authorization?.Scheme ?? "Bearer",
                        Name = swaggerConfig?.Authorization?.Name ?? "Authorization",
                        In = location
                    },
                    Array.Empty<string>()
                }
            });
            });
        }
    }

}
