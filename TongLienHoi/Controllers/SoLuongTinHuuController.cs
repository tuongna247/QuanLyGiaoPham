using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using Org.BouncyCastle.Ocsp;
using EntityState = System.Data.EntityState;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class SoLuongTinHuuController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        #region So Luong Tin Huu
        //
        // GET: /SoLuongTinHuu/Create

        public ActionResult Create(int? churchId)
        {
            GetDefaultValue();
            var item = new Church_TinHuu { Church_Id = churchId };
            return View(item);
        } 

        //
        // POST: /SoLuongTinHuu/Create

        [HttpPost]
        public ActionResult Create(Church_TinHuu church_tinhuu)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                church_tinhuu.Church_Id = CommonFunction.ConvertInt(Request["Church_ID"]);
                db.Church_TinHuu.Add(church_tinhuu);
                db.SaveChanges();
                return RedirectToAction("Edit", "SoLuongTinHuu", new { id = church_tinhuu.Id });
            }

            return View(church_tinhuu);
        }
        
        //
        // GET: /SoLuongTinHuu/Edit/5
 
        public ActionResult Edit(int id)
        {
            GetDefaultValue();
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            return View(church_tinhuu);
        }

        //
        // POST: /SoLuongTinHuu/Edit/5

        [HttpPost]
        public ActionResult Edit(Church_TinHuu church_tinhuu)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Church_TinHuu.Attach(church_tinhuu);
                db.Entry(church_tinhuu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuaTrinhThanhLap", "NhiemSo", new { id = church_tinhuu.Church_Id });
            }
            return View(church_tinhuu);
        }

        //
        // GET: /SoLuongTinHuu/Delete/5
 
        public ActionResult Delete(int id)
        {
            GetDefaultValue();
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            return View(church_tinhuu);
        }

        //
        // POST: /SoLuongTinHuu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            
            var church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            var churchid = church_tinhuu.Church_Id;
            db.Church_TinHuu.Remove(church_tinhuu);
            db.SaveChanges();
            return RedirectToAction("QuaTrinhThanhLap", "NhiemSo", new { id = churchid });
        }

        #endregion

        #region So Luong Dan Toc
        //
        // GET: /SoLuongTinHuu/Create

        public ActionResult CreateDanToc(int? soluong_id)
        {
            GetDefaultValue();
            var church = db.Church_TinHuu.FirstOrDefault(a => a.Id == soluong_id);
            var item = new Church_TinHuu_DanToc { Church_TinHuu_Id = church.Id };
            item.Church_TinHuu_Id = soluong_id;
            return View(item);
        }

        //
        // POST: /SoLuongTinHuu/Create

        [HttpPost]
        public ActionResult CreateDanToc(Church_TinHuu_DanToc church_tinhuu)
        {
            GetDefaultValue();
            var erros = new Dictionary<string, string>();
            church_tinhuu.Church_TinHuu_Id = CommonFunction.ConvertInt(Request["soluong_id"]);
            church_tinhuu.DanToc_Id = CommonFunction.ConvertInt(Request["DanToc_Id"]);
            if (church_tinhuu.DanToc_Id == 0 || church_tinhuu.DanToc_Id == null)
            {
                erros.Add("Church_TinHuu_Id", "Chọn loại dân tộc");
            }
            if(erros.Count==0)
            {
                
                church_tinhuu.SoLuong = CommonFunction.ConvertInt(Request["SoLuong"]);
                db.Church_TinHuu_DanToc.Add(church_tinhuu);
                db.SaveChanges();
                return RedirectToAction("Edit", "SoLuongTinHuu", new { id = church_tinhuu.Church_TinHuu_Id });
            }
            ViewBag.Errors = erros;
            return View(church_tinhuu);
        }

        //
        // GET: /SoLuongTinHuu/Edit/5

        public ActionResult EditDanToc(int id)
        {
            GetDefaultValue();
            var church_tinhuu = db.Church_TinHuu_DanToc.Single(c => c.Id == id);
            return View(church_tinhuu);
        }

        //
        // POST: /SoLuongTinHuu/Edit/5

        [HttpPost]
        public ActionResult EditDanToc(Church_TinHuu_DanToc church_tinhuu)
        {
            GetDefaultValue();
           var erros = new Dictionary<string, string>();
            church_tinhuu.DanToc_Id = CommonFunction.ConvertInt(Request["DanToc_Id"]);
            if (church_tinhuu.DanToc_Id == 0 || church_tinhuu.DanToc_Id == null)
            {
                erros.Add("Church_TinHuu_Id", "Chọn loại dân tộc");
            }
            if(erros.Count==0)
            {
                db.Church_TinHuu_DanToc.Attach(church_tinhuu);
                db.Entry(church_tinhuu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "SoLuongTinHuu", new { id = church_tinhuu.Church_TinHuu_Id });
            }
            ViewBag.Errors = erros;
            return View(church_tinhuu);
        }

        //
        // GET: /SoLuongTinHuu/Delete/5

        public ActionResult DeleteDanToc(int id)
        {
            GetDefaultValue();
            var delete = db.Church_TinHuu_DanToc.Single(c => c.Id == id);
            return View(delete);
        }

        //
        // POST: /SoLuongTinHuu/Delete/5

        [HttpPost, ActionName("DeleteDanToc")]
        public ActionResult DeleteDanTocConfirmed(int id)
        {
            GetDefaultValue();
            var church_tinhuu = db.Church_TinHuu_DanToc.Single(c => c.Id == id);
            var churchid = church_tinhuu.Church_TinHuu_Id;
            db.Church_TinHuu_DanToc.Remove(church_tinhuu);
            db.SaveChanges();
            return RedirectToAction("Edit", "SoLuongTinHuu", new { id = churchid });
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}