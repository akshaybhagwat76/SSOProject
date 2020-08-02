using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using App.SQLServer.Data;
using SSOApp.Models;
using Microsoft.EntityFrameworkCore;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.UI
{
    [SecurityHeaders]
    [Authorize]

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SecureAPIReturnedModel();
            //Secure API Calls Initiate Logsin and get back data.
            var getuser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (getuser.TenantCode == "ABCO")
            {
                return RedirectToAction("Index", "Tenant");
            }
            model = await SetupApp.SecureAPIGetUser("Admin", getuser.Id);
            ViewBag.Roles = await _userManager.GetRolesAsync(getuser);

            return View(model);
        }

        /// <summary>
        /// Shows the error page
        /// </summary>

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
