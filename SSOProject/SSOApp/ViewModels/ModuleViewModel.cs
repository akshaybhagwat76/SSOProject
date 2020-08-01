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
}
