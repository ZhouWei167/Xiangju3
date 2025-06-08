using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Server.Web.Models.Auth
{
    public class ApiAuthHandler : AuthenticationHandler<ApiAuthOptions>
    {
        private readonly AuthService _authService;
        private readonly ILogger<ApiAuthHandler> _logger;

        public ApiAuthHandler(
            IOptionsMonitor<ApiAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
            _logger = (ILogger<ApiAuthHandler>)logger.CreateLogger<ApiAuthHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 检查请求头中是否包含必要的认证信息
            if (!Request.Headers.TryGetValue("User", out var userValues) ||
                !Request.Headers.TryGetValue("Code", out var codeValues))
            {
                _logger.LogWarning("API请求缺少认证头");
                return Task.FromResult(AuthenticateResult.Fail("缺少认证头"));
            }

            var username = userValues.FirstOrDefault();
            var code = codeValues.FirstOrDefault();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("API认证头值为空");
                return Task.FromResult(AuthenticateResult.Fail("认证头值为空"));
            }

            // 这里可以根据实际需求进行更复杂的验证
            // 简单起见，我们只验证用户名是否为"BlackDragon"，代码是否为"1.0.0"
            if (username != "BlackDragon" || code != "1.0.0")
            {
                _logger.LogWarning($"API认证失败：用户名={username}, 代码={code}");
                return Task.FromResult(AuthenticateResult.Fail("认证失败"));
            }

            // 创建身份认证票据
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, UserRole.SuperAdmin.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            _logger.LogInformation($"API认证成功：用户名={username}");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class ApiAuthOptions : AuthenticationSchemeOptions
    {
    }
} 