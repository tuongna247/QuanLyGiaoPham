using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH
{
    public class BaseTinHuuController : Controller
    {
        private TongLienHoiEntities db = new TongLienHoiEntities();

        public BaseTinHuuController()
        {
            
        }
        public void GetDefaultValue()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");

            if (!CheckPermission() && Session[Constant.SessionLoginTinHuu] == null)
            {
                if (Request.Url != null) Response.Redirect("~/Tinhuu/Login?ReturnUrl="+Request.Url.AbsolutePath);
                else if (Request.UrlReferrer != null)
                    Response.Redirect("~/Tinhuu/Login?ReturnUrl=" + Request.UrlReferrer.AbsolutePath);

            }
        }

        public int GetChurchId()
        {
            if (Session[Constant.SessionLoginTinHuu] != null  && (Church_Users) Session[Constant.SessionLoginChurchId]!=null)
                return ((Church_Users) Session[Constant.SessionLoginChurchId]).Church_ID;
            return 65;

        }
        private TongLienHoiEntities _db = new TongLienHoiEntities();

        [HttpPost]
        public bool CheckPermission()
        {
            if (Session[Constant.SessionLoginTinHuu] == null)
            {
                if (Request.UrlReferrer != null && !Request.UrlReferrer.OriginalString.Contains("Login"))
                    Session["CurrentPage"] = Request.UrlReferrer.OriginalString;
#if DEBUG
                 var myCookie = Request.Cookies["cookiename"];
                if (myCookie!=null)
                {
                    var urs = myCookie.Values["UserName"];
                    var pass = myCookie.Values["Password"];
                    var user = _db.Users.FirstOrDefault(a => a.username.ToLower() == urs.ToLower() && a.password == pass);
                    Session[Constant.SessionLogin] = user;
                    ViewBag.IsLoginFail = true;
                    ViewBag.IfAuthen = true;
                    ViewBag.IsViewGiaoPham = true;
                    ViewBag.IsViewChiHoi = true;
                    ViewBag.IsViewVanThu = true;
                    ViewBag.IsEditGiaoPham = true;
                    ViewBag.IsEditChiHoi = true;
                    ViewBag.IsEditVanThu = true;
                    return false;
                }
#endif
                if (Request["f_op_login"] == "GO")
                {
                    var username = Request["login-email"];
                    var password = Request["login-password"];
#if DEBUG
                    SaveCookies(username, password);
#endif
                    var authens = _db.Users.FirstOrDefault(a => a.username.ToLower() == username.ToLower() );
                    if (authens != null && CommonFunction.CheckPassword(password, authens.password))
                    {
                        Session[Constant.SessionLogin] = authens;
                       
                        return true;
                    }
                    return false;
                }
            }

            if (Session[Constant.SessionLogin] != null)
            {
                var authen = Session[Constant.SessionLogin] as User;
                ViewBag.auth = authen;
                ViewBag.IfAuthen = true;
                ViewBag.IsViewGiaoPham = db.RoleDetails.FirstOrDefault(a => a.UserId == authen.id && a.RoleMapping == 1) != null;
                ViewBag.IsViewChiHoi =  db.RoleDetails.FirstOrDefault(a=>a.UserId==authen.id && a.RoleMapping==3)!=null;
                ViewBag.IsViewVanThu =  db.RoleDetails.FirstOrDefault(a=>a.UserId==authen.id && a.RoleMapping==5)!=null;
                ViewBag.IsEditGiaoPham =  db.RoleDetails.FirstOrDefault(a=>a.UserId==authen.id && a.RoleMapping==2)!=null;
                ViewBag.IsEditChiHoi =  db.RoleDetails.FirstOrDefault(a=>a.UserId==authen.id && a.RoleMapping==4)!=null;
                ViewBag.IsEditVanThu =  db.RoleDetails.FirstOrDefault(a=>a.UserId==authen.id && a.RoleMapping==6)!=null;
                if (authen != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveCookies(string username,string password)
        {
            var cookie = new HttpCookie("cookiename");
            cookie.Values.Add("UserName", username);
            cookie.Values.Add("Password", password);
            cookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(cookie);
        }

        protected void DeleteCookies()
        {
            var cookie = new HttpCookie("cookiename");
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);
        }
    }
}
