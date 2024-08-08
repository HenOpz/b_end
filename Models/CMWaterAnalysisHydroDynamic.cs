namespace CPOC_AIMS_II_Backend.Models
{
    public class CMWaterAnalysisHydroDynamic
    {
        public int id { get; set; }
        public int id_tag { get; set; }
        public DateTime? record_date { get; set; }
        public decimal? cr_90_per_mm_yr { get; set; }
        public decimal? cr_95_per_mm_yr { get; set; }
        public decimal? cr_99_per_mm_yr { get; set; }
        public decimal? cr_not_injected_mm_yr { get; set; }
        public int? id_status { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
}