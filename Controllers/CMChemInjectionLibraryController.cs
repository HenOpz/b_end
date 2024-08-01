using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMChemInjectionLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMChemInjectionLibraryController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMChemInjectionLibrary
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMChemInjectionLibrary>>> GetCMChemInjectionLibrary()
		{
			return await _context.CMChemInjectionLibrary.ToListAsync();
		}

		// GET: api/CMChemInjectionLibrary/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMChemInjectionLibrary>> GetCMChemInjectionLibrary(int id)
		{
			var data = await _context.CMChemInjectionLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMChemInjectionLibrary/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMChemInjectionLibrary>>> GetCMChemInjectionLibraryByTag(int id_tag)
		{
			return await _context.CMChemInjectionLibrary.Where(b => b.id_tag == id_tag).ToListAsync();
		}

		// POST: api/CMChemInjectionLibrary
		[HttpPost]
		public async Task<IActionResult> PostCMChemInjectionLibrary([FromForm] CMChemInjectionLibraryUpload form)
		{
			string? path = null;
			var transaction = _context.Database.BeginTransaction();
			try
			{
				if (!Directory.Exists("wwwroot/attach/cm_ci/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm_ci/");
				}
				CMChemInjectionLibrary data = new CMChemInjectionLibrary();
				if (form.file != null)
				{
					string ext = Path.GetExtension(form.file.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
					path = "wwwroot/attach/cm_ci/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
					data.file_path = path;
					data.file_type = ext;
					data.file_name = file_name;
					using (var steam = new FileStream(path, FileMode.Create))
					{
						await form.file.CopyToAsync(steam);
					}
				}

				data.id_tag = form.id_tag;
				data.note = form.note;
				data.created_by = form.created_by;
				data.created_date = DateTime.Now;
				data.updated_by = form.updated_by;
				data.updated_date = DateTime.Now;

				_context.CMChemInjectionLibrary.Add(data);
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

		// PUT: api/CMChemInjectionLibrary/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMChemInjectionLibrary(int id, [FromForm] CMChemInjectionLibraryUpload form)
		{
			if (id != form.id) return BadRequest();

			CMChemInjectionLibrary? data = await _context.CMChemInjectionLibrary.FirstOrDefaultAsync(a => a.id == id);
			if (data == null) return NotFound("Library not found.");

			string? path = null;

			var transaction = _context.Database.BeginTransaction();
			try
			{
				if (!Directory.Exists("wwwroot/attach/cm_ci/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm_ci/");
				}
				
				if (form.file != null)
				{
					string? oldImgPath = data.file_path;
					string ext = Path.GetExtension(form.file.FileName);
					string file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
					path = "wwwroot/attach/cm_ci/" + file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
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
				
				data.id_tag = form.id_tag;
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

		// DELETE: api/CMChemInjectionLibrary/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMChemInjectionLibrary(int id)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				CMChemInjectionLibrary? data = await _context.CMChemInjectionLibrary.FirstOrDefaultAsync(a => a.id == id);
				if (data == null) return NotFound(new { message = "Libraly not found", });

				_context.CMChemInjectionLibrary.Remove(data);
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

		private bool CMChemInjectionLibraryExists(int id)
		{
			return _context.CMChemInjectionLibrary.Any(e => e.id == id);
		}
	}
}