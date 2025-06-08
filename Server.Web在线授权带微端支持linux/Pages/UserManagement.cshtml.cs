using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.Auth;

namespace Server.Web.Pages
{
    [RequireAuth]
    public class UserManagementModel : PageModel
    {
        private readonly ILogger<UserManagementModel> _logger;
        private readonly AuthService _authService;

        public UserRole CurrentUserRole { get; private set; }

        public UserManagementModel(ILogger<UserManagementModel> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            // 检查当前用户是否有权限管理用户
            if (!_authService.CanCurrentUserManageUsers())
            {
                _logger.LogWarning($"用户 {User.Identity.Name} 尝试访问用户管理页面，但没有权限");
                return RedirectToPage("/AccessDenied");
            }

            // 获取当前用户角色
            var currentUser = _authService.GetCurrentUser();
            CurrentUserRole = currentUser?.Role ?? UserRole.Guest;

            return Page();
        }
    }
} 