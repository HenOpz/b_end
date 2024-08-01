using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisIonCountController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisIonCountController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMWaterAnalysisIonCount>> GetCMWaterAnalysisIonCountList()
		{
			var data = await _context.CMWaterAnalysisIonCount.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisIonCount>> GetCMWaterAnalysisIonCountById(int id)
		{
			var data = await _context.CMWaterAnalysisIonCount.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisIonCount>>> GetCMWaterAnalysisIonCountByTag(int id_tag)
		{
			return await _context.CMWaterAnalysisIonCount.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisIonCount(int id, CMWaterAnalysisIonCount data)
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
				if (!CMWaterAnalysisIonCountExists(id))
				{
					return NotFound($"CMWaterAnalysisIonCount with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisIonCount>> PostCMWaterAnalysisIonCount(CMWaterAnalysisIonCount data)
		{
			_context.CMWaterAnalysisIonCount.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisIonCountById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisIonCount(int id)
		{
			var data = await _context.CMWaterAnalysisIonCount.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWaterAnalysisIonCount.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisIonCountExists(id))
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

		private bool CMWaterAnalysisIonCountExists(int id)
		{
			return _context.CMWaterAnalysisIonCount.Any(e => e.id == id);
		}
	}
}