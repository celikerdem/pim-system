namespace PIMSystem.API.Models.Responses
{
    public class FilterDatatableResponse<TData>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public TData Data { get; set; }
        public string Error { get; set; }
    }
}