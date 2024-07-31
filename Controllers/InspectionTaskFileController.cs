using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InspectionTaskFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public InspectionTaskFileController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InspectionTaskFile>>> GetInspectionTaskFile()
		{
			return await _context.InspectionTaskFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<InspectionTaskFile>> GetInspectionTaskFile(int id)
		{
			var data = await _context.InspectionTaskFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-insp-task-file-by-id-task")]
		public async Task<ActionResult<List<InspectionTaskFile>>> GetInspectionTaskFileByIdTask(int id)
		{
			var data = await _context.InspectionTaskFile.Where(a => a.id_insp_task == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutInspectionTaskFile(int id, [FromForm] InspectionTaskFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			InspectionTaskFile? data = await _context.InspectionTaskFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/insp_task/"))
				{
					Directory.CreateDirectory("wwwroot/attach/insp_task/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/insp_task/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_insp_task = form.id_insp_task;
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
				if (!InspectionTaskFileExists(form.id))
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
		public async Task<ActionResult<InspectionTaskFile>> PostInspectionTaskFile([FromForm] InspectionTaskFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/insp_task/"))
			{
				Directory.CreateDirectory("wwwroot/attach/insp_task/");
			}
			InspectionTaskFile data = new InspectionTaskFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/insp_task/"))
				{
					Directory.CreateDirectory("wwwroot/attach/insp_task/");
				}
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/insp_task/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_insp_task = form.id_insp_task;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.note = form.note;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.InspectionTaskFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetInspectionTaskFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlockValveLibrary(int id)
		{
			var data = await _context.InspectionTaskFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.InspectionTaskFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool InspectionTaskFileExists(int id)
		{
			return _context.InspectionTaskFile.Any(e => e.id == id);
		}
	}
}