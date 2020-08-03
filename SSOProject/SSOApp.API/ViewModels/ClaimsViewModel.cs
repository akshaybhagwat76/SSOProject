using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.API.ViewModels
{
    public class ClaimsViewModel
    {
        [Required]
        public string Name { get; set; }
        public Guid ID { get; set; }

        public Guid TenantID { get; set; }

        public string UserFullName { get; set; }
        public string UserID { get; set; }

        public bool IsActive { get; set; }
    }
}
