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
using SSOApp.API.ViewModels;

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
        public async Task<IActionResult> SaveUser(SSOApp.Proxy.Response<SSOApp.API.ViewModels.UserViewModel> model)
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
            var response = new Response<AssignmentViewModule>();
            var roleName = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            response.Body = new AssignmentViewModule
            {
                AvailableValues = new List<ListItemValue>(),
                CurrentValues = new List<ListItemValue>()
            };

            var model = new RoletoUserViewModel();

            var userListByRole = await UserListByRole(roleid);
            var userListByTenant = await UserListByTenant();

            foreach (var module in userListByRole)
            {
                response.Body.CurrentValues.Add(new ListItemValue { DisplayText = module.FirstName + " "+module.LastName, DisplayValue = module.UserID.ToString() });
            }

            foreach (var module in userListByTenant)
            {
                response.Body.AvailableValues.Add(new ListItemValue { DisplayText = module.FirstName + " " + module.LastName, DisplayValue = module.UserID.ToString() });
            }

            response.Body.SelectedValue = roleid;
            response.Body.Controller = "Users";
            response.Body.Action = "SaveRolesForUser";
            response.Body.Entity = "User";
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode}) Role: {roleName}";
            response.PageTitle = "User Role";
            response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
            response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            return View("View", response);
            return View("View", response);
        }



        private async Task<List<UserViewModel>> UserListByRole(string roleid)
        {
            List<UserViewModel> response = new List<UserViewModel>();
            var clientResponse = await _client.Send($"APIUser/getusersbyrole?rid={roleid}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response = JsonConvert.DeserializeObject<List<UserViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }
            return response;
        }

        private async Task<List<UserViewModel>> UserListByTenant()
        {
            var getroles = new List<UserViewModel>();
            var clientResponse = await _client.Send($"APIUser/getusersbytenant?code={TenantCode}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                getroles = JsonConvert.DeserializeObject<List<UserViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }
            return getroles;
        }

        public async Task<IActionResult> SaveRolesForUser(IFormCollection formCollection)
        {
            var selectedUsers = formCollection["selectedValue"][0].Split(",");
            var selectedRole = formCollection["selectedforItem"][0];
            List<string> chkRoles = new List<string>();
            chkRoles.AddRange(selectedUsers);

            var assignmentViewModel = new AssignmentSaveViewModule
            {
                ListofAssignment = new List<string>()
            };
            assignmentViewModel.ListofAssignment.AddRange(selectedUsers);
            assignmentViewModel.SelectedValue = selectedRole;
            assignmentViewModel.TenantId = TenantId;
            var clientResponse = await _client.Send($"APIRoles/saveroletouser", HttpMethod.Post, JsonConvert.SerializeObject(assignmentViewModel));
            var clientResponseMessage = JsonConvert.DeserializeObject<dynamic>(await clientResponse.Content.ReadAsStringAsync());
            var response = new Response<string>();
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

            return RedirectToAction("AddRoleToUser", new { selectedRole = selectedRole, });
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var getuser = new UserViewModel();
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) User: {selectedUser.FirstName} {selectedUser.LastName}";
            var clientResponse = await _client.Send($"APIUser/getuserbyid?ID={id}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                getuser = JsonConvert.DeserializeObject<UserViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }

            return View(getuser);
        }

        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var getuser = new UserPasswordViewModel();
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) User: {selectedUser.FirstName} {selectedUser.LastName}";
            var clientResponse = await _client.Send($"APIUser/getuserbyid?ID={id}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                getuser = JsonConvert.DeserializeObject<UserPasswordViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }

            return View(getuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(UserPasswordViewModel model)
        {
            var clientResponse = await _client.Send($"APIUser/savechangepassword?ID={model.UserID}&datacurrent={model.NewPassword}&data={model.ConfirmPassword}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                TempData["Success"] = AccountOptions.API_Response_Saved;
                return RedirectToAction("Index");
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
                var clientResponse = await _client.Send($"APIUser/savepassword?ID={model.UserID}&datacurrent={model.NewPassword}&data={model.ConfirmPassword}", HttpMethod.Get);
                if (clientResponse.IsSuccessStatusCode)
                {
                    TempData["Success"] = AccountOptions.API_Response_Saved;
                    return RedirectToAction("Index");
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