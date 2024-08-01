using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExInspectionRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ExInspectionRecordController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ExInspectionRecord>>> GetExInspectionRecord()
		{
			return await _context.ExInspectionRecord.Where(a => a.is_active == true).ToListAsync();
		}
		
		[HttpGet]
		[Route("get-ex-insp-record-by-id-info")]
		public async Task<ActionResult<List<ExInspectionRecord>>> GetExInspectionRecordByIdInfo(int id)
		{
			var data = await _context.ExInspectionRecord.Where(a => a.id_info == id && a.is_active == true).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ExInspectionRecord>> GetExInspectionRecord(int id)
		{
			var data = await _context.ExInspectionRecord.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutExInspectionRecord(int id, ExInspectionRecord data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			data.updated_date = DateTime.Now;

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ExInspectionRecordExists(id))
				{
					return NotFound($"ExInspectionRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<ExInspectionRecord>> PostExInspectionRecord(ExInspectionRecord data)
		{
			data.created_date = DateTime.Now;
			_context.ExInspectionRecord.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetExInspectionRecord", new { id = data.id }, data);
		}
		
		[HttpDelete]
		[Route("delete-ex-insp-record")]
		public async Task<IActionResult> DeleteExInspectionRecord(int id)
		{
			var data = await _context.ExInspectionRecord.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			data.is_active = false;
			data.updated_date = DateTime.Now;
			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ExInspectionRecordExists(id))
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

		private bool ExInspectionRecordExists(int id)
		{
			return _context.ExInspectionRecord.Any(e => e.id == id);
		}
	}
}