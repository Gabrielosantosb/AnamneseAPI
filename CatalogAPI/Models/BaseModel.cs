using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Models
{
    public class BaseModel
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }
        //public string? Description { get; set; }
    }
}
