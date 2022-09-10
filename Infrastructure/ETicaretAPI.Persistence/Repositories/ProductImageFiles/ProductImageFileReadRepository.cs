using ETicaretAPI.Application.Repositories.ProductImageFiles;
using ETicaretAPI.Domain.Entities.Files;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.ProductImageFiles
{
   public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageReadRepository
    {
        public ProductImageFileReadRepository(ETicaretAPIDbContext context) : base(context)
        {

        }
    }
}
