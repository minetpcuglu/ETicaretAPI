using ETicaretAPI.Application.Abstractions.Services.Baskets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        private readonly IBasketAppService _basketAppService;

        public GetBasketItemsQueryHandler(IBasketAppService basketAppService)
        {
            _basketAppService = basketAppService;
        }
        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            //ilgili kulllanıya karsılık order olusturulmamıs basket listesi
            var basketItems = await _basketAppService.GetBasketItemsAsync();
            return basketItems.Select(bi => new GetBasketItemsQueryResponse
            {
                BasketItemId= bi.Id.ToString(),
                ProductName=bi.Product.Name,
                ProductPrice=bi.Product.Price,
                Quantity=bi.Quantity
            }).ToList();
        }
    }
}
