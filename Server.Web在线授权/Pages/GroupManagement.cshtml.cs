using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Web.Models.Auth;

namespace Server.Web.Pages
{
    [RequireSuperAdmin]
    public class GroupManagementModel : PageModel
    {
        private readonly ILogger<GroupManagementModel> _logger;
        private readonly AuthService _authService;

        public bool IsSuperAdmin { get; private set; }

        public GroupManagementModel(ILogger<GroupManagementModel> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public void OnGet()
        {
            _logger.LogInformation("访问用户组管理页面");
            IsSuperAdmin = _authService.IsCurrentUserSuperAdmin();

            // 如果不是超级管理员，则会被RequireSuperAdmin属性重定向到拒绝访问页面
        }
    }
} 