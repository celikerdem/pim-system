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
using PIMSystem.Core.Domain.Requests;
using PIMSystem.API.Models.UploadModels;

namespace PIMSystem.API.Controllers
{
    [Route("api/uploads")]
    [ApiController]
    public class UploadsController : BaseController
    {
        private const string CategoryTableName = "Category";
        private const string ProductTableName = "Product";
        private readonly IUploadService _uploadService;
        private readonly IUploadItemService _uploadItemService;
        private readonly IMqService _mqService;

        public UploadsController(IUploadService uploadService, IUploadItemService uploadItemService, IMqService mqService)
        {
            _uploadService = uploadService;
            _uploadItemService = uploadItemService;
            _mqService = mqService;
        }

        [HttpPost("csv")]
        public async Task<IActionResult> PostCsvFile(IFormFile file)
        {
            try
            {
                //Validate file name
                var tableName = string.Empty;
                if (file.FileName.Contains('.'))
                    switch (file.FileName.Split('.')[0])
                    {
                        case "CategoryData":
                            tableName = CategoryTableName;
                            break;
                        case "ProductData":
                            tableName = ProductTableName;
                            break;
                    }
                if (string.IsNullOrEmpty(tableName))
                    return BadRequest("File name is not accepted! Possible file names: CategoryData.csv, ProductData.csv");

                //Create Upload entity
                var entity = new Upload
                {
                    FileName = file.FileName,
                    TableName = tableName
                };
                var serviceResponse = await _uploadService.CreateUploadAsync(entity);
                if (serviceResponse.HasError)
                    return BadRequest(serviceResponse.Errors);

                int itemCount = 0;
                List<string> failedRows = null;
                List<object> eventMessages = new List<object>();

                //Read csv data
                switch (tableName)
                {
                    case CategoryTableName:
                        var csvCategoryResponse = CsvFileHelper.ReadFromStream<CategoryUploadModel>(file.OpenReadStream());
                        failedRows = csvCategoryResponse.FailedRows;
                        itemCount = csvCategoryResponse.Data.Count();
                        csvCategoryResponse.Data.ForEach(x =>
                        {
                            var eventMessage = new CategoryImportedEvent
                            {
                                UploadId = entity.Id,
                                CategoryID = x.CategoryID,
                                Name = x.Name
                            };
                            eventMessages.Add(eventMessage);
                        });
                        break;
                    case ProductTableName:
                        var csvProductResponse = CsvFileHelper.ReadFromStream<ProductUploadModel>(file.OpenReadStream());
                        failedRows = csvProductResponse.FailedRows;
                        itemCount = csvProductResponse.Data.Count();
                        csvProductResponse.Data.ForEach(x =>
                        {
                            var eventMessage = new ProductImportedEvent
                            {
                                UploadId = entity.Id,
                                ZamroID = x.ZamroID,
                                Name = x.Name,
                                Description = x.Description,
                                MinOrderQuantity = x.MinOrderQuantity,
                                UnitOfMeasure = x.UnitOfMeasure,
                                CategoryID = x.CategoryID,
                                PurchasePrice = x.PurchasePrice,
                                Available = x.Available
                            };
                            eventMessages.Add(eventMessage);
                        });
                        break;
                }

                //Update Upload entity with ItemCount
                entity.ItemCount = itemCount + failedRows.Count(); //Total uploaded data count
                var updateResponse = await _uploadService.UpdateUploadAsync(entity);
                if (updateResponse.HasError)
                    return BadRequest(updateResponse.Errors);

                //Create UploadItem for each failed row
                if (failedRows.Count() > 0)
                {
                    foreach (var failedRow in failedRows)
                    {
                        await _uploadItemService.CreateUploadItemAsync(new UploadItem
                        {
                            UploadId = entity.Id,
                            ItemId = -1,
                            Success = false
                        });
                    }
                }

                //Publish imported events
                Parallel.ForEach(eventMessages, async eventMessage =>
                {
                    await _mqService.PublishEvent(eventMessage);
                });

                return Accepted(new { id = entity.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetUploadProgress(int id)
        {
            var upload = await _uploadService.GetUploadAsync(id);
            if (upload.Data == null)
                return NotFound();

            var totalCount = upload.Data.ItemCount;

            var uploadItems = await _uploadItemService.GetUploadItemsAsync(new UploadItemPagedRequest
            {
                UploadId = upload.Data.Id,
                Offset = 0,
                Limit = int.MaxValue
            });

            return Ok(new
            {
                id = id,
                totalCount = totalCount,
                successCount = uploadItems.Items.Count(x => x.Success),
                failCount = uploadItems.Items.Count(x => !x.Success)
            });
        }
    }
}
