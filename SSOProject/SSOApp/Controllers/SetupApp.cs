using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.SQLServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSOApp.Models;
using SSOApp.ViewModels;

namespace SSOApp.Controllers
{
    public class SetupApp
    {
        public const int UserScreens = 3;
        public static async Task SetupRolesAndAdmin()
        {
            using (var client = new HttpClient())
            {
                //createrolesandadmin
                client.BaseAddress = new Uri("https://localhost:44391/APIAdmin/createrolesandadmin");
                var getTask = await client.GetAsync(client.BaseAddress);
            }
        }
        public static async Task SetupAdmin()
        {
            using (var client = new HttpClient())
            {
                //createrolesandadmin
                client.BaseAddress = new Uri("https://localhost:44391/APIAdmin/createdefaultadmin");
                var getTask = await client.GetAsync(client.BaseAddress);
            }
        }

        public static async Task<SecureAPIReturnedModel> SecureAPIGetUser(string rname, string userid)
        {
            APIReturnedModel result = await SecureAPILogin(userid);
            SecureAPIReturnedModel model = new SecureAPIReturnedModel();
            if (!string.IsNullOrEmpty(result.token))
            {
                model.User = await GetUserByID(userid, result.token);
                model.UserInRole = await IsUserInRole(model.User.UserName, rname, result.token);
                model.Tenant = await GetTenantByUser(model.User.UserName, result.token);
            }
            return model;
        }
        
        public static async Task<ApplicationUser> GetUserByID(string userid,string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5001/APIService/getuserbyid?id=" + userid);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    var resultjson = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);
                    return resultjson;
                }
            }
            catch { }
            return new ApplicationUser();
        }
        public static async Task<bool> IsUserInRole(string username,string rname, string token)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5001/APIService/userisinrole?username=" + username + "&rolename=" + rname);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    //postTask.Wait();
                    var resultjson = JsonConvert.DeserializeObject<bool>(apiResponse);
                    return resultjson;
                }
            }
            catch { }
            return false;
        }
        public static async Task<Tenant> GetTenantByUser(string username, string token)
        {
            try
            {
                using (var client = new HttpClient())
                  {
                    client.BaseAddress = new Uri("http://localhost:5001/APIService/gettenantbyuser?username=" + username);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var postTask = await client.GetAsync(client.BaseAddress);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    //postTask.Wait();
                    var resultjson = JsonConvert.DeserializeObject<Tenant>(apiResponse);
                    return resultjson;
                }
            }
            catch { }
            return new Tenant();
        }
        public static async Task<APIReturnedModel> SecureAPILogin(string userid)
        {
            try
            {              
                using (var client = new HttpClient())
                {
                    //Check tenant code from api.
                    APILoginViewModel model = new APILoginViewModel { UserID = userid };

                    client.BaseAddress = new Uri("http://localhost:5001/Identity/login");
                    APILoginViewModel apimodel = new APILoginViewModel { Username = model.UserID };
                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var postTask = await client.PostAsync(client.BaseAddress, stringContent);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    //postTask.Wait();
                    var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                    return resultjson;
                }
            }
            catch { }
            return new APIReturnedModel();
        }
    }
}