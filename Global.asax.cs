using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TimelyDepotMVC.Controllers;

namespace TimelyDepotMVC
{
    using AutoMapper;

    using TimelyDepotMVC.Helpers;
    using TimelyDepotMVC.Models.Admin;
    using TimelyDepotMVC.ModelsView;

    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    //Create the databases
    //Use: C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regsql as DOS command for the membership provider
    // Set the DAL, create the context and initializer classes
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            CreateModelMapping();
        }


        protected void CreateModelMapping()
        {
            Mapper.CreateMap<Invoice, ShipmentRequestView>();
        }
        /////////////////////////////////////////////////////////////////////
        // Name: Application_Error
        // Version: 1.1.0
        // Summary: Display the error in the Views/Shared/Error.cshtml page
        // Date: 4/2/2012
        // Author: Mario G Vernaza
        // Prerequisites: ViosMails.Controllers
        //      Set the error handle method, with the ActionResult Error method in the HomeController
        // Change History:
        // Date of change (dd/mm/yyyy) [MGV] – Description of change
        /////////////////////////////////////////////////////////////////////
        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception exception = Server.GetLastError();

            // Log the exception.
            //ILogger logger = Container.Resolve<ILogger>();
            //logger.Error(exception);

            Response.Clear();


            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Home");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Error");
            }
            else //It's an Http Exception, Let's handle it.
            {

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "Error");
                        break;
                    case 500:
                        // Server error.
                        routeData.Values.Add("action", "Error");
                        break;
                    // Here you can handle Views to other error codes.
                    // I choose a General error template  
                    default:
                        routeData.Values.Add("action", "Error");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            Server.ClearError();

            // Call target Controller and pass the routeData.
            IController errorController = new HomeController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));

        }

    }
}