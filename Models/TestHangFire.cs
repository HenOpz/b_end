namespace CPOC_AIMS_II_Backend.Models
{
    public class TestHangFire
    {
        [Key]
        public int id { get; set; }
        public string? content { get; set; }
        public DateTime? input_time { get; set; }
    }
}