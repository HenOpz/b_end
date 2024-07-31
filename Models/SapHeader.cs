namespace CPOC_AIMS_II_Backend.Models
{
	public class SapHeader
	{
		[Key]
		public int id { get; set; }
		public int? id_module { get; set; }
		public int? id_from_module { get; set; }
		public string? id_reference { get; set; }
		public string? notification { get; set; }
		public string? wo_order_no { get; set; }
		public string? notification_type { get; set; }
		public string? description { get; set; }
		public string? functional_location { get; set; }
		public string? equipment_no { get; set; }
		public string? technical_identification { get; set; }
		public int? manpower { get; set; }
		public decimal? total_man_hours { get; set; }
		public string? planner_grp { get; set; }
		public string? planner_grp_planning_plant { get; set; }
		public string? main_workctr { get; set; }
		public string? reported_by { get; set; }
		public string? required_start_date { get; set; }
		public string? required_start_time { get; set; }
		public string? required_end_date { get; set; }
		public string? required_end_time { get; set; }
		public string? priority { get; set; }
		public string? code_grp_object_part { get; set; }
		public string? object_part_code { get; set; }
		public string? code_grp_damage { get; set; }
		public string? damage_code { get; set; }
		public string? damage_free_text { get; set; }
		public string? code_grp_cause { get; set; }
		public string? cause_code { get; set; }
		public string? cause_grp_free_text { get; set; }
		public string? additional_data { get; set; }
		public string? accessibility { get; set; }
		public string? scaffolding_requirement { get; set; }
		public string? abc_indicator { get; set; }
		public DateTime? approve_date { get; set; }
		public bool? is_active { get; set; }
		public DateTime? changed_on { get; set; }
		public string? user_status { get; set; }
		public string? system_status { get; set; }
		public DateTime? notification_date { get; set; }
		public DateTime? basic_start_date { get; set; }
		public DateTime? basic_finish_date { get; set; }
		public string? unit_for_work { get; set; }
		public decimal? duration { get; set; }
		public string? normal_duration_unit { get; set; }
		public decimal? actual_work { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }

	}
}