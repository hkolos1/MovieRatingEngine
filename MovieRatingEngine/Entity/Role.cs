using System.Text.Json.Serialization;

namespace MovieRatingEngine.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum Role
    {
        Admin,
        User
    }
}
