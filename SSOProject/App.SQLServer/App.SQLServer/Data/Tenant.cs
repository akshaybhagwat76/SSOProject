using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("AspNetTenants")]
    public class Tenant
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Code { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DefaultValue(false)]
        public bool IsOnHold { get; set; } = false;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        public virtual IEnumerable<TenantRoles> Roles { get; set; }

        public virtual IEnumerable<TenantClaims> Claims { get; set; }
    }
}
