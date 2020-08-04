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
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SSOApp.Controllers.Home;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.Proxy;
using SSOApp.API.ViewModels;

namespace SSOApp.Controllers.Admin
{
    public class ClaimsManagementController : BaseController
    {

        public ClaimsManagementController(ApplicationDbContext context, IAPIClientProxy clientProxy) : base(context, clientProxy)
        {

        }

        public async Task<IActionResult> Index()
        {
            var response = new Response<List<ClaimsViewModel>>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Claim";
            var clientResponse = await _client.Send($"APIClaims/getallclaimssbytenant?tcode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {                
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<List<ClaimsViewModel>>(await clientResponse.Content.ReadAsStringAsync());
                response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
                response.ActionResponseCode= TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            }
            return View(response);
        }

        public IActionResult Create()
        {           
            var response = new Response<ClaimsViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Add Claim";
            response.Body = new ClaimsViewModel();
            return View(response);
        }

        public async Task<IActionResult> Edit(string cid, string tcode = null)
        {
            var response = new Response<ClaimsViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Edit Claim";
            var clientResponse = await _client.Send($"APIClaims/getclaimbyid?id={cid}&tcode={tcode}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<ClaimsViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                response.Message = "Error Occured, please try after some time.";
            }

            return View(response);
        }
       
        [HttpPost]
        public async Task<IActionResult> Edit(ClaimsViewModel model)
        {
            model.TenantID = TenantId;
            var getroles = new RoleViewModel();
            var response = new Response<string>();
            if (ModelState.IsValid)
            {
                var clientResponse = await _client.Send($"APIClaims/saveclaim", HttpMethod.Post, JsonConvert.SerializeObject(model));
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

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = new Response<string>();

            ClaimsViewModel model = new ClaimsViewModel { Name = "NotRequired", ID = new Guid(id) };
            var clientResponse = await _client.Send($"APIClaims/deleteclaim", HttpMethod.Post, JsonConvert.SerializeObject(model));
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