using LoggingApi.Database;
using LoggingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoggingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataRecordsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveData([FromBody] List<RequestModel> data)
        {
            _context.DataRecords.RemoveRange(_context.DataRecords);
            await _context.SaveChangesAsync();

            var dataRecords = data.Select(rm => new DataRecord
            {
                Code = rm.Code,
                Value = rm.Value
            }).OrderBy(x => x.Code).ToList();

            _context.DataRecords.AddRange(dataRecords);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataRecord>>> GetData([FromQuery] int? code)
        {
            var query = _context.DataRecords.AsQueryable();

            if (code.HasValue)
            {
                query = query.Where(dr => dr.Code == code.Value);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }
    }
}
