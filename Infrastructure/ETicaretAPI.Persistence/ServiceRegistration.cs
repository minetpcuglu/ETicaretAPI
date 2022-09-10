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
using ETicaretAPI.Persistence.Repositories.Files;
using ETicaretAPI.Application.Repositories.Files;
using ETicaretAPI.Persistence.Repositories.ProductImageFiles;
using ETicaretAPI.Application.Repositories.ProductImageFiles;
using ETicaretAPI.Application.Repositories.InvoiceFiles;
using ETicaretAPI.Persistence.Repositories.InvoiceFiles;

namespace ETicaretAPI.Persistence
{
    //Extension method static tanımlanır (IoC Container icin kuruldu (built))
   public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IFileReadRepository,FileReadRepository>();
            services.AddScoped<IProductImageReadRepository,ProductImageFileReadRepository>();
            services.AddScoped<IFileWriteRepository,FileWriteRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
            services.AddScoped<IInvoiceFileWriteRepository,InvoiceFileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository,InvoiceFileReadRepository>();

        }
    }
}
