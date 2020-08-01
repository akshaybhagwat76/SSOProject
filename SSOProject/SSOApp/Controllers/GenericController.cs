using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc;

namespace SSOApp.Controllers
{
    public class GenericController : Home.BaseController
    {
        public GenericController(ApplicationDbContext context) : base(context)
        {
        }
        public IActionResult Index()
        {
            var module = _context.ModuleDetails.FirstOrDefault(x => x.ID == new Guid("9D0486BB-6464-4F30-A64D-028B00B6C00A"));
            var modulefield= _context.ModuleFieldDetails.Where(x => x.ModuleDetailsID == module.ID).ToList();
            var claimDetails = _context.RoleModuleClaim.Where(x => x.ModuleID.ToString() == "9D0486BB-6464-4F30-A64D-028B00B6C00A" &&
            x.TenantId == TenantId && x.RoleID.ToString() == RoleId).ToList();
            var genericFormViewModel = new GenericFormViewModel
            {
                ModuleId = module.ID.ToString(),
                ModuleName = module.ModuleName,
                Options = new List<ModuleFieldOptions>(),
                Fields= modulefield,
                Claims= claimDetails
            };
            foreach(var field in modulefield)
            {                
                var options= _context.ModuleFieldOptions.Where(x => x.ModuleFieldDetailsID == field.ID).ToList();
                genericFormViewModel.Options.AddRange(options);
            }
            return View(genericFormViewModel);
        }

    }

    public class GenericFormViewModel
    {
        public string ModuleId { get; set; }

        public string ModuleName { get; set; }
        
        public List<ModuleFieldDetails> Fields { get; set; }

        public List<ModuleFieldOptions> Options { get; set; }

        public List<RoleModuleClaim> Claims { get; set; }
    }
}
