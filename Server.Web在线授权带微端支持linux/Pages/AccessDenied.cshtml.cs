using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Web.Pages
{
    public class AccessDeniedModel : PageModel
    {
        private readonly ILogger<AccessDeniedModel> _logger;

        public AccessDeniedModel(ILogger<AccessDeniedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogWarning($"用户访问被拒绝: {User?.Identity?.Name ?? "未登录用户"}, 路径: {HttpContext.Request.Path}");
        }
    }
} 