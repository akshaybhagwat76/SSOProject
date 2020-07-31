using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("AspNetTenantClaims")]
    public class TenantClaims
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public Guid TenantID { get; set; }
        
        [StringLength(500)]
        public string ClaimName { get; set; }

        public bool IsAvailable { get; set; }
    }
}
