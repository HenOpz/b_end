using System.ComponentModel.DataAnnotations.Schema;

namespace CPOC_AIMS_II_Backend.Models
{
	public class CMCorrosionCouponMonitorDetail
	{
		public int id { get; set; }
		public int? id_record { get; set; }
		public string? coupon_id { get; set; }
		public string? material { get; set; }
		public decimal density { get; set; }
		public decimal area { get; set; }
		[Column(TypeName = "decimal(18, 4)")]
		public decimal initial_weight { get; set; }
		[Column(TypeName = "decimal(18, 4)")]
		public decimal? final_weight { get; set; }
		[Column(TypeName = "decimal(18, 4)")]
		public decimal? weight_loss { get; set; }
		[Column(TypeName = "decimal(18, 4)")]
		public decimal? corrosion_rate { get; set; }
		[Column(TypeName = "decimal(18, 4)")]		
		public decimal? max_pit_depth { get; set; }
		[Column(TypeName = "decimal(18, 4)")]
		public decimal? pitting_rate { get; set; }
		public int? id_corrosion_rate_status { get; set; }
		public int? id_pitting_rate_status { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
}