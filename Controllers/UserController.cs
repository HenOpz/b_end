using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly MainDbContext _context;
		private readonly IConfiguration _configuration;

		public UserController(MainDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpGet]
        [Route("connection-test")]
        [Authorize]
        public JsonResult ConnectionTest()
        {
            var response = new { status = 1, status_text = "Back-end server is online", authorization = "authorized" };
            return new JsonResult(response);
        }

		[HttpGet]
		[Route("get-active-user-list")]
		public async Task<ActionResult<dynamic>> GetActiveUserList()
		{
			return await _context.User.Where(a => a.is_active).ToListAsync();
		}
		
		[HttpGet]
		[Route("get-active-user-by-id")]
		public async Task<ActionResult<dynamic>> GetActiveUserById(int id)
		{
			try
			{
				User? data = await _context.User.FirstOrDefaultAsync(a => a.id == id && a.is_active);
				
				if(data == null) return NotFound(new { message = "User not found." });
				
				return data;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpGet]
		[Route("get-active-user-by-id-role")]
		public async Task<ActionResult<dynamic>> GetActiveUserByIdRole(int id_role)
		{
			try
			{
				var data = await (from u in _context.User
							join r in _context.UserInRole
							on u.id equals r.id_user into jointable
							from res in jointable.DefaultIfEmpty()
							where res.id_role == id_role
							select new 
							{
								userinfo = u
							}).ToListAsync();
				
				if(data.Count == 0) return NotFound(new { message = "User not found." });
				
				return data;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpPost]
		[Route("login")]
		[AllowAnonymous]
		public async Task<ActionResult<dynamic>> Login(UserLogin data)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				string? secret = _configuration["Security:Secret"];
				if (secret == null) return NotFound(new { message = "Secret not found." });
				
				User? user = await _context.User.FirstOrDefaultAsync(a => a.uniqueId == data.uniqueId && a.is_active);
				
				if(user == null)
				{
					user = new User
					{
						username = data.username,
						name = data.name,
						uniqueId = data.uniqueId,
						is_active = true,
						img = null,
						login_last_date = DateTime.Now,
						login_token = null,
						updated_by = null,
						updated_date = DateTime.Now
					};
					_context.User.Add(user);
					await _context.SaveChangesAsync();
				}
				
				ResultLogin resLogin = new ResultLogin
				{
					id = user.id,
					username = user.username,
					name = user.name,
					uniqueId = user.uniqueId,
					Roles = await (from uir in _context.UserInRole
									join r in _context.MdUserRole on uir.id_role equals r.id
									where uir.id_user == user.id
									select new MdUserRole
									{
										id = r.id,
										code = r.code
									}).ToListAsync()
				};
				
				var token = AuthService.CreateToken(resLogin,secret);
				user.login_token = token;
				
				await _context.SaveChangesAsync();
				transaction.Commit();
				
				return new
					{
						user = resLogin,
						token = token,
						route = "/"
					};
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return BadRequest(new { message = ex.Message });
			}
		}
		
		[HttpPut]
		[Route("update-user")]
		public async Task<ActionResult<dynamic>> UpdateUser(int id,[FromForm] UserUpload data)
		{
			if (id != data.id) return BadRequest();
			
			var transaction = _context.Database.BeginTransaction();
			try
			{
				User? user_data = await _context.User.FirstOrDefaultAsync(a => a.id == id && a.is_active);
				if (user_data == null) return NotFound(new { message = "User not found." });
				
				string? img_path = user_data.img;
				if (data.img_file != null) //Change img to new one or Add new
				{
					if (!Directory.Exists("wwwroot/user/profile_img/")) Directory.CreateDirectory("wwwroot/user/profile_img/");
					if (System.IO.File.Exists(user_data.img)) System.IO.File.Delete(user_data.img);
					img_path = "wwwroot/user/profile_img/" + DateTime.Now.ToString("yyyyMMddHHmmss") + data.img_file.FileName;
					using (var stream = new FileStream(img_path, FileMode.Create))
					{
						await data.img_file.CopyToAsync(stream);
					}
				}
				else if(data.img == null) //Delete img
				{
					if (System.IO.File.Exists(data.img)) System.IO.File.Delete(data.img);
					img_path = null;
				}
				//Change nothing (img_file = null,img = [old one])
				
				// user_data.name = data.name;
				// user_data.uniqueId = data.uniqueId;
				user_data.img = img_path;
				user_data.updated_date = DateTime.Now;
				user_data.updated_by = data.updated_by;
				
				await _context.SaveChangesAsync();
				transaction.Commit();
				return Ok(new { message = "Update user successfully." });
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				if (System.IO.File.Exists(data.img)) System.IO.File.Delete(data.img);
				return NotFound(new { message = ex.Message });
			}
		}
		
		[HttpDelete]
		[Route("inactive-user")]
		public async Task<ActionResult<dynamic>> InactiveUser(int id,int updated_by)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				User? user_data = await _context.User.FirstOrDefaultAsync(a => a.id == id && a.is_active);
				if (user_data == null) return NotFound(new { message = "User not found." });
				
				user_data.is_active = false;
				user_data.updated_by = updated_by;
				user_data.updated_date = DateTime.Now;
				
				await _context.SaveChangesAsync();
				transaction.Commit();
				
				return Ok(new { message = "Inactive user successfully." });
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return NotFound(new { message = ex.Message });
			}
		}
	}
}