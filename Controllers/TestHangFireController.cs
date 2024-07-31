using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestHangFireController : ControllerBase
	{
		private readonly MainDbContext _context;
		public TestHangFireController(MainDbContext context)
		{
			_context = context;
		}
		
		[HttpPost]
		public async Task<ActionResult<TestHangFire>> PostTestHangFire()
		{
			TestHangFire data = new TestHangFire();
			data.content = "Test at - " + DateTime.Now.ToString();
			data.input_time = DateTime.Now;
			_context.TestHangFire.Add(data);
			await _context.SaveChangesAsync();

			return Ok(data);
		}
	}
}