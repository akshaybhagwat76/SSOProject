using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
        public string ID { get; set; }
        public string TenantName { get; set; }
        public string TenantCode { get; set; }
        public string UserFullName{ get; set; }
        public string UserID { get; set; }
      
    }


    public class TenantRoleModel
    {
        public Guid TennantId { get; set; }
        public Guid ModuleId { get; set; }
        public SelectList RolesUnSelectedList { get; set; }
        public SelectList RolesSelectedList { get; set; }
        public string[] SelectedRoleID { get; set; }
        public string[] UnSelectedRoleID { get; set; }




    }
}
