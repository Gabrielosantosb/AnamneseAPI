namespace CatalogAPI.Models
{
    public class Product : BaseModel
    {
        
        //public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Iventory { get; set; }


        public Category? Category { get; set; }

    }
}
