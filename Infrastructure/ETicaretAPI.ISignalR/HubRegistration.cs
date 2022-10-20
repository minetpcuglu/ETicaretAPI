using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.ISignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.ISignalR
{
   public static class HubRegistration
    {
        public static void AddMapHubs(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddSignalR();
        }
    }
}
