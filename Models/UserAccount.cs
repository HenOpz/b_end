namespace CPOC_AIMS_II_Backend.Models
{
	public class UserAccount
	{
		[Key]
		public int id { get; set; }
		[Required]
		public Guid uuid { get; set; }
		[Required]
		public required string username { get; set; }
		[Required]
		public required string password { get; set; }
		public int id_role { get; set; }
		public bool? is_active { get; set; }
		public bool? is_admin { get; set; }
	}
	
	public class UserInfo
	{
		[Key]
		public int id { get; set; }
		public int id_account { get; set; }
		public int? id_prefix { get; set; }
		public string? first_name { get; set; }
		public string? last_name { get; set; }
		public string? title { get; set; }
		public string? e_mail { get; set; }
		public string? sign_path { get; set; }
	}
	public class UserAccountWithInfo
    {
        public int id_account { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public int? id_role { get; set; }
        public string? role_name { get; set; }
        public bool? is_active { get; set; }
        public bool? is_admin { get; set; }
        public int? id_info { get; set; }
		public int? id_prefix { get; set; }
        public string? prefix { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? title { get; set; }
        public string? e_mail { get; set; }
        public string? sign_path { get; set; }
        public IFormFile? file { get; set; }
    }
    public class UserAccountWithInfoForEdit
    {
        public int id_account { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public int id_role { get; set; }
        public string? role_name { get; set; }
        public bool? is_admin { get; set; }
        public int? id_info { get; set; }
		public int? id_prefix { get; set; }
        public string? prefix { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? title { get; set; }
        public string? e_mail { get; set; }
        public string? sign_path { get; set; }
        public IFormFile? file { get; set; }
    }
	public class RequestPass
	{
		public int id_account { get; set; }
		public required string new_pw { get; set; }
	}
	public class ChangeOldPassword : RequestPass
	{
		public required string old_pw { get; set; }
	}
}