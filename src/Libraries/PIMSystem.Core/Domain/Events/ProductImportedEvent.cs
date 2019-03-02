namespace PIMSystem.Core.Domain.Events
{
    public class ProductImportedEvent
    {
        public string ZamroID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public int CategoryID { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool Available { get; set; }
    }
}