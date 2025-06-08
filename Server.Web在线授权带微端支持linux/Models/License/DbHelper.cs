using System;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Server.Web.Models.License
{
    public class DbHelper
    {
        private readonly string _connectionString;
        private readonly ILogger<DbHelper> _logger;

        public DbHelper(IConfiguration configuration, ILogger<DbHelper> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
            _logger = logger;
        }

        /// <summary>
        /// 初始化数据库，确保表存在
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                // 创建授权记录表
                string createLicenseTable = @"
                CREATE TABLE IF NOT EXISTS `license_records` (
                  `id` int NOT NULL AUTO_INCREMENT,
                  `hardware_id` varchar(50) NOT NULL,
                  `customer_name` varchar(100) DEFAULT NULL,
                  `product_name` varchar(100) DEFAULT NULL,
                  `expiration_date` datetime NOT NULL,
                  `issue_date` datetime NOT NULL,
                  `number_of_uses` int NOT NULL DEFAULT 0,
                  `number_of_instances` int NOT NULL DEFAULT 1,
                  `evaluation_time` int NOT NULL DEFAULT 0,
                  `evaluation_type` int NOT NULL DEFAULT 0,
                  `version` varchar(50) DEFAULT NULL,
                  `enable_evaluation_lock` tinyint(1) NOT NULL DEFAULT 0,
                  `enable_expiration_date_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_number_of_uses_lock` tinyint(1) NOT NULL DEFAULT 0,
                  `enable_number_of_instances_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `enable_hardware_lock` tinyint(1) NOT NULL DEFAULT 1,
                  `custom_data` text DEFAULT NULL,
                  `created_by` varchar(50) DEFAULT NULL,
                  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  PRIMARY KEY (`id`),
                  KEY `idx_hardware_id` (`hardware_id`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";

                ExecuteSql(createLicenseTable);
                _logger.LogInformation("授权记录表初始化成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化数据库失败");
                throw;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        public int ExecuteSql(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException ex)
                    {
                        _logger.LogError(ex, $"执行SQL失败: {sql}");
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 查询数据并返回DataTable
        /// </summary>
        public DataTable Query(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        DataTable dt = new DataTable();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                    catch (MySqlException ex)
                    {
                        _logger.LogError(ex, $"查询数据失败: {sql}");
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询并返回单个值
        /// </summary>
        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        object result = cmd.ExecuteScalar();
                        return result == DBNull.Value ? null : result;
                    }
                    catch (MySqlException ex)
                    {
                        _logger.LogError(ex, $"执行查询失败: {sql}");
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建数据库连接失败");
                throw;
            }
        }
    }
} 