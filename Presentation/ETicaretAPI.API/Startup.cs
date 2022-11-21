using ETicaretAPI.Application;
using ETicaretAPI.Application.Utilities.Security.JWT;
using ETicaretAPI.Application.CrossCuttingConcerns.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using ETicaretAPI.Persistence.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Application.Utilities.Security.Encryption;
using ETicaretAPI.Application.Configuration;
using Serilog;
using ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.Logging;
using Serilog.Context;
using Serilog.Sinks.MSSqlServer;
using ETicaretAPI.Application.Configuration.Logging;
using System.Collections.ObjectModel;
using ETicaretAPI.ISignalR;
using ETicaretAPI.ISignalR.Hubs;

namespace ETicaretAPI.API
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region SeriLog_ MsSql Log Table Create
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
            #endregion

            #region SeriLog Konfigruasyonu
            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console().WriteTo.Debug(Serilog.Events.LogEventLevel.Information)
               //.WriteTo.File("logs/log.txt")
               .WriteTo.MongoDBBson("mongodb://localhost:27017/Eticaret_db", collectionName: "Log")
               .WriteTo.File("Logs/log.txt")
               .WriteTo.Seq("http://localhost:5341/")
               .WriteTo.MSSqlServer(
               connectionString: configuration.GetConnectionString("SqlServer") /*logDB*/,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = true,
                    TableName = "logs",
                },
                appConfiguration: null,
                columnOptions: columnOpt
               )
               .Enrich.FromLogContext() //harici bir prop kullanmak isteniyorsa
               .Enrich.With<CustomUserNameColumn>()
               .MinimumLevel.Information().ReadFrom.Configuration(configuration)
               .CreateLogger();
            #endregion

            services.AddOptions(); //appsettting dosya okuma

            #region Context
            services.AddScoped<ETicaretAPIDbContext>();
            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            #endregion

            #region CORS 
            services.AddCors(options => options.AddDefaultPolicy(policy =>
              policy.WithOrigins("http://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));  //signalr için crediantel izin verildi
            #endregion

            #region FluentValidation
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())//filters devreye sokma 
                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidation>())//fluent validation için bir tane validation adresi versek yeterli <CreateProductValidation> reflection ile bulup register ediyor
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); //default geleni false yap
            #endregion

            #region IoC
            services.AddHttpContextAccessor();//clienttan gelen request neticesinde olusturualan http context nesnesine katmanlardaki claslar üzerinden (business logic üzerinden ) erişebilmemizi saglayan servis
            services.AddPersistenceServices(); //IoC container Extension ile yaptýk.
            services.AddApplicationServices(); //IoC container Extension ile yaptýk.
            services.AddInfrastructureServices(); //IoC container Extension ile yaptýk.
            services.AddStorage<LocalStorage>();//IoC container Extension ile yaptýk.  
            //services.AddStorage<AzureStorage>();//IoC container Extension ile yaptýk.  
                                                //services.AddStorage(StorageType.Local); -- Bu sekildede kullanýlabilir //enum ile kullaným hali
            services.AddSignalRServices();
            #endregion


            #region JWT
            //Jwt Authentication
            services.AddMyJwtAuthentication(Configuration);
            #endregion

            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETicaretAPI.API", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ETicaretAPI.API v1"));
            }

            #region SeriLog //nerede loglama istersen oranın ustune koy
            //app.UseSerilogRequestLogging(); //gerek yok 
            app.UseMiddleware<GlobalExceptionMiddleware>(); //log için middleware cagırıldı
            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles(); //file upload için

            app.UseCors();

            app.UseRouting();

            #region JWT
            app.UseAuthentication();
            #endregion



            app.UseAuthorization();

            #region User Authentice olmusmu
            app.Use(async (context, next) =>
            {
                //gelen context icerisinde user name geldi ve gelen requestte jwt varsa name döndür yoksa null degeri ver
                var userName = context.User.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
                LogContext.PushProperty("UserName", userName);
                await next(); //işlem devam etsin
            });
            #endregion

            #region SignalR
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ProductHub>("/products-hub"); //signalR
            });
            #endregion
        }
    }
}