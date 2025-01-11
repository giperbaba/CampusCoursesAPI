using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace repassAPI.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StudentStatus
{
    InQueue,
    Accepted,
    Declined
}