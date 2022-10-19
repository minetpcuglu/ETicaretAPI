using ETicaretAPI.Application.Configuration.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretAPI.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)/*.UseSerilog()*/
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    #region Ms Sql Log verilerin tutulmasý
                    webBuilder.UseStartup<Startup>().UseSerilog();
                    #endregion
                    //webBuilder.UseStartup<Startup>();

                });
    }
}
