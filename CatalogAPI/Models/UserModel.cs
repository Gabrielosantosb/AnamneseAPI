using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Models;

[Table("users")]
public class UserModel : BaseModel
{
    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }
    public virtual List<PacientModel> Patients { get; set; } = new List<PacientModel>();
    
}
