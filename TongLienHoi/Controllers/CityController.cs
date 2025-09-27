using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using System.IO;
using System.Globalization;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class CityController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        
        //
        // GET: /QuaTrinhThanHocController/Create

        public ActionResult Index()
        {
            var city = db.Cities.ToList();
            GetDefaultValue();
            return View(city);
        } 

        
        //
        // GET: /QuaTrinhThanHocController/Edit/5
 
        public ActionResult Edit(int id)
        {
            var city = db.Cities.Single(c => c.id == id);
            GetDefaultValue();
            return View(city);
        }

        //
        // POST: /QuaTrinhThanHocController/Edit/5

        [HttpPost]
        public ActionResult Edit(City edit)
        {

            var erros = ValidateFunction.CheckValidationCity(edit);
           
            if (erros.Count == 0)
            {
               
                var getedit = db.Cities.FirstOrDefault(a => a.id == edit.id);
                if (getedit != null)
                {
                    getedit.HoiNhanh = edit.HoiNhanh;
                    getedit.HoiNhanhCCT = edit.HoiNhanhCCT;
                    getedit.HoiNhanhCT = edit.HoiNhanhCT;
                    getedit.SoChiHoi = edit.SoChiHoi;
                    getedit.SoChiHoiCCT = edit.SoChiHoiCCT;
                    getedit.SoChiHoiCT = edit.SoChiHoiCT;
                    getedit.DiemNhom = edit.DiemNhom;
                    getedit.DiemNhomCT = edit.DiemNhomCT;
                    getedit.DiemNhomCCT = edit.DiemNhomCCT;
                    getedit.TinHuuDiNhom = edit.TinHuuDiNhom;
                    getedit.TinHuuChuaBapTem = edit.TinHuuChuaBapTem;
                    getedit.TinHuu = edit.TinHuu;
                    db.SaveChanges();
                    return RedirectToAction("Index", "City"); ;
                }
            }
            ViewBag.Errors = erros;
          
            GetDefaultValue();
            return View(edit);
        }

        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}