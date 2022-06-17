using MovieRatingEngine.Data;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class SeedDbService : ISeedDb
    {
        private readonly MovieContext _db;
        private readonly IAuthService _authService;

        public SeedDbService(MovieContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public async Task<string> Generate()
        {
            if (! await _db.Database.CanConnectAsync())
               return "Database is not created. Enter update-database command in Package Manager Console.";
            
            var login = await SeedDataToMovieContext.Generate(_db, _authService);
            return "Database is created and filled with test data. " + login;
        }

    }
}
