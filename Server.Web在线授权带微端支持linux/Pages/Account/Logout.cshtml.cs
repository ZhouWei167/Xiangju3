using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.Auth;

namespace Server.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(AuthService authService, ILogger<LogoutModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("用户已登出");
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("用户已登出");
            return RedirectToPage("/Index");
        }
    }
} 