namespace CPOC_AIMS_II_Backend.Models
{
	public class EmailModel
	{
		public List<string> To { get; set; } = new List<string>();
		public string? Subject { get; set; }
		public string? UniqueKey { get; set; }
		public string? NotificationDescription { get; set; }
		public string? FunctionalLocation { get; set; }
		public string? TechnicalIdentification { get; set; }
		public string? WorkCentre { get; set; }
		public string? UserActionRequired { get; set; }
		public string? DateTime { get; set; }
	}

	public class MailParamModel
	{
		public required string Param { get; set; }
		public required string Value { get; set; }
	}
	public class ErrorLogModel
    {
        public string? UniqueKey { get; set; }
        public string? FileName { get; set; }
        public string? NotificationNo { get; set; }
        public string? NotificationDescription { get; set; }
        public string? WONo { get; set; }
        public string? FunctionalLocation { get; set; }
        public string? TechnicalIdentification { get; set; }
        public string? WorkCentre { get; set; }
        public string? Date { get; set; }
        public string? ErrorMessage { get; set; }
        public string? UserActionSteps { get; set; }
    }
}