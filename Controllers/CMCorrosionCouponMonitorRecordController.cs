using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMCorrosionCouponMonitorRecordController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMCorrosionCouponMonitorRecordController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMCorrosionCouponMonitorRecord>> GetCMCorrosionCouponMonitorRecordList()
		{
			var data = await _context.CMCorrosionCouponMonitorRecord.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMCorrosionCouponMonitorRecord>> GetCMCorrosionCouponMonitorRecordById(int id)
		{
			var data = await _context.CMCorrosionCouponMonitorRecord.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}
		
		[HttpGet("ByTag/{id_tag}")]
		public async Task<ActionResult<IEnumerable<CMCorrosionCouponMonitorRecord>>> GetCMCorrosionCouponMonitorRecordByTag(int id_tag)
		{
			return await _context.CMCorrosionCouponMonitorRecord.Where(b => b.id_tag == id_tag).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMCorrosionCouponMonitorRecord(int id, CMCorrosionCouponMonitorRecord data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			if (data.install_date != null && data.remove_date != null)
			{
				data.expose_time = (data.remove_date.Value - data.install_date.Value).Days;
			}
			else
			{
				data.expose_time = null;
			}

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				var relatedDetails = await _context.CMCorrosionCouponMonitorDetail
												   .Where(d => d.id_record == data.id)
												   .ToListAsync();

				foreach (var detail in relatedDetails)
				{
					if (detail.final_weight != null)
					{
						detail.weight_loss = detail.initial_weight - detail.final_weight;
					}
					else
					{
						detail.weight_loss = null;
					}

					if (data.expose_time != null)
					{
						if (detail.weight_loss != null)
						{
							detail.corrosion_rate = ((detail.weight_loss.Value * 365 * 1000) / (detail.area * data.expose_time.Value * detail.density * (decimal)Math.Pow(2.54, 3)));
						}
						else
						{
							detail.corrosion_rate = null;
						}

						if (detail.max_pit_depth != null)
						{
							detail.pitting_rate = ((detail.max_pit_depth.Value * 365) / data.expose_time.Value);
						}
						else
						{
							detail.pitting_rate = null;
						}
					}
					else
					{
						detail.corrosion_rate = null;
						detail.pitting_rate = null;
					}

					_context.Entry(detail).State = EntityState.Modified;
				}

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMCorrosionCouponMonitorRecordExists(id))
				{
					return NotFound($"CMCorrosionCouponMonitorRecord with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}


		[HttpPost]
		public async Task<ActionResult<CMCorrosionCouponMonitorRecord>> PostCMCorrosionCouponMonitorRecord(CMCorrosionCouponMonitorRecord data)
		{
			if (data.install_date != null && data.remove_date != null)
			{
				data.expose_time = (data.remove_date.Value - data.install_date.Value).Days;
			}
			else
			{
				data.expose_time = null;
			}
			_context.CMCorrosionCouponMonitorRecord.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMCorrosionCouponMonitorRecordById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMCorrosionCouponMonitorRecord(int id)
		{
			var data = await _context.CMCorrosionCouponMonitorRecord.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMCorrosionCouponMonitorRecord.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMCorrosionCouponMonitorRecordExists(id))
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

		private bool CMCorrosionCouponMonitorRecordExists(int id)
		{
			return _context.CMCorrosionCouponMonitorRecord.Any(e => e.id == id);
		}
	}
}