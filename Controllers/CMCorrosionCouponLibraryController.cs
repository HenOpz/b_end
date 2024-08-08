using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMCorrosionCouponLibraryController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMCorrosionCouponLibraryController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMCorrosionCouponLibrary>>> GetCMCorrosionCouponLibrary()
		{
			var data = await _context.CMCorrosionCouponLibrary.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMCorrosionCouponLibrary>> GetCMCorrosionCouponLibrary(int id)
		{
			var data = await _context.CMCorrosionCouponLibrary.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMCorrosionCouponLibrary(int id, [FromForm] CMCorrosionCouponLibraryFileUpload form)
		{
			if (id != form.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			CMCorrosionCouponLibrary? data = await _context.CMCorrosionCouponLibrary.FindAsync(form.id);

			if (data == null) return NotFound();

			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-corrosion-coupon-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-corrosion-coupon-lib/");
				}
				if (System.IO.File.Exists(form.file_path))
				{
					System.IO.File.Delete(form.file_path);
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-corrosion-coupon-lib/" + file_name;

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
				if (!CMCorrosionCouponLibraryExists(id))
				{
					return NotFound($"CMCorrosionCouponLibrary with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMCorrosionCouponLibrary>> PostCMCorrosionCouponLibrary([FromForm] CMCorrosionCouponLibraryFileUpload form)
		{
			CMCorrosionCouponLibrary data = new CMCorrosionCouponLibrary();
			string path = "";
			string file_name = "";
			string file_ext = "";
			if (form.file != null)
			{
				if (!Directory.Exists("wwwroot/attach/cm-corrosion-coupon-lib/"))
				{
					Directory.CreateDirectory("wwwroot/attach/cm-corrosion-coupon-lib/");
				}

				file_name = Path.GetFileNameWithoutExtension(form.file.FileName);
				file_ext = Path.GetExtension(form.file.FileName);
				data.file_name = file_name + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
				file_name = data.file_name + file_ext;
				path = "wwwroot/attach/cm-corrosion-coupon-lib/" + file_name;

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

			_context.CMCorrosionCouponLibrary.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMCorrosionCouponLibrary", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMCorrosionCouponLibrary(int id)
		{
			var data = await _context.CMCorrosionCouponLibrary.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			if (System.IO.File.Exists(data.file_path))
			{
				System.IO.File.Delete(data.file_path);
			}

			_context.CMCorrosionCouponLibrary.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMCorrosionCouponLibraryExists(int id)
		{
			return _context.CMCorrosionCouponLibrary.Any(e => e.id == id);
		}
	}
}