using System.Text.Json.Serialization;

namespace CatalogAPI.Models
{
    public class Category : BaseModel
    {


        public string? Description { get; set; }

        [JsonIgnore]
        public List<Product>? Products { get; set; }


    }
}
