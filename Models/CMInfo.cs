namespace CPOC_AIMS_II_Backend.Models
{
	public class CMInfo
	{
		public int id { get; set; }
		public int id_platform { get; set; }
		public int id_system { get; set; }
		public string? tag_no { get; set; }
		public decimal? temp_c { get; set; }
		public string? desc { get; set; }
		public bool? is_pigging_opt { get; set; }
		public string? pigging_opt_desc { get; set; }
		public bool? is_water_analysis { get; set; }
		public string? water_analysis_desc { get; set; }
		public bool? is_micro_bacteria { get; set; }
		public string? micro_bacteria_desc { get; set; }
		public bool? is_corrosion_coupon { get; set; }
		public string? corrosion_coupon_desc { get; set; }
		public bool? is_er_probe { get; set; }
		public string? er_probe_desc { get; set; }
		public bool? is_ci { get; set; }
		public string? ci_desc { get; set; }
		public bool? is_rci { get; set; }
		public string? rci_desc { get; set; }
		public bool? is_active { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
	
	#region Produced Water Menu
	public class CMInfoProducedWaterWaterAnalysis
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? ph_lastest_period { get; set; }
		public decimal? ph_value { get; set; }
		public string? o2_lastest_period { get; set; }
		public decimal? o2_value { get; set; }
		public int? id_status_o2 { get; set; }
		public string? severity_level_o2 { get; set; }
		public string? color_name_o2 { get; set; }
		public string? color_code_o2 { get; set; }
		public string? ion_lastest_period { get; set; }
		public decimal? ion_value { get; set; }
	}
	public class CMInfoProducedWaterMicroBacteria
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? srb_lastest_period { get; set; }
		public decimal? srb_value { get; set; }
		public string? srb_severity_level { get; set; }
		public string? srb_color_name { get; set; }
		public string? srb_color_code { get; set; }
		public string? atp_lastest_period { get; set; }
		public decimal? atp_value { get; set; }
		public string? atp_severity_level { get; set; }
		public string? atp_color_name { get; set; }
		public string? atp_color_code { get; set; }
		public string? ghb_lastest_period { get; set; }
		public decimal? ghb_value { get; set; }
		public string? ghb_severity_level { get; set; }
		public string? ghb_color_name { get; set; }
		public string? ghb_color_code { get; set; }
		public string? apghb_lastest_period { get; set; }
		public decimal? apghb_value { get; set; }
		public string? apghb_severity_level { get; set; }
		public string? apghb_color_name { get; set; }
		public string? apghb_color_code { get; set; }
		public string? sulphide_lastest_period { get; set; }
		public decimal? sulphide_value { get; set; }
		public string? sulphide_severity_level { get; set; }
		public string? sulphide_color_name { get; set; }
		public string? sulphide_color_code { get; set; }
	}
	public class CMInfoProducedWaterCorrosionCoupon
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? remove_lastest_date { get; set; }
		public decimal? corrosion_rate { get; set; }
		public decimal? max_pit_depth { get; set; }
		public decimal? pitting_rate { get; set; }
	}
	public class CMInfoProducedWaterERProbe
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? record_lastest_date { get; set; }
		public decimal? metal_loss { get; set; }
		public decimal? corrosion_rate { get; set; }
		public string? note { get; set; }
	}
	#endregion
	
	#region Pipeline Menu
	public class CMInfoPipelineWaterAnalysis
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? ph_lastest_period { get; set; }
		public decimal? ph_value { get; set; }
		public string? co2_lastest_period { get; set; }
		public decimal? co2_value { get; set; }
		public string? o2_lastest_period { get; set; }
		public decimal? o2_value { get; set; }
		public int? id_status_o2 { get; set; }
		public string? severity_level_o2 { get; set; }
		public string? color_name_o2 { get; set; }
		public string? color_code_o2 { get; set; }
		public string? ion_lastest_period { get; set; }
		public decimal? ion_value { get; set; }
	}
	public class CMInfoPipelineMicroBacteria
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? srb_lastest_period { get; set; }
		public decimal? srb_value { get; set; }
		public string? srb_severity_level { get; set; }
		public string? srb_color_name { get; set; }
		public string? srb_color_code { get; set; }
		public string? atp_lastest_period { get; set; }
		public decimal? atp_value { get; set; }
		public string? atp_severity_level { get; set; }
		public string? atp_color_name { get; set; }
		public string? atp_color_code { get; set; }
		public string? ghb_lastest_period { get; set; }
		public decimal? ghb_value { get; set; }
		public string? ghb_severity_level { get; set; }
		public string? ghb_color_name { get; set; }
		public string? ghb_color_code { get; set; }
		public string? apghb_lastest_period { get; set; }
		public decimal? apghb_value { get; set; }
		public string? apghb_severity_level { get; set; }
		public string? apghb_color_name { get; set; }
		public string? apghb_color_code { get; set; }
		public string? sulphide_lastest_period { get; set; }
		public decimal? sulphide_value { get; set; }
		public string? sulphide_severity_level { get; set; }
		public string? sulphide_color_name { get; set; }
		public string? sulphide_color_code { get; set; }
	}
	public class CMInfoPipelineCorrosionCoupon
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? remove_lastest_date { get; set; }
		public decimal? corrosion_rate { get; set; }
		public decimal? max_pit_depth { get; set; }
		public decimal? pitting_rate { get; set; }
	}
	public class CMInfoPipelineERProbe
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? record_lastest_date { get; set; }
		public decimal? metal_loss { get; set; }
		public decimal? corrosion_rate { get; set; }
		public string? note { get; set; }
	}
	public class CMInfoPipelineChemInjection
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public decimal? gas_flow_rate_mmscfd { get; set; }
		public decimal? req_ci_injection_rate_liters_mmscfd { get; set; }
		public decimal? req_ci_rate_liters_day { get; set; }
		public decimal? yesterday_ci_tank_level_percent { get; set; }
		public decimal? today_ci_tank_level_percent { get; set; }
		public decimal? actual_ci_injection_liters_day { get; set; }
		public int? id_status { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
	}
	#endregion
	
	#region Cooling Medium
	public class CMInfoCoolingMediumWaterAnalysis
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? ph_lastest_period { get; set; }
		public decimal? ph_value { get; set; }
		public string? o2_lastest_period { get; set; }
		public decimal? o2_value { get; set; }
		public int? id_status_o2 { get; set; }
		public string? severity_level_o2 { get; set; }
		public string? color_name_o2 { get; set; }
		public string? color_code_o2 { get; set; }
		public string? ion_lastest_period { get; set; }
		public decimal? ion_value { get; set; }
	}
	public class CMInfoCoolingMediumMicroBacteria
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? srb_lastest_period { get; set; }
		public decimal? srb_value { get; set; }
		public string? srb_severity_level { get; set; }
		public string? srb_color_name { get; set; }
		public string? srb_color_code { get; set; }
		public string? atp_lastest_period { get; set; }
		public decimal? atp_value { get; set; }
		public string? atp_severity_level { get; set; }
		public string? atp_color_name { get; set; }
		public string? atp_color_code { get; set; }
		public string? ghb_lastest_period { get; set; }
		public decimal? ghb_value { get; set; }
		public string? ghb_severity_level { get; set; }
		public string? ghb_color_name { get; set; }
		public string? ghb_color_code { get; set; }
		public string? apghb_lastest_period { get; set; }
		public decimal? apghb_value { get; set; }
		public string? apghb_severity_level { get; set; }
		public string? apghb_color_name { get; set; }
		public string? apghb_color_code { get; set; }
		public string? sulphide_lastest_period { get; set; }
		public decimal? sulphide_value { get; set; }
		public string? sulphide_severity_level { get; set; }
		public string? sulphide_color_name { get; set; }
		public string? sulphide_color_code { get; set; }
	}
	public class CMInfoCoolingMediumCorrosionCoupon
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? remove_lastest_date { get; set; }
		public decimal? corrosion_rate { get; set; }
		public decimal? max_pit_depth { get; set; }
		public decimal? pitting_rate { get; set; }
	}
	public class CMInfoCoolingMediumERProbe
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? record_lastest_date { get; set; }
		public decimal? metal_loss { get; set; }
		public decimal? corrosion_rate { get; set; }
		public string? note { get; set; }
	}
	#endregion
	
	#region Sea Water
	public class CMInfoSeaWaterWaterAnalysis
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? ph_lastest_period { get; set; }
		public decimal? ph_value { get; set; }
		public string? o2_lastest_period { get; set; }
		public decimal? o2_value { get; set; }
		public int? id_status_o2 { get; set; }
		public string? severity_level_o2 { get; set; }
		public string? color_name_o2 { get; set; }
		public string? color_code_o2 { get; set; }
		public string? ion_lastest_period { get; set; }
		public decimal? ion_value { get; set; }
	}
	public class CMInfoSeaWaterMicroBacteria
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public string? srb_lastest_period { get; set; }
		public decimal? srb_value { get; set; }
		public string? srb_severity_level { get; set; }
		public string? srb_color_name { get; set; }
		public string? srb_color_code { get; set; }
		public string? atp_lastest_period { get; set; }
		public decimal? atp_value { get; set; }
		public string? atp_severity_level { get; set; }
		public string? atp_color_name { get; set; }
		public string? atp_color_code { get; set; }
		public string? ghb_lastest_period { get; set; }
		public decimal? ghb_value { get; set; }
		public string? ghb_severity_level { get; set; }
		public string? ghb_color_name { get; set; }
		public string? ghb_color_code { get; set; }
		public string? apghb_lastest_period { get; set; }
		public decimal? apghb_value { get; set; }
		public string? apghb_severity_level { get; set; }
		public string? apghb_color_name { get; set; }
		public string? apghb_color_code { get; set; }
		public string? sulphide_lastest_period { get; set; }
		public decimal? sulphide_value { get; set; }
		public string? sulphide_severity_level { get; set; }
		public string? sulphide_color_name { get; set; }
		public string? sulphide_color_code { get; set; }
	}
	public class CMInfoSeaWaterCorrosionCoupon
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? remove_lastest_date { get; set; }
		public decimal? corrosion_rate { get; set; }
		public decimal? max_pit_depth { get; set; }
		public decimal? pitting_rate { get; set; }
	}
	public class CMInfoSeaWaterERProbe
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public string? desc { get; set; }
		public DateTime? record_lastest_date { get; set; }
		public decimal? metal_loss { get; set; }
		public decimal? corrosion_rate { get; set; }
		public string? note { get; set; }
	}
	#endregion

	#region Dashboard
	public class PipelineDashboard
	{
		public int id_tag { get; set; }
		public int id_platform { get; set; }
		public string? platform { get; set; }
		public string? tag_no { get; set; }
		public int? id_status_piging { get; set; }
		public string? severity_level_piging { get; set; }
		public string? color_name_piging { get; set; }
		public string? color_code_piging { get; set; }
		public int? id_status_wa { get; set; }
		public string? severity_level_wa { get; set; }
		public string? color_name_wa { get; set; }
		public string? color_code_wa { get; set; }
		public int? id_status_mb { get; set; }
		public string? severity_level_mb { get; set; }
		public string? color_name_mb { get; set; }
		public string? color_code_mb { get; set; }
		public int? id_status_cc { get; set; }
		public string? severity_level_cc { get; set; }
		public string? color_name_cc { get; set; }
		public string? color_code_cc { get; set; }
		public int? id_status_er { get; set; }
		public string? severity_level_er { get; set; }
		public string? color_name_er { get; set; }
		public string? color_code_er { get; set; }
		public int? id_status_ci { get; set; }
		public string? severity_level_ci { get; set; }
		public string? color_name_ci { get; set; }
		public string? color_code_ci { get; set; }
		public int? id_status_rci { get; set; }
		public string? severity_level_rci { get; set; }
		public string? color_name_rci { get; set; }
		public string? color_code_rci { get; set; }
		public string? note { get; set; }
	}
	#endregion
}