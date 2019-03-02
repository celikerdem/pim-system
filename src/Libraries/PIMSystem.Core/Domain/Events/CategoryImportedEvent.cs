namespace PIMSystem.Core.Domain.Events
{
    public class CategoryImportedEvent
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}