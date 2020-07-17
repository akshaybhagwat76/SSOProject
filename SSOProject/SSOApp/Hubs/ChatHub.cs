using App.SQLServer.Data;
using IdentityServer4.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SSOApp.Controllers.UI;
using SSOApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SSOApp.Hubs
{
    public static class UserHandler
    {
        public static HashSet<UserSignalr> ConnectedIds = new HashSet<UserSignalr>();
    }
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChatHub(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private static int _userCount = 0;
        //private static List<UserSignalr> Users = new List<UserSignalr>();
        public async Task SendMessage(string user, string message, string myCaptainId, string myCaptainVal)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, myCaptainId, myCaptainVal);
        }
        public async Task UserOnline(string user, string pass, string tcode)
        {
            LoginInputModel model = new LoginInputModel();
            model.Username = user;
            model.Password = pass;
            model.TenantCode = tcode;
            model.RememberLogin = false;
            using (var client = new HttpClient())
            {
                //Check tenant code from api.
                client.BaseAddress = new Uri("https://localhost:44391/AppAccount/gettenant?teancode=" + tcode);
                var data = new
                {
                    teancode = tcode
                };
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //HTTP POST                  

                var postTask = await client.GetAsync(client.BaseAddress);
                string apiResponse = await postTask.Content.ReadAsStringAsync();
                //postTask.Wait();
                var getsiguser = await _userManager.FindByNameAsync(user);
                var resultjson = JsonConvert.DeserializeObject<APIReturnedModel>(apiResponse);
                if (!resultjson.status.Contains("Not"))
                {
                    var result = await _userManager.CheckPasswordAsync(getsiguser, model.Password);
                    if (result)
                    {
                        //UserHandler.ConnectedIds.Add(user);
                        var chk = UserHandler.ConnectedIds.FirstOrDefault(d => d.UserName == model.Username);
                        if (chk != null)
                        {
                            chk.Count = chk.Count + 1;
                        }
                        else
                        {
                            UserHandler.ConnectedIds.Add(new UserSignalr { UserName = user, Count = 1 });
                        }
                        chk = UserHandler.ConnectedIds.FirstOrDefault(d => d.UserName == user);
                        int count = chk.Count;
                        await Clients.All.SendAsync("ReceiveMessage", model.Username, count);
                    }
                }
            }
        }
        public async Task UserOnlineIncrement(string user, string pass, string tcode)
        {
            LoginInputModel model = new LoginInputModel();
            model.Username = user;
            model.Password = pass;
            model.TenantCode = tcode;
            model.RememberLogin = false;

            var getsiguser = await _userManager.FindByNameAsync(user);

            var result = await _userManager.CheckPasswordAsync(getsiguser, model.Password);
            if (result)
            {
                //UserHandler.ConnectedIds.Add(user);
                var chk = UserHandler.ConnectedIds.FirstOrDefault(d => d.UserName == model.Username);
                if (chk != null)
                {
                    chk.Count = chk.Count + 1;
                }
                else
                {
                    UserHandler.ConnectedIds.Add(new UserSignalr { UserName = user, Count = 1 });
                }                            
            }
        }
        public async Task CountUser(string user)
        {
            var ids = UserHandler.ConnectedIds;
            string message = string.Empty;
            var chk = UserHandler.ConnectedIds.FirstOrDefault(d => d.UserName == user);
            //int count = chk.Count;
            //if (chk.Count == 0)
            //{
                //message = "No";
            //}
            await Clients.All.SendAsync("ReceiveMessage", user, 0, message, ids);
        }

        public async Task UserOffline(string user)
        {

            var ids = UserHandler.ConnectedIds;
            string message = string.Empty;
            int count = 0;
            var getuser = await _userManager.FindByNameAsync(user);
            var chk = UserHandler.ConnectedIds.FirstOrDefault(d => d.UserName == user);
            if (chk.Count != 0)
            {
                chk.Count = chk.Count - 1;
                count = chk.Count;
                if (count == 0)
                {
                    message = "No";
                }
            }
            await Clients.All.SendAsync("OfflineMessage", getuser.UserName, message, count, ids);
        }
    }
}
