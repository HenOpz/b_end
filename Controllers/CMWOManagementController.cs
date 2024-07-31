using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMWOManagementController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMWOManagementController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetCMWOManagementList()
		{
			var data = await _context.CMWOManagement.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCMWOManagement(int id)
		{
			var data = await _context.CMWOManagement.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return Ok(data);
		}
		
		[HttpGet]
		[Route("get-cm-wo-management-for-chart")]
		public async Task<IActionResult> GetCMWOManagementForChart()
		{
			var data = await _context.CMWOManagement.OrderBy(a => a.record_month)
													.Take(5)
													.ToListAsync();
			if (data.Count == 0) return NotFound();
			
			return Ok(data);
		}
		
		[HttpGet]
		[Route("get-data-for-mapping-table")]
		public async Task<IActionResult> GetDataForMappingTable(int m_to, int y_to, int m_from, int y_from)
		{
			Dictionary<string,dynamic?> data = new Dictionary<string,dynamic?>();
			DateTime date_to = new DateTime(y_to, m_to,1);
			DateTime date_from = new DateTime(y_from, m_from, DateTime.DaysInMonth(y_from, m_from));
			
			try
			{
				var cm_wo = await _context.CMWOManagement.Where(a => a.record_month >= date_to && a.record_month <= date_from).ToListAsync();
				data.Add("cm_wo",cm_wo);
				
				var insp_camp = await _context.InspectionCampaign.Where(a => (a.start_date >= date_to && a.start_date <= date_from) || (a.end_date >= date_to && a.end_date <= date_from)).ToListAsync();
				data.Add("insp_camp",insp_camp);
				
				var recti_camp = await _context.RectificationCampaign.Where(a => a.target_completion >= date_to && a.target_completion <= date_from).ToListAsync();
				data.Add("recti_camp",recti_camp);
				
				var highlight_act = await _context.HighlightActivities.Where(a => a.report_date >= date_to && a.report_date <= date_from).ToListAsync();
				data.Add("highlight_act",highlight_act);
				
				return Ok(data);
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMWOManagement(int id, CMWOManagement data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			try
			{
				var last_record = await _context.CMWOManagement.Where(a => a.record_month == new DateTime(data.record_month.Year, data.record_month.Month, 1).AddMonths(-1)).FirstOrDefaultAsync();
				if (last_record != null)
				{
					data.cm_total = last_record.cm_total + data.cm_opened - data.cm_closed;
				}
				
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWOManagementExists(id))
				{
					return NotFound($"CMWOManagement with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMWOManagement>> AddCMWOManagement(CMWOManagement data)
		{
			var last_record = await _context.CMWOManagement.Where(a => a.record_month == new DateTime(data.record_month.Year, data.record_month.Month, 1).AddMonths(-1)).FirstOrDefaultAsync();
			if (last_record != null)
			{
				data.cm_total = last_record.cm_total + data.cm_opened - data.cm_closed;
			}
			else
			{
				data.cm_total = data.cm_opened - data.cm_closed;
			}

			data.record_month = new DateTime(data.record_month.Year, data.record_month.Month, 1);

			_context.CMWOManagement.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMWOManagement", new { id = data.id }, data);
		}

		[HttpDelete]
		[Route("delete-cm-wo-management")]
		public async Task<IActionResult> DeleteCMWOManagement(int id)
		{
			var data = await _context.CMWOManagement.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMWOManagement.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMWOManagementExists(id))
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

		private bool CMWOManagementExists(int id)
		{
			return _context.CMWOManagement.Any(e => e.id == id);
		}
	}
}