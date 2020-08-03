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
using SSOApp.Proxy;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class RolesManagementController : BaseController
    {
        public RolesManagementController(ApplicationDbContext context, IAPIClientProxy clientProxy) : base(context, clientProxy)
        {
        }

        public async Task<IActionResult> Index()
        {
            var response = new Response<List<RoleViewModel>>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Role";
            var clientResponse = await _client.Send($"APIRoles/getallrolesbytenant?tcode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<List<RoleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
                response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
                response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            }
            return View(response);
        }

        public IActionResult Create()
        {
            var response = new Response<RoleViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Add Role";
            response.Body = new RoleViewModel();
            return View(response);
        }

        public async Task<IActionResult> Edit(string rid)
        {
            var response = new Response<RoleViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Edit Claim";
            var clientResponse = await _client.Send($"APIRoles/getrolebyid?id={rid}&tcode={TenantCode}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<RoleViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                response.Message = "Error Occured, please try after some time.";
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.TenantCode = TenantCode;
                var response = new Response<string>();
                if (ModelState.IsValid)
                {
                    var clientResponse = await _client.Send($"APIRoles/saverole", HttpMethod.Post, JsonConvert.SerializeObject(model));
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
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = new Response<string>();

            RoleViewModel model = new RoleViewModel { Name = "NotRequired", ID = id };
            var clientResponse = await _client.Send($"APIRoles/deleterole", HttpMethod.Post, JsonConvert.SerializeObject(model));
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
            return RedirectToAction("UserIndex", new { selectedUserid = selectedUser });
        }


    }
}