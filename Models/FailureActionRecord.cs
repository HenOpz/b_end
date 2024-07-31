namespace CPOC_AIMS_II_Backend.Models
{
	public class FailureActionRecord
	{
		[Key]
		public int id { get; set; }
		public int id_failure { get; set; }
		public int id_failure_action_status { get; set; }
		public int id_discipline { get; set; }
		public string? fa_number { get; set; }
		public string? action_type { get; set; }
		public DateTime? action_date { get; set; }
		public string? action_details { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}

	public class FailureActionRecordView
	{
		public int id { get; set; }
		public int id_failure { get; set; }
		public string? fl_number { get; set; }
		public string? tag_no { get; set; }
		public int id_failure_action_status { get; set; }
		public string? failure_action_status { get; set; }
		public string? failure_action_color_code { get; set; }
		public int id_discipline { get; set; }
		public string? discipline { get; set; }
		public string? fa_number { get; set; }
		public string? action_type { get; set; }
		public DateTime? action_date { get; set; }
		public string? action_details { get; set; }
		public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
	}
}