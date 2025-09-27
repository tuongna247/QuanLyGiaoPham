using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using System.IO;
using System.Globalization;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class ChuongTrinhThanHocController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        
        //
        // GET: /QuaTrinhThanHocController/Create

        public ActionResult Create(int educationId)
        {
            var edu = db.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.Id == educationId);
            if (edu != null)
            {
                ViewBag.educationId = edu.ClergyId;
            }
            GetDefaultValue();
            return View();
        } 

        //
        // POST: /QuaTrinhThanHocController/Create

        [HttpPost]
        public ActionResult Create(BangCapNguoiPhoiNgau edit)
        {   
            GetDefaultValue();
            var educationId = CommonFunction.ConvertInt(Request["educationId"]);
            ViewBag.educationId = educationId;
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateDataBangCapNguoiPhoiNgau(edit);
                var ngayTotNghiep = Request["NgayTotNghiep"];
                var startdate = Request["StartDate"];
                var endate = Request["EndDate"];
                if (CommonFunction.IsValidateDateString(ngayTotNghiep))
                {
                    erros.Add("NgayTotNghiep", "Ngày cấp chứng chỉ không hợp lệ.");
                }
                if (CommonFunction.IsValidateDateString(startdate))
                {
                    erros.Add("StartDate", "Từ ngày không hợp lệ.");
                }
                if (CommonFunction.IsValidateDateString(endate))
                {
                    erros.Add("EndDate", "Đến không hợp lệ.");
                }
                if (erros.Count == 0)
                {
                    edit.NgayTotNghiep = ngayTotNghiep;
                    var bangCapNguoiPhoiNgau = new BangCapNguoiPhoiNgau
                                              {
                                                  ChuongTrinhThanHoc = edit.ChuongTrinhThanHoc,
                                                  NoiHoc = edit.NoiHoc,
                                                  NgayTotNghiep = edit.NgayTotNghiep, StartDate = startdate,EndDate = endate,   
                                                  QuanHeVoiGiaoPham_Id = educationId,
                                                  OrderId = edit.OrderId
                                              };
                    var fileRequest = string.Format("ChungChi");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_BangCap, educationId.ToString(CultureInfo.InvariantCulture));
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
                        bangCapNguoiPhoiNgau.ChungChi = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        bangCapNguoiPhoiNgau.ChungChi = string.Empty;
                    }
                    db.BangCapNguoiPhoiNgaus.Add(bangCapNguoiPhoiNgau);
                    db.SaveChanges();
                    return Redirect("~/Home/MucsuThongTinGiaDinh/" + bangCapNguoiPhoiNgau.QuanHeVoiGiaoPham.ClergyId);
                }
                ViewBag.Errors = erros;
            }
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Edit/5
 
        public ActionResult Edit(int id)
        {
            var edu = db.BangCapNguoiPhoiNgaus.Single(c => c.Id == id);
            ViewBag.educationId = edu.Id;
            edu.QuanHeVoiGiaoPham_Id = id;
            
            GetDefaultValue();
            return View(edu);
        }

        //
        // POST: /QuaTrinhThanHocController/Edit/5

        [HttpPost]
        public ActionResult Edit(BangCapNguoiPhoiNgau edit)
        {
            var educationId = CommonFunction.ConvertInt(Request["educationId"]);
            ViewBag.educationId = educationId;
            var erros = ValidateFunction.ValidateDataBangCapNguoiPhoiNgau(edit);
            var ngayTotNghiep = Request["NgayTotNghiep"];
            var startdate = Request["StartDate"];
            var endate = Request["EndDate"];
            if (CommonFunction.IsValidateDateString(ngayTotNghiep))
            {
                erros.Add("NgayTotNghiep", "Ngày cấp chứng chỉ không hợp lệ.");
            }
            if (CommonFunction.IsValidateDateString(startdate))
            {
                erros.Add("StartDate", "Từ ngày không hợp lệ.");
            }
            if (CommonFunction.IsValidateDateString(endate))
            {
                erros.Add("EndDate", "Đến không hợp lệ.");
            }
            var updateBangCap = db.BangCapNguoiPhoiNgaus.FirstOrDefault(a => a.Id == edit.Id);
            if (updateBangCap == null)
            {
                erros.Add("ErorsData", "Dữ liệu không hợp lệ.");
            }
            if (erros.Count == 0)
            {
                edit.NgayTotNghiep = ngayTotNghiep;
                
                if (updateBangCap != null)
                {
                    updateBangCap.ChuongTrinhThanHoc = edit.ChuongTrinhThanHoc;
                    updateBangCap.NoiHoc = edit.NoiHoc;
                    updateBangCap.NgayTotNghiep = edit.NgayTotNghiep;
                    updateBangCap.QuanHeVoiGiaoPham_Id = edit.QuanHeVoiGiaoPham_Id;
                    updateBangCap.StartDate = startdate;
                    updateBangCap.OrderId = edit.OrderId;
                    updateBangCap.EndDate = endate;
                    var fileRequest = string.Format("ChungChi");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_BangCap, edit.QuanHeVoiGiaoPham_Id.ToString());
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
                        updateBangCap.ChungChi = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        updateBangCap.ChungChi = string.Empty;
                    }
                    db.SaveChanges();
                    //return Redirect("~/ThongTinGiaDinh/edit/" + edit.QuanHeVoiGiaoPham_Id);
                    return Redirect("~/Home/MucsuThongTinGiaDinh/" + edit.QuanHeVoiGiaoPham.ClergyId);
                }
            }
            ViewBag.Errors = erros;
            GetDefaultValue();
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Delete/5
 
        public ActionResult Delete(int id)
        {
            var conduct = db.BangCapNguoiPhoiNgaus.Single(c => c.Id == id);
            ViewBag.Id = conduct.Id;
            GetDefaultValue();
            return View(conduct);
        }

        //
        // POST: /QuaTrinhThanHocController/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            
            var conduct = db.BangCapNguoiPhoiNgaus.Single(c => c.Id == id);
            var delete = conduct.QuanHeVoiGiaoPham.ClergyId;
            db.BangCapNguoiPhoiNgaus.Remove(conduct);
            db.SaveChanges();
            return Redirect("~/Home/MucsuThongTinGiaDinh/" + delete);
            //Redirect("~/Home/MucsuThongTinGiaDinh/" + edit.QuanHeVoiGiaoPham_Id);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}