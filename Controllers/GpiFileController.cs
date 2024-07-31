using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GpiFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public GpiFileController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<GpiFile>>> GetGpiFile()
		{
			return await _context.GpiFile.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<GpiFile>> GetGpiFile(int id)
		{
			var data = await _context.GpiFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet]
		[Route("get-gpi-file-by-id")]
		public async Task<ActionResult<List<GpiFile>>> GetGpiFileById(int id)
		{
			var data = await _context.GpiFile.Where(a => a.id_gpi_record == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutGpiFile(int id, [FromForm] GpiFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			GpiFile? data = await _context.GpiFile.FindAsync(form.id);

			if (data == null) return NotFound();

			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/gpi/"))
				{
					Directory.CreateDirectory("wwwroot/attach/gpi/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/gpi/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_extension = file_ext;
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
				if (!GpiFileExists(form.id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<GpiFile>> PostGpiFile([FromForm] GpiFileUpload form)
		{
			GpiFile data = new GpiFile();
			string path = "";
			string file_name = "";
			string file_ext = "";

			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/gpi/"))
				{
					Directory.CreateDirectory("wwwroot/attach/gpi/");
				}
				
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/gpi/" + file_name;
				data.note = form.note;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_extension = file_ext;
				data.id_gpi_record = form.id_gpi_record;
			}
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.GpiFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetGpiFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGpiFile(int id)
		{
			var data = await _context.GpiFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.GpiFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool GpiFileExists(int id)
		{
			return _context.GpiFile.Any(e => e.id == id);
		}
	}
}