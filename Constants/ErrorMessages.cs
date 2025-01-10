namespace repassAPI.Constants;

public static class ErrorMessages
{
    public const string RequiredField = "Field is required";
    
    //Incorrect data
    public const string IncorrectConfirmPassword = "Passwords must be identical";
    public const string IncorrectDate = "Incorrect birthdate";
    
    public const string InvalidDate = "User must be over 16 years old";
    
    //Incorrect data format
    public const string IncorrectPhoneFormat = "Incorrect phone format";
    public const string IncorrectEmailFormat = "Incorrect email format";
    public const string IncorrectPasswordFormat = "Password must be longer than 6 characters and have at least one number";

    //Login failed errors
    public const string ConflictEmail = "Email is already registered";
    public const string InvalidPassword = "Invalid password";
    
    //Token errors
    public const string InvalidToken = "Invalid token";
    
    //Not found errors
    public const string UserNotFound = "User with this email not found";
    
    //Success
    public const string Logout = "Logged Out";
}