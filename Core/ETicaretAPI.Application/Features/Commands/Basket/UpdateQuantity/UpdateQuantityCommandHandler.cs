using ETicaretAPI.Application.Abstractions.Services.Baskets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
    {

        private readonly IBasketAppService _basketAppService;

        public UpdateQuantityCommandHandler(IBasketAppService basketAppService)
        {
            _basketAppService = basketAppService;
        }

        public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
        {

           await _basketAppService.UpdateQuantityAsync(new()
            {
                BasketItemId = request.BasketItemId,
                Quantity = request.Quantity
            });
            return new(); //geriye ilgili nesne gönderildi

        }
    }
}
