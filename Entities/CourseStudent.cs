using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using repassAPI.Models.Enums;

namespace repassAPI.Entities;

[Table("course_student")]
public class CourseStudent
{
    public CourseStudent(Guid courseId, Guid studentId)
    {
        CourseId = courseId;
        StudentId = studentId;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("student_status")] public StudentStatus Status { get; set; } = StudentStatus.InQueue;

    [Column("midterm_result")] public StudentMark? MidtermResult { get; set; } = null;

    [Column("final_result")] public StudentMark? FinalResult { get; set; } = null;

    //навигационные свойства
    [ForeignKey("CourseId")]
    public Course Course { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; }
}