using ETicaretAPI.Application.Utilities.Security.Token;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application
{
   public static class ServiceRegistration
    {
        //Extension method static tanımlanır (IoC Container icin kuruldu (built))
     
            public static void AddApplicationServices(this IServiceCollection services)
            {
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddMediatR(typeof(ServiceRegistration));
            }
        
        
    }
}
