using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    [Table("ModuleField")]

    public class ModuleFieldDetails
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please Enter DBFeild ")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Field should not have space.")]
        public string DBFieldName { get; set; }

        [Required(ErrorMessage = "Please Enter Feild Name")]
        public string FieldLabel { get; set; }

        [Required(ErrorMessage = "Please Select Feild Type")]
        public string FieldType { get; set; }

        public string TenantCode { get; set; }

        public bool visible { get; set; } = true;

       
        public virtual ModuleDetails ModuleDetails { get; set; }
        public Guid ModuleDetailsID { get; set; }

    }



    [Table("ModuleFieldOption")]
    public class ModuleFieldOptions
    {
        [Key]
        public Guid ID { get; set; }
        public String Options { get; set; }
        public Guid ModuleFieldDetailsID { get; set; }
        public ModuleFieldDetails ModuleFieldDetails { get; set; }


    }
}
