using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Response;

public class UserShortResponse(string id, string fullName)
{
    public string Id { get; init; } = id;
    public string FullName { get; set; } = fullName;
}