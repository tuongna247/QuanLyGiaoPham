using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.SPEntity;
using EntityState = System.Data.EntityState;
namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{ 
    public class GiaoPhamController : Controller
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        //
        // GET: /Admin/GiaoPham/

        public ViewResult Index()
        {
            var clergies = db.Clergies.Include("Address");
            return View(clergies.ToList());
        }

        public ViewResult IndexGrid()
        {
            var clergies = db.Clergies.Include("Address");
            return View(clergies.ToList());
        }

        //
        // GET: /Admin/GiaoPham/Details/5

        public ViewResult Details(int id)
        {
            Clergy clergy = db.Clergies.Single(c => c.Id == id);
            return View(clergy);
        }

        //
        // GET: /Admin/GiaoPham/Create

        public ActionResult Create()
        {
            ViewBag.PermanentAddressId = new SelectList(db.Addresses, "Id", "Street");
            ViewBag.ContactAddressId = new SelectList(db.Addresses, "Id", "Street");
            ViewBag.FatherId = new SelectList(db.Clergies, "Id", "FirstName");
            ViewBag.MotherId = new SelectList(db.Clergies, "Id", "FirstName");
            ViewBag.WifeId = new SelectList(db.Clergies, "Id", "FirstName");
            return View();
        } 

        //
        // POST: /Admin/GiaoPham/Create

        [HttpPost]
        public ActionResult Create(Clergy clergy)
        {
            if (ModelState.IsValid)
            {
                db.Clergies.Add(clergy);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PermanentAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.PermanentAddressId);
            ViewBag.ContactAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.ContactAddressId);
            return View(clergy);
        }
        
        //
        // GET: /Admin/GiaoPham/Edit/5
 
        public ActionResult Edit(int id)
        {
            Clergy clergy = db.Clergies.Single(c => c.Id == id);
            ViewBag.PermanentAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.PermanentAddressId);
            ViewBag.ContactAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.ContactAddressId);
            return View(clergy);
        }

        //
        // POST: /Admin/GiaoPham/Edit/5

        [HttpPost]
        public ActionResult Edit(Clergy clergy)
        {
            if (ModelState.IsValid)
            {
                db.Clergies.Attach(clergy);
                db.Entry(clergy).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PermanentAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.PermanentAddressId);
            ViewBag.ContactAddressId = new SelectList(db.Addresses, "Id", "Street", clergy.ContactAddressId);
            return View(clergy);
        }

        //
        // GET: /Admin/GiaoPham/Delete/5
 
        public ActionResult Delete(int id)
        {
            Clergy clergy = db.Clergies.Single(c => c.Id == id);
            return View(clergy);
        }

        //
        // POST: /Admin/GiaoPham/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Clergy clergy = db.Clergies.Single(c => c.Id == id);
            db.Clergies.Remove(clergy);
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