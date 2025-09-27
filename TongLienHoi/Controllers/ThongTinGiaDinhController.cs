using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class ThongTinGiaDinhController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        
        //
        // GET: /QuaTrinhThanHocController/Create

        public ActionResult Create(int cleargyid)
        {   
            ViewBag.CleargyId = cleargyid;
            GetDefaultValue();
            return View();
        } 

        //
        // POST: /QuaTrinhThanHocController/Create

        [HttpPost]
        public ActionResult Create(QuanHeVoiGiaoPham edit)
        {   
            GetDefaultValue();
            var clergyId = CommonFunction.ConvertInt(Request["cleargyid"]);
            ViewBag.CleargyId = clergyId;
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateGiaDinhGiaoPham(edit);
                if (erros.Count == 0)
                {
                    var clergyEducation = new QuanHeVoiGiaoPham
                                              {
                                                  HoTen = edit.HoTen,
                                                  NgheNghiep = edit.NgheNghiep,
                                                  ClergyId = clergyId,
                                                  YearOfBirth = edit.YearOfBirth,
                                                  RelationShipId =edit.RelationShipId,
                                                  Description =  edit.Description
                                              };
                    db.QuanHeVoiGiaoPhams.Add(clergyEducation);
                    db.SaveChanges();
                    return Redirect("~/Home/MucsuThongTinGiaDinh/" + clergyId);
                }
                ViewBag.Errors = erros;
            }
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Edit/5
 
        public ActionResult Edit(int id)
        {
            var conduct = db.QuanHeVoiGiaoPhams.Single(c => c.Id == id);
            ViewBag.QHGD = db.QuanHeGiaDinhs.OrderBy(a => a.Name).ToList();
            ViewBag.CleargyId = conduct.ClergyId;
            GetDefaultValue();
            return View(conduct);
        }

        //
        // POST: /QuaTrinhThanHocController/Edit/5

        [HttpPost]
        public ActionResult Edit(QuanHeVoiGiaoPham edit)
        {
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateGiaDinhGiaoPham(edit);
                if (erros.Count == 0)
                {
                    var getedit = db.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.Id == edit.Id);
                    if (getedit != null)
                    {
                        getedit.HoTen = edit.HoTen;
                        getedit.RelationShipId = edit.RelationShipId;
                        getedit.YearOfBirth = edit.YearOfBirth;
                        getedit.NgheNghiep = edit.NgheNghiep;
                        getedit.Description = edit.Description;
                        db.SaveChanges();
                            return Redirect("~/Home/MucsuThongTinGiaDinh/" + edit.ClergyId);
                        //edit = db.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.Id == edit.Id);
                        //
                    }
                }
                ViewBag.Errors = erros;
            }
            ViewBag.QHGD = db.QuanHeGiaDinhs.OrderBy(a => a.Name).ToList();
           
            
            ViewBag.CleargyId = edit.ClergyId;
            GetDefaultValue();
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Delete/5
 
        public ActionResult Delete(int id)
        {
            var conduct = db.QuanHeVoiGiaoPhams.Single(c => c.Id == id);
            ViewBag.ClergyId = conduct.ClergyId;
            GetDefaultValue();
            return View(conduct);
        }

        //
        // POST: /QuaTrinhThanHocController/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            var conduct = db.QuanHeVoiGiaoPhams.Single(c => c.Id == id);
            var clergyId = conduct.ClergyId;
            var deleteRelation = db.BangCapNguoiPhoiNgaus.Where(a => a.QuanHeVoiGiaoPham_Id == id);
            foreach (var bangCapNguoiPhoiNgau in deleteRelation)
            {
                db.BangCapNguoiPhoiNgaus.Remove(bangCapNguoiPhoiNgau);
            }
            db.QuanHeVoiGiaoPhams.Remove(conduct);
            db.SaveChanges();
            return Redirect("~/Home/MucsuThongTinGiaDinh/" + clergyId);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}