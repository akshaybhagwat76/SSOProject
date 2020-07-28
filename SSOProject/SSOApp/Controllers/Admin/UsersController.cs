using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var getusers = new List<UserViewModel>();
            await BindTenantDD();         
            return View(getusers);
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
        public async Task<IActionResult> Create()
        {
            var getroles = new UserViewModel();
            await BindDropdowns(getroles);

            return View(getroles);
        }
        private async Task BindDropdowns(UserViewModel model)
        {
            using (var client = new HttpClient())
            {
                //getallusers
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getddroles");
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                model.DDRoleList = JsonConvert.DeserializeObject<List<SelectListItem>>(apiResponse);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            var result = await Save(model);
            if (result.Saved)
            {
                TempData["Success"] = AccountOptions.API_Response_Saved;
                return RedirectToAction("Index");
            }
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            await BindDropdowns(model);
            return View(result);
        }
        public async Task<IActionResult> GetUsersByTenant(string tcode)
        {
            var getusers = new List<UserViewModel>();

            using (var client = new HttpClient())
            {
                //getallusers
                if (!string.IsNullOrEmpty(tcode))
                {
                    //Select by code
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbytenant?code=" + tcode);
                }
                else
                {
                    //Select All
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/getallusers");
                }
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getusers = JsonConvert.DeserializeObject<List<UserViewModel>>(apiResponse);
            }
            return PartialView("_UserGrid", getusers);
        }
        public async Task<IActionResult> DeleteRole(string rolename, string userid)
        {
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/deletuserfromerole");
                var data = new UserToRolesViewModel()
                {
                    RoleName = rolename,
                    UserID = userid
                };
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                string apiResponse = await postTask.Content.ReadAsStringAsync();
                var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                if (resultjson.status == AccountOptions.API_Response_Success)
                    TempData["Success"] = AccountOptions.API_Response_Success;
                else
                    TempData["Failed"] = AccountOptions.API_Response_Failed;
            }
            return RedirectToAction("UserIndex", "RolesManagement", new { id = userid });
        }
        public async Task<IActionResult> AddRoleToUser(string userid, string tcode)
        {
            ViewBag.UserID = userid;
            ViewBag.TenantCode = tcode;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + tcode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                ViewBag.Roles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("https://localhost:44391/APIRoles/getrolesbyuser?ID=" + userid);
                var postTask1 = await client1.GetAsync(client1.BaseAddress);
                var apiResponse1 = await postTask1.Content.ReadAsStringAsync();
                ViewBag.UserRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse1);
            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var getuser = new UserViewModel();

            using (var client = new HttpClient())
            {
                //getrolebyname
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getuserbyid?ID=" + id);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getuser = JsonConvert.DeserializeObject<UserViewModel>(apiResponse);
            }
            await BindDropdowns(getuser);
            return View(getuser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model,string UserIsActive)
        {
            model.IsActive = UserIsActive == "on" ? true : false;
            var result = await Save(model);
            if (result.Saved)
            {
                TempData["Success"] = AccountOptions.API_Response_Saved;
                return RedirectToAction("Index");
            }
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            await BindDropdowns(result);
            return View(result);
        }
        public async Task<UserViewModel> Save(UserViewModel model)
        {
            var getusers = new UserViewModel();
            if (ModelState.IsValid || Request.Path.Value.Contains("Edit"))
            {
                using (var client = new HttpClient())
                {
                    //HTTP POST
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/saveuser");
                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                    if (resultjson.status == AccountOptions.API_Response_Saved)
                    {                  
                        model.Saved = true;
                        return model;
                    }
                }
            }
           
            model.Saved = false;
            return model;
        }
        public async Task<IActionResult> Delete(string id)
        {
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/deleteuser");
                DeleteUserModel model = new DeleteUserModel { UserID = id };
               
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

        public async Task<IActionResult> UsersByRole(string roleid)
        {
            var getusersbyrole = new UserToRolesViewModel();
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbyrole?rid=" + roleid);
                //TODO
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getusersbyrole = JsonConvert.DeserializeObject<UserToRolesViewModel>(apiResponse);
            }
            return View(getusersbyrole);
        }

        public async Task<IActionResult> AddUserToRole(string roleid, string tcode)
        {
            var model = new UserToRolesViewModel();

            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbyrole?rid=" + roleid);
                var postTask1 = await client1.GetAsync(client1.BaseAddress);
                var apiResponse1 = await postTask1.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<UserToRolesViewModel>(apiResponse1);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbytenant?code=" + tcode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                model.GetUserByTCode = JsonConvert.DeserializeObject<List<UserViewModel>>(apiResponse);
            }
            return View(model);
        }

        public async Task<IActionResult> SaveUsersToRole(string hdnRoleID, List<string> chkUsers)
        {
            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/saveuserrole");
                var data = new UserToRolesViewModel()
                {
                    RoleID = hdnRoleID,
                    UsersCheckbox = chkUsers
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
            return RedirectToAction("UsersByRole", new { roleid = hdnRoleID });
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var getuser = new UserViewModel();

            using (var client = new HttpClient())
            {
                //getrolebyname
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getuserbyid?ID=" + id);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getuser = JsonConvert.DeserializeObject<UserViewModel>(apiResponse);
            }
            return View(getuser);
        }

        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            var getuser = new UserPasswordViewModel();

            using (var client = new HttpClient())
            {
                //getrolebyname
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getuserbyid?ID=" + id);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getuser = JsonConvert.DeserializeObject<UserPasswordViewModel>(apiResponse);
            }
            return View(getuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(UserPasswordViewModel model)
        {
           
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/savechangepassword?ID=" + model.UserID + "&datacurrent=" + model.NewPassword + "&data=" + model.ConfirmPassword);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(apiResponse);
                    if (result)
                    {
                        TempData["Success"] = AccountOptions.API_Response_Saved;
                        return RedirectToAction("Index");
                    }
                }
          
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/savepassword?ID="+model.UserID + "&datacurrent=" + model.Password + "&data=" + model.NewPassword);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(apiResponse);
                    if (result)
                    {
                        TempData["Success"] = AccountOptions.API_Response_Saved;
                        return RedirectToAction("Edit", new { id = model.UserID });
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, AccountOptions.API_Response_Failed);
            }
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            return View(model);
        }
    }
}