using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
    public Guid Id { get; set; }
    
    [Column("full_name")]
    [Required]
    [StringLength(255)]
    public string FullName { get; set; } 
    
    [Column("birth_date")]
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Column("email")]
    [Required]
    [EmailAddress(ErrorMessage = Constants.ErrorMessages.IncorrectEmailFormat)]
    public string Email { get; init; }
    
    [Column("password")]
    [Required]
    [StringLength(255, ErrorMessage = Constants.ErrorMessages.IncorrectPasswordFormat, MinimumLength = 6)]
    public string Password { get; set; }
    
    [Column("is_admin")]
    [Required]
    public bool IsAdmin { get; set; }
    
    //навигационные свойства
    public ICollection<CourseStudent> StudingCourses { get; set; } = new List<CourseStudent>();
    public ICollection<CourseTeacher> TeachingCourses { get; set; } = new List<CourseTeacher>();
}