using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Hangfire;
using CPOC_AIMS_II_Backend.Controllers;
using CPOC_AIMS_II_Backend.Services;

namespace CPOC_AIMS_II_Backend
{
	public class Startup
	{
		public IConfigurationRoot configRoot { get; set; }
		public static string? ConnectionString { get; private set; }
		public static string? AimsSapPath { get; private set; }
		public static string? SapAimsPath { get; private set; }
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			configRoot = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json")
				.Build();
			AimsSapPath = Configuration.GetConnectionString("AimsSapPath");
			SapAimsPath = Configuration.GetConnectionString("SapAimsPath");
		}

		public IConfiguration Configuration { get; }
		public IEmailService EmailService { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//CORS
			services.AddCors(c =>
			{
				c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			// E-mail
			services.AddTransient<IEmailService, EmailService>();

			//JSON Serializer
			services.AddControllersWithViews().AddNewtonsoftJson(options =>
			options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
				.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
				= new DefaultContractResolver());

			// JWT
			var secret = configRoot["Security:Secret"]?.ToString() ?? throw new InvalidOperationException("Security secret is not configured.");
			var key = Encoding.ASCII.GetBytes(secret);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};

			});

			services.AddControllers();
			services.AddHttpClient();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document API CPOC AIMS II Dexon", Version = "v1" });
				// To Enable authorization using Swagger (JWT)    
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
						  {
							  Reference = new OpenApiReference
							  {
								  Type = ReferenceType.SecurityScheme,
								  Id = "Bearer"
							  }
						  },
						  new string[] {}

					}
				});
			});

			//Hangfire
			services.AddHangfire(config =>
			{
				config.UseSqlServerStorage(configRoot.GetConnectionString("dbstring"));
			});
			services.AddHangfireServer();

			// Add connect database
			services.AddDbContext<MainDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("dbstring")));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJob, IWebHostEnvironment env)
		{
			//Enable CORS
			//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseCors(options => options
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetIsOriginAllowed(origin => true) // allow any origin
				.AllowCredentials()
				.WithExposedHeaders("filename")
			); // allow credentials

			if (env.IsDevelopment() || env.IsProduction())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "CPOC_AIMS_II_Backend v1");
					c.RoutePrefix = "swagger";
					c.ConfigObject.AdditionalItems.Add("logger", "console");
					c.DocExpansion(docExpansion: Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
				});

			}

			app.UseFileServer(new FileServerOptions
			{
				FileProvider = new PhysicalFileProvider(
		Path.Combine(env.ContentRootPath, "wwwroot")),
				RequestPath = "/wwwroot",
				EnableDirectoryBrowsing = true
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication(); // JWT requires this

			app.UseAuthorization();

			// RecurringJob.AddOrUpdate<SapHeaderController>("jobId_1",(SapHeaderController) => SapHeaderController.GetSapHeaderExport(),Cron.Minutely);
			// RecurringJob.AddOrUpdate<TestHangFireController>("jobId_1",(TestHangFireController) => TestHangFireController.PostTestHangFire(),Cron.Minutely);
			// backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

			app.UseHangfireDashboard();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// Set connect database
			ConnectionString = configRoot.GetConnectionString("dbstring")
				   ?? throw new InvalidOperationException("Database connection string is not configured.");
		}

	}
}
