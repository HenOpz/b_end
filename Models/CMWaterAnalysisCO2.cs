namespace CPOC_AIMS_II_Backend.Models
{
    public class CMWaterAnalysisCO2
    {
        public int id { get; set; }
        public int id_tag { get; set; }
        public int? year { get; set; }
        public string? period { get; set; }
        public decimal? co2_val { get; set; }
        public int? id_status { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
}