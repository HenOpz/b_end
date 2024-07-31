using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExInspectionRegisterInfoFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ExInspectionRegisterInfoFileController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ExInspectionRegisterInfoFile>>> GetExInspectionRegisterInfoFile()
		{
			return await _context.ExInspectionRegisterInfoFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<ExInspectionRegisterInfoFile>> GetExInspectionRegisterInfoFile(int id)
		{
			var data = await _context.ExInspectionRegisterInfoFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-ex-inspection-register-info-by-id")]
		public async Task<ActionResult<List<ExInspectionRegisterInfoFile>>> GetExInspectionRegisterInfoFileById(int id)
		{
			var data = await _context.ExInspectionRegisterInfoFile.Where(a => a.id_info == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutExInspectionRegisterInfoFile(int id, [FromForm] ExInspectionRegisterInfoFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			ExInspectionRegisterInfoFile? data = await _context.ExInspectionRegisterInfoFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/exfile/"))
				{
					Directory.CreateDirectory("wwwroot/attach/exfile/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/exfile/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_info = form.id_info;
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
				if (!ExInspectionRegisterInfoFileExists(form.id))
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
		public async Task<ActionResult<ExInspectionRegisterInfoFile>> PostExInspectionRegisterInfoFile([FromForm] ExInspectionRegisterInfoFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/exfile/"))
			{
				Directory.CreateDirectory("wwwroot/attach/exfile/");
			}
			ExInspectionRegisterInfoFile data = new ExInspectionRegisterInfoFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/exfile/"))
				{
					Directory.CreateDirectory("wwwroot/attach/exfile/");
				}
				
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/exfile/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}

			data.id_info = form.id_info;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.ExInspectionRegisterInfoFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetExInspectionRegisterInfoFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlockValveLibrary(int id)
		{
			var data = await _context.ExInspectionRegisterInfoFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.ExInspectionRegisterInfoFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool ExInspectionRegisterInfoFileExists(int id)
		{
			return _context.ExInspectionRegisterInfoFile.Any(e => e.id == id);
		}
	}
}