using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//using HTTLVN.QLTLH.Areas.Admin.Controllers;
using HTTLVN.QLTLH.Code.ChiHoi;

namespace HTTLVN.QLTLH
{
    /// <summary>
    /// Summary description for TLHWebServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TLHWebServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            TinHuuController.ExportTinHuu();
            return "Hello World";
        }
    }
}
