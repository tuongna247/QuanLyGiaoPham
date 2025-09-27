using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        public void GetDefaultValue()
        {
          
            ViewBag.GroupGiaoPhamByTitles = db.V_GroupGiaoPhamByChucDanh.ToList();
            ViewBag.GroupHoiThanhByCapChiHoi = db.V_GroupHoiThanhByCapChiHoi.ToList();
            ViewBag.GetEndDay = db.V_GetEndDay.ToList();
            //ViewBag.GetAllEndDay = db.V_GetAllEndDay.ToList();
            ViewBag.Cities = db.Cities.ToList();
        }

    }
}
