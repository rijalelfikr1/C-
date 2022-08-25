using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEMV_MRO_Tracking.Models
{
    public static class TagHelper
    {
        public static string IsActive(this IHtmlHelper helper, string controller, string action)
        {
            ViewContext context = helper.ViewContext;

            RouteValueDictionary values = context.RouteData.Values;

            string _controller = values["controller"].ToString();

            string _action = values["action"].ToString();

            if ((action == _action) && (controller == _controller))
            {
                return "active";
            }
            else
            {
                return "";
            }
        }
    }
}
