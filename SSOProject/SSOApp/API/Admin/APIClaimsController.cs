using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.Controllers.UI;
using SSOApp.ViewModels;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APIClaimsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public APIClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getallclaims")]
        public async Task<List<ClaimsViewModel>> Index()
        {
            var result = new List<ClaimsViewModel>();
            var getclaims = await _context.TenantClaims.ToListAsync();
            foreach (var item in getclaims)
            {
                result.Add(new ClaimsViewModel
                {
                    Name = item.ClaimValue,
                    ID = item.ID.ToString(),
                });
            }
            return result;
        }

        private async Task<bool> IsTenantCodeAvailable(string code)
        {
            var getcode = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == code);
            if (getcode != null)
                return true;

            return false;
        }

        private async Task<string> CheckExistingClaim(string cName)
        {
            var checkalreadyexist = await _context.TenantClaims.AnyAsync(x => x.ClaimValue == cName);
            if (checkalreadyexist)
            {
                return AccountOptions.API_Response_Exist;
            }
            else
            {
                return string.Empty;
            }
        }

        [HttpPost("saveclaim")]
        public async Task<IActionResult> SaveClaim(ClaimsViewModel model)
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
                        var getclaimbyid = await _context.TenantClaims.SingleOrDefaultAsync(d => d.ID == new Guid(model.ID));
                        if (getclaimbyid.ClaimValue != model.Name)
                        {
                            //Check role exists
                            var checkalreadyexist = await CheckExistingClaim(model.Name);
                            if (!string.IsNullOrEmpty(checkalreadyexist))
                            {
                                //exists
                                message = AccountOptions.API_Response_Exist;
                            }
                            else
                            {
                                //Does not exist    //Update role                                
                                getclaimbyid.ClaimValue = model.Name;
                                await _context.SaveChangesAsync();
                                message = AccountOptions.API_Response_Saved;
                            }
                        }
                    }
                    else
                    {
                        //Add
                        if (string.IsNullOrEmpty(await CheckExistingClaim(model.Name)))
                        {
                            //Doesnot exist     //Add new
                            var claim = new TenantClaims();
                            claim.ClaimValue
                                = model.Name;
                            //TODO: 
                            claim.TenantID = new Guid("7D7B4614-C733-4A6D-A09D-608B4351B827");
                            await _context.TenantClaims.AddAsync(claim);
                            await _context.SaveChangesAsync();
                            var getrole = await _context.TenantClaims.SingleOrDefaultAsync(d => d.ClaimValue == model.Name);
                            model.ID = getrole.ID.ToString();
                            message = AccountOptions.API_Response_Saved;
                        }
                        else
                            message = AccountOptions.API_Response_Exist;
                    }
                    if (message == AccountOptions.API_Response_Saved || message == string.Empty)
                    {
                        //Save to tenantrole                        
                        var gettenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenantCode);
                        var gettenantroles = await _context.TenantClaims.FirstOrDefaultAsync(d => d.TenantID == gettenant.Id && d.ID == new Guid(model.ID));
                        if (gettenantroles == null)    //Add new tenant role
                            await _context.TenantRoles.AddAsync(new TenantRoles { RoleID = model.ID, TenantID = gettenant.Id });
                        else
                        {
                            gettenantroles.ID = new Guid(model.ID);
                            gettenantroles.TenantID = gettenant.Id;
                        }
                        await _context.SaveChangesAsync();
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

        [HttpGet("getclaimbyid")]
        public async Task<ClaimsViewModel> GetRole(string ID, string tcode)
        {
            return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(role =>
                   new ClaimsViewModel
                   {
                       Name = role.ClaimValue,
                       ID = role.ID.ToString(),
                       TenantCode = tcode
                   }).SingleOrDefaultAsync();
        }


        [HttpPost("deleteclaim")]
        public async Task<IActionResult> DeleteClaim(ClaimsViewModel model)
        {
            string message = string.Empty;
            try
            {
                var claim = await _context.TenantClaims.FirstOrDefaultAsync(x => x.ID == new Guid(model.ID));
                _context.TenantClaims.Remove(claim);
                var result = await _context.SaveChangesAsync();

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

        [HttpGet("getallclaimssbytenant")]
        public async Task<List<ClaimsViewModel>> RolesbyTenant(string tcode)
        {
            return await ClaimsbyTenantList(tcode);
        }

        private async Task<List<ClaimsViewModel>> ClaimsbyTenantList(string tcode)
        {
            var result = await (from d in _context.TenantClaims
                                join t in _context.Tenants on d.TenantID equals t.Id
                                where t.Code == tcode
                                select new ClaimsViewModel
                                {
                                    ID = d.ID.ToString(),
                                    Name = d.ClaimValue,
                                    TenantCode = t.Code,
                                    TenantName = t.Name
                                }).ToListAsync();

            return result;
        }
    }
}