namespace CPOC_AIMS_II_Backend.Models
{
    public class GpiRecordTXN
    {
		[Key]
        public int id { get; set; }
        public int id_gpi { get; set; }
        public int? id_user { get; set; }
        public int seq { get; set; }
        public int id_status { get; set; }
        public string? remark { get; set; }
        public DateTime txn_datetime { get; set; }
    }
}