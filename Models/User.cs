namespace CPOC_AIMS_II_Backend.Models
{
	public class User
	{
		[Key]
		public int id { get; set; }
		[Required]
		public string? username { get; set; }
		[Required]
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		public bool is_active { get; set; }
		public string? img { get; set; }
		public DateTime? login_last_date { get; set; }
		public string? login_token { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}
	
	public class UserUpload
	{
		public int id { get; set; }
		public required string username { get; set; }
		public string? first_name { get; set; }
		public string? last_name { get; set; }
		public bool is_active { get; set; }
		public string? img { get; set; }
        public IFormFile? img_file { get; set; }
		public DateTime? login_last_date { get; set; }
		public string? login_token { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserLogin
	{
		public string? username { get; set; }
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		// public string? first_name { get; set; }
		// public string? last_name { get; set; }
	}
	
	public class ResultLogin
	{
		public int id { get; set; }
		public string? username { get; set; }
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		public required List<MdUserRole> Roles { get; set; }
	}

}