using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{

    [Table("ModuleDetails")]
    public class ModuleDetails
    {
        [Key]
        public Guid ID { get; set; }

        public string ModuleName { get; set; }

        public string ModuleLabel { get; set; }

        public Guid TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

    }

    [Table("AspNetRoleModuleClaim")]
    public class RoleModuleClaim
    {

        public Guid TenantId { get; set; }

        [Key]
        [ForeignKey("Tenant_Role")]
        public Guid RoleID { get; set; }

        [Key]
        [ForeignKey("Role_Module")]
        public Guid ModuleID { get; set; }

        [Key]
        [ForeignKey("Module_Claim")]
        public Guid ClaimID { get; set; }

        public virtual ModuleDetails Module { get; set; }

    }

}
