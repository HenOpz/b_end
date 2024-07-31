namespace CPOC_AIMS_II_Backend.Models
{
    public class ExInspectionRecord
    {
        public int id { get; set; }
        public int id_info { get; set; }
        public DateTime inspection_date { get; set; }
        public string? project_no { get; set; }
        public string? report_no { get; set; }
        public bool? is_d_type { get; set; }
        public bool? is_c_type { get; set; }
        public bool? is_v_type { get; set; }
        public bool? is_initial { get; set; }
        public bool? is_periodic { get; set; }
        public bool? is_sample { get; set; }
        public string? remark { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
        public bool? is_active { get; set; }
    }
}