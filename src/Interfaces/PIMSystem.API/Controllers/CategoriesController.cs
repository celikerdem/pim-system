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
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCategoryRequest request)
        {
            var entity = new Category
            {
                CategoryId = request.CategoryId,
                Name = request.Name
            };

            var serviceResponse = await _categoryService.CreateCategoryAsync(entity);

            if (!serviceResponse.HasError)
                return Created(string.Empty, new { id = entity.Id });
            else
                return BadRequest(new { id = entity.Id, errors = serviceResponse.Errors });
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterCategories([FromBody]FilterDatatableRequest request)
        {
            var response = new FilterDatatableResponse<List<Category>>();
            var pagedRequest = new BasePagedRequest
            {
                Offset = request.Start,
                Limit = request.Length
            };
            var serviceResponse = await _categoryService.GetCategoriesAsync(pagedRequest);
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
