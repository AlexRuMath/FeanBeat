using LoggingApi.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoggingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestLogController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogModel>>> GetData()
        {
            var result = await _context.Logs.ToListAsync();
            return Ok(result);
        }
    }
}
