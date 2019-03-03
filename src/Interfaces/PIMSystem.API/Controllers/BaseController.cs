using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private const string OffsetPattern = @"Offset=";

        public void PreparePagination<T>(long offset, long limit, int total, string resource, BasePagedResponse<List<T>> response)
        {
            response.Index = offset;
            response.Total = total;
            response.PageSize = limit;

            string queryString = string.Empty;
            string baseQuery = $"api/{resource}";

            if (HttpContext.Request.QueryString.HasValue)
            {
                queryString = HttpContext.Request.QueryString.Value;
            }

            if (!queryString.Contains(OffsetPattern))
            {
                queryString += "&" + OffsetPattern + "0";
            }

            long tmpNextOffset = (offset + limit);
            if (tmpNextOffset < total)
            {
                string nextPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpNextOffset}", StringComparison.InvariantCultureIgnoreCase);
                response.Next = $"{baseQuery}{nextPage}";
            }

            long tmpPrevOffset = (offset - limit);
            if (tmpPrevOffset >= 0)
            {
                string prevPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpPrevOffset}", StringComparison.InvariantCultureIgnoreCase);
                response.Prev = $"{baseQuery}{prevPage}";
            }

            long tmpTotalPageOffset = (total - limit);
            if (tmpTotalPageOffset > 0)
            {
                string lastPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpTotalPageOffset}", StringComparison.InvariantCultureIgnoreCase);
                response.Last = $"{baseQuery}{lastPage}";
            }

            string tmpFirstPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}0", StringComparison.InvariantCultureIgnoreCase);
            response.First = $"{baseQuery}{tmpFirstPage}";
        }
    }
}