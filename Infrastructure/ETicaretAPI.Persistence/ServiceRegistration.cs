using Microsoft.EntityFrameworkCore;
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Persistence.Concretes;
using ETicaretAPI.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ETicaretAPI.Persistence.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Persistence.Repositories.Products;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Persistence.Repositories.Orders;

namespace ETicaretAPI.Persistence
{
    //Extension method static tanımlanır (IoC Container icin kuruldu (built))
   public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ICustomerReadRepository, CustomerReadRepository>();
            services.AddSingleton<IProductReadRepository, ProductReadRepository>();
            services.AddSingleton<IProductWriteRepository, ProductWriteRepository>();
            services.AddSingleton<IOrderReadRepository, OrderReadRepository>();
            services.AddSingleton<IOrderWriteRepository, OrderWriteRepository>();
            services.AddSingleton<ICustomerWriteRepository, CustomerWriteRepository>();
        }
    }
}
