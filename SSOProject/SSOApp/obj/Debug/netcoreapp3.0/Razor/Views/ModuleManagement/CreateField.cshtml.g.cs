#pragma checksum "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fb459147091ec2db03b5c2fca84b855ef935b819"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ModuleManagement_CreateField), @"mvc.1.0.view", @"/Views/ModuleManagement/CreateField.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fb459147091ec2db03b5c2fca84b855ef935b819", @"/Views/ModuleManagement/CreateField.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e2f8da519da6990308c8d8fd425631ca3abf1b7", @"/Views/_ViewImports.cshtml")]
    public class Views_ModuleManagement_CreateField : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SSOApp.API.ViewModels.FeildModelView>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("DbFieldName"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("FieldName"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("drpFieldType"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmField"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "CreateField", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:50%"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
  
    ViewData["Title"] = "CreateField";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>New Field</h1>

<style>
    .error {
        color: red;
        visibility: hidden;
    }
</style>



<div id=""dvstatus"" style=""display:none"">
    <label id=""lblstatus""></label>

</div>




<div class=""form-group"">
    <div class=""col-md-"">
        Tenant:");
#nullable restore
#line 28 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
          Write(ViewBag.TenantName);

#line default
#line hidden
#nullable disable
            WriteLiteral(".(");
#nullable restore
#line 28 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
                               Write(ViewBag.TenantCode);

#line default
#line hidden
#nullable disable
            WriteLiteral(")\r\n    </div>\r\n\r\n\r\n</div>\r\n\r\n\r\n\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b8198027", async() => {
                WriteLiteral("\r\n\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b8198293", async() => {
                    WriteLiteral("<span>Please correct the following errors</span>");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 39 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary = global::Microsoft.AspNetCore.Mvc.Rendering.ValidationSummary.ModelOnly;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-summary", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
#nullable restore
#line 40 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
Write(Html.HiddenFor(Model => Model.ModuleFieldDetails.ModuleDetailsID));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    ");
#nullable restore
#line 41 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
Write(Html.HiddenFor(Model => Model.ModuleFieldDetails.ID, new { id = "ModuleFieldId" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n    <div class=\"form-group\">\r\n        <label for=\"exampleInputEmail1\">DB Field Name:</label>\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "fb459147091ec2db03b5c2fca84b855ef935b81910718", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
#nullable restore
#line 45 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.DBFieldName);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b81912431", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
#nullable restore
#line 46 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.DBFieldName);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n    </div>\r\n\r\n    <div class=\"form-group\">\r\n        <label for=\"exampleInputEmail1\">Field Label:</label>\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "fb459147091ec2db03b5c2fca84b855ef935b81914255", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 52 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.FieldLabel);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b81915967", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
#nullable restore
#line 53 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.FieldLabel);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    </div>\r\n\r\n\r\n    <div class=\"form-group\">\r\n        <label for=\"exampleInputEmail1\">Field Type:</label>\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b81917789", async() => {
                    WriteLiteral("\r\n            ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b81918070", async() => {
                        WriteLiteral("Select");
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __tagHelperExecutionContext.AddHtmlAttribute("disabled", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                    BeginWriteTagHelperAttribute();
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_4.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
#nullable restore
#line 59 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.FieldType);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
#nullable restore
#line 59 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = ViewBag.FeildTypeList;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fb459147091ec2db03b5c2fca84b855ef935b81922033", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
#nullable restore
#line 63 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ModuleFieldDetails.FieldType);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    </div>\r\n\r\n\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<br />


<div class=""form-group"">
    <table id=""tbloption"" width=""50px"" style=""table-layout:fixed"" class=""table table-hover"">
        <thead>
            <tr>
                <td>
                    Order
                </td>
                <td>
                    Value
                </td>
                <td>
                    Action
                </td>
                <td>
                    <button type=""button"" class=""btn btn-primary"" id=""btnOption"" data-toggle=""modal"" data-target=""#modal"">
                        Add option
                    </button>
                </td>
            </tr>
        </thead>

    </table>

</div>





<!-- Modal -->
<div class=""modal fade"" id=""modal"" tabindex=""-1"" role=""dialog"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
    ");
#nullable restore
#line 102 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
Write(Html.Hidden("OptionId"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h5 class=""modal-title"" id=""exampleModalLabel"">Modal title</h5>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                <div class=""form-group"">
                    Option Value
                    <input class=""form-control"" id=""txtOption"" />
                    <span class=""error"">Please Enter options</span>
                </div>
            </div>
            <div class=""modal-footer"">
                <button type=""button"" class=""btn btn-secondary"" id=""btnClose"" data-dismiss=""modal"">Close</button>
                <button type=""button"" id=""btnSaveOption"" class=""btn btn-primary"">Save</button>
            </div>
        </div>
    </div>
</div>

<div style=");
            WriteLiteral("\"display:inline-block\">\r\n    <input class=\"btn btn-primary\" value=\"Save\" id=\"btnSave\" type=\"button\" />\r\n    <a");
            BeginWriteAttribute("href", " href=\"", 3841, "\"", 3887, 1);
#nullable restore
#line 128 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
WriteAttributeValue("", 3848, Url.Action("Index","ModuleManagement"), 3848, 39, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">Back To Module Feilds</a>
</div>

<script>
    //function setGuid() {
    //    function S4() {
    //        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    //    }
    //    return (S4() + S4() + ""-"" + S4() + ""-4"" + S4().substr(0, 3) + ""-"" + S4() + ""-"" + S4() + S4() + S4()).toLowerCase();
    //}
    var OptionsArray = [];
    o = 1;
    $('#myModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus')
    })

    function EditOption(currow)
    {
        //currow = $(currow).closest('tr');
        Id = $(currow).closest('tr').find('td:eq(0)').text();
        options = $(currow).closest('tr').find('td:eq(1)').text();
        $('#txtOption').val(options);
        $('#OptionId').val(options);
        $('#modal').modal('show')
        $('#btnSaveOption').val('Update');

    }


    function Delete(currow) {
        //var options = $(currow).closest('tr').find('td:eq(1)').text();
        //var Item = OptionsArray.filter(x => x.opt");
            WriteLiteral(@"ions == options)
        //const index = OptionsArray.indexOf(Item);
        OptionsArray.splice(currow, 1)

        var selectedTr = $(currow).closest('tr').remove();
        o--;
        BindTable(OptionsArray);
    }

    function BindTable(OptionsArray) {
        var tbody = $('<tbody/>')
        $.each(OptionsArray, function (i, val) {
            var row = $('<tr/>')
            row.append($('<td/>').html(i+1))
            row.append($('<td/>').html(val.options))
            row.append($('<td/>').html('<a href=""#"" onclick=EditOption(this)>Edit</a>|<a href=""#"" onclick=Delete('+i+')>Delete</a>'))
            tbody.append(row);
        })
        $('#tbloption tbody').remove();
        $('#tbloption').append(tbody);
               // Clear();
    }
    function Clear()
    {
        $('#txtOption').siblings('span.error').css('visibility', 'hidden')
        $('#btnSaveOption').val('')
        $('#OptionId').val('')
        $('#txtOption').val('');
        $('#modal').modal('hide'");
            WriteLiteral(");\r\n    }\r\n    $(document).ready(function () {\r\n        debugger;\r\n\r\n        var uploadedList = \'");
#nullable restore
#line 192 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
                       Write(Html.Raw(Json.Serialize(Model.ModuleFieldOption)));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"'
        var result = $.parseJSON(uploadedList);

        $.each(result, function (i, val) {

            OptionsArray.push({
                PsId: o++,
                options: val.options
            })

        })

        if(OptionsArray.length) {
            BindTable(OptionsArray)
        }




        $('#btnSave').click(function (event) {
            if (!$('#frmField').valid()) {
                $('#frmField').validate();
            }
            else {

                 var PsModuleFieldDetails = {
                DBFieldName: $('#DbFieldName').val(),
                FieldLabel: $('#FieldName').val(),
                     FieldType: $('#drpFieldType option:selected').text(),
                     ID: $('#ModuleFieldId').val(),
                     ModuleDetailsID: $('#ModuleFieldDetails_ModuleDetailsID').val()

            }
            var FeildModelView = {
                ModuleId:$('#ModuleId').val(),
                ModuleFieldDetails: PsModuleFieldDetails,
");
            WriteLiteral("                ModuleFieldOption: OptionsArray\r\n            }\r\n\r\n            $.ajax({\r\n                url:\'");
#nullable restore
#line 232 "C:\My Files\Freelancer Projects\SSOProject\SSOProject\SSOApp\Views\ModuleManagement\CreateField.cshtml"
                Write(Url.Action("CreateField", "ModuleManagement"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
                type: 'Post',
                data: { FeildModelView: FeildModelView },
                success: function(data)
                {
                    $('#dvstatus').css('display', 'block');
                    if (data == 'Saved') {
                        debugger;
                        window.location = ""/ModuleManagement/FieldsList"";

                    }
                    else {
                        $('#lblstatus').addClass('bg-danger').text(data)

                    }
                },

            })

            }
        })

        $('#btnClose').click(function () {
            Clear();
        })

        $('#btnSaveOption').click(function () {


            var Validation = true;
            if ($('#txtOption').val() == '') {
                $('#txtOption').siblings('span.error').css('visibility', 'visible')
                Validation = false;
            }
            else {
                $('#txtOption').siblings('span.error').css('");
            WriteLiteral(@"visibility', 'hidden')
            }

            if (Validation) {

                if ($('#btnSaveOption').val() == ""Update"") {
                    psOptionsId = $('#OptionId').val()
                    let updateOption = OptionsArray.filter(x => x.options == psOptionsId)
                    updateOption[0].options = $('#txtOption').val();

                }
                else {

                    let option = OptionsArray.filter(x => x.options == $('#txtOption').val())
                    if (option.length > 0) {
                        $('#txtOption').siblings('span.error').css('visibility', 'visible').text('Option already exists!!');
                        return;
                    }
                    OptionsArray.push({
                        PsId: o++,
                        options: $('#txtOption').val()
                    })
                }
                BindTable(OptionsArray);
                Clear();
            }
            else {
                return");
            WriteLiteral(";\r\n            }\r\n\r\n        })\r\n    })\r\n\r\n\r\n\r\n\r\n\r\n</script>");
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
