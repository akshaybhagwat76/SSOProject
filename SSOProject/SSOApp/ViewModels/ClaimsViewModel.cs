using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class ClaimsViewModel
    {
        [Required]
        public string Name { get; set; }
        public string ID { get; set; }

        public string TenantName { get; set; }
        public string TenantCode { get; set; }
        public string UserFullName { get; set; }
        public string UserID { get; set; }
    }
}
