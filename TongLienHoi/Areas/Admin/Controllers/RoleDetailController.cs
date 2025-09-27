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
    public class RoleDetailController : Controller
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        //
        // GET: /Admin/RoleDetail/

        public ViewResult Index()
        {
            var roledetails = db.RoleDetails.Include("RoleMapping1").Include("User");
            return View(roledetails.ToList());
        }

        //
        // GET: /Admin/RoleDetail/Details/5

        public ViewResult Details(int id)
        {
            RoleDetail roledetail = db.RoleDetails.Single(r => r.Id == id);
            return View(roledetail);
        }

        //
        // GET: /Admin/RoleDetail/Create

        public ActionResult Create()
        {
            ViewBag.RoleMapping = new SelectList(db.RoleMappings, "id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "id", "username");
            return View();
        } 

        //
        // POST: /Admin/RoleDetail/Create

        [HttpPost]
        public ActionResult Create(RoleDetail roledetail)
        {
            if (ModelState.IsValid)
            {
                db.RoleDetails.Add(roledetail);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.RoleMapping = new SelectList(db.RoleMappings, "id", "Name", roledetail.RoleMapping);
            ViewBag.UserId = new SelectList(db.Users, "id", "username", roledetail.UserId);
            return View(roledetail);
        }
        
        //
        // GET: /Admin/RoleDetail/Edit/5
 
        public ActionResult Edit(int id)
        {
            RoleDetail roledetail = db.RoleDetails.Single(r => r.Id == id);
            ViewBag.RoleMapping = new SelectList(db.RoleMappings, "id", "Name", roledetail.RoleMapping);
            ViewBag.UserId = new SelectList(db.Users, "id", "username", roledetail.UserId);
            return View(roledetail);
        }

        //
        // POST: /Admin/RoleDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(RoleDetail roledetail)
        {
            if (ModelState.IsValid)
            {
                db.RoleDetails.Attach(roledetail);
                db.Entry(roledetail).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleMapping = new SelectList(db.RoleMappings, "id", "Name", roledetail.RoleMapping);
            ViewBag.UserId = new SelectList(db.Users, "id", "username", roledetail.UserId);
            return View(roledetail);
        }

        //
        // GET: /Admin/RoleDetail/Delete/5
 
        public ActionResult Delete(int id)
        {
            RoleDetail roledetail = db.RoleDetails.Single(r => r.Id == id);
            return View(roledetail);
        }

        //
        // POST: /Admin/RoleDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RoleDetail roledetail = db.RoleDetails.Single(r => r.Id == id);
            db.RoleDetails.Remove(roledetail);
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