using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RectificationCampaignFileController : ControllerBase
    {
        private readonly MainDbContext _context;
		public RectificationCampaignFileController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RectificationCampaignFile>>> GetRectificationCampaignFile()
		{
			return await _context.RectificationCampaignFile.ToListAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<RectificationCampaignFile>> GetRectificationCampaignFile(int id)
		{
			var data = await _context.RectificationCampaignFile.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-recti-campaign-file-by-id-task")]
		public async Task<ActionResult<List<RectificationCampaignFile>>> GetRectificationCampaignFileByIdTask(int id)
		{
			var data = await _context.RectificationCampaignFile.Where(a => a.id_recti_campaign == id).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutRectificationCampaignFile(int id, [FromForm] RectificationCampaignFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			RectificationCampaignFile? data = await _context.RectificationCampaignFile.FindAsync(form.id);

			if (data == null) { return NotFound(); }

			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/recti_campaign/"))
				{
					Directory.CreateDirectory("wwwroot/attach/recti_campaign/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
			    data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_type;
				path = "wwwroot/attach/recti_campaign/" + file_name;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
				data.file_type = file_type;
			}
			data.id_recti_campaign = form.id_recti_campaign;
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
				if (!RectificationCampaignFileExists(form.id))
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
		public async Task<ActionResult<RectificationCampaignFile>> PostRectificationCampaignFile([FromForm] RectificationCampaignFileUpload form)
		{
			//create directory if its not exist
			if (!Directory.Exists("wwwroot/attach/recti_campaign/"))
			{
				Directory.CreateDirectory("wwwroot/attach/recti_campaign/");
			}
			RectificationCampaignFile data = new RectificationCampaignFile();
			string path = "";
			string file_name = "";
			string file_type = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/recti_campaign/"))
				{
					Directory.CreateDirectory("wwwroot/attach/recti_campaign/");
				}
				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_type = Path.GetExtension(form.file.FileName);
			    data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_name;
				path = "wwwroot/attach/recti_campaign/" + file_type;
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
                data.file_path = path;
                data.file_type = file_type;
			}

			data.id_recti_campaign = form.id_recti_campaign;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.note = form.note;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.RectificationCampaignFile.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetRectificationCampaignFile", new { id = data.id }, data);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlockValveLibrary(int id)
		{
			var data = await _context.RectificationCampaignFile.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.RectificationCampaignFile.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		
		private bool RectificationCampaignFileExists(int id)
		{
			return _context.RectificationCampaignFile.Any(e => e.id == id);
		}
    }
}