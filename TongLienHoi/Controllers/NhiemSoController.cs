using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.Warper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
//using Omu.AwesomeMvc;
using Path = System.IO.Path;
using CrystalDecisions.Shared;
using HTTLVN.QLTLH.Models.DBO;


namespace HTTLVN.QLTLH.Controllers
{
    public class NhiemSoController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        [ValidateInput(false)]
        public ActionResult Index(int? capchihoi)
        {
            GetDefaultValue();
            ViewData["capchihoi"] = capchihoi;
            string name = string.Empty;
            var list = GetOrders(name, capchihoi);
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection frm)
        {
            GetDefaultValue();
            if (!string.IsNullOrWhiteSpace(frm["capchihoi"]))
                ViewData["chucdanh"] = CommonFunction.ConvertInt(frm["capchihoi"]);
            if (frm["btnUpdateTen"] == "UpdateTen")
            {
                var list = db.Churches.ToList();
                foreach (var item in list)
                {
                    item.TenKhongDau = CommonFunction.ConvertToUnsign2(item.ChurchName).Trim();
                    db.SaveChanges();
                }
            }
            string name = string.Empty;
            int id = 0;
            int.TryParse(frm["capchihoi"],out id);
            var result = GetOrders(name, id);
            return View(result);
        }

        [HttpPost]
        public ActionResult GetDanhSachNhiemSo(ObjectViewModel objectModel)
        {
            var result = GetOrders(objectModel.StrParam1, objectModel.Id);
            return PartialView("_DanhSachNhiemSo", result);
        }

        #region Paging Index Page
        public ActionResult _Paging(string name, int id, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders(name, id).ToDataSourceResult(request));
        }

        private IEnumerable<v_church> GetOrders(string name, int? id)
        {
            var list = db.v_church.ToList();
            var result = new List<v_church>();
            if (id.HasValue && id.Value > 0)
                list = db.v_church.Where(a => a.CapChiHoiID == id).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                var list1 = new List<v_church>();
                var list2 = new List<v_church>();
                list1 = list.Where(a => a.TenKhongDau.ToLower().Contains(name.ToLower())).ToList();
                result.AddRange(list1);
                list2 = list.Where(a => a.ChurchName.ToLower().Contains(name.ToLower())).ToList();
                foreach (var vChurch in list2)
                {
                    if (result.FirstOrDefault(a => a.id != vChurch.id) == null)
                    {
                        result.Add(vChurch);
                    }
                }
            }
            else
            {
                result.AddRange(list);
            }
            foreach (var vChurch in result)
            {
                if (vChurch.AddressFull == null)
                {
                    vChurch.AddressFull = string.Empty;
                }
            }
            return result;
        }

        #endregion

        //Xem thông tin chi tiết nhiệm sở, tương tự như trang chi tiết mục sư
        public ActionResult ChiTietNhiemSo(int? id)
        {
            GetDefaultValue();
            if (id.HasValue && id.Value > 0)
            {
                var church = db.Churches.FirstOrDefault(a => a.Id == id);
                return View(church);
            }
            else
            {
                return View(db.Churches.First());
            }
        }

        public ActionResult Search()
        {
            GetDefaultValue();
            return View();
        }
        //Cho phép thêm mới và chỉnh sửa thông tin cơ bản (địa chỉ, di động)
        public ActionResult ChinhSuaNhiemSo(int? id)
        {
            GetDefaultValue();
            var church = id.HasValue ? db.Churches.Include("Church_Assignment_History").FirstOrDefault(a => a.Id == id) : new Church();
            if (church != null)
            {
                
                ViewBag.HavePeritToDelete = church.Clergy_AssignmentHistory.Count == 0;
                return View(church);
            }
            return Redirect("~/NhiemSo/Index");
            
        }

        //chi tiết bản đồ
        public ActionResult ChitietBanDo(int? id)
        {
            GetDefaultValue();
            var church = db.Churches.FirstOrDefault(a => a.Id == id);
            return View(church);
        }

        //Cho phép thêm mới và chỉnh sửa thông tin cơ bản (địa chỉ, di động)
        [HttpPost]
        public ActionResult ChinhSuaNhiemSo(Church edit)
        {
            GetDefaultValue();
            Dictionary<string, string> errorList = new Dictionary<string, string>();
            var church = edit.Id == 0 ? new Church() : db.Churches.FirstOrDefault(a => a.Id == edit.Id);
            if (Request["btnDeleteImage"] == "Xóa hình")
            {
                church.AnhDaiDien = "";
                db.SaveChanges();
                return View(edit);
            }
            if (Request["deleteChiHoi"] == "Xóa Chi Hội")
            {
                if (church!=null &&  church.Church_Assignment_History.Count == 0)
                {
                    var mucsuDangChucVus = db.Clergies.Where(a => a.CurrentChurchId == church.Id).ToList();
                    foreach (var msDangChucVu in mucsuDangChucVus)
                    {
                        msDangChucVu.CurrentChurchId = null;
                    }
                    db.Churches.Remove(church);
                    db.SaveChanges();
                    return Redirect("~/NhiemSo/Index");
                }
                return View(edit);
            }
            if (church != null)
            {
                ViewBag.HavePeritToDelete = church.Clergy_AssignmentHistory.Count == 0;
                church.ChurchName = edit.ChurchName.Trim();
                church.AddressFull = edit.AddressFull;
                church.TenKhongDau = CommonFunction.ConvertToUnsign2(edit.ChurchName.Trim());
                if (edit.CitiId.HasValue && edit.CitiId.Value > 0)
                    church.CitiId = edit.CitiId;
                else
                {
                    errorList.Add("CitiId", "Vui lòng chọn 'Tỉnh Thành'");
                }
                church.Email = edit.Email;
                //if (string.IsNullOrEmpty(church.Email))
                //{
                //    errorList.Add("Email", "Vui lòng nhập email");
                //}
                church.Phone = edit.Phone;
                church.NgayThanhLap = edit.NgayThanhLap;
                church.Longtitude = edit.Longtitude;
                church.Latitude = edit.Latitude;
                church.NgayChungNhan = edit.NgayChungNhan;


                if (edit.CapChiHoiID.HasValue && edit.CapChiHoiID.Value > 0)
                {
                    church.CapChiHoiID = edit.CapChiHoiID;
                }
                else
                {
                    errorList.Add("CapChiHoiID", "Vui lòng chọn 'Loại nhiệm sở'");
                }
                church.GhiChu = HttpUtility.HtmlDecode(edit.GhiChu);
                #region Save File Path
                //save upload image with specific format
                var fileRequest = string.Format("hinhdaidien");
                var file = Request.Files[fileRequest];

                if (file != null && file.FileName.Length > 0)
                {
                    var importPath = Path.Combine(Constant.Document_Church,
                  church.Id.ToString(CultureInfo.InvariantCulture));
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
                    church.AnhDaiDien = systemLocation;
                }
                if (errorList.Count > 0)
                {
                    ViewBag.Errors = errorList;
                }
                else
                {
                    if (church.Id == 0)
                    {
                        db.Churches.Add(church);
                    }
                    db.SaveChanges();

                }

                #endregion
            }
            return Redirect("~/NhiemSo/ChinhSuaNhiemSo/"+church.Id);
        }

        //Thông tin quá trình thành lập, mục sư nào đã hầu việc Chúa, công tác, chức vụ qua các năm, 
        public ActionResult QuaTrinhThanhLap(int? id)
        {
            GetDefaultValue();
            ViewData["id"] = id;
            var church = db.Churches.FirstOrDefault(a => a.Id == id);
            return View(church);
        }
        //Thông tin quá trình thành lập, mục sư nào đã hầu việc Chúa, công tác, chức vụ qua các năm, 
        public ActionResult TaiSanHoiThanh(int? id)
        {
            GetDefaultValue();
            ViewData["id"] = id;
            var church = db.Churches.FirstOrDefault(a => a.Id == id);
            return View(church);
        }

        [HttpPost]
        public ActionResult TaiSanHoiThanh(Church edit)
        {
            GetDefaultValue();
            ViewData["id"] = edit.Id;
            var church = db.Churches.FirstOrDefault(a => a.Id == edit.Id);
            church.DienTichDat = edit.DienTichDat;
            church.QuyMoCoSo = edit.QuyMoCoSo;
            #region Save File Path
            //save upload image with specific format
            var fileRequest = string.Format("taisanhoithanh");
            var file = Request.Files[fileRequest];
            var importPath = Path.Combine(Constant.Document_Church,
                church.Id.ToString(CultureInfo.InvariantCulture));
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
                church.GiayChungNhanQuyenSuDungDat = systemLocation;
            }
            db.SaveChanges();

            #endregion
            return View(church);
        }

    }


}

