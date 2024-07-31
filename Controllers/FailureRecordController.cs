using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FailureRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public FailureRecordController(MainDbContext context)
		{
			_context = context;
		}
		//string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<FailureRecord>>> GetFailureRecord()
		{
			return await _context.FailureRecord.Where(a => a.is_active == true).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<FailureRecord>> GetFailureRecord(int id)
		{
			var data = await _context.FailureRecord.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutFailureRecord(int id, FailureRecord data)
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
				if (!FailureRecordExists(id))
				{
					return NotFound($"FailureRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<FailureRecord>> PostFailureRecord(FailureRecord data)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					data.fl_number = GenReportNo((DateTime)data.findings_date);
					data.created_date = DateTime.Now;
					data.id_appr_status = 6;
					_context.FailureRecord.Add(data);
					await _context.SaveChangesAsync();

					FailureRecordTXN txn = new()
					{
						id_status = 6,
						id_user = data.created_by,
						id_failure = data.id,
						txn_datetime = DateTime.Now
					};
					_context.FailureRecordTXN.Add(txn);
					await _context.SaveChangesAsync();

					transaction.Complete();
				}

				return CreatedAtAction("GetFailureRecord", new { id = data.id }, data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete]
		[Route("delete-failure-record")]
		public async Task<IActionResult> DeleteFailureRecord(int id)
		{
			var data = await _context.FailureRecord.FindAsync(id);

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
				if (!FailureRecordExists(id))
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

		private bool FailureRecordExists(int id)
		{
			return _context.FailureRecord.Any(e => e.id == id);
		}
		
		private string GenReportNo(DateTime dt)
		{
			string report_no = "";
			List<FailureRecord> data = _context.FailureRecord.Where(a => a.fl_number != null).OrderByDescending(a => a.fl_number).ToList();
			FailureRecord? lastData = new FailureRecord();
			if (data != null)
			{
				
				lastData = data.Where(a => (a.findings_date.ToString("yyyy") == dt.ToString("yyyy")) && (a.findings_date.ToString("MM") == dt.ToString("MM"))).OrderByDescending(a => a.fl_number).FirstOrDefault();
			}

			if (lastData == null || lastData.fl_number == null)
			{
				report_no = dt.ToString("yyyy") + dt.ToString("MM") + "-0001";
			}
			else
			{
				int tmp_no = Convert.ToInt32(lastData.fl_number.Substring(lastData.fl_number.Length - 4)) + 1;

				report_no = dt.ToString("yyyy") + dt.ToString("MM") + "-" + tmp_no.ToString("D4");
			}

			return report_no;
		}
	}
}