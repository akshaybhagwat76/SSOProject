#pragma checksum "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d3ce076df1ea8acb034c6814db8046d95167fd06"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ModuleManagement_FieldsList), @"mvc.1.0.view", @"/Views/ModuleManagement/FieldsList.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d3ce076df1ea8acb034c6814db8046d95167fd06", @"/Views/ModuleManagement/FieldsList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e2f8da519da6990308c8d8fd425631ca3abf1b7", @"/Views/_ViewImports.cshtml")]
    public class Views_ModuleManagement_FieldsList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SSOApp.API.ViewModels.FeildModelView>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "CreateField", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditField", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
  
    ViewData["Title"] = "Roles";


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<h1>Module Feilds</h1>\r\n<p>\r\n");
#nullable restore
#line 10 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
     if (TempData["Success"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <label class=\"bg-success\">");
#nullable restore
#line 12 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
                             Write(TempData["Success"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n");
#nullable restore
#line 13 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
        TempData["Success"] = null;
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
     if (TempData["Failed"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <label class=\"bg-danger\">");
#nullable restore
#line 17 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
                            Write(TempData["Failed"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n");
#nullable restore
#line 18 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
        TempData["Failed"] = null;
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n\r\n<div style=\"float:right;\">\r\n   ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d3ce076df1ea8acb034c6814db8046d95167fd065559", async() => {
                WriteLiteral("<input type=\"button\"  name=\"btnCreate\" value=\"Add Feild\" />");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-moduleid", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 24 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
                                       WriteLiteral(Model.ModuleId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["moduleid"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-moduleid", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["moduleid"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
</div>


<div>
    <table width=""80px"" class=""table table-hover"">
        <thead>
            <tr>
                <td>
                    Feilds
                </td>
                <td>
                    Feild Type
                </td>
                <td>
                    Visible
                </td>
                <td>
                    Actions
                </td>
            </tr>
        </thead>
        
");
#nullable restore
#line 47 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
             foreach(var items in Model.ModuleFieldDetailsList)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 51 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
               Write(Html.DisplayFor(m => items.FieldLabel));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 54 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
               Write(Html.DisplayFor(m => items.FieldType));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 57 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
               Write(Html.EditorFor(m => items.visible));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d3ce076df1ea8acb034c6814db8046d95167fd069677", async() => {
                WriteLiteral("Edit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-Fieldid", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 60 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
                                                      WriteLiteral(items.ID);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["Fieldid"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-Fieldid", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["Fieldid"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </td>\r\n\r\n\r\n            </tr>\r\n");
#nullable restore
#line 65 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\FieldsList.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        \r\n\r\n    </table>\r\n</div>\r\n<div class=\"panel-body\" id=\"gridBodyRoles\">\r\n\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SSOApp.API.ViewModels.FeildModelView> Html { get; private set; }
    }
}
#pragma warning restore 1591
