using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSOApp.API.ViewModels;
using SSOApp.Controllers.UI;

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
        public async Task<List<ClaimsViewModel>> Index(string tenantId)
        {
            var getclaims = await _context.TenantClaims.Where(x => x.TenantID == new Guid(tenantId)).Select(x => new ClaimsViewModel
            {
                ID = x.ID,
                Name = x.ClaimName,
                IsActive = x.IsAvailable
            }).ToListAsync();

            return getclaims;
        }


        [HttpPost("saveclaim")]
        public async Task<IActionResult> SaveClaim(ClaimsViewModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                if (model.ID != Guid.Empty)
                {
                    //Update
                    var getclaimbyid = await _context.TenantClaims.SingleOrDefaultAsync(d => d.ID == model.ID && d.TenantID == model.TenantID);
                    if (model.Name != getclaimbyid.ClaimName)
                    {
                        if (await _context.TenantClaims.AnyAsync(x => x.ClaimName == model.Name))
                        {
                            responseCode = "Exists";
                            responseMessage = string.Format(AccountOptions.API_Response_Exist, "Claim");
                        }
                        else
                        {
                            getclaimbyid.ClaimName = model.Name;
                            getclaimbyid.IsAvailable = true;
                            await _context.SaveChangesAsync();
                            responseCode = "Success";
                            responseMessage = string.Format(AccountOptions.API_Response_Saved, "Claim");
                        }
                    }                   
                }
                else
                {
                    //Add
                    if (!await _context.TenantClaims.AnyAsync(x => x.ClaimName == model.Name))
                    {
                        //Doesnot exist     //Add new
                        var claim = new TenantClaims();
                        claim.ClaimName = model.Name;
                        claim.TenantID = model.TenantID;
                        claim.IsAvailable = true;
                        await _context.TenantClaims.AddAsync(claim);
                        await _context.SaveChangesAsync();
                        responseCode = "Success";
                        responseMessage = string.Format(AccountOptions.API_Response_Saved, "Claim");
                    }
                    else
                    {
                        responseCode = "Exists";
                        responseMessage = string.Format(AccountOptions.API_Response_Exist, "Claim");
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

        [HttpGet("getclaimbyid")]
        public async Task<ClaimsViewModel> GetRole(string ID, string tcode)
        {
            return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(x =>
                   new ClaimsViewModel
                   {
                       Name = x.ClaimName,
                       ID = x.ID,
                       TenantID = new Guid(tcode)
                   }).SingleOrDefaultAsync();
        }

        [HttpPost("deleteclaim")]
        public async Task<IActionResult> DeleteClaim(ClaimsViewModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                var claim = await _context.TenantClaims.FirstOrDefaultAsync(x => x.ID == model.ID);
                _context.TenantClaims.Remove(claim);
                var result = await _context.SaveChangesAsync();

                responseCode = "Success";
                responseMessage = string.Format(AccountOptions.API_Response_Deleted, "Claim");
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

        [HttpGet("getallclaimssbytenant")]
        public async Task<List<ClaimsViewModel>> RolesbyTenant(string tcode)
        {
            var result = await _context.TenantClaims.Where(x => x.TenantID == new Guid(tcode))
                .Select(y => new ClaimsViewModel
                {
                    ID = y.ID,
                    Name = y.ClaimName,
                    IsActive = y.IsAvailable,
                    TenantID = y.TenantID
                }).ToListAsync();

            return result;
        }

    }
}