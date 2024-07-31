namespace CPOC_AIMS_II_Backend.Models
{
	public class ExInspectionPictureLog
	{
		[Key]
		public int id { get; set; }
		public int id_inspection_record { get; set; }
		public int? id_chk_result { get; set; }
		public string? pic_path_1 { get; set; }
		public string? pic_path_2 { get; set; }
		public string? finding { get; set; }
		public string? recommendation { get; set; }
		public int id_finding_status { get; set; }
		public string? deenergize { get; set; }
		public string? quick_fix { get; set; }
		public string? interim_measure { get; set; }
		public string? interim_measure_validity { get; set; }
		public string? finding_comp_status { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}public class ExInspectionPictureLogView
	{
		public int id { get; set; }
		public int id_inspection_record { get; set; }
		public DateTime? inspection_date { get; set;}
		public int? id_chk_result { get; set; }
		public string? checklist_no_ref { get; set; }
		public string? pic_path_1 { get; set; }
		public string? pic_path_2 { get; set; }
		public string? finding { get; set; }
		public string? recommendation { get; set; }
		public int id_finding_status { get; set; }
		public string? finding_status { get; set; }
		public string? deenergize { get; set; }
		public string? quick_fix { get; set; }
		public string? interim_measure { get; set; }
		public string? interim_measure_validity { get; set; }
		public string? finding_comp_status { get; set; }
		public int? created_by { get; set; }
		public string? created_by_name { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public string? updated_by_name { get; set; }
		public DateTime? updated_date { get; set; }
	}
	public class ExInspectionPictureLogUpload
	{
		[Key]
		public int id { get; set; }
		public int id_inspection_record { get; set; }
		public int? id_chk_result { get; set; }
		public IFormFile? file_1 { get; set; }
		public string? pic_path_1 { get; set; }
		public IFormFile? file_2 { get; set; }
		public string? pic_path_2 { get; set; }
		public string? finding { get; set; }
		public string? recommendation { get; set; }
		public int id_finding_status { get; set; }
		public string? deenergize { get; set; }
		public string? quick_fix { get; set; }
		public string? interim_measure { get; set; }
		public string? interim_measure_validity { get; set; }
		public string? finding_comp_status { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
}