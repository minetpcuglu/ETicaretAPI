using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public async Task<List<Product>> GetProducts() 
            => new() 
            {
            new() { Id=Guid.NewGuid(),Name="Mine",Price=10 , UnitInStock=5},
            new() { Id=Guid.NewGuid(),Name="elif",Price=100 , UnitInStock=50},
            new() { Id=Guid.NewGuid(),Name="emre",Price=1000 , UnitInStock=500},

            };
      
    }
}
