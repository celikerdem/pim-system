using System.Threading.Tasks;

namespace PIMSystem.Core.Service.Event
{
    public interface IMqService
    {
        Task PublishEvent(object contract);
    }
}