using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("AspNetRoleModule")]
    public class RoleModules
    {
        [Key]
        [ForeignKey("Tenant_RoleModule")]
        public Guid TenantId { get; set; }

        [Key]
        [ForeignKey("Module_Roles")]
        public Guid ModuleID { get; set; }

        [Key]
        [StringLength(450)]
        [ForeignKey("Roles_Tenant")]
        public string RoleID { get; set; }

        public virtual IdentityRole Role { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
