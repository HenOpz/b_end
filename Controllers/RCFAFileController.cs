using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RCFAFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public RCFAFileController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RCFAFile>>> GetRCFAFile()
		{
			return await _context.RCFAFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<RCFAFile>> GetRCFAFile(int id)
		{
			var data = await _context.RCFAFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-rcfa-file-by-id")]
		public async Task<ActionResult<List<RCFAFile>>> GetRCFAFileById(int id)
		{
			var data = await _context.RCFAFile.Where(a => a.id_failure == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutRCFAFile(int id, [FromForm] RCFAFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			RCFAFile? data = await _context.RCFAFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/rcfa/"))
				{
					Directory.CreateDirectory("wwwroot/attach/rcfa/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/rcfa/" + file_name;
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
				if (!RCFAFileExists(form.id))
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
		public async Task<ActionResult<RCFAFile>> PostRCFAFile([FromForm] RCFAFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/rcfa/"))
			{
				Directory.CreateDirectory("wwwroot/attach/rcfa/");
			}
			
			var fl = await _context.FailureRecord.FirstOrDefaultAsync(a => a.id == form.id_failure);
			
			if (fl == null) return BadRequest("Failure Record was not found!");
			if (fl.is_rcfa == null || fl.is_rcfa == false) return BadRequest("This Failure Record can not create RCFA action!");
			
			RCFAFile data = new RCFAFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/rcfa/"))
				{
					Directory.CreateDirectory("wwwroot/attach/rcfa/");
				}
				
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/rcfa/" + file_name;
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

			_context.RCFAFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetRCFAFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRCFAFile(int id)
		{
			var data = await _context.RCFAFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.RCFAFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool RCFAFileExists(int id)
		{
			return _context.RCFAFile.Any(e => e.id == id);
		}
	}
}