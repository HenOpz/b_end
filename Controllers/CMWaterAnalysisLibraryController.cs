using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWaterAnalysisLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWaterAnalysisLibraryController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMWaterAnalysisLibrary>>> GetCMWaterAnalysisLibrary()
		{
			var data = await _context.CMWaterAnalysisLibrary.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMWaterAnalysisLibrary>> GetCMWaterAnalysisLibrary(int id)
		{
			var data = await _context.CMWaterAnalysisLibrary.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWaterAnalysisLibrary(int id, [FromForm] CMWaterAnalysisLibraryFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			CMWaterAnalysisLibrary? data = await _context.CMWaterAnalysisLibrary.FindAsync(form.id);

			if (data == null) return NotFound();

			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-water-ana-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-water-ana-lib/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-water-ana-lib/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_ext;
			}
			data.id_system = form.id_system;
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
				if (!CMWaterAnalysisLibraryExists(id))
				{
					return NotFound($"CMWaterAnalysisLibrary with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWaterAnalysisLibrary>> PostCMWaterAnalysisLibrary([FromForm] CMWaterAnalysisLibraryFileUpload form)
		{
			CMWaterAnalysisLibrary data = new CMWaterAnalysisLibrary();
			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-water-ana-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-water-ana-lib/");
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-water-ana-lib/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_ext;
			}
			data.id_system = form.id_system;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.CMWaterAnalysisLibrary.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWaterAnalysisLibrary", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMWaterAnalysisLibrary(int id)
		{
			var data = await _context.CMWaterAnalysisLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.CMWaterAnalysisLibrary.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMWaterAnalysisLibraryExists(int id)
		{
			return _context.CMWaterAnalysisLibrary.Any(e => e.id == id);
		}
	}
}