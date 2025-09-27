using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Omu.AwesomeMvc;
using Org.BouncyCastle.Ocsp;

namespace HTTLVN.QLTLH.Controllers
{
    public class TinHuuController : BaseTinHuuController
    {
        private TongLienHoiEntities db = new TongLienHoiEntities();
        //Danh sách tín hữu
        public ActionResult Index()
        {
            //GetDefaultValue();
            return View();
        }

        public ActionResult TinHuuDetail(int? id)
        {
            var tinhuu = db.Church_TinHuu.FirstOrDefault(a => a.Id == id);
            return View(tinhuu);
        }


        public ActionResult TinHuuEdit(int? id)
        {
            var listDanToc = db.DanTocs.OrderBy(a => a.Name).ToList();
            var tinhuu = db.Church_TinHuu.FirstOrDefault(a => a.Id == id) ?? new Church_TinHuu();
            ViewBag.DanToc = new SelectList(listDanToc, "Id", "Name", tinhuu.DanToc);
            var listBanNganh = db.Church_BanNganh.OrderBy(a => a.TenBanNganh).ToList();
            ViewBag.BanNganh_Id = new SelectList(listBanNganh, "Id", "TenBanNganh", tinhuu.BanNganh_Id);
            var listBanHatLe = db.Church_BanHatLe.OrderBy(a => a.TenBanHatLe).ToList();
            ViewBag.BanHatLe_Id = new SelectList(listBanHatLe, "Id", "TenBanHatLe", tinhuu.BanHatLe_Id);
            return View(tinhuu);
        }

        [HttpPost]
        public ActionResult TinHuuEdit(Church_TinHuu edit)
        {
            var listDanToc = db.DanTocs.OrderBy(a => a.Name).ToList();
            ViewBag.DanToc = new SelectList(listDanToc, "Id", "Name", edit.DanToc);
            var listBanNganh = db.Church_BanNganh.OrderBy(a => a.TenBanNganh).ToList();
            ViewBag.BanNganh_Id = new SelectList(listBanNganh, "Id", "TenBanNganh", edit.BanNganh_Id);
            var listBanHatLe = db.Church_BanHatLe.OrderBy(a => a.TenBanHatLe).ToList();
            ViewBag.BanHatLe_Id = new SelectList(listBanHatLe, "Id", "TenBanHatLe", edit.BanHatLe_Id);
            var tinhuu = db.Church_TinHuu.FirstOrDefault(a => a.Id == edit.Id) ??
                         new Church_TinHuu(){ CurrentChurch_Id =  GetChurchId()};
            {
                tinhuu.FirstName = edit.FirstName;
                tinhuu.LastName = edit.LastName;
                tinhuu.DateOfBirth = edit.DateOfBirth;
                tinhuu.MiddleName = edit.MiddleName;
                tinhuu.DanToc = edit.DanToc;
                tinhuu.NickName = edit.NickName;
                tinhuu.BaptismYear = edit.BaptismYear;
                tinhuu.BaptismDate = edit.BaptismDate;
                tinhuu.BelieveDate = edit.BelieveDate;
                tinhuu.BelieveYear = edit.BelieveYear;
                tinhuu.QuanHeGiaDinh = edit.QuanHeGiaDinh;
                tinhuu.NgheNghiep = edit.NgheNghiep;
                tinhuu.Phone = edit.Phone;
                tinhuu.Gender = edit.Gender;
                tinhuu.MobilePhone = edit.MobilePhone;
                tinhuu.Email = edit.Email;
                tinhuu.BanHatLe_Id = edit.BanHatLe_Id;
                tinhuu.BanNganh_Id = edit.BanNganh_Id;
                string full = tinhuu.FirstName + " " + tinhuu.MiddleName + " " + tinhuu.LastName;
                string khongdau = CommonFunction.ConvertToUnsign2(full);
                tinhuu.TenDayDuKhongDau = khongdau;

                #region Save File Path

                //save upload image with specific format
                var fileRequest = string.Format("hinhdaidien");
                var file = Request.Files[fileRequest];
                var importPath = Path.Combine(Constant.TinHuu_HinhDaiDien, tinhuu.CurrentChurch_Id.ToString(),
                    tinhuu.Id.ToString());
                if (file != null && file.FileName.Length > 0)
                {
                    var systemLocation = "";
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(file.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        file.SaveAs(physicalpath);
                    }
                    tinhuu.Avatar = systemLocation;
                    edit.Avatar = systemLocation;
                }

                #endregion

                #region Contact address

                if (!tinhuu.ContactAddressId.HasValue || tinhuu.ContactAddressId.Value == 0)
                {
                    var add = new Church_TinHuu_Addresses()
                    {
                        Street = Request["TinHuu_Addresses.Street"],

                    };
                    if (CommonFunction.ConvertInt(Request["TinHuu_Addresses.CityId"]) > 0)
                        add.CityId = CommonFunction.ConvertInt(Request["TinHuu_Addresses.CityId"]);
                    else
                        add.CityId = null;
                    db.Church_TinHuu_Addresses.AddObject(add);
                    db.SaveChanges();
                    tinhuu.ContactAddressId = add.Id;
                }
                else
                {
                    var add = db.Church_TinHuu_Addresses.FirstOrDefault(a => a.Id == tinhuu.ContactAddressId);
                    if (add != null)
                    {
                        add.Street = Request["TinHuu_Addresses.Street"];
                        if (CommonFunction.ConvertInt(Request["TinHuu_Addresses.CityId"]) > 0)
                            add.CityId = CommonFunction.ConvertInt(Request["TinHuu_Addresses.CityId"]);
                        else
                            add.CityId = null;
                        db.SaveChanges();
                        tinhuu.BirthAddressId = add.Id;
                    }
                }

                #endregion

                if (tinhuu.Id == 0)
                    db.Church_TinHuu.AddObject(tinhuu);
                db.SaveChanges();
                return View(tinhuu);
            }
        }


        public ActionResult HoGiaDinhDetail(int? id)
        {
            var hogiadinh = db.Church_HoGiaDinh.FirstOrDefault(a => a.id == id);

            return View(hogiadinh);
        }

        public ActionResult HoGiaDinhEdit(int? id)
        {
            var hogiadinh = db.Church_HoGiaDinh.FirstOrDefault(a => a.id == id) ?? new Church_HoGiaDinh();

            return View(hogiadinh);
        }

        [HttpPost]
        public ActionResult HoGiaDinhEdit(Church_HoGiaDinh edit)
        {
            var hogiadinh = db.Church_HoGiaDinh.FirstOrDefault(a => a.id == edit.id);
            if (hogiadinh != null)
            {
                #region Save File Path

                int tinhuuId = CommonFunction.ConvertInt(Request["TinHuu.Id"]);
                var tinhuu = db.Church_TinHuu.FirstOrDefault(a => a.Id == tinhuuId);
                //save upload image with specific format
                var fileRequest = string.Format("hinhdaidien");
                var file = Request.Files[fileRequest];
                var importPath = Path.Combine(Constant.TinHuu_HinhDaiDien, tinhuu.Id.ToString(),
                    edit.id.ToString());
                if (file != null && file.FileName.Length > 0)
                {
                    var systemLocation = "";
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(file.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        file.SaveAs(physicalpath);
                    }
                    hogiadinh.BigImages = systemLocation;
                    edit.BigImages = hogiadinh.BigImages;
                }

                #endregion

                db.SaveChanges();
            }
            return View(edit);
        }

        public ActionResult Login()
        {
            //GetDefaultValue();
            var myCookie = Request.Cookies["cookiename"];
            if (myCookie != null)
            {
                var urs = myCookie.Values["UserName"];
                var user1 = db.Church_Users.FirstOrDefault(a => a.username.ToLower() == urs.ToLower());
                Session[Constant.SessionLoginTinHuu] = user1;
                var myurl = Request["returnurl"];
                if (!string.IsNullOrEmpty(myurl) && myurl != ",")
                    return Redirect(myurl);
                 return Redirect("~/TinHuu/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            //btnLogin
            if (Request["btnlogin"] == "login")
            {
                var churchId =  CommonFunction.ConvertInt(Request["church_id"]);
                var username = Request["login-email"];
                var password = Request["login-password"];
                var user = db.Church_Users.FirstOrDefault(a => a.username.ToLower() == username.ToLower() && a.Church_ID == churchId);
                var erros = new Dictionary<string, string>();
                if (user == null)
                    erros.Add("UserName", "Tài khoản không tồn tại");
                if (user != null)
                {
                    var passwordSuccess = CommonFunction.CheckPassword(password, user.password);
                    if (!passwordSuccess)
                        erros.Add("PasswordWrong", "User name hoặc password sai");
                }
                if (erros.Count == 0)
                {
                    Session[Constant.SessionLoginTinHuu] = user;
                    SaveCookies(user.username, "");
                    var myurl = frm["returnurl"];
                    if (!string.IsNullOrEmpty(myurl) && myurl.IndexOf(",")==-1)
                        return Redirect(myurl);
                    return Redirect("~/TinHuu/Index");
                }
                ViewBag.Errors = erros;
                return View();
            }
            return View();
        }

        private void SaveCookies(string username, string password)
        {
            var cookie = new HttpCookie("cookiename");
            cookie.Values.Add("UserName", username);
            cookie.Values.Add("Password", password);
            cookie.Expires = DateTime.Now.AddHours(4);
            Response.Cookies.Add(cookie);
        }

        protected new void DeleteCookies()
        {
            var cookie = new HttpCookie("cookiename");
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);
        }

        public ActionResult LogOut()
        {
            ViewBag.Title = "Logout";
            ViewBag.SITE_DOMAIN_NAME = Constant.SITE_DOMAIN_NAME;
            Session[Constant.SessionLogin] = null;
            DeleteCookies();
            ViewBag.IsLoginFail = true;
            Response.Redirect("Login");
            return View();
        }

        //Thêm mới và edit hộ gia đình
        public ActionResult HoGiaDinh(int? id)
        {
            //GetDefaultValue();
            return View();
        }

        //Thêm mới và edit mỗi tín hữu
        public ActionResult TinHuu(int? id)
        {
            //GetDefaultValue();
            return View();
        }

     

        public ActionResult ThemMoiTinHuu(int? id)
        {
            var tinhuu = new Church_HoGiaDinh() { HomeOwner_Id = id};
            var chuho = db.Church_TinHuu.FirstOrDefault(a => a.Id == id);
            ViewBag.ChuHo = chuho;
            var listtinhuu = db.v_TinHuu.OrderBy(a => a.FullName).ToList();
            ViewBag.Church_TinHuu = new SelectList(listtinhuu, "Id", "FirstName", null);
            return View(tinhuu);
        }

        [HttpPost]
        public ActionResult ThemMoiTinHuu(Church_HoGiaDinh edit)
        {
            var listtinhuu = db.v_TinHuu.OrderBy(a => a.FullName).ToList();
            ViewBag.Church_TinHuu = new SelectList(listtinhuu, "Id", "FirstName", null);
            var chuho = db.Church_TinHuu.FirstOrDefault(a => a.Id == edit.HomeOwner_Id);
            ViewBag.ChuHo = chuho;
            var update = new Church_HoGiaDinh_TinHuu() {};
            update.Church_TinHuu = edit.Church_TinHuu;
            update.HoGiaDinh_Id = edit.HomeOwner_Id;
            update.TinHuu_Id = edit.Church_TinHuu.Id;
            db.Church_HoGiaDinh_TinHuu.AddObject(update);
            return Redirect("~/TinHuu/HoGiaDinhThanhVienEdit/"+edit.HomeOwner_Id);
            return View(update);
        }

        public ActionResult HoGiaDinhThanhVienEdit(int? id)
        {
            var hogiadinh = db.Church_HoGiaDinh.FirstOrDefault(a => a.id == id) ?? new Church_HoGiaDinh() { };

            return View(hogiadinh);
        }
        [HttpPost]
        public ActionResult HoGiaDinhThanhVienEdit(Church_HoGiaDinh edit)
        {
            var hogiadinh = db.Church_HoGiaDinh.FirstOrDefault(a => a.id == edit.id) ?? new Church_HoGiaDinh();

            return View(hogiadinh);
        }
        public ActionResult _Paging(string name, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders(name).ToDataSourceResult(request));

        }

        public ActionResult _PagingHoGiaDinh([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetHoGiaDinhs().ToDataSourceResult(request));

        }

        private IEnumerable<v_hogiadinh_tinhuu> GetHoGiaDinhs()
        {
            var result = db.v_hogiadinh_tinhuu.ToList();
            foreach (var item in result)
            {
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.MobilePhone =string.IsNullOrEmpty(item.MobilePhone)?"": CommonFunction.ConvertDouble(item.MobilePhone) + "";
                item.Phone = string.IsNullOrEmpty(item.Phone) ? "" : CommonFunction.ConvertDouble(item.Phone) + "";
                if (!string.IsNullOrWhiteSpace(item.BigImages) &&
                    System.IO.File.Exists(Request.MapPath(Url.Content(item.BigImages))))
                    item.BigImages = Url.Content(item.BigImages);
                else
                    item.BigImages = Url.Content("~/Content/Content/img/placeholders/image_dark_64x64.png");
            }
            return result;
        }

        private IEnumerable<TimTinHuu_Result> GetOrders(string name)
        {
            var result = db.TimTinHuu(name).ToList();
            foreach (var item in result)
            {
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.MobilePhone = string.IsNullOrEmpty(item.MobilePhone) ? "" : CommonFunction.ConvertDouble(item.MobilePhone) + "";
                item.Phone = string.IsNullOrEmpty(item.Phone) ? "" : CommonFunction.ConvertDouble(item.Phone) + "";
                if (!string.IsNullOrWhiteSpace(item.Avatar) &&
                    System.IO.File.Exists(Request.MapPath(Url.Content(item.Avatar))))
                    item.Avatar = Url.Content(item.Avatar);
                else
                    item.Avatar = Url.Content("~/Content/Content/img/placeholders/image_dark_64x64.png");
            }
            return result;
        }

    }
}
