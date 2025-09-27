using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Areas.Admin.Controllers
{
    public class Clergy_TitleHistoryController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        //
        // GET: /Admin/Default1/Details/5

        public ViewResult Details(int id)
        {
            GetDefaultValue();
            Clergy_TitleHistory clergy_titlehistory = db.Clergy_TitleHistory.Single(c => c.id == id);
            return View(clergy_titlehistory);
        }

        //
        // GET: /Admin/Default1/Create

        public ActionResult Create(int? cleargyid)
        {
            GetDefaultValue();
            var title = new Clergy_TitleHistory();
            ViewBag.CleargyId = cleargyid;
            return View(title);
        } 

        //
        // POST: /Admin/Default1/Create

        [HttpPost]
        public ActionResult Create(Clergy_TitleHistory clergyTitlehistory)
        {
            GetDefaultValue();
            #region save
            if (Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateDataTitleHistory(clergyTitlehistory);
                string EffectiveDate = Request["EffectiveDate"];
                if (CommonFunction.IsValidateDateString(EffectiveDate))
                {
                    erros.Add("EffectiveDate", "Ngày có hiệu lực không hợp lệ");
                }
                if(erros.Count==0)
                {
                    clergyTitlehistory.EffectiveDate = EffectiveDate;
                    var obj = new Clergy_TitleHistory
                    {
                        TitleId = clergyTitlehistory.TitleId,
                        CleargyId = CommonFunction.ConvertInt(Request["CleargyId"]),
                        EffectiveDate = clergyTitlehistory.EffectiveDate,
                        IsCurrent = clergyTitlehistory.IsCurrent,
                        DecisionNumber = clergyTitlehistory.DecisionNumber,
                        DocumentNumber = clergyTitlehistory.DocumentNumber
                    };
                    var importPath = Path.Combine(Constant.Document_import,
                                                  obj.CleargyId.Value.ToString(CultureInfo.InvariantCulture));
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
                        obj.ApprovalPaper = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtQuyetDinh"]))
                    {
                        obj.ApprovalPaper = string.Empty;
                    }
                    var requestPaper = Request.Files["RequestPaper"];
                    importPath = Path.Combine(Constant.Document_import,
                                              obj.CleargyId.Value.ToString(CultureInfo.InvariantCulture));
                    if (requestPaper != null && requestPaper.FileName.Length > 0)
                    {
                        var systemLocation = "";
                        var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(requestPaper.FileName));
                        if (fileName != null)
                        {
                            systemLocation = Path.Combine(importPath, fileName);
                            importPath = Request.MapPath(importPath);
                            var physicalpath = Request.MapPath(systemLocation);
                            if (!Directory.Exists(importPath))
                            {
                                Directory.CreateDirectory(importPath);
                            }
                            requestPaper.SaveAs(physicalpath);
                        }
                        obj.RequestPaper = systemLocation;
                    }
                    else if (string.IsNullOrEmpty(Request["txtBienBan"]))
                    {
                        obj.RequestPaper = string.Empty;
                    }
                    db.Clergy_TitleHistory.Add(obj);
                    db.SaveChanges();
                    if (obj.IsCurrent == true)
                    {
                        var cleargy = db.Clergies.FirstOrDefault(a => a.Id == obj.CleargyId);
                        if (cleargy != null)
                        {
                            cleargy.TitleId = obj.TitleId;
                            db.SaveChanges();
                        }
                    }

                    Response.Redirect("~/Home/MucsuThongTinChucVu/" + clergyTitlehistory.CleargyId);
                }
                ViewBag.Errors = erros;
            }
            #endregion
            ViewBag.CleargyId = clergyTitlehistory.CleargyId;
            return View(clergyTitlehistory);
        }

        //
        // GET: /Admin/Default1/Edit/5
 
        public ActionResult Edit(int id)
        {
            GetDefaultValue();
            var clergy_titlehistory = db.Clergy_TitleHistory.FirstOrDefault(c => c.id == id);
            return View(clergy_titlehistory);
        }

        //
        // POST: /Admin/Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(Clergy_TitleHistory clergy_titlehistory)
        {
            GetDefaultValue();
            
            if (  Request["btnSave"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateDataTitleHistory(clergy_titlehistory);
                string EffectiveDate = Request["EffectiveDate"];
                if (CommonFunction.IsValidateDateString(EffectiveDate))
                {
                    erros.Add("EffectiveDate", "Ngày có hiệu lực không hợp lệ");
                }
                if (erros.Count == 0)
                {
                    clergy_titlehistory.EffectiveDate = EffectiveDate;
                    var title = db.Clergy_TitleHistory.FirstOrDefault(a => a.id == clergy_titlehistory.id);
                    if (title != null)
                    {
                        title.EffectiveDate = clergy_titlehistory.EffectiveDate;
                        title.DecisionNumber = clergy_titlehistory.DecisionNumber;
                        title.DocumentNumber = clergy_titlehistory.DocumentNumber;
                        title.IsCurrent = clergy_titlehistory.IsCurrent;
                        var importPath = Path.Combine(Constant.Document_import,
                                                      title.CleargyId.Value.ToString(CultureInfo.InvariantCulture));
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
                            title.ApprovalPaper = systemLocation;
                        }
                        else if (string.IsNullOrEmpty(Request["txtQuyetDinh"]))
                        {
                            title.ApprovalPaper = string.Empty;
                        }
                        if (title.IsCurrent == true)
                        {
                            var cleargy = db.Clergies.FirstOrDefault(a => a.Id == title.CleargyId);
                            if (cleargy != null)
                            {
                                cleargy.TitleId = title.TitleId;
                                db.SaveChanges();
                            }
                        }

                        importPath = Path.Combine(Constant.Document_import,
                                                     title.CleargyId.Value.ToString(CultureInfo.InvariantCulture));
                        var requestPaper = Request.Files["RequestPaper"];
                        if (requestPaper != null && requestPaper.FileName.Length > 0)
                        {
                            var systemLocation = "";
                            var fileName = CommonFunction.GetNewFileName(importPath,
                                                                         Path.GetFileName(requestPaper.FileName));
                            if (fileName != null)
                            {
                                systemLocation = Path.Combine(importPath, fileName);
                                importPath = Request.MapPath(importPath);
                                var physicalpath = Request.MapPath(systemLocation);
                                if (!Directory.Exists(importPath))
                                {
                                    Directory.CreateDirectory(importPath);
                                }
                                requestPaper.SaveAs(physicalpath);
                            }
                            title.RequestPaper = systemLocation;
                        }
                        else if (string.IsNullOrEmpty(Request["txtBienBanChucDanh"]))
                        {
                            title.RequestPaper = string.Empty;
                        }
                        title.TitleId = clergy_titlehistory.TitleId;
                        db.SaveChanges();
                    }
                    
                    Response.Redirect("~/Home/MucsuThongTinChucVu/" + clergy_titlehistory.CleargyId);
                }
                ViewBag.Errors = erros;

            }
            //ViewBag.CleargyId = new SelectList(db.Clergies, "Id", "FirstName", clergy_titlehistory.CleargyId);
            //ViewBag.TitleId = new SelectList(db.ClergyTitles, "Id", "Title", clergy_titlehistory.TitleId);
            return View(clergy_titlehistory);
        }

        //
        // GET: /Admin/Default1/Delete/5
 
        public ActionResult Delete(int id)
        {
            GetDefaultValue();
            Clergy_TitleHistory clergy_titlehistory = db.Clergy_TitleHistory.Single(c => c.id == id);
            return View(clergy_titlehistory);
        }

        //
        // POST: /Admin/Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetDefaultValue();
            var clergyTitlehistory = db.Clergy_TitleHistory.FirstOrDefault(c => c.id == id);
            if(clergyTitlehistory!=null)
            {
                var giaoPhamId = clergyTitlehistory.CleargyId;
                db.Clergy_TitleHistory.Remove(clergyTitlehistory);
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