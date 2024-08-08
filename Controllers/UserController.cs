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
		[Route("get-user-list")]
		public async Task<ActionResult<dynamic>> GetUserList()
		{
			return await _context.User.ToListAsync();
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

				if (data == null) return NotFound(new { message = "User not found." });

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

				if (data.Count == 0) return NotFound(new { message = "User not found." });

				return data;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("register-user")]
		public async Task<ActionResult<dynamic>> ResigterUser(UserRegister form)
		{
			try
			{
				string pwChk = PasswordPolicy.IsValid(form.password);
				// int atIndex = data.email.IndexOf("@");

				if (pwChk == "")
				{
					List<User> UserList = await _context.User.ToListAsync();
					if (UserList.Exists(a => a.username == form.username))
					{
						return Content("Username already exists, please try another one.");
					}

					string? secret = _configuration["Security:Secret"];
					if (secret == null) return NotFound(new { message = "Secret not found." });
					string hashPw = AuthService.Sha256Hash(form.password, secret);

					User userData = new User
					{
						username = form.username,
						password = hashPw,
						id_login_method = 2,
						name = form.name,
						is_active = true,
						updated_date = DateTime.Now,
						updated_by = form.updated_by,
					};

					_context.User.Add(userData);
					await _context.SaveChangesAsync();

					UserInfo userInfo = new()
					{
						id_user = userData.id,
						id_company = form.id_company,
						name = form.name,
						email = form.email,
						position = form.position,
						is_active = true,
						updated_by = form.updated_by,
						updated_date = DateTime.Now
					};
					_context.UserInfo.Add(userInfo);
					await _context.SaveChangesAsync();

					return Ok(new { message = "Registation successfully." });
				}
				else
				{
					return Content(pwChk);
				}
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

				if (user == null)
				{
					user = new User
					{
						username = data.username,
						name = data.name,
						uniqueId = data.uniqueId,
						id_login_method = 1,
						is_active = true,
						login_last_date = DateTime.Now,
						login_token = null,
						updated_by = null,
						updated_date = DateTime.Now
					};
					_context.User.Add(user);
					await _context.SaveChangesAsync();

					UserInfo userInfo = new()
					{
						id_user = user.id,
						id_company = 1,
						name = data.name,
						email = data.username,
						is_active = true,
						updated_by = null,
						updated_date = DateTime.Now
					};
					_context.UserInfo.Add(userInfo);
					await _context.SaveChangesAsync();
				}

				ResultLogin resLogin = new ResultLogin
				{
					id = user.id,
					username = user.username,
					name = user.name,
					uniqueId = user.uniqueId,
					UserInfoes = await (from uif in _context.UserInfo
										join c in _context.MdUserCompany on uif.id_company equals c.id
										where uif.id_user == user.id && uif.is_active == true
										select new UserInfoView
										{
											id = uif.id,
											id_user = uif.id_user,
											id_company = uif.id_company,
											company_name = c.company_name,
											name = uif.name,
											email = uif.email,
											position = uif.position,
											pin = uif.pin,
											profile_img = uif.profile_img,
										}).ToListAsync(),
					Roles = await (from uir in _context.UserInRole
								   join r in _context.MdUserRole on uir.id_role equals r.id
								   where uir.id_user == user.id
								   select new MdUserRole
								   {
									   id = r.id,
									   code = r.code
								   }).ToListAsync(),
					failureRecordAuths = await (from fra in _context.FailureRecordAuth
												where fra.id_user == user.id
												select new FailureRecordAuth
												{
													id = fra.id,
													id_work_group = fra.id_work_group,
													authorized_name = fra.authorized_name,
													seq = fra.seq,
													id_user = user.id,
													id_role = fra.id_role,
												}).ToListAsync(),
					gpiRecordAuths = await (from gpi in _context.GpiRecordAuth
											where gpi.id_user == user.id
											select new GpiRecordAuth
											{
												id = gpi.id,
												id_discipline = gpi.id_discipline,
												authorized_name = gpi.authorized_name,
												seq = gpi.seq,
												id_user = gpi.id_user,
											}).ToListAsync()
				};

				var token = AuthService.CreateToken(resLogin, secret);
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

		[HttpPost]
        [Route("loginByVendor")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> LogInByVendor(UserVendorLogin form)
        {
            try
            {
				string? secret = _configuration["Security:Secret"];
                string hashPw = AuthService.Sha256Hash(form.password, secret);
                User? user = await _context.User.FirstOrDefaultAsync(a => (a.username == form.username) && (a.password == hashPw));
				
                if (user == null) return NotFound(new { message = "Incorrect username or password" });

				ResultLogin resLogin = new ResultLogin
				{
					id = user.id,
					username = user.username,
					name = user.name,
					uniqueId = user.uniqueId,
					UserInfoes = await (from uif in _context.UserInfo
										join c in _context.MdUserCompany on uif.id_company equals c.id
										where uif.id_user == user.id && uif.is_active == true
										select new UserInfoView
										{
											id = uif.id,
											id_user = uif.id_user,
											id_company = uif.id_company,
											company_name = c.company_name,
											name = uif.name,
											email = uif.email,
											position = uif.position,
											pin = uif.pin,
											profile_img = uif.profile_img,
										}).ToListAsync(),
					Roles = await (from uir in _context.UserInRole
									join r in _context.MdUserRole on uir.id_role equals r.id
									where uir.id_user == user.id
									select new MdUserRole
									{
										id = r.id,
										code = r.code
									}).ToListAsync(),
					failureRecordAuths = await (from fra in _context.FailureRecordAuth
												where fra.id_user == user.id
												select new FailureRecordAuth
												{
													id = fra.id,
													id_work_group = fra.id_work_group,
													authorized_name = fra.authorized_name,
													seq = fra.seq,
													id_user = user.id,
													id_role = fra.id_role,
												}).ToListAsync(),
					gpiRecordAuths = await (from gpi in _context.GpiRecordAuth
											where gpi.id_user == user.id
											select new GpiRecordAuth
											{
												id = gpi.id,
												id_discipline = gpi.id_discipline,
												authorized_name = gpi.authorized_name,
												seq = gpi.seq,
												id_user = gpi.id_user,
											}).ToListAsync()
				};

                var token = AuthService.CreateToken(resLogin, secret);
				user.login_token = token;

                return new
                {
                    user = resLogin,
					token = token,
					route = "/"
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }

        }

		[HttpDelete]
		[Route("inactive-user")]
		public async Task<ActionResult<dynamic>> InactiveUser(int id, int updated_by)
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

		[HttpGet]
		[Route("get-user-info-by-id-user")]
		public async Task<ActionResult<dynamic>> GetUserInfoByIdUser(int id)
		{
			try
			{
				var data = await _context.UserInfo.Where(a => a.id_user == id).ToListAsync();

				if (data == null) return NotFound(new { message = "User not found." });

				return data;
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("add-user-info")]
		public async Task<ActionResult<UserInfo>> AddUserInfo(UserInfo form)
		{
			_context.UserInfo.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUserInfoByIdUser", new { id = form.id_user }, form);
		}


		[HttpPut]
		[Route("edit-user-info")]
		public async Task<ActionResult<dynamic>> EditUserInfo(int id, UserInfoEdit form)
		{
			try
			{
				if (id != form.id)
				{
					return BadRequest();
				}

				var userInfo = await _context.UserInfo.Where(u => u.id == form.id).FirstOrDefaultAsync();

				if (userInfo != null)
				{
					userInfo.id_company = form.id_company;
					userInfo.name = form.name;
					userInfo.position = form.position;
					userInfo.email = form.email;
					userInfo.pin = form.pin;
					userInfo.updated_by = form.updated_by;
					userInfo.updated_date = DateTime.Now;
					await _context.SaveChangesAsync();
				}

				else
				{
					return BadRequest(new JsonResult("User info not found"));
				}
				return Ok(userInfo);
			}
			catch (Exception e)
			{
				return new JsonResult(e.Message);
			}

		}

		[HttpDelete]
		[Route("inactive-user-info")]
		public async Task<ActionResult<dynamic>> InactiveUserInfo(int id, int updated_by)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				UserInfo? user_data = await _context.UserInfo.FirstOrDefaultAsync(a => a.id == id && a.is_active);
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

		[HttpPost]
		[Route("upload-user-info-pic")]
		public async Task<ActionResult<dynamic>> UploadUserInfoPic([FromForm] UserInfoUploadPic form)
		{
			try
			{
				string path = "";
				var info = await _context.UserInfo.Where(u => u.id == form.id).FirstOrDefaultAsync();

				if (form.file != null && info != null)
				{
					if(!Directory.Exists("wwwroot/account/user/")) {
						Directory.CreateDirectory("wwwroot/account/user/");
					}
					string ext = Path.GetExtension(form.file.FileName);

					//Uplaod Profile
					if (form.type == 1)
					{
						if (System.IO.File.Exists(info.profile_img)) {
							System.IO.File.Delete(info.profile_img);
						}
						path = "wwwroot/account/user/" + info.id + "_profile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

						using (var stream = new FileStream(path, FileMode.Create))
						{
							await form.file.CopyToAsync(stream);
						}
						info.profile_img = path;
						await _context.SaveChangesAsync();
					}

					//Upload Signature
					else if (form.type == 2)
					{
						if (System.IO.File.Exists(info.signature)) {
							System.IO.File.Delete(info.signature);
						}
						path = "wwwroot/account/user/" + info.id + "_signature_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

						using (var stream = new FileStream(path, FileMode.Create))
						{
							await form.file.CopyToAsync(stream);
						}
						info.signature = path;
						await _context.SaveChangesAsync();
					}
				}
				else
				{
					return BadRequest(new JsonResult("User info not found"));
				}

				return Ok(info);
			}
			catch (Exception e)
			{
				return new JsonResult(e.Message);
			}
		}

		[HttpDelete("delete-user-info-pic")]
		public async Task<IActionResult> DeleteUserInfoPic(int id, int pic_type)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				UserInfo? info = await _context.UserInfo.FirstOrDefaultAsync(u => u.id == id);
				if (info == null) return NotFound(new { message = "User info not found." });

				if(pic_type == 1)
				{
					if (System.IO.File.Exists(info.profile_img))
					{
						System.IO.File.Delete(info.profile_img);
					}
					info.profile_img = "";
				}
				else if(pic_type == 2)
				{
					if (System.IO.File.Exists(info.signature))
					{
						System.IO.File.Delete(info.signature);
					}
					info.signature = "";
				}

				await _context.SaveChangesAsync();
				transaction.Commit();

				return Ok(info);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return BadRequest(new { message = ex.Message });
			}
		}

		public class PasswordPolicy
		{
			private static int Minimum_Length = 8;
			private static int Upper_Case_length = 1;
			//private static int Lower_Case_length = 1;
			private static int NonAlpha_length = 1;

			public static string IsValid(string Password)
			{
				if (Password.Length < Minimum_Length)
					return "Password should be a minimum of 8 characters long.";
				if (UpperCaseCount(Password) < Upper_Case_length)
					return "Password should have at least 1 uppercase letter.";
				//if (LowerCaseCount(Password) < Lower_Case_length)
				//    return false;
				if (NumericCount(Password) < 1)
					return "Password should have at least 1 number.";
				if (NonAlphaCount(Password) < NonAlpha_length)
					return "Password should have at least 1 special character.";
				return "";
			}

			private static int UpperCaseCount(string Password)
			{
				return Regex.Matches(Password, "[A-Z]").Count;
			}
			private static int LowerCaseCount(string Password)
			{
				return Regex.Matches(Password, "[a-z]").Count;
			}
			private static int NumericCount(string Password)
			{
				return Regex.Matches(Password, "[0-9]").Count;
			}
			private static int NonAlphaCount(string Password)
			{
				return Regex.Matches(Password, @"[^0-9a-zA-Z]").Count;
			}
		}


	}
}