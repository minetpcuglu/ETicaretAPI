using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.ISignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.ISignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        private readonly IHubContext<ProductHub> _hubContext;

        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
           //await _hubContext.Clients.All.SendAsync("receiverProductAddedMessage",message);
           await _hubContext.Clients.All.SendAsync(ReceiverFunctionNames.ProductAddedMessage,message);  //tüm clientlerda hangi fonksiyona özel mesaj gönderilcek
        }
    }
}
