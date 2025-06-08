using System;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Server.Web.Models.License
{
    public class LicenseRecordService
    {
        private readonly DbHelper _dbHelper;
        private readonly ILogger<LicenseRecordService> _logger;

        public LicenseRecordService(DbHelper dbHelper, ILogger<LicenseRecordService> logger)
        {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        /// <summary>
        /// 初始化授权记录相关数据库表
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                // 创建授权记录表
                string createLicenseRecordsTable = @"
                CREATE TABLE IF NOT EXISTS `license_records` (
                  `id` int NOT NULL AUTO_INCREMENT,
                  `hardware_id` varchar(100) NOT NULL,
                  `customer_name` varchar(100) DEFAULT NULL,
                  `product_name` varchar(100) DEFAULT NULL,
                  `expiration_date` datetime NOT NULL,
                  `issue_date` datetime NOT NULL,
                  `number_of_uses` int NOT NULL DEFAULT 0,
                  `number_of_instances` int NOT NULL DEFAULT 1,
                  `evaluation_time` int NOT NULL DEFAULT 0,
                  `evaluation_type` int NOT NULL DEFAULT 0,
                  `version` varchar(50) DEFAULT NULL,
                  `enable_evaluation_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_expiration_date_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_number_of_uses_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_number_of_instances_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_hardware_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `custom_data` text DEFAULT NULL,
                  `created_by` varchar(100) DEFAULT NULL,
                  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  PRIMARY KEY (`id`),
                  KEY `idx_hardware_id` (`hardware_id`),
                  KEY `idx_created_by` (`created_by`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";

                _dbHelper.ExecuteSql(createLicenseRecordsTable);
                
                // 创建用户授权映射表
                string createUserLicenseMappingTable = @"
                CREATE TABLE IF NOT EXISTS `user_license_mapping` (
                  `id` int NOT NULL AUTO_INCREMENT,
                  `user_id` int NOT NULL,
                  `license_id` int NOT NULL,
                  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  PRIMARY KEY (`id`),
                  UNIQUE KEY `idx_user_license` (`user_id`, `license_id`),
                  KEY `idx_user_id` (`user_id`),
                  KEY `idx_license_id` (`license_id`),
                  CONSTRAINT `fk_user_license_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
                  CONSTRAINT `fk_user_license_license` FOREIGN KEY (`license_id`) REFERENCES `license_records` (`id`) ON DELETE CASCADE
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";
                
                _dbHelper.ExecuteSql(createUserLicenseMappingTable);
                
                _logger.LogInformation("授权记录相关表初始化成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化授权记录数据库失败");
                throw;
            }
        }

        /// <summary>
        /// 保存授权记录到数据库
        /// </summary>
        public int SaveLicenseRecord(LicenseGenerator.LicenseInfo licenseInfo, string createdBy = null)
        {
            try
            {
                string sql = @"
                INSERT INTO license_records (
                    hardware_id, customer_name, product_name, expiration_date, issue_date,
                    number_of_uses, number_of_instances, evaluation_time, evaluation_type, version,
                    enable_evaluation_lock, enable_expiration_date_lock, enable_number_of_uses_lock,
                    enable_number_of_instances_lock, enable_hardware_lock, custom_data, created_by
                ) VALUES (
                    @hardware_id, @customer_name, @product_name, @expiration_date, @issue_date,
                    @number_of_uses, @number_of_instances, @evaluation_time, @evaluation_type, @version,
                    @enable_evaluation_lock, @enable_expiration_date_lock, @enable_number_of_uses_lock,
                    @enable_number_of_instances_lock, @enable_hardware_lock, @custom_data, @created_by
                )";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@hardware_id", licenseInfo.HardwareID),
                    new MySqlParameter("@customer_name", licenseInfo.CustomerName ?? (object)DBNull.Value),
                    new MySqlParameter("@product_name", licenseInfo.ProductName ?? (object)DBNull.Value),
                    new MySqlParameter("@expiration_date", licenseInfo.ExpirationDate),
                    new MySqlParameter("@issue_date", licenseInfo.IssueDate),
                    new MySqlParameter("@number_of_uses", licenseInfo.NumberOfUses),
                    new MySqlParameter("@number_of_instances", licenseInfo.NumberOfInstances),
                    new MySqlParameter("@evaluation_time", licenseInfo.EvaluationTime),
                    new MySqlParameter("@evaluation_type", (int)licenseInfo.EvaluationType),
                    new MySqlParameter("@version", licenseInfo.Version ?? (object)DBNull.Value),
                    new MySqlParameter("@enable_evaluation_lock", licenseInfo.EnableEvaluationLock),
                    new MySqlParameter("@enable_expiration_date_lock", licenseInfo.EnableExpirationDateLock),
                    new MySqlParameter("@enable_number_of_uses_lock", licenseInfo.EnableNumberOfUsesLock),
                    new MySqlParameter("@enable_number_of_instances_lock", licenseInfo.EnableNumberOfInstancesLock),
                    new MySqlParameter("@enable_hardware_lock", licenseInfo.EnableHardwareLock),
                    new MySqlParameter("@custom_data", JsonConvert.SerializeObject(licenseInfo.CustomData)),
                    new MySqlParameter("@created_by", createdBy ?? (object)DBNull.Value)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                _logger.LogInformation($"授权记录已保存，硬件ID: {licenseInfo.HardwareID}, 创建者: {createdBy}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存授权记录失败，硬件ID: {licenseInfo.HardwareID}");
                throw;
            }
        }

        /// <summary>
        /// 根据硬件ID获取授权记录
        /// </summary>
        public DataTable GetLicenseRecordsByHardwareId(string hardwareId)
        {
            try
            {
                string sql = "SELECT * FROM license_records WHERE hardware_id = @hardware_id ORDER BY created_at DESC";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@hardware_id", hardwareId)
                };

                return _dbHelper.Query(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取授权记录失败，硬件ID: {hardwareId}");
                throw;
            }
        }

        /// <summary>
        /// 获取所有授权记录
        /// </summary>
        public DataTable GetAllLicenseRecords(int limit = 100, int offset = 0)
        {
            try
            {
                string sql = "SELECT * FROM license_records ORDER BY created_at DESC LIMIT @limit OFFSET @offset";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@limit", limit),
                    new MySqlParameter("@offset", offset)
                };

                return _dbHelper.Query(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有授权记录失败");
                throw;
            }
        }

        /// <summary>
        /// 获取授权记录总数
        /// </summary>
        public int GetTotalLicenseRecordsCount()
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM license_records";
                object result = _dbHelper.ExecuteScalar(sql);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取授权记录总数失败");
                throw;
            }
        }

        /// <summary>
        /// 删除授权记录
        /// </summary>
        public int DeleteLicenseRecord(int id)
        {
            try
            {
                string sql = "DELETE FROM license_records WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", id)
                };

                int result = _dbHelper.ExecuteSql(sql, parameters);
                _logger.LogInformation($"授权记录已删除，ID: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除授权记录失败，ID: {id}");
                throw;
            }
        }

        /// <summary>
        /// 根据创建者获取授权记录
        /// </summary>
        public DataTable GetLicenseRecordsByCreator(string createdBy, int limit = 100, int offset = 0)
        {
            try
            {
                // 获取用户直接创建的授权记录
                string directSql = @"
                SELECT r.* 
                FROM license_records r
                WHERE r.created_by = @created_by 
                
                UNION
                
                -- 获取管理员为此用户创建的授权记录
                SELECT r.* 
                FROM license_records r
                INNER JOIN user_license_mapping m ON r.id = m.license_id
                INNER JOIN users u ON m.user_id = u.id
                WHERE u.username = @created_by
                
                ORDER BY created_at DESC
                LIMIT @limit OFFSET @offset";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@created_by", createdBy),
                    new MySqlParameter("@limit", limit),
                    new MySqlParameter("@offset", offset)
                };

                return _dbHelper.Query(directSql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取创建者为 {createdBy} 的授权记录失败");
                throw;
            }
        }

        /// <summary>
        /// 获取创建者的授权记录总数
        /// </summary>
        public int GetTotalLicenseRecordsCountByCreator(string createdBy)
        {
            try
            {
                string countSql = @"
                SELECT COUNT(*) FROM (
                    -- 用户直接创建的授权记录
                    SELECT r.id 
                    FROM license_records r
                    WHERE r.created_by = @created_by 
                    
                    UNION
                    
                    -- 管理员为此用户创建的授权记录
                    SELECT r.id 
                    FROM license_records r
                    INNER JOIN user_license_mapping m ON r.id = m.license_id
                    INNER JOIN users u ON m.user_id = u.id
                    WHERE u.username = @created_by
                ) AS combined_records";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@created_by", createdBy)
                };
                
                object result = _dbHelper.ExecuteScalar(countSql, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取创建者为 {createdBy} 的授权记录总数失败");
                throw;
            }
        }
    }
} 