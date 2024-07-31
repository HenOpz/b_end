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
	public class UserAccountController : ControllerBase
	{
		private readonly MainDbContext _context;
		public UserAccountController(MainDbContext context)
		{
			_context = context;
		}

		private readonly IConfiguration _configuration;

		string sqlDataSource = Startup.ConnectionString;

		// //USER LOGIN
		// [HttpPost]
		// [Route("login")]
		// [AllowAnonymous]
		// public async Task<ActionResult<dynamic>> LogIn(UserAccount form)
		// {
		// 	try
		// 	{
		// 		string? secret = _configuration["Security:Secret"];
		// 		string hashPw = AuthService.Sha256Hash(form.password,secret);
		// 		//UserID is response context template. All response object must matched this model.
		// 		UserAccount? dUser = await _context.UserAccount.Where(a => a.username == form.username).FirstOrDefaultAsync();

		// 		if (dUser == null) return NotFound(new { message = "Incorrect username or password" });

		// 		var token = AuthService.CreateToken(dUser,secret);
		// 		return new
		// 		{
		// 			user = dUser,
		// 			token = token
		// 		};

		// 		//return objectData;
		// 		//return NoContent();
		// 	}
		// 	catch (Exception ex)
		// 	{
		// 		return new { error = ex.Message };
		// 	}

		// }

		//Get User by Role
		[HttpGet]
		[Route("get-account-by-role")]
		public async Task<ActionResult<dynamic>> GetAccountByRole(int id_role)
		{
			var data = await _context.UserAccount.Where(z => z.id_role == id_role).ToListAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		//Get User info by id
		[HttpGet]
		[Route("get-info-by-id-account")]
		public async Task<ActionResult<dynamic>> GetUserInfoById(int id_account)
		{
			var data = await _context.UserInfo.Where(z => z.id_account == id_account).ToListAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		//Get User Data List
		[HttpGet]
		[Route("get-user-data-list")]
		public async Task<ActionResult<dynamic>> GetUserDataList()
		{
			List<UserAccountWithInfo> data = await (from acc in _context.UserAccount
													join info in _context.UserInfo on acc.id equals info.id_account
													join role in _context.MdUserRole on acc.id_role equals role.id
													join pre in _context.MdPrefix on info.id_prefix equals pre.id
													where acc.is_active == true
													select new UserAccountWithInfo
													{
														id_account = acc.id,
														username = acc.username,
														password = null,
														id_role = acc.id_role,
														role_name = role.code,
														is_active = acc.is_active,
														is_admin = acc.is_admin,
														id_info = info.id,
														id_prefix = info.id_prefix,
														prefix = pre.code,
														first_name = info.first_name,
														last_name = info.last_name,
														title = info.title,
														e_mail = info.e_mail
													}).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}

		//Get User Data by id
		[HttpGet]
		[Route("get-user-data-by-id-account")]
		public async Task<ActionResult<dynamic>> GetUserDataById(int id_account)
		{
			List<UserAccountWithInfo> data = await (from acc in _context.UserAccount
													join info in _context.UserInfo on acc.id equals info.id_account
													join role in _context.MdUserRole on acc.id_role equals role.id
													join pre in _context.MdPrefix on info.id_prefix equals pre.id
													where acc.id == id_account && acc.is_active == true
													select new UserAccountWithInfo
													{
														id_account = acc.id,
														username = acc.username,
														password = null,
														id_role = acc.id_role,
														role_name = role.code,
														is_active = acc.is_active,
														is_admin = acc.is_admin,
														id_info = info.id,
														id_prefix = info.id_prefix,
														prefix = pre.code,
														first_name = info.first_name,
														last_name = info.last_name,
														title = info.title,
														e_mail = info.e_mail,
														sign_path = info.sign_path
													}).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}

		//Get User Data by id role
		[HttpGet]
		[Route("get-user-data-by-id-role")]
		public async Task<ActionResult<dynamic>> GetUserDataByIdRole(int id_role)
		{
			List<UserAccountWithInfo> data = await (from acc in _context.UserAccount
													join info in _context.UserInfo on acc.id equals info.id_account
													join role in _context.MdUserRole on acc.id_role equals role.id
													join pre in _context.MdPrefix on info.id_prefix equals pre.id
													where role.id == id_role && acc.is_active == true
													select new UserAccountWithInfo
													{
														id_account = acc.id,
														username = acc.username,
														password = null,
														id_role = acc.id_role,
														role_name = role.code,
														is_active = acc.is_active,
														is_admin = acc.is_admin,
														id_info = info.id,
														id_prefix = info.id_prefix,
														prefix = pre.code,
														first_name = info.first_name,
														last_name = info.last_name,
														title = info.title,
														e_mail = info.e_mail,
														sign_path = info.sign_path
													}).ToListAsync();

			if (data.Count == 0)
			{
				return NotFound();
			}

			return data;
		}

		//Add Account
		[HttpPost]
		[Route("add-account")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> AddAccount([FromForm] UserAccountWithInfo info)
		{
			try
			{
				DataTable response = new DataTable();
				string path = "";
				if (info.password == null) return BadRequest("Something wrong with password.");
				string pwChk = PasswordPolicy.IsValid(info.password);
				if (pwChk == "")
				{
					string? secret = _configuration["Security:Secret"];
					List<UserAccount> UserList = await _context.UserAccount.ToListAsync();

					if (UserList.Exists(a => a.username == info.username))
					{
						return BadRequest("Username already exists, please choose another one.");
					}

					if (info.file != null)
					{
						Directory.CreateDirectory("wwwroot/attach/sign/");
						string ext = System.IO.Path.GetExtension(info.file.FileName);
						path = "wwwroot/attach/sign/" + Path.GetFileNameWithoutExtension(info.file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

						using (var stream = new FileStream(path, FileMode.Create))
						{
							await info.file.CopyToAsync(stream);
						}
					}
					if(secret == null) return StatusCode(401);
					string hashPw = AuthService.Sha256Hash(info.password, secret);
					string query = @"
								BEGIN TRANSACTION

								DECLARE @user_id INT;

								INSERT INTO UserAccount (username,password,id_role,is_active,is_admin)
								VALUES (@username,@password,@id_role,@is_active,@is_admin);

								SET @user_id = SCOPE_IDENTITY();

								INSERT INTO UserInfo (id_account,id_prefix,first_name,last_name,title,e_mail,sign_path)
								VALUES (@user_id,@id_prefix,@first_name,@last_name,@title,@e_mail,@sign_path)

								COMMIT TRANSACTION
							";

					SqlDataReader myReader;
					using (SqlConnection myCon = new SqlConnection(sqlDataSource))
					{
						myCon.Open();
						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myCommand.Parameters.AddWithValue("@username", info.username);
							myCommand.Parameters.AddWithValue("@password", hashPw);
							myCommand.Parameters.AddWithValue("@id_role", info.id_role);
							myCommand.Parameters.AddWithValue("@is_active", info.is_active);
							myCommand.Parameters.AddWithValue("@is_admin", info.is_admin);
							myCommand.Parameters.AddWithValue("@id_prefix", (info.id_prefix == null) ? DBNull.Value : info.id_prefix);
							myCommand.Parameters.AddWithValue("@first_name", (info.first_name == null) ? DBNull.Value : info.first_name);
							myCommand.Parameters.AddWithValue("@last_name", (info.last_name == null) ? DBNull.Value : info.last_name);
							myCommand.Parameters.AddWithValue("@title", (info.title == null) ? DBNull.Value : info.title);
							myCommand.Parameters.AddWithValue("@e_mail", (info.e_mail == null) ? DBNull.Value : info.e_mail);
							myCommand.Parameters.AddWithValue("@sign_path", (path == null) ? DBNull.Value : path);
							myReader = myCommand.ExecuteReader();
							response.Load(myReader);
							myReader.Close();
							myCon.Close();
						}
					}
				}
				else
				{
					return Content(pwChk);
				}
				return new JsonResult(response);
			}
			catch (SqlException e)
			{
				return new JsonResult(e.Message);
			}
		}

		[HttpPut]
		[Route("edit-user-data")]
		//[Authorize]
		public async Task<ActionResult<dynamic>> EditUserData(int id_account, [FromForm] UserAccountWithInfoForEdit info)
		{
			if (id_account != info.id_account)
			{
				return BadRequest();
			}

			try
			{
				var tmp = (from uinfo in _context.UserInfo
						  where uinfo.id_account == info.id_account
						  select new { Uinfo = uinfo }).FirstOrDefault();
				string? path = "";


				if (tmp != null)
				{
					if (info.file != null)
					{
						if (tmp.Uinfo.sign_path != null && tmp.Uinfo.sign_path != "")
						{
							System.IO.File.Delete(tmp.Uinfo.sign_path);
						}
						Directory.CreateDirectory("wwwroot/attach/sign/");
						string ext = System.IO.Path.GetExtension(info.file.FileName);
						path = "wwwroot/attach/sign/" + Path.GetFileNameWithoutExtension(info.file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

						using (var stream = new FileStream(path, FileMode.Create))
						{
							await info.file.CopyToAsync(stream);
						}
					}
					else
					{
						path = tmp.Uinfo.sign_path;
					}
				}
				else
				{
					return BadRequest("Something wrong with sign.");
				}

				DataTable response = new DataTable();

				string query = @"
							BEGIN TRANSACTION

							UPDATE UserAccount
							SET id_role = @id_role
							WHERE id = @id_account

							UPDATE UserInfo
							SET id_prefix = @id_prefix,
								first_name = @first_name,
								last_name = @last_name,
								title = @title,
								e_mail = @e_mail,
								sign_path = @sign_path
							WHERE id_account = @id_account
							
							COMMIT TRANSACTION
						";

				SqlDataReader myReader;
				using (SqlConnection myCon = new SqlConnection(sqlDataSource))
				{
					myCon.Open();
					using (SqlCommand myCommand = new SqlCommand(query, myCon))
					{
						//myCommand.Parameters.AddWithValue("@username", info.username);
						myCommand.Parameters.AddWithValue("@id_account", info.id_account);
						myCommand.Parameters.AddWithValue("@id_role", info.id_role);
						myCommand.Parameters.AddWithValue("@id_prefix", (info.id_prefix == null) ? DBNull.Value : info.id_prefix);
						myCommand.Parameters.AddWithValue("@first_name", (info.first_name == null) ? DBNull.Value : info.first_name);
						myCommand.Parameters.AddWithValue("@last_name", (info.last_name == null) ? DBNull.Value : info.last_name);
						myCommand.Parameters.AddWithValue("@title", (info.title == null) ? DBNull.Value : info.title);
						myCommand.Parameters.AddWithValue("@e_mail", (info.e_mail == null) ? DBNull.Value : info.e_mail);
						myCommand.Parameters.AddWithValue("@sign_path", (path == null) ? DBNull.Value : path);
						myReader = myCommand.ExecuteReader();
						response.Load(myReader);
						myReader.Close();
						myCon.Close();
					}
				}
				return new JsonResult(response);
			}
			catch (SqlException e)
			{
				return new JsonResult(e.Message);
			}
		}

		//Change Password
		[HttpPut]
		[Route("edit-user-password")]
		public async Task<ActionResult<dynamic>> EditPassword(ChangeOldPassword data)
		{
			string? secret = _configuration["Security:Secret"];
			if(secret == null) return StatusCode(401);
			string old_hashPw = AuthService.Sha256Hash(data.old_pw, secret);
			UserAccount? acc = await _context.UserAccount.FindAsync(data.id_account);
			if (acc == null) { return "The id_account was entered incorrectly"; }
			if (acc.password != old_hashPw) { return "Your old password was entered incorrectly"; }
			string pwChk = PasswordPolicy.IsValid(data.new_pw);
			if (pwChk != "") { return pwChk; }
			string hashPw = AuthService.Sha256Hash(data.new_pw, secret);
			if (old_hashPw == hashPw) { return "Password shouldn't use old one."; }

			try
			{
				DataTable response = new DataTable();

				if (pwChk == "")
				{
					string query = @"
							UPDATE UserAccount
							SET password = @password
							WHERE id = @id_account
						";

					SqlDataReader myReader;
					using (SqlConnection myCon = new SqlConnection(sqlDataSource))
					{
						myCon.Open();
						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myCommand.Parameters.AddWithValue("@password", hashPw);
							myCommand.Parameters.AddWithValue("@id_account", data.id_account);
							myReader = myCommand.ExecuteReader();
							response.Load(myReader);
							myReader.Close();
							myCon.Close();
						}
					}
				}

				return "Password changed.";
			}
			catch (SqlException e)
			{
				return new JsonResult(e.Message);
			}
		}

		////Request to change Password
		//[HttpPost]
		//[Route("request-edit-user-password")]
		//public async Task<IActionResult> RequestEditPassword(string email, string build_url)
		//{
		//    var User_Data = await (from a in _context.UserInfo
		//                           join b in _context.UserAccount on a.id_account equals b.id into jointable
		//                           from z in jointable.DefaultIfEmpty()
		//                           where a.e_mail == email
		//                           select new
		//                           {
		//                               Info = a,
		//                               Account = z
		//                           }).FirstOrDefaultAsync();
		//    if (User_Data == null)
		//    {
		//        return NotFound();
		//    }

		//    try
		//    {
		//        var json = Newtonsoft.Json.JsonConvert.SerializeObject(User_Data);
		//        var text_byte = Encoding.UTF8.GetBytes(json);
		//        string url_for_send = build_url + "?data=" + Convert.ToBase64String(text_byte);
		//        UserAccount userAccount = User_Data.Account;
		//        UserInfo userInfo = User_Data.Info;

		//        var builder = new BodyBuilder();
		//        builder.HtmlBody = String.Format(@"Dear {0}. {1} {2},<br><br>
		//                        <div style='margin-left: 40px;'>
		//                        Regarding your request to reset the password to access to E-NDE System.<br>
		//                        <span>Please reset your password by follow this link: </span> <a href = {3}>{3}</a> <br><br>
		//                        </div>
		//                        If you have any questions or problems regarding the system, please contact<br>
		//                        benjapont@sprc.co.th<br>
		//                        Tel. 038-699-000 Ext.7476<br>
		//                        - - - - - - - - - - - - - - - - - - -<br>
		//                        From E-NDE System<br>"
		//                            , new string[] { userInfo.prefix, userInfo.first_name, userInfo.last_name, url_for_send });
		//        MimeMessage mimeEmail = new MimeMessage();
		//        mimeEmail.From.Add(MailboxAddress.Parse(Startup.Config_Email));
		//        mimeEmail.To.Add(MailboxAddress.Parse(userInfo.e_mail));
		//        mimeEmail.Subject = "E-NDE System | Reset Password";
		//        mimeEmail.Body = builder.ToMessageBody();

		//        // send email
		//        using var smtp = new SmtpClient();
		//        smtp.Connect(Startup.SMTP_host, Startup.SMTP_port, SecureSocketOptions.None);
		//        //smtp.Authenticate(Startup.Authen_Email, Startup.Authen_pw);
		//        smtp.Send(mimeEmail);
		//        smtp.Disconnect(true);
		//        return Ok();
		//    }
		//    catch (Exception ex)
		//    {
		//        return StatusCode(500);
		//    }
		//}

		////Change password from email
		//[HttpPut]
		//[Route("edit-user-password-from-email")]
		//public async Task<ActionResult<dynamic>> EditPasswordFromEmail(RequestPass pass)
		//{
		//    UserAccount acc = await _context.UserAccount.FindAsync(pass.id_account);
		//    if (acc == null) { return "The id_account was entered incorrectly"; }

		//    string pwChk = PasswordPolicy.IsValid(pass.new_pw);
		//    if (pwChk != "") { return pwChk; }

		//    string hashPw = AuthService.Sha256Hash(pass.new_pw);

		//    if (acc.password == hashPw) { return "Password shouldn't use old one."; }

		//    try
		//    {
		//        DataTable response = new DataTable();

		//        if (pwChk == "")
		//        {
		//            string query = @"
		//                    UPDATE UserAccount
		//                    SET password = @password
		//                    WHERE id = @id_account
		//                ";

		//            SqlDataReader myReader;
		//            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
		//            {
		//                myCon.Open();
		//                using (SqlCommand myCommand = new SqlCommand(query, myCon))
		//                {
		//                    myCommand.Parameters.AddWithValue("@password", hashPw);
		//                    myCommand.Parameters.AddWithValue("@id_account", pass.id_account);
		//                    myReader = myCommand.ExecuteReader();
		//                    response.Load(myReader);
		//                    myReader.Close();
		//                    myCon.Close();
		//                }
		//            }
		//        }

		//        return "Password changed.";
		//    }
		//    catch (SqlException e)
		//    {
		//        return new JsonResult(e.Message);
		//    }
		//}

		[HttpPut]
		[Route("edit-user-info-by-id-account")]
		public async Task<IActionResult> EditUserInfo(int id_account, UserInfo form)
		{
			if (id_account != form.id_account)
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
				if (!UserAccountExists(id_account))
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

		//Delete only sign
		[HttpDelete]
		[Route("delete-only-sign")]
		public async Task<IActionResult> DeleteSignPath(int id_account)
		{
			UserInfo? data = await _context.UserInfo.Where(a => a.id_account == id_account).FirstOrDefaultAsync();

			if (data != null)
			{
				data.sign_path = "";
				_context.Entry(data).State = EntityState.Modified;
			}
			else
			{
				return BadRequest();
			}

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserAccountExists(id_account))
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

		// DELETE
		[HttpDelete]
		[Route("delete-account")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			UserAccount? data = await _context.UserAccount.FindAsync(id);

			if (data != null)
			{
				data.is_active = false;
				_context.Entry(data).State = EntityState.Modified;
			}
			else
			{
				return BadRequest();
			}

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserAccountExists(id))
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

		private bool UserAccountExists(int id)
		{
			return _context.UserAccount.Any(e => e.id == id);
		}
	}

	public class PasswordPolicy
	{
		private static int Minimum_Length = 16;
		private static int Upper_Case_length = 1;
		private static int Lower_Case_length = 1;
		private static int NonAlpha_length = 1;

		public static string IsValid(string Password)
		{
			if (Password.Length < Minimum_Length)
				return "Password should be a minimum of 16 characters long.";
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