using System.Web.Mvc;

namespace _24DH110457_MyStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }


     

        public override void RegisterArea(AreaRegistrationContext context)
        {
          
            context.MapRoute(
                "Admin_Orders_Route",
                "Admin/Orders/{action}/{id}",
                new { controller = "Order", action = "Index", id = UrlParameter.Optional }
            );

            
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    
    }
    
}
