using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models
{

    [Table ("products")]
    public class Product : BaseModel
    {
        [Column ("description")]
        public string? Description { get; set; }
        
        [Column("price")]
        public decimal? Price { get; set; }
        [Column("image")]
        public string? Image { get; set; }

        [Column("purchasedate")]
        public DateTime? PurchaseDate { get; set; }
        
        [Column("inventory")]
        public string? Inventory { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

    }
}
