using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMERProbeLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMERProbeLibraryController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMERProbeLibrary>>> GetCMERProbeLibrary()
		{
			var data = await _context.CMERProbeLibrary.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMERProbeLibrary>> GetCMERProbeLibrary(int id)
		{
			var data = await _context.CMERProbeLibrary.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMERProbeLibrary>>> GetCMERProbeLibraryByTag(int id_tag)
		{
			return await _context.CMERProbeLibrary.Where(b => b.id_tag == id_tag).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMERProbeLibrary(int id, [FromForm] CMERProbeLibraryFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			CMERProbeLibrary? data = await _context.CMERProbeLibrary.FindAsync(form.id);

			if (data == null) return NotFound();

			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-er-probe-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-er-probe-lib/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-er-probe-lib/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_ext;
			}
			data.id_tag = form.id_tag;
			data.note = form.note;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMERProbeLibraryExists(id))
				{
					return NotFound($"CMERProbeLibrary with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMERProbeLibrary>> PostCMERProbeLibrary([FromForm] CMERProbeLibraryFileUpload form)
		{
			CMERProbeLibrary data = new CMERProbeLibrary();
			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-er-probe-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-er-probe-lib/");
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-er-probe-lib/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_ext;
			}
			data.id_tag = form.id_tag;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.CMERProbeLibrary.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMERProbeLibrary", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMERProbeLibrary(int id)
		{
			var data = await _context.CMERProbeLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.CMERProbeLibrary.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMERProbeLibraryExists(int id)
		{
			return _context.CMERProbeLibrary.Any(e => e.id == id);
		}
	}
}