using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisPHController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisPHController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMWaterAnalysisPH>> GetCMWaterAnalysisPHList()
		{
			var data = await _context.CMWaterAnalysisPH.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisPH>> GetCMWaterAnalysisPHById(int id)
		{
			var data = await _context.CMWaterAnalysisPH.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisPH>>> GetCMWaterAnalysisPHByTag(int id_tag)
		{
			return await _context.CMWaterAnalysisPH.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisPH(int id, CMWaterAnalysisPH data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisPHExists(id))
				{
					return NotFound($"CMWaterAnalysisPH with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisPH>> PostCMWaterAnalysisPH(CMWaterAnalysisPH data)
		{
			_context.CMWaterAnalysisPH.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisPHById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisPH(int id)
		{
			var data = await _context.CMWaterAnalysisPH.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWaterAnalysisPH.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisPHExists(id))
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

		private bool CMWaterAnalysisPHExists(int id)
		{
			return _context.CMWaterAnalysisPH.Any(e => e.id == id);
		}
	}
}