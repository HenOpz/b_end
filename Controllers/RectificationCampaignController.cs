using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RectificationCampaignController : ControllerBase
	{
		private readonly MainDbContext _context;
		public RectificationCampaignController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RectificationCampaign>>> GetRectificationCampaign()
		{
			return await _context.RectificationCampaign.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<RectificationCampaign>> GetRectificationCampaign(int id)
		{
			var data = await _context.RectificationCampaign.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutRectificationCampaign(int id, RectificationCampaign data)
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
				if (!RectificationCampaignExists(id))
				{
					return NotFound($"RectificationCampaign with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<RectificationCampaign>> PostRectificationCampaign(RectificationCampaign data)
		{
			data.rc_number = GenReportNo();
			data.created_date = DateTime.Now;
			_context.RectificationCampaign.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetRectificationCampaign", new { id = data.id }, data);
		}
		
		[HttpDelete]
		[Route("delete-rectification-campaign")]
		public async Task<IActionResult> DeleteRectificationCampaign(int id)
		{
			var data = await _context.RectificationCampaign.FindAsync(id);

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
				if (!RectificationCampaignExists(id))
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

		private string GenReportNo()
		{
			string report_no = "";
			var data = _context.RectificationCampaign.Where(a => a.rc_number != null).OrderByDescending(a => a.rc_number).FirstOrDefault();

			if (data == null || (data.rc_number != null && !data.rc_number.StartsWith("IC")))
			{
				report_no = "RC-00001";
			}
			else
			{
				if (data.rc_number == null) return "";
				int tmp_no = Convert.ToInt32(data.rc_number.Substring(data.rc_number.Length - 5)) + 1;

				report_no = "RC-" + tmp_no.ToString("D5");
			}

			return report_no;
		}

		private bool RectificationCampaignExists(int id)
		{
			return _context.RectificationCampaign.Any(e => e.id == id);
		}
	}
}