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
	public class ReportGenerateController : ControllerBase
	{
		private readonly MainDbContext _context;
		public ReportGenerateController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("get-filter-report-data-by-period")]
		public async Task<IActionResult> GetFilterReportByPeriod(int fromMonth, int toMonth, int fromYear, int toYear)
		{
			try
			{
				Dictionary<string, dynamic> data_set = new Dictionary<string, dynamic>();

				DateTime startDate = new DateTime(fromYear, fromMonth, 1);
				DateTime endDate = new DateTime(toYear, toMonth, 1);

				var cm_wo = await _context.CMWOManagement.Where(a => a.record_month <= endDate)
														.OrderByDescending(a => a.record_month)
														.Take(5)
														.ToListAsync();

				data_set.Add("CM WO management", cm_wo);

				var insp_camp = await GetInspectionCampaignQuery()
										.Where(a => ((a.start_date >= startDate) && (a.start_date <= endDate)) || ((a.end_date >= startDate) && (a.end_date <= endDate)))
										.ToListAsync();

				data_set.Add("Inspection campaigns", insp_camp);

				var rect_plan = await GetRectificationCampaignQuery()
										.Where(a => (a.target_completion >= startDate) && (a.target_completion <= endDate))
										.ToListAsync();

				data_set.Add("Anomaly rectification plan", rect_plan);

				var hl_acti = await GetHighlightActivitiesQuery()
									.Where(a => (a.report_date >= startDate) && (a.report_date <= endDate))
									.ToListAsync();

				data_set.Add("Highlight activities", hl_acti);

				return Ok(data_set);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		private IQueryable<InspectionCampaignView> GetInspectionCampaignQuery()
		{
			return from ic in _context.InspectionCampaign
				   join stt in _context.MdInspectionCampaignStatus on ic.id_ic_status equals stt.id into icstt
				   from icsttResult in icstt.DefaultIfEmpty()

				   join cb in _context.User on ic.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on ic.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   where ic.is_active == true

				   orderby ic.start_date ascending

				   select new InspectionCampaignView
				   {
					   id = ic.id,
					   id_ic_status = ic.id_ic_status,
					   ic_status = icsttResult.status,
					   ic_status_color_code = icsttResult.color_code,
					   ic_number = ic.ic_number,
					   inspection_program = ic.inspection_program,
					   start_date = ic.start_date,
					   end_date = ic.end_date,
					   person_in_charge = ic.person_in_charge,
					   contractor = ic.contractor,
					   comments = ic.comments,
					   file_path = ic.file_path,
					   file_name = ic.file_name,
					   is_active = ic.is_active,
					   created_by = ic.created_by,
					   created_by_name = icbResult.name,
					   created_date = ic.created_date,
					   updated_by = ic.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = ic.updated_date,
				   };
		}

		private IQueryable<RectificationCampaignView> GetRectificationCampaignQuery()
		{
			return from rc in _context.RectificationCampaign
				   join wp in _context.MdRectificationStatus on rc.id_status_work_package equals wp.id into rcwp
				   from rcwpResult in rcwp.DefaultIfEmpty()

				   join man in _context.MdRectificationStatus on rc.id_status_manpower equals man.id into rcman
				   from rcmanResult in rcman.DefaultIfEmpty()

				   join eq in _context.MdRectificationStatus on rc.id_status_equipment equals eq.id into rceq
				   from rceqResult in rceq.DefaultIfEmpty()

				   join pob in _context.MdRectificationStatus on rc.id_status_pob equals pob.id into rcpob
				   from rcpobResult in rcpob.DefaultIfEmpty()

				   join ex in _context.MdRectificationStatus on rc.id_status_execute equals ex.id into rcex
				   from rcexResult in rcex.DefaultIfEmpty()

				   join cb in _context.User on rc.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on rc.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   where rc.is_active == true

				   select new RectificationCampaignView
				   {
					   id = rc.id,
					   rc_number = rc.rc_number,
					   rc_issue = rc.rc_issue,
					   person_in_charge = rc.person_in_charge,
					   contactor = rc.contactor,
					   target_completion = rc.target_completion,
					   comments = rc.comments,
					   id_status_work_package = rc.id_status_work_package,
					   status_work_package = rcwpResult.status,
					   status_work_package_color = rcwpResult.color_code,
					   id_status_manpower = rc.id_status_manpower,
					   status_manpower = rcmanResult.status,
					   status_manpower_color = rcmanResult.color_code,
					   id_status_equipment = rc.id_status_equipment,
					   status_equipment = rceqResult.status,
					   status_equipment_color = rceqResult.color_code,
					   id_status_pob = rc.id_status_pob,
					   status_pob = rcpobResult.status,
					   status_pob_color = rcpobResult.color_code,
					   id_status_execute = rc.id_status_execute,
					   status_execute = rcexResult.status,
					   status_execute_color = rcexResult.color_code,
					   is_active = rc.is_active,
					   created_by = rc.created_by,
					   created_by_name = icbResult.name,
					   created_date = rc.created_date,
					   updated_by = rc.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = rc.updated_date,
				   };
		}

		private IQueryable<HighlightActivitiesView> GetHighlightActivitiesQuery()
		{
			return from hl in _context.HighlightActivities
				   join pf in _context.MdPlatform on hl.id_platform equals pf.id into hlpf
				   from hlpfResult in hlpf.DefaultIfEmpty()

				   join at in _context.MdAssetType on hl.id_asset equals at.id into hlat
				   from hlatResult in hlat.DefaultIfEmpty()

				   join cb in _context.User on hl.created_by equals cb.id into icb
				   from icbResult in icb.DefaultIfEmpty()

				   join ub in _context.User on hl.updated_by equals ub.id into iub
				   from iubResult in iub.DefaultIfEmpty()

				   where hl.is_active == true

				   select new HighlightActivitiesView
				   {
					   id = hl.id,
					   id_platform = hl.id_platform,
					   platform_code_name = hlpfResult.code_name,
					   platform_full_name = hlpfResult.full_name,
					   phase = hlpfResult.phase,
					   id_asset = hl.id_asset,
					   asset_type = hlatResult.asset_type,
					   asset_icon = hlatResult.icon,
					   ha_number = hl.ha_number,
					   tag_number = hl.tag_number,
					   report_date = hl.report_date,
					   activities = hl.activities,
					   person_in_charge = hl.person_in_charge,
					   contractor = hl.contractor,
					   details = hl.details,
					   is_active = hl.is_active,
					   created_by = hl.created_by,
					   created_by_name = icbResult.name,
					   created_date = hl.created_date,
					   updated_by = hl.updated_by,
					   updated_by_name = iubResult.name,
					   updated_date = hl.updated_date,
				   };
		}
	}
}