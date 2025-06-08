using Microsoft.AspNetCore.Mvc;
using Server.Web.Models.Auth;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using Server.Web.Models.License;

namespace Server.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly AuthService _authService;
        private readonly DbHelper _dbHelper;

        public GroupController(ILogger<GroupController> logger, AuthService authService, DbHelper dbHelper)
        {
            _logger = logger;
            _authService = authService;
            _dbHelper = dbHelper;
        }

        /// <summary>
        /// 获取所有用户组
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetAllGroups()
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试获取所有用户组，但没有超级管理员权限");
                    return Forbid();
                }

                var groups = GetUserGroups();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有用户组失败");
                return BadRequest(new { message = $"获取所有用户组失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 创建或更新用户组
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult SaveGroup([FromBody] UserGroup group)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试创建或更新用户组，但没有超级管理员权限");
                    return Forbid();
                }

                if (group == null)
                {
                    return BadRequest(new { message = "无效的用户组数据" });
                }

                // 验证用户组名称
                if (string.IsNullOrWhiteSpace(group.Name))
                {
                    return BadRequest(new { message = "用户组名称不能为空" });
                }

                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (group.Id > 0)
                            {
                                // 更新现有用户组
                                string updateSql = @"
                                UPDATE user_groups 
                                SET name = @name, 
                                    description = @description, 
                                    minimum_role = @minimum_role, 
                                    can_generate_license = @can_generate_license, 
                                    can_view_records = @can_view_records, 
                                    can_manage_users = @can_manage_users 
                                WHERE id = @id";

                                MySqlCommand cmd = new MySqlCommand(updateSql, connection, transaction);
                                cmd.Parameters.AddWithValue("@id", group.Id);
                                cmd.Parameters.AddWithValue("@name", group.Name);
                                cmd.Parameters.AddWithValue("@description", group.Description ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@minimum_role", (int)group.MinimumRole);
                                cmd.Parameters.AddWithValue("@can_generate_license", group.CanGenerateLicense);
                                cmd.Parameters.AddWithValue("@can_view_records", group.CanViewRecords);
                                cmd.Parameters.AddWithValue("@can_manage_users", group.CanManageUsers);

                                cmd.ExecuteNonQuery();
                                _logger.LogInformation($"用户组 {group.Name} (ID: {group.Id}) 已更新");
                            }
                            else
                            {
                                // 创建新用户组
                                string insertSql = @"
                                INSERT INTO user_groups 
                                (name, description, minimum_role, can_generate_license, can_view_records, can_manage_users) 
                                VALUES 
                                (@name, @description, @minimum_role, @can_generate_license, @can_view_records, @can_manage_users);
                                SELECT LAST_INSERT_ID();";

                                MySqlCommand cmd = new MySqlCommand(insertSql, connection, transaction);
                                cmd.Parameters.AddWithValue("@name", group.Name);
                                cmd.Parameters.AddWithValue("@description", group.Description ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@minimum_role", (int)group.MinimumRole);
                                cmd.Parameters.AddWithValue("@can_generate_license", group.CanGenerateLicense);
                                cmd.Parameters.AddWithValue("@can_view_records", group.CanViewRecords);
                                cmd.Parameters.AddWithValue("@can_manage_users", group.CanManageUsers);

                                group.Id = Convert.ToInt32(cmd.ExecuteScalar());
                                _logger.LogInformation($"新用户组 {group.Name} 已创建，ID: {group.Id}");
                            }

                            transaction.Commit();
                            return Ok(new { id = group.Id, message = $"用户组 {group.Name} 已保存" });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError(ex, $"保存用户组失败: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存用户组失败");
                return BadRequest(new { message = $"保存用户组失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult DeleteGroup(int id)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试删除用户组，但没有超级管理员权限");
                    return Forbid();
                }

                if (id <= 0)
                {
                    return BadRequest(new { message = "无效的用户组ID" });
                }

                // 检查是否为系统预设的用户组
                string checkSystemGroupSql = "SELECT name FROM user_groups WHERE id = @id";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@id", id)
                };

                var result = _dbHelper.Query(checkSystemGroupSql, checkParams);
                if (result.Rows.Count == 0)
                {
                    return NotFound(new { message = $"ID为 {id} 的用户组不存在" });
                }

                string groupName = result.Rows[0]["name"].ToString();
                if (groupName == "管理员组" || groupName == "用户组")
                {
                    return BadRequest(new { message = $"不能删除系统预设的用户组: {groupName}" });
                }

                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 删除用户组映射关系
                            string deleteMappingSql = "DELETE FROM user_group_mapping WHERE group_id = @group_id";
                            MySqlCommand deleteMappingCmd = new MySqlCommand(deleteMappingSql, connection, transaction);
                            deleteMappingCmd.Parameters.AddWithValue("@group_id", id);
                            deleteMappingCmd.ExecuteNonQuery();

                            // 删除用户组
                            string deleteGroupSql = "DELETE FROM user_groups WHERE id = @id";
                            MySqlCommand deleteGroupCmd = new MySqlCommand(deleteGroupSql, connection, transaction);
                            deleteGroupCmd.Parameters.AddWithValue("@id", id);
                            deleteGroupCmd.ExecuteNonQuery();

                            transaction.Commit();
                            _logger.LogInformation($"用户组 {groupName} (ID: {id}) 已删除");
                            return Ok(new { message = $"用户组 {groupName} 已删除" });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError(ex, $"删除用户组失败: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除用户组失败");
                return BadRequest(new { message = $"删除用户组失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取用户组详情
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetGroupById(int id)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试获取用户组详情，但没有超级管理员权限");
                    return Forbid();
                }

                if (id <= 0)
                {
                    return BadRequest(new { message = "无效的用户组ID" });
                }

                string sql = "SELECT * FROM user_groups WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", id)
                };

                var result = _dbHelper.Query(sql, parameters);
                if (result.Rows.Count == 0)
                {
                    return NotFound(new { message = $"ID为 {id} 的用户组不存在" });
                }

                var group = ConvertDataRowToUserGroup(result.Rows[0]);
                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户组详情失败");
                return BadRequest(new { message = $"获取用户组详情失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取用户组成员
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult GetGroupMembers(int groupId)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试获取用户组成员，但没有超级管理员权限");
                    return Forbid();
                }

                if (groupId <= 0)
                {
                    return BadRequest(new { message = "无效的用户组ID" });
                }

                string sql = @"
                SELECT u.id, u.username, u.display_name, u.email, u.role
                FROM users u
                INNER JOIN user_group_mapping m ON u.id = m.user_id
                WHERE m.group_id = @group_id
                ORDER BY u.username";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@group_id", groupId)
                };

                var result = _dbHelper.Query(sql, parameters);
                
                // 转换为JSON友好的格式
                var members = new List<Dictionary<string, object>>();
                foreach (DataRow row in result.Rows)
                {
                    var member = new Dictionary<string, object>
                    {
                        ["id"] = Convert.ToInt32(row["id"]),
                        ["username"] = row["username"].ToString(),
                        ["display_name"] = row["display_name"] == DBNull.Value ? null : row["display_name"].ToString(),
                        ["email"] = row["email"] == DBNull.Value ? null : row["email"].ToString(),
                        ["role"] = Convert.ToInt32(row["role"])
                    };
                    members.Add(member);
                }

                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户组成员失败");
                return BadRequest(new { message = $"获取用户组成员失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 添加用户到用户组
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult AddUserToGroup([FromBody] UserGroupMappingRequest request)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试添加用户到用户组，但没有超级管理员权限");
                    return Forbid();
                }

                if (request == null || request.UserId <= 0 || request.GroupId <= 0)
                {
                    return BadRequest(new { message = "无效的请求参数" });
                }

                // 检查用户和用户组是否存在
                string checkUserSql = "SELECT COUNT(*) FROM users WHERE id = @user_id";
                MySqlParameter[] checkUserParams = {
                    new MySqlParameter("@user_id", request.UserId)
                };
                int userCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkUserSql, checkUserParams));
                if (userCount == 0)
                {
                    return NotFound(new { message = $"ID为 {request.UserId} 的用户不存在" });
                }

                string checkGroupSql = "SELECT COUNT(*) FROM user_groups WHERE id = @group_id";
                MySqlParameter[] checkGroupParams = {
                    new MySqlParameter("@group_id", request.GroupId)
                };
                int groupCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkGroupSql, checkGroupParams));
                if (groupCount == 0)
                {
                    return NotFound(new { message = $"ID为 {request.GroupId} 的用户组不存在" });
                }

                // 检查映射是否已存在
                string checkMappingSql = "SELECT COUNT(*) FROM user_group_mapping WHERE user_id = @user_id AND group_id = @group_id";
                MySqlParameter[] checkMappingParams = {
                    new MySqlParameter("@user_id", request.UserId),
                    new MySqlParameter("@group_id", request.GroupId)
                };
                int mappingCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkMappingSql, checkMappingParams));
                if (mappingCount > 0)
                {
                    return BadRequest(new { message = "该用户已在此用户组中" });
                }

                // 添加用户到用户组
                string insertSql = "INSERT INTO user_group_mapping (user_id, group_id) VALUES (@user_id, @group_id)";
                MySqlParameter[] insertParams = {
                    new MySqlParameter("@user_id", request.UserId),
                    new MySqlParameter("@group_id", request.GroupId)
                };
                _dbHelper.ExecuteSql(insertSql, insertParams);

                _logger.LogInformation($"用户 {request.UserId} 已添加到用户组 {request.GroupId}");
                return Ok(new { message = "用户已添加到用户组" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加用户到用户组失败");
                return BadRequest(new { message = $"添加用户到用户组失败: {ex.Message}" });
            }
        }

        /// <summary>
        /// 从用户组中移除用户
        /// </summary>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "ApiAuth,Cookies")]
        public ActionResult RemoveUserFromGroup(int userId, int groupId)
        {
            try
            {
                // 检查是否为超级管理员
                if (!IsSuperAdmin())
                {
                    _logger.LogWarning($"用户 {User.Identity.Name} 尝试从用户组中移除用户，但没有超级管理员权限");
                    return Forbid();
                }

                if (userId <= 0 || groupId <= 0)
                {
                    return BadRequest(new { message = "无效的请求参数" });
                }

                // 检查映射是否存在
                string checkMappingSql = "SELECT COUNT(*) FROM user_group_mapping WHERE user_id = @user_id AND group_id = @group_id";
                MySqlParameter[] checkMappingParams = {
                    new MySqlParameter("@user_id", userId),
                    new MySqlParameter("@group_id", groupId)
                };
                int mappingCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkMappingSql, checkMappingParams));
                if (mappingCount == 0)
                {
                    return NotFound(new { message = "该用户不在此用户组中" });
                }

                // 移除用户从用户组
                string deleteSql = "DELETE FROM user_group_mapping WHERE user_id = @user_id AND group_id = @group_id";
                MySqlParameter[] deleteParams = {
                    new MySqlParameter("@user_id", userId),
                    new MySqlParameter("@group_id", groupId)
                };
                _dbHelper.ExecuteSql(deleteSql, deleteParams);

                _logger.LogInformation($"用户 {userId} 已从用户组 {groupId} 中移除");
                return Ok(new { message = "用户已从用户组中移除" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从用户组中移除用户失败");
                return BadRequest(new { message = $"从用户组中移除用户失败: {ex.Message}" });
            }
        }

        #region 辅助方法

        /// <summary>
        /// 检查当前用户是否为超级管理员
        /// </summary>
        private bool IsSuperAdmin()
        {
            var currentUser = _authService.GetCurrentUser();
            return currentUser != null && currentUser.Role == UserRole.SuperAdmin;
        }

        /// <summary>
        /// 获取所有用户组
        /// </summary>
        private List<UserGroup> GetUserGroups()
        {
            string sql = "SELECT * FROM user_groups ORDER BY name";
            var result = _dbHelper.Query(sql);
            
            var groups = new List<UserGroup>();
            foreach (DataRow row in result.Rows)
            {
                var group = ConvertDataRowToUserGroup(row);
                groups.Add(group);
            }
            
            return groups;
        }

        /// <summary>
        /// 将DataRow转换为UserGroup对象
        /// </summary>
        private UserGroup ConvertDataRowToUserGroup(DataRow row)
        {
            return new UserGroup
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
                MinimumRole = (UserRole)Convert.ToInt32(row["minimum_role"]),
                CanGenerateLicense = Convert.ToBoolean(row["can_generate_license"]),
                CanViewRecords = Convert.ToBoolean(row["can_view_records"]),
                CanManageUsers = Convert.ToBoolean(row["can_manage_users"]),
                CreatedAt = Convert.ToDateTime(row["created_at"])
            };
        }

        #endregion
    }

    /// <summary>
    /// 用户组映射请求模型
    /// </summary>
    public class UserGroupMappingRequest
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
} 