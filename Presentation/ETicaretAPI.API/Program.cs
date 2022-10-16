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
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            SqlColumn sqlColumn = new SqlColumn();
            sqlColumn.ColumnName = "UserName";
            sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
            sqlColumn.PropertyName = "UserName";
            sqlColumn.DataLength = 50;
            sqlColumn.AllowNull = true;
            ColumnOptions columnOpt = new ColumnOptions();
            columnOpt.Store.Remove(StandardColumn.Properties);
            columnOpt.Store.Add(StandardColumn.LogEvent);
            columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console().WriteTo.Debug(Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("logs/log.txt")
                .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString("SqlServer") /*logDB*/,
                 sinkOptions: new MSSqlServerSinkOptions
                 {
                     AutoCreateSqlTable = true,
                     TableName = "logs",
                 },     
                 appConfiguration: null,
                 columnOptions:columnOpt
                )
                .Enrich.FromLogContext() //harici bir prop kullanmak isteniyorsa
                .Enrich.With<CustomUserNameColumn>()
                .MinimumLevel.Information().ReadFrom.Configuration(configuration)
                .CreateLogger(); 
            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
    public class CustomUserNameColumn : ILogEventEnricher
    {
        public void Enrich(Serilog.Events.LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var (username, value) = logEvent.Properties.FirstOrDefault(x => x.Key == "UserName");
            if (value != null)
            {
                var getValue = propertyFactory.CreateProperty(username, value);
                logEvent.AddPropertyIfAbsent(getValue);
            }
        }
    }
}
