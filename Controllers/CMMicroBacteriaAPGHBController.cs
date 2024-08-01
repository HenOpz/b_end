using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMMicroBacteriaAPGHBController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMMicroBacteriaAPGHBController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMMicroBacteriaAPGHB
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaAPGHB>>> GetCMMicroBacteriaAPGHB()
		{
			return await _context.CMMicroBacteriaAPGHB.ToListAsync();
		}

		// GET: api/CMMicroBacteriaAPGHB/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMMicroBacteriaAPGHB>> GetCMMicroBacteriaAPGHB(int id)
		{
			var data = await _context.CMMicroBacteriaAPGHB.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMMicroBacteriaAPGHB/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMMicroBacteriaAPGHB>>> GetCMMicroBacteriaAPGHBByTag(int id_tag)
		{
			return await _context.CMMicroBacteriaAPGHB.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		// POST: api/CMMicroBacteriaAPGHB
		[HttpPost]
		public async Task<ActionResult<CMMicroBacteriaAPGHB>> PostCMMicroBacteriaAPGHB(CMMicroBacteriaAPGHB data)
		{
			if (data.apghb_val != null)
			{
				if(data.apghb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if(data.apghb_val > (decimal)0.3 && data.apghb_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.apghb_val > 100)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}
			
			_context.CMMicroBacteriaAPGHB.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMMicroBacteriaAPGHB", new { id = data.id }, data);
		}

		// PUT: api/CMMicroBacteriaAPGHB/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMMicroBacteriaAPGHB(int id, CMMicroBacteriaAPGHB data)
		{
			if (id != data.id)
			{
				return BadRequest();
			}
			
			if (data.apghb_val != null)
			{
				if(data.apghb_val <= (decimal)0.3)
				{
					data.id_status = 3;
				}
				else if(data.apghb_val > (decimal)0.3 && data.apghb_val <= 100)
				{
					data.id_status = 2;
				}
				else if(data.apghb_val > 100)
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
				if (!CMMicroBacteriaAPGHBExists(id))
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

		// DELETE: api/CMMicroBacteriaAPGHB/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMMicroBacteriaAPGHB(int id)
		{
			var data = await _context.CMMicroBacteriaAPGHB.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			_context.CMMicroBacteriaAPGHB.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMMicroBacteriaAPGHBExists(int id)
		{
			return _context.CMMicroBacteriaAPGHB.Any(e => e.id == id);
		}
	}
}