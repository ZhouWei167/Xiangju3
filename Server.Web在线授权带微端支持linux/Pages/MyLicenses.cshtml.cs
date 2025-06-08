using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.Auth;

namespace Server.Web.Pages
{
    [RequireAuth]
    public class MyLicensesModel : PageModel
    {
        private readonly ILogger<MyLicensesModel> _logger;
        private readonly AuthService _authService;

        public MyLicensesModel(ILogger<MyLicensesModel> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public void OnGet()
        {
            _logger.LogInformation("访问我的授权记录页面");
        }
    }
} 