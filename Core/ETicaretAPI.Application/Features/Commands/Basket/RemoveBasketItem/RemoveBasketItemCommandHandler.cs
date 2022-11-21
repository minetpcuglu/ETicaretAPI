using ETicaretAPI.Application.Abstractions.Services.Baskets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
    {
        private readonly IBasketAppService _basketAppService;

        public RemoveBasketItemCommandHandler(IBasketAppService basketAppService)
        {
            _basketAppService = basketAppService;
        }

        public async Task<RemoveBasketItemCommandResponse> Handle(RemoveBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketAppService.RemoveBasketItemAsync(request.BasketItemId);
            return new(); //ilgili nesne verildi geriye döndürülme
        }
    }
}
