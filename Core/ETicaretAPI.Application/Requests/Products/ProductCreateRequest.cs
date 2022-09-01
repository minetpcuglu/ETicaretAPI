using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Requests.Products
{
   public class ProductCreateRequest
    {
        public string Name { get; set; }
        public int UnitInStock { get; set; }
        public float Price { get; set; }
    }
}
