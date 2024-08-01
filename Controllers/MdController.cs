using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MdController : ControllerBase
	{
		private readonly MainDbContext _context;

		public MdController(MainDbContext context)
		{
			_context = context;
		}
		
		#region MdAppModule
		[HttpGet]
		[Route("get-md-app-module-list")]
		public async Task<ActionResult<IEnumerable<MdAppModule>>> GetMdAppModuleList()
		{
			return await _context.MdAppModule.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-app-module-by-id")]
		public async Task<ActionResult<MdAppModule>> GetMdAppModule(int id)
		{
			var data = await _context.MdAppModule.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-app-module")]
		public async Task<ActionResult<MdAppModule>> AddMdAppModule(MdAppModule form)
		{
			_context.MdAppModule.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdAppModule", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-app-module")]
		public async Task<IActionResult> EditMdAppModule(int id, MdAppModule form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdAppModuleExists(id))
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

		[HttpDelete]
		[Route("delete-md-app-module")]
		public async Task<IActionResult> DeleteMdAppModule(int id)
		{
			var data = await _context.MdAppModule.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdAppModule.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdAppModuleExists(int id)
		{
			return _context.MdAppModule.Any(e => e.id == id);
		}
		#endregion

		#region MdMenu
		[HttpGet]
		[Route("get-md-menu-list")]
		public async Task<ActionResult<IEnumerable<MdMenu>>> GetMdMenuList()
		{
			return await _context.MdMenu.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-menu-by-id")]
		public async Task<ActionResult<MdMenu>> GetMdMenu(int id)
		{
			var data = await _context.MdMenu.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-menu-by-id-app-module")]
		public async Task<ActionResult<List<MdMenu>>> GetMdMenuByAppModule(int id_app_module)
		{
			var data = await _context.MdMenu.Where(a => a.id_app_module == id_app_module).ToListAsync();

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-menu")]
		public async Task<ActionResult<MdMenu>> AddMdMenu(MdMenu form)
		{
			_context.MdMenu.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdMenu", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-menu")]
		public async Task<IActionResult> EditMdMenu(int id, MdMenu form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdMenuExists(id))
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

		[HttpDelete]
		[Route("delete-md-menu")]
		public async Task<IActionResult> DeleteMdMenu(int id)
		{
			var data = await _context.MdMenu.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdMenu.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdMenuExists(int id)
		{
			return _context.MdMenu.Any(e => e.id == id);
		}
		#endregion
		
		#region MdUserRole
		[HttpGet]
		[Route("get-md-user-role-list")]
		public async Task<ActionResult<IEnumerable<MdUserRole>>> GetMdUserRoleList()
		{
			return await _context.MdUserRole.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-user-role-by-id")]
		public async Task<ActionResult<MdUserRole>> GetMdUserRole(int id)
		{
			var data = await _context.MdUserRole.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-user-role")]
		public async Task<ActionResult<MdUserRole>> AddMdUserRole(MdUserRole form)
		{
			_context.MdUserRole.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdUserRole", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-user-role")]
		public async Task<IActionResult> EditMdUserRole(int id, MdUserRole form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdUserRoleExists(id))
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

		[HttpDelete]
		[Route("delete-md-user-role")]
		public async Task<IActionResult> DeleteMdUserRole(int id)
		{
			var data = await _context.MdUserRole.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdUserRole.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdUserRoleExists(int id)
		{
			return _context.MdUserRole.Any(e => e.id == id);
		}
		#endregion

		#region MdPrefix
		[HttpGet]
		[Route("get-md-prefix-list")]
		public async Task<ActionResult<IEnumerable<MdPrefix>>> GetMdPrefixList()
		{
			return await _context.MdPrefix.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-prefix-by-id")]
		public async Task<ActionResult<MdPrefix>> GetMdPrefix(int id)
		{
			var data = await _context.MdPrefix.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-prefix")]
		public async Task<ActionResult<MdPrefix>> AddMdPrefix(MdPrefix form)
		{
			_context.MdPrefix.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdPrefix", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-prefix")]
		public async Task<IActionResult> EditMdPrefix(int id, MdPrefix form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdPrefixExists(id))
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

		[HttpDelete]
		[Route("delete-md-prefix")]
		public async Task<IActionResult> DeleteMdPrefix(int id)
		{
			var data = await _context.MdPrefix.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdPrefix.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdPrefixExists(int id)
		{
			return _context.MdPrefix.Any(e => e.id == id);
		}
		#endregion
	
		#region MdAssetType
		[HttpGet]
		[Route("get-md-asset-type-list")]
		public async Task<ActionResult<IEnumerable<MdAssetType>>> GetMdAssetTypeList()
		{
			return await _context.MdAssetType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-asset-type-by-id")]
		public async Task<ActionResult<MdAssetType>> GetMdAssetType(int id)
		{
			var data = await _context.MdAssetType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-asset-type")]
		public async Task<ActionResult<MdAssetType>> AddMdAssetType(MdAssetType form)
		{
			_context.MdAssetType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdAssetType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-asset-type")]
		public async Task<IActionResult> EditMdAssetType(int id, MdAssetType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdAssetTypeExists(id))
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

		[HttpDelete]
		[Route("delete-md-asset-type")]
		public async Task<IActionResult> DeleteMdAssetType(int id)
		{
			var data = await _context.MdAssetType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdAssetType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdAssetTypeExists(int id)
		{
			return _context.MdAssetType.Any(e => e.id == id);
		}
		#endregion
	
		#region MdInspectionCampaignStatus
		[HttpGet]
		[Route("get-md-insp-campaign-status-list")]
		public async Task<ActionResult<IEnumerable<MdInspectionCampaignStatus>>> GetMdInspectionCampaignStatusList()
		{
			return await _context.MdInspectionCampaignStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-insp-campaign-status-by-id")]
		public async Task<ActionResult<MdInspectionCampaignStatus>> GetMdInspectionCampaignStatus(int id)
		{
			var data = await _context.MdInspectionCampaignStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-insp-campaign-status")]
		public async Task<ActionResult<MdInspectionCampaignStatus>> AddMdInspectionCampaignStatus(MdInspectionCampaignStatus form)
		{
			_context.MdInspectionCampaignStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdInspectionCampaignStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-insp-campaign-status")]
		public async Task<IActionResult> EditMdInspectionCampaignStatus(int id, MdInspectionCampaignStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdInspectionCampaignStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-insp-campaign-status")]
		public async Task<IActionResult> DeleteMdInspectionCampaignStatus(int id)
		{
			var data = await _context.MdInspectionCampaignStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdInspectionCampaignStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdInspectionCampaignStatusExists(int id)
		{
			return _context.MdInspectionCampaignStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdInspectionTaskStatus
		[HttpGet]
		[Route("get-md-insp-task-status-list")]
		public async Task<ActionResult<IEnumerable<MdInspectionTaskStatus>>> GetMdInspectionTaskStatusList()
		{
			return await _context.MdInspectionTaskStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-insp-task-status-by-id")]
		public async Task<ActionResult<MdInspectionTaskStatus>> GetMdInspectionTaskStatus(int id)
		{
			var data = await _context.MdInspectionTaskStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-insp-task-status")]
		public async Task<ActionResult<MdInspectionTaskStatus>> AddMdInspectionTaskStatus(MdInspectionTaskStatus form)
		{
			_context.MdInspectionTaskStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdInspectionTaskStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-insp-task-status")]
		public async Task<IActionResult> EditMdInspectionTaskStatus(int id, MdInspectionTaskStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdInspectionTaskStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-insp-task-status")]
		public async Task<IActionResult> DeleteMdInspectionTaskStatus(int id)
		{
			var data = await _context.MdInspectionTaskStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdInspectionTaskStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdInspectionTaskStatusExists(int id)
		{
			return _context.MdInspectionTaskStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdInspectionType
		[HttpGet]
		[Route("get-md-insp-type-list")]
		public async Task<ActionResult<IEnumerable<MdInspectionType>>> GetMdInspectionTypeList()
		{
			return await _context.MdInspectionType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-insp-type-by-id")]
		public async Task<ActionResult<MdInspectionType>> GetMdInspectionType(int id)
		{
			var data = await _context.MdInspectionType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-insp-type")]
		public async Task<ActionResult<MdInspectionType>> AddMdInspectionType(MdInspectionType form)
		{
			_context.MdInspectionType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdInspectionType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-insp-type")]
		public async Task<IActionResult> EditMdInspectionType(int id, MdInspectionType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdInspectionTypeExists(id))
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

		[HttpDelete]
		[Route("delete-md-insp-type")]
		public async Task<IActionResult> DeleteMdInspectionType(int id)
		{
			var data = await _context.MdInspectionType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdInspectionType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdInspectionTypeExists(int id)
		{
			return _context.MdInspectionType.Any(e => e.id == id);
		}
		#endregion
	
		#region MdIntegrityStatus
		[HttpGet]
		[Route("get-md-integrity-status-list")]
		public async Task<ActionResult<IEnumerable<MdIntegrityStatus>>> GetMdIntegrityStatusList()
		{
			return await _context.MdIntegrityStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-integrity-status-by-id")]
		public async Task<ActionResult<MdIntegrityStatus>> GetMdIntegrityStatus(int id)
		{
			var data = await _context.MdIntegrityStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-integrity-status")]
		public async Task<ActionResult<MdIntegrityStatus>> AddMdIntegrityStatus(MdIntegrityStatus form)
		{
			_context.MdIntegrityStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdIntegrityStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-integrity-status")]
		public async Task<IActionResult> EditMdIntegrityStatus(int id, MdIntegrityStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdIntegrityStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-integrity-status")]
		public async Task<IActionResult> DeleteMdIntegrityStatus(int id)
		{
			var data = await _context.MdIntegrityStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdIntegrityStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdIntegrityStatusExists(int id)
		{
			return _context.MdIntegrityStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdMocNatureOfChange
		[HttpGet]
		[Route("get-md-moc-noc-list")]
		public async Task<ActionResult<IEnumerable<MdMocNatureOfChange>>> GetMdMocNatureOfChangeList()
		{
			return await _context.MdMocNatureOfChange.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-moc-noc-by-id")]
		public async Task<ActionResult<MdMocNatureOfChange>> GetMdMocNatureOfChange(int id)
		{
			var data = await _context.MdMocNatureOfChange.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-moc-noc")]
		public async Task<ActionResult<MdMocNatureOfChange>> AddMdMocNatureOfChange(MdMocNatureOfChange form)
		{
			_context.MdMocNatureOfChange.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdMocNatureOfChange", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-moc-noc")]
		public async Task<IActionResult> EditMdMocNatureOfChange(int id, MdMocNatureOfChange form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdMocNatureOfChangeExists(id))
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

		[HttpDelete]
		[Route("delete-md-moc-noc")]
		public async Task<IActionResult> DeleteMdMocNatureOfChange(int id)
		{
			var data = await _context.MdMocNatureOfChange.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdMocNatureOfChange.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdMocNatureOfChangeExists(int id)
		{
			return _context.MdMocNatureOfChange.Any(e => e.id == id);
		}
		#endregion
	
		#region MdMocResidualRiskLevel
		[HttpGet]
		[Route("get-md-moc-rrl-list")]
		public async Task<ActionResult<IEnumerable<MdMocResidualRiskLevel>>> GetMdMocResidualRiskLevelList()
		{
			return await _context.MdMocResidualRiskLevel.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-moc-rrl-by-id")]
		public async Task<ActionResult<MdMocResidualRiskLevel>> GetMdMocResidualRiskLevel(int id)
		{
			var data = await _context.MdMocResidualRiskLevel.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-moc-rrl")]
		public async Task<ActionResult<MdMocResidualRiskLevel>> AddMdMocResidualRiskLevel(MdMocResidualRiskLevel form)
		{
			_context.MdMocResidualRiskLevel.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdMocResidualRiskLevel", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-moc-rrl")]
		public async Task<IActionResult> EditMdMocResidualRiskLevel(int id, MdMocResidualRiskLevel form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdMocResidualRiskLevelExists(id))
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

		[HttpDelete]
		[Route("delete-md-moc-rrl")]
		public async Task<IActionResult> DeleteMdMocResidualRiskLevel(int id)
		{
			var data = await _context.MdMocResidualRiskLevel.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdMocResidualRiskLevel.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdMocResidualRiskLevelExists(int id)
		{
			return _context.MdMocResidualRiskLevel.Any(e => e.id == id);
		}
		#endregion
	
		#region MdMocStatus
		[HttpGet]
		[Route("get-md-moc-status-list")]
		public async Task<ActionResult<IEnumerable<MdMocStatus>>> GetMdMocStatusList()
		{
			return await _context.MdMocStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-moc-status-by-id")]
		public async Task<ActionResult<MdMocStatus>> GetMdMocStatus(int id)
		{
			var data = await _context.MdMocStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-moc-status")]
		public async Task<ActionResult<MdMocStatus>> AddMdMocStatus(MdMocStatus form)
		{
			_context.MdMocStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdMocStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-moc-status")]
		public async Task<IActionResult> EditMdMocStatus(int id, MdMocStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdMocStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-moc-status")]
		public async Task<IActionResult> DeleteMdMocStatus(int id)
		{
			var data = await _context.MdMocStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdMocStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdMocStatusExists(int id)
		{
			return _context.MdMocStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdPlatform
		[HttpGet]
		[Route("get-md-platform-list")]
		public async Task<ActionResult<IEnumerable<MdPlatform>>> GetMdPlatformList()
		{
			return await _context.MdPlatform.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-platform-by-id")]
		public async Task<ActionResult<MdPlatform>> GetMdPlatform(int id)
		{
			var data = await _context.MdPlatform.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-platform")]
		public async Task<ActionResult<MdPlatform>> AddMdPlatform(MdPlatform form)
		{
			_context.MdPlatform.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdPlatform", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-platform")]
		public async Task<IActionResult> EditMdPlatform(int id, MdPlatform form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdPlatformExists(id))
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

		[HttpDelete]
		[Route("delete-md-platform")]
		public async Task<IActionResult> DeleteMdPlatform(int id)
		{
			var data = await _context.MdPlatform.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdPlatform.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdPlatformExists(int id)
		{
			return _context.MdPlatform.Any(e => e.id == id);
		}
		#endregion
	
		#region MdRectificationStatus
		[HttpGet]
		[Route("get-md-rectification-status-list")]
		public async Task<ActionResult<IEnumerable<MdRectificationStatus>>> GetMdRectificationStatusList()
		{
			return await _context.MdRectificationStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-rectification-status-by-id")]
		public async Task<ActionResult<MdRectificationStatus>> GetMdRectificationStatus(int id)
		{
			var data = await _context.MdRectificationStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-rectification-status")]
		public async Task<ActionResult<MdRectificationStatus>> AddMdRectificationStatus(MdRectificationStatus form)
		{
			_context.MdRectificationStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdRectificationStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-rectification-status")]
		public async Task<IActionResult> EditMdRectificationStatus(int id, MdRectificationStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdRectificationStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-rectification-status")]
		public async Task<IActionResult> DeleteMdRectificationStatus(int id)
		{
			var data = await _context.MdRectificationStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdRectificationStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdRectificationStatusExists(int id)
		{
			return _context.MdRectificationStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdRiskRanking
		[HttpGet]
		[Route("get-md-risk-ranking-list")]
		public async Task<ActionResult<IEnumerable<MdRiskRanking>>> GetMdRiskRankingList()
		{
			return await _context.MdRiskRanking.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-risk-ranking-by-id")]
		public async Task<ActionResult<MdRiskRanking>> GetMdRiskRanking(int id)
		{
			var data = await _context.MdRiskRanking.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-risk-ranking")]
		public async Task<ActionResult<MdRiskRanking>> AddMdRiskRanking(MdRiskRanking form)
		{
			_context.MdRiskRanking.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdRiskRanking", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-risk-ranking")]
		public async Task<IActionResult> EditMdRiskRanking(int id, MdRiskRanking form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdRiskRankingExists(id))
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

		[HttpDelete]
		[Route("delete-md-risk-ranking")]
		public async Task<IActionResult> DeleteMdRiskRanking(int id)
		{
			var data = await _context.MdRiskRanking.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdRiskRanking.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdRiskRankingExists(int id)
		{
			return _context.MdRiskRanking.Any(e => e.id == id);
		}
		#endregion

		#region MdFailureImpact
		[HttpGet]
		[Route("get-md-failure-impact-list")]
		public async Task<ActionResult<IEnumerable<MdFailureImpact>>> GetMdFailureImpactList()
		{
			return await _context.MdFailureImpact.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-impact-by-id")]
		public async Task<ActionResult<MdFailureImpact>> GetMdFailureImpact(int id)
		{
			var data = await _context.MdFailureImpact.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-impact")]
		public async Task<ActionResult<MdFailureImpact>> AddMdFailureImpact(MdFailureImpact form)
		{
			_context.MdFailureImpact.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureImpact", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-impact")]
		public async Task<IActionResult> EditMdFailureImpact(int id, MdFailureImpact form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureImpactExists(id))
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

		[HttpDelete]
		[Route("delete-md-failure-impact")]
		public async Task<IActionResult> DeleteMdFailureImpact(int id)
		{
			var data = await _context.MdFailureImpact.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureImpact.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureImpactExists(int id)
		{
			return _context.MdFailureImpact.Any(e => e.id == id);
		}
		#endregion
	
		#region MdFailureActionStatus
		[HttpGet]
		[Route("get-md-failure-action-status-list")]
		public async Task<ActionResult<IEnumerable<MdFailureActionStatus>>> GetMdFailureActionStatusList()
		{
			return await _context.MdFailureActionStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-action-status-by-id")]
		public async Task<ActionResult<MdFailureActionStatus>> GetMdFailureActionStatus(int id)
		{
			var data = await _context.MdFailureActionStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-action-status")]
		public async Task<ActionResult<MdFailureActionStatus>> AddMdFailureActionStatus(MdFailureActionStatus form)
		{
			_context.MdFailureActionStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureActionStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-action-status")]
		public async Task<IActionResult> EditMdFailureActionStatus(int id, MdFailureActionStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureActionStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-failure-action-status")]
		public async Task<IActionResult> DeleteMdFailureActionStatus(int id)
		{
			var data = await _context.MdFailureActionStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureActionStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureActionStatusExists(int id)
		{
			return _context.MdFailureActionStatus.Any(e => e.id == id);
		}
		#endregion

		#region MdFailureDiscipline
		[HttpGet]
		[Route("get-md-failure-discipline-list")]
		public async Task<ActionResult<IEnumerable<MdFailureDiscipline>>> GetMdFailureDisciplineList()
		{
			return await _context.MdFailureDiscipline.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-discipline-by-id")]
		public async Task<ActionResult<MdFailureDiscipline>> GetMdFailureDiscipline(int id)
		{
			var data = await _context.MdFailureDiscipline.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-discipline")]
		public async Task<ActionResult<MdFailureDiscipline>> AddMdFailureDiscipline(MdFailureDiscipline form)
		{
			_context.MdFailureDiscipline.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureDiscipline", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-discipline")]
		public async Task<IActionResult> EditMdFailureDiscipline(int id, MdFailureDiscipline form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureDisciplineExists(id))
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

		[HttpDelete]
		[Route("delete-md-failure-discipline")]
		public async Task<IActionResult> DeleteMdFailureDiscipline(int id)
		{
			var data = await _context.MdFailureDiscipline.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureDiscipline.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureDisciplineExists(int id)
		{
			return _context.MdFailureDiscipline.Any(e => e.id == id);
		}
		#endregion
	
		#region MdFailureApprovalStatus
		[HttpGet]
		[Route("get-md-failure-approval-status-list")]
		public async Task<ActionResult<IEnumerable<MdFailureApprovalStatus>>> GetMdFailureApprovalStatusList()
		{
			return await _context.MdFailureApprovalStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-approval-status-by-id")]
		public async Task<ActionResult<MdFailureApprovalStatus>> GetMdFailureApprovalStatus(int id)
		{
			var data = await _context.MdFailureApprovalStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-approval-status")]
		public async Task<ActionResult<MdFailureApprovalStatus>> AddMdFailureApprovalStatus(MdFailureApprovalStatus form)
		{
			_context.MdFailureApprovalStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureApprovalStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-approval-status")]
		public async Task<IActionResult> EditMdFailureApprovalStatus(int id, MdFailureApprovalStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureApprovalStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-failure-approval-status")]
		public async Task<IActionResult> DeleteMdFailureApprovalStatus(int id)
		{
			var data = await _context.MdFailureApprovalStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureApprovalStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureApprovalStatusExists(int id)
		{
			return _context.MdFailureApprovalStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdTransactionStatus
		[HttpGet]
		[Route("get-md-txn-status-list")]
		public async Task<ActionResult<IEnumerable<MdTransactionStatus>>> GetMdTransactionStatusList()
		{
			return await _context.MdTransactionStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-txn-status-by-id")]
		public async Task<ActionResult<MdTransactionStatus>> GetMdTransactionStatus(int id)
		{
			var data = await _context.MdTransactionStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-txn-status")]
		public async Task<ActionResult<MdTransactionStatus>> AddMdTransactionStatus(MdTransactionStatus form)
		{
			_context.MdTransactionStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdTransactionStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-txn-status")]
		public async Task<IActionResult> EditMdTransactionStatus(int id, MdTransactionStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdTransactionStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-txn-status")]
		public async Task<IActionResult> DeleteMdTransactionStatus(int id)
		{
			var data = await _context.MdTransactionStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdTransactionStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdTransactionStatusExists(int id)
		{
			return _context.MdTransactionStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdModules
		[HttpGet]
		[Route("get-md-sap-modules-list")]
		public async Task<ActionResult<IEnumerable<MdModules>>> GetMdModulesList()
		{
			return await _context.MdModules.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-modules-by-id")]
		public async Task<ActionResult<MdModules>> GetMdModules(int id)
		{
			var data = await _context.MdModules.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-modules")]
		public async Task<ActionResult<MdModules>> AddMdModules(MdModules form)
		{
			_context.MdModules.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdModules", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-modules")]
		public async Task<IActionResult> EditMdModules(int id, MdModules form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdModulesExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-modules")]
		public async Task<IActionResult> DeleteMdModules(int id)
		{
			var data = await _context.MdModules.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdModules.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdModulesExists(int id)
		{
			return _context.MdModules.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapAccessibility
		[HttpGet]
		[Route("get-md-sap-accessibility-list")]
		public async Task<ActionResult<IEnumerable<MdSapAccessibility>>> GetMdSapAccessibilityList()
		{
			return await _context.MdSapAccessibility.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-accessibility-by-id")]
		public async Task<ActionResult<MdSapAccessibility>> GetMdSapAccessibility(int id)
		{
			var data = await _context.MdSapAccessibility.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-accessibility")]
		public async Task<ActionResult<MdSapAccessibility>> AddMdSapAccessibility(MdSapAccessibility form)
		{
			_context.MdSapAccessibility.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapAccessibility", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-accessibility")]
		public async Task<IActionResult> EditMdSapAccessibility(int id, MdSapAccessibility form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapAccessibilityExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-accessibility")]
		public async Task<IActionResult> DeleteMdSapAccessibility(int id)
		{
			var data = await _context.MdSapAccessibility.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapAccessibility.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapAccessibilityExists(int id)
		{
			return _context.MdSapAccessibility.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapCauseCode
		[HttpGet]
		[Route("get-md-sap-cause-code-list")]
		public async Task<ActionResult<IEnumerable<MdSapCauseCode>>> GetMdSapCauseCodeList()
		{
			return await _context.MdSapCauseCode.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-cause-code-by-id")]
		public async Task<ActionResult<MdSapCauseCode>> GetMdSapCauseCode(int id)
		{
			var data = await _context.MdSapCauseCode.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-cause-code")]
		public async Task<ActionResult<MdSapCauseCode>> AddMdSapCauseCode(MdSapCauseCode form)
		{
			_context.MdSapCauseCode.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapCauseCode", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-cause-code")]
		public async Task<IActionResult> EditMdSapCauseCode(int id, MdSapCauseCode form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapCauseCodeExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-cause-code")]
		public async Task<IActionResult> DeleteMdSapCauseCode(int id)
		{
			var data = await _context.MdSapCauseCode.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapCauseCode.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapCauseCodeExists(int id)
		{
			return _context.MdSapCauseCode.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapCodeGrpCause
		[HttpGet]
		[Route("get-md-sap-code-grp-cause-list")]
		public async Task<ActionResult<IEnumerable<MdSapCodeGrpCause>>> GetMdSapCodeGrpCauseList()
		{
			return await _context.MdSapCodeGrpCause.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-code-grp-cause-by-id")]
		public async Task<ActionResult<MdSapCodeGrpCause>> GetMdSapCodeGrpCause(int id)
		{
			var data = await _context.MdSapCodeGrpCause.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-code-grp-cause")]
		public async Task<ActionResult<MdSapCodeGrpCause>> AddMdSapCodeGrpCause(MdSapCodeGrpCause form)
		{
			_context.MdSapCodeGrpCause.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapCodeGrpCause", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-code-grp-cause")]
		public async Task<IActionResult> EditMdSapCodeGrpCause(int id, MdSapCodeGrpCause form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapCodeGrpCauseExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-code-grp-cause")]
		public async Task<IActionResult> DeleteMdSapCodeGrpCause(int id)
		{
			var data = await _context.MdSapCodeGrpCause.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapCodeGrpCause.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapCodeGrpCauseExists(int id)
		{
			return _context.MdSapCodeGrpCause.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapCodeGrpDamage
		[HttpGet]
		[Route("get-md-sap-code-grp-damage-list")]
		public async Task<ActionResult<IEnumerable<MdSapCodeGrpDamage>>> GetMdSapCodeGrpDamageList()
		{
			return await _context.MdSapCodeGrpDamage.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-code-grp-damage-by-id")]
		public async Task<ActionResult<MdSapCodeGrpDamage>> GetMdSapCodeGrpDamage(int id)
		{
			var data = await _context.MdSapCodeGrpDamage.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-code-grp-damage")]
		public async Task<ActionResult<MdSapCodeGrpDamage>> AddMdSapCodeGrpDamage(MdSapCodeGrpDamage form)
		{
			_context.MdSapCodeGrpDamage.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapCodeGrpDamage", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-code-grp-damage")]
		public async Task<IActionResult> EditMdSapCodeGrpDamage(int id, MdSapCodeGrpDamage form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapCodeGrpDamageExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-code-grp-damage")]
		public async Task<IActionResult> DeleteMdSapCodeGrpDamage(int id)
		{
			var data = await _context.MdSapCodeGrpDamage.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapCodeGrpDamage.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapCodeGrpDamageExists(int id)
		{
			return _context.MdSapCodeGrpDamage.Any(e => e.id == id);
		}
		#endregion

		#region MdSapCodeGrpObjectPart
		[HttpGet]
		[Route("get-md-sap-code-grp-object-part-list")]
		public async Task<ActionResult<IEnumerable<MdSapCodeGrpObjectPart>>> GetMdSapCodeGrpObjectPartList()
		{
			return await _context.MdSapCodeGrpObjectPart.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-code-grp-object-part-by-id")]
		public async Task<ActionResult<MdSapCodeGrpObjectPart>> GetMdSapCodeGrpObjectPart(int id)
		{
			var data = await _context.MdSapCodeGrpObjectPart.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-code-grp-object-part")]
		public async Task<ActionResult<MdSapCodeGrpObjectPart>> AddMdSapCodeGrpObjectPart(MdSapCodeGrpObjectPart form)
		{
			_context.MdSapCodeGrpObjectPart.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapCodeGrpObjectPart", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-code-grp-object-part")]
		public async Task<IActionResult> EditMdSapCodeGrpObjectPart(int id, MdSapCodeGrpObjectPart form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapCodeGrpObjectPartExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-code-grp-object-part")]
		public async Task<IActionResult> DeleteMdSapCodeGrpObjectPart(int id)
		{
			var data = await _context.MdSapCodeGrpObjectPart.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapCodeGrpObjectPart.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapCodeGrpObjectPartExists(int id)
		{
			return _context.MdSapCodeGrpObjectPart.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapObjectPartCode
		[HttpGet]
		[Route("get-md-sap-object-part-code-list")]
		public async Task<ActionResult<IEnumerable<MdSapObjectPartCode>>> GetMdSapObjectPartCodeList()
		{
			return await _context.MdSapObjectPartCode.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-object-part-code-by-id")]
		public async Task<ActionResult<MdSapObjectPartCode>> GetMdSapObjectPartCode(int id)
		{
			var data = await _context.MdSapObjectPartCode.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-object-part-code")]
		public async Task<ActionResult<MdSapObjectPartCode>> AddMdSapObjectPartCode(MdSapObjectPartCode form)
		{
			_context.MdSapObjectPartCode.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapObjectPartCode", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-object-part-code")]
		public async Task<IActionResult> EditMdSapObjectPartCode(int id, MdSapObjectPartCode form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapObjectPartCodeExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-object-part-code")]
		public async Task<IActionResult> DeleteMdSapObjectPartCode(int id)
		{
			var data = await _context.MdSapObjectPartCode.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapObjectPartCode.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapObjectPartCodeExists(int id)
		{
			return _context.MdSapObjectPartCode.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapDamageCode
		[HttpGet]
		[Route("get-md-sap-damage-code-list")]
		public async Task<ActionResult<IEnumerable<MdSapDamageCode>>> GetMdSapDamageCodeList()
		{
			return await _context.MdSapDamageCode.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-damage-code-by-id")]
		public async Task<ActionResult<MdSapDamageCode>> GetMdSapDamageCode(int id)
		{
			var data = await _context.MdSapDamageCode.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-damage-code")]
		public async Task<ActionResult<MdSapDamageCode>> AddMdSapDamageCode(MdSapDamageCode form)
		{
			_context.MdSapDamageCode.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapDamageCode", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-damage-code")]
		public async Task<IActionResult> EditMdSapDamageCode(int id, MdSapDamageCode form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapDamageCodeExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-damage-code")]
		public async Task<IActionResult> DeleteMdSapDamageCode(int id)
		{
			var data = await _context.MdSapDamageCode.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapDamageCode.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapDamageCodeExists(int id)
		{
			return _context.MdSapDamageCode.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapFunctionalLocation
		[HttpGet]
		[Route("get-md-sap-functional-location-list")]
		public async Task<ActionResult<IEnumerable<MdSapFunctionalLocation>>> GetMdSapFunctionalLocationList()
		{
			return await _context.MdSapFunctionalLocation.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-functional-location-by-id")]
		public async Task<ActionResult<MdSapFunctionalLocation>> GetMdSapFunctionalLocation(int id)
		{
			var data = await _context.MdSapFunctionalLocation.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-sap-functional-location-by-platform")]
		public async Task<ActionResult<List<MdSapFunctionalLocation>>> GetMdSapFunctionalLocation(string platform)
		{
			if(platform == "MDPP")
			{
				platform = "MPP";
			}
			else if(platform == "MDLQ")
			{
				platform = "MLQ";
			}
			var data = await _context.MdSapFunctionalLocation
											.Where(a => a.functional_location != null && a.functional_location.Contains(platform))
											.ToListAsync();
			if(!data.Any())
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-functional-location")]
		public async Task<ActionResult<MdSapFunctionalLocation>> AddMdSapFunctionalLocation(MdSapFunctionalLocation form)
		{
			_context.MdSapFunctionalLocation.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapFunctionalLocation", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-functional-location")]
		public async Task<IActionResult> EditMdSapFunctionalLocation(int id, MdSapFunctionalLocation form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapFunctionalLocationExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-functional-location")]
		public async Task<IActionResult> DeleteMdSapFunctionalLocation(int id)
		{
			var data = await _context.MdSapFunctionalLocation.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapFunctionalLocation.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapFunctionalLocationExists(int id)
		{
			return _context.MdSapFunctionalLocation.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapMainWorkCtr
		[HttpGet]
		[Route("get-md-sap-main-work-ctr-list")]
		public async Task<ActionResult<IEnumerable<MdSapMainWorkCtr>>> GetMdSapMainWorkCtrList()
		{
			return await _context.MdSapMainWorkCtr.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-main-work-ctr-by-id")]
		public async Task<ActionResult<MdSapMainWorkCtr>> GetMdSapMainWorkCtr(int id)
		{
			var data = await _context.MdSapMainWorkCtr.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-main-work-ctr")]
		public async Task<ActionResult<MdSapMainWorkCtr>> AddMdSapMainWorkCtr(MdSapMainWorkCtr form)
		{
			_context.MdSapMainWorkCtr.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapMainWorkCtr", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-main-work-ctr")]
		public async Task<IActionResult> EditMdSapMainWorkCtr(int id, MdSapMainWorkCtr form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapMainWorkCtrExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-main-work-ctr")]
		public async Task<IActionResult> DeleteMdSapMainWorkCtr(int id)
		{
			var data = await _context.MdSapMainWorkCtr.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapMainWorkCtr.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapMainWorkCtrExists(int id)
		{
			return _context.MdSapMainWorkCtr.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapPlannerGrp
		[HttpGet]
		[Route("get-md-sap-planner-grp-list")]
		public async Task<ActionResult<IEnumerable<MdSapPlannerGrp>>> GetMdSapPlannerGrpList()
		{
			return await _context.MdSapPlannerGrp.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-planner-grp-by-id")]
		public async Task<ActionResult<MdSapPlannerGrp>> GetMdSapPlannerGrp(int id)
		{
			var data = await _context.MdSapPlannerGrp.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-planner-grp")]
		public async Task<ActionResult<MdSapPlannerGrp>> AddMdSapPlannerGrp(MdSapPlannerGrp form)
		{
			_context.MdSapPlannerGrp.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapPlannerGrp", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-planner-grp")]
		public async Task<IActionResult> EditMdSapPlannerGrp(int id, MdSapPlannerGrp form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapPlannerGrpExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-planner-grp")]
		public async Task<IActionResult> DeleteMdSapPlannerGrp(int id)
		{
			var data = await _context.MdSapPlannerGrp.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapPlannerGrp.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapPlannerGrpExists(int id)
		{
			return _context.MdSapPlannerGrp.Any(e => e.id == id);
		}
		#endregion
	
		#region MdSapPlannerGrpPlanningPlant
		[HttpGet]
		[Route("get-md-sap-planner-grp-planning-plant-list")]
		public async Task<ActionResult<IEnumerable<MdSapPlannerGrpPlanningPlant>>> GetMdSapPlannerGrpPlanningPlantList()
		{
			return await _context.MdSapPlannerGrpPlanningPlant.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-planner-grp-planning-plant-by-id")]
		public async Task<ActionResult<MdSapPlannerGrpPlanningPlant>> GetMdSapPlannerGrpPlanningPlant(int id)
		{
			var data = await _context.MdSapPlannerGrpPlanningPlant.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-planner-grp-planning-plant")]
		public async Task<ActionResult<MdSapPlannerGrpPlanningPlant>> AddMdSapPlannerGrpPlanningPlant(MdSapPlannerGrpPlanningPlant form)
		{
			_context.MdSapPlannerGrpPlanningPlant.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapPlannerGrpPlanningPlant", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-planner-grp-planning-plant")]
		public async Task<IActionResult> EditMdSapPlannerGrpPlanningPlant(int id, MdSapPlannerGrpPlanningPlant form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapPlannerGrpPlanningPlantExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-planner-grp-planning-plant")]
		public async Task<IActionResult> DeleteMdSapPlannerGrpPlanningPlant(int id)
		{
			var data = await _context.MdSapPlannerGrpPlanningPlant.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapPlannerGrpPlanningPlant.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapPlannerGrpPlanningPlantExists(int id)
		{
			return _context.MdSapPlannerGrpPlanningPlant.Any(e => e.id == id);
		}
		#endregion

		#region MdSapScaffolding
		[HttpGet]
		[Route("get-md-sap-scaffolding-list")]
		public async Task<ActionResult<IEnumerable<MdSapScaffolding>>> GetMdSapScaffoldingList()
		{
			return await _context.MdSapScaffolding.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-sap-scaffolding-by-id")]
		public async Task<ActionResult<MdSapScaffolding>> GetMdSapScaffolding(int id)
		{
			var data = await _context.MdSapScaffolding.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-sap-scaffolding")]
		public async Task<ActionResult<MdSapScaffolding>> AddMdSapScaffolding(MdSapScaffolding form)
		{
			_context.MdSapScaffolding.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdSapScaffolding", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-sap-scaffolding")]
		public async Task<IActionResult> EditMdSapScaffolding(int id, MdSapScaffolding form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdSapScaffoldingExists(id))
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

		[HttpDelete]
		[Route("delete-md-sap-scaffolding")]
		public async Task<IActionResult> DeleteMdSapScaffolding(int id)
		{
			var data = await _context.MdSapScaffolding.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdSapScaffolding.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdSapScaffoldingExists(int id)
		{
			return _context.MdSapScaffolding.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionChecklistStatus
		[HttpGet]
		[Route("get-md-ex-insp-chk-status-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionChecklistStatus>>> GetMdExInspectionChecklistStatusList()
		{
			return await _context.MdExInspectionChecklistStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-chk-status-by-id")]
		public async Task<ActionResult<MdExInspectionChecklistStatus>> GetMdExInspectionChecklistStatus(int id)
		{
			var data = await _context.MdExInspectionChecklistStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-chk-status")]
		public async Task<ActionResult<MdExInspectionChecklistStatus>> AddMdExInspectionChecklistStatus(MdExInspectionChecklistStatus form)
		{
			_context.MdExInspectionChecklistStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionChecklistStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-chk-status")]
		public async Task<IActionResult> EditMdExInspectionChecklistStatus(int id, MdExInspectionChecklistStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionChecklistStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-chk-status")]
		public async Task<IActionResult> DeleteMdExInspectionChecklistStatus(int id)
		{
			var data = await _context.MdExInspectionChecklistStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionChecklistStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionChecklistStatusExists(int id)
		{
			return _context.MdExInspectionChecklistStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionPictureLogStatus
		[HttpGet]
		[Route("get-md-ex-insp-pic-log-status-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionPictureLogStatus>>> GetMdExInspectionPictureLogStatusList()
		{
			return await _context.MdExInspectionPictureLogStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-pic-log-status-by-id")]
		public async Task<ActionResult<MdExInspectionPictureLogStatus>> GetMdExInspectionPictureLogStatus(int id)
		{
			var data = await _context.MdExInspectionPictureLogStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-pic-log-status")]
		public async Task<ActionResult<MdExInspectionPictureLogStatus>> AddMdExInspectionPictureLogStatus(MdExInspectionPictureLogStatus form)
		{
			_context.MdExInspectionPictureLogStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionPictureLogStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-pic-log-status")]
		public async Task<IActionResult> EditMdExInspectionPictureLogStatus(int id, MdExInspectionPictureLogStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionPictureLogStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-pic-log-status")]
		public async Task<IActionResult> DeleteMdExInspectionPictureLogStatus(int id)
		{
			var data = await _context.MdExInspectionPictureLogStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionPictureLogStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionPictureLogStatusExists(int id)
		{
			return _context.MdExInspectionPictureLogStatus.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionAreaStandard
		[HttpGet]
		[Route("get-md-ex-insp-area-std-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionAreaStandard>>> GetMdExInspectionAreaStandardList()
		{
			return await _context.MdExInspectionAreaStandard.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-area-std-by-id")]
		public async Task<ActionResult<MdExInspectionAreaStandard>> GetMdExInspectionAreaStandard(int id)
		{
			var data = await _context.MdExInspectionAreaStandard.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-area-std")]
		public async Task<ActionResult<MdExInspectionAreaStandard>> AddMdExInspectionAreaStandard(MdExInspectionAreaStandard form)
		{
			_context.MdExInspectionAreaStandard.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionAreaStandard", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-area-std")]
		public async Task<IActionResult> EditMdExInspectionAreaStandard(int id, MdExInspectionAreaStandard form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionAreaStandardExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-area-std")]
		public async Task<IActionResult> DeleteMdExInspectionAreaStandard(int id)
		{
			var data = await _context.MdExInspectionAreaStandard.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionAreaStandard.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionAreaStandardExists(int id)
		{
			return _context.MdExInspectionAreaStandard.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionAreaClass
		[HttpGet]
		[Route("get-md-ex-insp-area-class-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionAreaClass>>> GetMdExInspectionAreaClassList()
		{
			return await _context.MdExInspectionAreaClass.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-area-class-by-id")]
		public async Task<ActionResult<MdExInspectionAreaClass>> GetMdExInspectionAreaClass(int id)
		{
			var data = await _context.MdExInspectionAreaClass.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-area-class-by-id-area")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionAreaClassByIdArea(int id_area)
		{
			var data = await _context.MdExInspectionAreaClass.Where(a => a.id_area_standard == id_area).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-area-class")]
		public async Task<ActionResult<MdExInspectionAreaClass>> AddMdExInspectionAreaClass(MdExInspectionAreaClass form)
		{
			_context.MdExInspectionAreaClass.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionAreaClass", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-area-class")]
		public async Task<IActionResult> EditMdExInspectionAreaClass(int id, MdExInspectionAreaClass form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionAreaClassExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-area-class")]
		public async Task<IActionResult> DeleteMdExInspectionAreaClass(int id)
		{
			var data = await _context.MdExInspectionAreaClass.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionAreaClass.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionAreaClassExists(int id)
		{
			return _context.MdExInspectionAreaClass.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionAreaTempClass
		[HttpGet]
		[Route("get-md-ex-insp-area-temp-class-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionAreaTempClass>>> GetMdExInspectionAreaTempClassList()
		{
			return await _context.MdExInspectionAreaTempClass.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-area-temp-class-by-id")]
		public async Task<ActionResult<MdExInspectionAreaTempClass>> GetMdExInspectionAreaTempClass(int id)
		{
			var data = await _context.MdExInspectionAreaTempClass.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-area-temp-class-by-id-area")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionAreaTempClassByIdArea(int id_area)
		{
			var data = await _context.MdExInspectionAreaTempClass.Where(a => a.id_area_standard == id_area).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-area-temp-class")]
		public async Task<ActionResult<MdExInspectionAreaTempClass>> AddMdExInspectionAreaTempClass(MdExInspectionAreaTempClass form)
		{
			_context.MdExInspectionAreaTempClass.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionAreaTempClass", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-area-temp-class")]
		public async Task<IActionResult> EditMdExInspectionAreaTempClass(int id, MdExInspectionAreaTempClass form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionAreaTempClassExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-area-temp-class")]
		public async Task<IActionResult> DeleteMdExInspectionAreaTempClass(int id)
		{
			var data = await _context.MdExInspectionAreaTempClass.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionAreaTempClass.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionAreaTempClassExists(int id)
		{
			return _context.MdExInspectionAreaTempClass.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionAreaGasGroup
		[HttpGet]
		[Route("get-md-ex-insp-area-gas-group-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionAreaGasGroup>>> GetMdExInspectionAreaGasGroupList()
		{
			return await _context.MdExInspectionAreaGasGroup.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-area-gas-group-by-id")]
		public async Task<ActionResult<MdExInspectionAreaGasGroup>> GetMdExInspectionAreaGasGroup(int id)
		{
			var data = await _context.MdExInspectionAreaGasGroup.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-area-gas-group-by-id-area")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionAreaGasGroupByIdArea(int id_area)
		{
			var data = await _context.MdExInspectionAreaGasGroup.Where(a => a.id_area_standard == id_area).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-area-gas-group")]
		public async Task<ActionResult<MdExInspectionAreaGasGroup>> AddMdExInspectionAreaGasGroup(MdExInspectionAreaGasGroup form)
		{
			_context.MdExInspectionAreaGasGroup.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionAreaGasGroup", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-area-gas-group")]
		public async Task<IActionResult> EditMdExInspectionAreaGasGroup(int id, MdExInspectionAreaGasGroup form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionAreaGasGroupExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-area-gas-group")]
		public async Task<IActionResult> DeleteMdExInspectionAreaGasGroup(int id)
		{
			var data = await _context.MdExInspectionAreaGasGroup.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionAreaGasGroup.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionAreaGasGroupExists(int id)
		{
			return _context.MdExInspectionAreaGasGroup.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipStandard
		[HttpGet]
		[Route("get-md-ex-insp-equip-std-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipStandard>>> GetMdExInspectionEquipStandardList()
		{
			return await _context.MdExInspectionEquipStandard.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-std-by-id")]
		public async Task<ActionResult<MdExInspectionEquipStandard>> GetMdExInspectionEquipStandard(int id)
		{
			var data = await _context.MdExInspectionEquipStandard.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-std")]
		public async Task<ActionResult<MdExInspectionEquipStandard>> AddMdExInspectionEquipStandard(MdExInspectionEquipStandard form)
		{
			_context.MdExInspectionEquipStandard.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipStandard", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-std")]
		public async Task<IActionResult> EditMdExInspectionEquipStandard(int id, MdExInspectionEquipStandard form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipStandardExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-std")]
		public async Task<IActionResult> DeleteMdExInspectionEquipStandard(int id)
		{
			var data = await _context.MdExInspectionEquipStandard.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipStandard.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipStandardExists(int id)
		{
			return _context.MdExInspectionEquipStandard.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipProtectionLevelCategory
		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-lv-cat-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipProtectionLevelCategory>>> GetMdExInspectionEquipProtectionLevelCategoryList()
		{
			return await _context.MdExInspectionEquipProtectionLevelCategory.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-lv-cat-by-id")]
		public async Task<ActionResult<MdExInspectionEquipProtectionLevelCategory>> GetMdExInspectionEquipProtectionLevelCategory(int id)
		{
			var data = await _context.MdExInspectionEquipProtectionLevelCategory.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-lv-cat-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipProtectionLevelCategoryByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipProtectionLevelCategory.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-protec-lv-cat")]
		public async Task<ActionResult<MdExInspectionEquipProtectionLevelCategory>> AddMdExInspectionEquipProtectionLevelCategory(MdExInspectionEquipProtectionLevelCategory form)
		{
			_context.MdExInspectionEquipProtectionLevelCategory.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipProtectionLevelCategory", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-protec-lv-cat")]
		public async Task<IActionResult> EditMdExInspectionEquipProtectionLevelCategory(int id, MdExInspectionEquipProtectionLevelCategory form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipProtectionLevelCategoryExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-protec-lv-cat")]
		public async Task<IActionResult> DeleteMdExInspectionEquipProtectionLevelCategory(int id)
		{
			var data = await _context.MdExInspectionEquipProtectionLevelCategory.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipProtectionLevelCategory.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipProtectionLevelCategoryExists(int id)
		{
			return _context.MdExInspectionEquipProtectionLevelCategory.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipProtectionType
		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-type-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipProtectionType>>> GetMdExInspectionEquipProtectionTypeList()
		{
			return await _context.MdExInspectionEquipProtectionType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-type-by-id")]
		public async Task<ActionResult<MdExInspectionEquipProtectionType>> GetMdExInspectionEquipProtectionType(int id)
		{
			var data = await _context.MdExInspectionEquipProtectionType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-protec-type-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipProtectionTypeByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipProtectionType.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-protec-type")]
		public async Task<ActionResult<MdExInspectionEquipProtectionType>> AddMdExInspectionEquipProtectionType(MdExInspectionEquipProtectionType form)
		{
			_context.MdExInspectionEquipProtectionType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipProtectionType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-protec-type")]
		public async Task<IActionResult> EditMdExInspectionEquipProtectionType(int id, MdExInspectionEquipProtectionType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipProtectionTypeExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-protec-type")]
		public async Task<IActionResult> DeleteMdExInspectionEquipProtectionType(int id)
		{
			var data = await _context.MdExInspectionEquipProtectionType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipProtectionType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipProtectionTypeExists(int id)
		{
			return _context.MdExInspectionEquipProtectionType.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipClass
		[HttpGet]
		[Route("get-md-ex-insp-equip-class-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipClass>>> GetMdExInspectionEquipClassList()
		{
			return await _context.MdExInspectionEquipClass.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-class-by-id")]
		public async Task<ActionResult<MdExInspectionEquipClass>> GetMdExInspectionEquipClass(int id)
		{
			var data = await _context.MdExInspectionEquipClass.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-class-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipClassByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipClass.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-class")]
		public async Task<ActionResult<MdExInspectionEquipClass>> AddMdExInspectionEquipClass(MdExInspectionEquipClass form)
		{
			_context.MdExInspectionEquipClass.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipClass", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-class")]
		public async Task<IActionResult> EditMdExInspectionEquipClass(int id, MdExInspectionEquipClass form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipClassExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-class")]
		public async Task<IActionResult> DeleteMdExInspectionEquipClass(int id)
		{
			var data = await _context.MdExInspectionEquipClass.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipClass.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipClassExists(int id)
		{
			return _context.MdExInspectionEquipClass.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipTempClass
		[HttpGet]
		[Route("get-md-ex-insp-equip-temp-class-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipTempClass>>> GetMdExInspectionEquipTempClassList()
		{
			return await _context.MdExInspectionEquipTempClass.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-temp-class-by-id")]
		public async Task<ActionResult<MdExInspectionEquipTempClass>> GetMdExInspectionEquipTempClass(int id)
		{
			var data = await _context.MdExInspectionEquipTempClass.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-temp-class-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipTempClassByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipTempClass.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-temp-class")]
		public async Task<ActionResult<MdExInspectionEquipTempClass>> AddMdExInspectionEquipTempClass(MdExInspectionEquipTempClass form)
		{
			_context.MdExInspectionEquipTempClass.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipTempClass", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-temp-class")]
		public async Task<IActionResult> EditMdExInspectionEquipTempClass(int id, MdExInspectionEquipTempClass form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipTempClassExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-temp-class")]
		public async Task<IActionResult> DeleteMdExInspectionEquipTempClass(int id)
		{
			var data = await _context.MdExInspectionEquipTempClass.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipTempClass.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipTempClassExists(int id)
		{
			return _context.MdExInspectionEquipTempClass.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipGasGroup
		[HttpGet]
		[Route("get-md-ex-insp-equip-gas-group-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipGasGroup>>> GetMdExInspectionEquipGasGroupList()
		{
			return await _context.MdExInspectionEquipGasGroup.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-gas-group-by-id")]
		public async Task<ActionResult<MdExInspectionEquipGasGroup>> GetMdExInspectionEquipGasGroup(int id)
		{
			var data = await _context.MdExInspectionEquipGasGroup.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-gas-group-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipGasGroupByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipGasGroup.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-gas-group")]
		public async Task<ActionResult<MdExInspectionEquipGasGroup>> AddMdExInspectionEquipGasGroup(MdExInspectionEquipGasGroup form)
		{
			_context.MdExInspectionEquipGasGroup.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipGasGroup", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-gas-group")]
		public async Task<IActionResult> EditMdExInspectionEquipGasGroup(int id, MdExInspectionEquipGasGroup form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipGasGroupExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-gas-group")]
		public async Task<IActionResult> DeleteMdExInspectionEquipGasGroup(int id)
		{
			var data = await _context.MdExInspectionEquipGasGroup.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipGasGroup.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipGasGroupExists(int id)
		{
			return _context.MdExInspectionEquipGasGroup.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipIpRating
		[HttpGet]
		[Route("get-md-ex-insp-equip-ip-rating-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipIpRating>>> GetMdExInspectionEquipIpRatingList()
		{
			return await _context.MdExInspectionEquipIpRating.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-ip-rating-by-id")]
		public async Task<ActionResult<MdExInspectionEquipIpRating>> GetMdExInspectionEquipIpRating(int id)
		{
			var data = await _context.MdExInspectionEquipIpRating.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-ip-rating-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipIpRatingByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipIpRating.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-ip-rating")]
		public async Task<ActionResult<MdExInspectionEquipIpRating>> AddMdExInspectionEquipIpRating(MdExInspectionEquipIpRating form)
		{
			_context.MdExInspectionEquipIpRating.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipIpRating", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-ip-rating")]
		public async Task<IActionResult> EditMdExInspectionEquipIpRating(int id, MdExInspectionEquipIpRating form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipIpRatingExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-ip-rating")]
		public async Task<IActionResult> DeleteMdExInspectionEquipIpRating(int id)
		{
			var data = await _context.MdExInspectionEquipIpRating.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipIpRating.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipIpRatingExists(int id)
		{
			return _context.MdExInspectionEquipIpRating.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipEnclosureType
		[HttpGet]
		[Route("get-md-ex-insp-equip-enclosure-type-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipEnclosureType>>> GetMdExInspectionEquipEnclosureTypeList()
		{
			return await _context.MdExInspectionEquipEnclosureType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-enclosure-type-by-id")]
		public async Task<ActionResult<MdExInspectionEquipEnclosureType>> GetMdExInspectionEquipEnclosureType(int id)
		{
			var data = await _context.MdExInspectionEquipEnclosureType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-ex-insp-equip-enclosure-type-by-id-equip")]
		public async Task<ActionResult<dynamic>> GetMdExInspectionEquipEnclosureTypeByIdEquip(int id_equip)
		{
			var data = await _context.MdExInspectionEquipEnclosureType.Where(a => a.id_equip_standard == id_equip).ToListAsync();

			if(data.Count == 0)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-enclosure-type")]
		public async Task<ActionResult<MdExInspectionEquipEnclosureType>> AddMdExInspectionEquipEnclosureType(MdExInspectionEquipEnclosureType form)
		{
			_context.MdExInspectionEquipEnclosureType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipEnclosureType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-enclosure-type")]
		public async Task<IActionResult> EditMdExInspectionEquipEnclosureType(int id, MdExInspectionEquipEnclosureType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipEnclosureTypeExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-enclosure-type")]
		public async Task<IActionResult> DeleteMdExInspectionEquipEnclosureType(int id)
		{
			var data = await _context.MdExInspectionEquipEnclosureType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipEnclosureType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipEnclosureTypeExists(int id)
		{
			return _context.MdExInspectionEquipEnclosureType.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEquipType
		[HttpGet]
		[Route("get-md-ex-insp-equip-type-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEquipType>>> GetMdExInspectionEquipTypeList()
		{
			return await _context.MdExInspectionEquipType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-type-by-id")]
		public async Task<ActionResult<MdExInspectionEquipType>> GetMdExInspectionEquipType(int id)
		{
			var data = await _context.MdExInspectionEquipType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-type")]
		public async Task<ActionResult<MdExInspectionEquipType>> AddMdExInspectionEquipType(MdExInspectionEquipType form)
		{
			_context.MdExInspectionEquipType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEquipType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-type")]
		public async Task<IActionResult> EditMdExInspectionEquipType(int id, MdExInspectionEquipType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEquipTypeExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-type")]
		public async Task<IActionResult> DeleteMdExInspectionEquipType(int id)
		{
			var data = await _context.MdExInspectionEquipType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEquipType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEquipTypeExists(int id)
		{
			return _context.MdExInspectionEquipType.Any(e => e.id == id);
		}
		#endregion
	
		#region MdExInspectionEnvStatus
		[HttpGet]
		[Route("get-md-ex-insp-equip-env-status-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionEnvStatus>>> GetMdExInspectionEnvStatusList()
		{
			return await _context.MdExInspectionEnvStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-equip-env-status-by-id")]
		public async Task<ActionResult<MdExInspectionEnvStatus>> GetMdExInspectionEnvStatus(int id)
		{
			var data = await _context.MdExInspectionEnvStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-equip-env-status")]
		public async Task<ActionResult<MdExInspectionEnvStatus>> AddMdExInspectionEnvStatus(MdExInspectionEnvStatus form)
		{
			_context.MdExInspectionEnvStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionEnvStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-equip-env-status")]
		public async Task<IActionResult> EditMdExInspectionEnvStatus(int id, MdExInspectionEnvStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionEnvStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-ex-insp-equip-env-status")]
		public async Task<IActionResult> DeleteMdExInspectionEnvStatus(int id)
		{
			var data = await _context.MdExInspectionEnvStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionEnvStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionEnvStatusExists(int id)
		{
			return _context.MdExInspectionEnvStatus.Any(e => e.id == id);
		}
		#endregion

		#region MdExInspectionDiscipline
		[HttpGet]
		[Route("get-md-ex-insp-discip-line-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionDiscipline>>> GetMdExInspectionDisciplineList()
		{
			return await _context.MdExInspectionDiscipline.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-discip-line-by-id")]
		public async Task<ActionResult<MdExInspectionDiscipline>> GetMdExInspectionDiscipline(int id)
		{
			var data = await _context.MdExInspectionDiscipline.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-discip-line")]
		public async Task<ActionResult<MdExInspectionDiscipline>> AddMdExInspectionDiscipline(MdExInspectionDiscipline form)
		{
			_context.MdExInspectionDiscipline.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionDiscipline", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-discip-line")]
		public async Task<IActionResult> EditMdExInspectionDiscipline(int id, MdExInspectionDiscipline form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionDisciplineExists(id))
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


		[HttpDelete]
		[Route("delete-md-ex-insp-discip-line")]
		public async Task<IActionResult> DeleteMdExInspectionDiscipline(int id)
		{
			var data = await _context.MdExInspectionDiscipline.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionDiscipline.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionDisciplineExists(int id)
		{
			return _context.MdExInspectionDiscipline.Any(e => e.id == id);
		}
		#endregion

		#region MdExInspectionGasGroup
		[HttpGet]
		[Route("get-md-ex-insp-gas-group-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionGasGroup>>> GetMdExInspectionGasGroupList()
		{
			return await _context.MdExInspectionGasGroup.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-gas-group-by-id")]
		public async Task<ActionResult<MdExInspectionGasGroup>> GetMdExInspectionGasGroup(int id)
		{
			var data = await _context.MdExInspectionGasGroup.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-gas-group")]
		public async Task<ActionResult<MdExInspectionGasGroup>> AddMdExInspectionGasGroup(MdExInspectionGasGroup form)
		{
			_context.MdExInspectionGasGroup.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionGasGroup", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-gas-group")]
		public async Task<IActionResult> EditMdExInspectionGasGroup(int id, MdExInspectionGasGroup form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionGasGroupExists(id))
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


		[HttpDelete]
		[Route("delete-md-ex-insp-gas-group")]
		public async Task<IActionResult> DeleteMdExInspectionGasGroup(int id)
		{
			var data = await _context.MdExInspectionGasGroup.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionGasGroup.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionGasGroupExists(int id)
		{
			return _context.MdExInspectionGasGroup.Any(e => e.id == id);
		}
		#endregion

		#region MdExInspectionLocation
		[HttpGet]
		[Route("get-md-ex-insp-location-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionLocation>>> GetMdExInspectionLocationList()
		{
			return await _context.MdExInspectionLocation.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-location-by-id")]
		public async Task<ActionResult<MdExInspectionLocation>> GetMdExInspectionLocation(int id)
		{
			var data = await _context.MdExInspectionLocation.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-location")]
		public async Task<ActionResult<MdExInspectionLocation>> AddMdExInspectionLocation(MdExInspectionLocation form)
		{
			_context.MdExInspectionLocation.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionLocation", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-location")]
		public async Task<IActionResult> EditMdExInspectionLocation(int id, MdExInspectionLocation form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionLocationExists(id))
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


		[HttpDelete]
		[Route("delete-md-ex-insp-location")]
		public async Task<IActionResult> DeleteMdExInspectionLocation(int id)
		{
			var data = await _context.MdExInspectionLocation.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionLocation.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionLocationExists(int id)
		{
			return _context.MdExInspectionLocation.Any(e => e.id == id);
		}
		#endregion

		#region MdExInspectionZone
		[HttpGet]
		[Route("get-md-ex-insp-zone-list")]
		public async Task<ActionResult<IEnumerable<MdExInspectionZone>>> GetMdExInspectionZoneList()
		{
			return await _context.MdExInspectionZone.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-ex-insp-zone-by-id")]
		public async Task<ActionResult<MdExInspectionZone>> GetMdExInspectionZone(int id)
		{
			var data = await _context.MdExInspectionZone.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-ex-insp-zone")]
		public async Task<ActionResult<MdExInspectionZone>> AddMdExInspectionZone(MdExInspectionZone form)
		{
			_context.MdExInspectionZone.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdExInspectionZone", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-ex-insp-zone")]
		public async Task<IActionResult> EditMdExInspectionZone(int id, MdExInspectionZone form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdExInspectionZoneExists(id))
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


		[HttpDelete]
		[Route("delete-md-ex-insp-zone")]
		public async Task<IActionResult> DeleteMdExInspectionZone(int id)
		{
			var data = await _context.MdExInspectionZone.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdExInspectionZone.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdExInspectionZoneExists(int id)
		{
			return _context.MdExInspectionZone.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiRepair
		[HttpGet]
		[Route("get-md-gpi-repair-list")]
		public async Task<ActionResult<IEnumerable<MdGpiRepair>>> GetMdGpiRepairList()
		{
			return await _context.MdGpiRepair.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-repair-by-id")]
		public async Task<ActionResult<MdGpiRepair>> GetMdGpiRepair(int id)
		{
			var data = await _context.MdGpiRepair.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-repair")]
		public async Task<ActionResult<MdGpiRepair>> AddMdGpiRepair(MdGpiRepair form)
		{
			_context.MdGpiRepair.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiRepair", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-repair")]
		public async Task<IActionResult> EditMdGpiRepair(int id, MdGpiRepair form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiRepairExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-repair")]
		public async Task<IActionResult> DeleteMdGpiRepair(int id)
		{
			var data = await _context.MdGpiRepair.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiRepair.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiRepairExists(int id)
		{
			return _context.MdGpiRepair.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiRepairType
		[HttpGet]
		[Route("get-md-gpi-repair-type-list")]
		public async Task<ActionResult<IEnumerable<MdGpiRepairType>>> GetMdGpiRepairTypeList()
		{
			return await _context.MdGpiRepairType.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-repair-type-by-id")]
		public async Task<ActionResult<MdGpiRepairType>> GetMdGpiRepairType(int id)
		{
			var data = await _context.MdGpiRepairType.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-repair-type")]
		public async Task<ActionResult<MdGpiRepairType>> AddMdGpiRepairType(MdGpiRepairType form)
		{
			_context.MdGpiRepairType.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiRepairType", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-repair-type")]
		public async Task<IActionResult> EditMdGpiRepairType(int id, MdGpiRepairType form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiRepairTypeExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-repair-type")]
		public async Task<IActionResult> DeleteMdGpiRepairType(int id)
		{
			var data = await _context.MdGpiRepairType.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiRepairType.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiRepairTypeExists(int id)
		{
			return _context.MdGpiRepairType.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiMainComponent
		[HttpGet]
		[Route("get-md-gpi-main-component-list")]
		public async Task<ActionResult<IEnumerable<MdGpiMainComponent>>> GetMdGpiMainComponentList()
		{
			return await _context.MdGpiMainComponent.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-main-component-by-id")]
		public async Task<ActionResult<MdGpiMainComponent>> GetMdGpiMainComponent(int id)
		{
			var data = await _context.MdGpiMainComponent.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-main-component")]
		public async Task<ActionResult<MdGpiMainComponent>> AddMdGpiMainComponent(MdGpiMainComponent form)
		{
			_context.MdGpiMainComponent.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiMainComponent", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-main-component")]
		public async Task<IActionResult> EditMdGpiMainComponent(int id, MdGpiMainComponent form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiMainComponentExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-main-component")]
		public async Task<IActionResult> DeleteMdGpiMainComponent(int id)
		{
			var data = await _context.MdGpiMainComponent.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiMainComponent.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiMainComponentExists(int id)
		{
			return _context.MdGpiMainComponent.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiDamageMechanism
		[HttpGet]
		[Route("get-md-gpi-damage-mechanism-list")]
		public async Task<ActionResult<IEnumerable<MdGpiDamageMechanism>>> GetMdGpiDamageMechanismList()
		{
			return await _context.MdGpiDamageMechanism.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-damage-mechanism-by-id")]
		public async Task<ActionResult<MdGpiDamageMechanism>> GetMdGpiDamageMechanism(int id)
		{
			var data = await _context.MdGpiDamageMechanism.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-damage-mechanism")]
		public async Task<ActionResult<MdGpiDamageMechanism>> AddMdGpiDamageMechanism(MdGpiDamageMechanism form)
		{
			_context.MdGpiDamageMechanism.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiDamageMechanism", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-damage-mechanism")]
		public async Task<IActionResult> EditMdGpiDamageMechanism(int id, MdGpiDamageMechanism form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiDamageMechanismExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-damage-mechanism")]
		public async Task<IActionResult> DeleteMdGpiDamageMechanism(int id)
		{
			var data = await _context.MdGpiDamageMechanism.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiDamageMechanism.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiDamageMechanismExists(int id)
		{
			return _context.MdGpiDamageMechanism.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiSeverityStatus
		[HttpGet]
		[Route("get-md-gpi-severity-list")]
		public async Task<ActionResult<IEnumerable<MdGpiSeverityStatus>>> GetMdGpiSeverityStatusList()
		{
			return await _context.MdGpiSeverityStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-severity-by-id")]
		public async Task<ActionResult<MdGpiSeverityStatus>> GetMdGpiSeverityStatus(int id)
		{
			var data = await _context.MdGpiSeverityStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-severity")]
		public async Task<ActionResult<MdGpiSeverityStatus>> AddMdGpiSeverityStatus(MdGpiSeverityStatus form)
		{
			_context.MdGpiSeverityStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiSeverityStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-severity")]
		public async Task<IActionResult> EditMdGpiSeverityStatus(int id, MdGpiSeverityStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiSeverityStatusExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-severity")]
		public async Task<IActionResult> DeleteMdGpiSeverityStatus(int id)
		{
			var data = await _context.MdGpiSeverityStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiSeverityStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiSeverityStatusExists(int id)
		{
			return _context.MdGpiSeverityStatus.Any(e => e.id == id);
		}
		#endregion

		#region MdGpiLocationDeck
		[HttpGet]
		[Route("get-md-gpi-location-deck-list")]
		public async Task<ActionResult<IEnumerable<MdGpiLocationDeck>>> GetMdGpiLocationDeckList()
		{
			return await _context.MdGpiLocationDeck.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-location-deck-by-id")]
		public async Task<ActionResult<MdGpiLocationDeck>> GetMdGpiLocationDeck(int id)
		{
			var data = await _context.MdGpiLocationDeck.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-location-deck")]
		public async Task<ActionResult<MdGpiLocationDeck>> AddMdGpiLocationDeck(MdGpiLocationDeck form)
		{
			_context.MdGpiLocationDeck.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiLocationDeck", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-location-deck")]
		public async Task<IActionResult> EditMdGpiLocationDeck(int id, MdGpiLocationDeck form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiLocationDeckExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-location-deck")]
		public async Task<IActionResult> DeleteMdGpiLocationDeck(int id)
		{
			var data = await _context.MdGpiLocationDeck.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiLocationDeck.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiLocationDeckExists(int id)
		{
			return _context.MdGpiLocationDeck.Any(e => e.id == id);
		}
		#endregion
		
		#region MdGpiDiscipline
		[HttpGet]
		[Route("get-md-gpi-discipline-list")]
		public async Task<ActionResult<IEnumerable<MdGpiDiscipline>>> GetMdGpiDisciplineList()
		{
			return await _context.MdGpiDiscipline.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-discipline-by-id")]
		public async Task<ActionResult<MdGpiDiscipline>> GetMdGpiDiscipline(int id)
		{
			var data = await _context.MdGpiDiscipline.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-discipline")]
		public async Task<ActionResult<MdGpiDiscipline>> AddMdGpiDiscipline(MdGpiDiscipline form)
		{
			_context.MdGpiDiscipline.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiDiscipline", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-discipline")]
		public async Task<IActionResult> EditMdGpiDiscipline(int id, MdGpiDiscipline form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiDisciplineExists(id))
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


		[HttpDelete]
		[Route("delete-md-gpi-discipline")]
		public async Task<IActionResult> DeleteMdGpiDiscipline(int id)
		{
			var data = await _context.MdGpiDiscipline.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdGpiDiscipline.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiDisciplineExists(int id)
		{
			return _context.MdGpiDiscipline.Any(e => e.id == id);
		}
		#endregion

		#region MdCMMicroBacteriaStatus
		[HttpGet]
		[Route("get-md-cm-micro-bact-status-list")]
		public async Task<ActionResult<IEnumerable<MdCMMicroBacteriaStatus>>> GetMdCMMicroBacteriaStatusList()
		{
			return await _context.MdCMMicroBacteriaStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-cm-micro-bact-status-by-id")]
		public async Task<ActionResult<MdCMMicroBacteriaStatus>> GetMdCMMicroBacteriaStatus(int id)
		{
			var data = await _context.MdCMMicroBacteriaStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-cm-micro-bact-status")]
		public async Task<ActionResult<MdCMMicroBacteriaStatus>> AddMdCMMicroBacteriaStatus(MdCMMicroBacteriaStatus form)
		{
			_context.MdCMMicroBacteriaStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdCMMicroBacteriaStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-cm-micro-bact-status")]
		public async Task<IActionResult> EditMdCMMicroBacteriaStatus(int id, MdCMMicroBacteriaStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdCMMicroBacteriaStatusExists(id))
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


		[HttpDelete]
		[Route("delete-md-cm-micro-bact-status")]
		public async Task<IActionResult> DeleteMdCMMicroBacteriaStatus(int id)
		{
			var data = await _context.MdCMMicroBacteriaStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdCMMicroBacteriaStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdCMMicroBacteriaStatusExists(int id)
		{
			return _context.MdCMMicroBacteriaStatus.Any(e => e.id == id);
		}
		#endregion

		#region MdCMChemInjectionStatus
		[HttpGet]
		[Route("get-md-cm-chem-inj-status-list")]
		public async Task<ActionResult<IEnumerable<MdCMChemInjectionStatus>>> GetMdCMChemInjectionStatusList()
		{
			return await _context.MdCMChemInjectionStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-cm-chem-inj-status-by-id")]
		public async Task<ActionResult<MdCMChemInjectionStatus>> GetMdCMChemInjectionStatus(int id)
		{
			var data = await _context.MdCMChemInjectionStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-cm-chem-inj-status")]
		public async Task<ActionResult<MdCMChemInjectionStatus>> AddMdCMChemInjectionStatus(MdCMChemInjectionStatus form)
		{
			_context.MdCMChemInjectionStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdCMChemInjectionStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-cm-chem-inj-status")]
		public async Task<IActionResult> EditMdCMChemInjectionStatus(int id, MdCMChemInjectionStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdCMChemInjectionStatusExists(id))
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


		[HttpDelete]
		[Route("delete-md-cm-chem-inj-status")]
		public async Task<IActionResult> DeleteMdCMChemInjectionStatus(int id)
		{
			var data = await _context.MdCMChemInjectionStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdCMChemInjectionStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdCMChemInjectionStatusExists(int id)
		{
			return _context.MdCMChemInjectionStatus.Any(e => e.id == id);
		}
		#endregion

		#region MdFailurePOF
		[HttpGet]
		[Route("get-md-failure-pof-list")]
		public async Task<ActionResult<IEnumerable<MdFailurePOF>>> GetMdFailurePOFList()
		{
			return await _context.MdFailurePOF.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-pof-by-id")]
		public async Task<ActionResult<MdFailurePOF>> GetMdFailurePOF(int id)
		{
			var data = await _context.MdFailurePOF.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-pof")]
		public async Task<ActionResult<MdFailurePOF>> AddMdFailurePOF(MdFailurePOF form)
		{
			_context.MdFailurePOF.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailurePOF", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-pof")]
		public async Task<IActionResult> EditMdFailurePOF(int id, MdFailurePOF form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailurePOFExists(id))
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


		[HttpDelete]
		[Route("delete-md-failure-pof")]
		public async Task<IActionResult> DeleteMdFailurePOF(int id)
		{
			var data = await _context.MdFailurePOF.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailurePOF.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailurePOFExists(int id)
		{
			return _context.MdFailurePOF.Any(e => e.id == id);
		}
		#endregion

		#region MdFailureCOF
		[HttpGet]
		[Route("get-md-failure-cof-list")]
		public async Task<ActionResult<IEnumerable<MdFailureCOF>>> GetMdFailureCOFList()
		{
			return await _context.MdFailureCOF.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-cof-by-id")]
		public async Task<ActionResult<MdFailureCOF>> GetMdFailureCOF(int id)
		{
			var data = await _context.MdFailureCOF.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-cof")]
		public async Task<ActionResult<MdFailureCOF>> AddMdFailureCOF(MdFailureCOF form)
		{
			_context.MdFailureCOF.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureCOF", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-cof")]
		public async Task<IActionResult> EditMdFailureCOF(int id, MdFailureCOF form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureCOFExists(id))
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


		[HttpDelete]
		[Route("delete-md-failure-cof")]
		public async Task<IActionResult> DeleteMdFailureCOF(int id)
		{
			var data = await _context.MdFailureCOF.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureCOF.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureCOFExists(int id)
		{
			return _context.MdFailureCOF.Any(e => e.id == id);
		}
		#endregion
		
		#region MdFailureRiskMatrix
		[HttpGet]
		[Route("get-md-failure-risk-matrix-list")]
		public async Task<ActionResult<IEnumerable<MdFailureRiskMatrix>>> GetMdFailureRiskMatrixList()
		{
			return await _context.MdFailureRiskMatrix.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-risk-matrix-by-id")]
		public async Task<ActionResult<MdFailureRiskMatrix>> GetMdFailureRiskMatrix(int id)
		{
			var data = await _context.MdFailureRiskMatrix.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}
		
		[HttpGet]
		[Route("get-md-failure-risk-matrix-by-pof-cof")]
		public async Task<ActionResult<MdFailureRiskMatrix>> GetMdFailureRiskMatrixByPofCof(int id_pof, int id_cof)
		{
			var data = await _context.MdFailureRiskMatrix.FirstOrDefaultAsync(a => a.id_pof == id_pof && a.id_cof == id_cof);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-risk-matrix")]
		public async Task<ActionResult<MdFailureRiskMatrix>> AddMdFailureRiskMatrix(MdFailureRiskMatrix form)
		{
			_context.MdFailureRiskMatrix.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureRiskMatrix", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-risk-matrix")]
		public async Task<IActionResult> EditMdFailureRiskMatrix(int id, MdFailureRiskMatrix form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureRiskMatrixExists(id))
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


		[HttpDelete]
		[Route("delete-md-failure-risk-matrix")]
		public async Task<IActionResult> DeleteMdFailureRiskMatrix(int id)
		{
			var data = await _context.MdFailureRiskMatrix.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureRiskMatrix.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureRiskMatrixExists(int id)
		{
			return _context.MdFailureRiskMatrix.Any(e => e.id == id);
		}
		#endregion

		#region MdMonth
		[HttpGet]
		[Route("get-md-month-list")]
		public async Task<ActionResult<IEnumerable<MdMonth>>> GetMdMonth()
		{
			return await _context.MdMonth.ToListAsync();
		}

		#endregion

		#region MdCMWaterAnalysisStatus
		[HttpGet]
		[Route("get-md-cm-water-analysis-status-list")]
		public async Task<ActionResult<IEnumerable<MdCMWaterAnalysisStatus>>> GetMdCMWaterAnalysisStatusList()
		{
			return await _context.MdCMWaterAnalysisStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-cm-water-analysis-status-by-id")]
		public async Task<ActionResult<MdCMWaterAnalysisStatus>> GetMdCMWaterAnalysisStatus(int id)
		{
			var data = await _context.MdCMWaterAnalysisStatus.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-cm-water-analysis-status")]
		public async Task<ActionResult<MdCMWaterAnalysisStatus>> AddMdCMWaterAnalysisStatus(MdCMWaterAnalysisStatus form)
		{
			_context.MdCMWaterAnalysisStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdCMWaterAnalysisStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-cm-water-analysis-status")]
		public async Task<IActionResult> EditMdCMWaterAnalysisStatus(int id, MdCMWaterAnalysisStatus form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdCMWaterAnalysisStatusExists(id))
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


		[HttpDelete]
		[Route("delete-md-cm-water-analysis-status")]
		public async Task<IActionResult> DeleteMdCMWaterAnalysisStatus(int id)
		{
			var data = await _context.MdCMWaterAnalysisStatus.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdCMWaterAnalysisStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdCMWaterAnalysisStatusExists(int id)
		{
			return _context.MdCMWaterAnalysisStatus.Any(e => e.id == id);
		}
		#endregion
		
		#region MdGpiRecordStatus

		[HttpGet]
		[Route("get-md-gpi-record-status-list")]
		public async Task<ActionResult<IEnumerable<MdGpiRecordStatus>>> GetMdGpiRecordStatus()
		{
			return await _context.MdGpiRecordStatus.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-gpi-record-status-by-id")]
		public async Task<ActionResult<MdGpiRecordStatus>> GeMdGpiRecordStatus(int id)
		{
			var data = await _context.MdGpiRecordStatus.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-gpi-record-status")]
		public async Task<ActionResult<MdGpiRecordStatus>> AddMdGpiRecordStatus(MdGpiRecordStatus form)
		{
			_context.MdGpiRecordStatus.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdGpiRecordStatus", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-gpi-record-status")]
		public async Task<IActionResult> EditMdGpiRecordStatus(int id, MdGpiRecordStatus form)
		{
			if (id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdGpiRecordStatusExists(id))
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

		[HttpDelete]
		[Route("delete-md-gpi-record-status")]
		public async Task<IActionResult> DeleteMdGpiRecordStatus(int id)
		{
			var data = await _context.MdGpiRecordStatus.FindAsync(id);
			if (data == null)
			{
				return NotFound();
			}
			_context.MdGpiRecordStatus.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdGpiRecordStatusExists(int id)
		{
			return _context.MdGpiRecordStatus.Any(e => e.id == id);
		}

		#endregion

		#region MdFailureAuthRole
		[HttpGet]
		[Route("get-md-failure-auth-role-list")]
		public async Task<ActionResult<IEnumerable<MdFailureAuthRole>>> GetMdFailureAuthRoleList()
		{
			return await _context.MdFailureAuthRole.ToListAsync();
		}

		[HttpGet]
		[Route("get-md-failure-auth-role-by-id")]
		public async Task<ActionResult<MdFailureAuthRole>> GetMdFailureAuthRole(int id)
		{
			var data = await _context.MdFailureAuthRole.FindAsync(id);

			if(data == null)
			{
				return NotFound();
			}
			return data;
		}

		[HttpPost]
		[Route("add-md-failure-auth-role")]
		public async Task<ActionResult<MdFailureAuthRole>> AddMdFailureAuthRole(MdFailureAuthRole form)
		{
			_context.MdFailureAuthRole.Add(form);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMdFailureAuthRole", new { id = form.id }, form);
		}

		[HttpPut]
		[Route("edit-md-failure-auth-role")]
		public async Task<IActionResult> EditMdFailureAuthRole(int id, MdFailureAuthRole form)
		{
			if(id != form.id)
			{
				return BadRequest();
			}
			_context.Entry(form).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MdFailureAuthRoleExists(id))
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

		[HttpDelete]
		[Route("delete-md-failure-auth-role")]
		public async Task<IActionResult> DeleteMdFailureAuthRole(int id)
		{
			var data = await _context.MdFailureAuthRole.FindAsync(id);
			if (data == null) 
			{
				return NotFound();
			}
			_context.MdFailureAuthRole.Remove(data);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool MdFailureAuthRoleExists(int id)
		{
			return _context.MdFailureAuthRole.Any(e => e.id == id);
		}
		#endregion
	}
}