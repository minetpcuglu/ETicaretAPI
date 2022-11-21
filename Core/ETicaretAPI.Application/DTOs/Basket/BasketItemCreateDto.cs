using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Basket
{
   public class BasketItemCreateDto
    {
        public string ProductId { get; set; }
        public int  Quantity { get; set; }
    }
}
