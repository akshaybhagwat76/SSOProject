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
                                    Name = r.Name
                                }).ToListAsync();

            return result;
        }

        [HttpGet("getallrolesbymodule")]
        public async Task<List<RoleViewModel>> RolesbyModule(string moduleId)
        {
            var result = await (from d in _context.ModuleRoles
                                join r in _context.Roles on d.RoleID equals r.Id
                                //join t in _context.Tenants on d.TenantID equals t.Id
                                where d.ModuleID == new Guid(moduleId)
                                select new RoleViewModel
                                {
                                    ID = r.Id,
                                    Name = r.Name,
                                    
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

                if (!string.IsNullOrEmpty(model.ID))
                {
                    //Update
                    var getrolebyid = await _context.Roles.SingleOrDefaultAsync(d => d.Id == model.ID);
                    if (getrolebyid.Name != model.Name)
                    {
                        //Check role exists
                        if (string.IsNullOrEmpty(await CheckExistingRole(model.Name)))
                        {
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
                        var role = new IdentityRole();
                        role.Name = model.Name;
                        await _roleManager.CreateAsync(role);
                        message = AccountOptions.API_Response_Saved;
                    }
                }


                if (message == AccountOptions.API_Response_Saved || message == string.Empty)
                {
                    var getrole = await _context.Roles.SingleOrDefaultAsync(d => d.Name == model.Name);
                    model.ID = getrole.Id;
                    //Save to tenantrole                        
                    var gettenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenantCode);
                    var gettenantroles = await _context.TenantRoles.FirstOrDefaultAsync(d => d.TenantID == gettenant.Id && d.RoleID == model.ID);
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

        [HttpPost("saveroletouser")]
        public async Task<IActionResult> SaveRolesToUser(UserToRolesViewModel model)
        {
            string message = string.Empty;
            try
            {
                var roleName = await _context.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleID);
                var previousroleUser = await _context.UserRoles.Where(x => x.RoleId == model.RoleID).ToListAsync();
                _context.UserRoles.RemoveRange(previousroleUser);
                await _context.SaveChangesAsync();
                foreach (var role in model.SelectedUsers)
                {
                    var user = await _userManager.FindByIdAsync(role);
                    await _userManager.AddToRoleAsync(user, roleName.Name);
                    message = AccountOptions.API_Response_Saved;
                }


                //if (result.Succeeded)
                //    message = AccountOptions.API_Response_Saved;
                //else
                //    message = AccountOptions.API_Response_Failed;


                //else
                //    message = AccountOptions.API_Response_Failed;
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
                var tenantRole = await _context.TenantRoles.FirstOrDefaultAsync(x => x.RoleID == model.ID);
                _context.TenantRoles.RemoveRange(tenantRole);
                await _context.SaveChangesAsync();

                message = AccountOptions.API_Response_Deleted;

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

    }
}