using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Orders;
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
        private readonly IOrderReadRepository _orderReadRepo;
        private readonly IProductWriteRepository _productWriteRepo;
        private readonly IOrderWriteRepository _orderWriteRepo;
        private readonly ICustomerWriteRepository _customerWriteRepo;

        public ProductsController(IProductService productService, IProductReadRepository productReadRepo, IOrderReadRepository orderReadRepo, IProductWriteRepository productWriteRepo, IOrderWriteRepository orderWriteRepo, ICustomerWriteRepository customerWriteRepo)
        {
            _productService = productService;
            _productReadRepo = productReadRepo;
            _orderReadRepo = orderReadRepo;
            _productWriteRepo = productWriteRepo;
            _orderWriteRepo = orderWriteRepo;
            _customerWriteRepo = customerWriteRepo;
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
            var customerId = Guid.NewGuid();
            await _customerWriteRepo.AddAsync(new() { Id = customerId, Name = "mineee" });
            await _orderWriteRepo.AddRangeAsync(new()
            {
                
                new() { Description = "bla",Address="kastamonu",CustomerId=customerId },
                new() {  Description = "2",Address="cankırı" ,CustomerId=customerId},
         
            });
            await _orderWriteRepo.SaveAsync();
        }

        [HttpGet("getbyıd")]
        public async Task GetById()
        {
            Order order = await _orderReadRepo.GetByIdAsync("a70c8d6b-6bb1-4996-4925-08da8a61c3b7");
            order.Address = "Van";
            await _orderWriteRepo.SaveAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var value = await _productReadRepo.GetByIdAsync(id);
            return Ok(value);
        }
    }
}
