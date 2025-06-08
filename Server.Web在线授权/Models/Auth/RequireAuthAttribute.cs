using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Web.Models.Auth
{
    /// <summary>
    /// 要求用户已登录的特性
    /// </summary>
    public class RequireAuthAttribute : TypeFilterAttribute
    {
        public RequireAuthAttribute() : base(typeof(RequireAuthFilter))
        {
        }
    }

    /// <summary>
    /// 要求用户有生成授权文件权限的特性
    /// </summary>
    public class RequireGenerateLicenseAttribute : TypeFilterAttribute
    {
        public RequireGenerateLicenseAttribute() : base(typeof(RequireGenerateLicenseFilter))
        {
        }
    }

    /// <summary>
    /// 要求用户有查看授权记录权限的特性
    /// </summary>
    public class RequireViewRecordsAttribute : TypeFilterAttribute
    {
        public RequireViewRecordsAttribute() : base(typeof(RequireViewRecordsFilter))
        {
        }
    }

    /// <summary>
    /// 要求用户有管理用户权限的特性
    /// </summary>
    public class RequireManageUsersAttribute : TypeFilterAttribute
    {
        public RequireManageUsersAttribute() : base(typeof(RequireManageUsersFilter))
        {
        }
    }

    /// <summary>
    /// 要求用户是超级管理员的特性
    /// </summary>
    public class RequireSuperAdminAttribute : TypeFilterAttribute
    {
        public RequireSuperAdminAttribute() : base(typeof(RequireSuperAdminFilter))
        {
        }
    }

    /// <summary>
    /// 验证用户是否已登录的过滤器
    /// </summary>
    public class RequireAuthFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public RequireAuthFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }

    /// <summary>
    /// 验证用户是否有查看授权记录权限的过滤器
    /// </summary>
    public class RequireViewRecordsFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public RequireViewRecordsFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (!_authService.CanCurrentUserViewRecords())
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
            }
        }
    }

    /// <summary>
    /// 验证用户是否有生成授权文件权限的过滤器
    /// </summary>
    public class RequireGenerateLicenseFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public RequireGenerateLicenseFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (!_authService.CanCurrentUserGenerateLicense())
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
            }
        }
    }

    /// <summary>
    /// 验证用户是否有管理用户权限的过滤器
    /// </summary>
    public class RequireManageUsersFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public RequireManageUsersFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (!_authService.CanCurrentUserManageUsers())
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
            }
        }
    }

    /// <summary>
    /// 验证用户是否是超级管理员的过滤器
    /// </summary>
    public class RequireSuperAdminFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public RequireSuperAdminFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (!_authService.IsCurrentUserSuperAdmin())
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
            }
        }
    }
} 