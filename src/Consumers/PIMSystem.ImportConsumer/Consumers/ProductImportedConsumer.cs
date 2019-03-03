using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PIMSystem.ImportConsumer.Consumers
{
    public class ProductImportedConsumer : IConsumer<ProductImportedEvent>
    {
        public ProductImportedConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ProductImportedEvent> context)
        {
            try
            {
                // var pimApiBaseUrl = "http://localhost:5000/api";
                var pimApiBaseUrl = "https://pim-system-api.azurewebsites.net/api";
                var productsApiEndpoint = pimApiBaseUrl + "/products";
                var uploadItemsApiEndpoint = pimApiBaseUrl + "/upload-items";

                //Create entity
                var entityId = 0;
                var client = new HttpClient();
                var postProductObject = new Product
                {
                    ZamroId = context.Message.ZamroID,
                    Name = context.Message.Name,
                    Description = context.Message.Description,
                    MinOrderQuantity = context.Message.MinOrderQuantity,
                    UnitOfMeasure = context.Message.UnitOfMeasure,
                    CategoryId = context.Message.CategoryID,
                    PurchasePrice = context.Message.PurchasePrice,
                    Available = context.Message.Available
                };
                var postProductResponse = await client.PostAsync(productsApiEndpoint
                    , new StringContent(JsonConvert.SerializeObject(postProductObject), Encoding.UTF8, "application/json"));
                if (postProductResponse.StatusCode == HttpStatusCode.Created)
                {
                    var responseObject = JObject.Parse(await postProductResponse.Content.ReadAsStringAsync());
                    entityId = Convert.ToInt32(responseObject["id"].ToString());
                }

                //Create UploadItem, success or fail
                if (context.Message.UploadId > 0 && entityId > 0)
                {
                    var postUploadItemObject = new UploadItem
                    {
                        UploadId = context.Message.UploadId,
                        ItemId = entityId,
                        Success = true
                    };
                    var postUploadItemResponse = await client.PostAsync(uploadItemsApiEndpoint
                        , new StringContent(JsonConvert.SerializeObject(postUploadItemObject), Encoding.UTF8, "application/json"));
                }
                else if (context.Message.UploadId > 0 && entityId <= 0)
                {
                    var postUploadItemObject = new UploadItem
                    {
                        UploadId = context.Message.UploadId,
                        ItemId = entityId,
                        Success = false
                    };
                    var postUploadItemResponse = await client.PostAsync(uploadItemsApiEndpoint
                        , new StringContent(JsonConvert.SerializeObject(postUploadItemObject), Encoding.UTF8, "application/json"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
