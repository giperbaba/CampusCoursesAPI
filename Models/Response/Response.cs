namespace repassAPI.Models.Response;

public class Response(string? status, string message)
{
    public string? Status { get; set; } = status;
    public string Message { get; set; } = message;
}