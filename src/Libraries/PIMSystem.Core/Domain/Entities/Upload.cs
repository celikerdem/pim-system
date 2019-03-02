namespace PIMSystem.Core.Domain.Entities
{
    public class Upload : BaseEntity
    {
        public string FileName { get; set; }
        public string TableName { get; set; }
        public int ItemCount { get; set; }
    }
}