using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using System.IO;
using System.Globalization;

namespace HTTLVN.QLTLH.Controllers
{ 
    public class GianDoanChucVuController : BaseController
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
        public ActionResult Create(Clergy_Conduct edit)
        {   
            GetDefaultValue();
            var clergyId = CommonFunction.ConvertInt(Request["cleargyid"]);
            ViewBag.CleargyId = clergyId;
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateDataClergyConDuct(edit);
                var fromdate = Request["Fromdate"];
                var todate = Request["ToDate"];
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("FromDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("ToDate", "Đến ngày không hợp lệ.");
                if (erros.Count == 0)
                {
                    edit.Fromdate = fromdate;
                    edit.ToDate = todate;
                    var clergyEducation = new Clergy_Conduct
                                              {
                                                  Fromdate = edit.Fromdate,
                                                  ToDate = edit.ToDate,
                                                  ClergyId = clergyId,
                                                  Paper = edit.Paper
                                              };
                    clergyEducation.SoVanBan = edit.SoVanBan;
                    var fileRequest = string.Format("VanBan");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_GiaoVuLenh, edit.ClergyId.ToString(CultureInfo.InvariantCulture));
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
                        clergyEducation.VanBan = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtQuyetDinh"]))
                    {
                        clergyEducation.VanBan = string.Empty;
                    }
                    db.Clergy_Conduct.Add(clergyEducation);
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
            var conduct = db.Clergy_Conduct.Single(c => c.id == id);
            ViewBag.CleargyId = conduct.ClergyId;
            GetDefaultValue();
            return View(conduct);
        }

        //
        // POST: /QuaTrinhThanHocController/Edit/5

        [HttpPost]
        public ActionResult Edit(Clergy_Conduct edit)
        {

            var erros = ValidateFunction.ValidateDataClergyConDuct(edit);
            var fromdate = Request["Fromdate"];
            var todate = Request["ToDate"];
            if (CommonFunction.IsValidateDateString(fromdate))
                erros.Add("FromDate", "Từ ngày không hợp lệ.");
            if (CommonFunction.IsValidateDateString(todate))
                erros.Add("ToDate", "Đến ngày không hợp lệ.");
            if (erros.Count == 0)
            {
                edit.Fromdate = fromdate;
                edit.ToDate =todate;
                var getedit = db.Clergy_Conduct.FirstOrDefault(a => a.id == edit.id);
                if (getedit != null)
                {
                    getedit.Fromdate = edit.Fromdate;
                    getedit.ToDate = edit.ToDate;
                    getedit.Paper = edit.Paper;
                    getedit.SoVanBan = edit.SoVanBan;
                    var fileRequest = string.Format("VanBan");
                    var file = Request.Files[fileRequest];
                    if (file != null && file.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var importPath = Path.Combine(Constant.Document_GiaoVuLenh, edit.ClergyId.ToString(CultureInfo.InvariantCulture));
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
                        getedit.VanBan = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtQuyetDinh"]))
                    {
                        getedit.VanBan = string.Empty;
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
            var conduct = db.Clergy_Conduct.Single(c => c.id == id);
            ViewBag.ClergyId = conduct.ClergyId;
            GetDefaultValue();
            return View(conduct);
        }

        //
        // POST: /QuaTrinhThanHocController/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            var conduct = db.Clergy_Conduct.Single(c => c.id == id);
            db.Clergy_Conduct.Remove(conduct);
            db.SaveChanges();
            return Redirect("~/Home/MucSuThongTinChucVu/" + conduct.ClergyId);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}