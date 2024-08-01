using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HighlightActivitiesController : ControllerBase
	{
		private readonly MainDbContext _context;
		public HighlightActivitiesController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<HighlightActivities>>> GetHighlightActivities()
		{
			return await _context.HighlightActivities.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<HighlightActivities>> GetHighlightActivities(int id)
		{
			var data = await _context.HighlightActivities.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutHighlightActivities(int id, HighlightActivities data)
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
				if (!HighlightActivitiesExists(id))
				{
					return NotFound($"HighlightActivities with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<HighlightActivities>> PostHighlightActivities(HighlightActivities data)
		{
			data.ha_number = GenReportNo();
			data.created_date = DateTime.Now;
			_context.HighlightActivities.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetHighlightActivities", new { id = data.id }, data);
		}
		
		[HttpDelete]
        [Route("delete-highlight-activities")]
        public async Task<IActionResult> DeleteHighlightActivities(int id)
        {
            var data = await _context.HighlightActivities.FindAsync(id);

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
                if (!HighlightActivitiesExists(id))
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

		private string GenReportNo()
		{
			string report_no = "";
			var data = _context.HighlightActivities.Where(a => a.ha_number != null).OrderByDescending(a => a.ha_number).FirstOrDefault();

			if (data == null || (data?.ha_number?.StartsWith("HA") != true))
			{
				report_no = "HA-00001";
			}
			else
			{
				int tmp_no = Convert.ToInt32(data.ha_number.Substring(data.ha_number.Length - 5)) + 1;

				report_no = "HA-" + tmp_no.ToString("D5");
			}

			return report_no;
		}

		private bool HighlightActivitiesExists(int id)
		{
			return _context.HighlightActivities.Any(e => e.id == id);
		}
	}
}