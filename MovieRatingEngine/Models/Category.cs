using System.Text.Json.Serialization;

namespace MovieRatingEngine.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        Movie,
        TvShow
    }
}
