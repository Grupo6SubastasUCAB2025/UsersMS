using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

namespace UsersMS.Core.Utilities
{
    public static class LoggingConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/UserService-log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        public static void UseCustomSerilogRequestLogging(this WebApplication app, Action<RequestLoggingOptions>? configure = null)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.GetLevel = (httpContext, elapsedMs, ex) =>
                    ex != null || httpContext.Response.StatusCode > 499
                        ? LogEventLevel.Error
                        : LogEventLevel.Information;
                options.IncludeQueryInRequestPath = true;

                configure?.Invoke(options);
            });
        }

        public static void RunLogger(this WebApplication app)
        {
            try
            {
                Log.Information("Iniciando el microservicio de usuarios...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error crítico durante el arranque del servicio.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
