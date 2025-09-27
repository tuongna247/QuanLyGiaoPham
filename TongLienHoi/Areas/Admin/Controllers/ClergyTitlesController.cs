using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class ClergyTitlesController : Controller
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        // GET: Admin/ClergyTitles
        public ActionResult Index()
        {
            return View(db.ClergyTitles.ToList());
        }

        // GET: Admin/ClergyTitles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClergyTitle clergyTitle = db.ClergyTitles.Find(id);
            if (clergyTitle == null)
            {
                return HttpNotFound();
            }
            return View(clergyTitle);
        }

        // GET: Admin/ClergyTitles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ClergyTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Short,IsDuongChuc,Status")] ClergyTitle clergyTitle)
        {
            if (ModelState.IsValid)
            {
                db.ClergyTitles.Add(clergyTitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clergyTitle);
        }

        // GET: Admin/ClergyTitles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClergyTitle clergyTitle = db.ClergyTitles.Find(id);
            if (clergyTitle == null)
            {
                return HttpNotFound();
            }
            return View(clergyTitle);
        }

        // POST: Admin/ClergyTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Short,IsDuongChuc,Status")] ClergyTitle clergyTitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clergyTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clergyTitle);
        }

        // GET: Admin/ClergyTitles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClergyTitle clergyTitle = db.ClergyTitles.Find(id);
            if (clergyTitle == null)
            {
                return HttpNotFound();
            }
            return View(clergyTitle);
        }

        // POST: Admin/ClergyTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClergyTitle clergyTitle = db.ClergyTitles.Find(id);
            db.ClergyTitles.Remove(clergyTitle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
