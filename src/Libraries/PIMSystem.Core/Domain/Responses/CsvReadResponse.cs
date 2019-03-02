using System.Collections.Generic;

namespace PIMSystem.Core.Domain.Responses
{
    public class CsvReadResponse<T>
    {
        public CsvReadResponse()
        {
            FailedRows = new List<string>();
        }

        public List<T> Data { get; set; }
        public List<string> FailedRows { get; set; }
        public int ItemCount { get; set; }
    }
}