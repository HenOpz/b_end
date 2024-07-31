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
		string sqlDataSource = Startup.ConnectionString;
		
		//Submit record
		[HttpPost]
		[Route("add-submit-txn")]
		public async Task<dynamic> AddSubmitTXN(int id_user,int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);
					
					if(lastTXN == null)
					{
						transaction.Complete();
						return NoContent();
					}
					else if(lastTXN.id_status == 6 || lastTXN.id_status == 4)
					{
						FailureRecordTXN data = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 5,
							remark = null,
							txn_datetime = DateTime.Now
						};
						_context.FailureRecordTXN.Add(data);
						await _context.SaveChangesAsync();
						
						var record = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == id_failure && a.is_active == true);
						if(record == null)
						{
							transaction.Dispose();
							return NoContent();
						}
						else
						{
							record.id_appr_status = 5;
							record.updated_by = id_user;
							record.updated_date = DateTime.Now;
							await _context.SaveChangesAsync();
						}
					}
					else
					{
						transaction.Complete();
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Submission is completed.");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		//Minor Approved record
		[HttpPost]
		[Route("add-minor-appr-txn")]
		public async Task<dynamic> AddMinorApproveTXN(int id_user,int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);
					
					if(lastTXN == null)
					{
						transaction.Complete();
						return NoContent();
					}
					else if(lastTXN.id_status == 5 || lastTXN.id_status == 3)
					{
						FailureRecordTXN data = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 2,
							remark = null,
							txn_datetime = DateTime.Now
						};
						_context.FailureRecordTXN.Add(data);
						await _context.SaveChangesAsync();
						
						var record = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == id_failure && a.is_active == true);
						if(record == null)
						{
							transaction.Dispose();
							return NoContent();
						}
						else
						{
							record.id_appr_status = 2;
							record.updated_by = id_user;
							record.updated_date = DateTime.Now;
							await _context.SaveChangesAsync();
						}
					}
					else
					{
						transaction.Complete();
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Minor Approval is completed.");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		//Major Approved record
		[HttpPost]
		[Route("add-major-appr-txn")]
		public async Task<dynamic> AddMajorApproveTXN(int id_user,int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);
					
					if(lastTXN == null)
					{
						transaction.Complete();
						return NoContent();
					}
					else if(lastTXN.id_status == 2)
					{
						FailureRecordTXN data = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 1,
							remark = null,
							txn_datetime = DateTime.Now
						};
						_context.FailureRecordTXN.Add(data);
						await _context.SaveChangesAsync();
						
						var record = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == id_failure && a.is_active == true);
						if(record == null)
						{
							transaction.Dispose();
							return NoContent();
						}
						else
						{
							record.id_appr_status = 1;
							record.updated_by = id_user;
							record.updated_date = DateTime.Now;
							await _context.SaveChangesAsync();
						}
					}
					else
					{
						transaction.Complete();
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Major Approval is completed.");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		//Minor Rejected record
		[HttpPost]
		[Route("add-minor-reject-txn")]
		public async Task<dynamic> AddMinorRejectTXN(int id_user,int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);
					
					if(lastTXN == null)
					{
						transaction.Complete();
						return NoContent();
					}
					else if(lastTXN.id_status == 5 || lastTXN.id_status == 3)
					{
						FailureRecordTXN data = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 4,
							remark = null,
							txn_datetime = DateTime.Now
						};
						_context.FailureRecordTXN.Add(data);
						await _context.SaveChangesAsync();
						
						var record = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == id_failure && a.is_active == true);
						if(record == null)
						{
							transaction.Dispose();
							return NoContent();
						}
						else
						{
							record.id_appr_status = 6;
							record.updated_by = id_user;
							record.updated_date = DateTime.Now;
							await _context.SaveChangesAsync();
						}
					}
					else
					{
						transaction.Complete();
						return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Minor Rejection is completed.");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		//Major Rejected record
		[HttpPost]
		[Route("add-major-reject-txn")]
		public async Task<dynamic> AddMajorRejectTXN(int id_user,int id_failure)
		{
			try
			{
				using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					var lastTXN = await _context.FailureRecordTXN.OrderByDescending(a => a.txn_datetime).FirstOrDefaultAsync(a => a.id_failure == id_failure);
					
					if(lastTXN == null)
					{
						transaction.Complete();
						return NoContent();
					}
					else if(lastTXN.id_status == 2)
					{
						FailureRecordTXN data = new()
						{
							id_failure = id_failure,
							id_user = id_user,
							id_status = 3,
							remark = null,
							txn_datetime = DateTime.Now
						};
						_context.FailureRecordTXN.Add(data);
						await _context.SaveChangesAsync();
						
						var record = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == id_failure && a.is_active == true);
						if(record == null)
						{
							transaction.Dispose();
							return NoContent();
						}
						else
						{
							record.id_appr_status = 5;
							record.updated_by = id_user;
							record.updated_date = DateTime.Now;
							await _context.SaveChangesAsync();
						}
					}
					else
					{
						transaction.Complete();
                        return Forbid("Transaction status is invalid.");
					}
					transaction.Complete();
				}
				return Ok("Major Rejection is completed.");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}