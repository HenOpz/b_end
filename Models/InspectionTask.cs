namespace CPOC_AIMS_II_Backend.Models
{
    public class InspectionTask
    {
		[Key]
        public int id { get; set; }
        public int? id_platform { get; set; }
        public int? id_asset { get; set; }
        public int? id_tag { get; set; }
        public int? id_insp_type { get; set; }
        public int? id_insp_task_status { get; set; }
        public string? it_number { get; set; }
        public string? tag_number { get; set; }
        public DateTime? due_insp_date { get; set; }
        public DateTime? plan_insp_date { get; set; }
        public decimal? plan_manhours { get; set; }
        public decimal? actual_manhours { get; set; }
        public int? insp_effectiveness { get; set; }
        public string? note { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
        public bool? is_active { get; set; }
    }
}