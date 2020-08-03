using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dataAccessService;

        public NavigationMenuViewComponent(ApplicationDbContext dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var module = await _dataAccessService.ModuleDetails.Select(x => new ModuleViewModel
            {
                ModuleName = x.ModuleName,
                ID = x.ID,
                ModuleLabel = x.ModuleLabel
            }).ToListAsync();

            return View(module);
        }
    }
}
