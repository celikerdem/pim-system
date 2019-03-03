using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BaseResponse<Category>> GetCategoryAsync(int id)
        {
            var response = new BaseResponse<Category>();

            try
            {
                var entity = await _repository.GetByIdAsync(id);
                response.Data = entity;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.Data = null;
            }

            return response;
        }

        public async Task<BasePagedResponse<List<Category>>> GetCategoriesAsync(BasePagedRequest request)
        {
            var response = new BasePagedResponse<List<Category>>();

            try
            {
                var query = await _repository.GetAll();
                var entityList = query.Skip(request.Offset)
                                      .Take(request.Limit)
                                      .ToList();
                response.Total = query.Count();
                response.Index = request.Offset / request.Limit;
                response.PageSize = request.Limit;
                response.Items = entityList;
            }
            catch (Exception ex)
            {
                response.Items = null;
            }

            return response;
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

        public async Task<BaseResponse<bool>> UpdateCategoryAsync(Category request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                await _repository.UpdateAsync(request.Id, request);
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.Data = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                await _repository.DeleteAsync(id);
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.Data = false;
            }

            return response;
        }
    }
}