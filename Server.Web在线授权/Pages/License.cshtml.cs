using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.Auth;

namespace Server.Web.Pages
{
    [RequireGenerateLicense]
    public class LicenseModel : PageModel
    {
        private readonly ILogger<LicenseModel> _logger;
        private readonly AuthService _authService;

        public bool IsAdmin { get; private set; }

        public LicenseModel(ILogger<LicenseModel> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public void OnGet()
        {
            _logger.LogInformation("访问授权文件生成页面");
            IsAdmin = _authService.IsCurrentUserAdmin();
        }
    }
} 