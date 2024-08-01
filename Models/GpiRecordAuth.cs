namespace CPOC_AIMS_II_Backend.Models
{
    public class GpiRecordAuth
	{
		[Key]
		public int id {get;set;}
		public int id_discipline {get;set;}
		public string? authorized_name {get;set;}
		public int seq {get;set;}
		public int id_user {get;set;}
	}
	
	public class GpiRecordAuthView
	{
		public int id {get;set;}
		public int id_discipline {get;set;}
		public string? discipline {get;set;}
		public string? authorized_name {get;set;}
		public int seq {get;set;}
		public int id_user {get;set;}
		public string? name { get; set; }

	}
}