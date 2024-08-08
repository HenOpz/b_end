namespace CPOC_AIMS_II_Backend.Models
{
	public class FailureRecordTXN
	{
		[Key]
		public int id { get; set; }
		public int id_failure { get; set; }
		public int? id_user { get; set; }
		public int? id_user_info { get; set; }
		public int seq { get; set; }
		public int id_status { get; set; }
		public string? remark { get; set; }
		public DateTime txn_datetime { get; set; }
	}
	public class FailureRecordApprovalSeq
	{
		public int? id_user { get; set; }
		public int? id_user_info { get; set; }
		public int seq { get; set; }
		public int id_status { get; set; }
		public DateTime txn_datetime { get; set; }
		public string? name { get; set; }
		public string? position { get; set; }
		public string? signature { get; set; }
		public string? company { get; set; }
	}
}