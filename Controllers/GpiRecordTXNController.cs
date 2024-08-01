using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GpiRecordTXNController : ControllerBase
	{
		private readonly MainDbContext _context;
		public GpiRecordTXNController(MainDbContext context)
		{
			_context = context;
		}

		//Get TXN by id_gpi
		[HttpGet]
		[Route("get-gpi-record-txn-by-id-gpi")]
		public async Task<ActionResult<IEnumerable<GpiRecordTXN>>> GetGpiRecordTXNByIdGpi(int id_gpi)
		{
			return await _context.GpiRecordTXN.Where(a => a.id_gpi == id_gpi).ToListAsync();
		}

		//Get last TXN by id_gpi
		[HttpGet]
		[Route("get-last-gpi-record-txn-by-id-gpi")]
		public async Task<dynamic> GetLastGpiRecordTXNByIdGpi(int id_gpi)
		{
			var data = await _context.GpiRecordTXN.Where(a => a.id_gpi == id_gpi).OrderByDescending(a => a.id).FirstOrDefaultAsync();
			if (data == null) return NotFound("GpiRecordTXN was not found.");

			return data;
		}

		//Submit record
		[HttpPost]
		[Route("add-submit-txn")]
		public async Task<IActionResult> AddSubmitTXN(int id_user, int id_gpi)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.GpiRecord.FirstOrDefaultAsync(x => x.id == id_gpi);
					if (fr == null)
					{
						return BadRequest("GpiRecord was not found.");
					}

					var auth = await _context.GpiRecordAuth.Where(x => x.id_discipline == fr.id_discipline).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("GpiRecordAuth was not found.");
					}

					var lastTXN = await _context.GpiRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_gpi == id_gpi);

					if (lastTXN != null && lastTXN.id_status == 5)
					{
						GpiRecordTXN newTXN = new()
						{
							id_gpi = id_gpi,
							id_user = id_user,
							id_status = 1,
							seq = 1,
							remark = null,
							txn_datetime = DateTime.Now
						};

						GpiRecordAuth? tmp = auth.Where(a => a.seq == 2).FirstOrDefault();
						if (tmp == null)
						{
							return BadRequest("Next authorized was not found.");
						}

						GpiRecordTXN nextTXN = new()
						{
							id_gpi = id_gpi,
							id_user = tmp.id_user,
							id_status = 2,
							seq = 2,
							remark = null,
							txn_datetime = DateTime.Now
						};

						_context.GpiRecordTXN.Add(newTXN);
						_context.GpiRecordTXN.Add(nextTXN);

						await _context.SaveChangesAsync();
					}
					else
					{
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Submission is completed.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		//Approval record
		[HttpPost]
		[Route("add-appr-txn")]
		public async Task<IActionResult> AddApproveTXN(int id_user, int id_gpi)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.GpiRecord.FirstOrDefaultAsync(x => x.id == id_gpi);
					if (fr == null)
					{
						return BadRequest("GpiRecord was not found.");
					}

					var auth = await _context.GpiRecordAuth.Where(x => x.id_discipline == fr.id_discipline).OrderBy(a => a.seq).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("GpiRecordAuth was not found.");
					}
					var auth_user = auth.Where(a => a.id_user == id_user).First();

					var lastTXN = await _context.GpiRecordTXN.Where(a => a.id_gpi == id_gpi).OrderByDescending(a => a.id).ToListAsync();

					if (lastTXN.Any())
					{
						if (lastTXN[0].id_user == id_user && lastTXN[0].id_status == 2 && lastTXN[0].seq == auth_user.seq)
						{
							GpiRecordTXN newTXN = new()
							{
								id_gpi = id_gpi,
								id_user = id_user,
								id_status = 3,
								seq = auth_user.seq,
								remark = null,
								txn_datetime = DateTime.Now
							};

							_context.GpiRecordTXN.Add(newTXN);

							if (fr.max_auth_seq != auth_user.seq)
							{
								var next_user = auth.Where(a => a.seq == auth_user.seq + 1).First();
								GpiRecordTXN nextTXN = new()
								{
									id_gpi = id_gpi,
									id_user = next_user.id_user,
									id_status = 2,
									seq = next_user.seq,
									remark = null,
									txn_datetime = DateTime.Now
								};

								_context.GpiRecordTXN.Add(nextTXN);
							}

							await _context.SaveChangesAsync();

						}
						else
						{
							return Forbid("Transaction status is invalid.");
						}
					}
					else
					{
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Approval is completed.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		//Reject record
		[HttpPost]
		[Route("add-reject-txn")]
		public async Task<IActionResult> AddRejectTXN(int id_user, int id_gpi, string? remark)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.GpiRecord.FirstOrDefaultAsync(x => x.id == id_gpi);
					if (fr == null)
					{
						return BadRequest("GpiRecord was not found.");
					}

					var auth = await _context.GpiRecordAuth.Where(x => x.id_discipline == fr.id_discipline).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("GpiRecordAuth was not found.");
					}
					var auth_user = auth.Where(a => a.id_user == id_user).First();

					var lastTXN = await _context.GpiRecordTXN.Where(a => a.id_gpi == id_gpi).OrderByDescending(a => a.id).ToListAsync();

					if (lastTXN.Any())
					{
						if (lastTXN[0].id_user == id_user && lastTXN[0].id_status == 2 && lastTXN[0].seq == auth_user.seq)
						{
							GpiRecordTXN newTXN = new()
							{
								id_gpi = id_gpi,
								id_user = id_user,
								id_status = 4,
								seq = auth_user.seq,
								remark = remark,
								txn_datetime = DateTime.Now
							};

							_context.GpiRecordTXN.Add(newTXN);

							if (auth_user.seq == 2)
							{
								var first_user = await _context.GpiRecordAuth.Where(a => a.id_user == fr.created_by).FirstAsync();
								GpiRecordTXN nextTXN = new()
								{
									id_gpi = id_gpi,
									id_user = first_user.id_user,
									id_status = 5,
									seq = first_user.seq,
									remark = remark,
									txn_datetime = DateTime.Now
								};

								_context.GpiRecordTXN.Add(nextTXN);
							}
							else
							{
								var pre_user = await _context.GpiRecordAuth.Where(a => a.seq == auth_user.seq - 1).FirstAsync();

								GpiRecordTXN nextTXN = new()
								{
									id_gpi = id_gpi,
									id_user = pre_user.id_user,
									id_status = 2,
									seq = pre_user.seq,
									remark = null,
									txn_datetime = DateTime.Now
								};

								_context.GpiRecordTXN.Add(nextTXN);
							}
							
							
							await _context.SaveChangesAsync();
						}
						else
						{
							return Forbid("Transaction status is invalid.");
						}
					}
					else
					{
						return Forbid("Transaction status is invalid.");
					}

					transaction.Complete();
				}
				return Ok("Rejection is completed.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}