using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories.ProductImageFiles;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Files;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFiles.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IProductImageReadRepository _productImageFileReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IStorageService _storageService;

        public DeleteProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageReadRepository productImageFileReadRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IStorageService storageService)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _storageService = storageService;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Product product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                 .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));
            if (productImageFile!=null)
            product?.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return new();
        }
    }
}
