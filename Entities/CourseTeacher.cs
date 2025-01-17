using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace repassAPI.Entities;

[Table("course_teacher")]
public class CourseTeacher
{
    public CourseTeacher(Guid courseId, Guid teacherId, bool isMainTeacher)
    {
        CourseId = courseId;
        TeacherId = teacherId;
        IsMainTeacher = isMainTeacher;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("teacher_id")]
    public Guid TeacherId { get; set; }
    
    [Column("is_main_teacher")]
    public bool IsMainTeacher { get; set; }

    //навигационные свойства
    [ForeignKey("CourseId")]
    public Course Course { get; set; }

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; }
}