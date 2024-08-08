using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMTagRegistrationLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMTagRegistrationLibraryController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMTagRegistrationLibrary>>> GetCMTagRegistrationLibrary()
		{
			var data = await _context.CMTagRegistrationLibrary.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMTagRegistrationLibrary>> GetCMTagRegistrationLibrary(int id)
		{
			var data = await _context.CMTagRegistrationLibrary.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMTagRegistrationLibrary(int id, [FromForm] CMTagRegistrationLibraryFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			CMTagRegistrationLibrary? data = await _context.CMTagRegistrationLibrary.FindAsync(form.id);

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
				data.id_system = form.id_system;
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
				if (!CMTagRegistrationLibraryExists(id))
				{
					return NotFound($"CMTagRegistrationLibrary with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMTagRegistrationLibrary>> PostCMTagRegistrationLibrary([FromForm] CMTagRegistrationLibraryFileUpload form)
		{
			CMTagRegistrationLibrary data = new CMTagRegistrationLibrary();
			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-er-probe-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-er-probe-lib/");
				}
				data.id_system = form.id_system;
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
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.CMTagRegistrationLibrary.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMTagRegistrationLibrary", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMTagRegistrationLibrary(int id)
		{
			var data = await _context.CMTagRegistrationLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.CMTagRegistrationLibrary.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMTagRegistrationLibraryExists(int id)
		{
			return _context.CMTagRegistrationLibrary.Any(e => e.id == id);
		}
	}
}