using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Services;
using System.Threading.Tasks;

namespace MovieRatingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDatabaseController : ControllerBase
    {
        private readonly ISeedDb _seedDb;

        public SeedDatabaseController(ISeedDb seedDb)
        {
            _seedDb = seedDb;
        }
        [HttpGet()]
        public async Task<IActionResult> SeedDatabase()
        {
             var r=await _seedDb.Generate();
            return Ok(r);
        }

    }
}
