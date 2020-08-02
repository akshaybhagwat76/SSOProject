using App.SQLServer.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class SaveModuleClaimViewModel
    {
        public Guid TenantId { get; set; }

        public Guid RoleID { get; set; }

        public Guid ModuleId { get; set; }

        public Guid ClaimID { get; set; }
    }
}
