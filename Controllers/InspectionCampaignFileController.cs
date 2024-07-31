using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InspectionCampaignFileController : ControllerBase
	{
		private readonly MainDbContext _context;
		public InspectionCampaignFileController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InspectionCampaignFile>>> GetInspectionCampaignFile()
		{
			return await _context.InspectionCampaignFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<InspectionCampaignFile>> GetInspectionCampaignFile(int id)
		{
			var data = await _context.InspectionCampaignFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-insp-campaign-file-by-id-task")]
		public async Task<ActionResult<List<InspectionCampaignFile>>> GetInspectionCampaignFileByIdTask(int id)
		{
			var data = await _context.InspectionCampaignFile.Where(a => a.id_insp_campaign == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutInspectionCampaignFile(int id, [FromForm] InspectionCampaignFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			InspectionCampaignFile? data = await _context.InspectionCampaignFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/insp_campaign/"))
				{
					Directory.CreateDirectory("wwwroot/attach/insp_campaign/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type ;
				path = "wwwroot/attach/insp_campaign/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_insp_campaign = form.id_insp_campaign;
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
				if (!InspectionCampaignFileExists(form.id))
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
		public async Task<ActionResult<InspectionCampaignFile>> PostInspectionCampaignFile([FromForm] InspectionCampaignFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/insp_campaign/"))
			{
				Directory.CreateDirectory("wwwroot/attach/insp_campaign/");
			}
			InspectionCampaignFile data = new InspectionCampaignFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/insp_campaign/"))
				{
					Directory.CreateDirectory("wwwroot/attach/insp_campaign/");
				}
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/insp_campaign/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}

			data.id_insp_campaign = form.id_insp_campaign;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.note = form.note;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.InspectionCampaignFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetInspectionCampaignFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlockValveLibrary(int id)
		{
			var data = await _context.InspectionCampaignFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.InspectionCampaignFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool InspectionCampaignFileExists(int id)
		{
			return _context.InspectionCampaignFile.Any(e => e.id == id);
		}
	}
}