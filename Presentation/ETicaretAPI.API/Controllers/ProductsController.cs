using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Features.Commands.ProductImageFiles.DeleteProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFiles.UploadProductImage;
using ETicaretAPI.Application.Features.Commands.Products.Create;
using ETicaretAPI.Application.Features.Commands.Products.DeleteProduct;
using ETicaretAPI.Application.Features.Commands.Products.Update;
using ETicaretAPI.Application.Features.Queries.ProductImageFiles.GetProductImages;
using ETicaretAPI.Application.Features.Queries.Products.GetAll;
using ETicaretAPI.Application.Features.Queries.Products.GetById;
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
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize/*(AuthenticationSchemes="Admin")*/]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProducts([FromQuery] GetAllProductQueryRequest request) //data queryden geliyo
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductGetById/{Id}")] //requestteki isimle aynı olmalıki bind edebilsin
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProduct([FromRoute] GetByIdProductQueryRequest request) //data routedan geliyo
        {
            GetByIdProductQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost()]
        [Route("AddProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok();
        }


        [HttpPut()]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete()]
        [Route("DeleteProduct/{Id}")] //[fromroute] ıd routen parametre olarak geliyor
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductCommandRequest request)
        {
            DeleteProductCommandResponse response = await _mediator.Send(request); ///var value = da olabilir
            return Ok();
        }

        [HttpPost()]
        [Route("upload")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)] //ıd route parametre olarak gelmiyor query string olrak geliyo o zaman form query denilir.
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest request)
        {
            request.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(request);
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
        [HttpGet]
        [Route("GetProductImageDetail/{Id}")]
        //[ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductImageDetail([FromRoute] GetProductImageQueryRequest request)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete()]
        [Route("DeleteImageProduct/{Id}")]   //Id routedan ımageId Queryden imageId
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteImageProduct([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] string imageId)
        {
            request.ImageId = imageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }
    }
}
