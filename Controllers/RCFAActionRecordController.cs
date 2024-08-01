using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RCFAActionRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public RCFAActionRecordController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetRCFAActionRecord()
		{
			var data = await GetRCFAActionRecordQuery().Where(a => a.is_active == true).ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRCFAActionRecord(int id)
		{
			var data = await GetRCFAActionRecordQuery().Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		[Route("get-rcfa-action-record-long-term-by-id")]
		public async Task<IActionResult> GetRCFAActionRecordLongtermById(int id_failure)
		{
			var data = await GetRCFAActionRecordQuery().Where(a => a.id_failure == id_failure && a.action_type == "long-term" && a.is_active == true).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound(new { message = "NoActionRecorded" });
			}

			return Ok(data);
		}


		[HttpGet]
		[Route("get-rcfa-action-record-short-term-by-id")]
		public async Task<IActionResult> GetRCFAActionRecordShorttermById(int id_failure)
		{
			var data = await GetRCFAActionRecordQuery().Where(a => a.id_failure == id_failure && a.action_type == "short-term" && a.is_active == true).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound(new { message = "NoActionRecorded" });
			}

			return Ok(data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutRCFAActionRecord(int id, RCFAActionRecord data)
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
				if (!RCFAActionRecordExists(id))
				{
					return NotFound($"RCFAActionRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<RCFAActionRecord>> PostRCFAActionRecord(RCFAActionRecord data)
		{
			var fl = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == data.id_failure);
			
			if (fl == null) return BadRequest("Failure Record was not found!");
			if (fl.is_rcfa == null || fl.is_rcfa == false) return BadRequest("This Failure Record can not create RCFA action!");
			
			if(data.action_type == null) return BadRequest("Can't generate RCFA action no without action type!");
			data.ra_number = GenReportNo(data.id_failure,data.action_type);
			data.created_date = DateTime.Now;
			_context.RCFAActionRecord.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetRCFAActionRecord", new { id = data.id }, data);
		}

		[HttpDelete]
		[Route("delete-rcfa-action-record")]
		public async Task<IActionResult> DeleteRCFAActionRecord(int id)
		{
			var data = await _context.RCFAActionRecord.FindAsync(id);

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
				if (!RCFAActionRecordExists(id))
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

		private bool RCFAActionRecordExists(int id)
		{
			return _context.RCFAActionRecord.Any(e => e.id == id);
		}

		private string GenReportNo(int id_failure, string action_type)
		{
			string act_type = "";
			string report_no = "";
			var data = _context.FailureRecord.Where(a => a.id == id_failure && a.is_active == true).FirstOrDefault();
			var actionData = _context.RCFAActionRecord.Where(a => (a.action_type == action_type) && (a.id_failure == id_failure) && (a.is_active == true)).OrderByDescending(a => a.ra_number).FirstOrDefault();

			if (action_type == "long-term")
			{
				act_type = "LT";
			}
			else if (action_type == "short-term")
			{
				act_type = "ST";
			}

			if (data != null && data.fl_number != null)
			{
				if (actionData == null || actionData.ra_number == null)
				{
					report_no = data.fl_number + "-R-" + act_type + "01";
				}
				else
				{
					int tmp_no = Convert.ToInt32(actionData.ra_number.Substring(actionData.ra_number.Length - 2)) + 1;
					report_no = data.fl_number + "-R-" + act_type + tmp_no.ToString("D2");
				}
			}


			return report_no;
		}

		private IQueryable<RCFAActionRecordView> GetRCFAActionRecordQuery()
		{
			return from fa in _context.RCFAActionRecord
				   join fr in _context.FailureRecord on fa.id_failure equals fr.id into fafr
				   from fafrResult in fafr.DefaultIfEmpty()

				   join stt in _context.MdFailureActionStatus on fa.id_rcfa_action_status equals stt.id into fastt
				   from fasttResult in fastt.DefaultIfEmpty()

				   join fd in _context.MdFailureDiscipline on fa.id_discipline equals fd.id into fafd
				   from fafdResult in fafd.DefaultIfEmpty()

				   join cb in _context.User on fa.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on fa.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   select new RCFAActionRecordView
				   {
					   id = fa.id,
					   id_failure = fa.id_failure,
					   fl_number = fafrResult.fl_number,
					   tag_no = fafrResult.tag_no,
					   id_rcfa_action_status = fa.id_rcfa_action_status,
					   rcfa_action_status = fasttResult.status,
					   rcfa_action_color_code = fasttResult.color_code,
					   id_discipline = fa.id_discipline,
					   discipline = fafdResult.discipline,
					   ra_number = fa.ra_number,
					   action_type = fa.action_type,
					   action_date = fa.action_date,
					   action_details = fa.action_details,
					   is_active = fa.is_active,
					   created_by = fa.created_by,
					   created_by_name = icbResult.name,
					   created_date = fa.created_date,
					   updated_by = fa.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = fa.updated_date
				   };

		}
	}
}