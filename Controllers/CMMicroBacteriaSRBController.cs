using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMMicroBacteriaSRBController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMMicroBacteriaSRBController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMMicroBacteriaSRB
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaSRB>>> GetCMMicroBacteriaSRB()
		{
			return await _context.CMMicroBacteriaSRB.ToListAsync();
		}

		// GET: api/CMMicroBacteriaSRB/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMMicroBacteriaSRB>> GetCMMicroBacteriaSRB(int id)
		{
			var data = await _context.CMMicroBacteriaSRB.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMMicroBacteriaSRB/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaSRB>>> GetCMMicroBacteriaSRBByTag(int id_tag)
		{
			return await _context.CMMicroBacteriaSRB.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		// POST: api/CMMicroBacteriaSRB
		[HttpPost]
		public async Task<ActionResult<CMMicroBacteriaSRB>> PostCMMicroBacteriaSRB(CMMicroBacteriaSRB data)
		{
			if (data.srb_val != null)
			{
				if (data.srb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if (data.srb_val > (decimal)0.3 && data.srb_val <= 100)
				{
					data.id_status = 2;
				}
				else if (data.srb_val > 100)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}

			_context.CMMicroBacteriaSRB.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMMicroBacteriaSRB", new { id = data.id }, data);
		}

		// PUT: api/CMMicroBacteriaSRB/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMMicroBacteriaSRB(int id, CMMicroBacteriaSRB data)
		{
			if (id != data.id)
			{
				return BadRequest();
			}

			if (data.srb_val != null)
			{
				if (data.srb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if (data.srb_val > (decimal)0.3 && data.srb_val <= 100)
				{
					data.id_status = 2;
				}
				else if (data.srb_val > 100)
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
				if (!CMMicroBacteriaSRBExists(id))
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

		// DELETE: api/CMMicroBacteriaSRB/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMMicroBacteriaSRB(int id)
		{
			var data = await _context.CMMicroBacteriaSRB.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			_context.CMMicroBacteriaSRB.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMMicroBacteriaSRBExists(int id)
		{
			return _context.CMMicroBacteriaSRB.Any(e => e.id == id);
		}
	}
}