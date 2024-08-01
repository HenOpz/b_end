using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMCorrosionCouponPictureLogController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMCorrosionCouponPictureLogController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMCorrosionCouponPictureLog>>> GetCMCorrosionCouponPictureLog()
		{
			var data = await _context.CMCorrosionCouponPictureLog.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMCorrosionCouponPictureLog>> GetCMCorrosionCouponPictureLog(int id)
		{
			var data = await _context.CMCorrosionCouponPictureLog.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_record}")]
		public async Task<ActionResult<IEnumerable<CMCorrosionCouponPictureLog>>> GetCMCorrosionCouponPictureLogByTag(int id_record)
		{
			return await _context.CMCorrosionCouponPictureLog.Where(b => b.id_record == id_record).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMCorrosionCouponPictureLog(int id, [FromForm] CMCorrosionCouponPictureLogFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			CMCorrosionCouponPictureLog? data = await _context.CMCorrosionCouponPictureLog.FindAsync(form.id);

			if (data == null) return NotFound();

			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-corrosion-coupon-pic-log/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-corrosion-coupon-pic-log/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-corrosion-coupon-pic-log/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
			}
			data.id_record = form.id_record;
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
				if (!CMCorrosionCouponPictureLogExists(id))
				{
					return NotFound($"CMCorrosionCouponPictureLog with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMCorrosionCouponPictureLog>> PostCMCorrosionCouponPictureLog([FromForm] CMCorrosionCouponPictureLogFileUpload form)
		{
			CMCorrosionCouponPictureLog data = new CMCorrosionCouponPictureLog();
			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-corrosion-coupon-pic-log/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-corrosion-coupon-pic-log/");
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-corrosion-coupon-pic-log/" + file_name;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await form.file.CopyToAsync(stream);
				}
				data.file_path = path;
			}
			data.id_record = form.id_record;
			data.note = form.note;
			data.created_by = form.created_by;
			data.created_date = DateTime.Now;
			data.updated_by = form.updated_by;
			data.updated_date = DateTime.Now;

			_context.CMCorrosionCouponPictureLog.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMCorrosionCouponPictureLog", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMCorrosionCouponPictureLog(int id)
		{
			var data = await _context.CMCorrosionCouponPictureLog.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.CMCorrosionCouponPictureLog.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMCorrosionCouponPictureLogExists(int id)
		{
			return _context.CMCorrosionCouponPictureLog.Any(e => e.id == id);
		}
	}
}