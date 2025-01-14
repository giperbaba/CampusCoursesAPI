using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace repassAPI.Entities;

[Table("notifications")]
public class Notification
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("course_id")]
    public Guid CourseId { get; set; }
    
    [Column("text")]
    public string Text { get; set; }
    
    [Column("is_important")]
    public bool IsImportant { get; set; }
    
    //навигационные свойства
    [ForeignKey("CourseId")]
    public Course Course { get; set; }
}