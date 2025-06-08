using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerWeb.Server.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Web;

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
            // 检查是否是资源下载API路径，如果是则直接放行
            var path = context.HttpContext.Request.Path.Value?.ToLower();
            if (path != null && (
                path.StartsWith("/api/file/") ||
                path.StartsWith("/api/libheader/") ||
                path.StartsWith("/api/libimage/") ||
                path.StartsWith("/api/sound/")))
            {
                // 资源下载API，直接放行
                Console.WriteLine($"资源下载API，跳过认证: {path}");
                return;
            }

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
        public Task? ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.HttpContext.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new { code = HttpStatusCode.Unauthorized, msg = "身份认证失败！" });
            var result = context.HttpContext.Response.WriteAsync(json);
            return result;
        }
    }
}

