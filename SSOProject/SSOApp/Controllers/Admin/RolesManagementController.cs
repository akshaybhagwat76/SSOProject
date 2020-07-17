using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class RolesManagementController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await BindTenantDD();
            var getroles = new List<RoleViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallroles");
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            return View(getroles);
        }
        private async Task BindTenantDD()
        {
            using (var client = new HttpClient())
            {
                //getallusers
                client.BaseAddress = new Uri("https://localhost:44391/APITenant/getddtenant");
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                ViewBag.TenantDD = JsonConvert.DeserializeObject<List<SelectListItem>>(apiResponse);
            }
        }
        public async Task<IActionResult> GetRolesByTenant(string tcode)
        {
            var getroles = new List<RoleViewModel>();

            using (var client = new HttpClient())
            {
                //getallusers
                if (!string.IsNullOrEmpty(tcode))
                {
                    //Select by code
                    client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + tcode);
                }
                else
                {
                    //Select All
                    client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallroles");
                }
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            return PartialView("_RolesGrid", getroles);
        }
        public IActionResult Create()
        {
            var getroles = new RoleViewModel();
            return View(getroles);
        }
        public async Task<IActionResult> Edit(string rid, string tcode = null)
        {
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getrolebyid?id=" + rid + "&tcode=" + tcode);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    getroles = JsonConvert.DeserializeObject<RoleViewModel>(apiResponse);
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View(getroles);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //HTTP POST
                    client.BaseAddress = new Uri("https://localhost:44391/APIRoles/saverole");
                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                    if (resultjson.status == AccountOptions.API_Response_Saved)
                    {
                        TempData["Success"] = AccountOptions.API_Response_Saved;
                        return RedirectToAction("Index");
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
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/deleterole");
                RoleViewModel model = new RoleViewModel { Name = "NotRequired", ID = id };
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
        public async Task<IActionResult> UserIndex(string id, string tcode, string tname)
        {
            ViewBag.UserID = id;
            ViewBag.TenantCode = tcode;
            ViewBag.TenantName = tname;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + tcode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                ViewBag.Roles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("https://localhost:44391/APIRoles/getrolesbyuser?ID=" + id);
                var postTask1 = await client1.GetAsync(client1.BaseAddress);
                var apiResponse1 = await postTask1.Content.ReadAsStringAsync();
                var userRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse1);
                ViewBag.UserRoles = userRoles;
            }
            return View();

        }

        public async Task<IActionResult> SaveUserRoles(IFormCollection formCollection)
        {
            string hdnUserID =formCollection["hdnUserId"];
            var dd = formCollection["myData"][0].Split(",");
            List<string> chkRoles=new List<string>();
            chkRoles.AddRange(dd);
            //chkRoles.AddRange();
            //chkRoles = dd;
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/saveuserrole");
                var data = new UserToRolesViewModel()
                {
                    UserID = hdnUserID,
                    Roles = chkRoles
                };
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                string apiResponse = await postTask.Content.ReadAsStringAsync();
                var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                if (resultjson.status == AccountOptions.API_Response_Saved)
                    TempData["Success"] = AccountOptions.API_Response_Saved;
                else
                    TempData["Failed"] = AccountOptions.API_Response_Failed;
            }
            return RedirectToAction("UserIndex", new { id = hdnUserID });
        }
    }
}