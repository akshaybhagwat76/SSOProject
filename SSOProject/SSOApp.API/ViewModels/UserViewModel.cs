using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string UserName { get; set; }

        //TODO: Uncomment tag
        // [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string NewPassword { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        //[Required]
        public string TenanntCode { get; set; }
        public string TenanntName { get; set; }
        public List<Tenant> Tenants { get; set; }
        public bool IsLoggedIn { get; set; }

        public List<int> ConcurrentLogin { get; set; }

        public List<SelectListItem> DDRoleList { get; set; }
        public List<SelectListItem> DDTenantist { get; set; }
        public List<string> SelectedRoles { get; set; }

        public bool Saved { get; set; }
    }

    public class UserPasswordViewModel
    {
        public string UserID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }
        //TODO: Uncomment tag
        [Required]
        public string ConfirmPassword { get; set; }


        [Compare("Password")]
        public string NewPassword { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        //[Required]
        public string TenanntCode { get; set; }
        public string TenanntName { get; set; }
        public List<Tenant> Tenants { get; set; }
        public bool IsLoggedIn { get; set; }

        public List<SelectListItem> DDRoleList { get; set; }
        public List<SelectListItem> DDTenantist { get; set; }
        public List<string> SelectedRoles { get; set; }

        public bool Saved { get; set; }
    }

    public class DeleteUserModel
    {
        public string UserID { get; set; }
    }
}
