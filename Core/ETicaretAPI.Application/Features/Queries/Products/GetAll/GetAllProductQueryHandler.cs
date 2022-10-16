using ETicaretAPI.Application.Repositories.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Products.GetAll
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly ILogger<GetAllProductQueryHandler> _logger;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ürünler listelendi");
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)//tracking veri tabanı ile ilgili herhangi bir işlem yapılmadıgı için false cagırılıyor
                .Skip(request.Page * request.Size) //önce skip ile hangi aralıga gidilcekse gidilir
                .Take(request.Size) //sonra take ile alınır
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.UnitInStock,
                    p.CreatedDate,
                    p.UpdatedDate
                }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
