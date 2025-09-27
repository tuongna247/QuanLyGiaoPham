using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;
using EntityState = System.Data.EntityState;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class ChucVusController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        // GET: Admin/ChucVus
        public ActionResult Index()
        {
            GetDefaultValue();
            return View(db.ChucVus.ToList());
        }

        // GET: Admin/ChucVus/Details/5
        public ActionResult Details(int? id)
        {
            GetDefaultValue();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // GET: Admin/ChucVus/Create
        public ActionResult Create()
        {
            GetDefaultValue();
            return View();
        }

        // POST: Admin/ChucVus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Name,Description,ShortTitle,Status")] ChucVu chucVu)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.ChucVus.Add(chucVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chucVu);
        }

        // GET: Admin/ChucVus/Edit/5
        public ActionResult Edit(int? id)
        {
            GetDefaultValue();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Description,ShortTitle,Status")] ChucVu chucVu)
        {
            GetDefaultValue();
            if (ModelState.IsValid)
            {
                db.Entry(chucVu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chucVu);
        }

        // GET: Admin/ChucVus/Delete/5
        public ActionResult Delete(int? id)
        {
            GetDefaultValue();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            ChucVu chucVu = db.ChucVus.Find(id);
            db.ChucVus.Remove(chucVu);
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
