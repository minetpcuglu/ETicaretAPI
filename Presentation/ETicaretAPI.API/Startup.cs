﻿using ETicaretAPI.Application;
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

            services.AddOptions(); //appsettting dosya okuma
            #region Context
            services.AddScoped<ETicaretAPIDbContext>();
            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            #endregion

            #region CORS 
            services.AddCors(options => options.AddDefaultPolicy(policy =>
              policy.WithOrigins("http://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
            #endregion

            #region FluentValidation
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())//filters devreye sokma 
                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidation>())//fluent validation için bir tane validation adresi versek yeterli <CreateProductValidation> reflection ile bulup register ediyor
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); //default geleni false yap
            #endregion

            #region IoC
            services.AddPersistenceServices(); //IoC container Extension ile yaptýk.
            services.AddApplicationServices(); //IoC container Extension ile yaptýk.
            services.AddInfrastructureServices(); //IoC container Extension ile yaptýk.
            //services.AddStorage<LocalStorage>();//IoC container Extension ile yaptýk.  
            services.AddStorage<AzureStorage>();//IoC container Extension ile yaptýk.  
                                                //services.AddStorage(StorageType.Local); -- Bu sekildede kullanýlabilir //enum ile kullaným hali
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

            app.UseHttpsRedirection();
            app.UseStaticFiles(); //file upload için

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}