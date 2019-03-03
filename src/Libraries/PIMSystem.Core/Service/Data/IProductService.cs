using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.Core.Service.Data
{
    public interface IProductService
    {
        Task<BaseResponse<Product>> GetProductAsync(int id);
        Task<BasePagedResponse<List<Product>>> GetProductsAsync(BasePagedRequest request);
        Task<BaseResponse<bool>> CreateProductAsync(Product request);
        Task<BaseResponse<bool>> UpdateProductAsync(Product request);
        Task<BaseResponse<bool>> DeleteProductAsync(int id);
    }
}