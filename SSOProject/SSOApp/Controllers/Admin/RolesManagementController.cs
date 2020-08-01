using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSOApp.Controllers.Home;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class RolesManagementController : BaseController
    {
        public RolesManagementController(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IActionResult> Index()
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            var getroles = new List<RoleViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + TenantCode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            return View(getroles);
        }

        public IActionResult Create()
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            var getroles = new RoleViewModel();
            return View(getroles);
        }

        public async Task<IActionResult> Edit(string rid)
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getrolebyid?id=" + rid + "&tcode=" + TenantCode);
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
            if (ModelState.IsValid)
            {
                model.TenantCode = TenantCode;
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

        public async Task<IActionResult> UserIndex(string selectedUserid)
        {
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == selectedUserid);
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) User: {selectedUser.FirstName} {selectedUser.LastName}";
            
            UserRoleViewModel userRoleView = new UserRoleViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + TenantCode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                userRoleView.AvaialbleRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("https://localhost:44391/APIRoles/getrolesbyuser?ID=" + selectedUserid);
                var postTask1 = await client1.GetAsync(client1.BaseAddress);
                var apiResponse1 = await postTask1.Content.ReadAsStringAsync();
                userRoleView.CurrentRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse1);
                var role = userRoleView.AvaialbleRoles.Except(userRoleView.CurrentRoles).ToList();
                userRoleView.AvaialbleRoles = role;
            }
            userRoleView.SelectedUserID = selectedUserid;
            return View(userRoleView);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRoles(IFormCollection formCollection)
        {
            var selectedRoles = formCollection["myData"][0].Split(",");
            var selectedUser = formCollection["selectedUser"][0];
            List<string> chkRoles = new List<string>();
            chkRoles.AddRange(selectedRoles);

            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/saveuserrole");
                var data = new UserToRolesViewModel()
                {
                    UserID = selectedUser,
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
            return RedirectToAction("UserIndex",new { selectedUserid = selectedUser });
        }

       
    }
}