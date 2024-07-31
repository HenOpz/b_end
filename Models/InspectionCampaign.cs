namespace CPOC_AIMS_II_Backend.Models
{
	public class InspectionCampaign
	{
		[Key]
		public int id { get; set; }
		public int? id_ic_status { get; set; }
		public string? ic_number { get; set; }
		public string? inspection_program { get; set; }
		public DateTime? start_date { get; set; }
		public DateTime? end_date { get; set; }
		public string? person_in_charge { get; set; }
		public string? contractor {get;set;}
		public string? comments { get; set; }
		public string? file_path { get; set; }
		public string? file_name { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}
	
	public class InspectionCampaignView
	{
		[Key]
		public int id { get; set; }
		public int? id_ic_status { get; set; }
		public string? ic_status { get; set; }
		public string? ic_status_color_code { get; set; }
		public string? ic_number { get; set; }
		public string? inspection_program { get; set; }
		public DateTime? start_date { get; set; }
		public DateTime? end_date { get; set; }
		public string? person_in_charge { get; set; }
		public string? contractor {get;set;}
		public string? comments { get; set; }
		public string? file_path { get; set; }
		public string? file_name { get; set; }
		public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}
}