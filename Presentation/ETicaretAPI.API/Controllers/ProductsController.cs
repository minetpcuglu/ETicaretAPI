using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductReadRepository _productReadRepo;
        private readonly IProductWriteRepository _writeReadRepo;

        public ProductsController(IProductService productService, IProductReadRepository productReadRepo, IProductWriteRepository writeReadRepo)
        {
            _productService = productService;
            _productReadRepo = productReadRepo;
            _writeReadRepo = writeReadRepo;
        }

        [HttpGet("getproducts")]
        public async  Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("get")]
        public async void Get()
        {
            await  _writeReadRepo.AddRangeAsync(new()
            {
                new() { Id = Guid.NewGuid(), Name = "1", CreatedDate = DateTime.Now, UnitInStock = 10 },
                new() { Id = Guid.NewGuid(), Name = "2", CreatedDate = DateTime.Now, UnitInStock = 20 },
                new() { Id = Guid.NewGuid(), Name = "3", CreatedDate = DateTime.Now, UnitInStock = 30 },

            });
           await _writeReadRepo.SaveAsync();
        }


    }
}
