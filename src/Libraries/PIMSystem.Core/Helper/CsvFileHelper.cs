using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using PIMSystem.Core.Domain.Responses;

namespace PIMSystem.Core.Helper
{
    public static class CsvFileHelper
    {
        public static CsvReadResponse<T> ReadFromStream<T>(Stream stream)
        {
            var response = new CsvReadResponse<T>();

            var config = new Configuration();
            config.ShouldSkipRecord = (rec) =>
            {
                foreach (var cell in rec)
                {
                    if (!string.IsNullOrEmpty(cell.Trim()))
                        return false;
                }
                return true;
            };
            config.ReadingExceptionOccurred = (ex) =>
            {
                response.FailedRows.Add(ex.ReadingContext.RawRecord);
                return false;
            };
            config.BadDataFound = (context) =>
            {
                response.FailedRows.Add(context.RawRecord);
            };

            TextReader reader = new StreamReader(stream);
            var csvReader = new CsvReader(reader, config);
            response.Data = csvReader.GetRecords<T>().ToList();

            return response;
        }
    }
}