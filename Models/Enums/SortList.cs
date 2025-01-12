using System.Text.Json.Serialization;

namespace repassAPI.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortList
{
    CreatedAsc,
    CreatedDesc
}