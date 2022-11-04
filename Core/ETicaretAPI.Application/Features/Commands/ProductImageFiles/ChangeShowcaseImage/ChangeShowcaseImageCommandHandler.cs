using ETicaretAPI.Application.Repositories.ProductImageFiles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFiles.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
       public readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table  //table girip include etmek
                      .Include(p => p.Products)
                      .SelectMany(p => p.Products, (pif, p) => new //çokaçok ilişki oldugundan ürünlere girip (productimageFile ile productları elde etmek )
                      {
                          pif,
                          p
                      });

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase); 
            //product image file dan showcase true olanlrı first or defult ile elde edildi

            if (data != null)
                data.pif.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageId)); //gelen yeni image true yapmak
            if (image != null)
                image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}

