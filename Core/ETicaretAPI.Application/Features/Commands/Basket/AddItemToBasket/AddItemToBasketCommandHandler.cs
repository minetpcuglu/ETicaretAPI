using ETicaretAPI.Application.Abstractions.Services.Baskets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Basket.AddItemToBasket
{
    public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
    {
        private readonly IBasketAppService _basketAppService;

        public AddItemToBasketCommandHandler(IBasketAppService basketAppService)
        {
            _basketAppService = basketAppService;
        }

        public async Task<AddItemToBasketCommandResponse> Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
        {
           await  _basketAppService.AddItemToBasketAsync(new()
            {
                ProductId = request.ProductId,
                Quantity=request.Quantity
            });
            return new();
        }
    }
}
