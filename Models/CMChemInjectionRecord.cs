namespace CPOC_AIMS_II_Backend.Models
{
	public class CMChemInjectionRecord
	{
		[Key]
		public int id { get; set; }
		public int id_tag { get; set; }
		public DateTime? record_date { get; set; }
		public decimal? gas_flow_rate_mmscfd { get; set; }
		public decimal? req_ci_injection_rate_liters_mmscfd { get; set; }
		public decimal? req_ci_rate_liters_day { get; set; }
		public decimal? yesterday_ci_tank_level_percent { get; set; }
		public decimal? today_ci_tank_level_percent { get; set; }
		public decimal? ci_tank_calc { get; set; }
		public decimal? actual_ci_injection_liters_day { get; set; }
		public string? remark { get; set; }
		public int? id_status { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
	public class CMChemInjectionRecordView
	{
		public int id { get; set; }
		public int id_tag { get; set; }
		public string? tag_no { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public DateTime? record_date { get; set; }
		public decimal? gas_flow_rate_mmscfd { get; set; }
		public decimal? req_ci_injection_rate_liters_mmscfd { get; set; }
		public decimal? req_ci_rate_liters_day { get; set; }
		public decimal? yesterday_ci_tank_level_percent { get; set; }
		public decimal? today_ci_tank_level_percent { get; set; }
		public decimal? ci_tank_calc { get; set; }
		public decimal? actual_ci_injection_liters_day { get; set; }
		public string? remark { get; set; }
		public int? id_status { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
	}
	public class MonthlyPassPercentage
	{
		public string? Month { get; set; }
		public decimal Percentage { get; set; }
	}
}