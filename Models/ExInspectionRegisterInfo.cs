using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPOC_AIMS_II_Backend.Models
{
	public class ExInspectionRegisterInfo
	{
		[Key]
		public int id { get; set; }
		public int? id_platfrom { get; set; }
		public string? tag_no { get; set; }
		public string? equip_desc { get; set; }
		public string? location { get; set; }
		public int? system { get; set; }
		public string? system_desc { get; set; }
		public int? id_area_standard { get; set; }
		public int? id_area_class { get; set; }
		public int? id_area_temp_class { get; set; }
		public int? id_area_gas_group { get; set; }
		public int? id_equip_standard { get; set; }
		public int? equip_protection_tag { get; set; }
		public int? id_equip_protection_level_category { get; set; }
		public int? id_equip_protection_type { get; set; }
		public int? id_equip_type { get; set; }
		public int? id_equip_class { get; set; }
		public int? id_equip_temp_class { get; set; }
		public int? id_equip_gas_group { get; set; }
		public int? id_equip_ip_rating { get; set; }
		public int? id_equip_enclosure_type { get; set; }
		public string? equip_manufacturer { get; set; }
		public string? model { get; set; }
		public string? serial_no { get; set; }
		public string? certifying_body { get; set; }
		public string? ex_cert_no { get; set; }
		public string? drawing_ref { get; set; }
		public DateTime? installation_date { get; set; }
		public string? operating_during_esd { get; set; }
		public string? category_inst_elect { get; set; }
		public int? id_water_corrosion_chemicals_status { get; set; }
		public int? id_dust_sand_status { get; set; }
		public int? id_uv_radiation_status { get; set; }
		public int? id_ambient_temp_status { get; set; }
		public int? id_temp_cycling_status { get; set; }
		public string? equip_remark { get; set; }
		public string? overview_img_path { get; set; }
		public bool? is_active { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
	public class ExInspectionRegisterInfoView
	{
		public int id { get; set; }
		public int? id_platfrom { get; set; }
		public string? platform_name { get; set; }
		public string? platform_full_name { get; set; }
		public int? phase { get; set; }
		public string? planning_plant { get; set; }
		public string? tag_no { get; set; }
		public string? equip_desc { get; set; }
		public string? location { get; set; }
		public int? system { get; set; }
		public string? system_desc { get; set; }
		public int? id_area_standard { get; set; }
		public string? area_standard { get; set; }
		public int? id_area_class { get; set; }
		public string? area_class { get; set; }
		public int? id_area_temp_class { get; set; }
		public string? area_temp_class { get; set; }
		public int? id_area_gas_group { get; set; }
		public string? area_gas_group { get; set; }
		public int? id_equip_standard { get; set; }
		public string? equip_standard { get; set; }
		public int? equip_protection_tag { get; set; }
		public int? id_equip_protection_level_category { get; set; }
		public string? equip_protection_level_category { get; set; }
		public int? id_equip_protection_type { get; set; }
		public string? equip_protection_type { get; set; }
		public int? id_equip_type { get; set; }
		public string? equip_type { get; set; }
		public int? id_equip_class { get; set; }
		public string? equip_class { get; set; }
		public int? id_equip_temp_class { get; set; }
		public string? equip_temp_class { get; set; }
		public string? equip_temp_class_desc { get; set; }
		public int? id_equip_gas_group { get; set; }
		public string? equip_gas_group { get; set; }
		public int? id_equip_ip_rating { get; set; }
		public string? equip_ip_rating { get; set; }
		public int? id_equip_enclosure_type { get; set; }
		public string? equip_enclosure_type { get; set; }
		public string? equip_manufacturer { get; set; }
		public string? model { get; set; }
		public string? serial_no { get; set; }
		public string? certifying_body { get; set; }
		public string? ex_cert_no { get; set; }
		public string? drawing_ref { get; set; }
		public DateTime? installation_date { get; set; }
		public string? operating_during_esd { get; set; }
		public string? category_inst_elect { get; set; }
		public int? id_water_corrosion_chemicals_status { get; set; }
		public int? id_dust_sand_status { get; set; }
		public int? id_uv_radiation_status { get; set; }
		public int? id_ambient_temp_status { get; set; }
		public int? id_temp_cycling_status { get; set; }
		public string? equip_remark { get; set; }
		public string? overview_img_path { get; set; }
		public bool? is_active { get; set; }
		public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
	}
	public class AttachPic
	{
		public int id_tag { get; set; }
		public IFormFile? file { get; set; }
		public string? file_path { get; set; }
		public int type { get; set; }
	}
}