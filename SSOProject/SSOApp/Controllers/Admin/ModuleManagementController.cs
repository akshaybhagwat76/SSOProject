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
using SSOApp.Utility;
using SSOApp.ViewModels;

namespace SSOApp.Controllers.Admin
{
    public class ModuleManagementController : BaseController
    {
        public FeildModelView FieldViewModel { get; set; }
        public TenantRoleModel TenantRoleViewModel { get; set; }

        public readonly ApplicationDbContext _context;
        IEnumerable<Claim> claims = null;

        public ModuleManagementController(ApplicationDbContext context) : base(context)
        {
            _context = context;
            TenantRoleViewModel = new TenantRoleModel();
            FieldViewModel = new FeildModelView();
            FieldViewModel.ModuleFieldDetails = new ModuleFieldDetails();
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.FieldType = new SelectList(context.FieldTypes.ToList(), "ID", "Name");
        }

        public async Task<IActionResult> Index()
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            var moduleList = await ModuleListByTenant();
            return View(moduleList);
        }

        private async Task<List<ModuleViewModel>> ModuleListByTenant()
        {
            var moduleList = new List<ModuleViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIModules/getallmodulesbytenant?tenantCode=" + TenantId);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                moduleList = JsonConvert.DeserializeObject<List<ModuleViewModel>>(apiResponse);
            }
            return moduleList;
        }

        private async Task<List<RoleViewModel>> RoleListByTenant()
        {
            var getroles = new List<RoleViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbytenant?tcode=" + TenantCode);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            return getroles;
        }

        private async Task<List<RoleViewModel>> RoleListByModule(string moduleId)
        {
            var getroles = new List<RoleViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIRoles/getallrolesbymodule?moduleId=" + moduleId);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<RoleViewModel>>(apiResponse);
            }
            return getroles;
        }


        private async Task<List<ModuleViewModel>> ModuleListByRole(string roleId)
        {
            var moduleList = new List<ModuleViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIModules/getallmodulesbyrole?roleId=" + roleId + "&tenantCode=" + TenantId);
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                moduleList = JsonConvert.DeserializeObject<List<ModuleViewModel>>(apiResponse);
            }
            return moduleList;
        }

        public IActionResult Create()
        {
            var getclaims = new ModuleViewModel();
            return View(getclaims);
        }

        public async Task<IActionResult> Edit(string cid, string tcode = null)
        {
            var getroles = new ModuleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIModule/getmodulebyid?moduleId=" + cid + "&tenantId=" + tcode);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    getroles = JsonConvert.DeserializeObject<ModuleViewModel>(apiResponse);
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View(getroles);
        }

        public async Task<IActionResult> FieldsList(string cid, string moduleId, string tcode = null)
        {
            string Tcode = CurrentUser.TenantCode;
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.ModuleId = moduleId;
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/getmodulefieldlistbyid?id=" + cid + "&tcode=" + Tcode + "&moduleid=" + moduleId);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    FieldViewModel.ModuleFieldDetailsList = JsonConvert.DeserializeObject<List<ModuleFieldDetails>>(apiResponse);
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
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //HTTP POST
                    try
                    {
                        model.Tenant = UsersTenant;
                        client.BaseAddress = new Uri("https://localhost:44391/APIModules/savemodule");
                        var json = JsonConvert.SerializeObject(model);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        var postTask = await client.PostAsync(client.BaseAddress, stringContent);

                        string apiResponse = await postTask.Content.ReadAsStringAsync();
                        var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                        if (resultjson.status == AccountOptions.API_Response_Saved)
                        {
                            TempData["Success"] = AccountOptions.API_Response_Saved;
                            return View("CreateField", new { cid = model.ID });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            TempData["Failed"] = AccountOptions.API_Response_Failed;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddModuleToRole(string roleid)
        {
            var roleName = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) Role: {roleName}";

            var model = new AssignmentViewModule
            {
                AvailableValues = new List<ListItemValue>(),
                CurrentValues = new List<ListItemValue>()
            };
            var moduleListByTenant = await ModuleListByTenant();
            var moduleListByRole = await ModuleListByRole(roleid);
            var finalMOduleByTenant = moduleListByTenant.Except(moduleListByRole);

            foreach (var module in moduleListByRole)
            {
                model.CurrentValues.Add(new ListItemValue { DisplayText = module.ModuleName, DisplayValue = module.ID.ToString() });
            }

            foreach (var module in finalMOduleByTenant)
            {
                model.AvailableValues.Add(new ListItemValue { DisplayText = module.ModuleName, DisplayValue = module.ID.ToString() });
            }

            model.Controller = "ModuleManagement";
            model.Action = "AddModuleToRole";
            model.Entity = "Module";
            model.SelectedValue = roleid;
            return View("View", model);
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIModules/savemodulebyroles");
                var json = JsonConvert.SerializeObject(assignmentViewModel);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //HTTP POST                        
                var postTask = await client.PostAsync(client.BaseAddress, stringContent);
                var result = postTask.Content;
            }
            return RedirectToAction("AddModuleToRole", new { roleid = selectedRole });
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToModule(string moduleid)
        {
            var moduleName = await _context.ModuleDetails.FirstOrDefaultAsync(x => x.ID == new Guid(moduleid));
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode}) Module: {moduleName.ModuleName}";

            var model = new AssignmentViewModule
            {
                AvailableValues = new List<ListItemValue>(),
                CurrentValues = new List<ListItemValue>()
            };

            var roleListByTenant = await RoleListByTenant();
            var roleListByModule = await RoleListByModule(moduleid);
            var finalMOduleByTenant = roleListByTenant.Except(roleListByModule);

            foreach (var module in finalMOduleByTenant)
            {
                model.AvailableValues.Add(new ListItemValue { DisplayText = module.Name, DisplayValue = module.ID.ToString() });
            }

            foreach (var module in roleListByModule)
            {
                model.CurrentValues.Add(new ListItemValue { DisplayText = module.Name, DisplayValue = module.ID.ToString() });
            }

            model.Controller = "ModuleManagement";
            model.Action = "AddRoleToModule";
            model.Entity = "Roles";
            model.SelectedValue = moduleid;
            return View("View", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToModule(IFormCollection formData)
        {
            //
            var selectedRole = formData["selectedValue"][0].Split(",");
            var selectedModule = formData["selectedforItem"][0];

            var assignmentViewModel = new AssignmentSaveViewModule
            {
                ListofAssignment = new List<string>()
            };
            assignmentViewModel.ListofAssignment.AddRange(selectedRole);
            assignmentViewModel.SelectedValue = selectedModule;
            assignmentViewModel.TenantId = TenantId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/APIModules/saverolesbymodule");
                var json = JsonConvert.SerializeObject(assignmentViewModel);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //HTTP POST                        
                var postTask = await client.PostAsync(client.BaseAddress, stringContent);
                var result = postTask.Content;
            }
            return RedirectToAction("AddRoleToModule", new { moduleid = selectedModule });
        }
    }
}