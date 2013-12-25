using GameDataLayer;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var db = new TournamentContext())
            {
                var tournament = new Tournament()
                {
                    Name = "Test",
                    Description = "Test Desc",
                    StartDate = DateTime.Now
                };

                db.Tournaments.Add(tournament);
                db.SaveChanges();
            }
        }
    }
}
