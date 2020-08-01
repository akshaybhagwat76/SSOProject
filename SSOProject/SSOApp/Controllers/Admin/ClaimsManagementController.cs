using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SSOApp.Controllers.Home;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    public class ClaimsManagementController : BaseController
    {
        public readonly ApplicationDbContext _myContext;
        public ClaimsManagementController(ApplicationDbContext context) : base(context)
        {
            _myContext = context;
        }

        public async Task<IActionResult> Index()
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            var getroles = new List<ClaimsViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIClaims/getallclaimssbytenant?tcode=" + TenantId);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<ClaimsViewModel>>(apiResponse);
            }
            return View(getroles);
        }

        public IActionResult Create()
        {
            var getclaims = new ClaimsViewModel();
            return View(getclaims);
        }

        public async Task<IActionResult> Edit(string cid, string tcode = null)
        {
            var getroles = new ClaimsViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIClaims/getclaimbyid?id=" + cid + "&tcode=" + tcode);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    getroles = JsonConvert.DeserializeObject<ClaimsViewModel>(apiResponse);
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View(getroles);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ClaimsViewModel model)
        {
            model.TenantID = TenantId;
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //HTTP POST
                    try
                    {

                        client.BaseAddress = new Uri("https://localhost:44391/APIClaims/saveclaim");
                        var json = JsonConvert.SerializeObject(model);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                        string apiResponse = await postTask.Content.ReadAsStringAsync();
                        var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                        TempData["Success"] = AccountOptions.API_Response_Saved;
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIClaims/deleteclaim");
                ClaimsViewModel model = new ClaimsViewModel { Name = "NotRequired", ID = new Guid(id) };
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                string apiResponse = await postTask.Content.ReadAsStringAsync();
                var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                if (resultjson.status == AccountOptions.API_Response_Deleted)
                    TempData["Success"] = AccountOptions.API_Response_Deleted;
                else
                    TempData["Failed"] = AccountOptions.API_Response_Failed;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetModuleClaim(string roleid)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == roleid);
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) Role: {role.Name}";
            //Get All Module
            var moduleList = new List<ModuleViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIClaims/getmoduleclaimbyrole?roleId=" + roleid + "&tenantId=" + TenantId);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                moduleList = JsonConvert.DeserializeObject<List<ModuleViewModel>>(apiResponse);
            }
            TempData["Roleid"] = roleid;
            return View("ModuleClaim", moduleList);
        }

        public async Task<IActionResult> SaveModuleClaim(IFormCollection formData)
        {
            var selectedClaim = formData["selctedClaim"][0].Split(",");
            var selectedRole = formData["roleID"][0];
            var lstModuleClaim = new List<RoleModuleClaim>();
            foreach (var item in selectedClaim)
            {
                RoleModuleClaim saveModuleClaimViewModel = new RoleModuleClaim
                {
                    TenantId = TenantId,
                    RoleID = new Guid(selectedRole),
                    ModuleID = new Guid(item.Split("_")[0]),
                    ClaimID = new Guid(item.Split("_")[1])
                };
                lstModuleClaim.Add(saveModuleClaimViewModel);
            }

            using (var client = new HttpClient())
            {
                //HTTP POST
                try
                {

                    client.BaseAddress = new Uri("https://localhost:44391/APIClaims/SaveModuleClaim");
                    var json = JsonConvert.SerializeObject(lstModuleClaim);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                    TempData["Success"] = resultjson.status;
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Error Occured";
                }
            }

            return RedirectToAction("GetModuleClaim", new { roleid = selectedRole });
        }
    }
}