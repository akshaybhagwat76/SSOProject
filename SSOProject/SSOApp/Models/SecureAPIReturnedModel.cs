using App.SQLServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.Models
{
    public class SecureAPIReturnedModel
    {
        public ApplicationUser User { get; set; }
        public Tenant Tenant { get; set; }

        public bool UserInRole { get; set; }
    }
}
