using System.ComponentModel.DataAnnotations;

namespace repassAPI.Utils;

public class DatetimeValidator: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (DateTime.TryParse(value?.ToString(), out var parsedDateTime))
        {
            return parsedDateTime < DateTime.Now;
        }
        
        return false;
    }
}