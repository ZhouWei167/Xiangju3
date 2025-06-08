using Microsoft.AspNetCore.Mvc;
using Server.Web.Controllers;
using Server.Web.Models.License;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Server.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MySql.Data.MySqlClient;

namespace Server.Web.Controllers
{
    [AuthorFilter]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILogger<LicenseController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly LicenseRecordService _licenseRecordService;
        private readonly AuthService _authService;
        private readonly DbHelper _dbHelper;

        public LicenseController(
            ILogger<LicenseController> logger, 
            IWebHostEnvironment env, 
            LicenseRecordService licenseRecordService,
            AuthService authService,
            DbHelper dbHelper)
        {
            _logger = logger;
            _environment = env;
            _licenseRecordService = licenseRecordService;
            _authService = authService;
            _dbHelper = dbHelper;
        }

        /// <summary>
        /// 获取本机硬件ID
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetLocalHardwareID()
        {
            try
            {
                _logger.LogInformation("开始获取本机硬件ID");
                
                // 捕获控制台输出
                var originalOut = Console.Out;
                var stringWriter = new StringWriter();
                Console.SetOut(stringWriter);
                
                // 获取硬件ID
                string hardwareId = Status.GetHardwareID(true, true, true, true);
                
                // 恢复控制台输出
                Console.SetOut(originalOut);
                
                // 获取调试信息
                string debugInfo = stringWriter.ToString();
                _logger.LogInformation($"成功获取硬件ID: {hardwareId}");
                _logger.LogInformation($"调试信息: {debugInfo}");
                
                return Ok(new { 
                    hardwareId = hardwareId, 
                    debugInfo = debugInfo 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取本机硬件ID失败");
                return BadRequest(new { message = $"获取本机硬件ID失败: {ex.Message}", stack = ex.StackTrace });
            }
        }

        /// <summary>
        /// 生成授权文件
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GenerateLicense([FromBody] LicenseGenerator.LicenseInfo licenseInfo, [FromQuery] bool forceGenerate = false)
        {
            try
            {
                // 检查用户是否已登录
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("未登录用户尝试生成授权文件");
                    return Forbid();
                }
                
                // 检查普通用户是否超过生成次数限制（管理员不受限制）
                if (User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme &&
                    !_authService.IsCurrentUserAdmin() &&
                    !_authService.CanCurrentUserGenerateMoreLicenses())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试生成授权文件，但已达到最大生成次数限制");
                    return BadRequest(new { message = "您已达到最大授权文件生成次数限制，无法继续生成" });
                }
                
                // API用户不受次数限制
                if (User.Identity.AuthenticationType == "ApiAuth")
                {
                    // API认证用户无需进一步检查
                }
                
                // 检查forceGenerate参数，只有管理员才能使用
                if (forceGenerate && User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme)
                {
                    if (!_authService.IsCurrentUserAdmin())
                    {
                        _logger.LogWarning($"非管理员用户 {User.Identity.Name} 尝试使用强制生成功能");
                        return BadRequest(new { message = "只有管理员才能使用强制生成功能" });
                    }
                }

                // 验证授权信息
                licenseInfo.Validate();
                
                // 对于普通用户，强制设置一些固定参数
                if (User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme &&
                    !_authService.IsCurrentUserAdmin())
                {
                    // 强制设置过期时间为30天后
                    licenseInfo.ExpirationDate = DateTime.Now.AddDays(30);
                    
                    // 强制设置客户名称为当前用户名
                    licenseInfo.CustomerName = User.Identity.Name;
                    
                    // 强制设置其他参数
                    licenseInfo.EnableEvaluationLock = true;
                    licenseInfo.EnableExpirationDateLock = true;
                    licenseInfo.EnableNumberOfUsesLock = true;
                    licenseInfo.EnableNumberOfInstancesLock = true;
                    licenseInfo.EnableHardwareLock = true;
                    licenseInfo.EvaluationType = Server.Web.Models.License.EvaluationType.Trial_Days;
                    licenseInfo.EvaluationTime = 30;
                    licenseInfo.Version = "1.0.0";
                    licenseInfo.ProductName = "伏羲授权";
                    
                    // 只有这些参数可以由用户设置：
                    // - licenseInfo.HardwareID (已由用户提供)
                    // - licenseInfo.CustomData (已由用户提供)
                }
                
                // 检查该硬件ID是否已存在授权记录
                var existingRecords = _licenseRecordService.GetLicenseRecordsByHardwareId(licenseInfo.HardwareID);
                if (existingRecords.Rows.Count > 0 && !forceGenerate)
                {
                    _logger.LogWarning($"硬件ID {licenseInfo.HardwareID} 已存在授权记录，不允许重复生成");
                    return BadRequest(new { 
                        message = $"该硬件ID已存在授权记录，不允许重复生成授权文件。如需强制生成，请添加forceGenerate=true参数。",
                        existingRecord = true,
                        hardwareId = licenseInfo.HardwareID
                    });
                }

                // 生成唯一的文件名
                string fileName = $"{licenseInfo.HardwareID}.license";
                string outputPath = Path.Combine(_environment.ContentRootPath, "Licenses", fileName);

                // 确保目录存在
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, "Licenses"));

                // 生成授权文件
                LicenseGenerator.GenerateLicenseFile(licenseInfo, outputPath);

                // 保存授权记录到数据库
                try
                {
                    // 获取创建者信息
                    string createdBy = null;
                    string targetUser = null;
                    
                    if (User.Identity.IsAuthenticated && User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme)
                    {
                        // 从Cookie认证获取用户名
                        createdBy = User.Identity.Name;
                        
                        // 增加用户的授权文件生成次数（普通用户）
                        if (!_authService.IsCurrentUserAdmin())
                        {
                            var currentUser = _authService.GetCurrentUser();
                            if (currentUser != null)
                            {
                                _authService.IncrementLicenseGenerationCount(currentUser.Id);
                                _logger.LogInformation($"用户 {currentUser.Username} 的授权文件生成次数已增加，当前为 {currentUser.LicenseGenerationCount + 1}");
                            }
                        }
                        else
                        {
                            // 对于管理员，如果指定了客户名称，检查该用户是否存在
                            if (!string.IsNullOrWhiteSpace(licenseInfo.CustomerName) && licenseInfo.CustomerName != createdBy)
                            {
                                var user = _authService.GetUserByUsername(licenseInfo.CustomerName);
                                if (user != null)
                                {
                                    // 将该用户设为目标用户
                                    targetUser = licenseInfo.CustomerName;
                                    _logger.LogInformation($"管理员 {createdBy} 为用户 {targetUser} 创建授权");
                                }
                                else
                                {
                                    // 如果用户不存在，仍然记录客户名称，但不进行用户绑定
                                    _logger.LogWarning($"管理员 {createdBy} 指定的客户名称 {licenseInfo.CustomerName} 不是系统用户");
                                }
                            }
                        }
                    }
                    else if (User.Identity.AuthenticationType == "ApiAuth")
                    {
                        // API认证用户
                        createdBy = "API:" + User.Identity.Name;
                        
                        // 对于API用户，如果指定了客户名称，检查该用户是否存在
                        if (!string.IsNullOrWhiteSpace(licenseInfo.CustomerName))
                        {
                            var user = _authService.GetUserByUsername(licenseInfo.CustomerName);
                            if (user != null)
                            {
                                // 将该用户设为目标用户
                                targetUser = licenseInfo.CustomerName;
                                _logger.LogInformation($"API用户 {createdBy} 为用户 {targetUser} 创建授权");
                            }
                        }
                    }
                    
                    // 对于普通用户，强制绑定客户名称为当前用户名
                    if (User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme &&
                        !_authService.IsCurrentUserAdmin())
                    {
                        // 强制覆盖客户名称
                        licenseInfo.CustomerName = User.Identity.Name;
                    }
                    
                    // 保存主记录
                    _licenseRecordService.SaveLicenseRecord(licenseInfo, createdBy);
                    _logger.LogInformation($"授权记录已保存到数据库，硬件ID: {licenseInfo.HardwareID}, 创建者: {createdBy}");
                    
                    // 如果有目标用户，并且与创建者不同，为目标用户也创建一条记录
                    if (!string.IsNullOrEmpty(targetUser) && targetUser != createdBy)
                    {
                        try
                        {
                            // 获取刚刚创建的记录ID
                            string getRecordSql = @"
                            SELECT id FROM license_records 
                            WHERE hardware_id = @hardware_id AND created_by = @created_by 
                            ORDER BY created_at DESC LIMIT 1";
                            
                            MySqlParameter[] getRecordParams = {
                                new MySqlParameter("@hardware_id", licenseInfo.HardwareID),
                                new MySqlParameter("@created_by", createdBy)
                            };
                            
                            var result = _dbHelper.Query(getRecordSql, getRecordParams);
                            if (result.Rows.Count > 0)
                            {
                                int recordId = Convert.ToInt32(result.Rows[0]["id"]);
                                
                                // 在用户关联表中创建映射
                                string insertMappingSql = @"
                                INSERT INTO user_license_mapping (user_id, license_id, created_at) 
                                VALUES ((SELECT id FROM users WHERE username = @username), @license_id, NOW())";
                                
                                MySqlParameter[] insertMappingParams = {
                                    new MySqlParameter("@username", targetUser),
                                    new MySqlParameter("@license_id", recordId)
                                };
                                
                                _dbHelper.ExecuteSql(insertMappingSql, insertMappingParams);
                                _logger.LogInformation($"已为用户 {targetUser} 创建授权记录映射，授权ID: {recordId}");
                            }
                        }
                        catch (Exception mapEx)
                        {
                            _logger.LogError(mapEx, $"为用户 {targetUser} 创建授权映射失败: {mapEx.Message}");
                            // 不中断主流程
                        }
                    }
                }
                catch (Exception dbEx)
                {
                    _logger.LogError(dbEx, $"保存授权记录到数据库失败，但授权文件已生成: {dbEx.Message}");
                    // 不要因为数据库错误而中断整个流程
                }

                // 读取生成的文件
                byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);

                // 返回文件供下载
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成授权文件失败");
                return BadRequest(new { message = $"生成授权文件失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 验证授权文件
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult<LicenseGenerator.LicenseInfo> ValidateLicense(IFormFile licenseFile)
        {
            try
            {
                if (licenseFile == null || licenseFile.Length == 0)
                {
                    return BadRequest(new { message = "未提供授权文件" });
                }

                // 临时保存上传的文件
                string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.license");
                
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    licenseFile.CopyTo(stream);
                }

                // 验证授权文件
                var licenseInfo = LicenseGenerator.LoadLicenseFile(tempPath);

                // 删除临时文件
                System.IO.File.Delete(tempPath);

                return Ok(licenseInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证授权文件失败");
                return BadRequest(new { message = $"验证授权文件失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取授权记录列表
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetLicenseRecords(int page = 1, int pageSize = 20)
        {
            try
            {
                // 检查用户是否有权限查看授权记录
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("未登录用户尝试查看授权记录");
                    return Forbid();
                }

                int offset = (page - 1) * pageSize;
                DataTable records;
                int total;

                // 如果是管理员或通过API认证，可以查看所有记录
                if (_authService.IsCurrentUserAdmin() || 
                    (User.Identity.AuthenticationType == "ApiAuth") ||
                    _authService.CanCurrentUserViewRecords())
                {
                    records = _licenseRecordService.GetAllLicenseRecords(pageSize, offset);
                    total = _licenseRecordService.GetTotalLicenseRecordsCount();
                }
                else
                {
                    // 普通用户只能查看自己创建的记录
                    string username = User.Identity.Name;
                    records = _licenseRecordService.GetLicenseRecordsByCreator(username, pageSize, offset);
                    total = _licenseRecordService.GetTotalLicenseRecordsCountByCreator(username);
                }

                // 转换DataTable为JSON友好的格式
                var result = new List<Dictionary<string, object>>();
                foreach (DataRow row in records.Rows)
                {
                    var item = new Dictionary<string, object>();
                    foreach (DataColumn column in records.Columns)
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
                    records = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取授权记录列表失败");
                return BadRequest(new { message = $"获取授权记录列表失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 根据硬件ID获取授权记录
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetLicenseRecordsByHardwareId(string hardwareId)
        {
            try
            {
                // 检查用户是否有权限查看授权记录
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("未登录用户尝试查看授权记录");
                    return Forbid();
                }

                if (string.IsNullOrEmpty(hardwareId))
                {
                    return BadRequest(new { message = "硬件ID不能为空" });
                }

                DataTable records;

                // 如果是管理员或通过API认证，可以查看所有记录
                if (_authService.IsCurrentUserAdmin() || 
                    (User.Identity.AuthenticationType == "ApiAuth") ||
                    _authService.CanCurrentUserViewRecords())
                {
                    records = _licenseRecordService.GetLicenseRecordsByHardwareId(hardwareId);
                }
                else
                {
                    // 普通用户只能查看自己创建的记录
                    string username = User.Identity.Name;
                    string sql = "SELECT * FROM license_records WHERE hardware_id = @hardware_id AND created_by = @created_by ORDER BY created_at DESC";
                    MySqlParameter[] parameters = {
                        new MySqlParameter("@hardware_id", hardwareId),
                        new MySqlParameter("@created_by", username)
                    };
                    records = _dbHelper.Query(sql, parameters);
                }

                // 转换DataTable为JSON友好的格式
                var result = new List<Dictionary<string, object>>();
                foreach (DataRow row in records.Rows)
                {
                    var item = new Dictionary<string, object>();
                    foreach (DataColumn column in records.Columns)
                    {
                        item[column.ColumnName] = row[column] == DBNull.Value ? null : row[column];
                    }
                    result.Add(item);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取硬件ID为{hardwareId}的授权记录失败");
                return BadRequest(new { message = $"获取授权记录失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 删除授权记录
        /// </summary>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult DeleteLicenseRecord(int id)
        {
            try
            {
                // 检查用户是否已登录
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("未登录用户尝试删除授权记录");
                    return Forbid();
                }

                // 检查是否有权限删除（只有管理员和API用户才能删除）
                if (!_authService.IsCurrentUserAdmin() && 
                    User.Identity.AuthenticationType != "ApiAuth" &&
                    !_authService.CanCurrentUserViewRecords())
                {
                    _logger.LogWarning($"普通用户 {User.Identity.Name} 尝试删除授权记录，操作被拒绝");
                    return Forbid();
                }

                // 先获取记录详情
                string checkSql = "SELECT * FROM license_records WHERE id = @id";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@id", id)
                };
                
                DataTable recordData = _dbHelper.Query(checkSql, checkParams);
                if (recordData.Rows.Count == 0)
                {
                    return NotFound(new { message = $"未找到ID为{id}的授权记录" });
                }

                int result = _licenseRecordService.DeleteLicenseRecord(id);
                if (result > 0)
                {
                    return Ok(new { message = $"授权记录已删除，ID: {id}" });
                }
                else
                {
                    return NotFound(new { message = $"未找到ID为{id}的授权记录" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除授权记录失败，ID: {id}");
                return BadRequest(new { message = $"删除授权记录失败: {ex.Message}" });
            }
        }
        
        /// <summary>
        /// 获取当前用户的授权记录
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetMyLicenseRecords(int page = 1, int pageSize = 20)
        {
            try
            {
                // 检查用户是否已登录
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("未登录用户尝试获取个人授权记录");
                    return Forbid();
                }
                
                // 获取当前用户名
                string currentUsername = User.Identity.Name;
                
                // 获取当前用户创建的授权记录
                int offset = (page - 1) * pageSize;
                DataTable records = _licenseRecordService.GetLicenseRecordsByCreator(currentUsername, pageSize, offset);
                int total = _licenseRecordService.GetTotalLicenseRecordsCountByCreator(currentUsername);
                
                // 转换DataTable为JSON友好的格式
                var result = new List<Dictionary<string, object>>();
                foreach (DataRow row in records.Rows)
                {
                    var item = new Dictionary<string, object>();
                    foreach (DataColumn column in records.Columns)
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
                    records = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取当前用户授权记录失败");
                return BadRequest(new { message = $"获取授权记录失败: {ex.Message}" });
            }
        }
    }
} 