using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InspectionCampaignController : ControllerBase
	{
		private readonly MainDbContext _context;
		public InspectionCampaignController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InspectionCampaign>>> GetInspectionCampaign()
		{
			return await _context.InspectionCampaign.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<InspectionCampaign>> GetInspectionCampaign(int id)
		{
			var data = await _context.InspectionCampaign.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutInspectionCampaign(int id, InspectionCampaign data)
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
				if (!InspectionCampaignExists(id))
				{
					return NotFound($"InspectionCampaign with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		
		[HttpPost]
		public async Task<ActionResult<InspectionCampaign>> PostInspectionCampaign(InspectionCampaign data)
		{
			data.ic_number = GenReportNo();
			data.created_date = DateTime.Now;
			_context.InspectionCampaign.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetInspectionCampaign", new { id = data.id }, data);
		}
		
		[HttpDelete]
        [Route("delete-insp-campaign")]
        public async Task<IActionResult> DeleteInspectionCampaign(int id)
        {
            var data = await _context.InspectionCampaign.FindAsync(id);

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
                if (!InspectionCampaignExists(id))
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
			var data = _context.InspectionCampaign.Where(a => a.ic_number != null).OrderByDescending(a => a.ic_number).FirstOrDefault();

			if (data == null || (data?.ic_number?.StartsWith("IC") != true))
			{
				report_no = "IC-00001";
			}
			else
			{
				int tmp_no = Convert.ToInt32(data.ic_number.Substring(data.ic_number.Length - 5)) + 1;

				report_no = "IC-" + tmp_no.ToString("D5");
			}

			return report_no;
		}

		private bool InspectionCampaignExists(int id)
		{
			return _context.InspectionCampaign.Any(e => e.id == id);
		}
	}
}