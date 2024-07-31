using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilemakerAPIController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public FilemakerAPIController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		[HttpPost("call-external-api")]
		public async Task<IActionResult> CallExternalApi()
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, "https://ai1.dexon-technology.com/fmi/data/v2/databases/Piping/sessions");
			request.Headers.Add("Authorization", "Basic dmlzaXRvcjpBaW1zMjAyMg==");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;

			try
			{
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					return Ok(responseContent);
				}
				else
				{
					return StatusCode((int)response.StatusCode, responseContent);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}