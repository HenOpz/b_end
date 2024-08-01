using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisDissolvedO2Controller : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisDissolvedO2Controller(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMWaterAnalysisDissolvedO2>> GetCMWaterAnalysisDissolvedO2List()
		{
			var data = await _context.CMWaterAnalysisDissolvedO2.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisDissolvedO2>> GetCMWaterAnalysisDissolvedO2ById(int id)
		{
			var data = await _context.CMWaterAnalysisDissolvedO2.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisDissolvedO2>>> GetCMWaterAnalysisDissolvedO2ByTag(int id_tag)
		{
			return await _context.CMWaterAnalysisDissolvedO2.Where(b => b.id_tag == id_tag).OrderBy(a => a.year).ThenBy(a => a.period).ToListAsync();
		}
		
		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisDissolvedO2>> PostCMWaterAnalysisDissolvedO2(CMWaterAnalysisDissolvedO2 data)
		{
			if (data.dissolved_o2_val == null)
			{
				data.id_status = 2;
			}
			else if (data.dissolved_o2_val != null)
			{
				if (data.dissolved_o2_val <= 30)
				{
					data.id_status = 3;
				}
				else if (data.dissolved_o2_val > 30)
				{
					data.id_status = 1;
				}
				else
				{
					data.id_status = null;
				}
			}

			_context.CMWaterAnalysisDissolvedO2.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisDissolvedO2ById", new { id = data.id }, data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisDissolvedO2(int id, CMWaterAnalysisDissolvedO2 data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			try
			{
				if (data.dissolved_o2_val == null)
				{
					data.id_status = 2;
				}
				else if (data.dissolved_o2_val != null)
				{
					if (data.dissolved_o2_val <= 30)
					{
						data.id_status = 3;
					}
					else if (data.dissolved_o2_val > 30)
					{
						data.id_status = 1;
					}
					else
					{
						data.id_status = null;
					}
				}
				
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisDissolvedO2Exists(id))
				{
					return NotFound($"CMWaterAnalysisDissolvedO2 with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisDissolvedO2(int id)
		{
			var data = await _context.CMWaterAnalysisDissolvedO2.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWaterAnalysisDissolvedO2.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWaterAnalysisDissolvedO2Exists(id))
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

		private bool CMWaterAnalysisDissolvedO2Exists(int id)
		{
			return _context.CMWaterAnalysisDissolvedO2.Any(e => e.id == id);
		}
	}
}