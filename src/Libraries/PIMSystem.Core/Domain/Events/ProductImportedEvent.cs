namespace PIMSystem.Core.Domain.Events
{
    public class ProductImportedEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public int CategoryId { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool Available { get; set; }
    }
}