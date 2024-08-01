using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FailureRecordTXNController : ControllerBase
	{
		private readonly MainDbContext _context;
		public FailureRecordTXNController(MainDbContext context)
		{
			_context = context;
		}
		
		//Get TXN by id_failure
		[HttpGet]
		[Route("get-failure-record-txn-by-id-failure")]
		public async Task<ActionResult<IEnumerable<FailureRecordTXN>>> GetFailureRecordTXNByIdFailure(int id_failure)
		{
			return await _context.FailureRecordTXN.Where(a => a.id_failure == id_failure).ToListAsync();
		}

		//Get last TXN by id_failure
		[HttpGet]
		[Route("get-last-failure-record-txn-by-id-failure")]
		public async Task<dynamic> GetLastFailureRecordTXNByIdFailure(int id_failure)
		{
			var data = await _context.FailureRecordTXN.Where(a => a.id_failure == id_failure).OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync();
			if(data == null) return NotFound("FailureRecordTXN was not found.");
			
			return data;
		}

		//Submit record
		[HttpPost]
		[Route("add-submit-txn")]
		public async Task<IActionResult> AddSubmitTXN(int id_user, int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.FailureRecord.FirstOrDefaultAsync(x => x.id == id_failure);
					if (fr == null)
					{
						return BadRequest("FailureRecord was not found.");
					}

					var auth = await _context.FailureRecordAuth.Where(x => x.id_work_group == fr.id_work_group).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("FailureRecordAuth was not found.");
					}

					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);

					if (lastTXN != null && lastTXN.id_status == 5)
					{
						FailureRecordTXN newTXN = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 1,
							seq = 1,
							remark = null,
							txn_datetime = DateTime.Now
						};

						FailureRecordAuth? tmp = auth.Where(a => a.seq == 2).FirstOrDefault();
						if (tmp == null)
						{
							return BadRequest("Next authorized was not found.");
						}

						FailureRecordTXN nextTXN = new()
						{
							id_failure = id_failure,
							id_user = tmp.id_user,
							id_status = 2,
							seq = 2,
							remark = null,
							txn_datetime = DateTime.Now
						};

						_context.FailureRecordTXN.Add(newTXN);
						_context.FailureRecordTXN.Add(nextTXN);

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
		public async Task<IActionResult> AddApproveTXN(int id_user, int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.FailureRecord.FirstOrDefaultAsync(x => x.id == id_failure);
					if (fr == null)
					{
						return BadRequest("FailureRecord was not found.");
					}

					var auth = await _context.FailureRecordAuth.Where(x => x.id_work_group == fr.id_work_group).OrderBy(a => a.seq).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("FailureRecordAuth was not found.");
					}
					var auth_user = auth.Where(a => a.id_user == id_user).First();

					var lastTXN = await _context.FailureRecordTXN.Where(a => a.id_failure == id_failure).OrderByDescending(a => a.seq).ToListAsync();

					if (lastTXN.Any())
					{
						if (lastTXN[0].id_user == id_user && lastTXN[0].id_status == 2 && lastTXN[0].seq == auth_user.seq)
						{
							FailureRecordTXN newTXN = new()
							{
								id_failure = id_failure,
								id_user = id_user,
								id_status = 3,
								seq = auth_user.seq,
								remark = null,
								txn_datetime = DateTime.Now
							};

							_context.FailureRecordTXN.Add(newTXN);

							if (fr.max_auth_seq != auth_user.seq)
							{
								var next_user = auth.Where(a => a.seq == auth_user.seq + 1).First();
								FailureRecordTXN nextTXN = new()
								{
									id_failure = id_failure,
									id_user = next_user.id_user,
									id_status = 2,
									seq = next_user.seq,
									remark = null,
									txn_datetime = DateTime.Now
								};
								
								_context.FailureRecordTXN.Add(nextTXN);
							}
							
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
		public async Task<IActionResult> AddRejectTXN(int id_user, int id_failure, string? remark)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var fr = await _context.FailureRecord.FirstOrDefaultAsync(x => x.id == id_failure);
					if (fr == null)
					{
						return BadRequest("FailureRecord was not found.");
					}

					var auth = await _context.FailureRecordAuth.Where(x => x.id_work_group == fr.id_work_group).ToListAsync();
					if (!auth.Any())
					{
						return BadRequest("FailureRecordAuth was not found.");
					}
					var auth_user = auth.Where(a => a.id_user == id_user).First();

					var lastTXN = await _context.FailureRecordTXN.Where(a => a.id_failure == id_failure).OrderByDescending(a => a.seq).ToListAsync();

					if (lastTXN.Any())
					{
						if (lastTXN[0].id_user == id_user && lastTXN[0].id_status == 2 && lastTXN[0].seq == auth_user.seq)
						{
							FailureRecordTXN newTXN = new()
							{
								id_failure = id_failure,
								id_user = id_user,
								id_status = 4,
								seq = auth_user.seq,
								remark = remark,
								txn_datetime = DateTime.Now
							};

							_context.FailureRecordTXN.Add(newTXN);

							if (auth_user.seq == 2)
							{
								var first_user = auth.Where(a => a.seq == 1).First();
								FailureRecordTXN nextTXN = new()
								{
									id_failure = id_failure,
									id_user = first_user.id_user,
									id_status = 5,
									seq = first_user.seq,
									remark = remark,
									txn_datetime = DateTime.Now
								};
								
								_context.FailureRecordTXN.Add(nextTXN);
							}
							else
							{
								var pre_user = auth.Where(a => a.seq == auth_user.seq - 1).First();
								
								FailureRecordTXN nextTXN = new()
								{
									id_failure = id_failure,
									id_user = pre_user.id_user,
									id_status = 2,
									seq = pre_user.seq,
									remark = null,
									txn_datetime = DateTime.Now
								};
								
								_context.FailureRecordTXN.Add(nextTXN);
							}
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