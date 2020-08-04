using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.API.ViewModels
{
    public class FieldViewModel
    {
        public int ID { get; set; }
        public string Feilds { get; set; }
        public string FeildTypeId { get; set; }
        public bool visible { get; set; }
        public SelectList FieldType { get; set; }
        public string TenantName { get; set; }
        public string TenantCode { get; set; }

        public string Module { get; set; }

    }
    public class FeildModelView
    {
        public string TenantCode { get; set; }
        public string TenantName { get; set; }
        public string ModuleId { get; set; }
        public SelectList FieldType { get; set; }
        public List<ModuleFieldDetails> ModuleFieldDetailsList { get; set; }

        public ModuleFieldDetails ModuleFieldDetails { get; set; }
        public List<ModuleFieldOptions> ModuleFieldOption { get; set; }
    }

}
