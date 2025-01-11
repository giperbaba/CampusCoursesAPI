namespace repassAPI.Models.Response;

public class CampusGroupResponse(string id, string name)
{
    public string Id { get; init; } = id;
    public string Name { get; set; } = name;
}