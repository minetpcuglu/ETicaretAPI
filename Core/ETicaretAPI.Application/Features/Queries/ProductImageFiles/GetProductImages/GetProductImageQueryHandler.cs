using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFiles.GetProductImages
{
    public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration configuration;

        public GetProductImageQueryHandler(IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
        {
            //var data = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
            //    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            try
            {

                //wwwroot/resource/product-images

                //GetProductImageQueryResponse response = new GetProductImageQueryResponse();
                Product product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath/*, response.Path  */);
                return product.ProductImageFiles.Select(p => new GetProductImageQueryResponse
                {

                    //Path=p.Path,
                    //Path = $"{p.Storage}\\{p.Path}",
                    //Path = $"{p.Path}",
                    //Path = "C:/Users/User/Desktop/ETicaret/ETicaretAPI/Presentation/ETicaretAPI.API/wwwroot" $"{p.Path}",
                    //Path = $"{p.Storage}/{p.Path}",
                    /* Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",*/  //azure göre configre edildi
                                                                                //Path = $"{}/{p.Path}",  //azure göre configre edildi

                    //Path = $"{p.Storage}/{p.Path}",
                    Path = $"{uploadPath}\\{p.Path}",
                    FileName = p.FileName,
                    Id = p.Id
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            //DirectoryInfo directory = new(path);
            //return directory.GetFiles().Select(f => f.Name).ToList();

        }
    }
}
