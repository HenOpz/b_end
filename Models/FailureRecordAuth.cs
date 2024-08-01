namespace CPOC_AIMS_II_Backend.Models
{
	public class FailureRecordAuth
	{
		[Key]
		public int id {get;set;}
		public int id_work_group {get;set;}
		public string? authorized_name {get;set;}
		public int seq {get;set;}
		public int id_user {get;set;}
		public int?  id_role {get;set;}
	}
	
	public class FailureRecordAuthView
	{
		public int id {get;set;}
		public int id_work_group {get;set;}
		public string? work_group {get;set;}
		public string? authorized_name {get;set;}
		public int seq {get;set;}
		public int id_user {get;set;}
		public string? name { get; set; }
		public int? id_role {get;set;}
		public string? role_name { get; set; }

	}
}