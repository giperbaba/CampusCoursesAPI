using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace repassAPI.Entities;

[Table("user_roles")]
public class UserRoles
{
    public UserRoles(Guid userId)
    {
        UserId = userId;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    [Column("user_id")]
    public Guid UserId { get; init; }

    [Column("is_student")] public bool IsStudent { get; set; } = false;

    [Column("is_teacher")] public bool IsTeacher { get; set; } = false;
    
    //навигационные свойства
    [ForeignKey("UserId")]
    public User User { get; init; }
}