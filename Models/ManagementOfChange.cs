using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPOC_AIMS_II_Backend.Models
{
    public class ManagementOfChange
    {
		[Key]
        public int id { get; set; }
        public int? id_moc_noc { get; set; }
        public int? id_moc_rrl { get; set; }
        public int? id_moc_status { get; set; }
        public string? moc_number { get; set; }
        public string? worksite { get; set; }
        public string? title { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public string? action { get; set; }
        public string? remark { get; set; }
        public string? initiator { get; set; }
        public string? moc_file_path { get; set; }
        public string? moc_file_name { get; set; }
        public string? ra_file_path { get; set; }
        public string? ra_file_name { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
        public bool? is_active { get; set; }
    }
}