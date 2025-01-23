using System.Text.Json;
using repassAPI.Data;
using repassAPI.Entities;

namespace repassAPI.Utils;

public static class DatabaseSeeder
{
    public static void SeedDatabase(DatabaseContext db)
    {
        if (!db.Users.Any())
        {
            var usersJson = File.ReadAllText("users.json");
            var users = JsonSerializer.Deserialize<List<User>>(usersJson);

            if (users != null)
            {
                foreach (var user in users)
                {
                    user.Id = Guid.NewGuid();
                    user.Password = PasswordHasher.Hash(user.Password);
                    db.Users.Add(user);
                
                    var userRole = new UserRoles(user.Id);
                    db.UserRoles.Add(userRole);
                
                    if (user.Email == "main@example.com")
                    {
                        userRole.IsTeacher = true;
                    }
                }
                db.SaveChanges();
            }
    }
    
    if (!db.CampusGroups.Any())
    {
        var groupsJson = File.ReadAllText("groups.json");
        var groups = JsonSerializer.Deserialize<List<CampusGroup>>(groupsJson);

        if (groups != null)
        {
            foreach (var group in groups)
            {
                db.CampusGroups.Add(group);
            }
            db.SaveChanges();
        }
    }
    
    if (!db.Courses.Any())
    {
        var coursesJson = File.ReadAllText("courses.json");
        var courses = JsonSerializer.Deserialize<List<Course>>(coursesJson);

        if (courses != null)
        {
            foreach (var course in courses)
            {
                var group = db.CampusGroups.FirstOrDefault(g => g.Id == course.CampusGroupId);
                if (group != null)
                {
                    course.CampusGroup = group;

                    var teacher = db.Users.FirstOrDefault(u => u.Email == "main@example.com");
                    if (teacher != null)
                    {
                        course.MainTeacherId = teacher.Id;
                    }

                    db.Courses.Add(course);
                }
            }
            db.SaveChanges();
        }
    }
    
    if (!db.CourseTeachers.Any())
    {
        var teachersJson = File.ReadAllText("teachers.json");
        var courseTeachers = JsonSerializer.Deserialize<List<CourseTeacher>>(teachersJson);

        if (courseTeachers != null)
        {
            foreach (var courseTeacher in courseTeachers)
            {
                var course = db.Courses.FirstOrDefault(c => c.Name == "health");
                var teacher = db.Users.FirstOrDefault(u => u.Email == "main@example.com");

                if (course != null && teacher != null)
                {
                    courseTeacher.CourseId = course.Id;
                    courseTeacher.TeacherId = teacher.Id;
                    courseTeacher.Course = course;
                    courseTeacher.Teacher = teacher;
                    db.CourseTeachers.Add(courseTeacher);
                }
            }
            db.SaveChanges();
        }
    }
    }
}