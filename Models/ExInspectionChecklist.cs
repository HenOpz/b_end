namespace CPOC_AIMS_II_Backend.Models
{
	public class ExInspectionChecklist
	{
		public int id { get; set; }
		public string? checklist_no { get; set; }
		public string? checklist_no_ref { get; set; }
		public string? checklist_content { get; set; }
		public bool? is_header { get; set; }
		public bool? is_subheader { get; set; }
		public string? d_type { get; set; }
		public string? c_type { get; set; }
		public string? v_type { get; set; }
		public int? sort_no { get; set; }
	}
	
	public class ExInspectionChecklistShow
	{
		public int id { get; set; }
		public string? checklist_no { get; set; }
		public string? checklist_no_ref { get; set; }
		public string? checklist_content { get; set; }
		public bool? is_header { get; set; }
		public bool? is_subheader { get; set; }
		public string? d_type { get; set; }
		public string? c_type { get; set; }
		public string? v_type { get; set; }
		public int? sort_no { get; set; }
		public ExInspectionChecklistResult? result  { get; set; }
	}

	public class ExInspectionChecklistResult
	{
		public int id { get; set; }
		public int? id_chk { get; set; }
		public int? id_inspection_record { get; set; }
		public int? id_result { get; set; }
		public string? comment { get; set; }
	}
}