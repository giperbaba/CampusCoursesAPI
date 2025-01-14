using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace repassAPI.Entities;

[Table("banned_tokens")]
public class AccessToken(string token)
{
    [Key]
    [Column("id")]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("token")]
    public string Token { get; set; } = token;
}