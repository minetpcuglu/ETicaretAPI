using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Requests.Products
{
   public class ProductUpdateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public float Price { get; set; }
    }
}
