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
    [Route("api/upload-items")]
    [ApiController]
    public class UploadItemsController : BaseController
    {
        private readonly IUploadItemService _uploadItemService;

        public UploadItemsController(IUploadItemService uploadItemService)
        {
            _uploadItemService = uploadItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUploadItemRequest request)
        {
            var entity = new UploadItem
            {
                UploadId = request.UploadId,
                ItemId = request.ItemId,
                Success = request.Success
            };

            var serviceResponse = await _uploadItemService.CreateUploadItemAsync(entity);

            if (!serviceResponse.HasError)
                return Created(string.Empty, new { id = entity.Id });
            else
                return BadRequest(new { id = entity.Id, errors = serviceResponse.Errors });
        }
    }
}
