using Microsoft.AspNetCore.Mvc;
using Server.Web.Models.Auth;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Server.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AuthService _authService;

        public UserController(ILogger<UserController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetAllUsers(int page = 1, int pageSize = 20)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试获取用户列表，但没有权限");
                    return Forbid();
                }

                int offset = (page - 1) * pageSize;
                DataTable users = _authService.GetAllUsers(pageSize, offset);
                int total = _authService.GetTotalUsersCount();

                // 转换DataTable为JSON友好的格式
                var result = new List<Dictionary<string, object>>();
                foreach (DataRow row in users.Rows)
                {
                    var item = new Dictionary<string, object>();
                    foreach (DataColumn column in users.Columns)
                    {
                        item[column.ColumnName] = row[column] == DBNull.Value ? null : row[column];
                    }
                    result.Add(item);
                }

                return Ok(new
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    users = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户列表失败");
                return BadRequest(new { message = $"获取用户列表失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 更新用户角色
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult UpdateUserRole([FromBody] UpdateUserRoleRequest request)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试更新用户角色，但没有权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0)
                {
                    return BadRequest(new { message = "无效的用户ID" });
                }

                if (!Enum.IsDefined(typeof(UserRole), request.NewRole))
                {
                    return BadRequest(new { message = "无效的角色值" });
                }

                // 获取当前用户
                var currentUser = _authService.GetCurrentUser();
                
                // 检查是否尝试设置为管理员或超级管理员
                if ((request.NewRole == (int)UserRole.Admin || request.NewRole == (int)UserRole.SuperAdmin) && 
                    currentUser.Role != UserRole.SuperAdmin)
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试将用户设置为管理员或超级管理员，但没有超级管理员权限");
                    return BadRequest(new { message = "只有超级管理员可以将用户设置为管理员或超级管理员" });
                }

                bool result = _authService.UpdateUserRole(request.UserId, (UserRole)request.NewRole);
                if (result)
                {
                    return Ok(new { message = "用户角色已更新" });
                }
                else
                {
                    return BadRequest(new { message = "更新用户角色失败，可能是权限不足或用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户角色失败");
                return BadRequest(new { message = $"更新用户角色失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 更新用户状态（启用/禁用）
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult UpdateUserStatus([FromBody] UpdateUserStatusRequest request)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试更新用户状态，但没有权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0)
                {
                    return BadRequest(new { message = "无效的用户ID" });
                }

                bool result = _authService.UpdateUserStatus(request.UserId, request.IsActive);
                if (result)
                {
                    return Ok(new { message = $"用户状态已更新为 {(request.IsActive ? "启用" : "禁用")}" });
                }
                else
                {
                    return BadRequest(new { message = "更新用户状态失败，可能是权限不足或用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户状态失败");
                return BadRequest(new { message = $"更新用户状态失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult ResetUserPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试重置用户密码，但没有权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0)
                {
                    return BadRequest(new { message = "无效的用户ID" });
                }

                if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length < 6)
                {
                    return BadRequest(new { message = "密码长度必须至少为6个字符" });
                }

                bool result = _authService.ResetUserPassword(request.UserId, request.NewPassword);
                if (result)
                {
                    return Ok(new { message = "用户密码已重置" });
                }
                else
                {
                    return BadRequest(new { message = "重置用户密码失败，可能是权限不足或用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置用户密码失败");
                return BadRequest(new { message = $"重置用户密码失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 重置用户的授权文件生成次数
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult ResetUserLicenseCount([FromBody] ResetLicenseCountRequest request)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试重置用户授权次数，但没有权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0)
                {
                    return BadRequest(new { message = "无效的用户ID" });
                }

                bool result = _authService.ResetLicenseGenerationCount(request.UserId);
                if (result)
                {
                    return Ok(new { message = "用户授权文件生成次数已重置为0" });
                }
                else
                {
                    return BadRequest(new { message = "重置用户授权文件生成次数失败，可能是用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置用户授权文件生成次数失败");
                return BadRequest(new { message = $"重置用户授权文件生成次数失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 设置用户的最大授权文件生成次数
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult SetUserMaxLicenses([FromBody] SetMaxLicensesRequest request)
        {
            try
            {
                // 检查权限
                if (!_authService.CanCurrentUserManageUsers())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试设置用户最大授权次数，但没有权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0)
                {
                    return BadRequest(new { message = "无效的用户ID" });
                }

                if (request.MaxLicenses < 0)
                {
                    return BadRequest(new { message = "最大授权次数不能小于0" });
                }

                bool result = _authService.SetMaxLicenseGenerations(request.UserId, request.MaxLicenses);
                if (result)
                {
                    return Ok(new { message = $"用户最大授权文件生成次数已设置为 {request.MaxLicenses}" });
                }
                else
                {
                    return BadRequest(new { message = "设置用户最大授权文件生成次数失败，可能是用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设置用户最大授权文件生成次数失败");
                return BadRequest(new { message = $"设置用户最大授权文件生成次数失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取当前用户的授权文件生成次数信息
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetMyLicenseGenerationInfo()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    return Unauthorized(new { message = "未登录或会话已过期" });
                }
                
                bool isAdmin = currentUser.Role == UserRole.Admin || currentUser.Role == UserRole.SuperAdmin;
                int currentCount = currentUser.LicenseGenerationCount;
                int maxCount = currentUser.MaxLicenseGenerations;
                
                return Ok(new
                {
                    isAdmin = isAdmin,
                    currentCount = currentCount,
                    maxCount = maxCount,
                    remainingCount = maxCount - currentCount,
                    canGenerateMore = isAdmin || currentCount < maxCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取授权生成信息失败");
                return BadRequest(new { message = $"获取授权生成信息失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取所有用户名列表
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetAllUsernames()
        {
            try
            {
                // 检查权限 - 只有管理员才能获取用户名列表
                if (!_authService.IsCurrentUserAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试获取用户名列表，但没有管理员权限");
                    return Forbid();
                }

                var usernames = _authService.GetAllUsernames();
                return Ok(usernames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户名列表失败");
                return BadRequest(new { message = $"获取用户名列表失败: {ex.Message}" });
            }
        }
    }

    public class UpdateUserRoleRequest
    {
        public int UserId { get; set; }
        public int NewRole { get; set; }
    }

    public class UpdateUserStatusRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ResetPasswordRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetLicenseCountRequest
    {
        public int UserId { get; set; }
    }

    public class SetMaxLicensesRequest
    {
        public int UserId { get; set; }
        public int MaxLicenses { get; set; }
    }
} 