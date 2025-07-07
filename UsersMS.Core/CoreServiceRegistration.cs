using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Core.Utilities;

namespace UsersMS.Core
{
    public static class CoreServiceRegistration
    {
        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            LoggingConfiguration.ConfigureSerilog(builder);
        }

        public static void UseLogging(WebApplication app)
        {
            LoggingConfiguration.UseCustomSerilogRequestLogging(app);
        }
    }
}
