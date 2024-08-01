using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExInspectionPictureLogController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ExInspectionPictureLogController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetExInspectionPictureLog()
		{
			var data = await GetExInspectionPictureLogQuery().ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetExInspectionPictureLog(int id)
		{
			var data = await GetExInspectionPictureLogQuery().FirstOrDefaultAsync(a => a.id == id);

			if (data == null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		[Route("get-ex-insp-pic-log-by-id-insp")]
		public async Task<IActionResult> GetExInspectionPictureLogByIdInsp(int id_insp)
		{
			var data = await GetExInspectionPictureLogQuery().Where(a => a.id_inspection_record == id_insp).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpPut]
		[Route("edit-ex-insp-pic-log")]
		public async Task<IActionResult> EditExInspectionPictureLog(int id, [FromForm] ExInspectionPictureLogUpload form)
		{
			if (id != form.id) return BadRequest();

			ExInspectionPictureLog? data = await _context.ExInspectionPictureLog.FirstOrDefaultAsync(a => a.id == id);
			if (data == null) return NotFound("Picture log not found.");

			string? path_1 = null;
			string? path_2 = null;

			var transaction = _context.Database.BeginTransaction();
			try
			{

				if (!Directory.Exists("wwwroot/attach/expic/"))
				{
					Directory.CreateDirectory("wwwroot/attach/expic/");
				}

				if (form.file_1 != null)
				{
					string? oldImgPath = data.pic_path_1;
					string ext = Path.GetExtension(form.file_1.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file_1.FileName);
					path_1 = "wwwroot/attach/expic/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.pic_path_1 = path_1;
					using (var steam = new FileStream(path_1, FileMode.Create))
					{
						await form.file_1.CopyToAsync(steam);
					}

					if (oldImgPath != null)
					{
						if (System.IO.File.Exists(oldImgPath))
						{
							System.IO.File.Delete(oldImgPath);
						}
					}
				}
				if (form.file_2 != null)
				{
					string? oldImgPath = data.pic_path_2;
					string ext = Path.GetExtension(form.file_2.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file_2.FileName);
					path_2 = "wwwroot/attach/expic/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.pic_path_2 = path_2;
					using (var steam = new FileStream(path_2, FileMode.Create))
					{
						await form.file_2.CopyToAsync(steam);
					}

					if (oldImgPath != null)
					{
						if (System.IO.File.Exists(oldImgPath))
						{
							System.IO.File.Delete(oldImgPath);
						}
					}
				}

				data.id_inspection_record = form.id_inspection_record;
				data.id_chk_result = form.id_chk_result;
				data.finding = form.finding;
				data.recommendation = form.recommendation;
				data.id_finding_status = form.id_finding_status;
				data.deenergize = form.deenergize;
				data.quick_fix = form.quick_fix;
				data.interim_measure = form.interim_measure;
				data.interim_measure_validity = form.interim_measure_validity;
				data.finding_comp_status = form.finding_comp_status;
				data.created_by = form.created_by;
				data.created_date = form.created_date;
				data.updated_by = form.updated_by;
				data.updated_date = DateTime.Now;

				await _context.SaveChangesAsync();
				transaction.Commit();
				return Ok(data);
			}
			catch (Exception ex)
			{
				try
				{
					if (path_1 != null)
					{
						if (System.IO.File.Exists(path_1))
						{
							System.IO.File.Delete(path_1);
						}
					}
					if (path_2 != null)
					{
						if (System.IO.File.Exists(path_2))
						{
							System.IO.File.Delete(path_2);
						}
					}

					transaction.Rollback();
					return BadRequest(new { message = ex.Message });
				}
				catch (Exception exc)
				{
					transaction.Rollback();
					return BadRequest(new { message = exc.Message });
				}
			}
		}

		[HttpPost]
		[Route("add-ex-insp-pic-log")]
		public async Task<IActionResult> AddExInspectionPictureLog([FromForm] ExInspectionPictureLogUpload form)
		{
			string? path_1 = null;
			string? path_2 = null;
			var transaction = _context.Database.BeginTransaction();
			try
			{

				if (!Directory.Exists("wwwroot/attach/expic/"))
				{
					Directory.CreateDirectory("wwwroot/attach/expic/");
				}
				ExInspectionPictureLog data = new ExInspectionPictureLog();
				if (form.file_1 != null)
				{
					string ext = Path.GetExtension(form.file_1.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file_1.FileName);
					path_1 = "wwwroot/attach/expic/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.pic_path_1 = path_1;
					using (var steam = new FileStream(path_1, FileMode.Create))
					{
						await form.file_1.CopyToAsync(steam);
					}
				}
				if (form.file_2 != null)
				{
					string ext = Path.GetExtension(form.file_2.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file_2.FileName);
					path_2 = "wwwroot/attach/expic/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.pic_path_2 = path_2;
					using (var steam = new FileStream(path_2, FileMode.Create))
					{
						await form.file_2.CopyToAsync(steam);
					}
				}

				data.id_inspection_record = form.id_inspection_record;
				data.id_chk_result = form.id_chk_result;
				data.finding = form.finding;
				data.recommendation = form.recommendation;
				data.id_finding_status = form.id_finding_status;
				data.deenergize = form.deenergize;
				data.quick_fix = form.quick_fix;
				data.interim_measure = form.interim_measure;
				data.interim_measure_validity = form.interim_measure_validity;
				data.finding_comp_status = form.finding_comp_status;
				data.created_by = form.created_by;
				data.created_date = DateTime.Now;
				data.updated_by = form.updated_by;
				data.updated_date = DateTime.Now;

				_context.ExInspectionPictureLog.Add(data);
				await _context.SaveChangesAsync();

				transaction.Commit();

				return Ok(data);
			}
			catch (Exception ex)
			{
				try
				{
					if (path_1 != null)
					{
						if (System.IO.File.Exists(path_1))
						{
							System.IO.File.Delete(path_1);
						}
					}
					if (path_2 != null)
					{
						if (System.IO.File.Exists(path_2))
						{
							System.IO.File.Delete(path_2);
						}
					}

					transaction.Rollback();
					return BadRequest(new { message = ex.Message });
				}
				catch (Exception exc)
				{
					transaction.Rollback();
					return BadRequest(new { message = exc.Message });
				}
			}
		}

		[HttpDelete]
		[Route("delete-ex-insp-pic-log")]
		public async Task<IActionResult> DeleteExInspectionPictureLog(int id)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				ExInspectionPictureLog? data = await _context.ExInspectionPictureLog.FirstOrDefaultAsync(a => a.id == id);
				if (data == null) return NotFound(new { message = "Picture log not found", });

				_context.ExInspectionPictureLog.Remove(data);
				await _context.SaveChangesAsync();
				transaction.Commit();

				if (System.IO.File.Exists(data.pic_path_1))
				{
					System.IO.File.Delete(data.pic_path_1);
				}
				if (System.IO.File.Exists(data.pic_path_2))
				{
					System.IO.File.Delete(data.pic_path_2);
				}
				
				return Ok();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}

		private IQueryable<ExInspectionPictureLogView> GetExInspectionPictureLogQuery()
		{
			return from pl in _context.ExInspectionPictureLog
				   join ir in _context.ExInspectionRecord on pl.id_inspection_record equals ir.id into plir
				   from plirResult in plir.DefaultIfEmpty()

				   join ckr in _context.ExInspectionChecklistResult on pl.id_chk_result equals ckr.id into plckr
				   from plckrResult in plckr.DefaultIfEmpty()
				   join ck in _context.ExInspectionChecklist on plckrResult.id_chk equals ck.id into ckc
				   from ckcResult in ckc.DefaultIfEmpty()

				   join stt in _context.MdExInspectionPictureLogStatus on pl.id_finding_status equals stt.id into plstt
				   from plsttResult in plstt.DefaultIfEmpty()

				   join cb in _context.User on pl.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on pl.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   select new ExInspectionPictureLogView
				   {
					   id = pl.id,
					   id_inspection_record = pl.id_inspection_record,
					   inspection_date = plirResult.inspection_date,
					   id_chk_result = pl.id_chk_result,
					   checklist_no_ref = ckcResult.checklist_no_ref,
					   pic_path_1 = pl.pic_path_1,
					   pic_path_2 = pl.pic_path_2,
					   finding = pl.finding,
					   recommendation = pl.recommendation,
					   id_finding_status = pl.id_finding_status,
					   finding_status = plsttResult.code,
					   deenergize = pl.deenergize,
					   quick_fix = pl.quick_fix,
					   interim_measure = pl.interim_measure,
					   interim_measure_validity = pl.interim_measure_validity,
					   finding_comp_status = pl.finding_comp_status,
					   created_by = pl.created_by,
					   created_by_name = icbResult.name,
					   created_date = pl.created_date,
					   updated_by = pl.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = pl.updated_date
				   };

		}
	}
}