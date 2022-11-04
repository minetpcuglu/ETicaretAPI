using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities.Files
{
   public class ProductImageFile:File
    {
        public bool Showcase { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
