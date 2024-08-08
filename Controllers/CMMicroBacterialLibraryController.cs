using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMMicroBacterialLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMMicroBacterialLibraryController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMMicroBacterialLibrary
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMMicroBacterialLibrary>>> GetCMMicroBacterialLibrary()
		{
			return await _context.CMMicroBacterialLibrary.ToListAsync();
		}

		// GET: api/CMMicroBacterialLibrary/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMMicroBacterialLibrary>> GetCMMicroBacterialLibrary(int id)
		{
			var data = await _context.CMMicroBacterialLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// POST: api/CMMicroBacterialLibrary
		[HttpPost]
		public async Task<IActionResult> PostCMMicroBacterialLibrary([FromForm] CMMicroBacterialLibraryUpload form)
		{
			string? path = null;
			var transaction = _context.Database.BeginTransaction();
			try
			{
				if (!Directory.Exists("wwwroot/attach/cm_micro_bact/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm_micro_bact/");
				}
				CMMicroBacterialLibrary data = new CMMicroBacterialLibrary();
				if (form.file != null)
				{
					string ext = Path.GetExtension(form.file.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
					path = "wwwroot/attach/cm_micro_bact/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.file_path = path;
					data.file_type = ext;
					data.file_name = file_name;
					using (var steam = new FileStream(path, FileMode.Create))
					{
						await form.file.CopyToAsync(steam);
					}
				}
				data.id_system = form.id_system;
				data.note = form.note;
				data.created_by = form.created_by;
				data.created_date = DateTime.Now;
				data.updated_by = form.updated_by;
				data.updated_date = DateTime.Now;

				_context.CMMicroBacterialLibrary.Add(data);
				await _context.SaveChangesAsync();

				transaction.Commit();

				return Ok(data);
			}
			catch (Exception ex)
			{
				try
				{
					if (path != null)
					{
						if (System.IO.File.Exists(path))
						{
							System.IO.File.Delete(path);
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

		// PUT: api/CMMicroBacterialLibrary/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMMicroBacterialLibrary(int id, [FromForm] CMMicroBacterialLibraryUpload form)
		{
			if (id != form.id) return BadRequest();

			CMMicroBacterialLibrary? data = await _context.CMMicroBacterialLibrary.FirstOrDefaultAsync(a => a.id == id);
			if (data == null) return NotFound("Library not found.");

			string? path = null;

			var transaction = _context.Database.BeginTransaction();
			try
			{
				if (!Directory.Exists("wwwroot/attach/cm_micro_bact/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm_micro_bact/");
				}
				
				if (form.file != null)
				{
					string? oldImgPath = data.file_path;
					string ext = Path.GetExtension(form.file.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
					path = "wwwroot/attach/cm_micro_bact/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.file_path = path;
					data.file_type = ext;
					data.file_name = file_name;
					using (var steam = new FileStream(path, FileMode.Create))
					{
						await form.file.CopyToAsync(steam);
					}

					if (oldImgPath != null)
					{
						if (System.IO.File.Exists(oldImgPath))
						{
							System.IO.File.Delete(oldImgPath);
						}
					}
				}
				data.id_system = form.id_system;
				data.note = form.note;
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
					if (path != null)
					{
						if (System.IO.File.Exists(path))
						{
							System.IO.File.Delete(path);
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

		// DELETE: api/CMMicroBacterialLibrary/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMMicroBacterialLibrary(int id)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				CMMicroBacterialLibrary? data = await _context.CMMicroBacterialLibrary.FirstOrDefaultAsync(a => a.id == id);
				if (data == null) return NotFound(new { message = "Libraly not found", });

				_context.CMMicroBacterialLibrary.Remove(data);
				await _context.SaveChangesAsync();
				transaction.Commit();

				if (System.IO.File.Exists(data.file_path))
				{
					System.IO.File.Delete(data.file_path);
				}
				
				return Ok();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}

		private bool CMMicroBacterialLibraryExists(int id)
		{
			return _context.CMMicroBacterialLibrary.Any(e => e.id == id);
		}
	}
}