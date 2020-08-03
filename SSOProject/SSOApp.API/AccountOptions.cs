using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.Controllers.UI
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public static bool IncludeWindowsGroups = false;

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
        public static string InvalidTenantErrorMessage = "Invalid tenant code";
        public static string InvalidCredentialsMaxScreen = "Maximum login screen reached";
        public static string InvalidRoleName = "Please enter a role name";
        public static string RoleNameExist = "Role name already exist";

        public static string API_Response_Saved = "{0} saved successfully";
        public static string API_Response_Deleted = "{0} deleted successfully";
        public static string API_Response_Exist = " {0} already Exist";
        public static string API_Response_Failed = "Error occured: {0}";
        public static string API_Response_Exception = "Exception";

        public static string API_Response_Success = "Success";
        public static string API_Response_Invalid_User = "Invalid User";
    }
}
