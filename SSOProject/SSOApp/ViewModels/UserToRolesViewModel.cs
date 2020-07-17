using App.SQLServer.Data;
using System.Collections.Generic;

namespace SSOApp.ViewModels
{
    public class UserToRolesViewModel
    {
        public string UserID { get; set; }

        public string RoleName { get; set; }

        public string RoleID { get; set; }
        public List<string> Roles { get; set; }
        public List<string> UsersCheckbox { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public Tenant  Tenant { get; set; }
        public string TenantCode { get; set; }
        public List<UserViewModel> GetUserByTCode { get; set; }
    }
}
