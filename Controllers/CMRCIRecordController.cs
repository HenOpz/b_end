using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMRCIRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMRCIRecordController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CMRCIRecord>>> GetCMRCIRecord()
		{
			return await _context.CMRCIRecord.ToListAsync();
		}

		[HttpGet]
		[Route("get-cm-rci-record-by-id-info")]
		public async Task<ActionResult<List<CMRCIRecord>>> GetCMRCIRecordByIdInfo(int id)
		{
			var data = await _context.CMRCIRecord.Where(a => a.id_tag == id).ToListAsync();

			if (!data.Any())
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet]
		[Route("get-latest-cm-rci-records")]
		public async Task<ActionResult<List<object>>> GetLatestCMRCIRecords()
		{
			var inf = await _context.CMInfo.Where(a => a.is_active == true && a.id_system == 4).ToListAsync();

			var platforms = await _context.MdPlatform.ToListAsync();

			var month = await _context.MdMonth.ToListAsync();

			List<CMInfoWithRCI> rciList = new List<CMInfoWithRCI>();

			foreach (CMInfo data in inf)
			{
				var latestRecord = await _context.CMRCIRecord.Where(a => a.id_tag == data.id)
												.OrderByDescending(a => a.year)
												.ThenByDescending(a => a.id_month)
												.FirstOrDefaultAsync();
				var pt = platforms.Where(a => a.id == data.id_platform).FirstOrDefault();
				var mt = latestRecord != null ? month.Where(a => a.id == latestRecord.id_month).FirstOrDefault() : null;
				rciList.Add(new CMInfoWithRCI
				{
					id_record = latestRecord != null ? latestRecord.id : null,
					id_tag = data.id,
					id_platform = data.id_platform,
					platform = pt != null ? pt.code_name : null,
					tag_no = data.tag_no,
					desc = data.desc,
					temp_c = data.temp_c,
					last_date = latestRecord != null && mt != null ? $"{mt.month_code}-{latestRecord.year}" : null,
					ci_injection_rate = latestRecord != null ? latestRecord.ci_injection_rate : null,
					rci_val = latestRecord != null ? latestRecord.rci_val : null,
					note = latestRecord != null ? latestRecord.note : null,
				});
			}

			if (!rciList.Any())
			{
				return NotFound();
			}

			return Ok(rciList);
		}

		[HttpGet]
		[Route("get-cm-rci-records-for-dashboard")]
		public async Task<ActionResult<List<RCIValueDashboard>>> GetCMRCIRecordsForDashboard(int year_no)
		{
			var activeCMInfos = await _context.CMInfo
				.Where(a => a.is_active.HasValue && a.is_active.Value && a.id_system == 4)
				.Select(a => new { a.id, a.tag_no })
				.ToListAsync();

			var months = await _context.MdMonth
				.Select(mt => new { mt.id, mt.month_code, mt.month_no })
				.ToListAsync();

			var cmInfoIds = activeCMInfos.Select(a => a.id).ToList();
			var cmRCIRecords = await _context.CMRCIRecord
				.Where(r => cmInfoIds.Contains(r.id_tag) && r.year == year_no)
				.ToListAsync();

			var dbList = activeCMInfos.Select(data =>
			{
				var values = months.Select(mt =>
				{
					var rciData = cmRCIRecords.FirstOrDefault(r => r.id_tag == data.id && r.id_month == mt.id);
					return new RCIValue
					{
						month_code = mt.month_code,
						month_no = mt.month_no,
						ci_injection_rate = rciData?.ci_injection_rate,
						rci_val = rciData?.rci_val
					};
				}).ToList();

				return new RCIValueDashboard
				{
					id_tag = data.id,
					tag_no = data.tag_no,
					values = values
				};
			}).ToList();

			return Ok(dbList);
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<CMRCIRecord>> GetCMRCIRecord(int id)
		{
			var data = await _context.CMRCIRecord.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMRCIRecord(int id, CMRCIRecord data)
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
				if (!CMRCIRecordExists(id))
				{
					return NotFound($"CMRCIRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMRCIRecord>> PostCMRCIRecord(CMRCIRecord data)
		{
			data.created_date = DateTime.Now;
			_context.CMRCIRecord.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMRCIRecord", new { id = data.id }, data);
		}

		[HttpDelete]
		[Route("delete-cm-rci-record")]
		public async Task<IActionResult> DeleteCMRCIRecord(int id)
		{
			var data = await _context.CMRCIRecord.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMRCIRecordExists(id))
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

		private bool CMRCIRecordExists(int id)
		{
			return _context.CMRCIRecord.Any(e => e.id == id);
		}
	}
}