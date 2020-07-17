using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSOApp.Controllers.UI;
using SSOApp.ViewModels;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APIRolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public APIRolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("getallroles")]
        public async Task<List<RoleViewModel>> Index()
        {
            var result = new List<RoleViewModel>();
            var getroles = await _context.Roles.ToListAsync();
            foreach (var item in getroles)
            {
                result.Add(new RoleViewModel
                {
                    Name = item.Name,
                    ID = item.Id,
                    TenantCode = GetCodeByRoleID(item.Id)
                });
            }
            return result;
        }

        private string GetCodeByRoleID(string id)
        {
            var getCode = (from r in _context.Roles
                           join rt in _context.TenantRoles on r.Id equals rt.RoleID
                           join t in _context.Tenants on rt.TenantID equals t.Id
                           where r.Id == id
                           select t).FirstOrDefault();
            if (getCode != null)
                return getCode.Code;

            return string.Empty;
        }

        [HttpGet("getallrolesbytenant")]
        public async Task<List<RoleViewModel>> RolesbyTenant(string tcode)
        {
            return await RolesbyTenantList(tcode);
        }
       
        private async Task<List<RoleViewModel>> RolesbyTenantList(string tcode)
        {
            var result = await (from d in _context.TenantRoles
                                join r in _context.Roles on d.RoleID equals r.Id
                                join t in _context.Tenants on d.TenantID equals t.Id
                                where t.Code == tcode
                                select new RoleViewModel
                                {
                                    ID = r.Id,
                                    Name = r.Name,
                                    TenantCode = t.Code,
                                    TenantName = t.Name
                                }).ToListAsync();

            return result;
        }

        [HttpGet("getrolebyid")]
        public async Task<RoleViewModel> GetRole(string ID, string tcode)
        {
            return await _context.Roles.Where(d => d.Id == ID).Select(role =>
                   new RoleViewModel
                   {
                       Name = role.Name,
                       ID = role.Id,
                       TenantCode = tcode
                   }).SingleOrDefaultAsync();
        }
        [HttpGet("getrolesbyuser")]
        public async Task<List<RoleViewModel>> GetRolesByUser(string ID)
        {
            List<RoleViewModel> model = new List<RoleViewModel>();
            ApplicationUser user = await _userManager.FindByIdAsync(ID);
            var result = await _userManager.GetRolesAsync(user);
            if (result.Count > 0)
            {
                //If User has roles
                foreach (string item in result)
                {
                    IdentityRole role = await _roleManager.FindByNameAsync(item);
                    model.Add(new RoleViewModel
                    {
                        ID = role.Id,
                        Name = role.Name,
                        TenantCode = user.TenantCode,
                        TenantName = _context.Tenants.FirstOrDefault(d => d.Code == user.TenantCode).Name,
                        UserFullName = user.FirstName + " " + user.LastName,
                        UserID = user.Id
                    });
                }
            }
            else
            {
                //If User dont have roles
                model.Add(new RoleViewModel
                {
                    TenantCode = user.TenantCode,
                    TenantName = _context.Tenants.SingleOrDefault(d => d.Code == user.TenantCode).Name,
                    UserFullName = user.FirstName + " " + user.LastName,
                    UserID = user.Id
                });
            }
            return model;
        }
        [HttpPost("saverole")]
        public async Task<IActionResult> SaveRole(RoleViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool checkcode = await IsTenantCodeAvailable(model.TenantCode);
                if (checkcode)
                {
                    if (!string.IsNullOrEmpty(model.ID))
                    {
                        //Update
                        var getrolebyid = await _context.Roles.SingleOrDefaultAsync(d => d.Id == model.ID);
                        if (getrolebyid.Name != model.Name)
                        {
                            //Check role exists
                            var checkalreadyexist = await CheckExistingRole(model.Name);
                            if (!string.IsNullOrEmpty(checkalreadyexist))
                            {
                                //exists
                                message = AccountOptions.API_Response_Exist;
                            }
                            else
                            {
                                //Does not exist    //Update role                                
                                getrolebyid.Name = model.Name;
                                getrolebyid.NormalizedName = model.Name.ToUpper();
                                await _context.SaveChangesAsync();
                                message = AccountOptions.API_Response_Saved;
                            }
                        }
                    }
                    else
                    {
                        //Add
                        if (string.IsNullOrEmpty(await CheckExistingRole(model.Name)))
                        {
                            //Doesnot exist     //Add new
                            var role = new IdentityRole();
                            role.Name = model.Name;
                            await _roleManager.CreateAsync(role);
                            var getrole = await _context.Roles.SingleOrDefaultAsync(d => d.Name == model.Name);
                            model.ID = getrole.Id;
                            message = AccountOptions.API_Response_Saved;
                        }
                        else
                            message = AccountOptions.API_Response_Exist;
                    }
                    if (message == AccountOptions.API_Response_Saved || message == string.Empty)
                    {
                        //Save to tenantrole                        
                        var gettenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenantCode);
                        var gettenantroles = await _context.TenantRoles.FirstOrDefaultAsync(d => d.TenantID == gettenant.Id && d.RoleID==model.ID);
                        if (gettenantroles == null)    //Add new tenant role
                            _context.TenantRoles.Add(new TenantRoles { RoleID = model.ID, TenantID = gettenant.Id });
                        else
                        {
                            gettenantroles.RoleID = model.ID;
                            gettenantroles.TenantID = gettenant.Id;
                        }
                        _context.SaveChanges();
                        message = AccountOptions.API_Response_Saved;
                    }
                }
                else
                {
                    message = AccountOptions.InvalidTenantErrorMessage;
                }
            }
            catch (Exception ex)
            {
                message = AccountOptions.API_Response_Exception;
            }
            return Ok(new
            {
                Status = message
            });
        }

        [HttpPost("saveuserrole")]
        public async Task<IActionResult> SaveUserRoles(UserToRolesViewModel model)
        {
            string message = string.Empty;
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserID);
                var previousroles = await _userManager.GetRolesAsync(user);
                var removeroles = await _userManager.RemoveFromRolesAsync(user, previousroles);
                if (removeroles.Succeeded)
                {
                    var result = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (result.Succeeded)
                        message = AccountOptions.API_Response_Saved;
                    else
                        message = AccountOptions.API_Response_Failed;
                }
                else
                    message = AccountOptions.API_Response_Failed;
            }
            catch (Exception ex)
            {
                message = AccountOptions.API_Response_Exception;
            }
            return Ok(new
            {
                Status = message
            });
        }
        private async Task<string> CheckExistingRole(string rName)
        {
            var checkalreadyexist = await _roleManager.RoleExistsAsync(rName);
            if (checkalreadyexist)
            {
                return AccountOptions.API_Response_Exist;
            }
            else
            {
                return string.Empty;
            }
        }
        [HttpPost("deleterole")]
        public async Task<IActionResult> DeleteRole(RoleViewModel model)
        {
            string message = string.Empty;
            try
            {                
                IdentityRole role = await _roleManager.FindByIdAsync(model.ID);
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    message = AccountOptions.API_Response_Deleted;
                else
                    message = AccountOptions.API_Response_Failed;
            }
            catch (Exception ex)
            {
                message = AccountOptions.API_Response_Exception;
            }
            return Ok(new
            {
                Status = message
            });
        }
        [HttpPost("deletuserfromerole")]
        public async Task<IActionResult> DeleteUserFromRole(UserToRolesViewModel model)
        {
            string message = string.Empty;
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.UserID);
                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                if (result.Succeeded)
                    message = AccountOptions.API_Response_Success;
                else
                    message = AccountOptions.API_Response_Failed;
            }
            catch (Exception ex)
            {
                message = AccountOptions.API_Response_Exception;
            }
            return Ok(new
            {
                Status = message
            });
        }

        [HttpGet("getddroles")]
        public async Task<List<SelectListItem>> GetDDRoles()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result = await _roleManager.Roles.Select(d => new SelectListItem { Text = d.Name, Value = d.Id }).ToListAsync();
            return result;
        }

        private async Task<bool> IsTenantCodeAvailable(string code)
        {
            var getcode = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == code);
            if (getcode != null)
                return true;

            return false;
        }
    }
}