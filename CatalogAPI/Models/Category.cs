using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models
{


    //[Table("categories")]
    public class Category : BaseModel
    {


        //[Column("description")]
        //public string? Description { get; set; }

        //[JsonIgnore]
        //public List<Product>? Products { get; set; }


    }
}
