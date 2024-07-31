namespace CPOC_AIMS_II_Backend.Models
{
	public class RectificationCampaignFile
	{
		public int id { get; set; }
		public int? id_recti_campaign { get; set; }
		public string? file_path { get; set; }
		public string? file_type { get; set; }
		public string? file_name { get; set; }
		public string? note { get; set; }
		public int? created_by { get; set; }
		public DateTime? created_date { get; set; }
		public int? updated_by { get; set; }
		public DateTime? updated_date { get; set; }
	}
	
	public class RectificationCampaignFileUpload
    {
        public int id { get; set; }
        public int? id_recti_campaign { get; set; }
        public IFormFile? file { get; set; }
        public string? file_path { get; set; }
        public string? file_type { get; set; }
        public string? file_name { get; set; }
        public string? note { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
}