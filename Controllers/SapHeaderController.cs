using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using CPOC_AIMS_II_Backend.Services;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SapHeaderController : ControllerBase
	{
		private readonly MainDbContext _context;
		private readonly IEmailService _emailService;
		public SapHeaderController(MainDbContext context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<SapHeader>>> GetSapHeaderList()
		{
			var data = await _context.SapHeader.Where(a => a.is_active == true).ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<SapHeader>> GetSapHeaderById(int id)
		{
			var data = await _context.SapHeader.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet]
		[Route("get-sap-header-by-module-from-module")]
		public async Task<ActionResult<SapHeader>> GetSapHeaderByModule(int id_module, int id_from_module)
		{
			var data = await _context.SapHeader.Where(a => (a.is_active == true) && (a.id_module == id_module) && (a.id_from_module == id_from_module)).ToListAsync();

			if (data.Count > 1)
			{
				return BadRequest("Something Wrong Sapheader has more than 1 record.");
			}

			if (data.Count == 0)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		[Route("get-export-sap-header")]
		public async Task<IActionResult> GetExportSapHeader()
		{
			var data = await _context.SapHeader.Where(a => a.is_active == true).ToListAsync();
			if (data.Count == 0)
			{
				//Create log record
				await SapHeaderRecordTXN("AIMS-SAP", "Fail", "No records available for export at the moment");
				return NotFound($"No records available for export at the moment");
			}

			String separator = ";";
			StringBuilder output = new StringBuilder();
			String[] headings = { "Unique Key", "Notification Type", "Description", "Functional Location", "Equipment No", "Technical Identification", "Planner Grp",
			"Planner Grp Planning Plant", "Main WorkCtr", "Reported by", "Required Start date", "Required Start Time", "Required End date",
			"Required End Time", "Priority", "Code Grp Object Part", "Object Part Code", "Code Grp Damage", "Damage Code", "Damage Free text", "Code grp Cause",
			"Cause Code", "Cause grp. Free text", "Addtional Data", "User Status", "ABC Indicator", "Plant Section", "Notification No"};
			output.AppendLine(string.Join(separator, headings));

			foreach (SapHeader i in data)
			{
				String[] newLine = {
					i.id_reference ?? "",
					i.notification_type ?? "",
					i.description ?? "",
					i.functional_location ?? "",
					i.equipment_no ?? "",
					i.technical_identification ?? "",
					i.planner_grp ?? "",
					i.planner_grp_planning_plant ?? "",
					i.main_workctr ?? "",
					i.reported_by  ?? "",
					i.required_start_date ?? "",
					i.required_start_time ?? "",
					i.required_end_date ?? "",
					i.required_end_time ?? "",
					i.priority ?? "",
					i.code_grp_object_part ?? "",
					i.object_part_code ?? "",
					i.code_grp_damage ?? "",
					i.damage_code ?? "",
					i.damage_free_text ?? "",
					i.code_grp_cause ?? "",
					i.cause_code ?? "",
					i.cause_grp_free_text ?? "",
					i.additional_data ?? "",
					i.user_status ?? "",
					i.abc_indicator ?? "",
					i.accessibility ?? "",
					i.notification ?? "",
				};

				// Check last field is empty
				// if (string.IsNullOrEmpty(newLine[newLine.Length - 1]))
				// {
				//     newLine[newLine.Length - 1] = ";";
				// }

				output.AppendLine(string.Join(separator, newLine));
			}

			DateTime currentDate = DateTime.Now;
			string formattedDate = currentDate.ToString("yyyyMMdd_HHmmss");
			string fileName = $"AIMS_{formattedDate}.csv";
			string filePath = "";

			try
			{
				string? folderPath = Startup.AimsSapPath; ;

				if (folderPath == "")
				{
					folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "D01", "AIMS-SAP");
				}

				if (!Directory.Exists(folderPath)) return NotFound($"The folder path {folderPath} was not found.");

				filePath = Path.Combine(folderPath, fileName);

				System.IO.File.WriteAllText(filePath, output.ToString());
				//System.IO.File.WriteAllText(fileName, output.ToString());

			}
			catch (Exception ex)
			{
				//Create log record
				await SapHeaderRecordTXN("AIMS-SAP", "Fail", "Data could not be written to the CSV file");

				// var emailModel = new EmailModel
				// {
				// 	To = new List<string> { "weerawat.suwattanapiset@dexon-technology.com"}, // replace with the actual recipient email
				// 	ErrorDescription = ex.Message,
				// 	UniqueKey = "123456", // replace with actual unique key
				// 	NotificationDescription = "Data could not be written to the CSV file",
				// 	FunctionalLocation = "Sample Functional Location", // replace with actual data
				// 	TechnicalIdentification = "Sample Technical Identification", // replace with actual data
				// 	WorkCentre = "Sample Work Centre", // replace with actual data
				// 	UserActionRequired = "Please check the application logs for more details." // provide actual user action steps
				// };
				// await _emailService.SendEmailAsync(emailModel);

				return StatusCode(500, $"Data could not be written to the CSV file : " + ex.Message);
			}
			//Create log record
			await SapHeaderRecordTXN("AIMS-SAP", "Success", $"Export file success {Path.GetFileName(filePath)}");
			return Ok($"Export File : {filePath}");
			//return Ok($"Export File : {fileName}");
		}

		private const int MaxRetryCount = 2;
		private const int DelayMinutes = 1;

		[HttpPut]
		[Route("import-sap-headers")]
		public async Task<IActionResult> ImportSapHeaders()
		{
			int retryCount = 0;

			while (retryCount < MaxRetryCount)
			{
				var importedData = new List<SapHeader?>();
				var errorLogs = new List<string>();
				var errorLogsList = new List<ErrorLogModel>();

				var validFunctionalLocations = await _context.MdSapFunctionalLocation.Select(fl => fl.functional_location).ToListAsync();
				var validMainWorkCtrs = await _context.MdSapMainWorkCtr.Select(mw => mw.code).ToListAsync();
				var validObjectPartCodes = await _context.MdSapObjectPartCode.Select(opc => opc.object_part_code).ToListAsync();
				var validDamageCodes = await _context.MdSapDamageCode.Select(dc => dc.damage_code).ToListAsync();
				var validCauseCodes = await _context.MdSapCauseCode.Select(cc => cc.cause_code).ToListAsync();
				var validAccessibility = await _context.MdSapAccessibility.Select(ac => ac.code).ToListAsync();

				string? folderPath = Startup.SapAimsPath;
				if (string.IsNullOrEmpty(folderPath))
				{
					folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "D01", "SAP-AIMS");
				}

				if (!Directory.Exists(folderPath))
					return NotFound($"The folder path {folderPath} was not found.");

				string processedFolder = Path.Combine(folderPath, "PROCESSED");
				if (!Directory.Exists(processedFolder))
					Directory.CreateDirectory(processedFolder);

				string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
				if (csvFiles.Length == 0)
				{
					retryCount++;

					// Create log record
					await SapHeaderRecordTXN("SAP-AIMS", "Fail", "File to import not found: " + retryCount.ToString() + " Attempt");

					if (retryCount >= MaxRetryCount)
					{
						// Case of no file from SAP
						List<MailParamModel> paramModel = new List<MailParamModel>
						{
							new MailParamModel { Param = "ErrorDescription", Value = "File to import not found in SAP-AIMS folder" },
							new MailParamModel { Param = "DateTime", Value = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") },
							new MailParamModel { Param = "UserActionRequired", Value = "Please contact SAP Team" }
						};

						var emailModel = new EmailModel
						{
							To = new List<string> { "weerawat.suwattanapiset@dexon-technology.com", "aritouch.sumpaothong@dexon-technology.com"},
							//To = new List<string> { "weerawat.suwattanapiset@dexon-technology.com", "aritouch.sumpaothong@dexon-technology.com", "piyanant.sri@hotmail.com" },
							Subject = "File to import not found in SAP-AIMS folder",
						};

						await _emailService.SendEmailNotFoundAsync(emailModel, paramModel, "SapHeaderNotFound");
						return StatusCode(404, "File to import not found in SAP-AIMS folder.");
					}
					await Task.Delay(TimeSpan.FromMinutes(DelayMinutes));
					continue;
				}

				foreach (string filePath in csvFiles)
				{
					bool hasError = false;
					string fileErrorMsg = "";

					using (var transaction = await _context.Database.BeginTransactionAsync())
					{
						try
						{
							using (var reader = new StreamReader(filePath))
							using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
							{
								csv.Context.TypeConverterCache.AddConverter<DateTime>(new CustomDateConverter());
								csv.Read();
								csv.ReadHeader();

								if (Path.GetFileName(filePath).EndsWith("_U.csv"))
								{
									if (csv.HeaderRecord.Length != 42)
									{
										string errorMessage = $"System Error: CSV file has missing fields in file {Path.GetFileName(filePath)}";
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Fail", errorMessage);
										transaction.Commit();
										errorLogs.Add(errorMessage);
										errorLogsList.Add(new ErrorLogModel
										{
											UniqueKey = "None",
											FileName = Path.GetFileName(filePath),
											NotificationNo = "None",
											NotificationDescription = "None",
											WONo = "None",
											FunctionalLocation = "None",
											TechnicalIdentification = "None",
											WorkCentre = "None",
											Date = DateTime.Now.ToString("dd/MM/yyyy"),
											ErrorMessage = errorMessage,
											UserActionSteps = "Please contact SAP Team"
										});
										continue;
									}

									while (csv.Read())
									{
										var functionalLocationCSV = csv.GetField(7);
										var mainWorkCtrCSV = csv.GetField(12);
										var objectPartCodeCSV = csv.GetField(28);
										var damageCodeCSV = csv.GetField(30);
										var causeCodeCSV = csv.GetField(33);
										var accessibilityCSV = csv.GetField(38);

										bool isValid = true;
										string rowErrorMsg = "";

										if (!validFunctionalLocations.Contains(functionalLocationCSV))
										{
											string error = $" Invalid functional location '{functionalLocationCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validMainWorkCtrs.Contains(mainWorkCtrCSV))
										{
											string error = $" Invalid main work center '{mainWorkCtrCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validObjectPartCodes.Contains(objectPartCodeCSV))
										{
											string error = $" Invalid object part code '{objectPartCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validDamageCodes.Contains(damageCodeCSV))
										{
											string error = $" Invalid damage code '{damageCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validCauseCodes.Contains(causeCodeCSV))
										{
											string error = $" Invalid cause code '{causeCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validAccessibility.Contains(accessibilityCSV))
										{
											string error = $" Invalid accessibility code '{accessibilityCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(3),
												WONo = csv.GetField(5),
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(9),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										var sapHeader = new SapHeader
										{
											id_reference = csv.GetField(0),
											notification = csv.GetField(1),
											notification_date = csv.GetField<DateTime?>(2),
											notification_desc = csv.GetField(3), // New
											notification_type = csv.GetField(4),
											wo_order_no = csv.GetField(5),
											description = csv.GetField(6),
											functional_location = functionalLocationCSV,
											equipment_no = csv.GetField(8),
											technical_identification = csv.GetField(9),
											planner_grp = csv.GetField(10),
											planner_grp_planning_plant = csv.GetField(11),
											main_workctr = mainWorkCtrCSV,
											reported_by = csv.GetField(13),
											basic_start_date = csv.GetField<DateTime?>(14),
											basic_finish_date = csv.GetField<DateTime?>(15),
											required_start_date = csv.GetField(16),
											required_start_time = csv.GetField(17),
											required_end_date = csv.GetField(18),
											required_end_time = csv.GetField(19),
											total_man_hours = csv.GetField<decimal?>(20),
											unit_for_work = csv.GetField(21),
											manpower = csv.GetField<int?>(22),
											duration = csv.GetField<decimal?>(23),
											normal_duration_unit = csv.GetField(24),
											actual_work = csv.GetField<decimal?>(25),
											priority = csv.GetField(26),
											code_grp_object_part = csv.GetField(27),
											object_part_code = objectPartCodeCSV,
											code_grp_damage = csv.GetField(29),
											damage_code = damageCodeCSV,
											damage_free_text = csv.GetField(31),
											code_grp_cause = csv.GetField(32),
											cause_code = causeCodeCSV,
											cause_grp_free_text = csv.GetField(34),
											user_status = csv.GetField(35),
											notification_user_status = csv.GetField(36), // New
											abc_indicator = csv.GetField(37),
											accessibility = accessibilityCSV,
											system_status = csv.GetField(39),
											notification_system_status = csv.GetField(40), // New
											changed_on = csv.GetField<DateTime?>(41),
											err_msg = rowErrorMsg,
										};

										if (!isValid)
										{
											hasError = true;
											fileErrorMsg += rowErrorMsg;
											importedData.Add(sapHeader);
											continue;
										}

										var sh = await _context.SapHeader.FirstOrDefaultAsync(s => s.id_reference == sapHeader.id_reference);
										if (sh != null)
										{
											sh.notification = sapHeader.notification;
											sh.notification_desc = sapHeader.notification_desc; // New
											sh.notification_date = sapHeader.notification_date;
											sh.notification_type = sapHeader.notification_type;
											sh.wo_order_no = sapHeader.wo_order_no;
											sh.description = sapHeader.description;
											sh.functional_location = sapHeader.functional_location;
											sh.equipment_no = sapHeader.equipment_no;
											sh.technical_identification = sapHeader.technical_identification;
											sh.planner_grp = sapHeader.planner_grp;
											sh.planner_grp_planning_plant = sapHeader.planner_grp_planning_plant;
											sh.main_workctr = sapHeader.main_workctr;
											sh.reported_by = sapHeader.reported_by;
											sh.basic_start_date = sapHeader.basic_start_date;
											sh.basic_finish_date = sapHeader.basic_finish_date;
											sh.required_start_date = sapHeader.required_start_date;
											sh.required_start_time = sapHeader.required_start_time;
											sh.required_end_date = sapHeader.required_end_date;
											sh.required_end_time = sapHeader.required_end_time;
											sh.total_man_hours = sapHeader.total_man_hours;
											sh.unit_for_work = sapHeader.unit_for_work;
											sh.manpower = sapHeader.manpower;
											sh.duration = sapHeader.duration;
											sh.normal_duration_unit = sapHeader.normal_duration_unit;
											sh.actual_work = sapHeader.actual_work;
											sh.priority = sapHeader.priority;
											sh.code_grp_object_part = sapHeader.code_grp_object_part;
											sh.object_part_code = sapHeader.object_part_code;
											sh.code_grp_damage = sapHeader.code_grp_damage;
											sh.damage_code = sapHeader.damage_code;
											sh.damage_free_text = sapHeader.damage_free_text;
											sh.code_grp_cause = sapHeader.code_grp_cause;
											sh.cause_code = sapHeader.cause_code;
											sh.cause_grp_free_text = sapHeader.cause_grp_free_text;
											sh.user_status = sapHeader.user_status;
											sh.notification_user_status = sapHeader.notification_user_status; // New
											sh.abc_indicator = sapHeader.abc_indicator;
											sh.accessibility = sapHeader.accessibility;
											sh.system_status = sapHeader.system_status;
											sh.notification_system_status = sapHeader.notification_system_status; // New
											sh.changed_on = sapHeader.changed_on;
											sh.err_msg = sapHeader.err_msg;
											_context.SapHeader.Update(sh);
										}
										else
										{
											_context.SapHeader.Add(sapHeader);
										}

										importedData.Add(sapHeader);
									}

									await _context.SaveChangesAsync();
									transaction.Commit();

									if (hasError)
									{
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Partial", "File imported with errors: " + Path.GetFileName(filePath));
										errorLogs.Add("File imported with errors: " + Path.GetFileName(filePath) + " Errors: " + fileErrorMsg);
									}
									else
									{
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Success", "File imported successfully: " + Path.GetFileName(filePath));
									}
								}
								else if (Path.GetFileName(filePath).EndsWith("_C.csv")) {
									if (csv.HeaderRecord.Length != 29) 
									{
										string errorMessage = $"System Error: CSV file has missing fields in file {Path.GetFileName(filePath)}";
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Fail", errorMessage);
										transaction.Commit();
										errorLogs.Add(errorMessage);
										errorLogsList.Add(new ErrorLogModel
										{
											UniqueKey = "None",
											FileName = Path.GetFileName(filePath),
											NotificationNo = "None",
											NotificationDescription = "None",
											WONo = "None",
											FunctionalLocation = "None",
											TechnicalIdentification = "None",
											WorkCentre = "None",
											Date = DateTime.Now.ToString("dd/MM/yyyy"),
											ErrorMessage = errorMessage,
											UserActionSteps = "Please contact SAP Team"
										});
										continue;
									}

									while (csv.Read())
									{

										var functionalLocationCSV = csv.GetField(5);
										var mainWorkCtrCSV = csv.GetField(10);
										var objectPartCodeCSV = csv.GetField(18);
										var damageCodeCSV = csv.GetField(20);
										var causeCodeCSV = csv.GetField(23);
										var accessibilityCSV = csv.GetField(27);

										bool isValid = true;
										string rowErrorMsg = "";
									

										if (!validFunctionalLocations.Contains(functionalLocationCSV))
										{
											string error = $" Invalid functional location '{functionalLocationCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validMainWorkCtrs.Contains(mainWorkCtrCSV))
										{
											string error = $" Invalid main work center '{mainWorkCtrCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validObjectPartCodes.Contains(objectPartCodeCSV))
										{
											string error = $" Invalid object part code '{objectPartCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validDamageCodes.Contains(damageCodeCSV))
										{
											string error = $" Invalid damage code '{damageCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validCauseCodes.Contains(causeCodeCSV))
										{
											string error = $" Invalid cause code '{causeCodeCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										if (!validAccessibility.Contains(accessibilityCSV))
										{
											string error = $" Invalid accessibility code '{accessibilityCSV}'";
											errorLogsList.Add(new ErrorLogModel
											{
												UniqueKey = csv.GetField(0),
												FileName = Path.GetFileName(filePath),
												NotificationNo = csv.GetField(1),
												NotificationDescription = csv.GetField(4),
												WONo = "",
												FunctionalLocation = functionalLocationCSV,
												TechnicalIdentification = csv.GetField(7),
												WorkCentre = mainWorkCtrCSV,
												Date = DateTime.Now.ToString("dd/MM/yyyy"),
												ErrorMessage = error,
												UserActionSteps = "Please contact SAP Team"
											});
											rowErrorMsg += error;
											isValid = false;
										}

										var sapHeader = new SapHeader
										{
											id_reference = csv.GetField(0),
											notification = csv.GetField(1),
											notification_date = csv.GetField<DateTime?>(2),
											notification_type = csv.GetField(3),
											notification_desc = csv.GetField(4), //New
											functional_location = functionalLocationCSV,
											equipment_no = csv.GetField(6),
											technical_identification = csv.GetField(7),
											planner_grp = csv.GetField(8),
											planner_grp_planning_plant = csv.GetField(9),
											main_workctr = mainWorkCtrCSV,
											reported_by = csv.GetField(11),
											required_start_date = csv.GetField(12),
											required_start_time = csv.GetField(13),
											required_end_date = csv.GetField(14),
											required_end_time = csv.GetField(15),
											priority = csv.GetField(16),
											code_grp_object_part = csv.GetField(17),
											object_part_code = objectPartCodeCSV,
											code_grp_damage = csv.GetField(19),
											damage_code = damageCodeCSV,
											damage_free_text = csv.GetField(21),
											code_grp_cause = csv.GetField(22),
											cause_code = causeCodeCSV,
											cause_grp_free_text = csv.GetField(24),
											notification_user_status = csv.GetField(25), //New
											abc_indicator = csv.GetField(26),
											accessibility = accessibilityCSV,
											notification_system_status = csv.GetField(28), //New
											err_msg = rowErrorMsg,
										};

										if (!isValid)
										{
											hasError = true;
											fileErrorMsg += rowErrorMsg;
											importedData.Add(sapHeader);
											continue;
										}

										var sh = await _context.SapHeader.FirstOrDefaultAsync(s => s.id_reference == sapHeader.id_reference);
										if (sh != null)
										{
											sh.notification = sapHeader.notification;
											sh.notification_desc = sapHeader.notification_desc; //New
											sh.notification_date = sapHeader.notification_date;
											sh.notification_type = sapHeader.notification_type;
											sh.description = sapHeader.description;
											sh.functional_location = sapHeader.functional_location;
											sh.equipment_no = sapHeader.equipment_no;
											sh.technical_identification = sapHeader.technical_identification;
											sh.planner_grp = sapHeader.planner_grp;
											sh.planner_grp_planning_plant = sapHeader.planner_grp_planning_plant;
											sh.main_workctr = sapHeader.main_workctr;
											sh.reported_by = sapHeader.reported_by;
											sh.required_start_date = sapHeader.required_start_date;
											sh.required_start_time = sapHeader.required_start_time;
											sh.required_end_date = sapHeader.required_end_date;
											sh.required_end_time = sapHeader.required_end_time;
											sh.priority = sapHeader.priority;
											sh.code_grp_object_part = sapHeader.code_grp_object_part;
											sh.object_part_code = sapHeader.object_part_code;
											sh.code_grp_damage = sapHeader.code_grp_damage;
											sh.damage_code = sapHeader.damage_code;
											sh.damage_free_text = sapHeader.damage_free_text;
											sh.code_grp_cause = sapHeader.code_grp_cause;
											sh.cause_code = sapHeader.cause_code;
											sh.cause_grp_free_text = sapHeader.cause_grp_free_text;
											sh.user_status = sapHeader.user_status;
											sh.notification_user_status = sapHeader.notification_user_status;
											sh.abc_indicator = sapHeader.abc_indicator;
											sh.accessibility = sapHeader.accessibility;
											sh.system_status = sapHeader.system_status;
											sh.notification_system_status = sapHeader.notification_system_status;
											sh.err_msg = sapHeader.err_msg;
											_context.SapHeader.Update(sh);
										}
										else
										{
											_context.SapHeader.Add(sapHeader);
										}

										importedData.Add(sapHeader);
									}

									await _context.SaveChangesAsync();
									transaction.Commit();

									if (hasError)
									{
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Partial", "File imported with errors: " + Path.GetFileName(filePath));
										errorLogs.Add("File imported with errors: " + Path.GetFileName(filePath) + " Errors: " + fileErrorMsg);
									}
									else {
										// Create log record
										await SapHeaderRecordTXN("SAP-AIMS", "Success", "File imported successfully: " + Path.GetFileName(filePath));
									}
								}
							}
						}
						catch (Exception ex)
						{
							await transaction.RollbackAsync();
							// Create log record
							await SapHeaderRecordTXN("SAP-AIMS", "Fail", "System Error: " + ex.Message);
							return StatusCode(500, $"System Error: {ex.Message}");
						}
					}

					string processedFileName = Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
					string processedFilePath = Path.Combine(processedFolder, processedFileName);
					System.IO.File.Move(filePath, processedFilePath);
				}

				if (errorLogs.Count > 0)
				{
					var paramModel = new List<MailParamModel>
					{
						new MailParamModel { Param = "ErrorDescription", Value = string.Join("<br>", errorLogs) },
						new MailParamModel { Param = "DateTime", Value = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") },
						new MailParamModel { Param = "UserActionRequired", Value = "Please check and correct the errors." }
					};

					var emailModel = new EmailModel
					{
						To = new List<string> { "weerawat.suwattanapiset@dexon-technology.com", "aritouch.sumpaothong@dexon-technology.com"},
						//To = new List<string> { "weerawat.suwattanapiset@dexon-technology.com", "aritouch.sumpaothong@dexon-technology.com", "piyanant.sri@hotmail.com" },
						Subject = "Errors encountered during SAP Header import",
					};

					await _emailService.SendEmailAsync(emailModel, paramModel, "SapHeaderError",errorLogsList);
				}

				return Ok(new { Message = "SAP Headers imported successfully", ImportedData = importedData });
			}

			return StatusCode(500, "Max retry attempts exceeded. Import failed.");
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutSapHeader(int id, SapHeader data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SapHeaderExists(id))
				{
					return NotFound($"SapHeader with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<SapHeader>> PostSapHeader(SapHeader data)
		{
			_context.SapHeader.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetSapHeaderById", new { id = data.id }, data);
		}

		[HttpDelete]
		[Route("delete-sap-header")]
		public async Task<IActionResult> DeleteSapHeader(int id)
		{
			var data = await _context.SapHeader.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			data.is_active = false;
			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SapHeaderExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		private bool SapHeaderExists(int id)
		{
			return _context.SapHeader.Any(e => e.id == id);
		}

		public class CustomDateConverter : ITypeConverter
		{
			public object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					return null;
				}
				if (DateTime.TryParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
				{
					return date;
				}
				return null;
			}

			public string? ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
			{
				if (value is DateTime date)
				{
					return date.ToString("yyyyMMdd");
				}
				return value?.ToString();
			}
		}

		private async Task SapHeaderRecordTXN(string type, string status, string description)
		{
			var errorRecord = new SapHeaderTXN
			{
				txn_type = type,
				txn_status = status,
				txn_desc = description,
				txn_datetime = DateTime.Now
			};

			_context.SapHeaderTXN.Add(errorRecord);
			await _context.SaveChangesAsync();
		}

	}
}