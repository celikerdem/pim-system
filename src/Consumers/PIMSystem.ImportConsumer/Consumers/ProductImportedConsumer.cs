using MassTransit;
using Newtonsoft.Json;
using PIMSystem.Core.Domain.Events;
using System;
using System.Net.Http;
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
                string apiUrl = "http://localhost:5000/api/categories";

                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = await client.PostAsync(apiUrl, null);
                if (httpResponse.IsSuccessStatusCode)
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
