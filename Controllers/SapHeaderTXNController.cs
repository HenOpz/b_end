using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SapHeaderTXNController : ControllerBase
	{
		private readonly MainDbContext _context;
		public SapHeaderTXNController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpGet]
		[Route("get-txn-list")]
		public async Task<ActionResult<IEnumerable<SapHeaderTXN>>> GetSapHeaderTXNList()
		{
			var data = await _context.SapHeaderTXN.ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}
		

		[HttpGet]
        public async Task<ActionResult<IEnumerable<SapHeaderTXN>>> GetSapHeaderTXNListInPeriod(DateTime fromDate , DateTime toDate)
        {
            var data = await _context.SapHeaderTXN.Where(a => a.txn_datetime >= fromDate && a.txn_datetime <= toDate).ToListAsync();
            if (data.Count == 0) return NotFound();
            return Ok(data);
        }
	}
}