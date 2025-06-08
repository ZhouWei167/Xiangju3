using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Web.Models.Auth
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }
        
        [StringLength(100)]
        public string DisplayName { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        public UserRole Role { get; set; } = UserRole.User;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastLoginAt { get; set; }
        
        /// <summary>
        /// 已生成的授权文件次数
        /// </summary>
        public int LicenseGenerationCount { get; set; } = 0;
        
        /// <summary>
        /// 允许生成的授权文件最大次数（仅对普通用户生效，管理员无限制）
        /// </summary>
        public int MaxLicenseGenerations { get; set; } = 2;

        /// <summary>
        /// 用户注册时的IP地址
        /// </summary>
        [StringLength(50)]
        public string RegisterIP { get; set; }
        
        /// <summary>
        /// 用户最后登录时的IP地址
        /// </summary>
        [StringLength(50)]
        public string LastLoginIP { get; set; }
    }
} 