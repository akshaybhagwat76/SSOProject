using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SSOApp.Proxy;
using SSOApp.API.ViewModels;

namespace SSOApp.Controllers
{
    public class GenericController : Home.BaseController
    {
        public GenericController(ApplicationDbContext context, IAPIClientProxy clientProxy) : base(context, clientProxy)
        {
        }

        public IActionResult Index(string moduleId)
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";

            var module = _context.ModuleDetails.FirstOrDefault(x => x.ID == new Guid(moduleId));
            var details = GetAllModuleItem(module.ID.ToString(), module.ModuleLabel);
            var claimDetails = (from c in _context.TenantClaims
                                join trc in _context.RoleModuleClaim on c.ID equals trc.ClaimID
                                where trc.ModuleID.ToString() == moduleId && trc.RoleID.ToString() == RoleId && trc.TenantId == TenantId
                                select new TenantClaims
                                {
                                    ClaimName = c.ClaimName
                                }).ToList();
            ModuleFieldValueListViewModel indexData = new ModuleFieldValueListViewModel
            {
                ID = module.ID,
                ModuleName = module.ModuleName,
                ModuleLabel = module.ModuleLabel,
                List = details,
                UserClaim = claimDetails ?? new List<TenantClaims>()

            };
            return View(indexData);
        }

        public IActionResult Create(string moduleId, string recId = null)
        {
            TempData["TenaneDetails"] = $"Tenant: {TenantName} (Code: {TenantCode})";
            //TODO: Find ROle ID for claim
            var module = _context.ModuleDetails.FirstOrDefault(x => x.ID == new Guid(moduleId));
            var modulefield = _context.ModuleFieldDetails.Where(x => x.ModuleDetailsID == module.ID)
                .Select(x => new ModuleFieldDetailsVIewModel
                {
                    DBFieldName = x.DBFieldName,
                    FieldLabel = x.FieldLabel,
                    FieldType = x.FieldType,
                    ID = x.ID
                }).ToList();
            var claimDetails = _context.RoleModuleClaim.Where(x => x.ModuleID.ToString() == moduleId &&
            x.TenantId == TenantId && x.RoleID.ToString() == RoleId).ToList();
            var genericFormViewModel = new GenericFormViewModel
            {
                SelectedRowId = recId,
                ModuleId = module.ID.ToString(),
                ModuleLabel = module.ModuleLabel,
                ModuleName = module.ModuleName,
                Options = new List<ModuleFieldOptions>(),
                Fields = modulefield,
                Claims = claimDetails
            };

            foreach (var field in modulefield)
            {
                if (!string.IsNullOrEmpty(recId))
                {
                    var selectedRow = GetAllModuleItem(module.ID.ToString(), module.ModuleLabel, recId);
                    if (selectedRow.Tables != null)
                    {
                        var value = Convert.ToString(selectedRow.Tables[0].Rows[0][field.DBFieldName]);
                        field.FieldValue = value;
                    }
                }

                var options = _context.ModuleFieldOptions.Where(x => x.ModuleFieldDetailsID == field.ID).ToList();
                genericFormViewModel.Options.AddRange(options);
            }
            return View(genericFormViewModel);
        }

        public IActionResult Save(Microsoft.AspNetCore.Http.IFormCollection inputData)
        {
            //Formate = {FiledName#DataType#FiledValue}
            var formData = inputData["formInput"][0].Split(",");
            StringBuilder columnName = new StringBuilder();
            StringBuilder columnValue = new StringBuilder();
            var tableName = inputData["moduleName"][0];
            var moduleId = inputData["moduleId"][0];
            var selectedRowId = inputData["selectedRowId"][0];

            foreach (var m in formData)
            {
                var values = m.Split("#");
                columnName.Append($"{values[0]},");
                columnValue.Append($"'{values[2].ToString().Replace('^', ',')}',");
            }
            string StrQuery = string.Empty;
            if (string.IsNullOrEmpty(selectedRowId))
                StrQuery = $"Insert into {tableName} ({columnName.ToString().Substring(0, columnName.Length - 1)}) Values ({columnValue.ToString().Substring(0, columnValue.Length - 1)})";
            else
            {
                var field = string.Empty;
                foreach (var str in formData)
                {
                    var values = str.Split("#");
                    field += values[0] + "='" + values[2].Replace('^',',') + "',";
                }
                StrQuery = $"Update {tableName} Set {field.Substring(0, field.Length - 1)}  where ID = {selectedRowId}";
            }

            UpdateDatainDB(StrQuery);
            return RedirectToAction("Index", new { moduleId = moduleId });

        }

        public IActionResult Delete(string moduleId, string recID)
        {
            var module = _context.ModuleDetails.FirstOrDefault(x => x.ID == new Guid(moduleId));
            string query = $"Delete from {module.ModuleLabel} where ID = {recID}";
            UpdateDatainDB(query);
            return RedirectToAction("Index", new { moduleId = moduleId });
        }
        public DataSet GetAllModuleItem(string moduleID, string moduleName, string recID = null)
        {
            string conString = _context.Database.GetDbConnection().ConnectionString;
            DataSet dset = new DataSet();
            SqlConnection con = new SqlConnection(conString);
            string query = string.Empty;
            if (string.IsNullOrEmpty(recID))
                query = $"Select * from {moduleName}";
            else
                query = $"select * from {moduleName} where ID = {recID}";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                //Creating instance of DataSet
                SqlDataAdapter dadapter = new SqlDataAdapter(query, con);
                dadapter.Fill(dset, moduleName);
                return dset;
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
                throw e;
            }
        }

        public void UpdateDatainDB(string query)
        {
            string conString = _context.Database.GetDbConnection().ConnectionString;
            SqlConnection con = new SqlConnection(conString);
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
    }

    public class GenericFormViewModel
    {
        public string SelectedRowId { get; set; }

        public string ModuleId { get; set; }

        public string ModuleName { get; set; }

        public string ModuleLabel { get; set; }

        public List<ModuleFieldDetailsVIewModel> Fields { get; set; }

        public List<ModuleFieldOptions> Options { get; set; }

        public List<RoleModuleClaim> Claims { get; set; }
    }
}
