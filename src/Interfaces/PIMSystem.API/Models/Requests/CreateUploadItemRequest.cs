namespace PIMSystem.API.Models.Requests
{
    public class CreateUploadItemRequest
    {
        public int UploadId { get; set; }
        public int ItemId { get; set; }
        public bool Success { get; set; }
    }
}