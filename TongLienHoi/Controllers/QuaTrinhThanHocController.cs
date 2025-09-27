using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using System.IO;
using System.Globalization;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class QuaTrinhThanHocController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        
        //
        // GET: /QuaTrinhThanHocController/Create

        public ActionResult Create(int cleargyid)
        {   
            ViewBag.CleargyId = cleargyid;
            GetDefaultValue();
            return View();
        } 

        //
        // POST: /QuaTrinhThanHocController/Create

        [HttpPost]
        public ActionResult Create(Clergy_Education edit)
        {   
            GetDefaultValue();
            var clergyId = CommonFunction.ConvertInt(Request["cleargyid"]);
            ViewBag.CleargyId = clergyId;
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateDataClergyEducation(edit);
                var fromdate = Request["FromDate"];
                var todate = Request["ToDate"];
                var ngayCapChungChi = Request["NgayCapChungChi"];
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("FromDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("ToDate", "Đến ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(ngayCapChungChi))
                    erros.Add("NgayCapChungChi", "Ngày cấp chứng chỉ không hợp lệ.");
                if (erros.Count == 0)
                {
                    edit.FromDate = fromdate;
                    edit.ToDate = todate;
                    edit.NgayCapChungChi = ngayCapChungChi;
                    
                    var clergyEducation = new Clergy_Education
                                              {
                                                  CollegeOrProgramme = edit.CollegeOrProgramme,
                                                  Degree = edit.Degree,
                                                  FromDate = edit.FromDate,
                                                  ToDate = edit.ToDate,
                                                  ClergyId = clergyId,
                                                  NgayCapChungChi = edit.NgayCapChungChi,
                                                  NoiChungNhan = edit.NoiChungNhan
                                              };
                    
                    var fileRequest = string.Format("chungchi");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_import, clergyId.ToString(CultureInfo.InvariantCulture));
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

                        var fileExtension = Path.GetExtension(systemLocation);
                        var fileType = MimeAssistant.GetMimeType(fileExtension);
                        var fileInfo = new FileInfo(Request.MapPath(systemLocation));
                        clergyEducation.Paper = systemLocation;
                        clergyEducation.filesize = CommonFunction.ConvertInt(fileInfo.Length);
                        clergyEducation.mine_type = fileType;

                    }else if(string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        clergyEducation.Paper = string.Empty;
                    }
                    db.Clergy_Education.Add(clergyEducation);
                    db.SaveChanges();
                    return Redirect("~/Home/MucSuThongTinChucVu/" + clergyId);
                }
                ViewBag.Errors = erros;
            }
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Edit/5
 
        public ActionResult Edit(int id)
        {
            Clergy_Education clergyEducation = db.Clergy_Education.Single(c => c.Id == id);
            ViewBag.CleargyId = clergyEducation.ClergyId;
            GetDefaultValue();
            return View(clergyEducation);
        }

        //
        // POST: /QuaTrinhThanHocController/Edit/5

        [HttpPost]
        public ActionResult Edit(Clergy_Education edit)
        {

            var erros = ValidateFunction.ValidateDataClergyEducation(edit);
            var fromdate = Request["FromDate"];
            var todate = Request["ToDate"];
            var ngayCapChungChi = Request["NgayCapChungChi"];
            if (CommonFunction.IsValidateDateString(fromdate))
                erros.Add("FromDate", "Từ ngày không hợp lệ.");
            if (CommonFunction.IsValidateDateString(todate))
                erros.Add("ToDate", "Đến ngày không hợp lệ.");
            if (CommonFunction.IsValidateDateString(ngayCapChungChi))
                erros.Add("NgayCapChungChi", "Ngày cấp chứng chỉ không hợp lệ.");
            if (erros.Count == 0)
            {
                edit.FromDate = fromdate;
                edit.ToDate = todate;
                edit.NgayCapChungChi = ngayCapChungChi;
                var getedit = db.Clergy_Education.FirstOrDefault(a => a.Id == edit.Id);
                if (getedit != null)
                {
                    getedit.FromDate = edit.FromDate;
                    getedit.ToDate = edit.ToDate;
                    getedit.CollegeOrProgramme = edit.CollegeOrProgramme;
                    getedit.Degree = edit.Degree;
                    getedit.NgayCapChungChi = edit.NgayCapChungChi;
                    getedit.NoiChungNhan = edit.NoiChungNhan;
                    var fileRequest = string.Format("chungchi");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_import, edit.ClergyId.ToString(CultureInfo.InvariantCulture));
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
                            var fileExtension = Path.GetExtension(systemLocation);
                            var fileType = MimeAssistant.GetMimeType(fileExtension);
                            var fileInfo = new FileInfo(Request.MapPath(systemLocation));
                            getedit.Paper = systemLocation;
                            getedit.filesize = CommonFunction.ConvertInt(fileInfo.Length);
                            getedit.mine_type = fileType;
                        }
                    }
                    else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        getedit.Paper = string.Empty;
                    }
                    db.SaveChanges();
                    return Redirect("~/Home/MucSuThongTinChucVu/" + edit.ClergyId);
                }
            }
            ViewBag.Errors = erros;
            ViewBag.CleargyId = edit.ClergyId;
            GetDefaultValue();
            return View(edit);
        }

        //
        // GET: /QuaTrinhThanHocController/Delete/5
 
        public ActionResult Delete(int id)
        {
            Clergy_Education education = db.Clergy_Education.Single(c => c.Id == id);
            ViewBag.ClergyId = education.ClergyId;
            GetDefaultValue();
            return View(education);
        }

        //
        // POST: /QuaTrinhThanHocController/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            Clergy_Education education = db.Clergy_Education.Single(c => c.Id == id);
            db.Clergy_Education.Remove(education);
            db.SaveChanges();
            return Redirect("~/Home/MucSuThongTinChucVu/" + education.ClergyId);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}