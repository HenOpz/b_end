using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML;
using ClosedXML.Excel;
using System.Globalization;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMChemInjectionRecordController : ControllerBase
	{
		private readonly MainDbContext _context;

		public CMChemInjectionRecordController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/CMChemInjectionRecord
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMChemInjectionRecord>>> GetCMChemInjectionRecord()
		{
			return await _context.CMChemInjectionRecord.ToListAsync();
		}

		// GET: api/CMChemInjectionRecord/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CMChemInjectionRecord>> GetCMChemInjectionRecord(int id)
		{
			var data = await _context.CMChemInjectionRecord.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		// GET: api/CMChemInjectionRecord/ByTag/5
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMChemInjectionRecord>>> GetCMChemInjectionRecordByTag(int id_tag)
		{
			return await _context.CMChemInjectionRecord.Where(b => b.id_tag == id_tag).ToListAsync();
		}

		// GET: api/CMChemInjectionRecord/ByTagMonthYear/5/4/2024
		[HttpGet("ByTagMonthYear/{id_tag}/{month}/{year}")]
		public async Task<ActionResult<IEnumerable<CMChemInjectionRecord>>> GetCMChemInjectionRecordByTagMonthYear(int id_tag, int month, int year)
		{
			return await _context.CMChemInjectionRecord
				.Where(b => b.id_tag == id_tag && b.record_date.HasValue && b.record_date.Value.Month == month && b.record_date.Value.Year == year)
				.ToListAsync();
		}

		// GET: api/CMChemInjectionRecord/ByDate/{date}
		[HttpGet("ByDate/{date}")]
		public async Task<ActionResult<IEnumerable<CMChemInjectionRecordView>>> GetCMChemInjectionRecordByDate(DateTime date)
		{
			var data = await (from record in _context.CMChemInjectionRecord
							join inf in _context.CMInfo on record.id_tag equals inf.id into recinf
							from recinfResult in recinf.DefaultIfEmpty()
							
							join pf in _context.MdPlatform on recinfResult.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							
							join stt in _context.MdCMChemInjectionStatus on record.id_status equals stt.id into recstt
							from recsttResult in recstt.DefaultIfEmpty()
							
							where record.record_date == date && recinfResult.is_active == true
							
							select new CMChemInjectionRecordView
							{
								id = record.id,
								id_tag = record.id_tag,
								tag_no = recinfResult.tag_no,
								id_platform = recinfResult.id_platform,
								platform = infpfResult.code_name,
								record_date = record.record_date,
								gas_flow_rate_mmscfd = record.gas_flow_rate_mmscfd,
								req_ci_injection_rate_liters_mmscfd = record.req_ci_injection_rate_liters_mmscfd,
								req_ci_rate_liters_day = record.req_ci_rate_liters_day,
								yesterday_ci_tank_level_percent = record.yesterday_ci_tank_level_percent,
								today_ci_tank_level_percent = record.today_ci_tank_level_percent,
								ci_tank_calc = record.ci_tank_calc,
								actual_ci_injection_liters_day = record.actual_ci_injection_liters_day,
								remark = record.remark,
								id_status = record.id_status,
								severity_level = recsttResult.severity_level,
								color_name = recsttResult.color_name,
								color_code = recsttResult.color_code,
							}
							).ToListAsync();
			return Ok(data);
		}

		// POST: api/CMChemInjectionRecord
		[HttpPost]
		public async Task<ActionResult<CMChemInjectionRecord>> PostCMChemInjectionRecord(CMChemInjectionRecord cMMicroBacteriaATP)
		{
			_context.CMChemInjectionRecord.Add(cMMicroBacteriaATP);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMChemInjectionRecord", new { id = cMMicroBacteriaATP.id }, cMMicroBacteriaATP);
		}

		[HttpPost("import")]
		public async Task<IActionResult> Import(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			string fileName = file.FileName;
			DateTime fileDate;

			try
			{
				fileDate = ExtractDateFromFileName(fileName);
			}
			catch (FormatException ex)
			{
				return BadRequest(ex.Message);
			}

			using var stream = file.OpenReadStream();
			List<CMChemInjectionRecord> data = ImportExcelData(stream, fileDate);

			_context.CMChemInjectionRecord.AddRange(data);
			await _context.SaveChangesAsync();

			return Ok("Data imported successfully.");
		}

		[HttpPost("multi-import")]
		public async Task<IActionResult> Import([FromForm] List<IFormFile> files)
		{
			if (files == null || files.Count == 0)
				return BadRequest("No files uploaded.");

			foreach (var file in files)
			{
				if (file == null || file.Length == 0)
					continue; // Skip empty files

				string fileName = file.FileName;
				DateTime fileDate;

				try
				{
					fileDate = ExtractDateFromFileName(fileName);
				}
				catch (FormatException ex)
				{
					return BadRequest($"File '{fileName}' error: {ex.Message}");
				}

				using var stream = file.OpenReadStream();
				List<CMChemInjectionRecord> data = ImportExcelData(stream, fileDate);

				_context.CMChemInjectionRecord.AddRange(data);
			}

			await _context.SaveChangesAsync();

			return Ok("Data imported successfully.");
		}

		// PUT: api/CMChemInjectionRecord/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMChemInjectionRecord(int id, CMChemInjectionRecord cMMicroBacteriaATP)
		{
			if (id != cMMicroBacteriaATP.id)
			{
				return BadRequest();
			}

			cMMicroBacteriaATP.updated_date = DateTime.Now;
			_context.Entry(cMMicroBacteriaATP).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMChemInjectionRecordExists(id))
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

		// DELETE: api/CMChemInjectionRecord/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMChemInjectionRecord(int id)
		{
			var data = await _context.CMChemInjectionRecord.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}

			_context.CMChemInjectionRecord.Remove(data);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CMChemInjectionRecordExists(int id)
		{
			return _context.CMChemInjectionRecord.Any(e => e.id == id);
		}

		[NonAction]
		private List<CMChemInjectionRecord> ImportExcelData(Stream excelStream, DateTime fileDate)
		{
			using var workbook = new XLWorkbook(excelStream);
			var worksheet = workbook.Worksheet("WELL  ");

			if (worksheet == null)
			{
				throw new Exception("Worksheet 'WELL' not found in the Excel file.");
			}

			var records = new List<CMChemInjectionRecord>();
			var joinInf = from inf in _context.CMInfo
						  join pf in _context.MdPlatform on inf.id_platform equals pf.id
						  where inf.id_system == 4
						  where inf.is_active == true
						  select new
						  {
							  inf.id,
							  inf.id_platform,
							  pf.code_name,
						  };

			for (int row = 322; row <= 338; row++)
			{
				var actual_ci_injection_liters_day = GetDecimalValue(worksheet.Cell(row, 13));
				var req_ci_rate_liters_day = GetDecimalValue(worksheet.Cell(row, 7));

				var record = new CMChemInjectionRecord
				{
					id_tag = joinInf.Where(a => a.code_name.Trim() == GetStringValue(worksheet.Cell(row, 2)).Trim())
									.Select(a => a.id)
									.FirstOrDefault(),
					record_date = fileDate,
					gas_flow_rate_mmscfd = GetDecimalValue(worksheet.Cell(row, 3)),
					req_ci_injection_rate_liters_mmscfd = GetDecimalValue(worksheet.Cell(row, 5)),
					req_ci_rate_liters_day = req_ci_rate_liters_day,
					yesterday_ci_tank_level_percent = GetDecimalValue(worksheet.Cell(row, 9)),
					today_ci_tank_level_percent = GetDecimalValue(worksheet.Cell(row, 11)),
					ci_tank_calc = GetDecimalValue(worksheet.Cell(row, 32)),
					actual_ci_injection_liters_day = actual_ci_injection_liters_day,
					remark = GetStringValue(worksheet.Cell(row, 18)),
					id_status = actual_ci_injection_liters_day >= req_ci_rate_liters_day ? 2 : 1,
					created_by = null,
					created_date = DateTime.Now,
					updated_by = null,
					updated_date = DateTime.Now,
				};

				records.Add(record);
			}

			return records;
		}

		[NonAction]
		private int GetIntValue(IXLCell cell)
		{
			if (cell.IsEmpty() || !int.TryParse(cell.GetValue<string>(), out int value))
			{
				return 0; // Consider zero as invalid for id_tag, you can adjust this as needed
			}
			return value;
		}

		[NonAction]
		private decimal? GetDecimalValue(IXLCell cell)
		{
			if (cell.IsEmpty() || !decimal.TryParse(cell.GetValue<string>(), out decimal value))
			{
				return null;
			}
			return value;
		}

		[NonAction]
		private string GetStringValue(IXLCell cell)
		{
			return cell?.GetString()?.Trim() ?? string.Empty;
		}

		[NonAction]
		public DateTime ExtractDateFromFileName(string fileName)
		{
			string datePart = fileName.Substring(0, 8);

			if (DateTime.TryParseExact(datePart, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
			{
				return parsedDate;
			}
			else
			{
				throw new FormatException("Invalid date format in file name.");
			}
		}
	}
}