using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using STA.DOMAIN.Util;
using System.Web.Security;

namespace STA.UI.WEB.Util
{
    public class Autenticacao : AuthorizeAttribute
    {
        // Custom property
        //public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Usuario",
                                action = "Autenticar"
                            })
                        );
        }
    }
}