namespace PIMSystem.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ZamroId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public int CategoryId { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool Available { get; set; }
    }
}