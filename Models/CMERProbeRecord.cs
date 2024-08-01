namespace CPOC_AIMS_II_Backend.Models
{
	public class CMERProbeRecord
	{
		public int id { get; set; }
		public int id_tag { get; set; }
		public DateTime? record_date { get; set; }
		public string? probe_type { get; set; }
		public string? part_no { get; set; }
		public string? probe_id { get; set; }
		public decimal? metal_loss { get; set; }
		public decimal? corrosion_rate { get; set; }
		public string? note { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public int? updated_date { get; set; }

	}
}