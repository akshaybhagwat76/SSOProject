using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using SSOApp.Controllers.Home;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.Proxy;
using SSOApp.Utility;
using SSOApp.API.ViewModels;
using SSOApp.API.ViewModels;
namespace SSOApp.Controllers.Admin
{
    public class ModuleManagementController : BaseController
    {
        public FeildModelView FieldViewModel { get; set; }
        public TenantRoleModel TenantRoleViewModel { get; set; }


        public ModuleManagementController(ApplicationDbContext context, IAPIClientProxy clientProxy) : base(context, clientProxy)
        {
            TenantRoleViewModel = new TenantRoleModel();
            FieldViewModel = new FeildModelView();
            FieldViewModel.ModuleFieldDetails = new ModuleFieldDetails();
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.FieldType = new SelectList(context.FieldTypes.ToList(), "ID", "Name");
        }

        public async Task<IActionResult> Index()
        {
            var response = new Response<List<ModuleViewModel>>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Module";
            var clientResponse = await _client.Send($"APIModules/getallmodulesbytenant?tenantCode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<List<ModuleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
                response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
                response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            }
            return View(response);
        }

        private async Task<List<ModuleViewModel>> ModuleListByTenant()
        {
            List<ModuleViewModel> response = new List<ModuleViewModel>();
            var clientResponse = await _client.Send($"APIModules/getallmodulesbytenant?tenantCode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response = JsonConvert.DeserializeObject<List<ModuleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }
            return response;
        }

        private async Task<List<RoleViewModel>> RoleListByTenant()
        {
            var getroles = new List<RoleViewModel>();
            var clientResponse = await _client.Send($"APIRoles/getallrolesbytenant?tcode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }
            return getroles;
        }

        private async Task<List<RoleViewModel>> RoleListByModule(string moduleId)
        {
            var getroles = new List<RoleViewModel>();

            var clientResponse = await _client.Send($"APIRoles/getallrolesbymodule?moduleId={moduleId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }
            return getroles;
        }


        private async Task<List<ModuleViewModel>> ModuleListByRole(string roleId)
        {
            var moduleList = new List<ModuleViewModel>();
            var clientResponse = await _client.Send($"APIModules/getallmodulesbyrole?roleId={roleId}&tenantCode={TenantId}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                moduleList = JsonConvert.DeserializeObject<List<ModuleViewModel>>(await clientResponse.Content.ReadAsStringAsync());
            }

            return moduleList;
        }

        public IActionResult Create()
        {
            var response = new Response<ModuleViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Add Module";
            response.Body = new ModuleViewModel();
            return View(response);
        }

        public async Task<IActionResult> Edit(string cid, string tcode = null)
        {
            var response = new Response<ModuleViewModel>();
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode})";
            response.PageTitle = "Edit Claim";
            var clientResponse = await _client.Send($"APIModules/getmodulebyid?moduleId={cid} &tenantId={tcode}", HttpMethod.Get);
            if (clientResponse.IsSuccessStatusCode)
            {
                response.Status = clientResponse.StatusCode;
                response.Body = JsonConvert.DeserializeObject<ModuleViewModel>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                response.Message = "Error Occured, please try after some time.";
            }

            return View(response);
        }

        public async Task<IActionResult> FieldsList(string cid, string moduleId, string tcode = null)
        {
            string Tcode = CurrentUser.TenantCode;
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.ModuleId = moduleId;
            if (ModelState.IsValid)
            {
                var clientResponse = await _client.Send($"APIModules/getmodulefieldlistbyid?id={cid}&tcode={Tcode}&moduleid={moduleId}", HttpMethod.Get);
                if (clientResponse.IsSuccessStatusCode)
                {
                    FieldViewModel.ModuleFieldDetailsList = JsonConvert.DeserializeObject<List<ModuleFieldDetails>>(await clientResponse.Content.ReadAsStringAsync());
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View(FieldViewModel);
        }

        //FieldCreate
        public IActionResult CreateField(string moduleid)
        {

            ViewBag.TenantName = CurrentUser.TenantName;
            ViewBag.TenantCode = CurrentUser.TenantCode;
            FieldViewModel.ModuleFieldDetails.ModuleDetailsID = new Guid(moduleid);
            ViewBag.FeildTypeList = new SelectList(_context.FieldTypes.AsNoTracking(), "Name", "Name");
            return View("CreateField", FieldViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateField(FeildModelView feildModelView)
        {

            ViewBag.TenantName = CurrentUser.TenantName;
            ViewBag.TenantCode = CurrentUser.TenantCode;
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    //HTTP POST
                    try
                    {
                        feildModelView.TenantCode = CurrentUser.TenantCode;

                        client.BaseAddress = new Uri("https://localhost:44391/APIModules/saveFieldmodule");
                        var json = JsonConvert.SerializeObject(feildModelView);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                        string apiResponse = await postTask.Content.ReadAsStringAsync();
                        var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                        if (resultjson.status == AccountOptions.API_Response_Saved)
                        {
                            TempData["Success"] = AccountOptions.API_Response_Saved;
                            return Json(resultjson.status);
                        }
                        else
                        {
                            ViewBag.Success = resultjson.status;
                            return Json(resultjson.status);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            //TempData["Failed"] = AccountOptions.API_Response_Failed;
            //return View("CreateField", AccountOptions.API_Response_Failed);
            return RedirectToAction("CreateField", new { moduleid = feildModelView.ModuleId });
        }


        public async Task<IActionResult> EditField(string Fieldid)
        {
            ViewBag.TenantName = CurrentUser.TenantName;
            ViewBag.TenantCode = CurrentUser.TenantCode;

            var keyValueContent = FieldViewModel.ToKeyValue();
            var formUrlEncodedContent = new FormUrlEncodedContent(keyValueContent);
            var urlEncodedString = formUrlEncodedContent.ReadAsStringAsync();
            ViewBag.FeildTypeList = new SelectList(_context.FieldTypes.AsNoTracking(), "Name", "Name");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/getFeildListByFeildId?FeildId=" + Fieldid);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    FieldViewModel = JsonConvert.DeserializeObject<FeildModelView>(apiResponse);
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View("CreateField", FieldViewModel);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(ModuleViewModel model)
        {
            var response = new Response<string>();
            if (ModelState.IsValid)
            {
                model.Tenant = new Tenant
                { Id = TenantId, Code = TenantCode };
                var clientResponse = await _client.Send($"APIModules/savemodule", HttpMethod.Post, JsonConvert.SerializeObject(model));
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

        [HttpGet]
        public async Task<IActionResult> AddModuleToRole(string roleid)
        {
            var roleName = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            var response = new Response<AssignmentViewModule>();

            response.Body = new AssignmentViewModule
            {
                AvailableValues = new List<ListItemValue>(),
                CurrentValues = new List<ListItemValue>()
            };

            var moduleListByTenant = await ModuleListByTenant();
            var moduleListByRole = await ModuleListByRole(roleid);
            var finalMOduleByTenant = moduleListByTenant.Except(moduleListByRole);

            foreach (var module in moduleListByRole)
            {
                response.Body.CurrentValues.Add(new ListItemValue { DisplayText = module.ModuleName, DisplayValue = module.ID.ToString() });
            }

            foreach (var module in finalMOduleByTenant)
            {
                response.Body.AvailableValues.Add(new ListItemValue { DisplayText = module.ModuleName, DisplayValue = module.ID.ToString() });
            }

            response.Body.Controller = "ModuleManagement";
            response.Body.Action = "AddModuleToRole";
            response.Body.Entity = "Module";
            response.Body.SelectedValue = roleid;
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode}) Role: {roleName}";
            response.PageTitle = "Role Module";
            response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
            response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            return View("View", response);
        }

        [HttpPost]
        public async Task<IActionResult> AddModuleToRole(IFormCollection formData)
        {
            //
            var selectedUsers = formData["selectedValue"][0].Split(",");
            var selectedRole = formData["selectedforItem"][0];

            var assignmentViewModel = new AssignmentSaveViewModule
            {
                ListofAssignment = new List<string>()
            };
            assignmentViewModel.ListofAssignment.AddRange(selectedUsers);
            assignmentViewModel.SelectedValue = selectedRole;
            assignmentViewModel.TenantId = TenantId;
            var clientResponse = await _client.Send($"APIModules/saverolesmodule", HttpMethod.Post, JsonConvert.SerializeObject(assignmentViewModel));
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
            return RedirectToAction("AddModuleToRole", new { roleid = selectedRole });
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToModule(string moduleid)
        {
            var response = new Response<AssignmentViewModule>();
            var moduleName = await _context.ModuleDetails.FirstOrDefaultAsync(x => x.ID == new Guid(moduleid));


            response.Body = new AssignmentViewModule
            {
                AvailableValues = new List<ListItemValue>(),
                CurrentValues = new List<ListItemValue>()
            };

            var roleListByTenant = await RoleListByTenant();
            var roleListByModule = await RoleListByModule(moduleid);
            var finalMOduleByTenant = roleListByTenant.Except(roleListByModule);

            foreach (var module in finalMOduleByTenant)
            {
                response.Body.AvailableValues.Add(new ListItemValue { DisplayText = module.Name, DisplayValue = module.ID.ToString() });
            }

            foreach (var module in roleListByModule)
            {
                response.Body.CurrentValues.Add(new ListItemValue { DisplayText = module.Name, DisplayValue = module.ID.ToString() });
            }

            response.Body.Controller = "ModuleManagement";
            response.Body.Action = "AddRoleToModule";
            response.Body.Entity = "Roles";
            response.Body.SelectedValue = moduleid;
            response.PageSubheading = $"Tenant: {TenantName} (Code: {TenantCode}) Module: {moduleName.ModuleName}";
            response.PageTitle = "Module Role";
            response.Message = TempData["MessageDetails"] == null ? "" : Convert.ToString(TempData["MessageDetails"]);
            response.ActionResponseCode = TempData["MessageCode"] == null ? "" : Convert.ToString(TempData["MessageCode"]);
            return View("View", response);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToModule(IFormCollection formData)
        {
            //
            var selectedRole = formData["selectedValue"][0].Split(",");
            var selectedModule = formData["selectedforItem"][0];

            var assignmentViewModel = new AssignmentSaveViewModule
            {
                ListofAssignment = new List<string>(),
                TenantId = TenantId
            };
            assignmentViewModel.ListofAssignment.AddRange(selectedRole);
            assignmentViewModel.SelectedValue = selectedModule;
            assignmentViewModel.TenantId = TenantId;
            var response = new Response<string>();
            var clientResponse = await _client.Send($"APIModules/savemoduleroles", HttpMethod.Post, JsonConvert.SerializeObject(assignmentViewModel));
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

            return RedirectToAction("AddRoleToModule", new { moduleid = selectedModule });
        }
    }
}