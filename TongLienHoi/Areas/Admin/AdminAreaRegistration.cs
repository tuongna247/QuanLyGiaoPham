using System.Web.Mvc;

namespace HTTLVN.QLTLH.Areas.Admin
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
                "Admin_default1",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "mucsu", id = UrlParameter.Optional }, new string[] { "HTTLVN.QLTLH.Areas.Admin.Controllers" });
        }
    }
}
