using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Code;
using System.Data.Entity.Validation;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{ 
    public class Clergy_AssignmentHistoryController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        public ActionResult Create(int? cleargyid)
        {
            GetDefaultValue();
            ViewBag.ClergyId = cleargyid;
            Clergy_AssignmentHistory assignment = new Clergy_AssignmentHistory();
            return View(assignment);
        } 

        //
        // POST: /Admin/Clergy_AssignmentHistory/Create

        [HttpPost]
        public ActionResult Create(Clergy_AssignmentHistory edit)
        {
            GetDefaultValue();
            var clergy = CommonFunction.ConvertInt(Request["cleargyid"]);
            ViewBag.ClergyId = clergy;
            if(Request["btnSave"]=="Lưu")
            {
                var fromdate = Request["StartDate"];
                var todate = Request["EndDate"];
                var skipChucVu = false;
                var currentChucVu = db.ChucVus.FirstOrDefault(a => a.id == edit.ChucVuId);
                if (currentChucVu != null && (currentChucVu.Name.Contains("Chờ Nhiệm sở")|| currentChucVu.Name.Contains("Du học")))
                {
                    skipChucVu = true;
                }
                var erros = ValidateFunction.ValidateDataClergyAssign(edit, skipChucVu);
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("StartDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("EndDate", "Đến năm không hợp lệ");
                if (!string.IsNullOrEmpty(fromdate))
                    edit.StartDate = fromdate;
                if (!string.IsNullOrEmpty(todate))
                    edit.EndDate = todate;
                
                var assign = new Clergy_AssignmentHistory
                {
                    ClergyId = edit.ClergyId,
                    ChurchId = edit.ChurchId,
                    StartDate =  edit.StartDate,
                    EndDate = edit.EndDate,
                    Information = edit.Information,
                    TitleId = edit.TitleId,
                    NamChucVu = edit.NamChucVu,
                    Term = edit.Term,
                    TyLeLuu = edit.TyLeLuu,
                    IsCurrent = edit.IsCurrent,
                    ChucVuId =  edit.ChucVuId,
                    PaperNumber = edit.PaperNumber,
                    TermNumber = edit.TermNumber,
                    TermType =  edit.TermType,
                    
                };
                if (skipChucVu)
                {
                    assign.ChurchId = null;
                }
                if (edit.CapChiHoi > 0)
                    assign.CapChiHoi = edit.CapChiHoi;
                else assign.CapChiHoi = null;
                assign.Role = edit.Role;
                if (string.IsNullOrEmpty(assign.Role))
                {
                    assign.Role = string.Empty;
                }
                assign.TyLeLuu = edit.TyLeLuu;
                assign.OfficeId = edit.OfficeId == 0 ? null : edit.OfficeId;
                assign.TermNumber = edit.TermNumber;    
                assign.ChinhQuyenCN = edit.ChinhQuyenCN;
                assign.ChinhQuyenCNSo = edit.ChinhQuyenCNSo;
                if (edit.OfficeId > 0)
                    assign.OfficeId = edit.OfficeId;
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
                    var CQCongNhan = Request.Files["ChinhQuyenCN"];
                    if (CQCongNhan != null && CQCongNhan.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        importPath = Path.Combine(Constant.Document_import,
                        assign.ClergyId.ToString(CultureInfo.InvariantCulture));
                        var fileName = CommonFunction.GetNewFileName(importPath,
                            Path.GetFileName(CQCongNhan.FileName));
                        if (fileName != null)
                        {
                            systemLocation = Path.Combine(importPath, fileName);
                            importPath = Request.MapPath(importPath);
                            var physicalpath = Request.MapPath(systemLocation);
                            if (!Directory.Exists(importPath))
                            {
                                Directory.CreateDirectory(importPath);
                            }
                            CQCongNhan.SaveAs(physicalpath);
                        }
                        assign.ChinhQuyenCN = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["ChinhQuyenCN"]))
                    {
                        assign.ChinhQuyenCN = string.Empty;
                    }
                    assign.ClergyId = clergy;
                    assign.Role = string.IsNullOrWhiteSpace(edit.Role) ? " " : edit.Role;
                    db.Clergy_AssignmentHistory.Add(assign);
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                   
                    if (edit.IsCurrent == true)
                    {
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == assign.ClergyId);
                        if (cleargy != null)
                        {
                            cleargy.CurrentAssignment_Id = assign.id;
                            cleargy.ChucVuId = assign.ChucVuId;
                            cleargy.ThoiGianChucVu = CommonFunction.ConvertCustomDateTime(edit.EndDate);
                            db.SaveChanges();
                        }
                    }

                    Response.Redirect("~/Home/MucsuThongTinChucVu/" + clergy);
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
            if(cleargy!=null)
                editObject.IsCurrent = cleargy.CurrentAssignment_Id == editObject.id;
            ViewBag.IsCurrent = editObject.IsCurrent;
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
                var fromdate = Request["StartDate"];
                var todate = Request["EndDate"];
                var skipChucVu = false;
                var currentChucVu = db.ChucVus.FirstOrDefault(a => a.id == edit.ChucVuId);
                if (currentChucVu != null && (currentChucVu.Name.Contains("Chờ Nhiệm sở") || currentChucVu.Name.Contains("Du học")))
                {
                    skipChucVu = true;
                }
                var erros = ValidateFunction.ValidateDataClergyAssign(edit, skipChucVu);
                if (CommonFunction.IsValidateDateString(fromdate))
                    erros.Add("StartDate", "Từ ngày không hợp lệ.");
                if (CommonFunction.IsValidateDateString(todate))
                    erros.Add("EndDate", "Đến năm không hợp lệ");
                if(!string.IsNullOrEmpty(fromdate))
                    edit.StartDate = fromdate;
                if (!string.IsNullOrEmpty(todate))
                    edit.EndDate = todate;
                if (assign != null && erros.Count == 0)
                {
                    assign.StartDate =  edit.StartDate;
                    assign.EndDate = edit.EndDate;
                    assign.ClergyId = edit.ClergyId;
                    assign.ChurchId = edit.ChurchId;
                    if (skipChucVu)
                    {
                        assign.ChurchId = null;
                    }
                    assign.Information = edit.Information;
                    assign.TitleId = edit.TitleId;
                    assign.NamChucVu = edit.NamChucVu;
                    assign.ChucVuId = edit.ChucVuId;
                    assign.PaperNumber = edit.PaperNumber;
                    assign.TermType = edit.TermType;
                    assign.Term = edit.Term;
                    assign.Role = edit.Role;
                    if (string.IsNullOrEmpty(assign.Role))
                    {
                        assign.Role = string.Empty;
                    }
                    assign.TyLeLuu = edit.TyLeLuu;
                    assign.OfficeId = edit.OfficeId == 0 ? null : edit.OfficeId;
                    assign.TermNumber = edit.TermNumber;
                    assign.ChinhQuyenCNSo = edit.ChinhQuyenCNSo;

                    if (edit.CapChiHoi.HasValue && edit.CapChiHoi.Value > 0)
                        assign.CapChiHoi = edit.CapChiHoi;
                    else assign.CapChiHoi = null;
                    assign.IsCurrent = edit.IsCurrent;
                    if (assign.IsCurrent == true)
                    {
                        db.Database.ExecuteSqlCommand(@"UPDATE Clergy_AssignmentHistory  SET IsCurrent =0 WHERE Id = {0}", edit.ClergyId);
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == edit.ClergyId);
                        if (cleargy != null)
                        {
                            cleargy.CurrentAssignment_Id = assign.id;
                            cleargy.ChucVuId = assign.ChucVuId;
                            cleargy.CurrentChurchId = edit.ChurchId;
                            if (skipChucVu)
                            {
                                cleargy.CurrentChurchId = null;
                            }
                            cleargy.ThoiGianChucVu = CommonFunction.ConvertCustomDateTime(edit.EndDate);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == edit.ClergyId);
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

                    var CQCongNhan = Request.Files["ChinhQuyenCN"];
                    if (CQCongNhan != null && CQCongNhan.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        importPath = Path.Combine(Constant.Document_import,
                        assign.ClergyId.ToString(CultureInfo.InvariantCulture));
                        var fileName = CommonFunction.GetNewFileName(importPath,
                            Path.GetFileName(CQCongNhan.FileName));
                        if (fileName != null)
                        {
                            systemLocation = Path.Combine(importPath, fileName);
                            importPath = Request.MapPath(importPath);
                            var physicalpath = Request.MapPath(systemLocation);
                            if (!Directory.Exists(importPath))
                            {
                                Directory.CreateDirectory(importPath);
                            }
                            CQCongNhan.SaveAs(physicalpath);
                        }
                        assign.ChinhQuyenCN = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["ChinhQuyenCN"]))
                    {
                        assign.ChinhQuyenCN = string.Empty;
                    }

                    db.SaveChanges();
                    Response.Redirect("~/Home/MucsuThongTinChucVu/" + edit.ClergyId);
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
                var giaoPhamId = clergyTitlehistory.ClergyId;
                db.Clergy_AssignmentHistory.Remove(clergyTitlehistory);
                db.SaveChanges();
                Response.Redirect("~/Home/MucsuThongTinChucVu/" + giaoPhamId);
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