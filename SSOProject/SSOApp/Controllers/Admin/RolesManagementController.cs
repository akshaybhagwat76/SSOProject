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
using SSOApp.API.ViewModels;

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

            var clientResponse = await _client.Send($"APIRoles/getrolesbyuser?ID={selectedUserid}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                userRoleView.CurrentRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }

            var clientResponse1 = await _client.Send($"APIRoles/getallrolesbytenant?tcode={TenantId}", HttpMethod.Get);
            if (clientResponse1.IsSuccessStatusCode)
            {
                userRoleView.AvaialbleRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(await clientResponse1.Content.ReadAsStringAsync());
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

            var data = new UserToRolesViewModel()
            {
                UserID = selectedUser,
                Roles = chkRoles
            };

            var clientResponse = await _client.Send($"APIRoles/saveuserrole", HttpMethod.Post, JsonConvert.SerializeObject(data));
            var clientResponseMessage = JsonConvert.DeserializeObject<dynamic>(await clientResponse.Content.ReadAsStringAsync());
            if (clientResponse.IsSuccessStatusCode)
            {

            }
            else
            {
                //response.Message = clientResponseMessage.MessageDetails;
            }
            return RedirectToAction("UserIndex", new { selectedUserid = selectedUser });
        }


    }
}