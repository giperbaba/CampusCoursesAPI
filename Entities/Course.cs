using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Entities;

[Table("courses")]
public class Course(
    string? name,
    int startYear,
    int maxStudentsCount,
    int remainingSlotsCount,
    CourseStatus status,
    Semester semester,
    string requirements,
    string annotations,
    DateTime createTime,
    Guid campusGroupId,
    Guid? mainTeacherId)
{
    [Key]
    [Column("id")]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    [Column("name")]  
    public string? Name { get; set; } = name;

    [Column("start_year")]  
    public int StartYear { get; set; } = startYear;

    [Column("max_students_count")] 
    [Range(1, 200, ErrorMessage = ErrorMessages.InvalidStudentsAmount)]
    public int MaxStudentsCount { get; set; } = maxStudentsCount;

    [Column("remaining_slots_count")]  
    [Range(1, 200, ErrorMessage = ErrorMessages.InvalidStudentsAmount)]
    public int RemainingSlotsCount { get; set; } = remainingSlotsCount;

    [Column("status")] 
    [EnumDataType(typeof(CourseStatus))]
    public CourseStatus Status { get; set; } = status;

    [Column("semester")]
    [EnumDataType(typeof(Semester))]
    public Semester Semester { get; set; } = semester;
    
    [Column("requirements")]
    public string Requirements { get; set; } = requirements;

    [Column("annotations")]
    public string Annotations { get; set; } = annotations;

    [Column("group_id")]
    public Guid CampusGroupId { get; set; } = campusGroupId;
    
    [Column("main_teacher_id")]
    public Guid? MainTeacherId { get; set; } = mainTeacherId;
    
    [Column("create_time")] 
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    //навигационные свойства 
    [ForeignKey("CampusGroupId")]
    public CampusGroup CampusGroup { get; set; }
    
    [ForeignKey("MainTeacherId")]
    public User? MainTeacher { get; set; }
    
    public ICollection<CourseStudent> Students { get; set; } = new List<CourseStudent>();
    public ICollection<CourseTeacher> Teachers { get; set; } = new List<CourseTeacher>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    
    [Range(1, 200, ErrorMessage = ErrorMessages.InvalidStudentsAmount)]
    public int StudentsEnrolledCount => Students.Count(s => s.Status == StudentStatus.Accepted);
    
    [Range(1, 200, ErrorMessage = ErrorMessages.InvalidStudentsAmount)]
    public int StudentsInQueueCount => Students.Count(s => s.Status == StudentStatus.InQueue);
}