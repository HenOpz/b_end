using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPOC_AIMS_II_Backend.Models
{
	public class MainDbContext : DbContext
	{
		public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
		{

		}

		#region DbSet
		public DbSet<CMRCIRecord> CMRCIRecord { get; set; }
		public DbSet<RCFAFile> RCFAFile { get; set; }
		public DbSet<RCFAActionRecord> RCFAActionRecord { get; set; }
		public DbSet<CMInfo> CMInfo { get; set; }
		public DbSet<CMWaterAnalysisPH> CMWaterAnalysisPH { get; set; }
		public DbSet<CMWaterAnalysisDissolvedO2> CMWaterAnalysisDissolvedO2 { get; set; }
		public DbSet<CMWaterAnalysisIonCount> CMWaterAnalysisIonCount { get; set; }
		public DbSet<CMWaterAnalysisLibrary> CMWaterAnalysisLibrary { get; set; }
		public DbSet<CMCorrosionCouponMonitorRecord> CMCorrosionCouponMonitorRecord { get; set; }
		public DbSet<CMCorrosionCouponMonitorDetail> CMCorrosionCouponMonitorDetail { get; set; }
		public DbSet<CMCorrosionCouponLibrary> CMCorrosionCouponLibrary { get; set; }
		public DbSet<CMCorrosionCouponPictureLog> CMCorrosionCouponPictureLog { get; set; }
		public DbSet<CMERProbeRecord> CMERProbeRecord { get; set; }
		public DbSet<CMERProbeLibrary> CMERProbeLibrary { get; set; }
		public DbSet<CMChemInjectionRecord> CMChemInjectionRecord { get; set; }
		public DbSet<CMChemInjectionLibrary> CMChemInjectionLibrary { get; set; }
		public DbSet<CMMicroBacteriaATP> CMMicroBacteriaATP { get; set; }
		public DbSet<CMMicroBacteriaGHB> CMMicroBacteriaGHB { get; set; }
		public DbSet<CMMicroBacteriaAPGHB> CMMicroBacteriaAPGHB { get; set; }
		public DbSet<CMMicroBacteriaSulphide> CMMicroBacteriaSulphide { get; set; }
		public DbSet<CMMicroBacteriaSRB> CMMicroBacteriaSRB { get; set; }
		public DbSet<CMMicroBacterialLibrary> CMMicroBacterialLibrary { get; set; }
		public DbSet<TestHangFire> TestHangFire { get; set; }
		public DbSet<ExInspectionPictureLog> ExInspectionPictureLog { get; set; }
		public DbSet<ExInspectionRegisterInfoFile> ExInspectionRegisterInfoFile { get; set; }
		public DbSet<ExInspectionRecord> ExInspectionRecord { get; set; }
		public DbSet<ExInspectionChecklistResult> ExInspectionChecklistResult { get; set; }
		public DbSet<ExInspectionChecklist> ExInspectionChecklist { get; set; }
		public DbSet<RectificationCampaignFile> RectificationCampaignFile { get; set; }
		public DbSet<InspectionCampaignFile> InspectionCampaignFile { get; set; }
		public DbSet<ExInspectionRegisterInfo> ExInspectionRegisterInfo { get; set; }
		public DbSet<FailureRecordTXN> FailureRecordTXN { get; set; }
		public DbSet<FailureRecordAuth> FailureRecordAuth { get; set; }
		public DbSet<FailureRecord> FailureRecord { get; set; }
		public DbSet<FailureFile> FailureFile { get; set; }
		public DbSet<FailureActionRecord> FailureActionRecord { get; set; }
		public DbSet<RectificationCampaign> RectificationCampaign { get; set; }
		public DbSet<InspectionCampaign> InspectionCampaign { get; set; }
		public DbSet<ManagementOfChange> ManagementOfChange { get; set; }
		public DbSet<InspectionTaskFile> InspectionTaskFile { get; set; }
		public DbSet<InspectionTask> InspectionTask { get; set; }
		public DbSet<HighlightActivities> HighlightActivities { get; set; }
		public DbSet<CMWOManagement> CMWOManagement { get; set; }
		public DbSet<UserInRole> UserInRole { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<UserAccount> UserAccount { get; set; }
		public DbSet<UserInfo> UserInfo { get; set; }
		public DbSet<UserInMenu> UserInMenu { get; set; }
		public DbSet<SapHeader> SapHeader { get; set; }
		public DbSet<SapHeaderTXN> SapHeaderTXN { get; set; }
		public DbSet<GpiRecord> GpiRecord { get; set; }
		public DbSet<GpiFile> GpiFile { get; set; }
		public DbSet<GpiRecordTXN> GpiRecordTXN { get; set; }
		public DbSet<GpiRecordAuth> GpiRecordAuth { get; set; }
		public DbSet<MdUserRole> MdUserRole { get; set; }
		public DbSet<MdPrefix> MdPrefix { get; set; }
		public DbSet<MdAssetType> MdAssetType { get; set; }
		public DbSet<MdInspectionCampaignStatus> MdInspectionCampaignStatus { get; set; }
		public DbSet<MdInspectionTaskStatus> MdInspectionTaskStatus { get; set; }
		public DbSet<MdInspectionType> MdInspectionType { get; set; }
		public DbSet<MdIntegrityStatus> MdIntegrityStatus { get; set; }
		public DbSet<MdMocNatureOfChange> MdMocNatureOfChange { get; set; }
		public DbSet<MdMocResidualRiskLevel> MdMocResidualRiskLevel { get; set; }
		public DbSet<MdMocStatus> MdMocStatus { get; set; }
		public DbSet<MdPlatform> MdPlatform { get; set; }
		public DbSet<MdRectificationStatus> MdRectificationStatus { get; set; }
		public DbSet<MdRiskRanking> MdRiskRanking { get; set; }
		public DbSet<MdFailureImpact> MdFailureImpact { get; set; }
		public DbSet<MdFailureActionStatus> MdFailureActionStatus { get; set; }
		public DbSet<MdFailureDiscipline> MdFailureDiscipline { get; set; }
		public DbSet<MdFailureApprovalStatus> MdFailureApprovalStatus { get; set; }
		public DbSet<MdTransactionStatus> MdTransactionStatus { get; set; }
		public DbSet<MdModules> MdModules { get; set; }
		public DbSet<MdSapAccessibility> MdSapAccessibility { get; set; }
		public DbSet<MdSapCauseCode> MdSapCauseCode { get; set; }
		public DbSet<MdSapCodeGrpCause> MdSapCodeGrpCause { get; set; }
		public DbSet<MdSapCodeGrpDamage> MdSapCodeGrpDamage { get; set; }
		public DbSet<MdSapCodeGrpObjectPart> MdSapCodeGrpObjectPart { get; set; }
		public DbSet<MdSapObjectPartCode> MdSapObjectPartCode { get; set; }
		public DbSet<MdSapDamageCode> MdSapDamageCode { get; set; }
		public DbSet<MdSapFunctionalLocation> MdSapFunctionalLocation { get; set; }
		public DbSet<MdSapMainWorkCtr> MdSapMainWorkCtr { get; set; }
		public DbSet<MdSapPlannerGrp> MdSapPlannerGrp { get; set; }
		public DbSet<MdSapPlannerGrpPlanningPlant> MdSapPlannerGrpPlanningPlant { get; set; }
		public DbSet<MdSapScaffolding> MdSapScaffolding { get; set; }
		public DbSet<MdExInspectionChecklistStatus> MdExInspectionChecklistStatus { get; set; }
		public DbSet<MdExInspectionPictureLogStatus> MdExInspectionPictureLogStatus { get; set; }
		public DbSet<MdAppModule> MdAppModule { get; set; }
		public DbSet<MdMenu> MdMenu { get; set; }
		public DbSet<MdExInspectionAreaStandard> MdExInspectionAreaStandard { get; set; }
		public DbSet<MdExInspectionAreaClass> MdExInspectionAreaClass { get; set; }
		public DbSet<MdExInspectionAreaTempClass> MdExInspectionAreaTempClass { get; set; }
		public DbSet<MdExInspectionAreaGasGroup> MdExInspectionAreaGasGroup { get; set; }
		public DbSet<MdExInspectionEquipStandard> MdExInspectionEquipStandard { get; set; }
		public DbSet<MdExInspectionEquipProtectionLevelCategory> MdExInspectionEquipProtectionLevelCategory { get; set; }
		public DbSet<MdExInspectionEquipProtectionType> MdExInspectionEquipProtectionType { get; set; }
		public DbSet<MdExInspectionEquipClass> MdExInspectionEquipClass { get; set; }
		public DbSet<MdExInspectionEquipTempClass> MdExInspectionEquipTempClass { get; set; }
		public DbSet<MdExInspectionEquipGasGroup> MdExInspectionEquipGasGroup { get; set; }
		public DbSet<MdExInspectionEquipIpRating> MdExInspectionEquipIpRating { get; set; }
		public DbSet<MdExInspectionEquipEnclosureType> MdExInspectionEquipEnclosureType { get; set; }
		public DbSet<MdExInspectionEquipType> MdExInspectionEquipType { get; set; }
		public DbSet<MdExInspectionEnvStatus> MdExInspectionEnvStatus { get; set; }
		public DbSet<MdExInspectionDiscipline> MdExInspectionDiscipline { get; set; }
		public DbSet<MdExInspectionGasGroup> MdExInspectionGasGroup { get; set; }
		public DbSet<MdExInspectionLocation> MdExInspectionLocation { get; set; }
		public DbSet<MdExInspectionZone> MdExInspectionZone { get; set; }
		public DbSet<MdGpiRepair> MdGpiRepair { get; set; }
		public DbSet<MdGpiRepairType> MdGpiRepairType { get; set; }
		public DbSet<MdGpiMainComponent> MdGpiMainComponent { get; set; }
		public DbSet<MdGpiDamageMechanism> MdGpiDamageMechanism { get; set; }
		public DbSet<MdGpiSeverityStatus> MdGpiSeverityStatus { get; set; }
		public DbSet<MdGpiLocationDeck> MdGpiLocationDeck { get; set; }
		public DbSet<MdGpiDiscipline> MdGpiDiscipline { get; set; }
		public DbSet<MdGpiApprovalStatus> MdGpiApprovalStatus { get; set; }
		public DbSet<MdWorkGroup> MdWorkGroup { get; set; }
		public DbSet<MdCMMicroBacteriaStatus> MdCMMicroBacteriaStatus { get; set; }
		public DbSet<MdCMChemInjectionStatus> MdCMChemInjectionStatus { get; set; }
		public DbSet<MdFailurePOF> MdFailurePOF { get; set; }
		public DbSet<MdFailureCOF> MdFailureCOF { get; set; }
		public DbSet<MdFailureRiskMatrix> MdFailureRiskMatrix { get; set; }
		public DbSet<MdMonth> MdMonth { get; set; }
		public DbSet<MdCMCorrosionCouponStatus> MdCMCorrosionCouponStatus { get; set; }
		public DbSet<MdCMERProbeStatus> MdCMERProbeStatus { get; set; }
		public DbSet<MdCMWaterAnalysisStatus> MdCMWaterAnalysisStatus { get; set; }
		public DbSet<MdGpiRecordStatus> MdGpiRecordStatus { get; set; }
		public DbSet<MdFailureAuthRole> MdFailureAuthRole { get; set; }


		#endregion
	}

	#region Master Data Model
	public class MdExInspectionPictureLogStatus
	{
		[Key]
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdUserRole
	{
		[Key]
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdPrefix
	{
		[Key]
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdAssetType
	{
		public int id { get; set; }
		public string? asset_type { get; set; }
		public string? icon { get; set; }
		public int? sort { get; set; }
	}
	public class MdInspectionCampaignStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdInspectionTaskStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdInspectionType
	{
		public int id { get; set; }
		public int id_asset { get; set; }
		public string? insp_type { get; set; }
		public int? sort { get; set; }
	}
	public class MdIntegrityStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdMocNatureOfChange
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdMocResidualRiskLevel
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdMocStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdPlatform
	{
		public int id { get; set; }
		public string? code_name { get; set; }
		public string? full_name { get; set; }
		public int? phase { get; set; }
		public string? planning_plant { get; set; }
	}
	public class MdRectificationStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdRiskRanking
	{
		public int id { get; set; }
		public string? risk_ranking { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdFailureImpact
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdFailureActionStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdFailureDiscipline
	{
		public int id { get; set; }
		public string? discipline { get; set; }
	}
	public class MdFailureApprovalStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
	}
	public class MdTransactionStatus
	{
		[Key]
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdModules
	{
		public int id { get; set; }
		public string? module_name { get; set; }
		public string? module_abbr { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapAccessibility
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapCauseCode
	{
		public int id { get; set; }
		public string? cause_code { get; set; }
		public string? cause_text { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapCodeGrpCause
	{
		public int id { get; set; }
		public string? cause_code { get; set; }
		public string? cause_text { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapCodeGrpDamage
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapCodeGrpObjectPart
	{
		public int id { get; set; }
		public string? object_part_code { get; set; }
		public string? object_part_text { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapObjectPartCode
	{
		public int id { get; set; }
		public string? object_part_code { get; set; }
		public string? object_part_text { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapDamageCode
	{
		public int id { get; set; }
		public string? damage_code { get; set; }
		public string? damage_code_text { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapFunctionalLocation
	{
		public int id { get; set; }
		public string? functional_location { get; set; }
		public string? description { get; set; }
		public int? planning_plant { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapMainWorkCtr
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public string? planner_grp { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapPlannerGrp
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapPlannerGrpPlanningPlant
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public int? sort { get; set; }
	}
	public class MdSapScaffolding
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? description { get; set; }
		public int? sort { get; set; }
	}
	public class MdExInspectionChecklistStatus
	{
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdAppModule
	{
		public int id { get; set; }
		public required string name { get; set; }
		public string? icon { get; set; }
		public decimal? icon_size { get; set; }
		public string? route { get; set; }
		public int seq { get; set; }
		public required string section { get; set; }
		public bool is_active { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}
	public class MdMenu
	{
		public int id { get; set; }
		[ForeignKey("MdAppModule")]
		public int id_app_module { get; set; }
		public required string name { get; set; }
		public string? icon { get; set; }
		public decimal? icon_size { get; set; }
		public string? route { get; set; }
		public int seq { get; set; }
		public bool is_active { get; set; }
		public DateTime? updated_date { get; set; }
		public int? updated_by { get; set; }
	}
	//Ex-Inspection
	public class MdExInspectionAreaStandard
	{
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionAreaClass
	{
		public int id { get; set; }
		public int? id_area_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionAreaTempClass
	{
		public int id { get; set; }
		public int? id_area_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionAreaGasGroup
	{
		public int id { get; set; }
		public int? id_area_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipStandard
	{
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipProtectionLevelCategory
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipProtectionType
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipClass
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipTempClass
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
		public string? desc { get; set; }
	}
	public class MdExInspectionEquipGasGroup
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipIpRating
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipEnclosureType
	{
		public int id { get; set; }
		public int? id_equip_standard { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEquipType
	{
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionEnvStatus
	{
		public int id { get; set; }
		public string? code { get; set; }
	}
	public class MdExInspectionDiscipline
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdExInspectionGasGroup
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdExInspectionLocation
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdExInspectionZone
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiRepair
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiRepairType
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiMainComponent
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiDamageMechanism
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiSeverityStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
		public string? color_code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiLocationDeck
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdGpiDiscipline
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? email { get; set; }
		public int sort { get; set; }
	}
	public class MdGpiApprovalStatus
	{
		public int id { get; set; }
		public string? status { get; set; }
	}
	public class MdWorkGroup
	{
		public int id { get; set; }
		public string? code { get; set; }
		public int? sort { get; set; }
	}
	public class MdCMMicroBacteriaStatus
	{
		public int id { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdCMChemInjectionStatus
	{
		public int id { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdFailurePOF
	{
		public int id { get; set; }
		public string? code { get; set; }
		public string? category { get; set; }
		public string? desc { get; set; }
	}
	public class MdFailureCOF
	{
		public int id { get; set; }
		public int? code { get; set; }
		public string? category { get; set; }
		public string? people_desc { get; set; }
		public string? environment_desc { get; set; }
		public string? production_loss_desc { get; set; }
		public string? reputation_desc { get; set; }
	}
	public class MdFailureRiskMatrix
	{
		public int id { get; set; }
		public int id_pof { get; set; }
		public int id_cof { get; set; }
		public string? risk_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdMonth
	{
		public int id { get; set; }
		public int month_no { get; set; }
		public string? month_code { get; set; }
	}
	public class MdCMCorrosionCouponStatus
	{
		public int id { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdCMERProbeStatus
	{
		public int id { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdCMWaterAnalysisStatus
	{
		public int id { get; set; }
		public string? severity_level { get; set; }
		public string? color_name { get; set; }
		public string? color_code { get; set; }
		public int? sort_no { get; set; }
	}
	public class MdGpiRecordStatus
    {
        [Key]
        public int id { get; set; }
        public string? code { get; set; }
    }
		public class MdFailureAuthRole
	{
		public int id { get; set; }
		public int id_work_group { get; set; }
		public string? role_name { get; set; }
	}
	#endregion
}