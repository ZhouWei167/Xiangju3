using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Web.Models.Auth
{
    public class UserGroup
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [StringLength(200)]
        public string Description { get; set; }
        
        public UserRole MinimumRole { get; set; } = UserRole.User;
        
        public bool CanGenerateLicense { get; set; } = false;
        
        public bool CanViewRecords { get; set; } = false;
        
        public bool CanManageUsers { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public List<int> UserIds { get; set; } = new List<int>();
    }
} 