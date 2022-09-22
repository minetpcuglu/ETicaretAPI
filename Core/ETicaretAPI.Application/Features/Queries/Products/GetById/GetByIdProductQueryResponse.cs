using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Products.GetById
{
   public class GetByIdProductQueryResponse
    {
        public string Name { get; set; }
        public int UnitInStock { get; set; }
        public float Price { get; set; }

    }
}
