using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.Core.Service.Data
{
    public interface IUploadService
    {
        Task<BaseResponse<Upload>> GetUploadAsync(int id);
        Task<BasePagedResponse<List<Upload>>> GetUploadsAsync(BasePagedRequest request);
        Task<BaseResponse<bool>> CreateUploadAsync(Upload request);
        Task<BaseResponse<bool>> UpdateUploadAsync(Upload request);
        Task<BaseResponse<bool>> DeleteUploadAsync(int id);
    }
}