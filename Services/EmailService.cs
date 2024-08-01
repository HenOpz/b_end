using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;

namespace CPOC_AIMS_II_Backend.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(EmailModel emailModel, List<MailParamModel> paramModel, string template, List<ErrorLogModel> errorLogs);
		Task SendEmailNotFoundAsync(EmailModel emailModel, List<MailParamModel> paramModel, string template);
	}

	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task SendEmailNotFoundAsync(EmailModel emailModel, List<MailParamModel> paramModel, string template)
		{
			if (emailModel == null)
				throw new ArgumentNullException(nameof(emailModel));

			var smtpServer = _configuration["EmailSettings:SmtpServer"]
							 ?? throw new InvalidOperationException("SMTP server configuration is missing.");
			var portString = _configuration["EmailSettings:Port"]
							 ?? throw new InvalidOperationException("Port configuration is missing.");
			var encrypt = _configuration["EmailSettings:Encrypt"]
						  ?? throw new InvalidOperationException("Encryption configuration is missing.");
			var senderName = _configuration["EmailSettings:SenderName"]
							 ?? throw new InvalidOperationException("Sender name configuration is missing.");
			var senderEmail = _configuration["EmailSettings:SenderEmail"]
							  ?? throw new InvalidOperationException("Sender email configuration is missing.");
			var username = _configuration["EmailSettings:Username"]
						   ?? throw new InvalidOperationException("Username configuration is missing.");
			var password = _configuration["EmailSettings:Password"]
						   ?? throw new InvalidOperationException("Password configuration is missing.");
			var timeoutString = _configuration["EmailSettings:Timeout"]
						?? "10000";
			var recipients = _configuration.GetSection("EmailSettings:Recipients").Get<List<string>>();

			if (!int.TryParse(portString, out var port))
			{
				throw new InvalidOperationException("Port configuration is not a valid number.");
			}
			if (!int.TryParse(timeoutString, out var timeout))
			{
				throw new InvalidOperationException("Timeout configuration is not a valid number.");
			}
			if (recipients == null || !recipients.Any())
			{
				throw new InvalidOperationException("No recipients specified in the configuration.");
			}

			try
			{
				using (var client = new SmtpClient())
				{
					// Set connect timeout
					client.Timeout = timeout;

					if (port != 25)
					{
						await client.ConnectAsync(smtpServer, port, SecureSocketOptions.Auto);
						await client.AuthenticateAsync(username, password);
					}
					else
					{
						await client.ConnectAsync(smtpServer, port, SecureSocketOptions.None);
					}

					var message = new MimeMessage();
					message.From.Add(new MailboxAddress(senderName, senderEmail));

					// foreach (var recipient in emailModel.To)
					// {
					// 	message.To.Add(MailboxAddress.Parse(recipient));
					// }
					recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));

					message.Subject = $"AIMS - {emailModel.Subject}";

					string pathFile = Path.Combine("MailTemplates/" + template + ".html");
					StreamReader smrRead = new StreamReader(pathFile);
					string strHtml = smrRead.ReadToEnd();
					smrRead.Close();

					foreach (MailParamModel param in paramModel)
					{
						strHtml = strHtml.Replace("{{" + param.Param + "}}", param.Value);
					}


					var bodyBuilder = new BodyBuilder();
					bodyBuilder.HtmlBody = strHtml;
					// bodyBuilder.TextBody = $@"
					//     This is an auto-generated notification from AIMS.
					//     Please do not reply to the email address.

					//     ---------------------------------------------------------------------------------

					//     Dear Sir/Madam,

					//     Kindly be informed that there is an error in AIMS for your attention.
					//     Unique Key: {emailModel.UniqueKey}
					//     Notification Description: {emailModel.NotificationDescription}
					//     Functional Location: {emailModel.FunctionalLocation}
					//     Technical Identification: {emailModel.TechnicalIdentification}
					//     Work Centre: {emailModel.WorkCentre}

					//     Error: {emailModel.ErrorDescription}
					//     User Action Required: {emailModel.UserActionRequired}
					// ";

					message.Body = bodyBuilder.ToMessageBody();

					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"Error sending email: {ex.Message}");
				throw; // Rethrow the exception to propagate it
			}
		}

		public async Task SendEmailAsync(EmailModel emailModel, List<MailParamModel> paramModel, string template, List<ErrorLogModel> errorLogs)
		{
			if (emailModel == null)
				throw new ArgumentNullException(nameof(emailModel));

			var smtpServer = _configuration["EmailSettings:SmtpServer"]
							 ?? throw new InvalidOperationException("SMTP server configuration is missing.");
			var portString = _configuration["EmailSettings:Port"]
							 ?? throw new InvalidOperationException("Port configuration is missing.");
			var encrypt = _configuration["EmailSettings:Encrypt"]
						  ?? throw new InvalidOperationException("Encryption configuration is missing.");
			var senderName = _configuration["EmailSettings:SenderName"]
							 ?? throw new InvalidOperationException("Sender name configuration is missing.");
			var senderEmail = _configuration["EmailSettings:SenderEmail"]
							  ?? throw new InvalidOperationException("Sender email configuration is missing.");
			var username = _configuration["EmailSettings:Username"]
						   ?? throw new InvalidOperationException("Username configuration is missing.");
			var password = _configuration["EmailSettings:Password"]
						   ?? throw new InvalidOperationException("Password configuration is missing.");
			var timeoutString = _configuration["EmailSettings:Timeout"]
						?? "10000";
			var recipients = _configuration.GetSection("EmailSettings:Recipients").Get<List<string>>();

			if (!int.TryParse(portString, out var port))
			{
				throw new InvalidOperationException("Port configuration is not a valid number.");
			}
			if (!int.TryParse(timeoutString, out var timeout))
			{
				throw new InvalidOperationException("Timeout configuration is not a valid number.");
			}
			if (recipients == null || !recipients.Any())
			{
				throw new InvalidOperationException("No recipients specified in the configuration.");
			}

			try
			{
				using (var client = new SmtpClient())
				{
					// Set connect timeout
					client.Timeout = timeout;

					if (port != 25)
					{
						await client.ConnectAsync(smtpServer, port, SecureSocketOptions.Auto);
						await client.AuthenticateAsync(username, password);
					}
					else
					{
						await client.ConnectAsync(smtpServer, port, SecureSocketOptions.None);
					}

					var message = new MimeMessage();
					message.From.Add(new MailboxAddress(senderName, senderEmail));

					recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));

					message.Subject = $"AIMS - {emailModel.Subject}";

					string pathFile = Path.Combine("MailTemplates/" + template + ".html");
					StreamReader smrRead = new StreamReader(pathFile);
					string strHtml = smrRead.ReadToEnd();
					smrRead.Close();

					// Generate the HTML for the error logs table
					var errorLogTableRows = new StringBuilder();
					foreach (var log in errorLogs)
					{
						errorLogTableRows.AppendLine("<tr>");
						errorLogTableRows.AppendLine($"<td>{log.UniqueKey}</td>");
						errorLogTableRows.AppendLine($"<td>{log.FileName}</td>");
						errorLogTableRows.AppendLine($"<td>{log.NotificationNo}</td>");
						errorLogTableRows.AppendLine($"<td>{log.NotificationDescription}</td>");
						errorLogTableRows.AppendLine($"<td>{log.WONo}</td>");
						errorLogTableRows.AppendLine($"<td>{log.FunctionalLocation}</td>");
						errorLogTableRows.AppendLine($"<td>{log.TechnicalIdentification}</td>");
						errorLogTableRows.AppendLine($"<td>{log.WorkCentre}</td>");
						errorLogTableRows.AppendLine($"<td>{log.Date}</td>");
						errorLogTableRows.AppendLine($"<td>{log.ErrorMessage}</td>");
						errorLogTableRows.AppendLine($"<td>{log.UserActionSteps}</td>");
						errorLogTableRows.AppendLine("</tr>");
					}

					// Replace placeholders
					strHtml = strHtml.Replace("{{errorLogTableRows}}", errorLogTableRows.ToString());

					foreach (var param in paramModel)
					{
						strHtml = strHtml.Replace("{{" + param.Param + "}}", param.Value);
					}

					var bodyBuilder = new BodyBuilder
					{
						HtmlBody = strHtml
					};

					message.Body = bodyBuilder.ToMessageBody();

					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"Error sending email: {ex.Message}");
				throw; // Rethrow the exception to propagate it
			}
		}

	}
}