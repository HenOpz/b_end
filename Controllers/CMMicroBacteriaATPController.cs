using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMMicroBacteriaATPController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMMicroBacteriaATPController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMMicroBacteriaATP
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaATP>>> GetCMMicroBacteriaATP()
		{
			return await _context.CMMicroBacteriaATP.ToListAsync();
		}

		// GET: api/CMMicroBacteriaATP/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMMicroBacteriaATP>> GetCMMicroBacteriaATP(int id)
		{
			var data = await _context.CMMicroBacteriaATP.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMMicroBacteriaATP/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaATP>>> GetCMMicroBacteriaATPByTag(int id_tag)
		{
			return await _context.CMMicroBacteriaATP.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		// POST: api/CMMicroBacteriaATP
		[HttpPost]
		public async Task<ActionResult<CMMicroBacteriaATP>> PostCMMicroBacteriaATP(CMMicroBacteriaATP data)
		{
			if (data.atp_val != null)
			{
				if(data.atp_val <= 10)
				{
					data.id_status = 3;
				}
				else if(data.atp_val > 10 && data.atp_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.atp_val > 100)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}
			_context.CMMicroBacteriaATP.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMMicroBacteriaATP", new { id = data.id }, data);
		}

		// PUT: api/CMMicroBacteriaATP/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMMicroBacteriaATP(int id, CMMicroBacteriaATP data)
		{
			if (id != data.id)
			{
				return BadRequest();
			}
			
			if (data.atp_val != null)
			{
				if(data.atp_val <= 10)
				{
					data.id_status = 3;
				}
				else if(data.atp_val > 10 && data.atp_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.atp_val > 100)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}

			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMMicroBacteriaATPExists(id))
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

		// DELETE: api/CMMicroBacteriaATP/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMMicroBacteriaATP(int id)
		{
			var data = await _context.CMMicroBacteriaATP.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			_context.CMMicroBacteriaATP.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMMicroBacteriaATPExists(int id)
		{
			return _context.CMMicroBacteriaATP.Any(e => e.id == id);
		}
	}
}