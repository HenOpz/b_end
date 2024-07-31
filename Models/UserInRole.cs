using System.ComponentModel.DataAnnotations.Schema;

namespace CPOC_AIMS_II_Backend.Models
{
	public class UserInRole
	{
		[Key]
		public int id { get; set; }
		[ForeignKey("MdUserRole")]
		public int? id_role { get; set; }
		[ForeignKey("User")]
		public int? id_user { get; set; }
		public bool is_active { get; set; }
		public DateTime? created_date { get; set; }
		public int? created_by { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}
	
	public class UserInRoleFull
	{
		public int id { get; set; }
		public int? id_role { get; set; }
		public virtual MdUserRole? GetUserRole { get; set; }
		public int? id_user { get; set; }
		public virtual User? GetUser { get; set; }
		public bool is_active { get; set; }
		public DateTime? created_date { get; set; }
		public int? created_by { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}
}