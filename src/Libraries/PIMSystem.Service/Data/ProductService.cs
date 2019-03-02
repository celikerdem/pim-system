using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;
using PIMSystem.Core.Service.Data;

namespace PIMSystem.Service.Data
{
    public class ProductService : IProductService
    {
        public Task<BaseResponse<bool>> CreateProductAsync(Product request)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<bool>> DeleteProductAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<Product>> GetProductAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<List<Product>>> GetProductsAsync(BasePagedRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateProductAsync(Product request)
        {
            throw new System.NotImplementedException();
        }
    }
}