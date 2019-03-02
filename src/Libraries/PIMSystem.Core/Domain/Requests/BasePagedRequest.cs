namespace PIMSystem.Core.Domain.Requests
{
    public class BasePagedRequest
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
