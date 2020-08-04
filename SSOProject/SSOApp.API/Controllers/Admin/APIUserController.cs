using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.Controllers.UI;
using SSOApp.API.ViewModels;

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
            return await _context.Users.Include(x => x.Tenant).Where(x => !x.isDeleted).Select(user =>
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
        public async Task<bool> SavePassword(string ID, string datacurrent, string data)
        {
            var getuser = await _userManager.FindByIdAsync(ID);

            var result = await _userManager.ChangePasswordAsync(getuser, datacurrent, data);
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
        public async Task<List<UserViewModel>> GetUsersByRole(string rid)
        {
            var result = new UserToRolesViewModel();
            var getuser = await (from ur in _context.UserRoles
                                 join u in _context.Users on ur.UserId equals u.Id
                                 join r in _context.Roles on ur.RoleId equals r.Id
                                 where r.Id == rid
                                 select new UserViewModel
                                 {
                                     UserID = u.Id,
                                     Email = u.Email,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,                                     
                                     TenanntCode = u.TenantCode,
                                 }).ToListAsync();
            return getuser;
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
            var getusers = await _context.Users.Include(x => x.Tenant).Where(x => !x.isDeleted && x.TenantCode == code).Select(user =>
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
            return getusers;
        }

        [HttpPost("saveuser")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                if (model.UserID == null)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    var checktenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenanntCode);

                    if (user != null)
                    {
                        responseCode = "Exists";
                        responseMessage = AccountOptions.API_Response_Exist;
                    }
                    else
                    {
                        var user1 = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            TenantCode = model.TenanntCode,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsActive = true
                        };
                        var createduser = await _userManager.CreateAsync(user1, model.Password);
                        if (createduser.Succeeded)
                        {
                            responseCode = "Success";
                            responseMessage = string.Format(AccountOptions.API_Response_Saved, "User");
                        }
                        else
                        {
                            responseCode = "Error";
                            responseMessage = string.Format(AccountOptions.API_Response_Failed, "User");
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
                    user.TenantCode = model.TenanntCode;

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        responseCode = "Success";
                        responseMessage = string.Format(AccountOptions.API_Response_Saved, "User");
                    }
                    else
                    {
                        responseCode = "Error";
                        responseMessage = string.Format(AccountOptions.API_Response_Failed, "User");
                    }
                }
            }
            catch (Exception ex)
            {
                responseCode = "Error";
                responseMessage = string.Format(AccountOptions.API_Response_Failed, ex.Message);
            }

            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            });
        }

        //call from Roles
        [HttpPost("saveuserrole")]
        public async Task<IActionResult> SaveUserRoles(UserToRolesViewModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;

            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleID);
                var previoususers = await GetUsersByRole(model.RoleID);
                foreach (var item in previoususers)
                {
                    _context.UserRoles.Remove(new IdentityUserRole<string> { RoleId = model.RoleID, UserId = item.UserID });
                    _context.SaveChanges();
                }
                foreach (var item in model.SelectedUsers)
                {
                    var getuser = await _userManager.FindByNameAsync(item);
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = model.RoleID, UserId = getuser.Id });
                    _context.SaveChanges();
                }
                responseCode = "Saved";
                responseMessage = string.Format(AccountOptions.API_Response_Saved, "User Role");
            }
            catch (Exception ex)
            {
                responseCode = "Error";
                responseMessage = string.Format(AccountOptions.API_Response_Saved, ex.Message);
            }

            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            }); ;
        }

        [HttpGet("savelastlogintime")]
        public async Task<IActionResult> SaveLastLogin(string username)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;

            try
            {
                var getuser = await _userManager.FindByNameAsync(username);
                getuser.LastLoginTime = DateTime.Now;
                var result = await _userManager.UpdateAsync(getuser);
                if (result.Succeeded)
                {
                    responseCode = "Success";
                    responseMessage = string.Format(AccountOptions.API_Response_Saved, "Last Login");
                }
                else
                {
                    responseCode = "failed";
                    responseMessage = string.Format(AccountOptions.API_Response_Failed, "Last Login");
                }
            }
            catch (Exception ex)
            {
                responseCode = "failed";
                responseMessage = string.Format(AccountOptions.API_Response_Failed, "Last Login: " + ex.Message);
            }
            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            }); ;
        }

        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser(DeleteUserModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.UserID);
                user.isDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    responseCode = "Success";
                    responseMessage = string.Format(AccountOptions.API_Response_Deleted, "User");
                }
                else
                {
                    responseCode = "failed";
                    responseMessage = string.Format(AccountOptions.API_Response_Failed, "User");
                }
            }
            catch (Exception ex)
            {
                responseCode = "failed";
                responseMessage = string.Format(AccountOptions.API_Response_Failed, "Delete User: " + ex.Message);
            }
            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            }); ;
        }
    }
}