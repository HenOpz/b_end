using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMCorrosionCouponMonitorDetailController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMCorrosionCouponMonitorDetailController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMCorrosionCouponMonitorDetail>> GetCMCorrosionCouponMonitorDetailList()
		{
			var data = await _context.CMCorrosionCouponMonitorDetail.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMCorrosionCouponMonitorDetail>> GetCMCorrosionCouponMonitorDetailById(int id)
		{
			var data = await _context.CMCorrosionCouponMonitorDetail.Where(a => a.id == id).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		[HttpGet("ByRecord/{id_record}")]
		public async Task<ActionResult<IEnumerable<CMCorrosionCouponMonitorDetail>>> GetCMCorrosionCouponMonitorDetailByTag(int id_record)
		{
			return await _context.CMCorrosionCouponMonitorDetail.Where(b => b.id_record == id_record).ToListAsync();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMCorrosionCouponMonitorDetail(int id, CMCorrosionCouponMonitorDetail data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			if (data.final_weight != null)
			{
				data.weight_loss = data.initial_weight - data.final_weight;
			}
			else
			{
				data.weight_loss = null;
			}

			try
			{
				var monitorRecord = await _context.CMCorrosionCouponMonitorRecord
												  .FirstOrDefaultAsync(r => r.id == data.id_record);

				if (monitorRecord != null)
				{
					if (data.weight_loss.HasValue && monitorRecord.expose_time.HasValue)
					{
						// data.corrosion_rate = (decimal)(data.weight_loss.Value * 365 * 1000 / (data.area * monitorRecord.expose_time.Value * data.density * (decimal)Math.Pow(2.54, 3)));

						data.corrosion_rate = (decimal)((87600 * data.weight_loss) / (data.area * monitorRecord.expose_time * 24 * data.density));
					}
					else
					{
						data.corrosion_rate = null;
					}

					if (data.max_pit_depth != null && monitorRecord.expose_time != null)
					{
						data.pitting_rate = (decimal)((data.max_pit_depth * 365) / monitorRecord.expose_time);
					}
					else
					{
						data.pitting_rate = null;
					}
				}
				else
				{
					return NotFound($"Monitor record not found for ID {data.id_record}.");
				}

				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMCorrosionCouponMonitorDetailExists(id))
				{
					return NotFound($"CMCorrosionCouponMonitorDetail with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMCorrosionCouponMonitorDetail>> PostCMCorrosionCouponMonitorDetail(CMCorrosionCouponMonitorDetail data)
		{
			if (data.final_weight != null)
			{
				data.weight_loss = data.initial_weight - data.final_weight;

				var monitorRecord = await _context.CMCorrosionCouponMonitorRecord
												  .FirstOrDefaultAsync(r => r.id == data.id_record);

				if (monitorRecord != null)
				{
					if (monitorRecord.expose_time != null)
					{
						// data.corrosion_rate = (decimal)((data.weight_loss * 365 * 1000) / (data.area * monitorRecord.expose_time * data.density * (decimal)Math.Pow(2.54, 3)));

						data.corrosion_rate = (decimal)((87600 * data.weight_loss) / (data.area * monitorRecord.expose_time * 24 * data.density));
					}
					else
					{
						data.corrosion_rate = null;
					}

					if (data.max_pit_depth != null && monitorRecord.expose_time != null)
					{
						data.pitting_rate = Math.Round((decimal)((data.max_pit_depth * 365) / monitorRecord.expose_time));
					}
					else
					{
						data.pitting_rate = null;
					}
				}
				else
				{
					return NotFound("Monitor record not found.");
				}
			}
			else
			{
				data.weight_loss = null;
				data.corrosion_rate = null;
			}

			_context.CMCorrosionCouponMonitorDetail.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMCorrosionCouponMonitorDetailById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMCorrosionCouponMonitorDetail(int id)
		{
			var data = await _context.CMCorrosionCouponMonitorDetail.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			_context.CMCorrosionCouponMonitorDetail.Remove(data);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMCorrosionCouponMonitorDetailExists(id))
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

		private bool CMCorrosionCouponMonitorDetailExists(int id)
		{
			return _context.CMCorrosionCouponMonitorDetail.Any(e => e.id == id);
		}
	}
}