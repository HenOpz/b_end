namespace CPOC_AIMS_II_Backend.Models
{
	public class RectificationCampaign
	{
		[Key]
		public int id { get; set; }
		public string? rc_number { get; set; }
		public string? rc_issue { get; set; }
		public string? person_in_charge { get; set; }
		public string? contactor { get; set; }
		public DateTime? target_completion { get; set; }
		public string? comments { get; set; }
		public int? id_status_work_package { get; set; }
		public int? id_status_manpower { get; set; }
		public int? id_status_equipment { get; set; }
		public int? id_status_pob { get; set; }
		public int? id_status_execute { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}
	public class RectificationCampaignView
	{
		[Key]
		public int id { get; set; }
		public string? rc_number { get; set; }
		public string? rc_issue { get; set; }
		public string? person_in_charge { get; set; }
		public string? contactor { get; set; }
		public DateTime? target_completion { get; set; }
		public string? comments { get; set; }
		public int? id_status_work_package { get; set; }
		public string? status_work_package { get; set; }
		public string? status_work_package_color { get; set; }
		public int? id_status_manpower { get; set; }
		public string? status_manpower { get; set; }
		public string? status_manpower_color { get; set; }
		public int? id_status_equipment { get; set; }
		public string? status_equipment { get; set; }
		public string? status_equipment_color { get; set; }
		public int? id_status_pob { get; set; }
		public string? status_pob { get; set; }
		public string? status_pob_color { get; set; }
		public int? id_status_execute { get; set; }
		public string? status_execute { get; set; }
		public string? status_execute_color { get; set; }
		public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}
}