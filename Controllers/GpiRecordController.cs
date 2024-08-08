using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using DocumentFormat.OpenXml.Wordprocessing;

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
		public async Task<ActionResult<dynamic>> GetGpiRecord(int id)
		{
			var data = await _context.GpiRecord.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}
			
			var txn = await _context.GpiRecordTXN.OrderBy(a => a.id).Where(a => a.id_gpi == data.id).ToListAsync();

			int seq = 1;
			var app_data = new List<GpiRecordApprovalSeq>();
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
						var sub = new GpiRecordApprovalSeq
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

			return  new { data, app_data };
		}

		[HttpGet]
		[Route("get-gpi-record-by-draft")]
		public async Task<ActionResult<dynamic>> GetGpiRecordListByDraft(int id_user)
		{
			var latestTransactions = await _context.GpiRecordTXN
				.GroupBy(txn => txn.id_gpi)
				.Select(g => g.OrderByDescending(txn => txn.id).FirstOrDefault())
				.ToListAsync();
				
			var gpiList = await _context.GpiRecord.Where(a => a.is_active == true).ToListAsync();

			var result = (from rc in gpiList
						  join txn in latestTransactions on rc.id equals txn.id_gpi
						  where rc.created_by == id_user && txn.id_status == 5
						  select new
						  {
							  GpiRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();


			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		[HttpGet]
		[Route("get-gpi-record-by-pending")]
		public async Task<ActionResult<dynamic>> GetGpiRecordListByPending(int id_user)
		{
			var latestTransactions = await _context.GpiRecordTXN
				.GroupBy(txn => txn.id_gpi)
				.Select(g => g.OrderByDescending(txn => txn.id).FirstOrDefault())
				.ToListAsync();
				
			var gpiList = await _context.GpiRecord.Where(a => a.is_active == true).ToListAsync();

			var result = (from rc in gpiList
						  join txn in latestTransactions on rc.id equals txn.id_gpi
						  where txn.id_user == id_user && txn.id_status == 2
						  select new
						  {
							  GpiRecord = rc,
							  IdStatus = txn.id_status
						  }).ToList();

			if (!result.Any())
			{
				return NotFound();
			}

			return result;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutGpiRecord(int id, GpiRecord data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}
			var auth = await _context.GpiRecordAuth.Where(a => a.id_discipline == data.id_discipline).OrderByDescending(a => a.seq).FirstOrDefaultAsync();
			if (auth == null)
			{
				return BadRequest("Authentication is invalid");
			}
			data.max_auth_seq = auth.seq;
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
			using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var auth = await _context.GpiRecordAuth.Where(a => a.id_discipline == data.id_discipline).OrderByDescending(a => a.seq).FirstOrDefaultAsync();
					if (auth == null)
					{
						return BadRequest("Authentication is invalid");
					}
					data.max_auth_seq = auth.seq;
					data.id_status = 1;
					data.gpi_number = GenReportNo((DateTime)data.gpi_date);
					data.created_date = DateTime.Now;
					_context.GpiRecord.Add(data);
					await _context.SaveChangesAsync();

					GpiRecordTXN txn = new()
					{
						id_status = 5,
						id_user = data.created_by,
						id_gpi = data.id,
						txn_datetime = DateTime.Now
					};
					_context.GpiRecordTXN.Add(txn);
					await _context.SaveChangesAsync();


					transaction.Complete();


					return CreatedAtAction("GetGpiRecord", new { id = data.id }, data);
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
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

			return CreatedAtAction("GetGpiRecord", new { id = data.id }, data);
		}

		private bool GpiRecordExists(int id)
		{
			return _context.GpiRecord.Any(e => e.id == id);
		}

		private string GenReportNo(DateTime dt)
		{
			string report_no = "";
			List<GpiRecord> data = _context.GpiRecord.Where(a => a.gpi_number != null && a.is_active == true).OrderByDescending(a => a.gpi_number).ToList();
			GpiRecord? lastData = new GpiRecord();

			if (data != null)
			{
				lastData = data.Where(a => (a.gpi_date.ToString("yyyy") == dt.ToString("yyyy"))).OrderByDescending(a => a.gpi_number).FirstOrDefault();
			}

			if (lastData == null || lastData.gpi_number == null)
			{
				report_no = "GPI-" + dt.ToString("yyyy") + "-001";
			}
			else
			{
				int tmp_no = Convert.ToInt32(lastData.gpi_number.Substring(lastData.gpi_number.Length - 3)) + 1;

				report_no = "GPI-" + dt.ToString("yyyy") + "-" + tmp_no.ToString("D3");
			}

			return report_no;
		}
	}
}