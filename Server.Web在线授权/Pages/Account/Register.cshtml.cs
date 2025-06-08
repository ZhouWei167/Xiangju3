using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Server.Web.Models.Auth;

namespace Server.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(AuthService authService, ILogger<RegisterModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "请输入用户名")]
            [Display(Name = "用户名")]
            [StringLength(50, ErrorMessage = "{0}的长度必须在{2}到{1}个字符之间。", MinimumLength = 3)]
            public string Username { get; set; }

            [Required(ErrorMessage = "请输入密码")]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            [StringLength(100, ErrorMessage = "{0}的长度必须在{2}到{1}个字符之间。", MinimumLength = 6)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "显示名称")]
            [StringLength(100, ErrorMessage = "{0}的长度不能超过{1}个字符。")]
            public string DisplayName { get; set; }

            [EmailAddress(ErrorMessage = "邮箱格式不正确")]
            [Display(Name = "电子邮箱")]
            [StringLength(100, ErrorMessage = "{0}的长度不能超过{1}个字符。")]
            public string Email { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // 创建新用户，默认为最低权限(User)
                var user = new User
                {
                    Username = Input.Username,
                    DisplayName = Input.DisplayName,
                    Email = Input.Email,
                    Role = UserRole.User, // 默认最低权限
                    IsActive = true
                };

                var result = _authService.RegisterUser(user, Input.Password);
                if (result)
                {
                    _logger.LogInformation($"用户 {Input.Username} 注册成功");
                    SuccessMessage = "注册成功，请登录";
                    return RedirectToPage("./Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "注册失败，用户名可能已存在");
                    return Page();
                }
            }

            // 如果模型验证失败，则重新显示表单
            return Page();
        }
    }
} 