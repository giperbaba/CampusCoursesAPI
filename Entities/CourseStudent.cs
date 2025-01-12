using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using repassAPI.Models.Enums;

namespace repassAPI.Entities;

[Table("course_student")]
public class CourseStudent
{
    public CourseStudent(Guid courseId, Guid studentId, StudentStatus status)
    {
        CourseId = courseId;
        StudentId = studentId;
        Status = status;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }
    
    [Column("student_status")]
    public StudentStatus Status { get; set; }

    //навигационные свойства
    [ForeignKey("CourseId")]
    public Course Course { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; }
}