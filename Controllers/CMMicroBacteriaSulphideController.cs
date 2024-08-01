using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CMMicroBacteriaSulphideController : ControllerBase
    {
        private readonly MainDbContext _context;

        public CMMicroBacteriaSulphideController(MainDbContext context)
        {
            _context = context;
        }

        // GET: api/CMMicroBacteriaSulphide
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMMicroBacteriaSulphide>>> GetCMMicroBacteriaSulphide()
        {
            return await _context.CMMicroBacteriaSulphide.ToListAsync();
        }

        // GET: api/CMMicroBacteriaSulphide/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CMMicroBacteriaSulphide>> GetCMMicroBacteriaSulphide(int id)
        {
            var data = await _context.CMMicroBacteriaSulphide.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        // GET: api/CMMicroBacteriaSulphide/ByTag/5
        [HttpGet("ByTag/{id_tag}")]
        public async Task<ActionResult<IEnumerable<CMMicroBacteriaSulphide>>> GetCMMicroBacteriaSulphideByTag(int id_tag)
        {
            return await _context.CMMicroBacteriaSulphide.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
        }

        // POST: api/CMMicroBacteriaSulphide
        [HttpPost]
        public async Task<ActionResult<CMMicroBacteriaSulphide>> PostCMMicroBacteriaSulphide(CMMicroBacteriaSulphide data)
        {
            _context.CMMicroBacteriaSulphide.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCMMicroBacteriaSulphide", new { id = data.id }, data);
        }

        // PUT: api/CMMicroBacteriaSulphide/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCMMicroBacteriaSulphide(int id, CMMicroBacteriaSulphide data)
        {
            if (id != data.id)
            {
                return BadRequest();
            }

            _context.Entry(data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CMMicroBacteriaSulphideExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/CMMicroBacteriaSulphide/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCMMicroBacteriaSulphide(int id)
        {
            var data = await _context.CMMicroBacteriaSulphide.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            _context.CMMicroBacteriaSulphide.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CMMicroBacteriaSulphideExists(int id)
        {
            return _context.CMMicroBacteriaSulphide.Any(e => e.id == id);
        }
    }
}