namespace PIMSystem.Core.Domain.Entities
{
    public class UploadItem : BaseEntity
    {
        public int UploadId { get; set; }
        public int ItemId { get; set; }
        public bool Success { get; set; }
    }
}