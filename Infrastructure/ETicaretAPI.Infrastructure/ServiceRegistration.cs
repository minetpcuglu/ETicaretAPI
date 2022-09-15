using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services.File;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    //Extension method static tanımlanır (IoC Container icin kuruldu (built))
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>(); //servise karsılık hangi sevisin clısacgını asagıda belirledik
        }
        public static void AddStorage<T>(this IServiceCollection services) where T :class,IStorage //tercih edilen 
        {
            services.AddScoped<IStorage, T>();
        }

        //enum ile kullanılmıs hali
        public static void AddStorage(this IServiceCollection services,StorageType storageType) //bagımlılık var
        {
            switch (storageType)
            {
                case StorageType.Local:
                    //services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    //services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    //services.AddScoped<IStorage, AWSStorage>();
                    break;
                default:
                   //services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

    }
}
