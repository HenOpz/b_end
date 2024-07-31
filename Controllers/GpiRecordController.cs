using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GpiRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public GpiRecordController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpGet]
		public async Task<ActionResult<IEnumerable<GpiRecord>>> GetGpiRecord()
		{
			return await _context.GpiRecord.Where(a => a.is_active == true).ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<GpiRecord>> GetGpiRecord(int id)
		{
			var data = await _context.GpiRecord.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutGpiRecord(int id, GpiRecord data)
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
				if (!GpiRecordExists(id))
				{
					return NotFound($"GpiRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<GpiRecord>> PostGpiRecord(GpiRecord data)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					data.created_date = DateTime.Now;
					_context.GpiRecord.Add(data);
					await _context.SaveChangesAsync();

					transaction.Complete();
				}

				return CreatedAtAction("GetGpiRecord", new { id = data.id }, data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		[HttpDelete]
		[Route("delete-gpi-record")]
		public async Task<IActionResult> DeleteGpiRecord(int id)
		{
			var data = await _context.GpiRecord.FindAsync(id);

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
				if (!GpiRecordExists(id))
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
		
		private bool GpiRecordExists(int id)
		{
			return _context.GpiRecord.Any(e => e.id == id);
		}
	}
}