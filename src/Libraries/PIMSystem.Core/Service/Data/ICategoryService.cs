using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.Core.Service.Data
{
    public interface ICategoryService
    {
        Task<BaseResponse<Category>> GetCategoryAsync(int id);
        Task<BasePagedResponse<List<Category>>> GetCategoriesAsync(BasePagedRequest request);
        Task<BaseResponse<bool>> CreateCategoryAsync(Category request);
        Task<BaseResponse<bool>> UpdateCategoryAsync(Category request);
        Task<BaseResponse<bool>> DeleteCategoryAsync(int id);
    }
}