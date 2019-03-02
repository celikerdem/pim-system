using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.Core.Service.Data
{
    public interface IUploadItemService
    {
        Task<BaseResponse<UploadItem>> GetUploadItemAsync(int id);
        Task<BaseResponse<List<UploadItem>>> GetUploadItemsAsync(BasePagedRequest request);
        Task<BaseResponse<bool>> CreateUploadItemAsync(UploadItem request);
        Task<BaseResponse<bool>> UpdateUploadItemAsync(UploadItem request);
        Task<BaseResponse<bool>> DeleteUploadItemAsync(int id);
    }
}