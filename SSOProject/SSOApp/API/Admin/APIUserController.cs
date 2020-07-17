using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public APIUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("getallusers")]
        public async Task<List<UserViewModel>> Index()
        {          
            return await _context.Users.Include(x=>x.Tenant).Select(user =>
                new UserViewModel
                {
                    UserID = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    TenanntCode = user.TenantCode,
                    TenanntName = user.Tenant.Name,
                    UserName = user.UserName,
                    Tenants = _context.Tenants.ToList(),
                    LastLoggedIn = user.LastLoginTime
                    
                }).ToListAsync();
        }

        [HttpGet("getuserbyid")]
        public async Task<UserViewModel> GetUser(string ID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(d => d.Id == ID);
            UserViewModel model = new UserViewModel
            {
                UserID = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                TenanntCode = user.TenantCode,                
                UserName = user.UserName,
                LastLoggedIn = user.LastLoginTime,
                SelectedRoles = await GetRolesByUser(ID)
            };
            return model;
        }

        [HttpGet("savechangepassword")]
        public async Task<bool> SaveChangePassword(string ID, string datacurrent, string data)
        {
            var getuser = await _userManager.FindByIdAsync(ID);
            await _userManager.RemovePasswordAsync(getuser);
            var result = await _userManager.AddPasswordAsync(getuser, data);
            if (result.Succeeded)
                return true;
            return false;
        }

        [HttpGet("savepassword")]
        public async Task<bool> SavePassword(string ID,string datacurrent,string data)
        {
            var getuser = await _userManager.FindByIdAsync(ID);

            var result=await _userManager.ChangePasswordAsync(getuser, datacurrent, data);
            if (result.Succeeded)
                return true;
            return false;
        }
            private async Task<List<string>> GetRolesByUser(string ID)
        {
            List<string> resultstring = new List<string>();
            ApplicationUser user = await _userManager.FindByIdAsync(ID);
            var result = await _userManager.GetRolesAsync(user);
            if (result.Count > 0)
            {
                //If User has roles
                foreach (string item in result)
                {
                    IdentityRole role = await _roleManager.FindByNameAsync(item);
                    resultstring.Add(role.Name);
                }
            }
            return resultstring;
        }
        [HttpGet("getusersbyrole")]
        public async Task<UserToRolesViewModel> GetUsersByRole(string rid)
        {
            var result = new UserToRolesViewModel();
            var getrole = await _roleManager.FindByIdAsync(rid);
            var getuser = await (from ur in _context.UserRoles
                                 join u in _context.Users on ur.UserId equals u.Id
                                 join r in _context.Roles on ur.RoleId equals r.Id
                                 where r.Id == rid
                                 select u).ToListAsync();
            result.RoleName = getrole.Name;
            result.RoleID = rid;
            result.Tenant = await GetTenantByRole(rid);
            result.Users = getuser;
            return result;
        }
        private async Task<Tenant> GetTenantByRole(string rid)
        {
            return await (from tr in _context.TenantRoles
                          join r in _context.Roles on tr.RoleID equals r.Id
                          join t in _context.Tenants on tr.TenantID equals t.Id
                          where r.Id == rid
                          select t).FirstOrDefaultAsync();
        }
        [HttpGet("getusersbytenant")]
        public async Task<List<UserViewModel>> GetUserByTenantCode(string code)
        {
            var result = await _context.Users.Where(d => d.TenantCode == code).Select(user =>
                new UserViewModel
                {
                    UserID = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    TenanntCode = user.Tenant.Code,
                    TenanntName = user.Tenant.Name,
                    UserName = user.UserName,
                    Tenants = _context.Tenants.ToList(),
                    LastLoggedIn = user.LastLoginTime
                }).ToListAsync();
            return result;
        }
        [HttpPost("saveuser")]       
        public async Task<IActionResult> Create(UserViewModel model)
        {
            string message = string.Empty;
            try
            {
                if (model.UserID == null)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    //TODO: Update Tenenatcode
                    //var checktenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenanntCode);
                    var checktenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == "ABCO");

                    if (user != null)
                        message = AccountOptions.API_Response_Exist;
                    else if (checktenant == null)
                        message = AccountOptions.InvalidTenantErrorMessage;
                    else
                    {
                        //TODO: Update TenantCode
                        var user1 = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            TenantCode = "ABCO",
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsActive = true
                        };
                        var createduser = await _userManager.CreateAsync(user1, model.Password);
                        if (createduser.Succeeded)
                        {
                            if (model.SelectedRoles.Count > 0)
                            {
                                await _userManager.AddToRolesAsync(user1, model.SelectedRoles);
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user1, "Employee");
                            }
                            message = AccountOptions.API_Response_Saved;

                        }
                        else
                        {
                            message = AccountOptions.API_Response_Failed;
                        }
                    }
                }
                else
                {
                    //Update
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    //user.IsActive = !string.IsNullOrEmpty(model.IsActive) ? true : false;
                    //TODO: Change tenant code
                    // var checktenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenanntCode);
                    var checktenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == "ABCO");

                    if (checktenant != null)
                        user.TenantCode = model.TenanntCode;

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        var getroles = await _userManager.GetRolesAsync(user);
                        var check = await _userManager.RemoveFromRolesAsync(user, getroles);
                        if (check.Succeeded)
                        {
                            if(model.SelectedRoles!=null)
                            {
                                check = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                                if (!check.Succeeded)
                                    message = AccountOptions.API_Response_Failed;
                                else
                                    message = AccountOptions.API_Response_Saved;

                            }
                            else
                            {

                                message = AccountOptions.API_Response_Saved;
                            }
                        }
                        else
                            message = AccountOptions.API_Response_Failed;
                    }
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
        //call from Roles
        [HttpPost("saveuserrole")]
        public async Task<IActionResult> SaveUserRoles(UserToRolesViewModel model)
        {
            string message = string.Empty;
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleID);
                var previoususers = await GetUsersByRole(model.RoleID);
                foreach (var item in previoususers.Users)
                {
                    _context.UserRoles.Remove(new IdentityUserRole<string> { RoleId = model.RoleID, UserId = item.Id });
                    _context.SaveChanges();
                }
                foreach (var item in model.UsersCheckbox)
                {
                    var getuser = await _userManager.FindByNameAsync(item);
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = model.RoleID, UserId = getuser.Id });
                    _context.SaveChanges();
                }
                message = AccountOptions.API_Response_Saved;
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

        [HttpGet("savelastlogintime")]
        public async Task<IActionResult> SaveLastLogin(string username)
        {
            string message = string.Empty;
            try { 
            var getuser = await _userManager.FindByNameAsync(username);
            getuser.LastLoginTime = DateTime.Now;
            var result = await _userManager.UpdateAsync(getuser);
            if(result.Succeeded)
                message = AccountOptions.API_Response_Saved;
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
        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser(DeleteUserModel model)
        {
            string message = string.Empty;
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.UserID);
                var result = await _userManager.DeleteAsync(user);
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
    }
}