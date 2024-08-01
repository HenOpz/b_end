using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExInspectionChecklistController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ExInspectionChecklistController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ExInspectionChecklist>>> GetExInspectionChecklist()
		{
			return await _context.ExInspectionChecklist.ToListAsync();
		}

		[HttpGet]
		[Route("get-ex-insp-chk-list-insp-id")]
		public async Task<ActionResult<dynamic>> GetExInspectionChecklistResultByInsp(int id_insp_record)
		{
			try
			{
				List<ExInspectionChecklistShow> ExInspectionChecklist = await (from chk in _context.ExInspectionChecklist
																			   orderby chk.sort_no ascending
																			   select new ExInspectionChecklistShow
																			   {
																				   id = chk.id,
																				   checklist_no = chk.checklist_no,
																				   checklist_no_ref = chk.checklist_no_ref,
																				   checklist_content = chk.checklist_content,
																				   is_header = chk.is_header,
																				   is_subheader = chk.is_subheader,
																				   d_type = chk.d_type,
																				   c_type = chk.c_type,
																				   v_type = chk.v_type,
																				   sort_no = chk.sort_no,
																				   result = null
																			   }).ToListAsync();
				List<ExInspectionChecklistResult> ExInspectionChecklistResult = await (from a in _context.ExInspectionChecklistResult
																					   where a.id_inspection_record == id_insp_record
																					   select new ExInspectionChecklistResult
																					   {
																						   id = a.id,
																						   id_chk = a.id_chk,
																						   id_inspection_record = a.id_inspection_record,
																						   id_result = a.id_result,
																						   comment = a.comment
																					   }).ToListAsync();
				foreach (ExInspectionChecklistShow i in ExInspectionChecklist)
				{
					i.result = ExInspectionChecklistResult.FirstOrDefault(a => a.id_chk == i.id);
				}

				return ExInspectionChecklist;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("add-all-ex-insp-chk-list")]
		public async Task<ActionResult<dynamic>> PostAllExInspectionChecklistResult(int id_insp_record)
		{
			using (var transactions = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var chkList = await _context.ExInspectionChecklist.ToListAsync();
					var result = chkList.Select(a => new ExInspectionChecklistResult
					{
						id_chk = a.id,
						id_inspection_record = id_insp_record,
						id_result = null,
						comment = null
					});
					await _context.ExInspectionChecklistResult.AddRangeAsync(result);

					await _context.SaveChangesAsync();
					transactions.Complete();
				}
				catch (Exception ex)
				{
					transactions.Dispose();
					return BadRequest(ex.Message);
				}
			}

			return CreatedAtAction("GetExInspectionChecklistResultByInsp", new { id = id_insp_record });
		}

		[HttpPut]
		[Route("edit-ex-insp-chk-list-result")]
		public async Task<IActionResult> PutExInspectionChecklistResult(ExInspectionChecklistResult form)
		{
			try
			{
				var data = await _context.ExInspectionChecklistResult.FirstOrDefaultAsync(a => a.id == form.id);
				if (data == null) { return NotFound("Checklist result not found"); }

				data.id_result = form.id_result;
				data.comment = form.comment;
				await _context.SaveChangesAsync();

				return Ok("Update successful");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete]
		[Route("delete-ex-insp-chk-list")]
		public async Task<IActionResult> DeleteExInspectionChecklistResult(int id_insp_record)
		{
			try
			{
				var Resultdata = await _context.ExInspectionChecklistResult.Where(a => a.id_inspection_record == id_insp_record).ToListAsync();
				if ( Resultdata.Count == 0 ) { return NotFound("Checklist result not found"); }
				
				_context.ExInspectionChecklistResult.RemoveRange(Resultdata);
				await _context.SaveChangesAsync();
				
				return Ok("Delete successful");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}