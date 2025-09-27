using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class ChurchTinHuuController : Controller
    {
        private TongLienHoiEntities db = new TongLienHoiEntities();

        //
        // GET: /ChurchTinHuu/

        public ViewResult Index()
        {
            var church_tinhuu = db.Church_TinHuu.Include("Church");
            return View(church_tinhuu.ToList());
        }

        //
        // GET: /ChurchTinHuu/Details/5

        public ViewResult Details(int id)
        {
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            return View(church_tinhuu);
        }

        //
        // GET: /ChurchTinHuu/Create

        public ActionResult Create()
        {
            ViewBag.Church_Id = new SelectList(db.Churches, "id", "ChurchName");
            return View();
        } 

        //
        // POST: /ChurchTinHuu/Create

        [HttpPost]
        public ActionResult Create(Church_TinHuu church_tinhuu)
        {
            if (ModelState.IsValid)
            {
                db.Church_TinHuu.AddObject(church_tinhuu);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.Church_Id = new SelectList(db.Churches, "id", "ChurchName", church_tinhuu.Id);
            return View(church_tinhuu);
        }
        
        //
        // GET: /ChurchTinHuu/Edit/5
 
        public ActionResult Edit(int id)
        {
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            ViewBag.Church_Id = new SelectList(db.Churches, "id", "ChurchName", church_tinhuu.Id);
            return View(church_tinhuu);
        }

        //
        // POST: /ChurchTinHuu/Edit/5

        [HttpPost]
        public ActionResult Edit(Church_TinHuu church_tinhuu)
        {
            if (ModelState.IsValid)
            {
                db.Church_TinHuu.Attach(church_tinhuu);
                db.ObjectStateManager.ChangeObjectState(church_tinhuu, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Church_Id = new SelectList(db.Churches, "id", "ChurchName", church_tinhuu.Id);
            return View(church_tinhuu);
        }

        //
        // GET: /ChurchTinHuu/Delete/5
 
        public ActionResult Delete(int id)
        {
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            return View(church_tinhuu);
        }

        //
        // POST: /ChurchTinHuu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Church_TinHuu church_tinhuu = db.Church_TinHuu.Single(c => c.Id == id);
            db.Church_TinHuu.DeleteObject(church_tinhuu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}