using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisCO2Controller : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisCO2Controller(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMWaterAnalysisCO2>> GetCMWaterAnalysisCO2List()
		{
			var data = await _context.CMWaterAnalysisCO2.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisCO2>> GetCMWaterAnalysisCO2ById(int id)
		{
			var data = await _context.CMWaterAnalysisCO2.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisCO2>>> GetCMWaterAnalysisCO2ByTag(int id_tag)
		{
			return await _context.CMWaterAnalysisCO2.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisCO2(int id, CMWaterAnalysisCO2 data)
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
				if (!CMWaterAnalysisCO2Exists(id))
				{
					return NotFound($"CMWaterAnalysisCO2 with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisCO2>> PostCMWaterAnalysisCO2(CMWaterAnalysisCO2 data)
		{
			_context.CMWaterAnalysisCO2.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisCO2ById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisCO2(int id)
		{
			var data = await _context.CMWaterAnalysisCO2.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWaterAnalysisCO2.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisCO2Exists(id))
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

		private bool CMWaterAnalysisCO2Exists(int id)
		{
			return _context.CMWaterAnalysisCO2.Any(e => e.id == id);
		}
	}
}