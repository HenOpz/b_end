using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPOC_AIMS_II_Backend.Models
{
    public class SapHeaderTXN
    {
        public int id { get; set; }
        public string? txn_desc { get; set; }
        public DateTime txn_datetime { get; set; }
        public string? status { get; set; }
        public string? note { get; set; }
    }
}