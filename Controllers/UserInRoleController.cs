using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserInRoleController : ControllerBase
	{
		private readonly MainDbContext _context;
		public UserInRoleController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpGet]
		[Route("get-user-in-role")]
		public async Task<ActionResult<dynamic>> GetUserInRoleList()
		{
			try
			{
				List<UserInRole>? data = await _context.UserInRole.Where(a => a.is_active).ToListAsync();
				List<UserInRoleFull> res = new List<UserInRoleFull>();
				
				if(data.Count == 0) return NotFound();
				
				foreach(UserInRole i in data)
				{
					UserInRoleFull tmp = new UserInRoleFull
					{
						id = i.id,
						id_role = i.id_role,
						id_user = i.id_user,
						is_active = i.is_active,
						created_date = i.created_date,
						created_by = i.created_by,
						updated_date = i.updated_date,
						updated_by = i.updated_by,
						GetUser = await _context.User.FirstOrDefaultAsync(a => a.id == i.id_user),
						GetUserRole = await _context.MdUserRole.FirstOrDefaultAsync(a => a.id == i.id_role)
					};

					res.Add(tmp);
				}
				
				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpGet]
		[Route("get-user-in-role-by-id-user")]
		public async Task<ActionResult<dynamic>> GetUserInRoleListByIdUser(int id_user)
		{
			try
			{
				List<UserInRole>? data = await _context.UserInRole.Where(a => a.id_user == id_user && a.is_active).ToListAsync();
				List<UserInRoleFull> res = new List<UserInRoleFull>();
				
				if(data.Count == 0) return NotFound();
				
				foreach(UserInRole i in data)
				{
					UserInRoleFull tmp = new UserInRoleFull
					{
						id = i.id,
						id_role = i.id_role,
						id_user = i.id_user,
						is_active = i.is_active,
						created_date = i.created_date,
						created_by = i.created_by,
						updated_date = i.updated_date,
						updated_by = i.updated_by,
						GetUser = await _context.User.FirstOrDefaultAsync(a => a.id == i.id_user),
						GetUserRole = await _context.MdUserRole.FirstOrDefaultAsync(a => a.id == i.id_role)
					};

					res.Add(tmp);
				}
				
				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpGet]
		[Route("get-user-in-role-by-id-role")]
		public async Task<ActionResult<dynamic>> GetUserInRoleListByIdRole(int id_role)
		{
			try
			{
				List<UserInRole>? data = await _context.UserInRole.Where(a => a.id_role == id_role && a.is_active).ToListAsync();
				List<UserInRoleFull> res = new List<UserInRoleFull>();
				
				if(data.Count == 0) return NotFound();
				
				foreach(UserInRole i in data)
				{
					UserInRoleFull tmp = new UserInRoleFull
					{
						id = i.id,
						id_role = i.id_role,
						id_user = i.id_user,
						is_active = i.is_active,
						created_date = i.created_date,
						created_by = i.created_by,
						updated_date = i.updated_date,
						updated_by = i.updated_by,
						GetUser = await _context.User.FirstOrDefaultAsync(a => a.id == i.id_user),
						GetUserRole = await _context.MdUserRole.FirstOrDefaultAsync(a => a.id == i.id_role)
					};

					res.Add(tmp);
				}
				
				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpPost]
		[Route("add-user-in-role")]
		public async Task<ActionResult<dynamic>> PostUserInRole(UserInRole data)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				UserInRole? inRole = await _context.UserInRole.FirstOrDefaultAsync(a => a.id_user == data.id_user && a.id_role == data.id_role && a.is_active);
				if(inRole != null) return BadRequest(new { message = "This User in Role is already have."});
				
				_context.UserInRole.Add(data);
				await _context.SaveChangesAsync();
				transaction.Commit();
				
				return Ok(inRole);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}
		
		[HttpPut]
		[Route("update-user-in-role")]
		public async Task<ActionResult<dynamic>> PutUserInRole(int id,UserInRole data)
		{
			if(id != data.id) return BadRequest();
			
			var transaction = _context.Database.BeginTransaction();
			try
			{
				UserInRole? inRole = await _context.UserInRole.FirstOrDefaultAsync(a => a.id_user == data.id_user && a.id_role == data.id_role && a.is_active);
				if(inRole != null) return BadRequest(new { message = "This User in Role is already have."});
				
				UserInRole? inRole1 = await _context.UserInRole.FirstOrDefaultAsync(a => a.id == id && a.is_active);
				if(inRole1 == null) return BadRequest(new { message = "UserInRole not found."});
				
				inRole1.id_user = data.id_user;
				inRole1.id_role = data.id_role;
				inRole1.updated_by = data.updated_by;
				inRole1.updated_date = DateTime.Now;
				
				await _context.SaveChangesAsync();
				transaction.Commit();
				
				return Ok(inRole1);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}
		
		[HttpDelete]
		[Route("inactive-user-in-role")]
		public async Task<ActionResult<dynamic>> InactiveUser(int id,int updated_by)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				UserInRole? user_data = await _context.UserInRole.FirstOrDefaultAsync(a => a.id == id && a.is_active);
				if (user_data == null) return NotFound(new { message = "User in Role not found." });
				
				user_data.is_active = false;
				user_data.updated_by = updated_by;
				user_data.updated_date = DateTime.Now;
				
				await _context.SaveChangesAsync();
				transaction.Commit();
				
				return Ok(new { message = "Inactive successfully." });
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}
	}
}