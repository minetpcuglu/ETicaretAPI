using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Domain.Entities;
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
        private readonly IProductWriteRepository _productWriteRepo;

        public ProductsController(IProductService productService, IProductReadRepository productReadRepo, IProductWriteRepository productWriteRepo)
        {
            _productService = productService;
            _productReadRepo = productReadRepo;
            _productWriteRepo = productWriteRepo;
        }

        [HttpGet("getproducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("get")]
        public async Task Get()
        {
            //await _productWriteRepo.AddRangeAsync(new()
            //{
            //    new() { Id = Guid.NewGuid(), Name = "1", CreatedDate = DateTime.Now, UnitInStock = 10 },
            //    new() { Id = Guid.NewGuid(), Name = "2", CreatedDate = DateTime.Now, UnitInStock = 20 },
            //    new() { Id = Guid.NewGuid(), Name = "3", CreatedDate = DateTime.Now, UnitInStock = 30 },
            //});
            Product p = await _productReadRepo.GetByIdAsync("530fb940-12ec-4e6b-9dc0-d9ea13edad26",false);
            p.Name = "Fen";
            await _productWriteRepo.SaveAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var value = await _productReadRepo.GetByIdAsync(id);
            return Ok(value);
        }
    }
}
