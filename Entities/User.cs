using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace repassAPI.Entities;

[Table("users")]
public class User
{
    public User(string fullName, DateTime birthDate, string email, string password, bool isAdmin = false)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }

    [Key]
    [Column("id")]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    [Column("full_name")]
    [Required]
    [StringLength(255)]
    public string FullName { get; set; } 
    
    [Column("birth_date")]
    [Required]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BirthDate { get; set; }
    
    [Column("email")]
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    
    [Column("password")]
    [Required]
    [StringLength(255, ErrorMessage = Constants.ErrorMessages.IncorrectPasswordFormat, MinimumLength = 6)]
    public string Password { get; init; }
    
    [Column("isAdmin")]
    [Required]
    public bool IsAdmin { get; set; }
    
}