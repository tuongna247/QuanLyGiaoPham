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
using Org.BouncyCastle.Ocsp;
using Path = System.IO.Path;
using CrystalDecisions.Shared;


namespace HTTLVN.QLTLH.Controllers
{
    public class CongNhanChucDanhController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        public ActionResult Create(int? churchId)
        {
            GetDefaultValue();
            ViewBag.churchId = churchId;
            var edit = new Clergy_AssignmentHistory {ChurchId = churchId};
            return View(edit);
        }

        //
        // POST: /Admin/Clergy_AssignmentHistory/Create

        [HttpPost]
        public ActionResult Create(Clergy_AssignmentHistory edit)
        {
            GetDefaultValue();
            var churchId = CommonFunction.ConvertInt(Request["churchid"]);
            if (Request["btnSave"] == "Lưu")
            {
                var fromdate = Request["StartDate"];
                var todate = Request["EndDate"];
                var erros = ValidateFunction.ValidateDataClergyAssign(edit,false);
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("StartDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("EndDate", "Đến năm không hợp lệ");
                if (edit.ClergyId == 0)
                    erros.Add("ClergyId", "Chưa chọn giáo phẩm");
                if (!string.IsNullOrEmpty(fromdate))
                    edit.StartDate = fromdate;
                if (!string.IsNullOrEmpty(todate))
                    edit.EndDate = todate;
                var assign = new Clergy_AssignmentHistory
                {
                    ClergyId = edit.ClergyId,
                    ChurchId = edit.ChurchId,
                    StartDate = edit.StartDate,
                    EndDate = edit.EndDate,
                    Information = edit.Information,
                    TitleId = edit.TitleId,
                    NamChucVu = edit.NamChucVu,
                    Term = edit.Term,
                    TyLeLuu = edit.TyLeLuu,
                    ChucVuId = edit.ChucVuId,
                    PaperNumber = edit.PaperNumber,
                    TermNumber = edit.TermNumber,
                    TermType = edit.TermType,

                };
                if (edit.CapChiHoi > 0)
                    assign.CapChiHoi = edit.CapChiHoi;
                else assign.CapChiHoi = null;
                if (edit.OfficeId > 0)
                    assign.OfficeId = edit.OfficeId;
                DateTime from = CommonFunction.ConvertCustomDateTime(assign.StartDate);
                DateTime to = CommonFunction.ConvertCustomDateTime(assign.EndDate);
                if (DateTime.Now >= from && DateTime.Now <= to)
                {
                    assign.IsCurrent = true;
                }
                if (erros.Count == 0)
                {
                    var importPath = Path.Combine(Constant.Document_import,
                                             assign.ClergyId.ToString(CultureInfo.InvariantCulture));
                    var approvalPaper = Request.Files["ApprovalPaper"];
                    if (approvalPaper != null && approvalPaper.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(approvalPaper.FileName));
                        if (fileName != null)
                        {
                            systemLocation = Path.Combine(importPath, fileName);
                            importPath = Request.MapPath(importPath);
                            var physicalpath = Request.MapPath(systemLocation);
                            if (!Directory.Exists(importPath))
                            {
                                Directory.CreateDirectory(importPath);
                            }
                            approvalPaper.SaveAs(physicalpath);
                        }
                        assign.Paper = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        assign.Paper = string.Empty;
                    }
                    var assignments = db.Clergy_AssignmentHistory.Where(a => a.ChurchId == edit.ChurchId && a.ChucVu != null && (a.ChucVu.Name.Equals("Tạm lo", StringComparison.CurrentCultureIgnoreCase) || a.ChucVu.Name.Equals("Kiêm nhiệm", StringComparison.CurrentCultureIgnoreCase))).ToList();
                    foreach (var assignment in assignments)
                    {
                        if (!string.IsNullOrEmpty(assign.EndDate))
                        {
                            assignment.EndDate = assign.StartDate;
                        }
                        assignment.IsCurrent = false;
                        db.Entry(assignment).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    assign.ClergyId = edit.ClergyId;
                    assign.Role = string.IsNullOrWhiteSpace(edit.Role) ? " " : edit.Role;
                    db.Clergy_AssignmentHistory.Add(assign);
                    db.SaveChanges();
                    if (edit.IsCurrent == true)
                    {
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == edit.ClergyId);
                        if (cleargy != null)
                        {
                            cleargy.CurrentAssignment_Id = assign.id;
                            db.SaveChanges();
                        }
                    }
                    //Response.Redirect("~/NhiemSo/QuaTrinhThanhLap/" + edit.ChurchId);
                    return RedirectToAction("QuaTrinhThanhLap", "NhiemSo", new { id = edit.ChurchId });
                }
                ViewBag.Errors = erros;
            }
            return View(edit);
        }

        //
        // GET: /Admin/Clergy_AssignmentHistory/Edit/5

        public ActionResult Edit(int id)
        {
            GetDefaultValue();
            var editObject = db.Clergy_AssignmentHistory.Single(c => c.id == id);
            var cleargy = db.Clergies.FirstOrDefault(a => a.Id == editObject.ClergyId);
            if ((cleargy != null && cleargy.CurrentAssignment_Id == editObject.id) || (editObject.IsCurrent.HasValue &&  editObject.IsCurrent.Value))
            {
                ViewBag.IsCurrent = true;
            }
            //ViewBag.IsCurrent
            return View(editObject);
        }

        //
        // POST: /Admin/Clergy_AssignmentHistory/Edit/5

        [HttpPost]
        public ActionResult Edit(Clergy_AssignmentHistory edit)
        {
            Clergy_AssignmentHistory assign;
            GetDefaultValue();
            if (Request["btnSave"] == "Lưu")
            {
                assign = db.Clergy_AssignmentHistory.FirstOrDefault(a => a.id == edit.id);
                var cleargy1 = db.Clergies.FirstOrDefault(a => a.Id == edit.ClergyId);
                if (cleargy1 != null && cleargy1.CurrentAssignment_Id == edit.id)
                {
                    ViewBag.IsCurrent = true;
                }
                edit.Clergy = assign.Clergy;
                var fromdate = Request["StartDate"];
                var todate = Request["EndDate"];
                var erros = ValidateFunction.ValidateDataClergyAssign(edit,false);
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("StartDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("EndDate", "Đến năm không hợp lệ");
                if (!string.IsNullOrEmpty(fromdate))
                    edit.StartDate = fromdate;
                if (!string.IsNullOrEmpty(todate))
                    edit.EndDate = todate;
               
                if (assign != null && erros.Count == 0)
                {
                    assign.ChurchId = edit.ChurchId;
                    assign.StartDate = edit.StartDate;
                    assign.EndDate = edit.EndDate;
                    assign.ClergyId = edit.ClergyId;
                    assign.ChurchId = edit.ChurchId;
                    assign.Information = edit.Information;
                    assign.TitleId = edit.TitleId;
                    assign.NamChucVu = edit.NamChucVu;
                    assign.ChucVuId = edit.ChucVuId;
                    assign.PaperNumber = edit.PaperNumber;
                    assign.TermType = edit.TermType;
                    assign.Term = edit.Term;
                    assign.TyLeLuu = edit.TyLeLuu;
                    assign.OfficeId = edit.OfficeId == 0 ? null : edit.OfficeId;
                    DateTime from = CommonFunction.ConvertCustomDateTime(assign.StartDate);
                    DateTime to = CommonFunction.ConvertCustomDateTime(assign.EndDate);
                    assign.IsCurrent = edit.IsCurrent;
                    if (DateTime.Now>=from && DateTime.Now <= to)
                    {
                        assign.IsCurrent = true;
                    }

                    assign.TermNumber = edit.TermNumber;
                    if (edit.CapChiHoi.HasValue && edit.CapChiHoi.Value > 0)
                        assign.CapChiHoi = edit.CapChiHoi;
                    else assign.CapChiHoi = null;
                   
                    if (assign.IsCurrent == true)
                    {
                        var assignments = db.Clergy_AssignmentHistory.Where(a => a.ChurchId == edit.ChurchId && a.ChucVu != null && (a.ChucVu.Name.Equals("Tạm lo", StringComparison.CurrentCultureIgnoreCase) || a.ChucVu.Name.Equals("Kiêm nhiệm", StringComparison.CurrentCultureIgnoreCase))).ToList();
                        foreach (var assignment in assignments)
                        {
                            if (!string.IsNullOrEmpty(assign.EndDate))
                            {
                                assignment.EndDate = assign.StartDate;
                            }
                            assignment.IsCurrent = false;
                            db.Entry(assignment).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == assign.ClergyId);
                        if (cleargy != null)
                        {
                            cleargy.CurrentAssignment_Id = assign.id;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == assign.ClergyId);
                        if (cleargy != null && cleargy.CurrentAssignment_Id == assign.id)
                        {
                            cleargy.CurrentAssignment_Id = null;
                            db.SaveChanges();
                        }
                    }
                    var importPath = Path.Combine(Constant.Document_import,
                        assign.ClergyId.ToString(CultureInfo.InvariantCulture));
                    var approvalPaper = Request.Files["ApprovalPaper"];
                    if (approvalPaper != null && approvalPaper.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var fileName = CommonFunction.GetNewFileName(importPath,
                            Path.GetFileName(approvalPaper.FileName));
                        if (fileName != null)
                        {
                            systemLocation = Path.Combine(importPath, fileName);
                            importPath = Request.MapPath(importPath);
                            var physicalpath = Request.MapPath(systemLocation);
                            if (!Directory.Exists(importPath))
                            {
                                Directory.CreateDirectory(importPath);
                            }
                            approvalPaper.SaveAs(physicalpath);
                        }
                        assign.Paper = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                    {
                        assign.Paper = string.Empty;
                    }
                    db.SaveChanges();
                    return RedirectToAction("QuaTrinhThanhLap", "NhiemSo", new { id = edit.ChurchId });
                }
                ViewBag.Errors = erros;
            }
            return View(edit);
        }

        //
        // GET: /Admin/Clergy_AssignmentHistory/Delete/5

        public ActionResult Delete(int id)
        {
            GetDefaultValue();
            var clergyAssignmenthistory = db.Clergy_AssignmentHistory.Single(c => c.id == id);
            var cleargy = db.Clergies.FirstOrDefault(a => a.Id == clergyAssignmenthistory.ClergyId);
            if (cleargy.CurrentAssignment_Id == clergyAssignmenthistory.id)
            {
                ViewBag.IsCurrent = true;
            }
            return View(clergyAssignmenthistory);
        }

        //
        // POST: /Admin/Clergy_AssignmentHistory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            var clergyTitlehistory = db.Clergy_AssignmentHistory.FirstOrDefault(c => c.id == id);
            if (clergyTitlehistory != null)
            {
                var churchId = clergyTitlehistory.ChurchId;
                db.Clergy_AssignmentHistory.Remove(clergyTitlehistory);
                db.SaveChanges();
                Response.Redirect("~/NhiemSo/QuaTrinhThanhLap/" + churchId);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}