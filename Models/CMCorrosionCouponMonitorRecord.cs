namespace CPOC_AIMS_II_Backend.Models
{
    public class CMCorrosionCouponMonitorRecord
    {
        public int id { get; set; }
        public int id_tag { get; set; }
        public DateTime? install_date { get; set; }
        public DateTime? remove_date { get; set; }
        public int? expose_time { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }

    }
}