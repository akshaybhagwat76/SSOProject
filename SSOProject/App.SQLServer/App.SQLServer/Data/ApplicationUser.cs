using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public bool isDeleted { get; set; }

        [StringLength(50)]
        [Required]
        public string TenantCode { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
