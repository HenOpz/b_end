using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.ComponentModel.Design;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserInMenuController : ControllerBase
	{
		private readonly MainDbContext _context;
		public UserInMenuController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserInMenu>>> GetUserInMenu()
		{
			return await _context.UserInMenu.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserInMenu>> GetUserInMenu(int id)
		{
			var data = await _context.UserInMenu.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet]
		[Route("get-menu-list-by-id-user")]
		public async Task<ActionResult<dynamic>> GetMenuListByIdUser(int id_user)
		{
			try
			{
				User? user_data = await _context.User.FirstOrDefaultAsync(a => a.id == id_user && a.is_active);
				if (user_data == null) return NotFound(new { message = "User not found" });

				if (user_data.username == null) return NotFound(new { message = "Something wrong with username." });

				List<MdMenu> res = await (from uim in _context.UserInMenu
									   join m in _context.MdMenu
									   on uim.id_menu equals m.id into jointable
									   from joint in jointable.DefaultIfEmpty()
									   where uim.id_user == id_user
									   select new MdMenu
									   {
										   id = joint.id,
										   id_app_module = joint.id_app_module,
										   name = joint.name,
										   icon = joint.icon,
										   icon_size = joint.icon_size,
										   route = joint.route,
										   seq = joint.seq,
										   is_active = joint.is_active,
										   updated_by = joint.updated_by,
										   updated_date = joint.updated_date
									   }).ToListAsync();

				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("get-user-in-menu-list-by-id-user")]
		public async Task<ActionResult<dynamic>> GetUserInMenuList(int id_user, int id_app_module)
		{
			try
			{
				User? user_data = await _context.User.FirstOrDefaultAsync(a => a.id == id_user && a.is_active);
				if (user_data == null) return NotFound(new { message = "User not found" });

				if (user_data.username == null) return NotFound(new { message = "Something wrong with username." });

				var res = new UserInMenuWithMdMenu
				{
					id = user_data.id,
					username = user_data.username,
					name = user_data.name,
					uniqueId = user_data.uniqueId,
					is_active = user_data.is_active,
					GetMdMenu = await (from uim in _context.UserInMenu
									   join m in _context.MdMenu
									   on uim.id_menu equals m.id into jointable
									   from joint in jointable.DefaultIfEmpty()
									   where uim.id_user == id_user && joint.id_app_module == id_app_module && uim.is_active
									   select new MdMenu
									   {
										   id = joint.id,
										   id_app_module = joint.id_app_module,
										   name = joint.name,
										   icon = joint.icon,
										   icon_size = joint.icon_size,
										   route = joint.route,
										   seq = joint.seq,
										   is_active = joint.is_active,
										   updated_by = joint.updated_by,
										   updated_date = joint.updated_date
									   }
										).ToListAsync()
				};

				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("get-user-in-module-list-by-id-user")]
		public async Task<ActionResult<dynamic>> GetUserInModuleList(int id_user)
		{
			try
			{
				User? user_data = await _context.User.FirstOrDefaultAsync(a => a.id == id_user && a.is_active);
				if (user_data == null) return NotFound(new { message = "User not found" });

				List<int> inMenu = await (from uim in _context.UserInMenu
										  join m in _context.MdMenu
										  on uim.id_menu equals m.id into jointable
										  from joint in jointable.DefaultIfEmpty()
										  where uim.id_user == id_user && uim.is_active
										  select
											  joint.id_app_module
									).Distinct().ToListAsync();

				if (user_data.username == null) return NotFound(new { message = "Something wrong with username." });

				var res = new UserInModuleWithMdAppModule
				{
					id = user_data.id,
					username = user_data.username,
					name = user_data.name,
					uniqueId = user_data.uniqueId,
					is_active = user_data.is_active,
					GetMdAppModule = await _context.MdAppModule.Where(a => inMenu.Contains(a.id)).ToListAsync()
				};

				return res;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut]
		[Route("modify-user-in-menu")]
		public async Task<IActionResult> ModifyUserInMenu(int id_user, List<int> id_menu, int created_by)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				var user = await _context.User.FindAsync(id_user);
				if (user == null) return NotFound("User is not found.");
				
				List<UserInMenu> ogData = await _context.UserInMenu.Where(a => a.id_user == id_user).ToListAsync();
				_context.UserInMenu.RemoveRange(ogData);
				
				List<UserInMenu> data = new List<UserInMenu>();

				if(id_menu.Count > 0)
				{
					foreach(int i in id_menu)
					{
						data.Add(new UserInMenu{ id_user = id_user, 
												 id_menu = i, 
												 is_active = true, 
												 created_by = created_by,
												 created_date = DateTime.Now,
												 updated_by = created_by,
												 updated_date = DateTime.Now});
					}
				}
				else
				{
					return BadRequest("List of id_menu is empty.");
				}
				
				_context.UserInMenu.AddRange(data);
				await _context.SaveChangesAsync();
				
				transaction.Commit();
				
				return Ok();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpDelete]
		[Route("delete-user-in-menu")]
		public async Task<IActionResult> DeleteUserInMenu(int id, int updated_by)
		{
			var data = await _context.UserInMenu.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			data.is_active = false;
			data.updated_by = updated_by;
			data.updated_date = DateTime.Now;
			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserInMenuExists(id))
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

		private bool UserInMenuExists(int id)
		{
			return _context.UserInMenu.Any(e => e.id == id);
		}
	}
}