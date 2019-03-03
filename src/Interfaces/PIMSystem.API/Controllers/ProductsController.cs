using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PIMSystem.Core.Service.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.API.Models.Requests;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.API.Models.Responses;

namespace PIMSystem.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateProductRequest request)
        {
            var entity = new Product
            {
                ZamroId = request.ZamroId,
                Name = request.Name,
                Description = request.Description,
                MinOrderQuantity = request.MinOrderQuantity,
                UnitOfMeasure = request.UnitOfMeasure,
                CategoryId = request.CategoryId,
                PurchasePrice = request.PurchasePrice,
                Available = request.Available
            };

            var serviceResponse = await _productService.CreateProductAsync(entity);

            if (!serviceResponse.HasError)
                return Created(string.Empty, new { id = entity.Id });
            else
                return BadRequest(new { id = entity.Id, errors = serviceResponse.Errors });
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts([FromBody]FilterDatatableRequest request)
        {
            var response = new FilterDatatableResponse<List<Product>>();
            var pagedRequest = new BasePagedRequest
            {
                Offset = request.Start,
                Limit = request.Length
            };
            var serviceResponse = await _productService.GetProductsAsync(pagedRequest);
            response.Data = serviceResponse.Items;
            response.RecordsTotal = serviceResponse.Total;
            response.RecordsFiltered = serviceResponse.Total;
            response.Draw = request.Draw;

            if (serviceResponse.Items != null)
                return Ok(response);
            else
                return NotFound(serviceResponse);
        }
    }
}
