using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FailureRecordAuthController : ControllerBase
	{
		private readonly MainDbContext _context;
		public FailureRecordAuthController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpGet]
		[Route("failure-record-auth-list")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> ListFailureRecordAuth()
		{
			var data = await GetFailureRecordAuthQuery().ToListAsync();
			if (data.Count == 0) return NotFound(new { message = "Failure record Auth not found." });
			return Ok(data);
		}
		
		[HttpGet]
		[Route("failure-record-by-id")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> GetFailureRecordAuthById(int id)
		{
			var data = await GetFailureRecordAuthQuery().FirstOrDefaultAsync(a => a.id == id);
			if (data == null) return NotFound(new { message = "Failure record Auth not found." });
			return Ok(data);
		}

		[HttpGet]
		[Route("failure-record-by-id-work-group")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> GetFailureRecordAuthByIdWorkGroup(int id_work_group)
		{
			var data = await GetFailureRecordAuthQuery().Where(a => a.id_work_group == id_work_group).ToListAsync();
			if (data.Count == 0) return NotFound(new { message = "Failure record Auth not found." });
			return Ok(data);
		}
		
		[HttpPost]
		[Route("add-failure-record-auth")]
		public async Task<ActionResult<FailureRecordAuth>> AddFailureRecordAuth(FailureRecordAuth form)
		{
			_context.FailureRecordAuth.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetFailureRecordAuthById", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-failure-record-auth")]
		public async Task<IActionResult> EditFailureRecordAuth(int id, FailureRecordAuth form)
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
				if (!FailureRecordAuthExists(id))
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
		[Route("delete-failure-record-auth")]
		public async Task<IActionResult> DeleteFailureRecordAuth(int id)
		{
			var data = await _context.FailureRecordAuth.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.FailureRecordAuth.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool FailureRecordAuthExists(int id)
		{
			return _context.FailureRecordAuth.Any(e => e.id == id);
		}
		
		private IQueryable<FailureRecordAuthView> GetFailureRecordAuthQuery()
		{
			return from au in _context.FailureRecordAuth
					join wg in _context.MdWorkGroup on au.id_work_group equals wg.id into auwg
					from auwgResult in auwg.DefaultIfEmpty()
					
					join us in _context.User on au.id_user equals us.id into auus
					from auusResult in auus.DefaultIfEmpty()

					join rl in _context.MdFailureAuthRole on au.id_role equals rl.id into aurl
					from aurlResult in aurl.DefaultIfEmpty()
					
					orderby au.seq
					
					select new FailureRecordAuthView
					{
						id = au.id,
						id_work_group = au.id_work_group,
						work_group = auwgResult.code,
						authorized_name = au.authorized_name,
						seq = au.seq,
						id_user = au.id_user,
						name = auusResult.name,
						id_role = au.id_role,
						role_name = aurlResult.role_name,
					};
		}
	}
}