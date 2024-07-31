using System.ComponentModel.DataAnnotations.Schema;

namespace CPOC_AIMS_II_Backend.Models
{
	public class UserInMenu
	{
		[Key]
		public int id { get; set; }
		[ForeignKey("MdMenu")]
		public int id_menu { get; set; }
		[ForeignKey("User")]
		public int id_user { get; set; }
		public bool is_active { get; set; }
		public DateTime? created_date { get; set; }
		public int? created_by { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}

	public class UserInMenuWithMdMenu
	{
		public int id { get; set; }
		public required string username { get; set; }
		public string? name { get; set; }
		public string? uniqueId { get; set; }
		// public string? first_name { get; set; }
		// public string? last_name { get; set; }
		public bool? is_active { get; set; }
		public string? img { get; set; }
		public virtual List<MdMenu>? GetMdMenu { get; set; }
	}
	
	public class UserInModuleWithMdAppModule
	{
		public int id { get; set; }
		public required string username { get; set; }
		public string? name { get; set; }
        public string? uniqueId { get; set; }
		// public string? first_name { get; set; }
		// public string? last_name { get; set; }
		public bool? is_active { get; set; }
		public string? img { get; set; }
		public virtual List<MdAppModule>? GetMdAppModule { get; set; }
	}
}