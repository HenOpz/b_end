using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CPOC_AIMS_II_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CMInfoController : ControllerBase
	{
		private readonly MainDbContext _context;
		public CMInfoController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<CMInfo>> GetCMInfoList()
		{
			var data = await _context.CMInfo.Where(a => a.is_active == true).ToListAsync();
			if (data.Count == 0) return NotFound();
			return Ok(data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CMInfo>> GetCMInfoById(int id)
		{
			var data = await _context.CMInfo.Where(a => (a.is_active == true) && (a.id == id)).FirstOrDefaultAsync();

			if (data == null)
			{
				return NotFound();
			}

			return data;
		}

		#region Produced Water Menu
		[HttpGet]
		[Route("get-tag-produced-water-view-in-water-analysis")]
		public async Task<ActionResult<IEnumerable<CMInfoProducedWaterWaterAnalysis>>> GetCMInfoProducedWaterWaterAnalysis()
		{
			try
			{
				var recentPh = await _context.CMWaterAnalysisPH
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentDissolvedO2 = await _context.CMWaterAnalysisDissolvedO2
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentIonCount = await _context.CMWaterAnalysisIonCount
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 2 && inf.is_water_analysis == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMWaterAnalysisStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join ph in recentPh on inf.id equals ph.id_tag into infph
							from phResult in infph.DefaultIfEmpty()
							join o2 in recentDissolvedO2 on inf.id equals o2.id_tag into info2
							from o2Result in info2.DefaultIfEmpty()
							let o2sttResult = status.FirstOrDefault(s => s.id == o2Result?.id_status)
							join ic in recentIonCount on inf.id equals ic.id_tag into infic
							from icResult in infic.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoProducedWaterWaterAnalysis
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								ph_lastest_period = phResult != null ? $"{phResult.period}/{phResult.year}" : null,
								ph_value = phResult?.ph_val,
								o2_lastest_period = o2Result != null ? $"{o2Result.period}/{o2Result.year}" : null,
								o2_value = o2Result?.dissolved_o2_val,
								id_status_o2 = o2Result?.id_status,
								severity_level_o2 = o2sttResult?.severity_level,
								color_name_o2 = o2sttResult?.color_name,
								color_code_o2 = o2sttResult?.color_code,
								ion_lastest_period = icResult != null ? $"{icResult.period}/{icResult.year}" : null,
								ion_value = icResult?.ion_count,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}


		[HttpGet]
		[Route("get-tag-produced-water-view-in-micro-bact")]
		public async Task<ActionResult<IEnumerable<CMInfoProducedWaterMicroBacteria>>> GetCMInfoProducedWaterMicroBacteria()
		{
			try
			{
				var recentSrb = await _context.CMMicroBacteriaSRB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentAtp = await _context.CMMicroBacteriaATP
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentGhb = await _context.CMMicroBacteriaGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentApghb = await _context.CMMicroBacteriaAPGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentSulphide = await _context.CMMicroBacteriaSulphide
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 2 && inf.is_micro_bacteria == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();
				var status = await _context.MdCMMicroBacteriaStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join srb in recentSrb on inf.id equals srb?.id_tag into infsrb
							from srbResult in infsrb.DefaultIfEmpty()
							join srbsttResult in status on srbResult?.id_status equals srbsttResult.id into srbstt
							from srbsttResult in srbstt.DefaultIfEmpty()
							join atp in recentAtp on inf.id equals atp?.id_tag into infatp
							from atpResult in infatp.DefaultIfEmpty()
							join atpsttResult in status on atpResult?.id_status equals atpsttResult.id into atpstt
							from atpsttResult in atpstt.DefaultIfEmpty()
							join ghb in recentGhb on inf.id equals ghb?.id_tag into infghb
							from ghbResult in infghb.DefaultIfEmpty()
							join ghbsttResult in status on ghbResult?.id_status equals ghbsttResult.id into ghbstt
							from ghbsttResult in ghbstt.DefaultIfEmpty()
							join apghb in recentApghb on inf.id equals apghb?.id_tag into infapghb
							from apghbResult in infapghb.DefaultIfEmpty()
							join apghbsttResult in status on apghbResult?.id_status equals apghbsttResult.id into apghbstt
							from apghbsttResult in apghbstt.DefaultIfEmpty()
							join sulphide in recentSulphide on inf.id equals sulphide?.id_tag into infsulphide
							from sulphideResult in infsulphide.DefaultIfEmpty()
							join sulphidesttResult in status on sulphideResult?.id_status equals sulphidesttResult.id into sulphidestt
							from sulphidesttResult in sulphidestt.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoProducedWaterMicroBacteria
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								srb_lastest_period = srbResult != null ? $"{srbResult.period}/{srbResult.year}" : null,
								srb_value = srbResult?.srb_val,
								srb_severity_level = srbsttResult?.severity_level,
								srb_color_name = srbsttResult?.color_name,
								srb_color_code = srbsttResult?.color_code,
								atp_lastest_period = atpResult != null ? $"{atpResult.period}/{atpResult.year}" : null,
								atp_value = atpResult?.atp_val,
								atp_severity_level = atpsttResult?.severity_level,
								atp_color_name = atpsttResult?.color_name,
								atp_color_code = atpsttResult?.color_code,
								ghb_lastest_period = ghbResult != null ? $"{ghbResult.period}/{ghbResult.year}" : null,
								ghb_value = ghbResult?.ghb_val,
								ghb_severity_level = ghbsttResult?.severity_level,
								ghb_color_name = ghbsttResult?.color_name,
								ghb_color_code = ghbsttResult?.color_code,
								apghb_lastest_period = apghbResult != null ? $"{apghbResult.period}/{apghbResult.year}" : null,
								apghb_value = apghbResult?.apghb_val,
								apghb_severity_level = apghbsttResult?.severity_level,
								apghb_color_name = apghbsttResult?.color_name,
								apghb_color_code = apghbsttResult?.color_code,
								sulphide_lastest_period = sulphideResult != null ? $"{sulphideResult.period}/{sulphideResult.year}" : null,
								sulphide_value = sulphideResult?.sulphide_val,
								sulphide_severity_level = sulphidesttResult?.severity_level,
								sulphide_color_name = sulphidesttResult?.color_name,
								sulphide_color_code = sulphidesttResult?.color_code,
							}).ToList();
				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-produced-water-view-in-corrosion-coupon")]
		public async Task<ActionResult<IEnumerable<CMInfoProducedWaterCorrosionCoupon>>> GetCMInfoProducedWaterCorrosionCoupon()
		{
			try
			{
				var recentCc = await _context.CMCorrosionCouponMonitorRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.remove_date)
								  .FirstOrDefault())
					.ToListAsync();

				var monitorDetails = await _context.CMCorrosionCouponMonitorDetail.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 2 && inf.is_corrosion_coupon == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				// Join data in-memory
				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join cc in recentCc on inf.id equals cc?.id_tag into infcc
							from ccResult in infcc.DefaultIfEmpty()
							join ccd in monitorDetails on ccResult?.id equals ccd.id_record into ccccd
							from ccccdResult in ccccd.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoProducedWaterCorrosionCoupon
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								remove_lastest_date = ccResult?.remove_date,
								corrosion_rate = ccccdResult?.corrosion_rate,
								max_pit_depth = ccccdResult?.max_pit_depth,
								pitting_rate = ccccdResult?.pitting_rate,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-produced-water-view-in-er-probe")]
		public async Task<ActionResult<IEnumerable<CMInfoProducedWaterERProbe>>> GetCMInfoProducedWaterERProbe()
		{
			try
			{
				var recentEr = await _context.CMERProbeRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.record_date)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 2 && inf.is_er_probe == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join er in recentEr on inf.id equals er.id_tag into infer
							from erResult in infer.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoProducedWaterERProbe
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								record_lastest_date = erResult?.record_date,
								metal_loss = erResult?.metal_loss,
								corrosion_rate = erResult?.corrosion_rate,
								note = erResult?.note,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}
		#endregion

		#region Pipeline Menu
		[HttpGet]
		[Route("get-tag-pipeline-view-in-water-analysis")]
		public async Task<ActionResult<IEnumerable<CMInfoPipelineWaterAnalysis>>> GetCMInfoPipelineWaterAnalysis()
		{
			try
			{
				var recentPh = await _context.CMWaterAnalysisPH
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentDissolvedO2 = await _context.CMWaterAnalysisDissolvedO2
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentIonCount = await _context.CMWaterAnalysisIonCount
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();
				
				var recentCO2 = await _context.CMWaterAnalysisCO2
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4 && inf.is_water_analysis == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMWaterAnalysisStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join ph in recentPh on inf.id equals ph.id_tag into infph
							from phResult in infph.DefaultIfEmpty()
							join co2 in recentCO2 on inf.id equals co2.id_tag into infco2
							from co2Result in infco2.DefaultIfEmpty()
							join o2 in recentDissolvedO2 on inf.id equals o2.id_tag into info2
							from o2Result in info2.DefaultIfEmpty()
							let o2sttResult = status.FirstOrDefault(s => s.id == o2Result?.id_status)
							join ic in recentIonCount on inf.id equals ic.id_tag into infic
							from icResult in infic.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoPipelineWaterAnalysis
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								ph_lastest_period = phResult != null ? $"{phResult.period}/{phResult.year}" : null,
								ph_value = phResult?.ph_val,
								co2_lastest_period = co2Result != null ? $"{co2Result.period}/{co2Result.year}" : null,
								co2_value = co2Result?.co2_val,
								o2_lastest_period = o2Result != null ? $"{o2Result.period}/{o2Result.year}" : null,
								o2_value = o2Result?.dissolved_o2_val,
								id_status_o2 = o2Result?.id_status,
								severity_level_o2 = o2sttResult?.severity_level,
								color_name_o2 = o2sttResult?.color_name,
								color_code_o2 = o2sttResult?.color_code,
								ion_lastest_period = icResult != null ? $"{icResult.period}/{icResult.year}" : null,
								ion_value = icResult?.ion_count,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-pipeline-view-in-micro-bact")]
		public async Task<ActionResult<IEnumerable<CMInfoPipelineMicroBacteria>>> GetCMInfoPipelineMicroBacteria()
		{
			try
			{
				var recentSrb = await _context.CMMicroBacteriaSRB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentAtp = await _context.CMMicroBacteriaATP
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentGhb = await _context.CMMicroBacteriaGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentApghb = await _context.CMMicroBacteriaAPGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentSulphide = await _context.CMMicroBacteriaSulphide
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4 && inf.is_micro_bacteria == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMMicroBacteriaStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join srb in recentSrb on inf.id equals srb?.id_tag into infsrb
							from srbResult in infsrb.DefaultIfEmpty()
							join srbsttResult in status on srbResult?.id_status equals srbsttResult.id into srbstt
							from srbsttResult in srbstt.DefaultIfEmpty()
							join atp in recentAtp on inf.id equals atp?.id_tag into infatp
							from atpResult in infatp.DefaultIfEmpty()
							join atpsttResult in status on atpResult?.id_status equals atpsttResult.id into atpstt
							from atpsttResult in atpstt.DefaultIfEmpty()
							join ghb in recentGhb on inf.id equals ghb?.id_tag into infghb
							from ghbResult in infghb.DefaultIfEmpty()
							join ghbsttResult in status on ghbResult?.id_status equals ghbsttResult.id into ghbstt
							from ghbsttResult in ghbstt.DefaultIfEmpty()
							join apghb in recentApghb on inf.id equals apghb?.id_tag into infapghb
							from apghbResult in infapghb.DefaultIfEmpty()
							join apghbsttResult in status on apghbResult?.id_status equals apghbsttResult.id into apghbstt
							from apghbsttResult in apghbstt.DefaultIfEmpty()
							join sulphide in recentSulphide on inf.id equals sulphide?.id_tag into infsulphide
							from sulphideResult in infsulphide.DefaultIfEmpty()
							join sulphidesttResult in status on sulphideResult?.id_status equals sulphidesttResult.id into sulphidestt
							from sulphidesttResult in sulphidestt.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoPipelineMicroBacteria
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								srb_lastest_period = srbResult != null ? $"{srbResult.period}/{srbResult.year}" : null,
								srb_value = srbResult?.srb_val,
								srb_severity_level = srbsttResult?.severity_level,
								srb_color_name = srbsttResult?.color_name,
								srb_color_code = srbsttResult?.color_code,
								atp_lastest_period = atpResult != null ? $"{atpResult.period}/{atpResult.year}" : null,
								atp_value = atpResult?.atp_val,
								atp_severity_level = atpsttResult?.severity_level,
								atp_color_name = atpsttResult?.color_name,
								atp_color_code = atpsttResult?.color_code,
								ghb_lastest_period = ghbResult != null ? $"{ghbResult.period}/{ghbResult.year}" : null,
								ghb_value = ghbResult?.ghb_val,
								ghb_severity_level = ghbsttResult?.severity_level,
								ghb_color_name = ghbsttResult?.color_name,
								ghb_color_code = ghbsttResult?.color_code,
								apghb_lastest_period = apghbResult != null ? $"{apghbResult.period}/{apghbResult.year}" : null,
								apghb_value = apghbResult?.apghb_val,
								apghb_severity_level = apghbsttResult?.severity_level,
								apghb_color_name = apghbsttResult?.color_name,
								apghb_color_code = apghbsttResult?.color_code,
								sulphide_lastest_period = sulphideResult != null ? $"{sulphideResult.period}/{sulphideResult.year}" : null,
								sulphide_value = sulphideResult?.sulphide_val,
								sulphide_severity_level = sulphidesttResult?.severity_level,
								sulphide_color_name = sulphidesttResult?.color_name,
								sulphide_color_code = sulphidesttResult?.color_code,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-pipeline-view-in-corrosion-coupon")]
		public async Task<ActionResult<IEnumerable<CMInfoPipelineCorrosionCoupon>>> GetCMInfoPipelineCorrosionCoupon()
		{
			try
			{
				var recentCc = await _context.CMCorrosionCouponMonitorRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.remove_date)
								  .FirstOrDefault())
					.ToListAsync();

				var monitorDetails = await _context.CMCorrosionCouponMonitorDetail.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4 && inf.is_corrosion_coupon == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join cc in recentCc on inf.id equals cc?.id_tag into infcc
							from ccResult in infcc.DefaultIfEmpty()
							join ccd in monitorDetails on ccResult?.id equals ccd.id_record into ccccd
							from ccccdResult in ccccd.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoPipelineCorrosionCoupon
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								remove_lastest_date = ccResult?.remove_date,
								corrosion_rate = ccccdResult?.corrosion_rate,
								max_pit_depth = ccccdResult?.max_pit_depth,
								pitting_rate = ccccdResult?.pitting_rate,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-pipeline-view-in-er-probe")]
		public async Task<ActionResult<IEnumerable<CMInfoPipelineERProbe>>> GetCMInfoPipelineERProbe()
		{
			try
			{
				var recentEr = await _context.CMERProbeRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.record_date)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4 && inf.is_er_probe == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join er in recentEr on inf.id equals er.id_tag into infer
							from erResult in infer.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoPipelineERProbe
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								record_lastest_date = erResult?.record_date,
								metal_loss = erResult?.metal_loss,
								corrosion_rate = erResult?.corrosion_rate,
								note = erResult?.note,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-pipeline-view-in-chem-injection")]
		public async Task<ActionResult<IEnumerable<CMInfoPipelineChemInjection>>> GetCMInfoPipelineChemInjectionByDate(DateTime dt)
		{
			try
			{
				var recentCi = await _context.CMChemInjectionRecord
					.Where(wa => wa.record_date == dt)
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4 && inf.is_ci == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMChemInjectionStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join ci in recentCi on inf.id equals ci.id_tag into infci
							from ciResult in infci.DefaultIfEmpty()
							join stt in status on ciResult.id_status equals stt.id into cistt
							from cisttResult in cistt.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoPipelineChemInjection
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								gas_flow_rate_mmscfd = ciResult.gas_flow_rate_mmscfd,
								req_ci_injection_rate_liters_mmscfd = ciResult.req_ci_injection_rate_liters_mmscfd,
								req_ci_rate_liters_day = ciResult.req_ci_rate_liters_day,
								yesterday_ci_tank_level_percent = ciResult.yesterday_ci_tank_level_percent,
								today_ci_tank_level_percent = ciResult.today_ci_tank_level_percent,
								actual_ci_injection_liters_day = ciResult.actual_ci_injection_liters_day,
								id_status = ciResult.id_status,
								severity_level = cisttResult.severity_level,
								color_name = cisttResult.color_name,
								color_code = cisttResult.color_code,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-pipeline-view-in-chem-injection-percentage")]
		public ActionResult<IDictionary<CMInfo, List<MonthlyPassPercentage>>> GetCMInfoPipelineChemInjectionPercentages(int year)
		{
			var cmInfos = _context.CMInfo.Where(a => a.id_system == 4 && a.is_active == true && a.is_ci == true).ToList();
			var chemInjectionRecords = _context.CMChemInjectionRecord.ToList();

			var result = CalculatePassPercentages(cmInfos, chemInjectionRecords, year);

			return Ok(result);
		}
		#endregion

		#region Cooling Medium
		[HttpGet]
		[Route("get-tag-cooling-medium-view-in-water-analysis")]
		public async Task<ActionResult<IEnumerable<CMInfoCoolingMediumWaterAnalysis>>> GetCMInfoCoolingMediumWaterAnalysis()
		{
			try
			{
				var recentPh = await _context.CMWaterAnalysisPH
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentDissolvedO2 = await _context.CMWaterAnalysisDissolvedO2
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentIonCount = await _context.CMWaterAnalysisIonCount
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 1 && inf.is_water_analysis == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMWaterAnalysisStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join ph in recentPh on inf.id equals ph.id_tag into infph
							from phResult in infph.DefaultIfEmpty()
							join o2 in recentDissolvedO2 on inf.id equals o2.id_tag into info2
							from o2Result in info2.DefaultIfEmpty()
							let o2sttResult = status.FirstOrDefault(s => s.id == o2Result?.id_status)
							join ic in recentIonCount on inf.id equals ic.id_tag into infic
							from icResult in infic.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoCoolingMediumWaterAnalysis
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								ph_lastest_period = phResult != null ? $"{phResult.period}/{phResult.year}" : null,
								ph_value = phResult?.ph_val,
								o2_lastest_period = o2Result != null ? $"{o2Result.period}/{o2Result.year}" : null,
								o2_value = o2Result?.dissolved_o2_val,
								id_status_o2 = o2Result?.id_status,
								severity_level_o2 = o2sttResult?.severity_level,
								color_name_o2 = o2sttResult?.color_name,
								color_code_o2 = o2sttResult?.color_code,
								ion_lastest_period = icResult != null ? $"{icResult.period}/{icResult.year}" : null,
								ion_value = icResult?.ion_count,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-cooling-medium-view-in-micro-bact")]
		public async Task<ActionResult<IEnumerable<CMInfoCoolingMediumMicroBacteria>>> GetCMInfoCoolingMediumMicroBacteria()
		{
			try
			{
				var recentSrb = await _context.CMMicroBacteriaSRB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentAtp = await _context.CMMicroBacteriaATP
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentGhb = await _context.CMMicroBacteriaGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentApghb = await _context.CMMicroBacteriaAPGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentSulphide = await _context.CMMicroBacteriaSulphide
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 1 && inf.is_micro_bacteria == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMMicroBacteriaStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join srb in recentSrb on inf.id equals srb?.id_tag into infsrb
							from srbResult in infsrb.DefaultIfEmpty()
							join srbsttResult in status on srbResult?.id_status equals srbsttResult.id into srbstt
							from srbsttResult in srbstt.DefaultIfEmpty()
							join atp in recentAtp on inf.id equals atp?.id_tag into infatp
							from atpResult in infatp.DefaultIfEmpty()
							join atpsttResult in status on atpResult?.id_status equals atpsttResult.id into atpstt
							from atpsttResult in atpstt.DefaultIfEmpty()
							join ghb in recentGhb on inf.id equals ghb?.id_tag into infghb
							from ghbResult in infghb.DefaultIfEmpty()
							join ghbsttResult in status on ghbResult?.id_status equals ghbsttResult.id into ghbstt
							from ghbsttResult in ghbstt.DefaultIfEmpty()
							join apghb in recentApghb on inf.id equals apghb?.id_tag into infapghb
							from apghbResult in infapghb.DefaultIfEmpty()
							join apghbsttResult in status on apghbResult?.id_status equals apghbsttResult.id into apghbstt
							from apghbsttResult in apghbstt.DefaultIfEmpty()
							join sulphide in recentSulphide on inf.id equals sulphide?.id_tag into infsulphide
							from sulphideResult in infsulphide.DefaultIfEmpty()
							join sulphidesttResult in status on sulphideResult?.id_status equals sulphidesttResult.id into sulphidestt
							from sulphidesttResult in sulphidestt.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoCoolingMediumMicroBacteria
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								srb_lastest_period = srbResult != null ? $"{srbResult.period}/{srbResult.year}" : null,
								srb_value = srbResult?.srb_val,
								srb_severity_level = srbsttResult?.severity_level,
								srb_color_name = srbsttResult?.color_name,
								srb_color_code = srbsttResult?.color_code,
								atp_lastest_period = atpResult != null ? $"{atpResult.period}/{atpResult.year}" : null,
								atp_value = atpResult?.atp_val,
								atp_severity_level = atpsttResult?.severity_level,
								atp_color_name = atpsttResult?.color_name,
								atp_color_code = atpsttResult?.color_code,
								ghb_lastest_period = ghbResult != null ? $"{ghbResult.period}/{ghbResult.year}" : null,
								ghb_value = ghbResult?.ghb_val,
								ghb_severity_level = ghbsttResult?.severity_level,
								ghb_color_name = ghbsttResult?.color_name,
								ghb_color_code = ghbsttResult?.color_code,
								apghb_lastest_period = apghbResult != null ? $"{apghbResult.period}/{apghbResult.year}" : null,
								apghb_value = apghbResult?.apghb_val,
								apghb_severity_level = apghbsttResult?.severity_level,
								apghb_color_name = apghbsttResult?.color_name,
								apghb_color_code = apghbsttResult?.color_code,
								sulphide_lastest_period = sulphideResult != null ? $"{sulphideResult.period}/{sulphideResult.year}" : null,
								sulphide_value = sulphideResult?.sulphide_val,
								sulphide_severity_level = sulphidesttResult?.severity_level,
								sulphide_color_name = sulphidesttResult?.color_name,
								sulphide_color_code = sulphidesttResult?.color_code,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-cooling-medium-view-in-corrosion-coupon")]
		public async Task<ActionResult<IEnumerable<CMInfoCoolingMediumCorrosionCoupon>>> GetCMInfoCoolingMediumCorrosionCoupon()
		{
			try
			{
				var recentCc = await _context.CMCorrosionCouponMonitorRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.remove_date)
								  .FirstOrDefault())
					.ToListAsync();

				var monitorDetails = await _context.CMCorrosionCouponMonitorDetail.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 1 && inf.is_corrosion_coupon == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				// Join data in-memory
				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join cc in recentCc on inf.id equals cc?.id_tag into infcc
							from ccResult in infcc.DefaultIfEmpty()
							join ccd in monitorDetails on ccResult?.id equals ccd.id_record into ccccd
							from ccccdResult in ccccd.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoCoolingMediumCorrosionCoupon
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								remove_lastest_date = ccResult?.remove_date,
								corrosion_rate = ccccdResult?.corrosion_rate,
								max_pit_depth = ccccdResult?.max_pit_depth,
								pitting_rate = ccccdResult?.pitting_rate,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-cooling-medium-view-in-er-probe")]
		public async Task<ActionResult<IEnumerable<CMInfoCoolingMediumERProbe>>> GetCMInfoCoolingMediumERProbe()
		{
			try
			{
				var recentEr = await _context.CMERProbeRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.record_date)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 1 && inf.is_er_probe == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join er in recentEr on inf.id equals er.id_tag into infer
							from erResult in infer.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoCoolingMediumERProbe
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								record_lastest_date = erResult?.record_date,
								metal_loss = erResult?.metal_loss,
								corrosion_rate = erResult?.corrosion_rate,
								note = erResult?.note,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}
		#endregion

		#region Sea Water
		[HttpGet]
		[Route("get-tag-sea-water-view-in-water-analysis")]
		public async Task<ActionResult<IEnumerable<CMInfoSeaWaterWaterAnalysis>>> GetCMInfoSeaWaterWaterAnalysis()
		{
			try
			{
				var recentPh = await _context.CMWaterAnalysisPH
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentDissolvedO2 = await _context.CMWaterAnalysisDissolvedO2
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentIonCount = await _context.CMWaterAnalysisIonCount
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 3 && inf.is_water_analysis == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMWaterAnalysisStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join ph in recentPh on inf.id equals ph.id_tag into infph
							from phResult in infph.DefaultIfEmpty()
							join o2 in recentDissolvedO2 on inf.id equals o2.id_tag into info2
							from o2Result in info2.DefaultIfEmpty()
							let o2sttResult = status.FirstOrDefault(s => s.id == o2Result?.id_status)
							join ic in recentIonCount on inf.id equals ic.id_tag into infic
							from icResult in infic.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoSeaWaterWaterAnalysis
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								ph_lastest_period = phResult != null ? $"{phResult.period}/{phResult.year}" : null,
								ph_value = phResult?.ph_val,
								o2_lastest_period = o2Result != null ? $"{o2Result.period}/{o2Result.year}" : null,
								o2_value = o2Result?.dissolved_o2_val,
								id_status_o2 = o2Result?.id_status,
								severity_level_o2 = o2sttResult?.severity_level,
								color_name_o2 = o2sttResult?.color_name,
								color_code_o2 = o2sttResult?.color_code,
								ion_lastest_period = icResult != null ? $"{icResult.period}/{icResult.year}" : null,
								ion_value = icResult?.ion_count,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-sea-water-view-in-micro-bact")]
		public async Task<ActionResult<IEnumerable<CMInfoSeaWaterMicroBacteria>>> GetCMInfoSeaWaterMicroBacteria()
		{
			try
			{
				var recentSrb = await _context.CMMicroBacteriaSRB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentAtp = await _context.CMMicroBacteriaATP
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentGhb = await _context.CMMicroBacteriaGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentApghb = await _context.CMMicroBacteriaAPGHB
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var recentSulphide = await _context.CMMicroBacteriaSulphide
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.year)
								  .ThenByDescending(wa => wa.period)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 3 && inf.is_micro_bacteria == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var status = await _context.MdCMMicroBacteriaStatus.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join srb in recentSrb on inf.id equals srb?.id_tag into infsrb
							from srbResult in infsrb.DefaultIfEmpty()
							join srbsttResult in status on srbResult?.id_status equals srbsttResult.id into srbstt
							from srbsttResult in srbstt.DefaultIfEmpty()
							join atp in recentAtp on inf.id equals atp?.id_tag into infatp
							from atpResult in infatp.DefaultIfEmpty()
							join atpsttResult in status on atpResult?.id_status equals atpsttResult.id into atpstt
							from atpsttResult in atpstt.DefaultIfEmpty()
							join ghb in recentGhb on inf.id equals ghb?.id_tag into infghb
							from ghbResult in infghb.DefaultIfEmpty()
							join ghbsttResult in status on ghbResult?.id_status equals ghbsttResult.id into ghbstt
							from ghbsttResult in ghbstt.DefaultIfEmpty()
							join apghb in recentApghb on inf.id equals apghb?.id_tag into infapghb
							from apghbResult in infapghb.DefaultIfEmpty()
							join apghbsttResult in status on apghbResult?.id_status equals apghbsttResult.id into apghbstt
							from apghbsttResult in apghbstt.DefaultIfEmpty()
							join sulphide in recentSulphide on inf.id equals sulphide?.id_tag into infsulphide
							from sulphideResult in infsulphide.DefaultIfEmpty()
							join sulphidesttResult in status on sulphideResult?.id_status equals sulphidesttResult.id into sulphidestt
							from sulphidesttResult in sulphidestt.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoSeaWaterMicroBacteria
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								srb_lastest_period = srbResult != null ? $"{srbResult.period}/{srbResult.year}" : null,
								srb_value = srbResult?.srb_val,
								srb_severity_level = srbsttResult?.severity_level,
								srb_color_name = srbsttResult?.color_name,
								srb_color_code = srbsttResult?.color_code,
								atp_lastest_period = atpResult != null ? $"{atpResult.period}/{atpResult.year}" : null,
								atp_value = atpResult?.atp_val,
								atp_severity_level = atpsttResult?.severity_level,
								atp_color_name = atpsttResult?.color_name,
								atp_color_code = atpsttResult?.color_code,
								ghb_lastest_period = ghbResult != null ? $"{ghbResult.period}/{ghbResult.year}" : null,
								ghb_value = ghbResult?.ghb_val,
								ghb_severity_level = ghbsttResult?.severity_level,
								ghb_color_name = ghbsttResult?.color_name,
								ghb_color_code = ghbsttResult?.color_code,
								apghb_lastest_period = apghbResult != null ? $"{apghbResult.period}/{apghbResult.year}" : null,
								apghb_value = apghbResult?.apghb_val,
								apghb_severity_level = apghbsttResult?.severity_level,
								apghb_color_name = apghbsttResult?.color_name,
								apghb_color_code = apghbsttResult?.color_code,
								sulphide_lastest_period = sulphideResult != null ? $"{sulphideResult.period}/{sulphideResult.year}" : null,
								sulphide_value = sulphideResult?.sulphide_val,
								sulphide_severity_level = sulphidesttResult?.severity_level,
								sulphide_color_name = sulphidesttResult?.color_name,
								sulphide_color_code = sulphidesttResult?.color_code,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-sea-water-view-in-corrosion-coupon")]
		public async Task<ActionResult<IEnumerable<CMInfoSeaWaterCorrosionCoupon>>> GetCMInfoSeaWaterCorrosionCoupon()
		{
			try
			{
				var recentCc = await _context.CMCorrosionCouponMonitorRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.remove_date)
								  .FirstOrDefault())
					.ToListAsync();

				var monitorDetails = await _context.CMCorrosionCouponMonitorDetail.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 3 && inf.is_corrosion_coupon == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				// Join data in-memory
				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join cc in recentCc on inf.id equals cc?.id_tag into infcc
							from ccResult in infcc.DefaultIfEmpty()
							join ccd in monitorDetails on ccResult?.id equals ccd.id_record into ccccd
							from ccccdResult in ccccd.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoSeaWaterCorrosionCoupon
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								remove_lastest_date = ccResult?.remove_date,
								corrosion_rate = ccccdResult?.corrosion_rate,
								max_pit_depth = ccccdResult?.max_pit_depth,
								pitting_rate = ccccdResult?.pitting_rate,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("get-tag-sea-water-view-in-er-probe")]
		public async Task<ActionResult<IEnumerable<CMInfoSeaWaterERProbe>>> GetCMInfoSeaWaterERProbe()
		{
			try
			{
				var recentEr = await _context.CMERProbeRecord
					.GroupBy(wa => wa.id_tag)
					.Select(g => g.OrderByDescending(wa => wa.record_date)
								  .FirstOrDefault())
					.ToListAsync();

				var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 3 && inf.is_er_probe == true)
					.ToListAsync();

				var platforms = await _context.MdPlatform.ToListAsync();

				var data = (from inf in cmInfos
							join pf in platforms on inf.id_platform equals pf.id into infpf
							from infpfResult in infpf.DefaultIfEmpty()
							join er in recentEr on inf.id equals er.id_tag into infer
							from erResult in infer.DefaultIfEmpty()
							orderby inf.tag_no
							select new CMInfoSeaWaterERProbe
							{
								id_tag = inf.id,
								id_platform = infpfResult.id,
								platform = infpfResult?.code_name,
								tag_no = inf.tag_no,
								desc = inf.desc,
								record_lastest_date = erResult?.record_date,
								metal_loss = erResult?.metal_loss,
								corrosion_rate = erResult?.corrosion_rate,
								note = erResult?.note,
							}).ToList();

				if (!data.Any())
				{
					return NotFound("CMInfo Not found.");
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		#endregion

		#region Dashboard
		[HttpGet]
		[Route("get-pipeline-cm-dashboard")]
		public async Task<ActionResult<IEnumerable<PipelineDashboard>>> GetPipelineDashboard()
		{
			var cmInfos = await _context.CMInfo
					.Where(inf => inf.is_active == true && inf.id_system == 4)
					.ToListAsync();

			var idTags = cmInfos.Select(info => info.id).ToList();

			// Fetch the latest records grouped by id_tag (period and year)
			async Task<List<dynamic>> FetchLatestRecords<T>(IQueryable<T> query) where T : class
			{
				var records = await query.ToListAsync();

				return records
						.Where(e =>
						{
							var idTag = e.GetType().GetProperty("id_tag")?.GetValue(e) as int?;
							return idTag.HasValue && idTags.Contains(idTag.Value);
						})
						.GroupBy(wa => wa.GetType().GetProperty("id_tag")?.GetValue(wa))
						.Select(g => g.OrderByDescending(wa => wa.GetType().GetProperty("year")?.GetValue(wa))
									  .ThenByDescending(wa => wa.GetType().GetProperty("period")?.GetValue(wa))
									  .FirstOrDefault())
						.Cast<dynamic>()
						.ToList();
			}

			// Fetch the latest records grouped by id_tag (date)
			async Task<List<dynamic>> FetchLatestRecordsII<T>(IQueryable<T> query) where T : class
			{
				var records = await query.ToListAsync();

				return records
						.Where(e =>
						{
							var idTag = e.GetType().GetProperty("id_tag")?.GetValue(e) as int?;
							return idTag.HasValue && idTags.Contains(idTag.Value);
						})
						.GroupBy(wa => wa.GetType().GetProperty("id_tag")?.GetValue(wa))
						.Select(g => g.OrderByDescending(wa => wa.GetType().GetProperty("record_date")?.GetValue(wa))
									  .FirstOrDefault())
						.Cast<dynamic>()
						.ToList();
			}

			var worstWAStatusByTag = new Dictionary<int, int>();
			var worstMBStatusByTag = new Dictionary<int, int>();
			//var worstCCStatusByTag = new Dictionary<int, int>();
			var worstERStatusByTag = new Dictionary<int, int>();
			var worstCIStatusByTag = new Dictionary<int, int>();

			// Function to update the worst status for each id_tag
			void UpdateWorstStatus(List<dynamic> records, Dictionary<int, int> statusDictionary)
			{
				foreach (var record in records)
				{
					if (record != null)
					{
						int idTag = (int)record.id_tag;
						int status = (int)record.id_status;
						if (statusDictionary.ContainsKey(idTag))
						{
							if (status < statusDictionary[idTag])
							{
								statusDictionary[idTag] = status;
							}
						}
						else
						{
							statusDictionary[idTag] = status;
						}
					}
				}
			}

			// Fetch and update statuses
			var recentPh = await FetchLatestRecords(_context.CMWaterAnalysisPH);
			var recentDissolvedO2 = await FetchLatestRecords(_context.CMWaterAnalysisDissolvedO2);
			var recentIonCount = await FetchLatestRecords(_context.CMWaterAnalysisIonCount);
			UpdateWorstStatus(recentPh, worstWAStatusByTag);
			UpdateWorstStatus(recentDissolvedO2, worstWAStatusByTag);
			UpdateWorstStatus(recentIonCount, worstWAStatusByTag);

			var recentSrb = await FetchLatestRecords(_context.CMMicroBacteriaSRB);
			var recentAtp = await FetchLatestRecords(_context.CMMicroBacteriaATP);
			var recentGhb = await FetchLatestRecords(_context.CMMicroBacteriaGHB);
			var recentApghb = await FetchLatestRecords(_context.CMMicroBacteriaAPGHB);
			var recentSulphide = await FetchLatestRecords(_context.CMMicroBacteriaSulphide);
			UpdateWorstStatus(recentSrb, worstMBStatusByTag);
			UpdateWorstStatus(recentAtp, worstMBStatusByTag);
			UpdateWorstStatus(recentGhb, worstMBStatusByTag);
			UpdateWorstStatus(recentApghb, worstMBStatusByTag);
			UpdateWorstStatus(recentSulphide, worstMBStatusByTag);

			// var recentCc = await _context.CMCorrosionCouponMonitorRecord
			// 		.GroupBy(wa => wa.id_tag)
			// 		.Select(g => g.OrderByDescending(wa => wa.remove_date)
			// 					  .FirstOrDefault())
			// 		.ToListAsync();

			var recentERProbe = await FetchLatestRecordsII(_context.CMERProbeRecord);
			UpdateWorstStatus(recentERProbe, worstERStatusByTag);

			var recentChemInjection = await FetchLatestRecordsII(_context.CMChemInjectionRecord);
			UpdateWorstStatus(recentChemInjection, worstCIStatusByTag);

			// Fetch additional status info
			var CCStatus = await _context.MdCMCorrosionCouponStatus.ToListAsync();
			var WAStatus = await _context.MdCMWaterAnalysisStatus.ToListAsync();
			var ERStatus = await _context.MdCMERProbeStatus.ToListAsync();
			var CIStatus = await _context.MdCMChemInjectionStatus.ToListAsync();
			var MBStatus = await _context.MdCMMicroBacteriaStatus.ToListAsync();
			var pf = await _context.MdPlatform.ToListAsync();

			List<PipelineDashboard> result = new List<PipelineDashboard>();

			// Construct the result
			foreach (CMInfo inf in cmInfos)
			{
				var data = new PipelineDashboard
				{
					id_tag = inf.id,
					id_platform = inf.id_platform,
					platform = pf.FirstOrDefault(a => a.id == inf.id_platform)?.code_name ?? null,
					tag_no = inf.tag_no,
					id_status_piging = null,
					severity_level_piging = null,
					color_name_piging = null,
					color_code_piging = null,
					id_status_wa = worstWAStatusByTag.TryGetValue(inf.id, out var id_wa) ? id_wa : null,
					severity_level_wa = null,
					color_name_wa = null,
					color_code_wa = null,
					id_status_mb = worstMBStatusByTag.TryGetValue(inf.id, out var id_mb) ? id_mb : null,
					severity_level_mb = null,
					color_name_mb = null,
					color_code_mb = null,
					id_status_cc = null,
					severity_level_cc = null,
					color_name_cc = null,
					color_code_cc = null,
					id_status_er = worstERStatusByTag.TryGetValue(inf.id, out var id_er) ? id_er : null,
					severity_level_er = null,
					color_name_er = null,
					color_code_er = null,
					id_status_ci = worstCIStatusByTag.TryGetValue(inf.id, out var id_ci) ? id_ci : null,
					severity_level_ci = null,
					color_name_ci = null,
					color_code_ci = null
				};

				var waStatus = WAStatus.FirstOrDefault(a => a.id == data.id_status_wa);
				if (waStatus != null)
				{
					data.severity_level_wa = waStatus.severity_level;
					data.color_name_wa = waStatus.color_name;
					data.color_code_wa = waStatus.color_code;
				}

				var erStatus = ERStatus.FirstOrDefault(a => a.id == data.id_status_er);
				if (erStatus != null)
				{
					data.severity_level_er = erStatus.severity_level;
					data.color_name_er = erStatus.color_name;
					data.color_code_er = erStatus.color_code;
				}

				var mbStatus = MBStatus.FirstOrDefault(a => a.id == data.id_status_mb);
				if (mbStatus != null)
				{
					data.severity_level_mb = mbStatus.severity_level;
					data.color_name_mb = mbStatus.color_name;
					data.color_code_mb = mbStatus.color_code;
				}

				var ciStatus = CIStatus.FirstOrDefault(a => a.id == data.id_status_ci);
				if (ciStatus != null)
				{
					data.severity_level_ci = ciStatus.severity_level;
					data.color_name_ci = ciStatus.color_name;
					data.color_code_ci = ciStatus.color_code;
				}

				result.Add(data);
			}

			return Ok(result);
		}

		#endregion

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCMInfo(int id, CMInfo data)
		{
			if (id != data.id)
			{
				return BadRequest("ID mismatch between URL and body.");
			}

			try
			{
				_context.Entry(data).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMInfoExists(id))
				{
					return NotFound($"CMInfo with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<CMInfo>> PostCMInfo(CMInfo data)
		{
			_context.CMInfo.Add(data);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCMInfoById", new { id = data.id }, data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCMInfo(int id)
		{
			var data = await _context.CMInfo.FindAsync(id);

			if (data == null)
			{
				return NotFound();
			}

			data.is_active = false;
			_context.Entry(data).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CMInfoExists(id))
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

		private bool CMInfoExists(int id)
		{
			return _context.CMInfo.Any(e => e.id == id);
		}

		private IDictionary<string, List<MonthlyPassPercentage>> CalculatePassPercentages(List<CMInfo> cmInfos, List<CMChemInjectionRecord> chemInjectionRecords, int year)
		{
			var result = new Dictionary<string, List<MonthlyPassPercentage>>();

			DateTime currentDate = DateTime.Now;

			foreach (var cmInfo in cmInfos)
			{
				if (!result.ContainsKey(cmInfo.tag_no))
				{
					result[cmInfo.tag_no] = new List<MonthlyPassPercentage>();
				}

				var passCountsByMonth = new Dictionary<int, int>();

				var records = chemInjectionRecords.Where(r => r.id_tag == cmInfo.id && r.record_date.HasValue && r.record_date.Value.Year == year);

				foreach (var record in records)
				{
					int month = record.record_date.Value.Month;

					if (!passCountsByMonth.ContainsKey(month))
					{
						passCountsByMonth[month] = 0;
					}

					if (record.id_status == 2)
					{
						passCountsByMonth[month]++;
					}
				}

				for (int month = 1; month <= 12; month++)
				{
					int daysInMonth = DateTime.DaysInMonth(year, month);
					if (year == currentDate.Year && month == currentDate.Month)
					{
						daysInMonth = currentDate.Day;
					}

					int passCount = passCountsByMonth.ContainsKey(month) ? passCountsByMonth[month] : 0;
					decimal passPercentage = (decimal)passCount / daysInMonth * 100;

					var monthlyPercentage = new MonthlyPassPercentage
					{
						Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month),
						Percentage = passPercentage
					};

					result[cmInfo.tag_no].Add(monthlyPercentage);
				}
			}

			return result;
		}
	}
}