using System;
using System.Collections.Generic;
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

        [StringLength(250)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        public virtual IEnumerable<TenantRoles> Roles { get; set; }
        public virtual IEnumerable<TenantClaims> Claims { get; set; }
    }
}
