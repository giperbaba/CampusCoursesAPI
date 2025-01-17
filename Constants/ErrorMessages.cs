namespace repassAPI.Constants;

public static class ErrorMessages
{
    public const string RequiredField = "Field is required";
    
    //Incorrect data
    public const string IncorrectConfirmPassword = "Passwords must be identical";
    public const string IncorrectDate = "Incorrect birthdate";
    
    //Incorrect data format
    public const string IncorrectEmailFormat = "Incorrect email format";
    public const string IncorrectPasswordFormat = "Password must be longer than 6 characters and have at least one number";

    //Invalid data
    public const string InvalidDate = "User must be over 16 years old";
    
    public const string InvalidStartCoursesYear = "StartYear must be between 2000 and 2029.";
    public const string InvalidStudentsAmount = "MaxStudentsCount must be between 1 and 200.";

    public const string InvalidCourseStatus = "Course status cannot be changed to a previous one";
    public const string InvalidCourseStatusInRequest = "Campus course is not open for signing up";

    public const string AlreadyIsTeacher = "User is already a teacher for this course.";
    public const string AlreadyIsStudent = "User is already a student for this course.";
    
    //Login failed errors
    public const string ConflictEmail = "Email is already registered";
    public const string InvalidPassword = "Invalid password";
    
    //Token errors
    public const string InvalidToken = "Invalid token";
    
    //Not found errors
    public const string UserNotFound = "User not found";
    public const string GroupNotFound = "Group not found";
    public const string CourseNotFound = "Course not found";
    
    //Forbidden errors
    public const string AccessDenied = "You must be admin";
    public const string AccessDeniedAdminMainTeacher = "You must be admin or main teacher";
    public const string AccessDeniedAdminTeacher = "You must be admin or teacher";
    //Success
    public const string Logout = "Logged Out";
    
    //Conflict
    public const string ConflictStudentsCount = "There are already more students in the course than the new maximum number";
    public const string ConflictCourseRemainingSlots = "There are no more places on the course";
    public const string ConflictStudentIsNotInTheCourse = "Student not found in the course.";
    public const string ConflictStudentIsNotInTheQueue = "The student is not in the course queue.";
}