using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FailureFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public FailureFileController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<FailureFile>>> GetFailureFile()
		{
			return await _context.FailureFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<FailureFile>> GetFailureFile(int id)
		{
			var data = await _context.FailureFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-failure-file-by-id")]
		public async Task<ActionResult<List<FailureFile>>> GetFailureFileById(int id)
		{
			var data = await _context.FailureFile.Where(a => a.id_failure == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFailureFile(int id, [FromForm] FailureFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			FailureFile? data = await _context.FailureFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/failure/"))
				{
					Directory.CreateDirectory("wwwroot/attach/failure/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/failure/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_failure = form.id_failure;
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
				if (!FailureFileExists(form.id))
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
		public async Task<ActionResult<FailureFile>> PostFailureFile([FromForm] FailureFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/failure/"))
			{
				Directory.CreateDirectory("wwwroot/attach/failure/");
			}
			FailureFile data = new FailureFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/failure/"))
				{
					Directory.CreateDirectory("wwwroot/attach/failure/");
				}
				
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/failure/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}

			data.id_failure = form.id_failure;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.FailureFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetFailureFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFailureFile(int id)
		{
			var data = await _context.FailureFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.FailureFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool FailureFileExists(int id)
		{
			return _context.FailureFile.Any(e => e.id == id);
		}
	}
}