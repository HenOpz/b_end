using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InspectionTaskController : ControllerBase
	{
		private readonly MainDbContext _context;
		public InspectionTaskController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InspectionTask>>> GetInspectionTask()
		{
			return await _context.InspectionTask.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<InspectionTask>> GetInspectionTask(int id)
		{
			var data = await _context.InspectionTask.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutInspectionTask(int id, InspectionTask data)
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
				if (!InspectionTaskExists(id))
				{
					return NotFound($"InspectionTask with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<InspectionTask>> PostInspectionTask(InspectionTask data)
		{
			data.it_number = GenReportNo();
			data.created_date = DateTime.Now;
			data.updated_date = DateTime.Now;
			_context.InspectionTask.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetInspectionTask", new { id = data.id }, data);
		}

		[HttpDelete]
		[Route("delete-insp-task")]
		public async Task<IActionResult> DeleteInspectionTask(int id)
		{
			var data = await _context.InspectionTask.FindAsync(id);

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
				if (!InspectionTaskExists(id))
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

		private bool InspectionTaskExists(int id)
		{
			return _context.InspectionTask.Any(e => e.id == id);
		}

		private string GenReportNo()
		{
			string report_no = "";
			var data = _context.InspectionTask.Where(a => a.it_number != null).OrderByDescending(a => a.it_number).FirstOrDefault();

			if (data == null || (data?.it_number?.StartsWith("IT")) != true)
			{
				report_no = "IT-00001";
			}
			else
			{
				int tmp_no = Convert.ToInt32(data.it_number.Substring(data.it_number.Length - 5)) + 1;

				report_no = "IT-" + tmp_no.ToString("D5");
			}

			return report_no;
		}
	}
}