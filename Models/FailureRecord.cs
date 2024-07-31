namespace CPOC_AIMS_II_Backend.Models
{
	public class FailureRecord
	{
		[Key]
		public int id { get; set; }
		public int? id_platform { get; set; }
		public int? id_failure_impact { get; set; }
		public int? id_discipline { get; set; }
		public int id_appr_status { get; set; }
		public string? fl_number { get; set; }
		public string? tag_no { get; set; }
		public string? drawing_no { get; set; }
		public string? unit { get; set; }
		public string? equipment_type { get; set; }
		public string? details { get; set; }
		public DateTime findings_date { get; set; }
		public string? production_loss { get; set; }
		public string? material_cost { get; set; }
		public string? environment_impact { get; set; }
		public string? health_and_safety { get; set; }
		public string? findings { get; set; }
		public string? mitigation { get; set; }
		public string? remark { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
		public bool? is_active { get; set; }
    }
}