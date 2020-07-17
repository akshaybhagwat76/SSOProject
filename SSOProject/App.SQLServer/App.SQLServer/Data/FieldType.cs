using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("FieldType")]
    public class FieldType
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(500)]
        public string Name { get; set; }
    }
}
