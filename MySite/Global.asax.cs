using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MySite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
       

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;
                var test = httpException.GetHttpCode();
                switch (test)
                {
                    case 404:
                        // page not found
                        action = "/HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "/HttpError500";
                        break;
                    default:
                        action = "/General";
                        break;
                }

                string returnUrl = Request.FilePath;
                string errorMessage = (exception.Message);
                // clear error on server
                Server.ClearError();

                string redirect = String.Format("~/Error{0}?returnUrl={1}&message={2}", action, returnUrl, errorMessage);
                Response.Redirect(redirect);
            }
        }
    }
}