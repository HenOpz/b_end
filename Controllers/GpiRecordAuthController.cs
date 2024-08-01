using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GpiRecordAuthController : ControllerBase
	{
		private readonly MainDbContext _context;
		public GpiRecordAuthController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpGet]
		[Route("gpi-record-auth-list")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> ListGpiRecordAuth()
		{
			var data = await GetGpiRecordAuthQuery().ToListAsync();
			if (data.Count == 0) return NotFound(new { message = "Gpi record Auth not found." });
			return Ok(data);
		}
		
		[HttpGet]
		[Route("gpi-record-by-id")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> GetGpiRecordAuthById(int id)
		{
			var data = await GetGpiRecordAuthQuery().FirstOrDefaultAsync(a => a.id == id);
			if (data == null) return NotFound(new { message = "Gpi record Auth not found." });
			return Ok(data);
		}

		[HttpGet]
		[Route("gpi-record-by-id-discipline")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> GetGpiRecordAuthByIdDiscipline(int id_discipline)
		{
			var data = await GetGpiRecordAuthQuery().Where(a => a.id_discipline == id_discipline).ToListAsync();
			if (data.Count == 0) return NotFound(new { message = "Gpi record Auth not found." });
			return Ok(data);
		}
		
		[HttpPost]
		[Route("add-gpi-record-auth")]
		public async Task<ActionResult<GpiRecordAuth>> AddGpiRecordAuth(GpiRecordAuth form)
		{
			_context.GpiRecordAuth.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetGpiRecordAuthById", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-gpi-record-auth")]
		public async Task<IActionResult> EditGpiRecordAuth(int id, GpiRecordAuth form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!GpiRecordAuthExists(id))
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

		[HttpDelete]
		[Route("delete-gpi-record-auth")]
		public async Task<IActionResult> DeleteGpiRecordAuth(int id)
		{
			var data = await _context.GpiRecordAuth.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.GpiRecordAuth.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool GpiRecordAuthExists(int id)
		{
			return _context.GpiRecordAuth.Any(e => e.id == id);
		}
		
		private IQueryable<GpiRecordAuthView> GetGpiRecordAuthQuery()
		{
			return from au in _context.GpiRecordAuth
					join wg in _context.MdGpiDiscipline on au.id_discipline equals wg.id into auwg
					from auwgResult in auwg.DefaultIfEmpty()
					
					join us in _context.User on au.id_user equals us.id into auus
					from auusResult in auus.DefaultIfEmpty()
					
					orderby au.seq
					
					select new GpiRecordAuthView
					{
						id = au.id,
						id_discipline = au.id_discipline,
						discipline = auwgResult.code,
						authorized_name = au.authorized_name,
						seq = au.seq,
						id_user = au.id_user,
						name = auusResult.name,
					};
		}
	}
}