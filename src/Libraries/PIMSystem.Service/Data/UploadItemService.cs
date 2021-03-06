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
    public class UploadItemService : IUploadItemService
    {
        private readonly IRepository<UploadItem> _repository;

        public UploadItemService(IRepository<UploadItem> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<UploadItem>> GetUploadItemAsync(int id)
        {
            var response = new BaseResponse<UploadItem>();

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

        public async Task<BasePagedResponse<List<UploadItem>>> GetUploadItemsAsync(UploadItemPagedRequest request)
        {
            var response = new BasePagedResponse<List<UploadItem>>();

            try
            {
                var query = await _repository.GetAll();
                if (request.UploadId > 0)
                    query = query.Where(x => x.UploadId == request.UploadId);
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

        public async Task<BaseResponse<bool>> CreateUploadItemAsync(UploadItem request)
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

        public async Task<BaseResponse<bool>> UpdateUploadItemAsync(UploadItem request)
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

        public async Task<BaseResponse<bool>> DeleteUploadItemAsync(int id)
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