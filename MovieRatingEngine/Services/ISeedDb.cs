using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface ISeedDb
    {
        Task<string> Generate();
    }
}
