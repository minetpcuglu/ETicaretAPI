using ETicaretAPI.Persistence;
using ETicaretAPI.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretAPI.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Context
            services.AddScoped<ETicaretAPIDbContext>();
            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            #endregion

            #region CORS 
            services.AddCors(options => options.AddDefaultPolicy(policy =>
              policy.WithOrigins("http://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
            #endregion

            services.AddControllers();
            services.AddControllersWithViews();

            services.AddPersistenceServices(); //IoC container Extension ile yaptýk.
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
