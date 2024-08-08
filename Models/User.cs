namespace CPOC_AIMS_II_Backend.Models
{
	public class User
	{
		[Key]
		public int id { get; set; }
		[Required]
		public int id_login_method { get; set; }
		[Required]
		public string? username { get; set; }
		public string? password { get; set; }
		[Required]
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		public bool is_active { get; set; }
		public DateTime? login_last_date { get; set; }
		public string? login_token { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserRegister
	{
		[Required]
		public string? username { get; set; }
		public string? password { get; set; }
		[Required]
		public string? name { get; set; }
		public string? email { get; set; }
		public string? position { get; set; }
		public int id_company{ get; set; }
		public bool is_active { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserLogin
	{
		public string? username { get; set; }
		public string? name { get; set; }
		public string? uniqueId { get; set; }
	}

	public class UserVendorLogin
	{
		public string? username { get; set; }
		public string? password { get; set; }
	}
	
	public class ResultLogin
	{
		public int id { get; set; }
		public string? username { get; set; }
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		public required List<MdUserRole> Roles { get; set; }
		public List<FailureRecordAuth>? failureRecordAuths{ get; set; }
		public List<GpiRecordAuth>? gpiRecordAuths{ get; set; }
		public List<UserInfoView>? UserInfoes{ get; set; }
	}

	public class UserInfo
	{
		[Key]
		public int id { get; set; }
		public int id_user { get; set; }
		public int id_company { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
		public string? position { get; set; }
		public string? profile_img { get; set; }
		public string? signature { get; set; }
		public string? pin { get; set; }
		public bool is_active { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserInfoView
	{
		[Key]
		public int id { get; set; }
		public int id_user { get; set; }
		public int id_company { get; set; }
		public string? company_name { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
		public string? position { get; set; }
		public string? profile_img { get; set; }
		public string? signature { get; set; }
		public string? pin { get; set; }
	}

	public class UserInfoEdit
	{
		[Key]
		public int id { get; set; }
		public int id_user { get; set; }
		public int id_company { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
		public string? position { get; set; }
		public string? pin { get; set; }
		public bool is_active { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserInfoUploadPic
    {
        public int id { get; set; }
        public IFormFile? file { get; set; }
        public int type { get; set; }
        public string? profile_img { get; set; }
        public string? signature { get; set; }
    }
}

