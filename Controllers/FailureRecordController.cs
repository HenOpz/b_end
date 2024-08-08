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
		public async Task<ActionResult<dynamic>> GetFailureRecord(int id)
		{
			var data = await _context.FailureRecord.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			var txn = await _context.FailureRecordTXN.OrderBy(a => a.id).Where(a => a.id_failure == data.id).ToListAsync();

			int seq = 1;
			var app_data = new List<FailureRecordApprovalSeq>();
			var userInf = await _context.UserInfo.ToListAsync();
			var comp = await _context.MdUserCompany.ToListAsync();
			foreach (var row in txn)
			{
				if (row.seq == seq)
				{
					if (row.id_status == 1 || row.id_status == 3)
					{
						var uif = userInf.Where(a => a.id == row.id_user_info).FirstOrDefault();
						var cmp = uif != null ? comp.FirstOrDefault(a => a.id == uif.id_company) : null;
						var sub = new FailureRecordApprovalSeq
						{
							id_user = row.id_user,
							id_user_info = row.id_user_info,
							seq = row.seq,
							id_status = row.id_status,
							txn_datetime = row.txn_datetime,
							name = uif != null ? uif.name : null,
							position = uif != null ? uif.position : null,
							signature = uif != null ? uif.signature : null,
							company = cmp != null ? cmp.company_name : null,
						};
						app_data.Add(sub);
						seq = seq + 1;
					}
					if (row.id_status == 4)
					{
						if (app_data.Count == 1)
						{
							app_data.Clear();
							seq = 1;
						}
						else
						{
							app_data.RemoveAt(app_data.Count - 1);
							seq = seq - 1;
						}
					}
				}
			}

			return new { data, app_data };
		}

		[HttpGet]
		[Route("get-failure-record-by-draft")]
		public async Task<ActionResult<dynamic>> GetFailureRecordDraftByIdUser(int id_user)
		{
			var lastTXN = await _context.FailureRecordTXN
							.GroupBy(txn => txn.id_failure)
							.Select(f => f.OrderByDescending(txn => txn.id).FirstOrDefault())
							.ToListAsync();

			var failureList = await _context.FailureRecord.Where(a => a.is_active == true).ToListAsync();

			var result = (from rc in failureList
						  join txn in lastTXN on rc.id equals txn.id_failure
						  where rc.created_by == id_user && txn.id_status == 5
						  select new

						  {
							  FailureRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();

			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		[HttpGet]
		[Route("get-failure-record-by-pending")]
		public async Task<ActionResult<dynamic>> GetFailureRecordPendingByIdUser(int id_user)
		{
			var lastTXN = await _context.FailureRecordTXN
				.GroupBy(txn => txn.id_failure)
				.Select(g => g.OrderByDescending(txn => txn.id).FirstOrDefault())
				.ToListAsync();

			var failureList = await _context.FailureRecord.Where(a => a.is_active == true).ToListAsync();

			var result = (from rc in failureList
						  join txn in lastTXN on rc.id equals txn.id_failure
						  where txn.id_user == id_user && txn.id_status == 2
						  select new
						  {
							  FailureRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();

			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		// [HttpGet]
		// [Route("get-failure-record-approved-by-id-user")]
		// public async Task<ActionResult<IEnumerable<FailureRecord>>> GetFailureRecordApprByIdUser(int id_user)
		// {
		// 	var data = await GetFailureRecordsByUserAsync(id_user, 3);

		// 	if (data == null || !data.Any())
		// 	{
		// 		return NotFound();
		// 	}

		// 	return Ok(data);
		// }

		[HttpGet]
		[Route("get-failure-record-by-draft-is-rcfa")]
		public async Task<ActionResult<dynamic>> GetFailureRecordDraftByIdUserIsRCFA(int id_user)
		{
			var lastTXN = await _context.FailureRecordTXN
							.GroupBy(txn => txn.id_failure)
							.Select(f => f.OrderByDescending(txn => txn.id).FirstOrDefault())
							.ToListAsync();

			var failureList = await _context.FailureRecord.Where(a => a.is_active == true && a.is_rcfa == true).ToListAsync();

			var result = (from rc in failureList
						  join txn in lastTXN on rc.id equals txn.id_failure
						  where rc.created_by == id_user && txn.id_status == 5
						  select new

						  {
							  FailureRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();

			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		[HttpGet]
		[Route("get-failure-record-by-pending-is-rcfa")]
		public async Task<ActionResult<dynamic>> GetFailureRecordPendingByIdUserIsRCFA(int id_user)
		{
			var lastTXN = await _context.FailureRecordTXN
							.GroupBy(txn => txn.id_failure)
							.Select(f => f.OrderByDescending(txn => txn.id).FirstOrDefault())
							.ToListAsync();

			var failureList = await _context.FailureRecord.Where(a => a.is_active == true && a.is_rcfa == true).ToListAsync();

			var result = (from rc in failureList
						  join txn in lastTXN on rc.id equals txn.id_failure
						  where rc.created_by == id_user && txn.id_status == 2
						  select new

						  {
							  FailureRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();

			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		// [HttpGet]
		// [Route("get-failure-record-approved-by-id-user-is-rcfa")]
		// public async Task<ActionResult<IEnumerable<FailureRecord>>> GetFailureRecordApprByIdUserIsRCFA(int id_user)
		// {
		// 	var data = await GetFailureRecordsByUserAsyncIsRCFA(id_user, 3);

		// 	if (data == null || !data.Any())
		// 	{
		// 		return NotFound();
		// 	}

		// 	return Ok(data);
		// }

		[HttpGet]
		[Route("get-failure-record-list-is-rcfa")]
		public async Task<ActionResult<List<FailureRecord>>> GetFailureRecordIsRCFA()
		{
			var data = await _context.FailureRecord.Where(a => (a.is_active == true) && (a.is_rcfa == true)).ToListAsync();

			if (!data.Any())
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet]
		[Route("get-failure-record-by-id-is-rcfa")]
		public async Task<ActionResult<FailureRecord>> GetFailureRecordIsRCFA(int id)
		{
			var data = await _context.FailureRecord.Where(a => (a.is_active == true) && (a.id == id) && (a.is_rcfa == true)).FirstOrDefaultAsync();

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

			if ((data.id_cof_environment != null || data.id_cof_people != null || data.id_cof_production_loss != null || data.id_cof_reputation != null) && (data.id_pof != null))
			{
				var risk = await GetRiskMatrix(data.id_cof_people, data.id_cof_environment, data.id_cof_production_loss, data.id_cof_reputation, data.id_pof);

				if (risk != null)
				{
					data.id_risk = risk.id;
				}
			}

			data.updated_by = data.updated_by;
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
			using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var user = await _context.FailureRecordAuth.FirstOrDefaultAsync(a => a.id_user == data.created_by);
					if (user == null) { return BadRequest("User is invalid"); }
					var auth = await _context.FailureRecordAuth.Where(x => x.id_work_group == data.id_work_group && x.id_role == user.id_role).OrderByDescending(a => a.seq).FirstOrDefaultAsync();
					if (auth == null) { return BadRequest("Authentication is invalid"); }

					data.max_auth_seq = auth.seq;
					data.fl_number = GenReportNo((DateTime)data.findings_date);
					data.created_date = DateTime.Now;

					if ((data.id_cof_environment != null || data.id_cof_people != null || data.id_cof_production_loss != null || data.id_cof_reputation != null) && (data.id_pof != null))
					{
						var risk = await GetRiskMatrix(data.id_cof_people, data.id_cof_environment, data.id_cof_production_loss, data.id_cof_reputation, data.id_pof);

						if (risk != null)
						{
							data.id_risk = risk.id;
						}
					}

					_context.FailureRecord.Add(data);
					await _context.SaveChangesAsync();

					FailureRecordTXN txn = new()
					{
						id_status = 5,
						id_user = data.created_by,
						id_failure = data.id,
						txn_datetime = DateTime.Now
					};
					_context.FailureRecordTXN.Add(txn);
					await _context.SaveChangesAsync();

					transaction.Complete();


					return CreatedAtAction("GetFailureRecord", new { id = data.id }, data);
				}
				catch (Exception ex)
				{
					transaction.Dispose();
					return BadRequest(ex.Message);
				}
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

		[NonAction]
		public async Task<IEnumerable<FailureRecord>> GetFailureRecordsByUserAsync(int userId, int statusId)
		{
			var records = await _context.FailureRecord
				.Where(fr => _context.FailureRecordTXN
					.Where(txn => txn.id_user == userId && txn.id_status == statusId)
					.OrderByDescending(txn => txn.txn_datetime)
					.FirstOrDefault(txn => txn.id_failure == fr.id) != null)
				.ToListAsync();

			return records;
		}

		[NonAction]
		public async Task<IEnumerable<FailureRecord>> GetFailureRecordsByUserAsyncIsRCFA(int userId, int statusId)
		{
			var records = await _context.FailureRecord
				.Where(fr => _context.FailureRecordTXN
					.Where(txn => txn.id_user == userId && txn.id_status == statusId)
					.OrderByDescending(txn => txn.txn_datetime)
					.FirstOrDefault(txn => txn.id_failure == fr.id) != null && fr.is_rcfa == true)
				.ToListAsync();

			return records;
		}

		[NonAction]
		public async Task<MdFailureRiskMatrix?> GetRiskMatrix(int? id_cof_people, int? id_cof_environment, int? id_cof_production_loss, int? id_cof_reputation, int? id_pof)
		{
			int? max_id_cof = null;

			if (id_cof_people.HasValue)
				max_id_cof = id_cof_people.Value;

			if (id_cof_environment.HasValue)
				max_id_cof = max_id_cof.HasValue ? Math.Max(max_id_cof.Value, id_cof_environment.Value) : id_cof_environment.Value;

			if (id_cof_production_loss.HasValue)
				max_id_cof = max_id_cof.HasValue ? Math.Max(max_id_cof.Value, id_cof_production_loss.Value) : id_cof_production_loss.Value;

			if (id_cof_reputation.HasValue)
				max_id_cof = max_id_cof.HasValue ? Math.Max(max_id_cof.Value, id_cof_reputation.Value) : id_cof_reputation.Value;

			var risk = await _context.MdFailureRiskMatrix.FirstOrDefaultAsync(a => a.id_cof == max_id_cof && a.id_pof == id_pof);

			return risk;
		}

		private string GenReportNo(DateTime dt)
		{
			string report_no = "";
			List<FailureRecord> data = _context.FailureRecord.Where(a => a.fl_number != null && a.is_active == true).OrderByDescending(a => a.fl_number).ToList();
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