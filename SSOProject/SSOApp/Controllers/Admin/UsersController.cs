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
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Newtonsoft.Json;
using SSOApp.Controllers.Home;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.Proxy;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        public UsersController(ApplicationDbContext context, IAPIClientProxy clientProxy) : base(context, clientProxy)
        {

        }

        public async Task<IActionResult> Index()
        {
            var response = new Response<List<UserViewModel>>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Role";
            var clientResponse = await _client.Send($"APIUser/getusersbytenant?code={TenantCode}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<List<UserViewModel>>(await clientResponse.Content.ReadAsStringAsync());
                response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
                response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            }
            return View(response);

        }

        public IActionResult Create()
        {
            var response = new Response<UserViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Add User";
            response.Body = new UserViewModel();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUser(SSOApp.Proxy.Response<SSOApp.ViewModels.UserViewModel> model)
        {
            model.Body.TenanntCode = TenantCode;
            var response = new Response<string>();
            if (ModelState.IsValid)
            {

                var clientResponse = await _client.Send($"APIUser/saveuser", HttpMethod.Post, JsonConvert.SerializeObject(model.Body));
                var clientResponseMessage = JsonConvert.DeserializeObject<dynamic>(await clientResponse.Content.ReadAsStringAsync());
                if (clientResponse.IsSuccessStatusCode)
                {
                    response.Status = clientResponse.StatusCode;
                    response.ActionResponseCode = clientResponseMessage.MessageCode;
                    response.Message = clientResponseMessage.MessageDetails;
                }
                else
                {
                    response.Message = clientResponseMessage.MessageDetails;
                }
                TempData["MessageCode"] = response.ActionResponseCode;
                TempData["MessageDetails"] = response.Message;
                return RedirectToAction("Index");
            }

            return View("Create", model);
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

        public async Task<IActionResult> Edit(string id)
        {
            var response = new Response<UserViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Edit User";
            var clientResponse = await _client.Send($"APIUser/getuserbyid?ID={id}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<UserViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                response.Message = "Error Occured, please try after some time.";
            }

            return View(response);
        }

        public async Task<IActionResult> Delete(string id)
        {

            var response = new Response<UserViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Edit User";
            DeleteUserModel model = new DeleteUserModel { UserID = id };
            var clientResponse = await _client.Send($"APIUser/deleteuser", HttpMethod.Post, JsonConvert.SerializeObject(model));
            var clientResponseMessage = JsonConvert.DeserializeObject<dynamic>(await clientResponse.Content.ReadAsStringAsync());
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<UserViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                response.Message = "Error Occured, please try after some time.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddRoleToUser(string roleid)
        {
            var model = new RoletoUserViewModel();

            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbyrole?rid=" + roleid);
                var postTask1 = await client1.GetAsync(client1.BaseAddress);
                var apiResponse1 = await postTask1.Content.ReadAsStringAsync();
                model.CurrentUser = JsonConvert.DeserializeObject<List<ApplicationUser>>(apiResponse1);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIUser/getusersbytenant?code=" + TenantCode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                model.AvaialbleUser = JsonConvert.DeserializeObject<List<ApplicationUser>>(apiResponse);
            }
            model.SelectedRoleId = roleid;
            return View(model);
        }

        public async Task<IActionResult> SaveRolesForUser(IFormCollection formCollection)
        {
            var selectedUsers = formCollection["selectedUsers"][0].Split(",");
            var selectedRole = formCollection["selectedRole"][0];
            List<string> chkRoles = new List<string>();
            chkRoles.AddRange(selectedUsers);

            using (var client = new HttpClient())
            {
                //HTTP POST
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/saveroletouser");
                var data = new UserToRolesViewModel()
                {
                    SelectedUsers = selectedUsers.ToList(),
                    RoleID = selectedRole
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
            return RedirectToAction("AddRoleToUser", new { selectedRole = selectedRole, });
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var getuser = new UserViewModel();
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) User: {selectedUser.FirstName} {selectedUser.LastName}";
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
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var getuser = new UserPasswordViewModel();
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) User: {selectedUser.FirstName} {selectedUser.LastName}";
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
                    client.BaseAddress = new Uri("https://localhost:44391/APIUser/savepassword?ID=" + model.UserID + "&datacurrent=" + model.Password + "&data=" + model.NewPassword);
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