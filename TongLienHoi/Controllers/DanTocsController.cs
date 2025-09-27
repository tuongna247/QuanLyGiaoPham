using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Areas.Admin.Controllers;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Controllers
{
    public class DanTocsController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        // GET: DanTocs
        public ActionResult Index()
        {
            GetDefaultValue();
            return View(db.DanTocs.ToList());
        }

        
        // GET: DanTocs/Create
        public ActionResult Create()
        {
            GetDefaultValue();
            return View();
        }

        // POST: DanTocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsDeleted,CreatedDate")] DanToc danToc)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.DanTocs.Add(danToc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danToc);
        }

        // GET: DanTocs/Edit/5
        public ActionResult Edit(int? id)
        {
            GetDefaultValue();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanToc danToc = db.DanTocs.Find(id);
            if (danToc == null)
            {
                return HttpNotFound();
            }
            return View(danToc);
        }

        // POST: DanTocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IsDeleted,CreatedDate")] DanToc danToc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danToc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danToc);
        }

        // GET: DanTocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanToc danToc = db.DanTocs.Find(id);
            if (danToc == null)
            {
                return HttpNotFound();
            }
            return View(danToc);
        }

        // POST: DanTocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanToc danToc = db.DanTocs.Find(id);
            db.DanTocs.Remove(danToc);
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
