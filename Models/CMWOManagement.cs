namespace CPOC_AIMS_II_Backend.Models
{
    public class CMWOManagement
    {
        public int id { get; set; }
        public DateTime record_month { get; set; }
        public int? cm_total { get; set; }
        public int cm_closed { get; set; }
        public int cm_opened { get; set; }
        public string? note { get; set; }
    }
}