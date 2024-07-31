using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementOfChangeController : ControllerBase
    {
        private readonly MainDbContext _context;
		public ManagementOfChangeController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ManagementOfChange>>> GetManagementOfChange()
		{
			return await _context.ManagementOfChange.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ManagementOfChange>> GetManagementOfChange(int id)
		{
			var data = await _context.ManagementOfChange.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutManagementOfChange(int id, ManagementOfChange data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			data.updated_date = DateTime.Now;

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ManagementOfChangeExists(id))
				{
					return NotFound($"ManagementOfChange with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<ManagementOfChange>> PostManagementOfChange(ManagementOfChange data)
		{
			data.created_date = DateTime.Now;
			_context.ManagementOfChange.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetManagementOfChange", new { id = data.id }, data);
		}
		
		[HttpDelete]
        [Route("delete-management-of-change")]
        public async Task<IActionResult> DeleteManagementOfChange(int id)
        {
            var data = await _context.ManagementOfChange.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            data.is_active = false;
            data.updated_date = DateTime.Now;
            _context.Entry(data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagementOfChangeExists(id))
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

		private bool ManagementOfChangeExists(int id)
		{
			return _context.ManagementOfChange.Any(e => e.id == id);
		}
    }
}