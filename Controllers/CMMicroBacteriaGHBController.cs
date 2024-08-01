using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMMicroBacteriaGHBController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMMicroBacteriaGHBController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMMicroBacteriaGHB
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaGHB>>> GetCMMicroBacteriaGHB()
		{
			return await _context.CMMicroBacteriaGHB.ToListAsync();
		}

		// GET: api/CMMicroBacteriaGHB/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMMicroBacteriaGHB>> GetCMMicroBacteriaGHB(int id)
		{
			var data = await _context.CMMicroBacteriaGHB.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMMicroBacteriaGHB/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaGHB>>> GetCMMicroBacteriaGHBByTag(int id_tag)
		{
			return await _context.CMMicroBacteriaGHB.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		// POST: api/CMMicroBacteriaGHB
		[HttpPost]
		public async Task<ActionResult<CMMicroBacteriaGHB>> PostCMMicroBacteriaGHB(CMMicroBacteriaGHB data)
		{
			if (data.ghb_val != null)
			{
				if(data.ghb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if(data.ghb_val > (decimal)0.3 && data.ghb_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.ghb_val > 100)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}
			
			_context.CMMicroBacteriaGHB.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMMicroBacteriaGHB", new { id = data.id }, data);
		}

		// PUT: api/CMMicroBacteriaGHB/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMMicroBacteriaGHB(int id, CMMicroBacteriaGHB data)
		{
			if (id != data.id)
			{
				return BadRequest();
			}
			
			if (data.ghb_val != null)
			{
				if(data.ghb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if(data.ghb_val > (decimal)0.3 && data.ghb_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.ghb_val > 100)
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
				if (!CMMicroBacteriaGHBExists(id))
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

		// DELETE: api/CMMicroBacteriaGHB/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMMicroBacteriaGHB(int id)
		{
			var data = await _context.CMMicroBacteriaGHB.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			_context.CMMicroBacteriaGHB.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMMicroBacteriaGHBExists(int id)
		{
			return _context.CMMicroBacteriaGHB.Any(e => e.id == id);
		}
	}
}