-- 添加用户与授权的映射表
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 更新版本号
INSERT INTO `system_settings` (`key`, `value`, `description`, `created_at`, `updated_at`)
VALUES ('db_version', '1.1.0', '数据库版本号', NOW(), NOW())
ON DUPLICATE KEY UPDATE `value` = '1.1.0', `updated_at` = NOW(); 