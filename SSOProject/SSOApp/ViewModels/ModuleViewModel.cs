using App.SQLServer.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class ModuleViewModel
    {
        [Required]
        public string ModuleName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage ="Field should not have space.")]
        public string ModuleLabel { get; set; }

        public Guid ID { get; set; }

        public Tenant Tenant { get; set; }

        public string UserFullName { get; set; }

        public string UserID { get; set; }

        public List<TenantClaims> ModuleClaim { get; set; }
    }

    public class ModuleFieldValueListViewModel
    {
        public string ModuleName { get; set; }

        public string ModuleLabel { get; set; }

        public Guid ID { get; set; }

        public System.Data.DataSet List { get;set; }

        public List<TenantClaims> UserClaim { get; set; }

    }
    public class ModuleFieldDetailsVIewModel
    {
        [Key]
        public Guid ID { get; set; }

        public string DBFieldName { get; set; }

        [Required(ErrorMessage = "Please Enter Feild Name")]
        public string FieldLabel { get; set; }

        [Required(ErrorMessage = "Please Select Feild Type")]
        public string FieldType { get; set; }

        public string FieldValue { get; set; }

    }
}
