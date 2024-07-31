global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using CPOC_AIMS_II_Backend;
global using CPOC_AIMS_II_Backend.Models;
global using TestAPI.Services;

namespace CPOC_AIMS_II_Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
