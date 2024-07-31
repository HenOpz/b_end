using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExInspectionRegisterInfoController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ExInspectionRegisterInfoController(MainDbContext context)
		{
			_context = context;
		}
		string sqlDataSource = Startup.ConnectionString;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ExInspectionRegisterInfo>>> GetExInspectionRegisterInfo()
		{
			var data =  await GetExInspectionRegisterInfoQuery().Where(a => a.is_active == true).ToListAsync();
			
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ExInspectionRegisterInfo>> GetExInspectionRegisterInfo(int id)
		{
			var data = await GetExInspectionRegisterInfoQuery().Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutExInspectionRegisterInfo(int id, ExInspectionRegisterInfo data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			data.updated_date = DateTime.Now;

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ExInspectionRegisterInfoExists(id))
				{
					return NotFound($"ExInspectionRegisterInfo with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<ExInspectionRegisterInfo>> PostExInspectionRegisterInfo(ExInspectionRegisterInfo data)
		{
			data.created_date = DateTime.Now;
			_context.ExInspectionRegisterInfo.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetExInspectionRegisterInfo", new { id = data.id }, data);
		}

		[HttpPost]
		[Route("attach-pic")]
		public async Task<ActionResult<dynamic>> AttachPic([FromForm] AttachPic data, int id_info)
		{
			try
			{
				var info = await _context.ExInspectionRegisterInfo.FirstOrDefaultAsync(a => a.id == id_info);
				if (info == null) { return NotFound($"ExInspectionRegisterInfo with ID {id_info} not found."); };

				string path = "wwwroot/attach/ex_insp_info/";
				if (data.file != null)
				{
					if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
					path += data.file.FileName;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						await data.file.CopyToAsync(stream);
					}

					info.overview_img_path = path;
					await _context.SaveChangesAsync();
				}

				return info;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete]
		[Route("delete-pic")]
		public async Task<ActionResult<dynamic>> DeletePic(int id_info)
		{
			try
			{
				var info = await _context.ExInspectionRegisterInfo.FirstOrDefaultAsync(a => a.id == id_info);
				if (info == null) { return NotFound($"ExInspectionRegisterInfo with ID {id_info} not found."); };

				if (info.overview_img_path == null) { return NotFound($"ExInspectionRegisterInfo with ID {id_info} do not have picture."); }

				info.overview_img_path = null;
				await _context.SaveChangesAsync();

				return info;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete]
		[Route("delete-ex-inspection-register-info")]
		public async Task<IActionResult> DeleteExInspectionRegisterInfo(int id)
		{
			var data = await _context.ExInspectionRegisterInfo.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			data.is_active = false;
			data.updated_date = DateTime.Now;
			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ExInspectionRegisterInfoExists(id))
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

		private bool ExInspectionRegisterInfoExists(int id)
		{
			return _context.ExInspectionRegisterInfo.Any(e => e.id == id);
		}

		private IQueryable<ExInspectionRegisterInfoView> GetExInspectionRegisterInfoQuery()
		{
			return from inf in _context.ExInspectionRegisterInfo
				   join pf in _context.MdPlatform on inf.id_platfrom equals pf.id into infpf
				   from infpfResult in infpf.DefaultIfEmpty()

				   join a in _context.MdExInspectionAreaStandard on inf.id_area_standard equals a.id into infa
				   from infaResult in infa.DefaultIfEmpty()

				   join a_c in _context.MdExInspectionAreaClass on inf.id_area_class equals a_c.id into infa_c
				   from infa_cResult in infa_c.DefaultIfEmpty()

				   join a_tc in _context.MdExInspectionAreaTempClass on inf.id_area_temp_class equals a_tc.id into infa_tc
				   from infa_tcResult in infa_tc.DefaultIfEmpty()

				   join a_gg in _context.MdExInspectionAreaGasGroup on inf.id_area_gas_group equals a_gg.id into infa_gg
				   from infa_ggResult in infa_gg.DefaultIfEmpty()

				   join e in _context.MdExInspectionEquipStandard on inf.id_equip_standard equals e.id into infe
				   from infeResult in infe.DefaultIfEmpty()

				   join e_plc in _context.MdExInspectionEquipProtectionLevelCategory on inf.id_equip_protection_level_category equals e_plc.id into infe_plc
				   from infe_plcResult in infe_plc.DefaultIfEmpty()

				   join e_pt in _context.MdExInspectionEquipProtectionType on inf.id_equip_protection_type equals e_pt.id into infe_pt
				   from infe_ptResult in infe_pt.DefaultIfEmpty()

				   join e_t in _context.MdExInspectionEquipType on inf.id_equip_type equals e_t.id into infe_t
				   from infe_tResult in infe_t.DefaultIfEmpty()

				   join e_c in _context.MdExInspectionEquipClass on inf.id_equip_class equals e_c.id into infe_c
				   from infe_cResult in infe_c.DefaultIfEmpty()

				   join e_tc in _context.MdExInspectionEquipTempClass on inf.id_area_temp_class equals e_tc.id into infe_tc
				   from infe_tcResult in infe_tc.DefaultIfEmpty()

				   join e_gg in _context.MdExInspectionEquipGasGroup on inf.id_equip_gas_group equals e_gg.id into infe_gg
				   from infe_ggResult in infe_gg.DefaultIfEmpty()

				   join e_ir in _context.MdExInspectionEquipIpRating on inf.id_equip_ip_rating equals e_ir.id into infe_ir
				   from infe_irResult in infe_ir.DefaultIfEmpty()

				   join e_et in _context.MdExInspectionEquipEnclosureType on inf.id_equip_enclosure_type equals e_et.id into infe_et
				   from infe_etResult in infe_et.DefaultIfEmpty()

				   join cb in _context.User on inf.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on inf.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   where inf.is_active == true

				   select new ExInspectionRegisterInfoView
				   {
					   id = inf.id,
					   id_platfrom = inf.id_platfrom,
					   platform_name = infpfResult.code_name,
					   platform_full_name = infpfResult.full_name,
					   phase = infpfResult.phase,
					   planning_plant = infpfResult.planning_plant,
					   tag_no = inf.tag_no,
					   equip_desc = inf.equip_desc,
					   location = inf.location,
					   system = inf.system,
					   system_desc = inf.system_desc,
					   id_area_standard = inf.id_area_standard,
					   area_standard = infaResult.code,
					   id_area_class = inf.id_area_class,
					   area_class = infa_cResult.code,
					   id_area_temp_class = inf.id_area_temp_class,
					   area_temp_class = infa_tcResult.code,
					   id_area_gas_group = inf.id_area_gas_group,
					   area_gas_group = infa_ggResult.code,
					   id_equip_standard = inf.id_equip_standard,
					   equip_standard = infeResult.code,
					   equip_protection_tag = inf.equip_protection_tag,
					   id_equip_protection_level_category = inf.id_equip_protection_level_category,
					   equip_protection_level_category = infe_plcResult.code,
					   id_equip_protection_type = inf.id_equip_protection_type,
					   equip_protection_type = infe_ptResult.code,
					   id_equip_type = inf.id_equip_type,
					   equip_type = infe_tResult.code,
					   id_equip_class = inf.id_equip_class,
					   equip_class = infe_cResult.code,
					   id_equip_temp_class = inf.id_equip_temp_class,
					   equip_temp_class = infe_tcResult.code,
					   id_equip_gas_group = inf.id_equip_gas_group,
					   equip_gas_group = infe_ggResult.code,
					   id_equip_ip_rating = inf.id_equip_ip_rating,
					   equip_ip_rating = infe_irResult.code,
					   id_equip_enclosure_type = inf.id_equip_enclosure_type,
					   equip_enclosure_type = infe_etResult.code,
					   equip_manufacturer = inf.equip_manufacturer,
					   model = inf.model,
					   serial_no = inf.serial_no,
					   certifying_body = inf.certifying_body,
					   ex_cert_no = inf.ex_cert_no,
					   drawing_ref = inf.drawing_ref,
					   installation_date = inf.installation_date,
					   operating_during_esd = inf.operating_during_esd,
					   category_inst_elect = inf.category_inst_elect,
					   id_water_corrosion_chemicals_status = inf.id_water_corrosion_chemicals_status,
					   id_dust_sand_status = inf.id_dust_sand_status,
					   id_uv_radiation_status = inf.id_uv_radiation_status,
					   id_ambient_temp_status = inf.id_ambient_temp_status,
					   id_temp_cycling_status = inf.id_temp_cycling_status,
					   equip_remark = inf.equip_remark,
					   overview_img_path = inf.overview_img_path,
					   is_active = inf.is_active,
					   created_by = inf.created_by,
					   created_by_name = icbResult.name,
					   created_date = inf.created_date,
					   updated_by = inf.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = inf.updated_date,
				   };
		}
	}
}