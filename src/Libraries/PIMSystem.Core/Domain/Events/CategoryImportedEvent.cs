namespace PIMSystem.Core.Domain.Events
{
    public class CategoryImportedEvent
    {
        public int UploadId { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}