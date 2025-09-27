using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
//using Telerik.Web.Mvc;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        static char[] separateChars = { ',' };
        //
        // GET: /Admin/Users/

        public ViewResult Index()
        {
            GetDefaultValue();
            return View();
        }

        public ActionResult _Paging([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders().ToDataSourceResult(request));
         
        }

        private IEnumerable<GetPermissionList_Result> GetOrders()
        {
            var result = db.GetPermissionList().ToList();
            result = ShowWithRoles(result);
            return result;
        }

        private List<GetPermissionList_Result> ShowWithRoles(List<GetPermissionList_Result> list)
        {
            var result = new List<GetPermissionList_Result>();
            var groups = list.GroupBy(a => a.username).ToList();
            foreach (var group in groups )
            {
                var userRole = new GetPermissionList_Result { username = @group.Key };
                string role = String.Empty;
                var groupNew = @group.ToList();
                for (int i = 0; i < groupNew.Count; i++)
                {
                    role += groupNew[i].Name + ", ";
                }
                userRole.Name = role.Length > 0 ? role.Substring(0, role.Length - 2) : role;
                userRole.id = !@group.Any() ? 0 : group.ToList()[0].id;
                result.Add(userRole);

            }
            return result;
        }

        //
        // GET: /Admin/Users/Create

        public ActionResult Create()
        {
            GetDefaultValue();
            ViewBag.RoleMappings = db.RoleMappings.ToList();
            return View();
        } 

        //
        // POST: /Admin/Users/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            GetDefaultValue();
            ViewBag.RoleMappings = db.RoleMappings.ToList();
            bool isDublicate = db.Users.FirstOrDefault(a => a.username == user.username) != null;
            ViewBag.IsDublicate = isDublicate;
            if (Request["btnSave"] != "Lưu" || string.IsNullOrEmpty(user.username) || isDublicate) return View(user);
            var addUser = new User {username = user.username};
            if (!string.IsNullOrEmpty(user.password))
                    addUser.password = CommonFunction.SetPassword(user.password);
            db.Users.Add(addUser);
                db.SaveChanges();
            if (Request["roleMapping"] != "")
            {
                var role = Request["roleMapping"];
                if (string.IsNullOrEmpty(role)) return RedirectToAction("Index");
                var addRoles = role.Split(separateChars);
                foreach (var detail in addRoles.Select(addRole => new RoleDetail
                {
                    UserId = addUser.id,
                    RoleMapping = CommonFunction.ConvertInt(addRole)
                }))
                {
                    db.RoleDetails.Add(detail);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        //
        // GET: /Admin/Users/Edit/5
 
        public ActionResult Edit(int id)
        {
            GetDefaultValue();
            User user = db.Users.Single(u => u.id == id);
            ViewBag.RoleMappings = db.RoleMappings.ToList();
            return View(user);
        }

        //
        // POST: /Admin/Users/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            GetDefaultValue();
            ViewBag.RoleMappings = db.RoleMappings.ToList();
            if (Request["btnSave"] != "Lưu" || string.IsNullOrEmpty(user.username)) return View(user);
            var updateUser = db.Users.FirstOrDefault(a => a.id == user.id);
            if (updateUser != null)
            {
                updateUser.username = user.username;
                if (!string.IsNullOrEmpty(user.password))
                    updateUser.password =CommonFunction.SetPassword(user.password);
                db.SaveChanges();
            }
            if (Request["roleMapping"] == "") return RedirectToAction("Index");
            var role = Request["roleMapping"];
            var deleteRole = db.RoleDetails.Where(a => a.UserId == user.id);
            foreach (var roleDetail in deleteRole)
            {
                db.RoleDetails.Remove(roleDetail);
            }
            db.SaveChanges();
            if (role != null)
            {
                var addRoles = role.Split(separateChars);
                foreach (var detail in addRoles.Select(addRole => new RoleDetail
                {
                    UserId = user.id,
                    RoleMapping = CommonFunction.ConvertInt(addRole)
                }))
                {
                    db.RoleDetails.Add(detail);
                }
                db.SaveChanges();    
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Users/Delete/5
 
        public ActionResult Delete(int id)
        {
            GetDefaultValue();
            User user = db.Users.Single(u => u.id == id);
            return View(user);
        }

        //
        // POST: /Admin/Users/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            User user = db.Users.Single(u => u.id == id);
            var deletes = db.RoleDetails.Where(a => a.UserId == id);
            foreach (var roleDetail in deletes)
            {
                db.RoleDetails.Remove(roleDetail);
            }
            db.Users.Remove(user);
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