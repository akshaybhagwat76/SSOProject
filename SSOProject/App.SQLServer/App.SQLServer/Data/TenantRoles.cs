using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("AspNetTenantRoles")]
    public class TenantRoles
    {
        [Key]
        [ForeignKey("Tenant_Roles")]
        public Guid TenantID { get; set; }
        
        [Key]
        [StringLength(450)]
        [ForeignKey("Roles_Tenant")]
        public string RoleID { get; set; }

        public virtual IdentityRole Role { get; set; }


    }
}
