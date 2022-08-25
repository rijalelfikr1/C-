#pragma checksum "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b6545e50a2aaa640c3b41b5722074c4a08e11e41"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin__HistoryPartnumber), @"mvc.1.0.view", @"/Views/Admin/_HistoryPartnumber.cshtml")]
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
#line 1 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\_ViewImports.cshtml"
using SEMV_MRO_Tracking;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\_ViewImports.cshtml"
using SEMV_MRO_Tracking.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b6545e50a2aaa640c3b41b5722074c4a08e11e41", @"/Views/Admin/_HistoryPartnumber.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5230c810e1008409dbdcd1f36ffa59a94702ca73", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin__HistoryPartnumber : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<h4 class=""mt-3"">Packing History</h4>
<table id=""transactionTable"" class=""table table-sm table-bordered table-striped"" style=""font-size: 8pt;width: 100%"">
    <thead>
        <tr class=""bg-success"">
            <th class=""align-middle text-center"">Purchase Number</th>
            <th class=""align-middle text-center"">Part Number</th>
            <th class=""align-middle text-center"">Quantity Req</th>
            <th class=""align-middle text-center"">Request By</th>
            <th class=""align-middle text-center"">Transaction Type</th>
            <th class=""align-middle text-center"">Timestamp</th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 19 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
           if (Model.TransactionDetails != null)
            {
                foreach (var item in Model.TransactionDetails)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 24 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Order_ID));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 25 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Part_Number));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 26 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Quantity_Req));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 27 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Request_By));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 28 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Request_Type));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                        <td class=\"align-middle text-center\">");
#nullable restore
#line 29 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"
                                                        Write(Html.DisplayFor(model => item.Timestamp));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\n                    </tr>\n");
#nullable restore
#line 31 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"

                }
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\n                    <td colspan=\"6\" class=\"align-middle\">No Data to Display</td>\n                </tr>\n");
#nullable restore
#line 39 "D:\SEMV_MRO_Tracking\SEMV_MRO_Tracking\Views\Admin\_HistoryPartnumber.cshtml"

            }
        

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\n</table>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
