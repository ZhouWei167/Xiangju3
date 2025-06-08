using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data;
using Server.Web.Models.License;

namespace Server.Web.Models.Auth
{
    public class AuthService
    {
        private readonly DbHelper _dbHelper;
        private readonly ILogger<AuthService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DbHelper dbHelper, ILogger<AuthService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _dbHelper = dbHelper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 初始化认证相关数据库表
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                // 创建用户表
                string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS `users` (
                  `id` int NOT NULL AUTO_INCREMENT,
                  `username` varchar(50) NOT NULL,
                  `password_hash` varchar(100) NOT NULL,
                  `display_name` varchar(100) DEFAULT NULL,
                  `email` varchar(100) DEFAULT NULL,
                  `role` int NOT NULL DEFAULT 1,
                  `is_active` tinyint(1) NOT NULL DEFAULT 1,
                  `license_generation_count` int NOT NULL DEFAULT 0,
                  `max_license_generations` int NOT NULL DEFAULT 2,
                  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  `last_login_at` datetime DEFAULT NULL,
                  `register_ip` varchar(50) DEFAULT NULL,
                  `last_login_ip` varchar(50) DEFAULT NULL,
                  PRIMARY KEY (`id`),
                  UNIQUE KEY `idx_username` (`username`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";

                _dbHelper.ExecuteSql(createUsersTable);
                _logger.LogInformation("用户表初始化成功");

                // 创建用户组表
                string createUserGroupsTable = @"
                CREATE TABLE IF NOT EXISTS `user_groups` (
                  `id` int NOT NULL AUTO_INCREMENT,
                  `name` varchar(50) NOT NULL,
                  `description` varchar(200) DEFAULT NULL,
                  `minimum_role` int NOT NULL DEFAULT 1,
                  `can_generate_license` tinyint(1) NOT NULL DEFAULT 0,
                  `can_view_records` tinyint(1) NOT NULL DEFAULT 0,
                  `can_manage_users` tinyint(1) NOT NULL DEFAULT 0,
                  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  PRIMARY KEY (`id`),
                  UNIQUE KEY `idx_name` (`name`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";

                _dbHelper.ExecuteSql(createUserGroupsTable);
                _logger.LogInformation("用户组表初始化成功");

                // 创建用户-组关系表
                string createUserGroupMappingTable = @"
                CREATE TABLE IF NOT EXISTS `user_group_mapping` (
                  `user_id` int NOT NULL,
                  `group_id` int NOT NULL,
                  PRIMARY KEY (`user_id`, `group_id`),
                  KEY `fk_group_id` (`group_id`),
                  CONSTRAINT `fk_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
                  CONSTRAINT `fk_group_id` FOREIGN KEY (`group_id`) REFERENCES `user_groups` (`id`) ON DELETE CASCADE
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";

                _dbHelper.ExecuteSql(createUserGroupMappingTable);
                _logger.LogInformation("用户-组关系表初始化成功");

                // 检查是否存在管理员账户，如果不存在则创建默认管理员
                string checkAdminSql = "SELECT COUNT(*) FROM users WHERE role = @role";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@role", (int)UserRole.SuperAdmin)
                };

                int adminCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkAdminSql, checkParams));
                if (adminCount == 0)
                {
                    // 创建默认管理员账户
                    string createAdminSql = @"
                    INSERT INTO users (username, password_hash, display_name, email, role)
                    VALUES (@username, @password_hash, @display_name, @email, @role)";

                    MySqlParameter[] adminParams = {
                        new MySqlParameter("@username", "admin"),
                        new MySqlParameter("@password_hash", HashPassword("admin123")),
                        new MySqlParameter("@display_name", "系统管理员"),
                        new MySqlParameter("@email", "admin@example.com"),
                        new MySqlParameter("@role", (int)UserRole.SuperAdmin)
                    };

                    _dbHelper.ExecuteSql(createAdminSql, adminParams);
                    _logger.LogInformation("已创建默认管理员账户");

                    // 创建默认用户组
                    string createAdminGroupSql = @"
                    INSERT INTO user_groups (name, description, minimum_role, can_generate_license, can_view_records, can_manage_users)
                    VALUES (@name, @description, @minimum_role, @can_generate_license, @can_view_records, @can_manage_users)";

                    MySqlParameter[] adminGroupParams = {
                        new MySqlParameter("@name", "管理员组"),
                        new MySqlParameter("@description", "具有所有权限的管理员组"),
                        new MySqlParameter("@minimum_role", (int)UserRole.Admin),
                        new MySqlParameter("@can_generate_license", true),
                        new MySqlParameter("@can_view_records", true),
                        new MySqlParameter("@can_manage_users", true)
                    };

                    _dbHelper.ExecuteSql(createAdminGroupSql, adminGroupParams);
                    _logger.LogInformation("已创建默认管理员组");

                    // 创建普通用户组
                    MySqlParameter[] userGroupParams = {
                        new MySqlParameter("@name", "用户组"),
                        new MySqlParameter("@description", "普通用户组，可以生成授权文件"),
                        new MySqlParameter("@minimum_role", (int)UserRole.User),
                        new MySqlParameter("@can_generate_license", true),
                        new MySqlParameter("@can_view_records", false),
                        new MySqlParameter("@can_manage_users", false)
                    };

                    string createUserGroupSql = @"
                    INSERT INTO user_groups (name, description, minimum_role, can_generate_license, can_view_records, can_manage_users)
                    VALUES (@name, @description, @minimum_role, @can_generate_license, @can_view_records, @can_manage_users)";
                    
                    _dbHelper.ExecuteSql(createUserGroupSql, userGroupParams);
                    _logger.LogInformation("已创建默认用户组");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化认证数据库失败");
                throw;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                string sql = "SELECT * FROM users WHERE username = @username AND is_active = 1";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@username", username)
                };

                DataTable dt = _dbHelper.Query(sql, parameters);
                if (dt.Rows.Count == 0)
                {
                    _logger.LogWarning($"登录失败：用户名 {username} 不存在或已禁用");
                    return false;
                }

                DataRow userRow = dt.Rows[0];
                string storedHash = userRow["password_hash"].ToString();

                if (VerifyPassword(password, storedHash))
                {
                    // 获取客户端IP地址
                    string ipAddress = GetClientIPAddress();
                    
                    // 更新最后登录时间和IP
                    string updateSql = "UPDATE users SET last_login_at = @last_login_at, last_login_ip = @last_login_ip WHERE id = @id";
                    MySqlParameter[] updateParams = {
                        new MySqlParameter("@last_login_at", DateTime.Now),
                        new MySqlParameter("@last_login_ip", ipAddress),
                        new MySqlParameter("@id", userRow["id"])
                    };
                    _dbHelper.ExecuteSql(updateSql, updateParams);

                    // 创建身份认证票据
                    var user = new User
                    {
                        Id = Convert.ToInt32(userRow["id"]),
                        Username = userRow["username"].ToString(),
                        DisplayName = userRow["display_name"]?.ToString(),
                        Email = userRow["email"]?.ToString(),
                        Role = (UserRole)Convert.ToInt32(userRow["role"]),
                        IsActive = Convert.ToBoolean(userRow["is_active"]),
                        CreatedAt = Convert.ToDateTime(userRow["created_at"]),
                        LastLoginAt = DateTime.Now,
                        LastLoginIP = ipAddress,
                        RegisterIP = userRow["register_ip"]?.ToString(),
                        LicenseGenerationCount = userRow["license_generation_count"] != DBNull.Value ? Convert.ToInt32(userRow["license_generation_count"]) : 0,
                        MaxLicenseGenerations = userRow["max_license_generations"] != DBNull.Value ? Convert.ToInt32(userRow["max_license_generations"]) : 2
                    };

                    await SignInAsync(user);
                    _logger.LogInformation($"用户 {username} 登录成功，IP: {ipAddress}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"登录失败：用户 {username} 密码错误");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"登录过程中发生错误：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("用户已登出");
        }

        /// <summary>
        /// 注册新用户
        /// </summary>
        public bool RegisterUser(User user, string password)
        {
            try
            {
                // 获取客户端IP地址
                string ipAddress = GetClientIPAddress();
                user.RegisterIP = ipAddress;
                
                // 检查用户名是否已存在
                string checkSql = "SELECT COUNT(*) FROM users WHERE username = @username";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@username", user.Username)
                };

                int existingCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkSql, checkParams));
                if (existingCount > 0)
                {
                    _logger.LogWarning($"注册失败：用户名 {user.Username} 已存在");
                    return false;
                }

                // 创建新用户
                string sql = @"
                INSERT INTO users (username, password_hash, display_name, email, role, is_active, register_ip)
                VALUES (@username, @password_hash, @display_name, @email, @role, @is_active, @register_ip)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@username", user.Username),
                    new MySqlParameter("@password_hash", HashPassword(password)),
                    new MySqlParameter("@display_name", user.DisplayName ?? (object)DBNull.Value),
                    new MySqlParameter("@email", user.Email ?? (object)DBNull.Value),
                    new MySqlParameter("@role", (int)user.Role),
                    new MySqlParameter("@is_active", user.IsActive),
                    new MySqlParameter("@register_ip", user.RegisterIP ?? (object)DBNull.Value)
                };

                _dbHelper.ExecuteSql(sql, parameters);
                _logger.LogInformation($"用户 {user.Username} 注册成功，IP: {user.RegisterIP}");
                
                // 获取新用户的ID
                string getUserIdSql = "SELECT id FROM users WHERE username = @username";
                MySqlParameter[] getUserIdParams = {
                    new MySqlParameter("@username", user.Username)
                };
                
                DataTable userDataTable = _dbHelper.Query(getUserIdSql, getUserIdParams);
                if (userDataTable.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(userDataTable.Rows[0]["id"]);
                    
                    // 获取默认的用户组ID
                    string getDefaultGroupSql = "SELECT id FROM user_groups WHERE name = '用户组' LIMIT 1";
                    DataTable groupDataTable = _dbHelper.Query(getDefaultGroupSql);
                    
                    if (groupDataTable.Rows.Count > 0)
                    {
                        int defaultGroupId = Convert.ToInt32(groupDataTable.Rows[0]["id"]);
                        
                        // 将用户添加到默认用户组
                        string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                        MySqlParameter[] addToGroupParams = {
                            new MySqlParameter("@user_id", userId),
                            new MySqlParameter("@group_id", defaultGroupId)
                        };
                        
                        _dbHelper.ExecuteSql(addToGroupSql, addToGroupParams);
                        _logger.LogInformation($"用户 {user.Username} 已添加到默认用户组");
                    }
                    else
                    {
                        _logger.LogWarning($"未找到默认用户组，无法将用户 {user.Username} 添加到用户组");
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注册用户失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public User GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return null;
            }

            return GetUserById(userId);
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        public User GetUserById(int userId)
        {
            try
            {
                string sql = "SELECT * FROM users WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", userId)
                };

                DataTable dt = _dbHelper.Query(sql, parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = dt.Rows[0];
                return new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    Username = row["username"].ToString(),
                    PasswordHash = row["password_hash"].ToString(),
                    DisplayName = row["display_name"]?.ToString(),
                    Email = row["email"]?.ToString(),
                    Role = (UserRole)Convert.ToInt32(row["role"]),
                    IsActive = Convert.ToBoolean(row["is_active"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    LastLoginAt = row["last_login_at"] != DBNull.Value ? Convert.ToDateTime(row["last_login_at"]) : null,
                    RegisterIP = row["register_ip"]?.ToString(),
                    LastLoginIP = row["last_login_ip"]?.ToString(),
                    LicenseGenerationCount = row["license_generation_count"] != DBNull.Value ? Convert.ToInt32(row["license_generation_count"]) : 0,
                    MaxLicenseGenerations = row["max_license_generations"] != DBNull.Value ? Convert.ToInt32(row["max_license_generations"]) : 2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户信息失败：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取用户所属的所有用户组
        /// </summary>
        public List<UserGroup> GetUserGroups(int userId)
        {
            try
            {
                string sql = @"
                SELECT g.* FROM user_groups g
                INNER JOIN user_group_mapping m ON g.id = m.group_id
                WHERE m.user_id = @user_id";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@user_id", userId)
                };

                DataTable dt = _dbHelper.Query(sql, parameters);
                var groups = new List<UserGroup>();

                foreach (DataRow row in dt.Rows)
                {
                    groups.Add(new UserGroup
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        Description = row["description"]?.ToString(),
                        MinimumRole = (UserRole)Convert.ToInt32(row["minimum_role"]),
                        CanGenerateLicense = Convert.ToBoolean(row["can_generate_license"]),
                        CanViewRecords = Convert.ToBoolean(row["can_view_records"]),
                        CanManageUsers = Convert.ToBoolean(row["can_manage_users"]),
                        CreatedAt = Convert.ToDateTime(row["created_at"])
                    });
                }

                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户组失败：{ex.Message}");
                return new List<UserGroup>();
            }
        }

        /// <summary>
        /// 检查当前用户是否有权限生成授权文件
        /// </summary>
        public bool CanCurrentUserGenerateLicense()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            // 超级管理员拥有所有权限
            if (user.Role == UserRole.SuperAdmin)
            {
                return true;
            }

            // 检查用户所属组的权限
            var userGroups = GetUserGroups(user.Id);
            return userGroups.Any(g => g.CanGenerateLicense);
        }

        /// <summary>
        /// 检查用户是否可以生成更多授权文件（未超过最大生成次数）
        /// </summary>
        public bool CheckCanGenerateMoreLicenses(int userId)
        {
            try
            {
                var user = GetUserById(userId);
                if (user == null)
                {
                    _logger.LogWarning($"检查生成次数失败：用户ID {userId} 不存在");
                    return false;
                }

                // 管理员不受生成次数限制
                if (user.Role == UserRole.Admin || user.Role == UserRole.SuperAdmin)
                {
                    return true;
                }

                // 检查是否超过最大生成次数
                return user.LicenseGenerationCount < user.MaxLicenseGenerations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查用户 {userId} 是否可以生成更多授权文件时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 检查当前用户是否可以生成更多授权文件
        /// </summary>
        public bool CanCurrentUserGenerateMoreLicenses()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }
            
            return CheckCanGenerateMoreLicenses(user.Id);
        }

        /// <summary>
        /// 增加用户的授权文件生成次数
        /// </summary>
        public bool IncrementLicenseGenerationCount(int userId)
        {
            try
            {
                string sql = "UPDATE users SET license_generation_count = license_generation_count + 1 WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", userId)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"增加用户 {userId} 的授权文件生成次数时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 重置用户的授权文件生成次数
        /// </summary>
        public bool ResetLicenseGenerationCount(int userId)
        {
            try
            {
                string sql = "UPDATE users SET license_generation_count = 0 WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", userId)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"重置用户 {userId} 的授权文件生成次数时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 设置用户的最大授权文件生成次数
        /// </summary>
        public bool SetMaxLicenseGenerations(int userId, int maxGenerations)
        {
            try
            {
                string sql = "UPDATE users SET max_license_generations = @max_generations WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", userId),
                    new MySqlParameter("@max_generations", maxGenerations)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置用户 {userId} 的最大授权文件生成次数时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 检查当前用户是否有权限查看授权记录
        /// </summary>
        public bool CanCurrentUserViewRecords()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            // 超级管理员拥有所有权限
            if (user.Role == UserRole.SuperAdmin)
            {
                return true;
            }

            // 检查用户所属组的权限
            var userGroups = GetUserGroups(user.Id);
            return userGroups.Any(g => g.CanViewRecords);
        }

        /// <summary>
        /// 检查当前用户是否有权限管理用户
        /// </summary>
        public bool CanCurrentUserManageUsers()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            // 超级管理员拥有所有权限
            if (user.Role == UserRole.SuperAdmin)
            {
                return true;
            }

            // 检查用户所属组的权限
            var userGroups = GetUserGroups(user.Id);
            return userGroups.Any(g => g.CanManageUsers);
        }

        /// <summary>
        /// 检查当前用户是否为管理员或超级管理员
        /// </summary>
        public bool IsCurrentUserAdmin()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            // 检查用户角色是否为管理员或超级管理员
            return user.Role == UserRole.Admin || user.Role == UserRole.SuperAdmin;
        }

        /// <summary>
        /// 检查当前用户是否为超级管理员
        /// </summary>
        public bool IsCurrentUserSuperAdmin()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            // 检查用户角色是否为超级管理员
            return user.Role == UserRole.SuperAdmin;
        }

        /// <summary>
        /// 创建身份认证票据并登录
        /// </summary>
        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (!string.IsNullOrEmpty(user.DisplayName))
            {
                claims.Add(new Claim("DisplayName", user.DisplayName));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        /// <summary>
        /// 对密码进行哈希处理
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        private bool VerifyPassword(string password, string storedHash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == storedHash;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        public DataTable GetAllUsers(int limit = 100, int offset = 0)
        {
            try
            {
                string sql = @"
                SELECT id, username, display_name, email, role, is_active, created_at, last_login_at, 
                       register_ip, last_login_ip 
                FROM users 
                ORDER BY id ASC 
                LIMIT @limit OFFSET @offset";
                
                MySqlParameter[] parameters = {
                    new MySqlParameter("@limit", limit),
                    new MySqlParameter("@offset", offset)
                };

                return _dbHelper.Query(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有用户列表失败");
                throw;
            }
        }

        /// <summary>
        /// 获取用户总数
        /// </summary>
        public int GetTotalUsersCount()
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM users";
                object result = _dbHelper.ExecuteScalar(sql);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户总数失败");
                throw;
            }
        }

        /// <summary>
        /// 更新用户角色
        /// </summary>
        public bool UpdateUserRole(int userId, UserRole newRole)
        {
            try
            {
                // 获取当前用户
                var currentUser = GetCurrentUser();
                if (currentUser == null || !CanCurrentUserManageUsers())
                {
                    _logger.LogWarning("无权限更新用户角色");
                    return false;
                }

                // 超级管理员只能由超级管理员修改
                if (newRole == UserRole.SuperAdmin && currentUser.Role != UserRole.SuperAdmin)
                {
                    _logger.LogWarning("只有超级管理员可以设置超级管理员角色");
                    return false;
                }

                // 管理员只能由超级管理员设置
                if (newRole == UserRole.Admin && currentUser.Role != UserRole.SuperAdmin)
                {
                    _logger.LogWarning("只有超级管理员可以设置管理员角色");
                    return false;
                }

                // 获取目标用户
                var targetUser = GetUserById(userId);
                if (targetUser == null)
                {
                    _logger.LogWarning($"用户ID {userId} 不存在");
                    return false;
                }

                // 不能修改比自己角色高的用户
                if ((int)targetUser.Role > (int)currentUser.Role)
                {
                    _logger.LogWarning("不能修改比自己角色高的用户");
                    return false;
                }

                // 不能将用户角色设置为比自己高的角色
                if ((int)newRole > (int)currentUser.Role)
                {
                    _logger.LogWarning("不能将用户角色设置为比自己高的角色");
                    return false;
                }

                // 开始数据库事务，确保角色更新和用户组更新在一个事务中
                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. 更新用户角色
                            string updateRoleSql = "UPDATE users SET role = @role WHERE id = @id";
                            MySqlCommand updateRoleCmd = new MySqlCommand(updateRoleSql, connection, transaction);
                            updateRoleCmd.Parameters.AddWithValue("@role", (int)newRole);
                            updateRoleCmd.Parameters.AddWithValue("@id", userId);
                            updateRoleCmd.ExecuteNonQuery();

                            // 2. 清除用户原有的用户组关系
                            string clearGroupsSql = "DELETE FROM user_group_mapping WHERE user_id = @user_id";
                            MySqlCommand clearGroupsCmd = new MySqlCommand(clearGroupsSql, connection, transaction);
                            clearGroupsCmd.Parameters.AddWithValue("@user_id", userId);
                            clearGroupsCmd.ExecuteNonQuery();

                            // 3. 根据新角色添加用户到适当的用户组
                            // 查找适合新角色的用户组
                            string findGroupsSql = "SELECT id FROM user_groups WHERE minimum_role <= @role ORDER BY minimum_role DESC LIMIT 1";
                            MySqlCommand findGroupsCmd = new MySqlCommand(findGroupsSql, connection, transaction);
                            findGroupsCmd.Parameters.AddWithValue("@role", (int)newRole);
                            int? groupId = null;
                            using (var reader = findGroupsCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    groupId = reader.GetInt32(0);
                                }
                            }

                            if (groupId.HasValue)
                            {
                                // 添加用户到找到的用户组
                                string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                                MySqlCommand addToGroupCmd = new MySqlCommand(addToGroupSql, connection, transaction);
                                addToGroupCmd.Parameters.AddWithValue("@user_id", userId);
                                addToGroupCmd.Parameters.AddWithValue("@group_id", groupId.Value);
                                addToGroupCmd.ExecuteNonQuery();
                                _logger.LogInformation($"用户 {userId} 已添加到用户组 {groupId.Value}");
                            }
                            else
                            {
                                // 如果没有找到适合的用户组，根据角色创建默认的组关系
                                int defaultGroupId = 0;
                                
                                if (newRole == UserRole.Admin || newRole == UserRole.SuperAdmin)
                                {
                                    // 查找或创建管理员组
                                    string findAdminGroupSql = "SELECT id FROM user_groups WHERE name = '管理员组' LIMIT 1";
                                    MySqlCommand findAdminGroupCmd = new MySqlCommand(findAdminGroupSql, connection, transaction);
                                    using (var reader = findAdminGroupCmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            defaultGroupId = reader.GetInt32(0);
                                        }
                                    }
                                }
                                else
                                {
                                    // 查找或创建普通用户组
                                    string findUserGroupSql = "SELECT id FROM user_groups WHERE name = '用户组' LIMIT 1";
                                    MySqlCommand findUserGroupCmd = new MySqlCommand(findUserGroupSql, connection, transaction);
                                    using (var reader = findUserGroupCmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            defaultGroupId = reader.GetInt32(0);
                                        }
                                    }
                                }
                                
                                if (defaultGroupId > 0)
                                {
                                    // 添加用户到默认用户组
                                    string addToGroupSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                                    MySqlCommand addToGroupCmd = new MySqlCommand(addToGroupSql, connection, transaction);
                                    addToGroupCmd.Parameters.AddWithValue("@user_id", userId);
                                    addToGroupCmd.Parameters.AddWithValue("@group_id", defaultGroupId);
                                    addToGroupCmd.ExecuteNonQuery();
                                    _logger.LogInformation($"用户 {userId} 已添加到默认用户组 {defaultGroupId}");
                                }
                                else
                                {
                                    _logger.LogWarning($"未找到适合用户 {userId} 的用户组");
                                }
                            }

                            // 提交事务
                            transaction.Commit();
                            _logger.LogInformation($"用户 {userId} 角色已更新为 {newRole}，用户组关系已同步更新");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // 回滚事务
                            transaction.Rollback();
                            _logger.LogError(ex, $"更新用户角色和用户组关系失败：{ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新用户角色失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新用户状态（启用/禁用）
        /// </summary>
        public bool UpdateUserStatus(int userId, bool isActive)
        {
            try
            {
                // 获取当前用户
                var currentUser = GetCurrentUser();
                if (currentUser == null || !CanCurrentUserManageUsers())
                {
                    _logger.LogWarning("无权限更新用户状态");
                    return false;
                }

                // 获取目标用户
                var targetUser = GetUserById(userId);
                if (targetUser == null)
                {
                    _logger.LogWarning($"用户ID {userId} 不存在");
                    return false;
                }

                // 不能修改比自己角色高的用户
                if ((int)targetUser.Role > (int)currentUser.Role)
                {
                    _logger.LogWarning("不能修改比自己角色高的用户");
                    return false;
                }

                // 不能禁用自己
                if (userId == currentUser.Id)
                {
                    _logger.LogWarning("不能修改自己的状态");
                    return false;
                }

                string sql = "UPDATE users SET is_active = @is_active WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@is_active", isActive),
                    new MySqlParameter("@id", userId)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                _logger.LogInformation($"用户 {userId} 状态已更新为 {(isActive ? "启用" : "禁用")}");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新用户状态失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        public bool ResetUserPassword(int userId, string newPassword)
        {
            try
            {
                // 获取当前用户
                var currentUser = GetCurrentUser();
                if (currentUser == null || !CanCurrentUserManageUsers())
                {
                    _logger.LogWarning("无权限重置用户密码");
                    return false;
                }

                // 获取目标用户
                var targetUser = GetUserById(userId);
                if (targetUser == null)
                {
                    _logger.LogWarning($"用户ID {userId} 不存在");
                    return false;
                }

                // 不能修改比自己角色高的用户
                if ((int)targetUser.Role > (int)currentUser.Role)
                {
                    _logger.LogWarning("不能修改比自己角色高的用户的密码");
                    return false;
                }

                string sql = "UPDATE users SET password_hash = @password_hash WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@password_hash", HashPassword(newPassword)),
                    new MySqlParameter("@id", userId)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                _logger.LogInformation($"用户 {userId} 密码已重置");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"重置用户密码失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        private string GetClientIPAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            return ipAddress;
        }

        /// <summary>
        /// 获取所有用户名列表
        /// </summary>
        public List<string> GetAllUsernames()
        {
            try
            {
                string sql = "SELECT username FROM users ORDER BY username";
                DataTable result = _dbHelper.Query(sql);
                
                List<string> usernames = new List<string>();
                foreach (DataRow row in result.Rows)
                {
                    usernames.Add(row["username"].ToString());
                }
                
                return usernames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户名列表失败");
                throw;
            }
        }
        
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        public User GetUserByUsername(string username)
        {
            try
            {
                string sql = "SELECT * FROM users WHERE username = @username";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@username", username)
                };
                
                DataTable result = _dbHelper.Query(sql, parameters);
                if (result.Rows.Count > 0)
                {
                    return ConvertDataRowToUser(result.Rows[0]);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户 {username} 信息失败");
                throw;
            }
        }
        
        /// <summary>
        /// 将DataRow转换为User对象
        /// </summary>
        private User ConvertDataRowToUser(DataRow row)
        {
            return new User
            {
                Id = Convert.ToInt32(row["id"]),
                Username = row["username"].ToString(),
                PasswordHash = row["password_hash"].ToString(),
                DisplayName = row["display_name"] == DBNull.Value ? null : row["display_name"].ToString(),
                Email = row["email"] == DBNull.Value ? null : row["email"].ToString(),
                Role = (UserRole)Convert.ToInt32(row["role"]),
                IsActive = Convert.ToBoolean(row["is_active"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                LastLoginAt = row["last_login_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["last_login_at"]),
                RegisterIP = row["register_ip"] == DBNull.Value ? null : row["register_ip"].ToString(),
                LastLoginIP = row["last_login_ip"] == DBNull.Value ? null : row["last_login_ip"].ToString(),
                LicenseGenerationCount = row["license_generation_count"] == DBNull.Value ? 0 : Convert.ToInt32(row["license_generation_count"]),
                MaxLicenseGenerations = row["max_license_generations"] == DBNull.Value ? 2 : Convert.ToInt32(row["max_license_generations"])
            };
        }
    }
} 