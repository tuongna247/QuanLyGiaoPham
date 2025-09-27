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
    public class TinhThanhController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        //
        // GET: /Admin/TinhThanh/

        public ViewResult Index()
        {
            GetDefaultValue();
            return View(db.Cities.ToList());
        }

        //
        // GET: /Admin/TinhThanh/Details/5

        public ViewResult Details(int id)
        {
            GetDefaultValue();
            City city = db.Cities.Single(c => c.id == id);
            return View(city);
        }

        //
        // GET: /Admin/TinhThanh/Create

        public ActionResult Create()
        {
            GetDefaultValue();
            return View();
        } 

        //
        // POST: /Admin/TinhThanh/Create

        [HttpPost]
        public ActionResult Create(City city)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(city);
        }
        
        //
        // GET: /Admin/TinhThanh/Edit/5
 
        public ActionResult Edit(int id)
        {
            GetDefaultValue();
            City city = db.Cities.Single(c => c.id == id);
            return View(city);
        }

        //
        // POST: /Admin/TinhThanh/Edit/5

        [HttpPost]
        public ActionResult Edit(City city)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Cities.Attach(city);
                db.Entry(city).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        //
        // GET: /Admin/TinhThanh/Delete/5
 
        public ActionResult Delete(int id)
        {
            City city = db.Cities.Single(c => c.id == id);
            return View(city);
        }

        //
        // POST: /Admin/TinhThanh/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            City city = db.Cities.Single(c => c.id == id);
            db.Cities.Remove(city);
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