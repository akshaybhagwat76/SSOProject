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
        public List<string> SelectedUsers { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public Tenant  Tenant { get; set; }
        public string TenantCode { get; set; }
        public List<UserViewModel> GetUserByTCode { get; set; }
    }

    public class UserRoleViewModel
    {
        public List<RoleViewModel> AvaialbleRoles { get; set; }

        public List<RoleViewModel> CurrentRoles { get; set; }

        public string SelectedUserID { get; set; }

    }

    public class RoletoUserViewModel
    {
        public List<ApplicationUser> AvaialbleUser { get; set; }

        public List<ApplicationUser> CurrentUser { get; set; }

        public string SelectedRoleId { get; set; }

    }
}
