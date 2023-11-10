namespace CatalogAPI.Models
{
    public class Category : BaseModel
    {


        public string? Description { get; set; }

        public List<Product>? Products { get; set; }


    }
}
