#pragma checksum "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34d91c78189f206cf8eab7501ac6cdde3c6e8fe4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Generic_Index), @"mvc.1.0.view", @"/Views/Generic/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\_ViewImports.cshtml"
using SSOApp;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"34d91c78189f206cf8eab7501ac6cdde3c6e8fe4", @"/Views/Generic/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e2f8da519da6990308c8d8fd425631ca3abf1b7", @"/Views/_ViewImports.cshtml")]
    public class Views_Generic_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SSOApp.API.ViewModels.ModuleFieldValueListViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
  
    ViewData["Title"] = Model.ModuleLabel;
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div style=\"float:right;\">\r\n");
#nullable restore
#line 8 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
     foreach (var claim in Model.UserClaim)
    {
        if (claim.ClaimName == "Add")
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <a");
            BeginWriteAttribute("href", " href=\"", 304, "\"", 362, 1);
#nullable restore
#line 12 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
WriteAttributeValue("", 311, Url.Content("~/Generic/Create?moduleId="+Model.ID), 311, 51, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><input type=\"button\" class=\"btn btn-success\" name=\"btnCreate\" value=\"Add\" /></a>\r\n");
#nullable restore
#line 13 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n<table class=\"table\">\r\n    <thead class=\"thead-dark\">\r\n        <tr>\r\n");
#nullable restore
#line 19 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
             foreach (System.Data.DataColumn dc in Model.List.Tables[0].Columns)
            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <th>");
#nullable restore
#line 22 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
               Write(dc);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n");
#nullable restore
#line 23 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            <th>Actions</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 28 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
         foreach (System.Data.DataRow dr in Model.List.Tables[0].Rows)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n");
#nullable restore
#line 31 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                 foreach (System.Data.DataColumn myColumn in Model.List.Tables[0].Columns)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <td>");
#nullable restore
#line 33 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                   Write(dr[myColumn.ColumnName].ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 34 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <td>\r\n");
#nullable restore
#line 36 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                     foreach (var claim in Model.UserClaim)
                    {
                        if (claim.ClaimName == "Edit" || claim.ClaimName == "Update")
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <a");
            BeginWriteAttribute("href", " href=\"", 1304, "\"", 1384, 1);
#nullable restore
#line 40 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
WriteAttributeValue("", 1311, Url.Content("~/Generic/Create?moduleId=" + Model.ID + "&recID=" + dr[0]), 1311, 73, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><input type=\"button\" class=\"btn btn-success\" name=\"btnCreate\" value=\"Edit\" /></a>\r\n");
#nullable restore
#line 41 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                        }
                        if (claim.ClaimName == "Delete")
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <a");
            BeginWriteAttribute("href", " href=\"", 1611, "\"", 1691, 1);
#nullable restore
#line 44 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
WriteAttributeValue("", 1618, Url.Content("~/Generic/Delete?moduleId=" + Model.ID + "&recID=" + dr[0]), 1618, 73, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <input type=\"button\" class=\"btn btn-danger\" name=\"btnDelete\" value=\"Delete\" onclick=\"return confirm(\'Are you sure want to delete this role?\')\" />\r\n                            </a>\r\n");
#nullable restore
#line 47 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"
                        }
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </td>\r\n            </tr>\r\n");
#nullable restore
#line 51 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\Generic\Index.cshtml"

        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SSOApp.API.ViewModels.ModuleFieldValueListViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
