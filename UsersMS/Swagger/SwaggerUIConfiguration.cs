namespace UsersMS.Swagger
{
    public static class SwaggerUIConfiguration
    {
        public static void UseSwaggerUIConfiguration(this IApplicationBuilder app, IHostEnvironment env, IConfiguration configuration)
        {
            var swaggerConfig = configuration.GetSection("Swagger").Get<SwaggerConfig>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(swaggerConfig?.Endpoint ?? "/swagger/v1/swagger.json", swaggerConfig?.Title ?? "API");
                    options.RoutePrefix = swaggerConfig?.RoutePrefix ?? string.Empty;
                });
            }
        }
    }

}
