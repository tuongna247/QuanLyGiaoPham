using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.SPEntity;
using HTTLVN.QLTLH.Models.Warper;
using Kendo.Mvc.Extensions;
using Telerik.Web.Mvc;
using Kendo.Mvc.UI;
using EntityState = System.Data.Entity.EntityState;
using GiaoPhamThoiGianPhucVu_Result = HTTLVN.QLTLH.Models.GiaoPhamThoiGianPhucVu_Result;
using TimKiemGiaoPhamHetHan_Result = HTTLVN.QLTLH.Models.TimKiemGiaoPhamHetHan_Result;
using TimKiemGiaoPhamNangCao_Result = HTTLVN.QLTLH.Models.TimKiemGiaoPhamNangCao_Result;
using TimKiemNangCao_Result = HTTLVN.QLTLH.Models.TimKiemNangCao_Result;
using TimKiemVoiNhieuThamSo_Result = HTTLVN.QLTLH.Models.TimKiemVoiNhieuThamSo_Result;
using System.Text.RegularExpressions;

//using CrystalDecisions.Shared;

namespace HTTLVN.QLTLH.Controllers
{
    public class HomeController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();

        //
        // GET: /Index/

        public ActionResult Index()
        {
            using (CodeFirstTongLienHoiEntities tongLienHoiEntities = new CodeFirstTongLienHoiEntities())
            {
                var now = DateTime.Now;
                var next3Month = now.AddMonths(3);
                ViewBag.GetEndDay = tongLienHoiEntities.V_GetEndDay.Where(a => a.KetThucChucVu <= next3Month).ToList();
                //var allEndDate  = tongLienHoiEntities.V_GetAllEndDay.ToList();
                //ViewBag.GetAllEndDay = allEndDate.ToList();
                var clergies =
                      tongLienHoiEntities.Clergies.ToList();
                if (Request["btnlogin"] == "login")
                {
                    var username = Request["login-email"];
                    var password = Request["login-password"];
                    var rememberme = Request["rememberme"] == "1";
                    var user =
                        db.Users.FirstOrDefault(a => a.username.ToLower() == username.ToLower());
                    if (user != null && CommonFunction.CheckPassword(password, user.password))
                    {
                        Session[Constant.SessionLogin] = user;
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        return View(clergies);
                    }
                }
                GetDefaultValue();
                return View(clergies);
            }


        }

        public ActionResult GiaoPhamManNhiem()
        {
            GetDefaultValue();
            using (CodeFirstTongLienHoiEntities tongLienHoiEntities = new CodeFirstTongLienHoiEntities())
            {
                var now = DateTime.Now;
                var next3Month = now.AddMonths(3);
                List<HTTLVN.QLTLH.Models.V_GetAllEndDay> bindList = new List<Models.V_GetAllEndDay>();
                var endates =
                    tongLienHoiEntities.V_GetEndDay.Where(a => a.KetThucChucVu <= next3Month).OrderBy(a => a.KetThucChucVu).ToList();
                var clergys = db.Clergies.ToList();
                foreach (var item in endates)
                {
                    var _item = new Models.V_GetAllEndDay();
                    var clergy = clergys.FirstOrDefault(a => a.Id == item.Id);
                    _item.Clergy = clergy;
                    if (clergy.Clergy_AssignmentHistory != null && clergy.Clergy_AssignmentHistory.Count() > 0)
                    {
                        var nhiemso = clergy.Clergy_AssignmentHistory.OrderByDescending(a => CommonFunction.ConvertDateTime(a.EndDate)).FirstOrDefault(a => a.IsCurrent == true);
                        if (nhiemso != null && nhiemso.Church != null)
                        {
                            _item.NhiemSo = nhiemso.Church.ChurchName;
                            if (nhiemso.Church.City != null)
                                _item.TinhThanh = nhiemso.Church.City.Name;
                        }
                    }
                    else if (clergy.Clergy_AssignmentHistory == null && clergy.Clergy_AssignmentHistory1 != null)
                    {
                        var nhiemso = clergy.Clergy_AssignmentHistory1;
                        if (nhiemso != null && nhiemso.Church != null)
                        {
                            _item.NhiemSo = nhiemso.Church.ChurchName;
                            if (nhiemso.Church.City != null)
                                _item.TinhThanh = nhiemso.Church.City.Name;
                        }
                    }
                    _item.FirstName = item.FirstName;
                    _item.Id = item.Id;
                    _item.MiddleName = item.MiddleName;
                    _item.LastName = item.LastName;
                    _item.KetThucChucVu = item.KetThucChucVu;
                    bindList.Add(_item);
                }
                ViewBag.GiaoPhamManNhiem = bindList;
            }

            return View();
        }

        #region Search

        #region GemCode
        //public ActionResult SearchMucSuHetHan()
        //{
        //    GetDefaultValue();
        //    ViewData["todate"] = DateTime.Now.AddMonths(3);
        //    ViewData["fromdate"] = DateTime.Now;
        //    return View();
        //}



        //[HttpPost]
        //public ActionResult SearchMucSuHetHan(FormCollection frm)
        //{
        //    GetDefaultValue();
        //    ViewData["todate"] = DateTime.Now.AddMonths(3);
        //    ViewData["fromdate"] = DateTime.Now;
        //    if (frm["btnexport_excel"] == "xuất file excel")
        //    {
        //        var fromdate = CommonFunction.ConvertDateTime(frm["fromdate"], true);
        //        var todate = CommonFunction.ConvertDateTime(frm["todate"], true);
        //        var chucdanh = CommonFunction.ConvertInt(frm["chucdanh"]);
        //        var vanthus = GetGiaoPhamHetHan(frm["firstname"], fromdate, todate, chucdanh).OrderBy(a => a.EndDate).ToList();
        //        ///Save file excel o day
        //        var ms = new MemoryStream();
        //        using (var sl = new SLDocument())
        //        {
        //            var tbl = sl.CreateTable("B3", "F6");
        //            tbl.SetTableStyle(SLTableStyleTypeValues.Medium4);
        //            tbl.HasAutoFilter = false;
        //            sl.InsertTable(tbl);

        //            sl.SetColumnWidth("C", 30);
        //            sl.SetColumnWidth("D", 15);
        //            sl.SetColumnWidth("E", 20);
        //            sl.SetColumnWidth("F", 20);

        //            sl.MergeWorksheetCells(1, 2, 1, 6);
        //            sl.SetCellValue("B1", "Danh sách giáo phẩm hết nhiệm kỳ từ ngày: " + fromdate.ToString("yy-mmm-dd") + " đến ngày:" + todate.ToString("yy-mmm-dd"));

        //            sl.SetCellValue("B3", "STT");
        //            sl.SetCellValue("C3", "Họ và tên");
        //            sl.SetCellValue("D3", "Ngày hết hạn");
        //            sl.SetCellValue("E3", "Chức vụ");
        //            sl.SetCellValue("F3", "Tên Nhiệm sở");
        //            for (var i = 4; i < vanthus.Count + 4; i++)
        //            {
        //                sl.SetCellValueNumeric("B" + i, i - 3 + "");
        //                sl.SetCellValue("C" + i, vanthus[i - 4].FirstName + " " + vanthus[i - 4].LastName);
        //                var dateTime = vanthus[i - 4].EndDate;
        //                //if (dateTime != null)
        //                //    sl.SetCellValueNumeric("D" + i, dateTime.Value.ToString("yy-mm-dd"));
        //                sl.SetCellValue("E" + i, vanthus[i - 4].TenChucDanh);
        //                sl.SetCellValue("F" + i, vanthus[i - 4].TenNhiemSo);
        //            }

        //            sl.SaveAs(ms);
        //        }
        //        ms.Position = 0;

        //        return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        //    }
        //    return View();
        //}

        //public ActionResult Search()
        //{
        //    GetDefaultValue();
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Search(FormCollection frm)
        //{
        //    GetDefaultValue();
        //    return View();
        //}
        //public ActionResult SearchThoiGianPhucVu()
        //{
        //    GetDefaultValue();

        //    return View();
        //}

        //public ActionResult Search2()
        //{
        //    GetDefaultValue();
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Search2(FormCollection frm, int? currentPage)
        //{
        //    GetDefaultValue();
        //    return View();
        //}

        //step2
        //public ActionResult MucsuQuaTrinhThanHoc(int? id, string request,int? dataid)
        //{
        //    GetDefaultValue();
        //    if (request == "delete" && dataid > 0)
        //    {
        //        var edu = db.Clergy_Education.FirstOrDefault(a => a.Id == dataid);
        //        if (edu != null )
        //        {
        //            db.Clergy_Education.Delete(edu);
        //            db.SaveChanges();
        //        }
        //    }
        //    var clergy = db.Clergies.FirstOrDefault(c => c.Id == id);
        //    ViewBag.GiaoPhamId = id;
        //    var list = new List<QuaTrinhThanHocDBO>();
        //    if (clergy != null)
        //    {
        //        ViewBag.GiaoPham = clergy;
        //        list.AddRange(clergy.Clergy_Education.Select(quaTrinhThanHocDbo => new QuaTrinhThanHocDBO(quaTrinhThanHocDbo)));
        //        if (request == "addmore")
        //        {
        //            if (id != null) list.Add(new QuaTrinhThanHocDBO(){ GiaoPhamId =  id.Value});
        //        }

        //    }
        //    return View(list);
        //}

        //[HttpPost]
        //public ActionResult MucsuQuaTrinhThanHoc(List<QuaTrinhThanHocDBO> giaopham)
        //{
        //    GetDefaultValue();
        //    var giaophamId = CommonFunction.ConvertInt(Request["GiaoPhamId"]);
        //    ViewBag.GiaoPhamId = giaophamId;
        //    var clergy = db.Clergies.FirstOrDefault(c => c.Id == giaophamId);
        //    ViewBag.GiaoPham = clergy;
        //    if (Request["btnAddMore"] == "Thêm mới")
        //    {
        //        return RedirectToAction("MucsuQuaTrinhThanHoc", new { id = giaophamId, request = "addmore" });
        //    }
        //    #region Save
        //    if (Request["btnnext"] == "Lưu" && giaopham != null && giaopham.Count>0)
        //    {

        //        foreach (var quaTrinhThanHocDbo in giaopham)
        //        {
        //            int degreeId = quaTrinhThanHocDbo.Id;
        //            var education = db.Clergy_Education.FirstOrDefault(a => a.Id == degreeId) ?? new Clergy_Education();
        //            var fileRequest = string.Format("chungchi.{0}", degreeId);
        //            var file = Request.Files[fileRequest];
        //            var importPath = Path.Combine(Constant.Document_import,
        //                                          ViewBag.GiaoPhamId.ToString(CultureInfo.InvariantCulture));
        //            if (file != null && file.FileName.Length > 0)
        //            {
        //                var systemLocation = "";
        //                var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(file.FileName));
        //                if (fileName != null)
        //                {
        //                    systemLocation = Path.Combine(importPath, fileName);
        //                    importPath = Request.MapPath(importPath);
        //                    var physicalpath = Request.MapPath(systemLocation);
        //                    if (!Directory.Exists(importPath))
        //                    {
        //                        Directory.CreateDirectory(importPath);
        //                    }
        //                    file.SaveAs(physicalpath);
        //                }

        //                var fileExtension = Path.GetExtension(systemLocation);
        //                var fileType = MimeAssistant.GetMimeType(fileExtension);
        //                var fileInfo = new FileInfo(Request.MapPath(systemLocation));
        //                education.Paper = systemLocation;
        //                education.filesize = CommonFunction.ConvertInt(fileInfo.Length);
        //                education.mine_type = fileType;

        //            }
        //            education.CollegeOrProgramme = quaTrinhThanHocDbo.CollegeOrProgramme;
        //            education.Degree = quaTrinhThanHocDbo.Degree;
        //            if (education.ClergyId == 0 && giaopham != null)
        //                education.ClergyId = ViewBag.GiaoPhamId;
        //            education.CourseId = quaTrinhThanHocDbo.CourseId;
        //            education.FromDate = quaTrinhThanHocDbo.FromDate;
        //            education.ToDate = quaTrinhThanHocDbo.ToDate;
        //            if (education.ClergyId == 0)
        //                education.ClergyId = giaophamId;
        //            if (education.Id == 0)
        //            {
        //                db.Clergy_Education.Add(education);
        //            }
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("MucsuQuaTrinhThanHoc", new { id = ViewBag.GiaoPhamId });
        //    }
        //    #endregion
        //    return RedirectToAction("MucsuQuaTrinhThanHoc", new { id = ViewBag.GiaoPhamId });
        //    return View(giaopham);
        //}



        #endregion

        public ActionResult SearchNangCao()
        {
            GetDefaultValue();
            ViewData["tunam"] = DateTime.Now.AddYears(-40);
            ViewData["dennam"] = DateTime.Now;
            return View();
        }

        [HttpPost]
        public ActionResult SearchNangCao(FormCollection frm)
        {
            GetDefaultValue();
            if (Request["btnXuat"] == "Xuất")
            {
                #region Export Pdf And Excel

                var tinhthanh = CommonFunction.ConvertInt(frm["tinhthanh"]);
                var chucvutu = CommonFunction.ConvertInt(frm["chucvutu"]);
                var chucvuden = CommonFunction.ConvertInt(frm["chucvuden"]);
                var nhiemkytu = CommonFunction.ConvertInt(frm["nhiemkytu"]);
                var nhiemkyden = CommonFunction.ConvertInt(frm["nhiemkyden"]);
                var namsinhtu = CommonFunction.ConvertInt(frm["namsinhtu"]);
                var namsinhden = CommonFunction.ConvertInt(frm["namsinhden"]);
                var tinhtrang = CommonFunction.ConvertInt(frm["tinhtrang"]);
                var chucdanh = CommonFunction.ConvertInt(frm["chucdanh"]);
                var chucvu = CommonFunction.ConvertInt(frm["chucvu"]);
                var nhiemso = CommonFunction.ConvertInt(frm["nhiemso"]);
                var capnhiemso = CommonFunction.ConvertInt(frm["capnhiemso"]);
                var dantoc = CommonFunction.ConvertInt(frm["dantoc"]);
                var hocvi = CommonFunction.ConvertInt(frm["hocvi"]);
                var cmnd = string.IsNullOrEmpty(frm["cmnd"]) ? null : frm["cmnd"];
                var ttTlh = !string.IsNullOrEmpty(frm["tt_tlh"]) && frm["tt_tlh"] == "Yes";
                var btsTlh = !string.IsNullOrEmpty(frm["bts_tlh"]) && frm["bts_tlh"] == "Yes";
                var hdgp = !string.IsNullOrEmpty(frm["hdgp"]) && frm["hdgp"] == "Yes";
                var nguoikinh = !string.IsNullOrEmpty(frm["nguoikinh"]) && frm["nguoikinh"] == "Yes";
                var name = string.IsNullOrEmpty(frm["clergyName"]) ? null : frm["clergyName"];

                var list =
                    GetSearchNangCao(tinhthanh, chucvutu, chucvuden, nhiemkytu, nhiemkyden, namsinhtu, namsinhden,
                        tinhtrang, chucdanh, chucvu, nhiemso, capnhiemso, dantoc, hocvi, cmnd, name, ttTlh, btsTlh, hdgp, nguoikinh, true).ToList();
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.MiddleName))
                        item.FullName = item.FirstName + " " + item.LastName;
                    else
                        item.FullName = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                    item.FullName = item.FullName.ToUpper();
                    //item.TenNhiemSo = Regex.Replace(item.TenNhiemSo, "<.*?>", String.Empty);
                }

                var rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GiaoPhamSearchNangCao.rpt"));
                var datatable = CommonFunction.ToDataTable(list);
                rd.SetDataSource(datatable);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                if (Request["dataType"] == "Excel")
                {
                    var stream = rd.ExportToStream(ExportFormatType.ExcelWorkbook);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/xlsx",
                        string.Format("giaopham_report_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Pdf")
                {
                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf",
                        string.Format("giaopham_report_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Doc")
                {
                    var stream = rd.ExportToStream(ExportFormatType.WordForWindows);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/doc",
                        string.Format("giaopham_report_{0}.doc", DateTime.Now.ToString("dd_MM_yyyy")));
                }

                #endregion

                ViewBag.LoaiVanThu = ViewData["loaivanthu"];
            }
            return View();
        }



        public ActionResult _PagingDienKhac(string name, int? chucvu, int? chucdanh, int? capchihoi, bool ishethan, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrdersDienKhac(name, chucvu, chucdanh, capchihoi, ishethan).ToDataSourceResult(request));

        }

        private IEnumerable<TimKiemVoiNhieuThamSo_Result> GetOrdersDienKhac(string name, int? chucvu, int? chucdanh, int? capchihoi, bool ishetnhiemky)
        {
            using (CodeFirstTongLienHoiEntities tongLienHoiEntities = new CodeFirstTongLienHoiEntities())
            {
                var result = tongLienHoiEntities.TimKiemVoiNhieuThamSo(name, 0, chucvu, capchihoi).ToList();
                //dinh cu nuoc ngoai
                switch (chucdanh)
                {
                    case 102:
                        result = result.Where(a => a.DinhCuNuocNgoai.HasValue && a.DinhCuNuocNgoai.Value).ToList();
                        break;
                    case 103:
                        result = result.Where(a => a.KyLuat.HasValue && a.KyLuat.Value).ToList();
                        break;
                    case 104:
                        result = result.Where(a => !string.IsNullOrWhiteSpace(a.DateOfDeath) || (a.IsVeVoiChua.HasValue && a.IsVeVoiChua.Value)).ToList();
                        break;
                    case 105:
                        result = result.Where(a => a.DuHoc.HasValue && a.DuHoc.Value).ToList();
                        break;
                    case 108:
                        result = result.Where(a => a.NghiChucVu.HasValue && a.NghiChucVu.Value).ToList();
                        break;
                }
                //huu tri
                if (chucdanh == 106)
                {
                    result = result.Where(a => a.HuuTri.HasValue && a.HuuTri.Value).ToList();
                }
                if (chucdanh == 107)
                {
                    result = result.Where(a => a.QuaPhu.HasValue && a.QuaPhu.Value).ToList();
                }
                var hethan = db.Clergy_AssignmentHistory.ToList();
                if (ishetnhiemky)
                {
                    var groupHetHanByMucSu = hethan.GroupBy(a => a.ClergyId);
                    var mucsus = groupHetHanByMucSu.Select(@group => result.Find(a => a.Id == @group.Key)).Where(findout => findout != null).ToList();
                    result = mucsus;
                }
                var listAssigMents = db.Clergy_AssignmentHistory.ToList();
                result = ConverFixAvarta(result, listAssigMents);
                result = SortOrderByBTSTLH(result);
                result = RemoveNhiemSo(result);
                return result;
            }
        }



        public ActionResult _Paging(string name, int? chucvu, int? chucdanh, int? capchihoi, bool ishethan, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders(name, chucvu, chucdanh, capchihoi).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        private IEnumerable<TimKiemVoiNhieuThamSo_Result> GetOrders(string name, int? chucvu, int? chucdanh, int? capchihoi)
        {
            using (CodeFirstTongLienHoiEntities tongLienHoiEntities = new CodeFirstTongLienHoiEntities())
            {
                var result =
                    tongLienHoiEntities.TimKiemVoiNhieuThamSo(name, chucdanh, chucvu, capchihoi)
                        .Where(
                            a =>
                                string.IsNullOrEmpty(a.DateOfDeath) && (a.KyLuat == false || a.KyLuat == null) &&
                                 (a.NghiChucVu == false || a.NghiChucVu == null) &&
                                (a.HuuTri == false || a.HuuTri == null) &&
                                (a.DinhCuNuocNgoai == false || a.DinhCuNuocNgoai == null))
                        .ToList();
                var listAssigMents = db.Clergy_AssignmentHistory.ToList();
                result = ConverFixAvarta(result, listAssigMents);
                result = SortOrderByBTSTLH(result);
                return result;
            }
        }



        private List<TimKiemVoiNhieuThamSo_Result> SortOrderByBTSTLH(List<TimKiemVoiNhieuThamSo_Result> list)
        {
            List<TimKiemVoiNhieuThamSo_Result> result = new List<TimKiemVoiNhieuThamSo_Result>();

            result = list.OrderByDescending(a => a.Sort).ToList();

            return result;
        }


        public ActionResult GetData([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders().ToDataSourceResult(request));
        }

        private IEnumerable<MucSuDBO> GetOrders()
        {
            var list = db.Clergies.ToList();
            return list.Select(a => new MucSuDBO() { FirstName = a.FirstName, LastName = a.LastName });

        }
        private List<TimKiemVoiNhieuThamSo_Result> RemoveNhiemSo(List<TimKiemVoiNhieuThamSo_Result> result)
        {
            var list = new List<TimKiemVoiNhieuThamSo_Result>();
            foreach (var item in result)
            {
                item.TenNhiemSo =  string.Empty;
                list.Add(item);
            }
            return list;
        }
        private List<TimKiemVoiNhieuThamSo_Result> ConverFixAvarta(List<TimKiemVoiNhieuThamSo_Result> result, List<Models.Clergy_AssignmentHistory> listAssigMents)
        {
            var list = new List<TimKiemVoiNhieuThamSo_Result>();
            foreach (var item in result)
            {
                if (item.CurrentAssignment_Id.HasValue)
                {
                    var assignment = listAssigMents.FirstOrDefault(a => a.id == item.CurrentAssignment_Id.Value);
                    if (assignment != null)
                    {
                        var chucdanh = string.Empty;
                        if (assignment.ChucVu != null)
                            chucdanh = assignment.ChucVu.Name;
                        var nhiemso = string.Empty;
                        if (!string.IsNullOrEmpty(chucdanh))
                            nhiemso += chucdanh + " ";
                        if (assignment.Church != null && assignment.Church.CapChiHoi != null)
                            nhiemso = assignment.Church.CapChiHoi.Description;
                        if (assignment.Church != null)
                        {
                            nhiemso = nhiemso + " " + assignment.Church.ChurchName;
                            item.TenNhiemSo = string.Format("<a href='/NhiemSo/ChiTietNhiemSo/{0}' class=''> {1} </a>",
                                assignment.ChurchId.Value, nhiemso);
                        }
                        else
                        {
                            item.TenNhiemSo = string.Format("{0}", "Chờ Nhiệm Sở");
                        }

                    }
                }
                if (!string.IsNullOrWhiteSpace(item.Avatar) && System.IO.File.Exists(Request.MapPath(Url.Content(item.Avatar))))
                    item.Avatar = Url.Content(item.Avatar);
                else
                    item.Avatar = Url.Content("~/Content/Content/img/placeholders/image_dark_64x64.png");
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.TenNhiemSo = item.TenNhiemSo ?? string.Empty;

                list.Add(item);
            }
            return list;
        }

      
        public ActionResult UpdateSoNamChucVu()
        {
            //
            var giaopham = db.Clergies.ToList();
            foreach (var clergy in giaopham)
            {
                var sonamChucvu = 0;
                var soThangChucVu = 0;
                var nhanNhiemSos = clergy.Clergy_AssignmentHistory.ToList();
                string endDate = string.Empty;
                var titleHistory = clergy.Clergy_TitleHistory.OrderBy(a => CommonFunction.ConvertCustomDateTime(a.EffectiveDate)).FirstOrDefault();
                var gianDoancvs = clergy.Clergy_Conduct.Where(a => !string.IsNullOrEmpty(a.Fromdate) && !string.IsNullOrEmpty(a.ToDate)).ToList();
                var thoigianChucVu = DateTime.MinValue;
                var timeSpanChucVu = new TimeSpan();
                var isGianDoanChucVu = false;
                var gianDoanChucVuDetail = " ";
                int? currentChurchId = -1;
                if (nhanNhiemSos.Count > 0)
                {
                    var currentNhiemSo = nhanNhiemSos.Where(a => a.IsCurrent == true && a.id == clergy.CurrentAssignment_Id).OrderByDescending(a => CommonFunction.ConvertCustomDateTime(a.EndDate)).FirstOrDefault();
                    if (currentNhiemSo != null && CommonFunction.ConvertCustomDateTime(currentNhiemSo.EndDate) > DateTime.MinValue)
                    {
                        thoigianChucVu = CommonFunction.ConvertCustomDateTime(currentNhiemSo.EndDate);
                        db.Database.ExecuteSqlCommand(@"UPDATE Clergy SET KetThucChucVu = {0} WHERE Id = {1}", thoigianChucVu, clergy.Id);
                    }
                }
                if (nhanNhiemSos.Count > 0 && titleHistory != null)
                {
                    var ngayBoNhiem = CommonFunction.ConvertCustomDateTime(clergy.NgayBoNhiem);
                    if (ngayBoNhiem > DateTime.MinValue)
                    {

                        TimeSpan tspGianDoan = new TimeSpan();
                        if (gianDoancvs.Count > 0)
                        {
                            isGianDoanChucVu = true;
                            foreach (var gdcv in gianDoancvs)
                            {
                                tspGianDoan += (CommonFunction.ConvertCustomDateTime(gdcv.ToDate) -
                                                CommonFunction.ConvertCustomDateTime(gdcv.Fromdate));
                                gianDoanChucVuDetail += string.Format(" {0} ;", gdcv.Paper);
                            }
                        }
                        timeSpanChucVu = (DateTime.Now - ngayBoNhiem);
                        sonamChucvu = (int)((timeSpanChucVu - tspGianDoan).TotalDays / 365);
                        soThangChucVu = (int)((timeSpanChucVu - tspGianDoan).TotalDays % 365) / 30;
                    }
                }
                if (isGianDoanChucVu)
                {

                }
                //thoigianChucVu = CommonFunction.ConvertCustomDateTime(endDate); //dateTime.AddYears(-sonamChucvu).AddMonths(-soThangChucVu);
                if (sonamChucvu > 0)
                {
                    db.Database.ExecuteSqlCommand(
                        @"UPDATE Clergy 
                          SET SoNamChucVu = {0}, SoThangChucVu={1} , GianDoanCVChitiet = '{2}' WHERE Id = {3}", sonamChucvu, soThangChucVu, gianDoanChucVuDetail, clergy.Id);
                }
            }
            return Redirect("~/Home/SearchNangCao");
        }

        public ActionResult UpdateTenKhongDau()
        {
            using (CodeFirstTongLienHoiEntities tongLienHoiEntities = new CodeFirstTongLienHoiEntities())
            {
                var giaopham = tongLienHoiEntities.Clergies.ToList();
                foreach (var clergy in giaopham)
                {
                    string full = clergy.FirstName + " " + clergy.MiddleName + " " + clergy.LastName;
                    string khongdau = CommonFunction.ConvertToUnsign2(full);
                    khongdau = khongdau.Trim();
                    clergy.TenDayDuKhongDau = khongdau;
                    db.Database.ExecuteSqlCommand("UPDATE Clergy SET TenDayDuKhongDau = {0} WHERE Id = {1}", khongdau, clergy.Id);
                }
            }
            return Redirect("~/Home/toanbogiaopham");
        }

        public ActionResult ToanBoGiaoPham()
        {
            GetDefaultValue();
            return View();
        }


        #endregion


        #region TimKiemHetHan


        [GridAction]
        public ActionResult _PagingGiaoPhamHetHan(string name, DateTime fromdate, DateTime todate, int chucdanh)
        {
            return View(new Telerik.Web.Mvc.GridModel<TimKiemGiaoPhamHetHan_Result>
            {
                Data = GetGiaoPhamHetHan(name, fromdate, todate, chucdanh)
            });
        }

        private IEnumerable<TimKiemGiaoPhamHetHan_Result> GetGiaoPhamHetHan(string name, DateTime fromdate, DateTime todate, int chucdanh)
        {
            var result = db.TimKiemGiaoPhamHetHan(name, fromdate, chucdanh, todate).ToList();

            foreach (var item in result)
            {
                item.Avatar = Url.Content(string.IsNullOrWhiteSpace(item.Avatar) ? "~/Content/Content/img/placeholders/image_dark_64x64.png" : item.Avatar);
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.TenNhiemSo = item.TenNhiemSo ?? string.Empty;
            }

            return result;
        }

        public ActionResult _PagingSearchGiaoPhamNangCao(string hovaten, int kethon, int dantoc, int quequan, int nhiemso, int chucdanh, int tunamphucvu, int dennamphucvu, int cachtinh, int chucvu, DataSourceRequest request)
        {
            return Json(GetSearchGiaoPhamNangCao(hovaten, kethon, dantoc, quequan, nhiemso, chucdanh, tunamphucvu, dennamphucvu, cachtinh, chucvu).ToDataSourceResult(request));

        }

        private IEnumerable<TimKiemGiaoPhamNangCao_Result> GetSearchGiaoPhamNangCao(string hovaten, int kethon, int dantoc, int quequan, int nhiemso, int chucdanh, int tunamphucvu, int dennamphucvu, int cachtinh, int chucvu)
        {
            var result = db.TimKiemGiaoPhamNangCao(hovaten, kethon, dantoc, quequan, nhiemso, tunamphucvu, dennamphucvu, cachtinh, chucdanh, chucvu).OrderBy(a => a.ChucVuId).ToList();
            var groupResult = result.GroupBy(a => a.Id).Select(a => a.First());
            var list = new List<TimKiemGiaoPhamNangCao_Result>();

            foreach (var item in groupResult)
            {
                item.Avatar = Url.Content(string.IsNullOrWhiteSpace(item.Avatar) ? "~/Content/Content/img/placeholders/image_dark_64x64.png" : item.Avatar);
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.TenNhiemSo = item.TenNhiemSo ?? string.Empty;
                list.Add(item);
            }


            return list;
        }

        public ActionResult _PagingSearchNangCao(int tinhthanh, int chucvutu, int chucvuden, int nhiemkytu, int nhiemkyden, int namsinhtu, int namsinhden,
            int tinhtrang, int chucdanh, int chucvu, int nhiemso, int capnhiemso, int dantoc, int hocvi, string cmnd, string name, bool tt_tlh, bool bts_tlh, bool hdgp, bool nguoikinh, DataSourceRequest request)
        {
            return Json(GetSearchNangCao(tinhthanh, chucvutu, chucvuden, nhiemkytu, nhiemkyden, namsinhtu, namsinhden, tinhtrang, chucdanh, chucvu, nhiemso, capnhiemso, dantoc, hocvi, cmnd, name, tt_tlh, bts_tlh, hdgp, nguoikinh, false).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
        private IEnumerable<TimKiemNangCao_Result> GetSearchNangCao(int tinhthanh, int chucvutu, int chucvuden, int nhiemkytu, int nhiemkyden, int namsinhtu, int namsinhden,
            int tinhtrang, int chucdanh, int chucvu, int nhiemso, int capnhiemso, int dantoc, int hocvi, string cmnd, string name, bool tt_tlh, bool bts_tlh, bool hdgp, bool nguoikinh, bool isExport)
        {
            var result = db.TimKiemNangCao(tinhthanh, chucvutu, chucvuden, nhiemkytu, nhiemkyden, namsinhtu, namsinhden, tinhtrang, chucdanh, chucvu, nhiemso, capnhiemso, dantoc, hocvi, cmnd).ToList();
            var assignments = db.Clergy_AssignmentHistory.ToList();
            var groupResult = result.GroupBy(a => a.Id).Select(a => a.First());
            var list = new List<TimKiemNangCao_Result>();
            foreach (var item in groupResult)
            {
                item.Avatar = Url.Content(string.IsNullOrWhiteSpace(item.Avatar) ? "~/Content/Content/img/placeholders/image_dark_64x64.png" : item.Avatar);
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.TenChucDanh = item.TenChucDanh ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.TenNhiemSo = item.TenNhiemSo ?? string.Empty;
                var ngayBoNhiem = CommonFunction.ConvertDateTimeYearOnly(item.NgayBoNhiem);
                if(ngayBoNhiem > DateTime.MinValue)
                {
                    var timeSpanChucVu = (DateTime.Now - ngayBoNhiem);
                    var sonamChucvu = (int)(timeSpanChucVu.TotalDays / 365);
                    var soThangChucVu = (int)(timeSpanChucVu.TotalDays % 365) / 30;
                    item.NamChucVu = sonamChucvu;
                    item.ThangChucVu = soThangChucVu;
                    item.NamChucVu = item.NamChucVu ?? 0;
                }
                else
                {
                    item.NamChucVu = 0;
                    item.ThangChucVu = 0;
                }
                list.Add(item);
            }
            if (chucvutu > 0 || chucvuden > 0)
            {
                if (chucvutu > 0 && chucvuden == 0)
                {
                    list = list.Where(a => a.NamChucVu >= chucvutu).ToList();
                }
                else if (chucvutu == 0 && chucvuden > 0)
                {
                    list = list.Where(a => a.NamChucVu <= chucvuden).ToList();
                }
                else
                {
                    list = list.Where(a => a.NamChucVu <= chucvuden && a.NamChucVu >= chucvutu).ToList();
                }
                list = list.OrderByDescending(a => a.CapChiHoiId == 3).ThenByDescending(a => a.NamChucVu).ToList();

            }
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim().ToLower();
                list =
                    list.Where(a => (a.FirstName + " " + a.MiddleName + " " + a.LastName).ToLower().Trim().Contains(name) || a.TenDayDuKhongDau.ToLower().Contains(name))
                        .ToList();
            }
            //bool tt_tlh, bool bts_tlh, bool hdgp, bool nguoikinh
            if (nguoikinh)
            {
                list = list.Where(a => a.DanToc != 2).ToList();
            }
            if (tt_tlh)
            {
                list = list.Where(a => a.CurrentChurchId != 202).ToList();
            }
            var tenNhiemSo = string.Empty;
            foreach (var item in list)
            {
                var assignment = assignments.FirstOrDefault(a => item.CurrentAssignment_Id != null && a.id == item.CurrentAssignment_Id.Value);
                if (assignment != null)
                {
                    if (assignment.CapChiHoi1 != null)
                        tenNhiemSo = assignment.CapChiHoi1.Description;
                    if (assignment.Church != null)
                    {
                        item.CurrentChurchId = assignment.ChurchId;
                        tenNhiemSo = tenNhiemSo + " " + assignment.Church.ChurchName;
                    }
                    if (assignment.ChurchId.HasValue)
                        item.TenNhiemSo = isExport ? tenNhiemSo :
                            $"<a href='/NhiemSo/ChiTietNhiemSo/{assignment.ChurchId.Value}' class=''> {tenNhiemSo} </a>";
                }
                if (item.CurrentChurchId == null)
                {
                    item.CurrentChurchId = 0;
                }
                if (!string.IsNullOrWhiteSpace(item.Avatar) && System.IO.File.Exists(Request.MapPath(Url.Content(item.Avatar))))
                    item.Avatar = Url.Content(item.Avatar);
                else
                    item.Avatar = Url.Content("~/Content/Content/img/placeholders/image_dark_64x64.png");
            }
            return list.OrderByDescending(a => a.NamChucVu).ThenByDescending(a => a.ThangChucVu).ToList();
        }


        [GridAction]
        public ActionResult _PagingThoiGianPhucVu(string tukhoa, int chucvu, int chucdanh, int coquan, int tunam, int dennam, int cachtinh)
        {
            return View(new Telerik.Web.Mvc.GridModel<GiaoPhamThoiGianPhucVu_Result>
            {
                Data = GetThoiGianPhucVu(tukhoa, chucvu, chucdanh, coquan, tunam, dennam, cachtinh)
            });
        }

        private IEnumerable<GiaoPhamThoiGianPhucVu_Result> GetThoiGianPhucVu(string tukhoa, int chucvu, int chucdanh, int coquan, int tunam, int dennam, int cachtinh)
        {
            var result = db.GiaoPhamThoiGianPhucVu(tukhoa, chucvu, chucdanh, coquan, tunam, dennam, cachtinh).OrderBy(a => a.ChucVuId).Distinct().ToList();
            //remove dublicate id
            var groupResult = result.GroupBy(a => a.Id).Select(a => a.First());
            var list = new List<GiaoPhamThoiGianPhucVu_Result>();

            foreach (var item in groupResult)
            {
                item.Avatar = Url.Content(string.IsNullOrWhiteSpace(item.Avatar) ? "~/Content/Content/img/placeholders/image_dark_64x64.png" : item.Avatar);
                item.FirstName = item.FirstName ?? string.Empty;
                item.MiddleName = item.MiddleName ?? string.Empty;
                item.LastName = item.LastName ?? string.Empty;
                item.TenNhiemSo = item.TenNhiemSo ?? string.Empty;
                list.Add(item);
            }
            return list;
        }

        #endregion

        #region Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            //btnLogin
            if (Request["btnlogin"] == "login")
            {
                var username = Request["login-email"];
                var password = Request["login-password"];
                bool rememberme = Request["rememberme"] == "1";
                var user = db.Users.FirstOrDefault(a => a.username.ToLower() == username.ToLower());
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

                    ViewBag.IsViewGiaoPham =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 1) != null;
                    ViewBag.IsViewChiHoi =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 3) != null;
                    ViewBag.IsViewVanThu =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 5) != null;
                    ViewBag.IsEditGiaoPham =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 2) != null;
                    ViewBag.IsEditChiHoi =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 4) != null;
                    ViewBag.IsEditVanThu =
                        db.RoleDetails.FirstOrDefault(a => a.UserId == user.id && a.RoleMapping == 6) != null;
                    Session[Constant.SessionLogin] = user;
                    var myurl = frm["returnurl"];

                    if (!string.IsNullOrEmpty(myurl))
                        return Redirect(myurl);
                    else return Redirect("~/Home/Index");
                }
                ViewBag.Errors = erros;
                return View();
            }
            return View();
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

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection frm)
        {
            return View();
        }

        #endregion

        #region Thong tin Muc Su
        //step1
        public ActionResult MucsuThongTin(int? id)
        {
            GetDefaultValue();
            Models.Clergy clergy;
            if (id.HasValue && id.Value > 0)
                clergy =
                    db.Clergies.Include("DanToc1")
                        .Include("Address1")
                        .Include("Address")
                        .Include("Address2")
                        .FirstOrDefault(c => c.Id == id);
            else
                clergy = new Models.Clergy();
            var listDanToc = db.DanTocs.OrderBy(a => a.Name).ToList();
            if (clergy.QuanHeVoiGiaoPhams != null)
                ViewBag.VoGiaoPham = clergy.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.QuanHeGiaDinh != null && a.QuanHeGiaDinh.Id == 5);
            ViewBag.DanToc = new SelectList(listDanToc, "Id", "Name", clergy.DanToc);
            ViewBag.AllCities = db.Cities.OrderBy(a => a.Name).ToList();
            ViewBag.IDApprovedPlace = new SelectList(db.Cities.OrderBy(a => a.Name), "id", "Name",
                clergy.IDApprovedPlace);
            ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
            return View(clergy);
        }

        [HttpPost]
        public ActionResult MucsuThongTin(Models.Clergy giaopham)
        {
            var messages = new List<string>();
            var clergy = giaopham.Id > 0 ? db.Clergies.FirstOrDefault(c => c.Id == giaopham.Id) : new Clergy();
            ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
            ViewBag.AllCities = db.Cities.OrderBy(a => a.Name).ToList();
            var listDanToc = db.DanTocs.OrderBy(a => a.Name).ToList();
            string approvedId = Request["IDApprovedDate"];
            ViewBag.DanToc = new SelectList(listDanToc, "Id", "Name", clergy.DanToc);
            ViewBag.IDApprovedPlace = new SelectList(db.Cities.OrderBy(a => a.Name), "id", "Name", clergy.IDApprovedPlace);
            giaopham.IDApprovedDate = Request["IDApprovedDate"];
            if (!string.IsNullOrWhiteSpace(giaopham.BelieveDate))
                giaopham.BelieveDate = giaopham.BelieveDate.Trim();
            if (!string.IsNullOrWhiteSpace(giaopham.BaptismDate))
                giaopham.BaptismDate = giaopham.BaptismDate.Trim();
            var erros = ValidateFunction.ValidateDataGiaoPham(giaopham);
            if (clergy.QuanHeVoiGiaoPhams != null)
                ViewBag.VoGiaoPham = clergy.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.QuanHeGiaDinh != null && a.QuanHeGiaDinh.Id == 5);
            if (CommonFunction.IsValidateDateString(approvedId))
            {
                erros.Add("IDApprovedDate", "Ngày cấp CMND không hợp lệ");
            }
            ViewBag.Errors = erros;
            #region Delete picture 
            if (Request["btnDelete"] == "Xóa")
            {
                var id = clergy.Id;
                var deletObj = db.Clergies.FirstOrDefault(a => a.Id == id);
                if (deletObj != null)
                {
                    db.Clergies.Remove(deletObj);
                    db.SaveChanges();
                    return RedirectToAction("ToanBoGiaoPham", new { id = clergy.Id });
                }

            }
            #endregion
            #region delete account
            if (Request["btnDeleteImage"] == "Xóa hình")
            {
                clergy.Avatar = "";
                db.SaveChanges();
                return RedirectToAction("MucsuThongTin", new { id = clergy.Id });
            }
            #endregion
            #region Save 
            if (Request["next"] == "Lưu" && erros.Count == 0)
            {
                int dienkhac = CommonFunction.ConvertInt(Request["DienKhac"]);
                clergy.DienKhacLyDo = giaopham.DienKhacLyDo;
                clergy.DienKhacNgay = giaopham.DienKhacNgay;
                clergy.FirstName = !string.IsNullOrEmpty(giaopham.FirstName) ? giaopham.FirstName.Trim() : "";
                clergy.LastName = !string.IsNullOrEmpty(giaopham.LastName) ? giaopham.LastName.Trim() : "";
                clergy.MiddleName = !string.IsNullOrEmpty(giaopham.MiddleName) ? giaopham.MiddleName.Trim() : "";
                clergy.NickName = giaopham.NickName;
                clergy.NoiSinh = giaopham.NoiSinh;
                clergy.Gender = giaopham.Gender;
                if (!string.IsNullOrEmpty(giaopham.DateOfBirth))
                    clergy.DateOfBirth = giaopham.DateOfBirth;
                clergy.PlaceOfBirth = giaopham.PlaceOfBirth;
                clergy.DanToc = giaopham.DanToc;
                clergy.IDApprovedPlace = giaopham.IDApprovedPlace;
                clergy.BelieveDate = giaopham.BelieveDate;
                clergy.BaptismDate = giaopham.BaptismDate;
                clergy.IdentityNumber = giaopham.IdentityNumber;
                clergy.NgayBoNhiem = giaopham.NgayBoNhiem;
                //

                if (giaopham.IsVeVoiChua.HasValue && giaopham.IsVeVoiChua.Value)
                {
                    var dateofDeath = giaopham.DateOfDeath;
                    if (string.IsNullOrEmpty(dateofDeath))
                    {
                        clergy.DateOfDeath = "";
                    }
                    else
                    {
                        clergy.DateOfDeath = dateofDeath;
                    }
                    clergy.IsVeVoiChua = giaopham.IsVeVoiChua;

                }
                else
                {
                    clergy.IsVeVoiChua = giaopham.IsVeVoiChua;
                    clergy.DateOfDeath = "";
                }
                if (!CommonFunction.IsValidateDateString(approvedId))
                    clergy.IDApprovedDate = giaopham.IDApprovedDate;
                clergy.Phone = giaopham.Phone;
                clergy.MobilePhone = giaopham.MobilePhone;
                clergy.Email = giaopham.Email;

                clergy.BaptismYear = giaopham.BaptismYear;
                clergy.BelieveYear = giaopham.BelieveYear;
                var titleText = string.Empty;
                if (giaopham.TitleId > 0)
                {
                    clergy.TitleId = giaopham.TitleId;
                    var title = db.ClergyTitles.FirstOrDefault(a => a.Id == giaopham.TitleId);
                    if (title != null)
                    {
                        if (title.Title.ToLower().Contains("quả"))
                            dienkhac = 5;
                        else if ((title.Title.ToLower().Contains("trí")))
                            dienkhac = 4;

                    }

                }

                if (dienkhac == 0)
                {
                    clergy.DuHoc = false;
                    clergy.DinhCuNuocNgoai = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = false;
                    clergy.QuaPhu = false;
                    clergy.NghiChucVu = false;
                }
                //duhoc
                else if (dienkhac == 1)
                {
                    clergy.DuHoc = true;
                    clergy.DinhCuNuocNgoai = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = false;
                    clergy.QuaPhu = false;
                    clergy.NghiChucVu = false;
                }
                else if (dienkhac == 2)
                {
                    clergy.DinhCuNuocNgoai = true;
                    clergy.DuHoc = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = false;
                    clergy.QuaPhu = false;
                    clergy.NghiChucVu = false;
                }
                else if (dienkhac == 3)
                {
                    clergy.DinhCuNuocNgoai = false;
                    clergy.DuHoc = false;
                    clergy.KyLuat = true;
                    clergy.HuuTri = false;
                    clergy.QuaPhu = false;
                    clergy.NghiChucVu = false;
                }
                else if (dienkhac == 4)
                {
                    clergy.DinhCuNuocNgoai = false;
                    clergy.DuHoc = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = true;
                    clergy.QuaPhu = false;
                    clergy.NghiChucVu = false;
                }
                else if (dienkhac == 5)
                {
                    clergy.DinhCuNuocNgoai = false;
                    clergy.DuHoc = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = false;
                    clergy.NghiChucVu = false;
                    clergy.QuaPhu = true;
                }
                else if (dienkhac == 6)
                {
                    clergy.DinhCuNuocNgoai = false;
                    clergy.DuHoc = false;
                    clergy.KyLuat = false;
                    clergy.HuuTri = false;
                    clergy.NghiChucVu = true;
                    clergy.QuaPhu = false;
                }

               



                string full = clergy.FirstName + " " + clergy.MiddleName + " " + clergy.LastName;
                full = full.Trim();
                string khongdau = CommonFunction.ConvertToUnsign2(full);
                clergy.TenDayDuKhongDau = khongdau;

                #region Save File Path

                //save upload image with specific format
                var fileRequest = string.Format("hinhdaidien");
                var file = Request.Files[fileRequest];
                var importPath = Path.Combine(Constant.Document_Avatar,
                    giaopham.Id.ToString(CultureInfo.InvariantCulture));

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
                    clergy.Avatar = systemLocation;
                }

                #endregion

                #region Address

                if (!clergy.BirthAddressId.HasValue || clergy.BirthAddressId.Value == 0)
                {
                    var add = new Address
                    {
                        Street = Request["Address2.Street"],

                    };
                    if (CommonFunction.ConvertInt(Request["Address2.CityId"]) > 0)
                        add.CityId = CommonFunction.ConvertInt(Request["Address2.CityId"]);
                    else
                        add.CityId = null;
                    db.Addresses.Add(add);
                    db.SaveChanges();
                    clergy.BirthAddressId = add.Id;
                }
                else
                {
                    var add = db.Addresses.FirstOrDefault(a => a.Id == clergy.BirthAddressId);
                    if (add != null)
                    {
                        add.Street = Request["Address2.Street"];
                        if (CommonFunction.ConvertInt(Request["Address2.CityId"]) > 0)
                            add.CityId = CommonFunction.ConvertInt(Request["Address2.CityId"]);
                        else
                            add.CityId = null;
                        db.SaveChanges();
                        clergy.BirthAddressId = add.Id;
                    }
                }

               

                if (!clergy.PermanentAddressId.HasValue || clergy.PermanentAddressId.Value == 0)
                {
                    var add = new Address
                    {
                        Street = Request["Address.Street"],
                    };
                    if (CommonFunction.ConvertInt(Request["Address.CityId"]) > 0)
                        add.CityId = CommonFunction.ConvertInt(Request["Address.CityId"]);
                    else
                        add.CityId = null;
                    db.Addresses.Add(add);
                    db.SaveChanges();
                    clergy.PermanentAddressId = add.Id;
                }
                else
                {
                    var add = db.Addresses.FirstOrDefault(a => a.Id == clergy.PermanentAddressId);
                    if (add != null)
                    {
                        add.Street = Request["Address.Street"];
                        if (CommonFunction.ConvertInt(Request["Address.CityId"]) > 0)
                            add.CityId = CommonFunction.ConvertInt(Request["Address.CityId"]);
                        else
                            add.CityId = null;
                        db.SaveChanges();
                        clergy.PermanentAddressId = add.Id;
                    }
                }
                if (!clergy.ContactAddressId.HasValue || clergy.ContactAddressId.Value == 0)
                {
                    var add = new Address
                    {
                        Street = Request["Address3.Street"],
                    };
                    if (CommonFunction.ConvertInt(Request["Address3.CityId"]) > 0)
                        add.CityId = CommonFunction.ConvertInt(Request["Address3.CityId"]);
                    else
                        add.CityId = null;
                    db.Addresses.Add(add);
                    db.SaveChanges();
                    clergy.ContactAddressId = add.Id;
                }
                else
                {
                    Address add = db.Addresses.FirstOrDefault(a => a.Id == clergy.ContactAddressId);
                    if (add != null)
                    {
                        add.Street = Request["Address3.Street"];
                        if (CommonFunction.ConvertInt(Request["Address3.CityId"]) > 0)
                            add.CityId = CommonFunction.ConvertInt(Request["Address3.CityId"]);
                        else
                            add.CityId = null;
                        db.SaveChanges();
                        clergy.ContactAddressId = add.Id;
                    }

                }
                if (!clergy.CurrentAddressId.HasValue || clergy.CurrentAddressId.Value == 0)
                {
                    var add = new Address
                    {
                        Street = Request["Address1.Street"],
                    };
                    if (CommonFunction.ConvertInt(Request["Address1.CityId"]) > 0)
                        add.CityId = CommonFunction.ConvertInt(Request["Address1.CityId"]);
                    else
                        add.CityId = null;
                    db.Addresses.Add(add);
                    db.SaveChanges();
                    clergy.CurrentAddressId = add.Id;
                }
                else
                {
                    Address add = db.Addresses.FirstOrDefault(a => a.Id == clergy.CurrentAddressId);
                    if (add != null)
                    {
                        add.Street = Request["Address1.Street"];
                        if (CommonFunction.ConvertInt(Request["Address1.CityId"]) > 0)
                            add.CityId = CommonFunction.ConvertInt(Request["Address1.CityId"]);
                        else
                            add.CityId = null;
                        db.SaveChanges();
                        clergy.CurrentAddressId = add.Id;
                    }

                }
                #endregion

                if (clergy.Id == 0)
                {
                    db.Clergies.Add(clergy);
                    messages.Add("Thêm mới thành công.");
                }
                else
                {
                    messages.Add("Chỉnh sửa thành công.");
                }
                db.SaveChanges();

                if (giaopham.ChucVuId > 0)
                {
                    clergy.ChucVuId = giaopham.ChucVuId;
                    var chucvu = db.ChucVus.FirstOrDefault(a => a.id == giaopham.ChucVuId.Value);
                    if (chucvu != null && chucvu.Name.Contains("Chờ Nhiệm sở"))
                    {
                        var currentAssigment = new Clergy_AssignmentHistory();
                        currentAssigment.ChucVu = chucvu;
                        currentAssigment.ClergyId = clergy.Id;
                        currentAssigment.ChurchId = null;
                        db.Clergy_AssignmentHistory.Add(currentAssigment);
                        clergy.CurrentAssignment_Id = currentAssigment.id;
                    }
                }
                else clergy.ChucVuId = null;

                ViewBag.Message = messages;
                GetDefaultValue();
                return View(clergy);
            }
            #endregion
            GetDefaultValue();
            return View(giaopham);
        }


        //step3
        public ActionResult MucsuThongTinChucVu(int? id, string request)
        {
            GetDefaultValue();
            Clergy clergy = db.Clergies.FirstOrDefault(c => c.Id == id);
            //SetEducation(clergy.Clergy_Education);
            ViewBag.GiaoPham = clergy;
            var viewAssign = db.v_assign.Where(a => a.ClergyId == clergy.Id).OrderByDescending(a => a.id).ToList();
            ViewBag.Assign = viewAssign;
            if (clergy != null && request == "addnhiemso")
            {

                if (clergy.Clergy_AssignmentHistory == null)
                {
                    clergy.Clergy_AssignmentHistory = new EntityCollection<Clergy_AssignmentHistory>
                                                          {new Clergy_AssignmentHistory()};
                }
                else
                {
                    clergy.Clergy_AssignmentHistory.Add(new Clergy_AssignmentHistory());
                }
            }
            if (clergy != null && request == "addmore")
            {
                //luu gia tri hien tai va add them moi
                if (clergy.Clergy_TitleHistory == null)
                {
                    clergy.Clergy_TitleHistory = new EntityCollection<Clergy_TitleHistory> { new Clergy_TitleHistory() };
                }
                else
                {
                    clergy.Clergy_TitleHistory.Add(new Clergy_TitleHistory());
                }

            }
            return View(clergy);
        }


        [HttpPost]
        public ActionResult MucsuThongTinChucVu(Clergy giaopham)
        {

            Clergy clergy = db.Clergies.FirstOrDefault(c => c.Id == giaopham.Id);
            //SetEducation(clergy.Clergy_Education);
            ViewBag.GiaoPham = clergy;
            ViewBag.Assign = db.v_assign.Where(a => a.ClergyId == clergy.Id).ToList();
            ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
            if (Request["btnAddMore"] == "Thêm công nhận chức danh")
            {
                return RedirectToAction("MucsuThongTinChucVu", new { id = giaopham.Id, request = "addmore" });
            }
            if (Request["btnThemNhiemSo"] == "Thêm nhiệm sở")
            {
                return RedirectToAction("MucsuThongTinChucVu", new { id = giaopham.Id, request = "addnhiemso" });
            }
            #region Save
            if (Request["btnnext"] == "Lưu")
            {
                var tittleCount = CommonFunction.GetValueBySplitString(Request["title.id"]);
                var EffectiveDates = CommonFunction.GetValueBySplitString(Request["title.EffectiveDate"]);
                var ClergyTitles = CommonFunction.GetValueBySplitString(Request["title.ClergyTitle.Title"]);
                for (int i = 0; i < tittleCount.Length; i++)
                {
                    int degreeId = CommonFunction.ConvertInt(tittleCount[i]);
                    var chucdanhGiaoPham = db.Clergy_TitleHistory.FirstOrDefault(a => a.id == degreeId) ??
                                           new Clergy_TitleHistory();
                    var fileRequest = string.Format("chungchi.{0}", degreeId);
                    var file = Request.Files[fileRequest];
                    var importPath = Path.Combine(Constant.Document_import,
                                                  giaopham.Id.ToString(CultureInfo.InvariantCulture));
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
                        var fileExtension = Path.GetExtension(systemLocation);
                        var fileType = MimeAssistant.GetMimeType(fileExtension);
                        var fileInfo = new FileInfo(Request.MapPath(systemLocation));
                        chucdanhGiaoPham.RequestPaper = systemLocation;
                    }
                    chucdanhGiaoPham.EffectiveDate = EffectiveDates[i];
                    if (chucdanhGiaoPham.id == 0)
                    {
                        db.Clergy_TitleHistory.Add(chucdanhGiaoPham);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("MucsuThongTinChucVu", new { id = giaopham.Id });
            }
            #endregion
            GetDefaultValue();
            return View(clergy);
        }

        //step4
        public ActionResult MucsuThongTinGiaDinh(int? id, string request, int? dataid)
        {
            GetDefaultValue();
            var messages = new List<string>();
            if (request == "delete" && dataid > 0)
            {
                var edu = db.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.Id == dataid);
                if (edu != null)
                {
                    db.QuanHeVoiGiaoPhams.Remove(edu);
                    db.SaveChanges();
                    messages.Add("Xóa thành công");
                }
            }
            if (request == "addnewsuccess")
            {
                messages.Add("Thêm mới thành công");
            }
            else if (request == "updatesuccess")
            {
                messages.Add("Cập nhật thành công");
            }
            var clergy = db.Clergies.FirstOrDefault(c => c.Id == id);

            ViewBag.GiaoPham = clergy;
            ViewBag.GiaoPhamId = id;

            var list = new List<QuanHeGiaDinhDBO>();
            if (clergy != null)
            {
                ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
                if (clergy.QuanHeVoiGiaoPhams != null)
                    ViewBag.VoGiaoPham = clergy.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.QuanHeGiaDinh != null && a.QuanHeGiaDinh.Id == 5);
                list.AddRange(clergy.QuanHeVoiGiaoPhams.Select(quanHeVoiGiaoPham => new QuanHeGiaDinhDBO(quanHeVoiGiaoPham)));
            }
            ViewBag.Message = messages;
            list = list.OrderBy(a => a.NamSinh).ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult MucsuThongTinGiaDinh(List<QuanHeGiaDinhDBO> qhgd)
        {

            var giaophamId = CommonFunction.ConvertInt(Request["GiaoPhamId"]);
            var messages = new List<string>();
            ViewBag.GiaoPhamId = giaophamId;
            var clergy = db.Clergies.FirstOrDefault(a => a.Id == giaophamId);
            ViewBag.GiaoPham = clergy;
            var updateList = new List<QuanHeGiaDinhDBO>();
            ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
            if (clergy.QuanHeVoiGiaoPhams != null)
                ViewBag.VoGiaoPham = clergy.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.QuanHeGiaDinh != null && a.QuanHeGiaDinh.Id == 5);
            if (Request["btnAddMore"] == "thêm")
            {
                if (clergy != null)
                {
                    var list = db.QuanHeVoiGiaoPhams.Where(a => a.ClergyId == clergy.Id).ToList();
                    updateList = list.Select(Mapper.DynamicMap<QuanHeVoiGiaoPham, QuanHeGiaDinhDBO>).ToList();
                    var list1 = updateList.Where(a => a.Id == 0).ToList();
                    if (list1.Count == 0)
                        updateList.Add(new QuanHeGiaDinhDBO { GiaoPhamId = clergy.Id });
                }
            }
            if (Request["cancel"] == "Hủy")
            {
                var list = db.QuanHeVoiGiaoPhams.Where(a => a.ClergyId == clergy.Id).ToList();
                updateList = list.Select(Mapper.DynamicMap<QuanHeVoiGiaoPham, QuanHeGiaDinhDBO>).ToList();
                return RedirectToAction("MucsuThongTinGiaDinh", new { id = clergy.Id });

            }
            if (Request["next"] == "Lưu")
            {
                var erros = ValidateFunction.ValidateGiaDinhGiaoPham(qhgd);
                var isAddNew = false;
                if (erros.Count == 0)
                {
                    foreach (var quanHeGiaDinhDbo in qhgd)
                    {
                        var qhgdId = quanHeGiaDinhDbo.Id;
                        var quanhe = db.QuanHeVoiGiaoPhams.FirstOrDefault(a => a.Id == qhgdId) ??
                                     new QuanHeVoiGiaoPham();
                        quanhe.HoTen = quanHeGiaDinhDbo.HoTen;
                        quanhe.YearOfBirth = quanHeGiaDinhDbo.NamSinh;
                        quanhe.NgheNghiep = quanHeGiaDinhDbo.NgheNghiep;
                        quanhe.Description = quanHeGiaDinhDbo.Description;
                        quanhe.RelationShipId = quanHeGiaDinhDbo.RelationShipId;
                        quanhe.ClergyId = quanHeGiaDinhDbo.GiaoPhamId > 0 ? quanHeGiaDinhDbo.GiaoPhamId : giaophamId;
                        if (quanhe.Id == 0)
                        {
                            db.QuanHeVoiGiaoPhams.Add(quanhe);
                            isAddNew = true;
                            messages.Add("Thêm mới thành công");
                        }
                        quanHeGiaDinhDbo.Id = quanhe.Id;
                        db.SaveChanges();
                    }
                    return RedirectToAction("MucsuThongTinGiaDinh", isAddNew ? new { id = clergy.Id, request = "addnewsuccess" } : new { id = clergy.Id, request = "updatesuccess" });
                }
                updateList = qhgd;
                ViewBag.Errors = erros;
                ViewBag.Message = messages;
            }
            updateList = updateList.OrderBy(a => a.NamSinh).ToList();
            GetDefaultValue();
            return View(updateList);
        }

        public ActionResult MucsuDetail(int? id)
        {
            GetDefaultValue();
            var clergy = db.Clergies.FirstOrDefault(c => c.Id == id);
            var ngayBoNhiem = CommonFunction.ConvertDateTimeYearOnly(clergy.NgayBoNhiem);
            if (ngayBoNhiem > DateTime.MinValue)
            {
                var timeSpanChucVu = (DateTime.Now - ngayBoNhiem);
                var sonamChucvu = (int)(timeSpanChucVu.TotalDays / 365);
                var soThangChucVu = (int)(timeSpanChucVu.TotalDays % 365) / 30;
                clergy.SoNamChucVu = sonamChucvu;
                clergy.SoThangChucVu = soThangChucVu;
                clergy.SoNamChucVu = clergy.SoNamChucVu ?? 0;
            }
            else
            {
                clergy.SoNamChucVu = 0;
                clergy.SoThangChucVu = 0;
            }
            db.SaveChanges();
            ViewBag.ChucDanhMucSu = clergy.ClergyTitle != null ? clergy.ClergyTitle.Title : "";
            return View(clergy);
        }

        public ActionResult MucSuDienKhac(int? titleid, int? cityId)
        {
            GetDefaultValue();
            ViewData["chucdanh"] = titleid;
            return View();
        }

        public ActionResult MucSu(int? titleid, int? cityId)
        {
            GetDefaultValue();
            ViewData["chucdanh"] = titleid;
            return View();
        }

        [HttpPost]
        public ActionResult MucSu(FormCollection frm)
        {
            var chucvu = frm["chucdanh"];

            ViewData["chucdanh"] = chucvu;
            GetDefaultValue();

            return View();
        }
        #endregion

        #region Document

        public FileResult Document(int? id, string sheet)
        {
            if (id > 0 && !string.IsNullOrWhiteSpace(sheet))
            {
                var document = db.Clergy_Education.FirstOrDefault(a => a.Id == id);
                if (document != null && System.IO.File.Exists(Request.MapPath(document.Paper)))
                {
                    string contentType = "application/pdf";
                    return File(document.Paper, contentType, sheet);
                }
            }
            else
            {
                var path = Path.Combine(Constant.Document_import, sheet);
                if (System.IO.File.Exists(path))
                {
                    var extension = Path.GetExtension(path);
                    string contentType = MimeAssistant.GetMimeType(extension);
                    return File(path, contentType, sheet);
                }
            }
            return null;
        }

        public FileResult DocumentAssignment(int? id, string sheet)
        {
            if (id > 0 && !string.IsNullOrWhiteSpace(sheet))
            {
                var document = db.Clergy_AssignmentHistory.FirstOrDefault(a => a.id == id);
                if (document != null && System.IO.File.Exists(Request.MapPath(document.Paper)))
                {
                    string contentType = "application/pdf";
                    return File(document.Paper, contentType, sheet);
                }
            }
            else
            {
                var path = Path.Combine(Constant.Document_import, sheet);
                if (System.IO.File.Exists(path))
                {
                    var extension = Path.GetExtension(path);
                    string contentType = MimeAssistant.GetMimeType(extension);
                    return File(path, contentType, sheet);
                }
            }
            return null;
        }

        public FileResult Assignment(int? id, string sheet)
        {
            if (id > 0 && !string.IsNullOrWhiteSpace(sheet))
            {
                var document = db.Clergy_TitleHistory.FirstOrDefault(a => a.id == id);
                if (document != null && System.IO.File.Exists(Request.MapPath(document.RequestPaper)))
                {
                    string contentType = "application/pdf";
                    return File(document.RequestPaper, contentType, sheet);
                }
            }
            else
            {
                var path = Path.Combine(Constant.Document_import, sheet);
                if (System.IO.File.Exists(path))
                {
                    var extension = Path.GetExtension(path);
                    string contentType = MimeAssistant.GetMimeType(extension);
                    return File(path, contentType, sheet);
                }
            }
            return null;
        }

        #endregion

        #region Import Data
        public ActionResult ImportData()
        {
            GetDefaultValue();
            return View();
        }

        //[HttpPost]
        //public ActionResult ImportData(FormCollection frm)
        //{
        //    GetDefaultValue();
        //    var fileUpload = Request.Files["fileUpload"];
        //    if (fileUpload != null && fileUpload.FileName.Length > 0)
        //    {
        //        string path = Path.GetFileName(fileUpload.FileName);
        //        using (var stream = fileUpload.InputStream)
        //        {
        //            var file = Path.GetExtension(fileUpload.FileName);
        //            IExcelDataReader reader = null;
        //            switch (file)
        //            {
        //                case ".xls":
        //                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //                    break;
        //                case ".xlsx":
        //                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //                    break;
        //            }
        //            if (reader == null)
        //                return null;
        //            reader.IsFirstRowAsColumnNames = false;
        //            var ds = reader.AsDataSet();
        //            if (ds != null)
        //            {
        //                var chucdanhs = db.ClergyTitles.ToList();
        //                var dttable = ds.Tables[0];
        //                var Cities = db.Cities.ToList();
        //                foreach (DataRow row in dttable.Rows)
        //                {
        //                    var itemArrays = row.ItemArray;
        //                    var id = CommonFunction.ConvertInt(itemArrays[0].ToString());
        //                    if (id > 0)
        //                    {
        //                        var Ma_chucdanh = itemArrays[1] as string;
        //                        var firstname = itemArrays[2] as string;
        //                        var lastname = itemArrays[3] as string;
        //                        var Ten_chucvu = itemArrays[4] as string;
        //                        DateTime ngaysinh = CommonFunction.ConvertDateTime(itemArrays[5].ToString());
        //                        int namsinh = CommonFunction.ConvertInt(itemArrays[6].ToString());
        //                        string giaovulenh = itemArrays[7] as string;
        //                        string ngaycqThuan = itemArrays[8] as string;
        //                        string ngayBoNhiem = itemArrays[9].ToString();
        //                        int nhiemkyThang = CommonFunction.ConvertInt(itemArrays[10].ToString());
        //                        string ngaymanhiem = itemArrays[11].ToString();
        //                        string capchihoi = itemArrays[12] as string;
        //                        string tennhiemso = itemArrays[13] as string;
        //                        string diachinhiemso = itemArrays[14] as string;
        //                        string tinhthanh = itemArrays[15] as string;
        //                        string cmnd = itemArrays[16] as string;
        //                        string capngay = itemArrays[17] as string;
        //                        string nguyenquan = itemArrays[18] as string;
        //                        int masotinhthanh = CommonFunction.ConvertInt(itemArrays[20].ToString());
        //                        string khoa = itemArrays[21] as string;
        //                        string congnhantd = itemArrays[22] as string;
        //                        string cqCongnhantd = itemArrays[23] as string;
        //                        string congnhanMsnc = itemArrays[24] as string;
        //                        string cqcongnhanMsnc = itemArrays[25] as string;
        //                        string tanphongMucsu = itemArrays[26] as string;
        //                        string kygvlNhiemKy1 = itemArrays[27] as string;
        //                        string kygvlNhiemKy2 = itemArrays[28] as string;
        //                        string kygvlNhiemKy3 = itemArrays[29] as string;
        //                        string ghichu = itemArrays[30] as string;
        //                        Church nhiemso = null;
        //                        if (!string.IsNullOrWhiteSpace(tennhiemso))
        //                        {
        //                            nhiemso = new Church {ChurchName = tennhiemso, AddressFull = diachinhiemso};
        //                            var _capchihoi = db.CapChiHois.FirstOrDefault(a => a.Name == capchihoi);
        //                            if (_capchihoi != null)
        //                                nhiemso.CapChiHoiID = _capchihoi.id;
        //                            var city = Cities.FirstOrDefault(a => a.Name == tinhthanh);
        //                            if (city != null)
        //                                nhiemso.CitiId = city.id;
        //                            if (db.Churches.FirstOrDefault(a => a.ChurchName == nhiemso.ChurchName) == null)
        //                            {
        //                                db.Churches.Add(nhiemso);
        //                                db.SaveChanges();
        //                            }
        //                        }

        //                        var mucsu = new Clergy
        //                                        {
        //                                            FirstName = firstname,
        //                                            LastName = lastname,
        //                                            Id = id,
        //                                            IdentityNumber = cmnd,
        //                                            PlaceOfBirth = nguyenquan,
        //                                            TinhThanhId = masotinhthanh
        //                                        };

        //                        var mucsucapchihoi = db.CapChiHois.FirstOrDefault(a => a.Name == capchihoi);
        //                        if (mucsucapchihoi != null)
        //                            mucsu.CapChiHoiId = mucsucapchihoi.id;
        //                        mucsu.DiaChiNhiemSo = diachinhiemso;
        //                        mucsu.TenNhiemSo = tennhiemso;
        //                        mucsu.TinhThanhId = masotinhthanh;
        //                        if (nhiemso != null && nhiemso.id > 0 && !string.IsNullOrWhiteSpace(diachinhiemso))
        //                        {
        //                            mucsu.CurrentChurchId = nhiemso.id;
        //                        }
        //                        mucsu.NoicapNgayCap = capngay;
        //                        var chucdanhByShort = db.ClergyTitles.FirstOrDefault(a => a.Short == Ma_chucdanh);
        //                        if (chucdanhByShort != null)
        //                            mucsu.TitleId = chucdanhByShort.Id;
        //                        var chucvu = db.ChucVus.FirstOrDefault(a => a.Name == Ten_chucvu);
        //                        if (chucvu != null)
        //                            mucsu.ChucVuId = chucvu.id;
        //                        db.Clergies.Add(mucsu);
        //                        db.SaveChanges();
        //                        if (nhiemso != null && nhiemkyThang > 0)
        //                        {
        //                            var assign = new Clergy_AssignmentHistory
        //                                             {
        //                                                 ClergyId = mucsu.Id,

        //                                                 Term = nhiemkyThang,
        //                                                 Paper = giaovulenh + " " + ngaycqThuan,
        //                                                 Role = ""
        //                                             };
        //                            if (nhiemso.id > 0)
        //                                assign.ChurchId = nhiemso.id;

        //                            assign.StartDate = ngayBoNhiem;
        //                            assign.EndDate = ngaymanhiem;
        //                            db.Clergy_AssignmentHistory.Add(assign);
        //                            db.SaveChanges();
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(khoa))
        //                        {
        //                            var edu = new Clergy_Education {CollegeOrProgramme = khoa, ClergyId = mucsu.Id};
        //                            db.Clergy_Education.Add(edu);
        //                            db.SaveChanges();
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(tanphongMucsu))
        //                        {
        //                            var tanphong = new Clergy_TitleHistory();
        //                            tanphong.TitleId = db.ClergyTitles.FirstOrDefault(a => a.Short == "MS").Id;
        //                            if (CommonFunction.ConvertDateTime(tanphongMucsu) > DateTime.MinValue)
        //                                tanphong.EffectiveDate = tanphongMucsu;
        //                            tanphong.CleargyId = mucsu.Id;
        //                            db.Clergy_TitleHistory.Add(tanphong);
        //                            db.SaveChanges();
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(congnhantd))
        //                        {
        //                            var tanphong = new Clergy_TitleHistory();
        //                            tanphong.TitleId = db.ClergyTitles.FirstOrDefault(a => a.Short == "TĐ").Id;
        //                            tanphong.CleargyId = mucsu.Id;
        //                            tanphong.ApprovalPaper = congnhantd;
        //                            tanphong.RequestPaper = cqCongnhantd;
        //                            db.Clergy_TitleHistory.Add(tanphong);
        //                            db.SaveChanges();
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(congnhanMsnc))
        //                        {
        //                            var tanphong = new Clergy_TitleHistory();
        //                            tanphong.TitleId = db.ClergyTitles.FirstOrDefault(a => a.Short == "MSNC").Id;
        //                            tanphong.CleargyId = mucsu.Id;
        //                            tanphong.ApprovalPaper = congnhanMsnc;
        //                            db.Clergy_TitleHistory.Add(tanphong);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return View();
        //}

        private IList<string> GetTablenames(DataTableCollection tables)
        {
            var tableList = new List<string>();
            foreach (var table in tables)
            {
                tableList.Add(table.ToString());
            }

            return tableList;
        }

        #endregion



    }
}