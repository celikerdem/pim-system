using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PIMSystem.Core.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Domain.Requests;
using PIMSystem.Core.Domain.Responses;
using PIMSystem.Core.Service.Data;

namespace PIMSystem.Service.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<bool>> CreateCategoryAsync(Category request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                await _repository.CreateAsync(request);
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.Data = false;
            }

            return response;
        }

        public Task<BaseResponse<bool>> DeleteCategoryAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<List<Category>>> GetCategoriesAsync(BasePagedRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<Category>> GetCategoryAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateCategoryAsync(Category request)
        {
            throw new System.NotImplementedException();
        }
    }
}