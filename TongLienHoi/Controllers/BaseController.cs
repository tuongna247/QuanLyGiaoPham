using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.SPEntity;

namespace HTTLVN.QLTLH
{
    public class BaseController : Controller
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        public BaseController()
        {
            
        }
        public void GetDefaultValue()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");

#if DEBUG
            ViewBag.IsViewGiaoPham = true;
            ViewBag.IsViewChiHoi = true;
            ViewBag.IsViewVanThu = true;
            ViewBag.IsEditGiaoPham = true;
            ViewBag.IsEditChiHoi = true;
            ViewBag.IsEditVanThu = true;
#else
            if (!CheckPermission() && Session[Constant.SessionLogin] == null)
            {
                if (Request.Url != null) Response.Redirect("~/Home/Login?ReturnUrl="+Request.Url.AbsolutePath);
                else if (Request.UrlReferrer != null)
                    Response.Redirect("~/Home/Login?ReturnUrl=" + Request.UrlReferrer.AbsolutePath);

            }
#endif


            //update cái nầy
            var list = db.V_GroupGiaoPhamByChucDanh.ToList();
            var groupgiaopham = db.V_GroupGiaoPhamByChucDanhNotDuongChuc.ToList();
            var quaphu = groupgiaopham.Where(a => a.Title.ToLower().Contains("quả")).Sum(a => a.Expr1);
            var huutri = groupgiaopham.Where(a => a.Title.ToLower().Contains("trí")).Sum(a => a.Expr1);
            var dinhcuNN = db.Clergies.Count(a => a.DinhCuNuocNgoai.HasValue && a.DinhCuNuocNgoai.Value);
            var duhocCnt = db.Clergies.Count(a => a.DuHoc.HasValue && a.DuHoc.Value);
            var kyLuatCnt = db.Clergies.Count(a => a.KyLuat.HasValue && a.KyLuat.Value);
            var nghiChucVuCnt = db.Clergies.Count(a => a.NghiChucVu.HasValue && a.NghiChucVu.Value);
            var vevoiChua = db.Clergies.Count(a => a.DateOfDeath != null && a.DateOfDeath != "" || (a.IsVeVoiChua.HasValue && a.IsVeVoiChua.Value));
            
            //var huuTri = db.Clergies.Count(a => a.HuuTri != null && a.HuuTri.Value);
            ViewBag.DienKhacCount = quaphu + huutri + dinhcuNN+duhocCnt+kyLuatCnt+vevoiChua;
            ViewBag.QuaPhuCount = quaphu;
            ViewBag.HuuTriCount = huutri;
            ViewBag.GroupGiaoPhamByTitles = list;
            ViewBag.DinhCuNuocNgoaiCount = dinhcuNN;
            ViewBag.DuHocCount = duhocCnt;
            ViewBag.NghiChucVuCount = nghiChucVuCnt;
            ViewBag.BiKyLuatCount = kyLuatCnt;
            ViewBag.VeVoiChuaCount = vevoiChua;
            //ViewBag.HuuTriCount = huuTri;
            ViewBag.QuaPhus = groupgiaopham.Where(a => a.Title.ToLower().Contains("quả")).ToList();
            // bViewBag.HuuTris = groupgiaopham.Where(a => a.Title.Contains("trí")).ToList();
            ViewBag.GroupGiaoPhamByChucDanhNotDuongChuc = groupgiaopham.Where(a => a.Status != 1 && a.IsDuongChuc == null).OrderBy(a => a.Title).ToList();
            ViewBag.GroupHoiThanhByCapChiHoi = db.V_GroupHoiThanhByCapChiHoi.OrderBy(a => a.sortId).ToList();
            ViewBag.Cities = db.Cities.ToList();
        }

        private CodeFirstTongLienHoiEntities _db = new CodeFirstTongLienHoiEntities();

        [HttpPost]
        public bool CheckPermission()
        {
            if (Session[Constant.SessionLogin] == null)
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
