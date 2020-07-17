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
using SSOApp.Controllers.UI;
using SSOApp.ViewModels;

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

        [HttpGet("getallmodules")]
        public async Task<List<ModuleViewModel>> Index()
        {
            var result = new List<ModuleViewModel>();
            var getclaims = await _context.ModuleDetails.ToListAsync();
            foreach (var item in getclaims)
            {
                result.Add(new ModuleViewModel
                {
                    Name = item.LableName,
                    TableName = item.TableName,
                    ID = item.ID.ToString(),
                });
            }
            return result;
        }

        private async Task<bool> IsTenantCodeAvailable(string code)
        {
            var t =  _context.Tenants.ToList();
            var getcode = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == code);
            if (getcode != null)
                return true;

            return false;
        }

        private async Task<string> CheckExistingModule(string cName)
        {
            var checkalreadyexist = await _context.ModuleDetails.AnyAsync(x => x.LableName == cName);
            if (checkalreadyexist)
            {
                return AccountOptions.API_Response_Exist;
            }
            else
            {
                return string.Empty;
            }
        }

        //private async Task<string> CheckExistingFieldModule(string cName)
        //{
        //    var checkalreadyexist = await _context.ModuleFieldDetails.AnyAsync(x => x.FieldName == cName);
        //    if (checkalreadyexist)
        //    {
        //        return AccountOptions.API_Response_Exist;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        [HttpPost("savemodule")]
        public async Task<IActionResult> SaveModule(ModuleViewModel model)
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
                        var getclaimbyid = await _context.ModuleDetails.SingleOrDefaultAsync(d => d.ID == new Guid(model.ID));
                        if (getclaimbyid.LableName != model.Name)
                        {
                            //Check role exists
                            var checkalreadyexist = await CheckExistingModule(model.Name);
                            if (!string.IsNullOrEmpty(checkalreadyexist))
                            {
                                //exists
                                message = AccountOptions.API_Response_Exist;
                            }
                            else
                            {
                                //Does not exist    //Update role                                
                                getclaimbyid.LableName = model.Name;
                                await _context.SaveChangesAsync();
                                message = AccountOptions.API_Response_Saved;
                            }
                        }
                    }
                    else
                    {
                        //Add
                        if (string.IsNullOrEmpty(await CheckExistingModule(model.Name)))
                        {
                            //Doesnot exist     //Add new
                            var PstenantId =  _context.Tenants.Where(d => d.Code == model.TenantCode).Select(d=>d.Id).FirstOrDefault();
                            var claim = new ModuleDetails();
                            claim.LableName
                                = model.Name;
                            claim.TableName = model.TableName;
                            claim.TenantId =PstenantId;
                            //TODO: 
                            claim.ID = Guid.NewGuid();
                            try
                            {

                                CreateTable(model.TableName);
                                await _context.ModuleDetails.AddAsync(claim);
                            }
                            catch (Exception ex)
                            {

                            }

                            await _context.SaveChangesAsync();
                            var getrole = await _context.ModuleDetails.SingleOrDefaultAsync(d => d.LableName == model.Name);
                            model.ID = getrole.ID.ToString();
                            message = AccountOptions.API_Response_Saved;
                        }
                        else
                            message = AccountOptions.API_Response_Exist;
                    }
                    if (message == AccountOptions.API_Response_Saved || message == string.Empty)
                    {
                        //Save to tenantrole                        
                        //var gettenant = await _context.Tenants.FirstOrDefaultAsync(d => d.Code == model.TenantCode);
                        //var gettenantroles = await _context.TenantClaims.FirstOrDefaultAsync(d => d.TenantID == gettenant.Id && d.ID == new Guid(model.ID));
                        //if (gettenantroles == null)    //Add new tenant role
                        //  _context.TenantRoles.Add(new TenantRoles { RoleID = model.ID, TenantID = gettenant.Id });
                        //else
                        {
                            //  gettenantroles.ID = new Guid(model.ID);
                            //gettenantroles.TenantID = gettenant.Id;
                        }
                        //_context.SaveChanges();
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

                            var ModuleFieldOptions =_context.ModuleFieldOptions.Where(d => d.ModuleFieldDetailsID == model.ModuleFieldDetails.ID).ToList();
                            if (ModuleFieldOptions.Count>0)
                            {
                                _context.ModuleFieldOptions.RemoveRange(ModuleFieldOptions);
                                await _context.SaveChangesAsync();

                            }
                            if(model.ModuleFieldOption!=null)
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
                                message = "DBFieldName Already"+ AccountOptions.API_Response_Exist;
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

        //getFeildListByFeildId
        [HttpGet("getFeildListByFeildId")]
         public async Task<FeildModelView> GetFeildListByFeildId(string FeildId)
         {
            var psFieldGuid = new Guid(FeildId);
            try
            {
                FieldViewModel =  (from MFD in _context.ModuleFieldDetails
                                    //join l in _context.ModuleFieldOptions on MFD.ID equals l.FieldId into p
                                    //from MFO in p.DefaultIfEmpty()
                                    where MFD.ID==psFieldGuid
                                     select new FeildModelView
                                    {
                                         ModuleFieldDetails = new ModuleFieldDetails
                                        {
                                            ID = MFD.ID,
                                            DBFieldName = MFD.DBFieldName,
                                            FieldType = MFD.FieldType,
                                            TenantCode = MFD.TenantCode,
                                            visible = MFD.visible,
                                            FieldLabel = MFD.FieldLabel,
                                            ModuleDetailsID=MFD.ModuleDetailsID
                                         }
                                        ,
                                        ModuleFieldOption = _context.ModuleFieldOptions.Where(m=>m.ModuleFieldDetailsID==psFieldGuid).ToList()

                                    }).FirstOrDefault();

            }
            catch(Exception ex)
            {

            }

            return FieldViewModel;
        }

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

        [HttpGet("getclaimbyid")]
        public async Task<ModuleViewModel> GetRole(string ID, string tcode)
        {
            return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(role =>
                   new ModuleViewModel
                   {
                       Name = role.ClaimValue,
                       ID = role.ID.ToString(),
                       TenantCode = tcode
                   }).SingleOrDefaultAsync();
        }


        [HttpGet("getclaimbyid")]
        public async Task<ModuleViewModel> GetFields(string ID, string tcode)
        {
            return await _context.TenantClaims.Where(d => d.ID == new Guid(ID)).Select(role =>
                   new ModuleViewModel
                   {
                       Name = role.ClaimValue,
                       ID = role.ID.ToString(),
                       TenantCode = tcode
                   }).SingleOrDefaultAsync();
        }

        public DbConnection GetConnection()
        {
            string conString = _context.Database.GetDbConnection().ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            return con;
        }


        [HttpGet("getmodulefieldlistbyid")]
        public async Task<List<ModuleFieldDetails>> GetFieldsList(string ID, string tcode,string moduleId)
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

        private List<FieldViewModel> PrintSchemaPlain(DataTable schemaTable)
        {
            List<FieldViewModel> fields = new List<FieldViewModel>();
            foreach (DataRow row in schemaTable.Rows)
            {
            //    FieldViewModel field = new FieldViewModel();
            //    field.Name = row.Field<string>("ColumnName");
            //    field.Type = row.Field<Type>("DataType").ToString();
            //    fields.Add(field);
            }

            return fields;
        }
        [HttpPost("deleteclaim")]
        public async Task<IActionResult> DeleteClaim(ModuleViewModel model)
        {
            string message = string.Empty;
            try
            {
                var claim = await _context.TenantClaims.FirstOrDefaultAsync(x => x.ID == new Guid(model.ID));
                _context.TenantClaims.Remove(claim);
                var result = _context.SaveChanges();

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

        [HttpGet("getallmodulesbytenant")]
        public async Task<List<ModuleViewModel>> RolesbyTenant(string tcode)
        {
            return await ClaimsbyTenantList(tcode);
        }

        private async Task<List<ModuleViewModel>> ClaimsbyTenantList(string tcode)
        {
            var result = await (from d in _context.ModuleDetails
                                join t in _context.Tenants on d.TenantId equals t.Id
                                where t.Code == tcode
                                select new ModuleViewModel
                                {
                                    ID = d.ID.ToString(),
                                    Name = d.LableName,
                                    TenantCode = t.Code,
                                    TenantName = t.Name
                                }).ToListAsync();

            return result;
        }

        [HttpPost("saveTenantRole")]
        public async Task<IActionResult> SaveTenantRole(TenantRoleModel model)
        {
            List<TenantRoles> _TenantRoles = new List<TenantRoles>();

            foreach(var items in model.SelectedRoleID)
            {
                _TenantRoles.Add(new TenantRoles { TenantID = model.TennantId, ModuleID = model.ModuleId.ToString(), RoleID = items });
            }

            string message = string.Empty;
            try
            {
                _context.TenantRoles.AddRange(_TenantRoles);
                var result =await _context.SaveChangesAsync();
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

    }
}