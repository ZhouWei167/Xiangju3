using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.License;
using Server.Web.Models.Auth;

namespace Server.Web.Pages
{
    [RequireViewRecords]
    public class LicenseRecordsModel : PageModel
    {
        private readonly ILogger<LicenseRecordsModel> _logger;
        private readonly LicenseRecordService _licenseRecordService;

        public LicenseRecordsModel(ILogger<LicenseRecordsModel> logger, LicenseRecordService licenseRecordService)
        {
            _logger = logger;
            _licenseRecordService = licenseRecordService;
        }

        public void OnGet()
        {
            _logger.LogInformation("访问授权记录管理页面");
        }
    }
} 