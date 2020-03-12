using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Utilities
{
    public static class Utilities
    {
        public static string IsBoardActive(this IHtmlHelper htmlHelper, string controller, string action, string board)
        {

            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();
            var routeBoard = routeData.Values["boardSlug"].ToString();

            var returnActive = (controller == routeController && (action == routeAction) && (board == routeBoard));
            System.Diagnostics.Debug.WriteLine(board + " " + returnActive);
            return returnActive ? "active" : "";
        }
    }
}
