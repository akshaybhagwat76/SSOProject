using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using SSOApp.Controllers.UI;
using SSOApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Text;
using SSOApp.Models;

namespace SSOApp.Controllers
{
    public class TenantController : Controller
    {
        // GET: TenantController
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TenantController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Tenants.Where(x => !x.IsDelete)
                .Select(x => new TenantViewModel
                {
                    Id = x.Id,
                    TenantName = x.Name,
                    //FirstName = x.FirstName,
                    //LastName = x.LastName,
                    //UserName = x.UserName,
                    Code = x.Code,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    IsOnHold = x.IsOnHold,
                    //Password = x.Password
                }).ToListAsync();

            return View(result);
        }

        // GET: TenantController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TenantController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTenant(Guid Id, TenantViewModel model)
        {
            try
            {
                var tenant = new Tenant();
                if (Id == Guid.Empty)
                {
                    tenant = new Tenant
                    {
                        Id = Guid.NewGuid(),
                        Name = model.TenantName,
                        Code = model.Code,
                        Email = model.Email,
                        IsActive = true,
                        IsOnHold = false,
                    };
                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();

                    var user = new UserViewModel
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsActive = true,
                        TenanntCode = model.Code,
                        Password= model.Password,
                        SelectedRoles =new List<string> { "Admin"}
                    };

                    using (var client = new HttpClient())
                    {
                        //HTTP POST
                        client.BaseAddress = new Uri("https://localhost:44391/APIUser/saveuser");
                        var json = JsonConvert.SerializeObject(user);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                        string apiResponse = await postTask.Content.ReadAsStringAsync();
                        var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);

                    }
                }
                else
                {
                    var user = _context.Tenants.FirstOrDefault<Tenant>(x => x.Id == Id);
                    user.IsActive = model.IsActive;
                    user.Name = model.TenantName;
                    user.Email = model.Email;
                    user.Code = model.Code;
                    await _context.SaveChangesAsync();
                    var applicationUser = await _userManager.FindByEmailAsync(model.Email);
                    if (applicationUser != null)
                    {
                        applicationUser.UserName = model.UserName;
                        applicationUser.Email = model.Email;
                        applicationUser.FirstName = model.FirstName;
                        applicationUser.LastName = model.LastName;
                        applicationUser.TenantCode = model.Code;
                        await _userManager.UpdateAsync(applicationUser);
                    }
                }                  

               
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                if (Id == Guid.Empty)
                {
                    return View("Edit");
                }
                else
                {
                    return View("Create");
                }
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            string message = string.Empty;
            try
            {
                var tenant = _context.Tenants.FirstOrDefault<Tenant>(x => x.Id == id);
                var selectedTenant = new TenantViewModel
                {
                    Code = tenant.Code,
                    Email = tenant.Email,
                    TenantName = tenant.Name,
                    Id = tenant.Id,

                };

                var user = await _userManager.FindByEmailAsync(selectedTenant.Email);
                selectedTenant.FirstName = user?.FirstName;
                selectedTenant.LastName = user?.LastName;
                selectedTenant.UserName = user?.UserName;
                
                return View(selectedTenant);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("saveuserrole")]
        public async Task<IActionResult> UpdateTanent(TenantViewModel model)
        {
            string message = string.Empty;
            try
            {
                var user = _context.Tenants.FirstOrDefault<Tenant>(x => x.Id == model.Id);
                if (user != null)
                {
                    if (model.Action == "statusUpdate")
                        user.IsActive = model.IsActive;
                    else if (model.Action == "onHoldUpdate")
                        user.IsOnHold = model.IsOnHold;

                    await _context.SaveChangesAsync();

                    message = AccountOptions.API_Response_Saved;
                }
                else
                {
                    message = AccountOptions.API_Response_Invalid_User;
                }

            }
            catch (Exception ex)
            {
                message = AccountOptions.API_Response_Exception;
            }
            return Ok(new
            {
                Status = message
            });
        }
    }
}
