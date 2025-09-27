using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;
using EntityState = System.Data.EntityState;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class NhiemSoController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        //
        // GET: /Admin/NhiemSo/

        public ViewResult Index()
        {
            GetDefaultValue();
            var churches = db.Churches.Include("CapChiHoi").Include("City").OrderBy(a=>a.ChurchName);
            return View(churches.ToList());
        }

        //
        // GET: /Admin/NhiemSo/Details/5

        //public ViewResult Details(int id)
        //{
        //    GetDefaultValue();
        //    Church church = db.Churches.Single(c => c.Id == id);
        //    return View(church);
        //}

        //
        // GET: /Admin/NhiemSo/Create

        public ActionResult Create()
        {
            GetDefaultValue();
            ViewBag.CapChiHoiID = new SelectList(db.CapChiHois, "id", "Name");
            ViewBag.CitiId = new SelectList(db.Cities, "id", "Name");
            return View();
        } 

        //
        // POST: /Admin/NhiemSo/Create

        [HttpPost]
        public ActionResult Create(Church church)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Churches.Add(church);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CapChiHoiID = new SelectList(db.CapChiHois, "id", "Name", church.CapChiHoiID);
            ViewBag.CitiId = new SelectList(db.Cities, "id", "Name", church.CitiId);
            return View(church);
        }
        
        //
        // GET: /Admin/NhiemSo/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    GetDefaultValue();
        //    Church church = db.Churches.Single(c => c.Id == id);
        //    ViewBag.CapChiHoiID = new SelectList(db.CapChiHois, "id", "Name", church.CapChiHoiID);
        //    ViewBag.CitiId = new SelectList(db.Cities, "id", "Name", church.CitiId);
        //    return View(church);
        //}

        //
        // POST: /Admin/NhiemSo/Edit/5

        [HttpPost]
        public ActionResult Edit(Church church)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Churches.Attach(church);
                
                db.Entry(church).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CapChiHoiID = new SelectList(db.CapChiHois, "id", "Name", church.CapChiHoiID);
            ViewBag.CitiId = new SelectList(db.Cities, "id", "Name", church.CitiId);
            return View(church);
        }

        //
        // GET: /Admin/NhiemSo/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    GetDefaultValue();
        //    Church church = db.Churches.Single(c => c.Id == id);
        //    return View(church);
        //}

        ////
        //// POST: /Admin/NhiemSo/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    GetDefaultValue();
        //    Church church = db.Churches.Single(c => c.Id == id);
        //    db.Churches.Delete(church);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}