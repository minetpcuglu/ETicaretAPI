using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Repositories.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Products.Create
{
    //Mediatr kütüphanesi 
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductHubService _productHubService;

        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
        {
            _productWriteRepository = productWriteRepository;
            _productHubService = productHubService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var products = _productWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                UnitInStock = request.UnitInStock,
                Price = request.Price
            });
            await _productWriteRepository.SaveAsync();
            await _productHubService.ProductAddedMessageAsync($"{request.Name} isminde ürün eklenmiştir.");
            //throw new Exception("Log ekleme işllemi için calısıyor");
            return new(); //herhangi bir parametresi olmasıdıgndan CreateProductCommandResponse geriye döndürüyoruz
        }
    }
}
