using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.Controllers.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SSOApp
{
    public class ModuleViewComponant:ViewComponent
    {
        private readonly ApplicationDbContext db;

        public ModuleViewComponant(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }
        private async Task<List<ModuleDetails>> GetItemsAsync()
        {
            return await  db.ModuleDetails.ToListAsync();
        }
    }
}
