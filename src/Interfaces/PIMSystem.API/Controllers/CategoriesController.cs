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
                return Ok(new { id = entity.Id });
            else
                return BadRequest(serviceResponse.Errors);
        }
    }
}
