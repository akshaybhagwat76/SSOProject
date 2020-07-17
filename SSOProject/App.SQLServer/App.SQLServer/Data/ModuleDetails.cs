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
        
        public string LableName { get; set; }

        public string TableName { get; set; }

        public Guid TenantId { get; set; }

    }
}
