namespace CPOC_AIMS_II_Backend.Models
{
	public class HighlightActivities
	{
		[Key]
		public int id {get;set;}
		public int id_platform {get;set;}
		public int id_asset {get;set;}
		public string? ha_number {get;set;}
		public string? tag_number {get;set;}
		public DateTime report_date {get;set;}
		public string? activities {get;set;}
		public string? person_in_charge {get;set;}
		public string? contractor {get;set;}
		public string? details {get;set;}
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
		public bool is_active {get;set;}
	}
	public class HighlightActivitiesView
	{
		[Key]
		public int id {get;set;}
		public int id_platform {get;set;}
		public string? platform_code_name {get;set;}
		public string? platform_full_name {get;set;}
		public int? phase {get;set;}
		public int id_asset {get;set;}
		public string? asset_type {get;set;}
		public string? asset_icon {get;set;}
		public string? ha_number {get;set;}
		public string? tag_number {get;set;}
		public DateTime report_date {get;set;}
		public string? activities {get;set;}
		public string? person_in_charge {get;set;}
		public string? contractor {get;set;}
		public string? details {get;set;}
        public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
		public bool is_active {get;set;}
	}
}