using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using Newtonsoft.Json;

namespace CPOC_AIMS_II_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SapHeaderController : ControllerBase
    {
        private readonly MainDbContext _context;
        public SapHeaderController(MainDbContext context)
        {
            _context = context;
        }
        string sqlDataSource = Startup.ConnectionString;

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
            if (data.Count == 0) return NotFound();

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
                return StatusCode(500, $"Data could not be written to the CSV file : " + ex.Message);
            }

            return Ok($"Export File : {filePath}");
            //return Ok($"Export File : {fileName}");
        }

        [HttpPut]
        [Route("import-process-sap-headers")]
        public async Task<IActionResult> ImportProcessSapHeaders()
        {
            var importedData = new List<SapHeader?>();
            var errorLogs = new List<string>();
            try
            {
                string? folderPath = Startup.SapAimsPath;
                if (folderPath == "")
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
                    return NotFound("No CSV files found in the directory.");

                foreach (string filePath in csvFiles)
                {
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

                                if (csv.HeaderRecord.Length != 39)
                                {
                                    string errorMessage = $"CSV file {filePath} has missing fields.";
                                    errorLogs.Add(errorMessage);
                                    continue;
                                }

                                while (csv.Read())
                                {
                                    var sapHeader = new SapHeader
                                    {
                                        id_reference = csv.GetField(0),
                                        notification = csv.GetField(1),
                                        notification_date = csv.GetField<DateTime?>(2),
                                        notification_type = csv.GetField(3),
                                        wo_order_no = csv.GetField(4),
                                        description = csv.GetField(5),
                                        functional_location = csv.GetField(6),
                                        equipment_no = csv.GetField(7),
                                        technical_identification = csv.GetField(8),
                                        planner_grp = csv.GetField(9),
                                        planner_grp_planning_plant = csv.GetField(10),
                                        main_workctr = csv.GetField(11),
                                        reported_by = csv.GetField(12),
                                        basic_start_date = csv.GetField<DateTime?>(13),
                                        basic_finish_date = csv.GetField<DateTime?>(14),
                                        required_start_date = csv.GetField(15),
                                        required_start_time = csv.GetField(16),
                                        required_end_date = csv.GetField(17),
                                        required_end_time = csv.GetField(18),
                                        total_man_hours = csv.GetField<decimal?>(19),
                                        unit_for_work = csv.GetField(20),
                                        manpower = csv.GetField<int?>(21),
                                        duration = csv.GetField<decimal?>(22),
                                        normal_duration_unit = csv.GetField(23),
                                        actual_work = csv.GetField<decimal?>(24),
                                        priority = csv.GetField(25),
                                        code_grp_object_part = csv.GetField(26),
                                        object_part_code = csv.GetField(27),
                                        code_grp_damage = csv.GetField(28),
                                        damage_code = csv.GetField(29),
                                        damage_free_text = csv.GetField(30),
                                        code_grp_cause = csv.GetField(31),
                                        cause_code = csv.GetField(32),
                                        cause_grp_free_text = csv.GetField(33),
                                        user_status = csv.GetField(34),
                                        abc_indicator = csv.GetField(35),
                                        accessibility = csv.GetField(36),
                                        system_status = csv.GetField(37),
                                        changed_on = csv.GetField<DateTime?>(38),
                                    };

                                    var sh = await _context.SapHeader.FirstOrDefaultAsync(s => s.id_reference == sapHeader.id_reference);
                                    if (sh != null)
                                    {
                                        sh.notification = sapHeader.notification;
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
                                        sh.abc_indicator = sapHeader.abc_indicator;
                                        sh.accessibility = sapHeader.accessibility;
                                        sh.system_status = sapHeader.system_status;
                                        sh.changed_on = sapHeader.changed_on;
                                    }
                                    importedData.Add(sh);
                                }
                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            string fileName = Path.GetFileName(filePath);
                            string destFilePath = Path.Combine(processedFolder, fileName);
                            System.IO.File.Move(filePath, destFilePath);

                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            string errorMessage = $"Error importing CSV file: {filePath}. Exception: {ex.Message}";
                            errorLogs.Add(errorMessage);
                        }
                    }
                }
                string resultMessage = errorLogs.Count == 0 ? "CSV files update success." : "CSV files some errors.";
                return Ok(new
                {
                    message = resultMessage,
                    data = importedData,
                    errors = errorLogs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred: {ex.Message}");
            }
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

        // public static int? TryParseInt(string value)
        // {
        //     if (int.TryParse(value, out int result))
        //     {
        //         return result;
        //     }
        //     return null;
        // }

        // public static decimal? TryParseDecimal(string value)
        // {
        //     if (decimal.TryParse(value, out decimal result))
        //     {
        //         return result;
        //     }
        //     return null;
        // }

    }
}