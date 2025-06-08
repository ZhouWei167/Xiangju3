using Library;
using ServerWeb.Server.Controllers;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Data;
using Server.Web.Models.License;
using Server.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationManager();
if (File.Exists("appsettings.json"))
{
    config.AddJsonFile("appsettings.json");
}
var environment = builder.Environment.EnvironmentName;
if (File.Exists($"appsettings.{environment}.json"))
{
    config.AddJsonFile($"appsettings.{environment}.json");
}

#region 资源文件
// 获取资源路径
string resoucePath = string.IsNullOrEmpty(config["resouce"]) ? AppContext.BaseDirectory : config["resouce"];

// 规范化路径，适应不同操作系统
if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
{
    // Linux/macOS 系统路径处理
    resoucePath = resoucePath.Replace("\\", "/");
    if (resoucePath.StartsWith("C:") || resoucePath.StartsWith("c:"))
    {
        // 如果是 Windows 格式的路径 (C:\...)，转换为 Linux 格式
        resoucePath = resoucePath.Substring(2).Replace("\\", "/");
        if (!resoucePath.StartsWith("/"))
        {
            resoucePath = "/" + resoucePath;
        }
    }
    
    // 如果目录不存在，尝试在当前目录或上级目录查找
    if (!Directory.Exists(resoucePath))
    {
        var alternativePaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Client").Replace("\\", "/"),
            Path.Combine(AppContext.BaseDirectory, "../Client").Replace("\\", "/"),
            Path.Combine(AppContext.BaseDirectory, "../../Client").Replace("\\", "/"),
            "/work/game145/Debug/Client",
            "/game145/Debug/Client",
            "/Client"
        };
        
        foreach (var path in alternativePaths)
        {
            if (Directory.Exists(path))
            {
                resoucePath = path;
                Console.WriteLine($"找到替代资源路径: {resoucePath}");
                break;
            }
        }
    }
}
else
{
    // Windows 系统路径处理
    resoucePath = resoucePath.Replace("/", "\\");
}

Library.Globals.ResoucePath = resoucePath;
GameController.AppVersion = config["appVersion"];
GameController.Author = config["author"];
Console.WriteLine($"资源路径: {Globals.ResoucePath}");
Console.WriteLine($"应用版本: {GameController.AppVersion}");
Console.WriteLine($"作者: {GameController.Author}");

// 检查资源目录是否存在
if (!Directory.Exists(Globals.ResoucePath))
{
    Console.WriteLine($"警告: 资源目录不存在: {Globals.ResoucePath}");
    
    // 尝试创建资源目录
    try
    {
        Directory.CreateDirectory(Globals.ResoucePath);
        Console.WriteLine($"已创建资源目录: {Globals.ResoucePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"创建资源目录失败: {ex.Message}");
    }
}

// 检查Data目录是否存在
string dataPath = Path.Combine(Globals.ResoucePath, "Data").Replace("\\", "/");
if (!Directory.Exists(dataPath))
{
    Console.WriteLine($"警告: Data目录不存在: {dataPath}");
    
    // 尝试创建Data目录
    try
    {
        Directory.CreateDirectory(dataPath);
        Console.WriteLine($"已创建Data目录: {dataPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"创建Data目录失败: {ex.Message}");
    }
}

// 检查Sound目录是否存在
string soundPath = Path.Combine(Globals.ResoucePath, "Sound").Replace("\\", "/");
if (!Directory.Exists(soundPath))
{
    Console.WriteLine($"警告: Sound目录不存在: {soundPath}");
    
    // 尝试创建Sound目录
    try
    {
        Directory.CreateDirectory(soundPath);
        Console.WriteLine($"已创建Sound目录: {soundPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"创建Sound目录失败: {ex.Message}");
    }
}

// 加载资源文件
int loadedCount = 0;
int failedCount = 0;
foreach (KeyValuePair<LibraryFile, string> library in Libraries.LibraryList)
{
    var fullpath = Path.Combine(Globals.ResoucePath, library.Value).Replace("\\", "/");
    if (File.Exists(fullpath))
    {
        try
        {
            GameController.LibraryList[library.Value] = new MirLibrary(fullpath);
            loadedCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载资源文件失败: {fullpath}, 错误: {ex.Message}");
            failedCount++;
        }
    }
    else
    {
        Console.WriteLine($"资源文件不存在: {fullpath}");
        failedCount++;
    }
}
Console.WriteLine($"已加载资源文件: {loadedCount}个, 失败: {failedCount}个");
#endregion

// 注册数据库服务
builder.Services.AddSingleton<DbHelper>();
builder.Services.AddScoped<LicenseRecordService>();

// 注册认证服务
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

// 添加认证
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/AccessDenied";
    options.Cookie.Name = "ZirconServer.Auth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
})
.AddScheme<ApiAuthOptions, ApiAuthHandler>("ApiAuth", options => { });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// 配置应用程序URL
string urls = config["urls"] ?? "http://localhost:5000";
builder.WebHost.UseUrls(urls);

// 强制设置监听端口5000
if (!urls.Contains(":5000"))
{
    builder.WebHost.UseUrls("http://localhost:5000");
    Console.WriteLine("已强制设置监听端口为5000");
}

var app = builder.Build();

// 初始化数据库
try
{
    var dbHelper = app.Services.GetRequiredService<DbHelper>();
    dbHelper.InitializeDatabase();
    Console.WriteLine("数据库初始化成功");

    // 创建作用域来解析作用域服务
    using (var scope = app.Services.CreateScope())
    {
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
        var licenseRecordService = scope.ServiceProvider.GetRequiredService<LicenseRecordService>();
        
        authService.InitializeDatabase();
        licenseRecordService.InitializeDatabase();
        
        // 紧急修复可能被锁定的管理员账户
        EmergencyRepairAdminAccounts(scope.ServiceProvider);
        
        // 修复现有用户的用户组关系
        FixUserGroupMappings(scope.ServiceProvider);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"数据库初始化失败: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // 在开发环境中使用详细的异常页面
    app.UseDeveloperExceptionPage();
}

// 添加资源下载API认证中间件
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    if (path != null && (
        path.StartsWith("/api/file/") ||
        path.StartsWith("/api/libheader/") ||
        path.StartsWith("/api/libimage/") ||
        path.StartsWith("/api/sound/")))
    {
        // 对于资源下载API，添加一个标记，表示已通过认证
        context.Items["SkipAuthorization"] = true;
        Console.WriteLine($"资源下载API，跳过认证: {path}");
    }
    
    await next();
});

// 添加API请求日志记录中间件
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path.StartsWithSegments("/api/"))
    {
        Console.WriteLine($"收到API请求: {context.Request.Method} {path}");
        Console.WriteLine($"请求头部: User={context.Request.Headers["User"]}, Code={context.Request.Headers["Code"]}");
    }
    
    await next();
    
    if (path.StartsWithSegments("/api/"))
    {
        Console.WriteLine($"API响应状态码: {context.Response.StatusCode} 路径: {path}");
    }
});

// 添加静态文件请求日志记录中间件
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path.StartsWithSegments("/favicon.ico") || 
        path.StartsWithSegments("/css") || 
        path.StartsWithSegments("/js"))
    {
        Console.WriteLine($"请求静态资源: {path}");
    }
    
    await next();
    
    if (context.Response.StatusCode == 404)
    {
        Console.WriteLine($"资源未找到(404): {path}");
    }
});

// 输出当前目录和wwwroot路径，用于调试
Console.WriteLine($"当前目录: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"ContentRootPath: {app.Environment.ContentRootPath}");
Console.WriteLine($"WebRootPath: {app.Environment.WebRootPath}");

// 确保wwwroot目录存在
var wwwrootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
Console.WriteLine($"wwwrootPath: {wwwrootPath}");
Console.WriteLine($"wwwroot目录是否存在: {Directory.Exists(wwwrootPath)}");

if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
    Console.WriteLine("已创建wwwroot目录");
}

// 确保css和js目录存在
var cssPath = Path.Combine(wwwrootPath, "css");
var jsPath = Path.Combine(wwwrootPath, "js");

if (!Directory.Exists(cssPath))
{
    Directory.CreateDirectory(cssPath);
    Console.WriteLine("已创建css目录");
}

if (!Directory.Exists(jsPath))
{
    Directory.CreateDirectory(jsPath);
    Console.WriteLine("已创建js目录");
}

// 确保site.css和site.js文件存在
var cssSitePath = Path.Combine(cssPath, "site.css");
var jsSitePath = Path.Combine(jsPath, "site.js");

if (!File.Exists(cssSitePath))
{
    File.WriteAllText(cssSitePath, @"html {
  font-size: 14px;
}

@media (min-width: 768px) {
  html {
    font-size: 16px;
  }
}

html {
  position: relative;
  min-height: 100%;
}

body {
  margin-bottom: 60px;
}

.footer {
  position: absolute;
  bottom: 0;
  width: 100%;
  white-space: nowrap;
  line-height: 60px;
}");
    Console.WriteLine("已创建site.css文件");
}

if (!File.Exists(jsSitePath))
{
    File.WriteAllText(jsSitePath, "// 站点JavaScript文件\r\nconsole.log(\"站点JavaScript已加载\");");
    Console.WriteLine("已创建site.js文件");
}

// 设置静态文件中间件
app.UseStaticFiles();

// 输出静态文件的完整路径，用于调试
Console.WriteLine($"CSS文件路径: {cssSitePath}");
Console.WriteLine($"CSS文件是否存在: {File.Exists(cssSitePath)}");
Console.WriteLine($"JS文件路径: {jsSitePath}");
Console.WriteLine($"JS文件是否存在: {File.Exists(jsSitePath)}");

// 创建一个简单的favicon.ico文件，避免404错误
var faviconPath = Path.Combine(wwwrootPath, "favicon.ico");
if (!File.Exists(faviconPath))
{
    Console.WriteLine("创建favicon.ico文件");
    // 创建一个最小的favicon.ico文件
    using (var fs = new FileStream(faviconPath, FileMode.Create))
    {
        fs.WriteByte(0);
    }
}
Console.WriteLine($"Favicon路径: {faviconPath}");
Console.WriteLine($"Favicon是否存在: {File.Exists(faviconPath)}");

// 创建Licenses目录用于存储生成的授权文件
var licensesPath = Path.Combine(app.Environment.ContentRootPath, "Licenses");
if (!Directory.Exists(licensesPath))
{
    Directory.CreateDirectory(licensesPath);
}

// 设置路由
app.UseRouting();

// 添加认证和授权中间件
app.UseAuthentication();
app.UseAuthorization();

// 设置终结点
app.MapRazorPages();
app.MapControllers();

app.Run();

// 修复用户组映射的辅助函数
void FixUserGroupMappings(IServiceProvider serviceProvider)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var dbHelper = serviceProvider.GetRequiredService<DbHelper>();
    
    try
    {
        // 1. 获取默认用户组ID
        string getDefaultGroupSql = "SELECT id FROM user_groups WHERE name = '用户组' LIMIT 1";
        var defaultGroupResult = dbHelper.Query(getDefaultGroupSql);
        
        if (defaultGroupResult.Rows.Count == 0)
        {
            logger.LogWarning("修复用户组映射失败：未找到默认用户组");
            return;
        }
        
        int defaultGroupId = Convert.ToInt32(defaultGroupResult.Rows[0]["id"]);
        
        // 获取管理员组ID
        string getAdminGroupSql = "SELECT id FROM user_groups WHERE name = '管理员组' LIMIT 1";
        var adminGroupResult = dbHelper.Query(getAdminGroupSql);
        
        int? adminGroupId = null;
        if (adminGroupResult.Rows.Count > 0)
        {
            adminGroupId = Convert.ToInt32(adminGroupResult.Rows[0]["id"]);
        }
        
        // 2. 查找所有普通用户（角色为User）
        string findUsersSql = "SELECT id FROM users WHERE role = 1";
        var usersResult = dbHelper.Query(findUsersSql);
        
        // 3. 修复每个普通用户的组映射
        int fixedCount = 0;
        foreach (DataRow userRow in usersResult.Rows)
        {
            int userId = Convert.ToInt32(userRow["id"]);
            
            // 检查用户是否已经在某个组中
            string checkMappingSql = "SELECT COUNT(*) FROM user_group_mapping WHERE user_id = @user_id";
            MySqlParameter[] checkParams = {
                new MySqlParameter("@user_id", userId)
            };
            
            int mappingCount = Convert.ToInt32(dbHelper.ExecuteScalar(checkMappingSql, checkParams));
            
            // 如果用户不在任何组中，添加到默认组
            if (mappingCount == 0)
            {
                string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                MySqlParameter[] addParams = {
                    new MySqlParameter("@user_id", userId),
                    new MySqlParameter("@group_id", defaultGroupId)
                };
                
                dbHelper.ExecuteSql(addToGroupSql, addParams);
                fixedCount++;
            }
        }
        
        logger.LogInformation($"普通用户组映射修复完成：已将 {fixedCount} 个用户添加到默认用户组");
        
        // 4. 修复管理员用户的组映射
        if (adminGroupId.HasValue)
        {
            // 查找所有管理员用户（角色为Admin或SuperAdmin）
            string findAdminsSql = "SELECT id FROM users WHERE role >= 2";
            var adminsResult = dbHelper.Query(findAdminsSql);
            
            int fixedAdminCount = 0;
            foreach (DataRow adminRow in adminsResult.Rows)
            {
                int adminId = Convert.ToInt32(adminRow["id"]);
                
                // 检查管理员是否在管理员组中
                string checkAdminMappingSql = "SELECT COUNT(*) FROM user_group_mapping WHERE user_id = @user_id AND group_id = @group_id";
                MySqlParameter[] checkAdminParams = {
                    new MySqlParameter("@user_id", adminId),
                    new MySqlParameter("@group_id", adminGroupId.Value)
                };
                
                int adminMappingCount = Convert.ToInt32(dbHelper.ExecuteScalar(checkAdminMappingSql, checkAdminParams));
                
                // 如果管理员不在管理员组中
                if (adminMappingCount == 0)
                {
                    // 先清除其他组映射
                    string clearGroupsSql = "DELETE FROM user_group_mapping WHERE user_id = @user_id";
                    MySqlParameter[] clearParams = {
                        new MySqlParameter("@user_id", adminId)
                    };
                    
                    dbHelper.ExecuteSql(clearGroupsSql, clearParams);
                    
                    // 添加到管理员组
                    string addToAdminGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                    MySqlParameter[] addAdminParams = {
                        new MySqlParameter("@user_id", adminId),
                        new MySqlParameter("@group_id", adminGroupId.Value)
                    };
                    
                    dbHelper.ExecuteSql(addToAdminGroupSql, addAdminParams);
                    fixedAdminCount++;
                }
            }
            
            logger.LogInformation($"管理员用户组映射修复完成：已将 {fixedAdminCount} 个管理员添加到管理员组");
        }
        else
        {
            logger.LogWarning("未找到管理员组，无法修复管理员用户组映射");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "修复用户组映射时发生错误");
    }
}

// 紧急修复管理员账户函数
void EmergencyRepairAdminAccounts(IServiceProvider serviceProvider)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var dbHelper = serviceProvider.GetRequiredService<DbHelper>();
    
    try
    {
        // 获取当前的管理员账户数量
        string getAdminCountSql = "SELECT COUNT(*) FROM users WHERE role >= 2 AND is_active = 1";
        int adminCount = Convert.ToInt32(dbHelper.ExecuteScalar(getAdminCountSql));
        
        // 如果没有活跃的管理员账户，尝试恢复默认的管理员账户
        if (adminCount == 0)
        {
            logger.LogWarning("系统中没有活跃的管理员账户，尝试修复...");
            
            // 检查是否存在 admin 账户
            string checkAdminSql = "SELECT id, role, is_active FROM users WHERE username = 'admin' LIMIT 1";
            var adminResult = dbHelper.Query(checkAdminSql);
            
            if (adminResult.Rows.Count > 0)
            {
                int adminId = Convert.ToInt32(adminResult.Rows[0]["id"]);
                int role = Convert.ToInt32(adminResult.Rows[0]["role"]);
                bool isActive = Convert.ToBoolean(adminResult.Rows[0]["is_active"]);
                
                // 如果admin账户存在但被禁用或角色不是管理员
                if (!isActive || role < 2)
                {
                    // 启用账户并设置为超级管理员
                    string updateAdminSql = "UPDATE users SET is_active = 1, role = 3 WHERE id = @id";
                    MySqlParameter[] updateParams = {
                        new MySqlParameter("@id", adminId)
                    };
                    
                    dbHelper.ExecuteSql(updateAdminSql, updateParams);
                    logger.LogWarning($"已恢复管理员账户 'admin'，ID: {adminId}");
                    
                    // 确保admin账户在管理员组中
                    string getAdminGroupSql = "SELECT id FROM user_groups WHERE name = '管理员组' LIMIT 1";
                    var adminGroupResult = dbHelper.Query(getAdminGroupSql);
                    
                    if (adminGroupResult.Rows.Count > 0)
                    {
                        int adminGroupId = Convert.ToInt32(adminGroupResult.Rows[0]["id"]);
                        
                        // 清除现有组映射
                        string clearGroupsSql = "DELETE FROM user_group_mapping WHERE user_id = @user_id";
                        MySqlParameter[] clearParams = {
                            new MySqlParameter("@user_id", adminId)
                        };
                        
                        dbHelper.ExecuteSql(clearGroupsSql, clearParams);
                        
                        // 添加到管理员组
                        string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                        MySqlParameter[] addParams = {
                            new MySqlParameter("@user_id", adminId),
                            new MySqlParameter("@group_id", adminGroupId)
                        };
                        
                        dbHelper.ExecuteSql(addToGroupSql, addParams);
                        logger.LogWarning($"已将管理员账户 'admin' 添加到管理员组");
                    }
                }
                else
                {
                    logger.LogInformation("管理员账户 'admin' 状态正常");
                }
            }
            else
            {
                // 如果没有admin账户，创建一个默认的admin账户
                logger.LogWarning("未找到管理员账户，创建默认管理员账户...");
                
                // 创建默认管理员账户
                string createAdminSql = @"
                INSERT INTO users (username, password_hash, display_name, email, role, is_active)
                VALUES (@username, @password_hash, @display_name, @email, @role, @is_active)";
                
                // 计算密码哈希
                string passwordHash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin123"));
                    passwordHash = Convert.ToBase64String(hashedBytes);
                }
                
                MySqlParameter[] createParams = {
                    new MySqlParameter("@username", "admin"),
                    new MySqlParameter("@password_hash", passwordHash),
                    new MySqlParameter("@display_name", "系统管理员"),
                    new MySqlParameter("@email", "admin@example.com"),
                    new MySqlParameter("@role", 3), // SuperAdmin
                    new MySqlParameter("@is_active", true)
                };
                
                dbHelper.ExecuteSql(createAdminSql, createParams);
                logger.LogWarning("已创建默认管理员账户 'admin'，密码: admin123");
                
                // 获取新创建的admin账户ID
                string getNewAdminIdSql = "SELECT id FROM users WHERE username = 'admin' LIMIT 1";
                var newAdminResult = dbHelper.Query(getNewAdminIdSql);
                
                if (newAdminResult.Rows.Count > 0)
                {
                    int newAdminId = Convert.ToInt32(newAdminResult.Rows[0]["id"]);
                    
                    // 确保管理员组存在
                    string getAdminGroupSql = "SELECT id FROM user_groups WHERE name = '管理员组' LIMIT 1";
                    var adminGroupResult = dbHelper.Query(getAdminGroupSql);
                    
                    int adminGroupId;
                    if (adminGroupResult.Rows.Count > 0)
                    {
                        adminGroupId = Convert.ToInt32(adminGroupResult.Rows[0]["id"]);
                    }
                    else
                    {
                        // 创建管理员组
                        string createAdminGroupSql = @"
                        INSERT INTO user_groups (name, description, minimum_role, can_generate_license, can_view_records, can_manage_users)
                        VALUES (@name, @description, @minimum_role, @can_generate_license, @can_view_records, @can_manage_users)";
                        
                        MySqlParameter[] groupParams = {
                            new MySqlParameter("@name", "管理员组"),
                            new MySqlParameter("@description", "具有所有权限的管理员组"),
                            new MySqlParameter("@minimum_role", 2), // Admin
                            new MySqlParameter("@can_generate_license", true),
                            new MySqlParameter("@can_view_records", true),
                            new MySqlParameter("@can_manage_users", true)
                        };
                        
                        dbHelper.ExecuteSql(createAdminGroupSql, groupParams);
                        logger.LogWarning("已创建管理员组");
                        
                        // 获取新创建的管理员组ID
                        adminGroupId = Convert.ToInt32(dbHelper.ExecuteScalar(getAdminGroupSql));
                    }
                    
                    // 将admin账户添加到管理员组
                    string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                    MySqlParameter[] addParams = {
                        new MySqlParameter("@user_id", newAdminId),
                        new MySqlParameter("@group_id", adminGroupId)
                    };
                    
                    dbHelper.ExecuteSql(addToGroupSql, addParams);
                    logger.LogWarning("已将新管理员账户添加到管理员组");
                }
            }
        }
        else
        {
            logger.LogInformation($"系统中有 {adminCount} 个活跃的管理员账户，无需修复");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "紧急修复管理员账户时发生错误");
    }
}
