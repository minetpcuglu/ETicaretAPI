﻿using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Files;
using ETicaretAPI.Application.Repositories.InvoiceFiles;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Application.Repositories.ProductImageFiles;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Requests.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IStorageService _storageService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IProductImageReadRepository _productImageFileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration configuration;

        public ProductsController(IProductService productService, IStorageService storageService, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageReadRepository productImageFileReadRepository, IFileWriteRepository fileWriteRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IFileReadRepository fileReadRepository, IWebHostEnvironment webHostEnvironment, IConfiguration configuration = null)
        {
            _productService = productService;
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _fileReadRepository = fileReadRepository;
            _webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetProducts([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)//tracking veri tabanı ile ilgili herhangi bir işlem yapılmadıgı için false cagırılıyor
                .Skip(pagination.Page * pagination.Size) //önce skip ile hangi aralıga gidilcekse gidilir
                .Take(pagination.Size) //sonra take ile alınır
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.UnitInStock,
                    p.CreatedDate,
                    p.UpdatedDate
                });
            return Ok(new
            {
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
            var products = _productReadRepository.GetByIdAsync(id, false);
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
                Name = request.Name,
                UnitInStock = request.UnitInStock,
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
        public async Task<IActionResult> Upload(string id)
        {
            var datas = await _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id, true); //tracking true update işlemi yapıcak

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>(){product}
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price=new Random().Next()
            //}).ToList());
            //await _invoiceFileWriteRepository.SaveAsync();

            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ETicaretAPI.Domain.Entities.Files.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _fileWriteRepository.SaveAsync();

            ////var d1 = _productImageFileReadRepository.GetAll(false);
            return Ok();
        }

        [HttpPost()]
        [Route("AddProductImageFile")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddProductImageFile([FromBody] ProductCreateRequest request)
        {
            if (ModelState.IsValid)
            {

            }
            var products = _productWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                UnitInStock = request.UnitInStock,
                Price = request.Price
            });
            await _productWriteRepository.SaveAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProductImageDetail/{id}")]
        //[ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductImageDetail(string id)
        {

            //var data = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
            //    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

          
            Product product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                 .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                //Path = $"{p.Storage}/{p.Path}",
                Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",  //azure göre configre edildi
                //Path = $"{}/{p.Path}",  //azure göre configre edildi
                p.FileName,
                p.Id
            })) ;
        }
        [HttpDelete()]
        [Route("DeleteImageProduct/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteImageProduct(string id, string imageId)
        {
            Product product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                 .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
