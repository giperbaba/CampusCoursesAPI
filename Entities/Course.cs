using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
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
    Guid campusGroupId)
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
    public int MaxStudentsCount { get; set; } = maxStudentsCount;

    [Column("remaining_slots_count")]  
    public int RemainingSlotsCount { get; set; } = remainingSlotsCount;

    [Column("status")] 
    [EnumDataType(typeof(CourseStatus))]
    public CourseStatus Status { get; set; } = status;

    [Column("semester")]
    [EnumDataType(typeof(Semester))]
    public Semester Semester { get; set; } = semester;

    [Column("group_id")]
    public Guid CampusGroupId { get; set; } = campusGroupId;

    //навигационные свойства
    [ForeignKey("CampusGroupId")]
    public CampusGroup CampusGroup { get; set; }
    
    public ICollection<CourseStudent> Students { get; set; } = new List<CourseStudent>();
    public ICollection<CourseTeacher> Teachers { get; set; } = new List<CourseTeacher>();
}