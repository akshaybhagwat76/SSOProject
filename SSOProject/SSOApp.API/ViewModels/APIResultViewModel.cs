using App.SQLServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.API.ViewModels
{
    public class APIResultViewModel
    {
        public ApplicationUser User { get; set; }
        public bool IsAdmin { get; set; }
        public string Roles { get; set; }

    }
}
