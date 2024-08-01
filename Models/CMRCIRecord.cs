namespace CPOC_AIMS_II_Backend.Models
{
	public class CMRCIRecord
	{
		[Key]
		public int id { get; set; }
		public int id_tag { get; set; }
		public int year { get; set; }
		public int id_month { get; set; }
		public decimal? ci_injection_rate { get; set; }
		public decimal? rci_val { get; set; }
		public int? id_status { get; set; }
		public string? note { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
	public class CMInfoWithRCI
	{
		public int? id_record { get; set; }
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public decimal? temp_c { get; set; }
		public string? last_date { get; set; }
		public decimal? ci_injection_rate { get; set; }
		public decimal? rci_val { get; set; }
		public string? note { get; set; }
	}
	public class RCIValue
	{
		public int month_no { get; set; }
		public string? month_code { get; set; }
		public decimal? ci_injection_rate { get; set; }
		public decimal? rci_val { get; set; }
	}
	public class RCIValueDashboard
	{
		public int id_tag { get; set; }
		public string? tag_no { get; set; }
		public List<RCIValue>? values { get; set; }
	}
}