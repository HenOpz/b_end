namespace CPOC_AIMS_II_Backend.Models
{
	public class GpiRecord
	{
		[Key]
		public int id { get; set; }
		public int? id_platform { get; set; }
		public int? id_main_workctr { get; set; }
		public int? id_repair { get; set; }
		public int? id_severity { get; set; }
		public string? asset_type { get; set; }
		public required string tag_no { get; set; }
		public DateTime gpi_date { get; set; }
		public DateTime? expected_finish_date { get; set; }
		public string? findings { get; set; }
		public string? recommendation { get; set; }
		public string? main_component_free_text { get; set; }
		public string? repair_type_free_text { get; set; }
		public string? damage_mechanism_free_text { get; set; }
		public string? location_deck { get; set; }
		public bool? is_active { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
}