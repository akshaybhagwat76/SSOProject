using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SSOApp.Controllers.UI;
using SSOApp.API.ViewModels;

namespace SSOApp.API.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class APIModulesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public FeildModelView FieldViewModel { get; set; }

        public APIModulesController(ApplicationDbContext context)
        {
            _context = context;
            FieldViewModel = new FeildModelView();
            FieldViewModel.ModuleFieldDetails = new ModuleFieldDetails();
            FieldViewModel.ModuleFieldDetailsList = new List<ModuleFieldDetails>();
            FieldViewModel.FieldType = new SelectList(_context.FieldTypes.ToList(), "ID", "Name");


        }

        private async Task<string> CheckExistingClaim(string cName)
        {
            var checkalreadyexist = await _context.TenantClaims.AnyAsync(x => x.ClaimName == cName);
            if (checkalreadyexist)
            {
                return AccountOptions.API_Response_Exist;
            }
            else
            {
                return string.Empty;
            }
        }

        [HttpGet("getallmodules")]
        public async Task<List<ModuleViewModel>> Index()
        {
            var result = new List<ModuleViewModel>();
            var getclaims = await _context.ModuleDetails.ToListAsync();
            foreach (var item in getclaims)
            {
                result.Add(new ModuleViewModel
                {
                    ModuleName = item.ModuleName,
                    ModuleLabel = item.ModuleLabel,
                    ID = item.ID,
                });
            }
            return result;
        }

        [HttpGet("getmodulebyid")]
        public async Task<ModuleViewModel> GetModuleByID(string moduleId, string tenantId)
        {
            var result = new ModuleViewModel();
            var getclaims = await _context.ModuleDetails.FirstOrDefaultAsync(x => x.ID == new Guid(moduleId) && x.TenantId == new Guid(tenantId));
            result.ModuleName = getclaims.ModuleName;
            result.ModuleLabel = getclaims.ModuleLabel;
            result.ID = getclaims.ID;
            return result;
        }

        private async Task<bool> IsTenantCodeAvailable(string code)
        {
            var t = _context.Tenants.ToList();
            var getcode = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == code);
            if (getcode != null)
                return true;

            return false;
        }

        private async Task<string> CheckExistingModule(string cName)
        {
            var checkalreadyexist = await _context.ModuleDetails.AnyAsync(x => x.ModuleName == cName);
            if (checkalreadyexist)
            {
                return AccountOptions.API_Response_Exist;
            }
            else
            {
                return string.Empty;
            }
        }

        ////private async Task<string> CheckExistingFieldModule(string cName)
        ////{
        ////    var checkalreadyexist = await _context.ModuleFieldDetails.AnyAsync(x => x.FieldName == cName);
        ////    if (checkalreadyexist)
        ////    {
        ////        return AccountOptions.API_Response_Exist;
        ////    }
        ////    else
        ////    {
        ////        return string.Empty;
        ////    }
        ////}

        [HttpPost("savemodule")]
        public async Task<IActionResult> SaveModule(ModuleViewModel model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {

                if (model.ID != Guid.Empty)
                {
                    //Update
                    var getclaimbyid = await _context.ModuleDetails.SingleOrDefaultAsync(d => d.ID == model.ID);
                    if (getclaimbyid.ModuleName != model.ModuleName)
                    {
                        //Check role exists
                        var checkalreadyexist = await CheckExistingModule(model.ModuleName);
                        if (!string.IsNullOrEmpty(checkalreadyexist))
                        {
                            responseCode = "Exists";
                            responseMessage = string.Format(AccountOptions.API_Response_Exist, "Module");
                        }
                        else
                        {
                            //Does not exist    //Update role                                
                            getclaimbyid.ModuleName = model.ModuleName;
                            await _context.SaveChangesAsync();
                            responseCode = "Success";
                            responseMessage = string.Format(AccountOptions.API_Response_Saved, "Claim");
                        }

                    }
                }
                else
                {
                    //Add
                    if (string.IsNullOrEmpty(await CheckExistingModule(model.ModuleName)))
                    {
                        var module = new ModuleDetails
                        {
                            ModuleName = model.ModuleName,
                            ModuleLabel = model.ModuleLabel,
                            TenantId = model.Tenant.Id,
                            ID = Guid.NewGuid()
                        };

                        try
                        {
                            CreateTable(model.ModuleLabel);
                            await _context.ModuleDetails.AddAsync(module);
                        }
                        catch (Exception ex)
                        {

                        }
                        await _context.SaveChangesAsync();
                        responseCode = "Success";
                        responseMessage = string.Format(AccountOptions.API_Response_Saved, "Module");
                    }
                    else
                    {
                        responseCode = "Exists";
                        responseMessage = string.Format(AccountOptions.API_Response_Exist, "Module");
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

        [HttpPost("saveFieldmodule")]
        public async Task<IActionResult> SaveFieldmodule(FeildModelView model)
        {
            string message = string.Empty;
            //var PsModuleId = new Guid(model.ModuleId);
            try
            {
                bool checkcode = await IsTenantCodeAvailable(model.TenantCode);
                if (checkcode)
                {
                    if (model.ModuleFieldDetails != null)
                    {
                        var ModuleFieldDetails = await _context.ModuleFieldDetails.SingleOrDefaultAsync(d => d.ID == model.ModuleFieldDetails.ID);// && d.ModeuleDetailId== PsModuleId);

                        if (ModuleFieldDetails != null)
                        {

                            //Does not exist    //Update role                                
                            ModuleFieldDetails.DBFieldName = model.ModuleFieldDetails.DBFieldName;
                            ModuleFieldDetails.FieldLabel = model.ModuleFieldDetails.FieldLabel;
                            ModuleFieldDetails.FieldType = model.ModuleFieldDetails.FieldType;
                            ModuleFieldDetails.TenantCode = model.TenantCode;
                            _context.ModuleFieldDetails.Update(ModuleFieldDetails);
                            await _context.SaveChangesAsync();

                            var ModuleFieldOptions = _context.ModuleFieldOptions.Where(d => d.ModuleFieldDetailsID == model.ModuleFieldDetails.ID).ToList();
                            if (ModuleFieldOptions.Count > 0)
                            {
                                _context.ModuleFieldOptions.RemoveRange(ModuleFieldOptions);
                                await _context.SaveChangesAsync();

                            }
                            if (model.ModuleFieldOption != null)
                            {
                                model.ModuleFieldOption.ForEach(z => z.ModuleFieldDetailsID = model.ModuleFieldDetails.ID);
                                _context.ModuleFieldOptions.AddRange(model.ModuleFieldOption);
                                await _context.SaveChangesAsync();
                            }

                            message = AccountOptions.API_Response_Saved;
                        }
                        else
                        {
                            //Add
                            //checkk if exist   
                            if (_context.ModuleFieldDetails.Any(x => x.DBFieldName == model.ModuleFieldDetails.DBFieldName))
                            {
                                message = "DBFieldName Already" + AccountOptions.API_Response_Exist;
                            }
                            else if (_context.ModuleFieldDetails.Any(x => x.FieldLabel == model.ModuleFieldDetails.FieldLabel))
                            {
                                message = "Field Label Already" + AccountOptions.API_Response_Exist;
                            }
                            else
                            {
                                //save [ModuleField]
                                //model.ModuleFieldDetails.ModuleDetailsID= PsModuleId;
                                model.ModuleFieldDetails.TenantCode = model.TenantCode;
                                _context.ModuleFieldDetails.Add(model.ModuleFieldDetails);
                                await _context.SaveChangesAsync();

                                //save [ModuleFieldOptions]
                                if (model.ModuleFieldOption != null)
                                {
                                    model.ModuleFieldOption.ForEach(z => z.ModuleFieldDetailsID = model.ModuleFieldDetails.ID);
                                    _context.ModuleFieldOptions.AddRange(model.ModuleFieldOption);
                                    await _context.SaveChangesAsync();
                                }

                                message = AccountOptions.API_Response_Saved;
                            }
                        }

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

        //Call from ROle Screen for setting module
        [HttpPost("saverolesmodule")]
        public async Task<IActionResult> SaveRoleModules(AssignmentSaveViewModule model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                //Save to tenantrole                        
                var moduleList = _context.ModuleRoles.Where(x => x.RoleID == model.SelectedValue).ToList();
                if (moduleList != null && moduleList.Count > 0)
                    _context.ModuleRoles.RemoveRange(moduleList);
                foreach (var module in model.ListofAssignment)
                {
                    var roleModule = new RoleModules { RoleID = model.SelectedValue, ModuleID = new Guid(module), TenantId = model.TenantId };
                    await _context.ModuleRoles.AddAsync(roleModule);
                    await _context.SaveChangesAsync();
                }
                responseCode = "Success";
                responseMessage = string.Format(AccountOptions.API_Response_Saved, "Claim");
            }
            catch (Exception ex)
            {
                responseCode = "Failed";
                responseMessage = string.Format(AccountOptions.API_Response_Failed, ex.Message);
            }

            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            });
        }

        [HttpPost("savemoduleroles")]
        public async Task<IActionResult> SaveModuleRoles(AssignmentSaveViewModule model)
        {
            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            try
            {
                //Save to tenantrole                        
                var moduleList = _context.ModuleRoles.Where(x => x.ModuleID == new Guid(model.SelectedValue)).ToList();
                if (moduleList != null && moduleList.Count > 0)
                    _context.ModuleRoles.RemoveRange(moduleList);
                foreach (var module in model.ListofAssignment)
                {
                    var roleModule = new RoleModules { RoleID = module, ModuleID = new Guid(model.SelectedValue), TenantId = model.TenantId };
                    await _context.ModuleRoles.AddAsync(roleModule);
                    await _context.SaveChangesAsync();
                }
                responseCode = "Success";
                responseMessage = string.Format(AccountOptions.API_Response_Saved, "Claim");
            }
            catch (Exception ex)
            {
                responseCode = "Failed";
                responseMessage = string.Format(AccountOptions.API_Response_Failed, "Claim");
            }

            return Ok(new
            {
                MessageCode = responseCode,
                MessageDetails = responseMessage
            });
        }

        //getFeildListByFeildId
        //[HttpGet("getFeildListByFeildId")]
        //public async Task<FeildModelView> GetFeildListByFeildId(string FeildId)
        //{
        //    var psFieldGuid = new Guid(FeildId);
        //    try
        //    {
        //        FieldViewModel = (from MFD in _context.ModuleFieldDetails
        //                              //join l in _context.ModuleFieldOptions on MFD.ID equals l.FieldId into p
        //                              //from MFO in p.DefaultIfEmpty()
        //                          where MFD.ID == psFieldGuid
        //                          select new FeildModelView
        //                          {
        //                              ModuleFieldDetails = new ModuleFieldDetails
        //                              {
        //                                  ID = MFD.ID,
        //                                  DBFieldName = MFD.DBFieldName,
        //                                  FieldType = MFD.FieldType,
        //                                  TenantCode = MFD.TenantCode,
        //                                  visible = MFD.visible,
        //                                  FieldLabel = MFD.FieldLabel,
        //                                  ModuleDetailsID = MFD.ModuleDetailsID
        //                              }
        //                             ,
        //                              ModuleFieldOption = _context.ModuleFieldOptions.Where(m => m.ModuleFieldDetailsID == psFieldGuid).ToList()

        //                          }).FirstOrDefault();

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return FieldViewModel;
        //}

        private void CreateTable(string tableName)
        {
            string conString = _context.Database.GetDbConnection().ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            string query =
            @"CREATE TABLE " + tableName + "(ID int IDENTITY(1,1) NOT NULL); ";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        //[HttpGet("getclaimbyid")]
        //public async Task<ModuleViewModel> GetRole(string ID, string tcode)
        //{
        //    return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(role =>
        //           new ModuleViewModel
        //           {
        //               Name = role.ClaimValue,
        //               ID = role.ID.ToString(),
        //               TenantCode = tcode
        //           }).SingleOrDefaultAsync();
        //}


        //[HttpGet("getclaimbyid")]
        //public async Task<ModuleViewModel> GetFields(string ID, string tcode)
        //{
        //    return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(role =>
        //           new ModuleViewModel
        //           {
        //               Name = role.ClaimValue,
        //               ID = role.ID.ToString(),
        //               TenantCode = tcode
        //           }).SingleOrDefaultAsync();
        //}

        //public DbConnection GetConnection()
        //{
        //    string conString = _context.Database.GetDbConnection().ConnectionString;
        //    SqlConnection con = new SqlConnection(conString);
        //    return con;
        //}


        [HttpGet("getmodulefieldlistbyid")]
        public async Task<List<ModuleFieldDetails>> GetFieldsList(string ID, string tcode, string moduleId)
        {
            //var psmoduleID = new Guid(moduleId);
            var TenantName = _context.Tenants.Where(x => x.Code == tcode).Select(x => x.Name).FirstOrDefault();
            var ModuleFieldDetails = await (from md in _context.ModuleFieldDetails
                                            join ft in _context.FieldTypes on md.FieldType equals ft.Name
                                            where md.TenantCode == tcode
                                            select new ModuleFieldDetails
                                            {
                                                ID = md.ID,
                                                FieldLabel = md.FieldLabel,
                                                FieldType = ft.Name,
                                                visible = md.visible,
                                                DBFieldName = md.DBFieldName,
                                                TenantCode = md.TenantCode,
                                                //ModeuleDetailId=psmoduleID
                                            }).ToListAsync();


            return ModuleFieldDetails;
            //return await _context.ModuleFieldDetails.ToListAsync(); //feildModelView;
        }

        //private List<FieldViewModel> PrintSchemaPlain(DataTable schemaTable)
        //{
        //    List<FieldViewModel> fields = new List<FieldViewModel>();
        //    foreach (DataRow row in schemaTable.Rows)
        //    {
        //    //    FieldViewModel field = new FieldViewModel();
        //    //    field.Name = row.Field<string>("ColumnName");
        //    //    field.Type = row.Field<Type>("DataType").ToString();
        //    //    fields.Add(field);
        //    }

        //    return fields;
        //}

        //[HttpPost("deleteclaim")]
        //public async Task<IActionResult> DeleteClaim(ModuleViewModel model)
        //{
        //    string message = string.Empty;
        //    try
        //    {
        //        var claim = await _context.TenantClaims.FirstOrDefaultAsync(x => x.ID == new Guid(model.ID));
        //        _context.TenantClaims.Remove(claim);
        //        var result = _context.SaveChanges();

        //        message = AccountOptions.API_Response_Deleted;

        //    }
        //    catch (Exception ex)
        //    {
        //        message = AccountOptions.API_Response_Exception;
        //    }
        //    return Ok(new
        //    {
        //        Status = message
        //    });
        //}

        [HttpGet("getallmodulesbytenant")]
        public async Task<List<ModuleViewModel>> ModulebyTenant(string tenantCode)
        {
            var result = await _context.ModuleDetails.Where(x => x.Tenant.Id == new Guid(tenantCode)).Include("Tenant")
               .Select(p => new ModuleViewModel
               {
                   ID = p.ID,
                   ModuleName = p.ModuleName,
                   Tenant = p.Tenant
               }).ToListAsync();

            return result;
        }

        [HttpGet("getallmodulesbyrole")]
        public async Task<List<ModuleViewModel>> ModuleByRole(string roleId)
        {
            var moduleList = await (from mr in _context.ModuleRoles
                                    join md in _context.ModuleDetails on mr.ModuleID equals md.ID
                                    where mr.RoleID == roleId
                                    select new ModuleViewModel
                                    {
                                        ID = md.ID,
                                        ModuleName = md.ModuleName
                                    }).ToListAsync();
            return moduleList;
        }


        //[HttpGet("getmoduleclaimbyrole")]
        //public async Task<List<ModuleViewModel>> ModuleClaimByRole(string roleId)
        //{
        //   //var details = from module in _context.ModuleDetails
        //   //              join RoleModuleClaim

        //   // return moduleList;
        //}


        //[HttpPost("saveTenantRole")]
        //public async Task<IActionResult> SaveTenantRole(TenantRoleModel model)
        //{
        //    List<TenantRoles> _TenantRoles = new List<TenantRoles>();

        //    foreach(var items in model.SelectedRoleID)
        //    {
        //        _TenantRoles.Add(new TenantRoles { TenantID = model.TennantId,  RoleID = items });
        //    }

        //    string message = string.Empty;
        //    try
        //    {
        //        _context.TenantRoles.AddRange(_TenantRoles);
        //        var result =await _context.SaveChangesAsync();
        //        message = AccountOptions.API_Response_Saved;

        //    }
        //    catch (Exception ex)
        //    {
        //        message = AccountOptions.API_Response_Exception;
        //    }
        //    return Ok(new
        //    {
        //        Status = message
        //    });
        //}

    }
}