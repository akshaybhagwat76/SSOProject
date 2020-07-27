using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SSOApp.ViewModels
{
    public class TenantViewModel
    {

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string TenantName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsOnHold { get; set; }

        public bool IsActive { get; set; }

        public string Action { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
