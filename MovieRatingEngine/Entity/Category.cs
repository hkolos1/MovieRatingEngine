using System.Text.Json.Serialization;

namespace MovieRatingEngine.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        Movie,
        TvShow
    }
}
