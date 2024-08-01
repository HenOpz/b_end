using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMERProbeRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMERProbeRecordController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMERProbeRecord>> GetCMERProbeRecordList()
		{
			var data = await _context.CMERProbeRecord.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMERProbeRecord>> GetCMERProbeRecordById(int id)
		{
			var data = await _context.CMERProbeRecord.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMERProbeRecord>>> GetCMERProbeRecordByTag(int id_tag)
		{
			return await _context.CMERProbeRecord.Where(b => b.id_tag == id_tag).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMERProbeRecord(int id, CMERProbeRecord data)
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
				if (!CMERProbeRecordExists(id))
				{
					return NotFound($"CMERProbeRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMERProbeRecord>> PostCMERProbeRecord(CMERProbeRecord data)
		{
			_context.CMERProbeRecord.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMERProbeRecordById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMERProbeRecord(int id)
		{
			var data = await _context.CMERProbeRecord.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMERProbeRecord.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMERProbeRecordExists(id))
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

		private bool CMERProbeRecordExists(int id)
		{
			return _context.CMERProbeRecord.Any(e => e.id == id);
		}
	}
}