using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PIMSystem.Core.Service.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.API.Models.Requests;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Cors;
using PIMSystem.Core.Helper;
using PIMSystem.Core.Domain.Responses;
using PIMSystem.Core.Domain.Events;
using PIMSystem.Core.Service.Event;

namespace PIMSystem.API.Controllers
{
    [Route("api/uploads")]
    [ApiController]
    public class UploadsController : BaseController
    {
        private readonly IUploadService _uploadService;
        private readonly IMqService _mqService;

        public UploadsController(IUploadService uploadService, IMqService mqService)
        {
            _uploadService = uploadService;
            _mqService = mqService;
        }

        [HttpPost("csv")]
        public async Task<IActionResult> PostCsvFile(IFormFile file)
        {
            //Validate file name
            var tableName = string.Empty;
            if (file.FileName.Contains('.'))
                switch (file.FileName.Split('.')[0])
                {
                    case "CategoryData":
                        tableName = "Category";
                        break;
                    case "ProductData":
                        tableName = "Product";
                        break;
                }
            if (string.IsNullOrEmpty(tableName))
                return BadRequest("File name is not accepted! Possible file names: CategoryData.csv, ProductData.csv");

            int itemCount = 0;
            List<string> failedRows = null;
            List<object> eventMessages = null;

            //Read csv data
            switch (tableName)
            {
                case "Category":
                    var csvCategoryResponse = CsvFileHelper.ReadFromStream<CategoryImportedEvent>(file.OpenReadStream());
                    failedRows = csvCategoryResponse.FailedRows;
                    itemCount = csvCategoryResponse.Data.Count();
                    eventMessages = csvCategoryResponse.Data.Select(x => x as object).ToList();
                    break;
                case "Product":
                    var csvProductResponse = CsvFileHelper.ReadFromStream<ProductImportedEvent>(file.OpenReadStream());
                    failedRows = csvProductResponse.FailedRows;
                    itemCount = csvProductResponse.Data.Count();
                    eventMessages = csvProductResponse.Data.Select(x => x as object).ToList();
                    break;
            }
            // if (failedRows.Count() > 0)
            //     return BadRequest(failedRows);

            //Create Upload entity
            var entity = new Upload
            {
                FileName = file.FileName,
                TableName = tableName,
                ItemCount = itemCount
            };
            var serviceResponse = await _uploadService.CreateUploadAsync(entity);
            if (serviceResponse.HasError)
                return BadRequest(serviceResponse.Errors);

            //Publish imported events
            Parallel.ForEach(eventMessages, async eventMessage =>
            {
                await _mqService.PublishEvent(eventMessage);
            });

            return Accepted(new { id = entity.Id });
        }

        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetUploadProgress(int id)
        {
            var entity = await _uploadService.GetUploadAsync(id);
            return Ok();
        }
    }
}
