using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerWeb.Server.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Server.Web.Controllers
{
    public class AuthorFilterAttribute : Attribute, IActionFilter
    {
        public AuthorFilterAttribute()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 检查是否有AllowAnonymous特性
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                return; // 如果有AllowAnonymous特性，跳过验证
            }
            
            // 检查是否已通过Cookie认证
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return; // 如果已通过Cookie认证，跳过API认证检查
            }
            
            // 只有当没有通过Cookie认证时，才检查API认证头
            var author = context.HttpContext.Request.Headers["User"].FirstOrDefault();
            var safeCode = context.HttpContext.Request.Headers["Code"].FirstOrDefault();
            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(safeCode))
            {
                Error(context, author ?? "未知用户", safeCode ?? "空安全码");
                return;
            }
            author = HttpUtility.UrlDecode(author);
            if (!(/*safeCode == GameController.AppVersion &&*/ author == GameController.Author))
            {
                Error(context, author, safeCode);
                return;
            }
        }


        private void Error(ActionExecutingContext context, string author, string safeCode)
        {
            context.Result = new AuthorResult();
        }
    }

    public class AuthorResult : IActionResult
    {
        [RequiresUnreferencedCode("json")]
        [RequiresDynamicCode("json")]
        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.HttpContext.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new { code = HttpStatusCode.Unauthorized, msg = "身份认证失败！" });
            return context.HttpContext.Response.WriteAsync(json);
        }
    }
}

