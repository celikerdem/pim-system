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
    public class UploadService : IUploadService
    {
        private readonly IRepository<Upload> _repository;

        public UploadService(IRepository<Upload> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<Upload>> GetUploadAsync(int id)
        {
            var response = new BaseResponse<Upload>();

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

        public async Task<BasePagedResponse<List<Upload>>> GetUploadsAsync(BasePagedRequest request)
        {
            var response = new BasePagedResponse<List<Upload>>();

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

        public async Task<BaseResponse<bool>> CreateUploadAsync(Upload request)
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

        public async Task<BaseResponse<bool>> UpdateUploadAsync(Upload request)
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

        public async Task<BaseResponse<bool>> DeleteUploadAsync(int id)
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