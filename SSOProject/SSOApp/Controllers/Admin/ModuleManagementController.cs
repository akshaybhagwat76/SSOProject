using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
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
        public ModuleManagementController(ApplicationDbContext context)
        {

            _context = context;
            TenantRoleViewModel=new TenantRoleModel();
 



            FieldViewModel = new FeildModelView();
            FieldViewModel.ModuleFieldDetails = new ModuleFieldDetails();
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.FieldType = new SelectList(_context.FieldTypes.ToList(), "ID", "Name");




            
        }



        public async Task<IActionResult> Index()
        {
            await BindTenantDD();
            var getroles = new List<ModuleViewModel>();
            using (var client = new HttpClient())
            {
                //getallroles
                client.BaseAddress = new Uri("https://localhost:44391/APIModule/getallmodules");
                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                getroles = JsonConvert.DeserializeObject<List<ModuleViewModel>>(apiResponse);
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

        public async Task<IActionResult> GetModuleByTenant(string tcode)
        {
            //TODO: removoe tcode
            tcode = "ABCO";
            var getroles = new List<ModuleViewModel>();

            using (var client = new HttpClient())
            {
                //getallusers
                if (!string.IsNullOrEmpty(tcode))
                {
                    //Select by code
                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/getallmodulesbytenant?tcode=" + tcode);
                }
                else
                {
                    //Select All
                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/getallmodules");
                }
                try
                {
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    getroles = JsonConvert.DeserializeObject<List<ModuleViewModel>>(apiResponse);

                }
                catch (Exception ex)
                {

                }
            }
            return PartialView("_ModuleGrid", getroles);
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
                    client.BaseAddress = new Uri("https://localhost:44391/APIModule/getFeildListByFeildId?id=" + cid + "&tcode=" + tcode);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    getroles = JsonConvert.DeserializeObject<ModuleViewModel>(apiResponse);
                }
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidRoleName);
            return View(getroles);
        }

        public async Task<IActionResult> FieldsList(string cid, string moduleId,string tcode = null)
        {
            string Tcode = CurrentUser.TenantCode;
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.ModuleId = moduleId;
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //getrolebyname
                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/getmodulefieldlistbyid?id=" + cid + "&tcode=" + Tcode+"&moduleid="+moduleId);
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
            FieldViewModel.ModuleFieldDetails.ModuleDetailsID =new Guid(moduleid);
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
            return Json(AccountOptions.API_Response_Failed);
        }

        
        public async Task<IActionResult> EditField(string Fieldid)
        {
            ViewBag.TenantName = CurrentUser.TenantName;
            ViewBag.TenantCode = CurrentUser.TenantCode;

            var keyValueContent = FieldViewModel.ToKeyValue();
            var formUrlEncodedContent = new FormUrlEncodedContent(keyValueContent);
            var urlEncodedString =  formUrlEncodedContent.ReadAsStringAsync();
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
            model.TenantCode = "ABCO";
            var getroles = new RoleViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //HTTP POST
                    try
                    {

                        client.BaseAddress = new Uri("https://localhost:44391/APIModules/savemodule");
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
                    catch (Exception ex)
                    {

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
                client.BaseAddress = new Uri("https://localhost:44391/APIModule/deletemodule");
                ClaimsViewModel model = new ClaimsViewModel { Name = "NotRequired", ID = id };
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

        public async Task<IActionResult> Roles(string cid,string tcode,string moduleId)
        {
            var PstenantId = _context.Tenants.Where(m => m.Code == tcode).Select(m => m.Id).FirstOrDefault();
            var PsmoduleId = new Guid(moduleId);
            var RolesUnSelectedList = (from r in _context.Roles
                                                       select new
                                                       {
                                                           Id = r.Id,
                                                           Name = r.Name
                                                       }
                                                     ).ToList();

            TenantRoleViewModel.RolesUnSelectedList = new SelectList(RolesUnSelectedList, "Id", "Name");



            var psmoduleId = new Guid(moduleId);
            var list= (from m in _context.TenantRoles
                                                     join r in _context.Roles on m.RoleID equals r.Id
                                                     where(m.ModuleID==moduleId && m.TenantID==PstenantId)
                                                     select new
                                                     {
                                                         Id=m.RoleID,
                                                         Name=r.Name
                                                     }
                                                     ).ToList();

            TenantRoleViewModel.RolesSelectedList = new SelectList(list, "Id", "Name");
            TenantRoleViewModel.TennantId = PstenantId;
            TenantRoleViewModel.ModuleId = psmoduleId;
            return View(TenantRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(TenantRoleModel TenantRoleModel)
        {
            using (var client = new HttpClient())
            {
                //HTTP POST
                try
                {

                    client.BaseAddress = new Uri("https://localhost:44391/APIModules/saveTenantRole");
                    var json = JsonConvert.SerializeObject(TenantRoleModel);
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

            return View(TenantRoleViewModel);
        }

    }
}