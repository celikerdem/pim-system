using System;
using System.Threading.Tasks;
using MassTransit;
using Newtonsoft.Json;
using PIMSystem.Core.Service.Event;
using Polly;

namespace PIMSystem.Service.Event
{
    public class RabbitMqService : IMqService
    {
        private readonly IBusControl _busControl;

        public RabbitMqService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task PublishEvent(object contract)
        {
            var maxRetryCount = 5;
            await Policy.Handle<Exception>()
            .WaitAndRetry(
                maxRetryCount,
                retryAttempt => TimeSpan.FromSeconds(5)
            )
            .Execute(async () =>
                {
                    await _busControl.Publish(contract);
                });
        }
    }
}