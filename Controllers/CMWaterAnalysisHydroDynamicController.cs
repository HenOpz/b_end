using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisHydroDynamicController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisHydroDynamicController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMWaterAnalysisHydroDynamic>> GetCMWaterAnalysisHydroDynamicList()
		{
			var data = await _context.CMWaterAnalysisHydroDynamic.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisHydroDynamic>> GetCMWaterAnalysisHydroDynamicById(int id)
		{
			var data = await _context.CMWaterAnalysisHydroDynamic.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisHydroDynamic>>> GetCMWaterAnalysisHydroDynamicByTag(int id_tag)
		{
			return await _context.CMWaterAnalysisHydroDynamic.Where(b => b.id_tag == id_tag).OrderBy(a => a.record_date).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisHydroDynamic(int id, CMWaterAnalysisHydroDynamic data)
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
				if (!CMWaterAnalysisHydroDynamicExists(id))
				{
					return NotFound($"CMWaterAnalysisHydroDynamic with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisHydroDynamic>> PostCMWaterAnalysisHydroDynamic(CMWaterAnalysisHydroDynamic data)
		{
			_context.CMWaterAnalysisHydroDynamic.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisHydroDynamicById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisHydroDynamic(int id)
		{
			var data = await _context.CMWaterAnalysisHydroDynamic.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWaterAnalysisHydroDynamic.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisHydroDynamicExists(id))
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

		private bool CMWaterAnalysisHydroDynamicExists(int id)
		{
			return _context.CMWaterAnalysisHydroDynamic.Any(e => e.id == id);
		}
	}
}