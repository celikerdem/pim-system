using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PIMSystem.Core.Service.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.API.Models.Requests;

namespace PIMSystem.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService _ProductService;

        public ProductsController(IProductService ProductService)
        {
            _ProductService = ProductService;
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

            var serviceResponse = await _ProductService.CreateProductAsync(entity);

            if (!serviceResponse.HasError)
                return Ok(new { id = entity.Id });
            else
                return BadRequest(serviceResponse.Errors);
        }
    }
}
