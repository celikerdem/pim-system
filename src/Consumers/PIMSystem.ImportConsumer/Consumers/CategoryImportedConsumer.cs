using MassTransit;
using Microsoft.Extensions.Configuration;
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
    public class CategoryImportedConsumer : IConsumer<CategoryImportedEvent>
    {
        public CategoryImportedConsumer()
        {
        }

        public async Task Consume(ConsumeContext<CategoryImportedEvent> context)
        {
            try
            {
                // var pimApiBaseUrl = "http://localhost:5000/api";
                var pimApiBaseUrl = "https://pim-system-api.azurewebsites.net/api";
                var categoriesApiEndpoint = pimApiBaseUrl + "/categories";
                var uploadItemsApiEndpoint = pimApiBaseUrl + "/upload-items";

                //Create entity
                var entityId = 0;
                var client = new HttpClient();
                var postCategoryObject = new Category
                {
                    CategoryId = context.Message.CategoryID,
                    Name = context.Message.Name
                };
                var postCategoryResponse = await client.PostAsync(categoriesApiEndpoint
                    , new StringContent(JsonConvert.SerializeObject(postCategoryObject), Encoding.UTF8, "application/json"));
                if (postCategoryResponse.StatusCode == HttpStatusCode.Created)
                {
                    var responseObject = JObject.Parse(await postCategoryResponse.Content.ReadAsStringAsync());
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
