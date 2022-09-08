﻿using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.IServices.File;
using ETicaretAPI.Application.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Requests.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductService productService, IFileService fileService, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _fileService = fileService;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetProducts([FromQuery]Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count() ;
            var products = _productReadRepository.GetAll(false)//tracking veri tabanı ile ilgili herhangi bir işlem yapılmadıgı için false cagırılıyor
                .Skip(pagination.Page * pagination.Size) //önce skip ile hangi aralıga gidilcekse gidilir
                .Take(pagination.Size) //sonra take ile alınır
                .Select(p=> new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.UnitInStock,
                    p.CreatedDate,
                    p.UpdatedDate
                }); 
            return Ok(new {
                totalCount,
                products
            });
        }

        [HttpGet]
        [Route("GetProductGetById/{id}")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetProduct(string id)
        {
            var products = _productReadRepository.GetByIdAsync(id,false);
            return Ok(products);
        }

        [HttpPost()]
        [Route("AddProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreateRequest request)
        {
            if (ModelState.IsValid)
            {

            }
            var products = _productWriteRepository.AddAsync(new()
            {
                Name=request.Name,
                UnitInStock=request.UnitInStock,
                Price = request.Price
            });
            await _productWriteRepository.SaveAsync();
            return Ok(products);
        }


        [HttpPut()]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            Product product = await _productReadRepository.GetByIdAsync(request.Id);
            product.Name = request.Name;
            product.Price = request.Price;
            product.UnitInStock = request.UnitInStock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete()]
        [Route("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpPost()]
        [Route("upload")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Upload()
        {
           await _fileService.UploadAsync("resource/products-images", Request.Form.Files);
           return Ok();
        }
    }
}
