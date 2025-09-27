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
using Path = System.IO.Path;
using CrystalDecisions.Shared;


namespace HTTLVN.QLTLH.Controllers
{
    public class VanThuController : BaseController
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        HtmlViewRenderer _htmlViewRenderer = new HtmlViewRenderer();
        StandardPdfRenderer _standardPdfRenderer = new StandardPdfRenderer();

        public VanThuController()
        {
            
        }

        
        //
        // GET: /VanThu/
        // liệt kê danh sách văn thư và xuất report ra excel hoặc pdf
        [ValidateInput(false)]
        public ActionResult Index(int? query)
        {
            GetDefaultValue();
            string loaivanthu = "0";
            if (string.IsNullOrWhiteSpace(Request["giaopham-page"]) && query > 0)
            {
                ViewBag.LoaiVanThu = query.ToString();
                loaivanthu = query.ToString();
            }
            
            const string tungay = "";
            const string dengay = "";
            const string nguoigui = "";
            const string nguoinhan = "";
            const string veviec = "";
            const string trangthai = "both";
            ViewBag.LoaiVanThu = loaivanthu;
            var model = GetOrders(loaivanthu, tungay, dengay, nguoigui, nguoinhan, veviec, trangthai);
            ViewData["orders"] = model;
            return View();
        }

        public ActionResult _Paging(string loaivanthu, string tungay, string dengay, string nguoigui, string nguoinhan, string veviec, string trangthai, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetOrders(loaivanthu, tungay, dengay, nguoigui, nguoinhan,veviec,trangthai).ToDataSourceResult(request));
        }

        private IEnumerable<v_vanThu> GetOrders(string loaivanthu, string tungay, string dengay, string nguoigui, string nguoinhan, string veviec, string trangthai)
        {
            var list = db.v_vanThu.ToList();
            if (!string.IsNullOrWhiteSpace(nguoigui))
                list = list.Where(a => a.NguoiGui != null && a.NguoiGui.ToLower().Contains(nguoigui.Trim().ToLower())).ToList();
            if (!string.IsNullOrWhiteSpace(nguoinhan))
                list = list.Where(a => a.NguoiNhan != null && a.NguoiNhan.ToLower().Contains(nguoinhan.Trim().ToLower())).ToList();
            if (!string.IsNullOrWhiteSpace(veviec))
                list = list.Where(a => a.TieuDe != null && a.TieuDe.ToLower().Contains(veviec.Trim().ToLower())).ToList();
            if (loaivanthu != "0")
                list = list.Where(a => a.LoaiVanThu == loaivanthu).ToList();
            if (trangthai == "yes")
                list = list.Where(a => a.TrangThaiPheDuyet.HasValue && a.TrangThaiPheDuyet.Value).ToList();
            else if (trangthai == "no")
                list = list.Where(a => a.TrangThaiPheDuyet.HasValue && !a.TrangThaiPheDuyet.Value || !a.TrangThaiPheDuyet.HasValue).ToList();
            //từ ngày đến ngày ngày/tháng/năm
            if (!string.IsNullOrWhiteSpace(tungay) && string.IsNullOrWhiteSpace(dengay))
            {
                var tungayType = CommonFunction.GetDateType(tungay);
                switch (tungayType)
                {
                    case DateType.Year:
                        {
                            var from = CommonFunction.ConvertInt(tungay);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value.Year >= from)
                                    .ToList();
                        }
                        break;
                    case DateType.FulDate:
                        {
                            var from = CommonFunction.ConvertDateTime(tungay);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from)
                                    .ToList();
                        }
                        break;
                    case DateType.MonthYear:
                        {
                            var from = CommonFunction.ConvertCustomDateTime(tungay, true);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from)
                                    .ToList();
                        }
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(dengay) && string.IsNullOrWhiteSpace(tungay))
            {
                var denngayType = CommonFunction.GetDateType(dengay);
                switch (denngayType)
                {
                    case DateType.Year:
                        {
                            var to = CommonFunction.ConvertInt(dengay);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value.Year <= to)
                                    .ToList();
                        }
                        break;
                    case DateType.FulDate:
                        {
                            var to = CommonFunction.ConvertDateTime(dengay);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value <= to)
                                    .ToList();
                        }
                        break;
                    case DateType.MonthYear:
                        {
                            var to = CommonFunction.ConvertCustomDateTime(dengay, false);
                            list =
                                list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu <= to)
                                    .ToList();
                        }
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(tungay) && !string.IsNullOrWhiteSpace(dengay))
            {
                var tungayType = CommonFunction.GetDateType(tungay);
                var denngayType = CommonFunction.GetDateType(dengay);
                if (tungayType == DateType.Year && denngayType == DateType.Year)
                {
                    var from = CommonFunction.ConvertInt(tungay);
                    var to = CommonFunction.ConvertInt(dengay);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value.Year >= from && a.NgayVanThu.Value.Year <= to)
                            .ToList();
                }
                else if (tungayType == DateType.FulDate && denngayType == DateType.FulDate)
                {
                    var from = CommonFunction.ConvertDateTime(tungay);
                    var to = CommonFunction.ConvertDateTime(dengay);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value <= to)
                            .ToList();
                }
                else if (tungayType == DateType.MonthYear && denngayType == DateType.MonthYear)
                {
                    var from = CommonFunction.ConvertCustomDateTime(tungay, true);
                    var to = CommonFunction.ConvertCustomDateTime(dengay, false);
                    list =
                        list.Where(
                            a =>
                                a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value <= to)
                            .ToList();
                }
                else if (tungayType == DateType.FulDate && denngayType == DateType.Year)
                {
                    var from = CommonFunction.ConvertDateTime(tungay);
                    var to = CommonFunction.ConvertInt(dengay);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value.Year <= to)
                            .ToList();
                }
                else if (tungayType == DateType.FulDate && denngayType == DateType.MonthYear)
                {
                    var from = CommonFunction.ConvertDateTime(tungay);
                    var to = CommonFunction.ConvertDateTime(dengay, false);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value <= to)
                            .ToList();
                }
                else if (denngayType == DateType.FulDate && tungayType == DateType.Year)
                {
                    var from = CommonFunction.ConvertDateTime(dengay);
                    var to = CommonFunction.ConvertInt(tungay);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value.Year <= to)
                            .ToList();
                }
                else if (denngayType == DateType.FulDate && tungayType == DateType.MonthYear)
                {
                    var from = CommonFunction.ConvertDateTime(dengay, true);
                    var to = CommonFunction.ConvertDateTime(tungay);
                    list =
                        list.Where(a => a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value <= to)
                            .ToList();
                }
                else if (tungayType == DateType.MonthYear && denngayType == DateType.Year)
                {
                    var from = CommonFunction.ConvertCustomDateTime(tungay, true);
                    var to = CommonFunction.ConvertInt(dengay);
                    list =
                        list.Where(
                            a =>
                                a.NgayVanThu.HasValue && a.NgayVanThu.Value >= from && a.NgayVanThu.Value.Year <= to)
                            .ToList();
                }
                else if (tungayType == DateType.Year && denngayType == DateType.MonthYear)
                {
                    var from = CommonFunction.ConvertInt(tungay);
                    var to = CommonFunction.ConvertCustomDateTime(dengay, false);
                    list =
                        list.Where(
                            a =>
                                a.NgayVanThu.HasValue && a.NgayVanThu.Value.Year >= from && a.NgayVanThu <= to)
                            .ToList();
                }
            }

            SubStringContent(list);
            list = list.OrderByDescending(a => a.NgayVanThu).ToList();
            return list;
        }

        private static void SubStringContent(IEnumerable<v_vanThu> news)
        {
            foreach (var newse in news)
            {
                newse.TinhTrang = newse.TrangThaiPheDuyet.HasValue && newse.TrangThaiPheDuyet.Value ? "Đã giải quyết" : "Chưa giải quyết";

                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(newse.VanBan))
                {
                    sb.AppendFormat(@"<a  href='{0}?iframe=true&width=90%&height=100%' rel='prettyPhoto[iframe]'> 
                        <button class='btn btn-xs btn-info'>{1}</button> </a>", VirtualPathUtility.ToAbsolute(newse.VanBan),
                        CommonFunction.HumanReadFile(newse.VanBan));
                    newse.VanBan = sb.ToString();
                }
                else newse.VanBan = string.Empty;

                if (string.IsNullOrWhiteSpace(newse.NoiDung) || newse.NoiDung.Length <= 200) continue;
                var noHTML = Regex.Replace(newse.NoiDung, @"<[^>]+>", "").Trim();
                newse.NoiDung = CommonFunction.TruncateAtWord(noHTML, 200);



            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection frm)
        {
            GetDefaultValue();
            var tungay = frm["tungay"];
            var dengay = frm["dengay"];
            var nguoigui = frm["nguoigui"];
            var nguoinhan = frm["nguoinhan"];
            var veviec = frm["veviec"];
            var trangthai = frm["trangthai"];
            var loaivanthu = frm["loaivanthu"];
            var vanthus = GetOrders(loaivanthu, tungay, dengay, nguoigui, nguoinhan, veviec, trangthai).ToList();

            if (Request["btnXuat"] == "Xuất")
            {
                #region Export Pdf And Excel

                switch (trangthai)
                {
                    case "both":
                        trangthai = "Tất cả";
                        break;
                    case "yes":
                        trangthai = "Đã giải quyết";
                        break;
                    case "no":
                        trangthai = "Chưa giải quyết";
                        break;
                }
                switch (loaivanthu)
                {
                    case "0":
                        loaivanthu = "Tất cả văn thư";
                        break;
                    case "1":
                        loaivanthu = "Văn thư đến";
                        break;
                    case "2":
                        loaivanthu = "Văn thư đi";
                        break;
                }
                var list = new List<VanThuReport>();
                foreach (var vVanThu in vanthus)
                {
                    var vanthu = Mapper.DynamicMap<v_vanThu, VanThuReport>(vVanThu);
                    vanthu.NgayVanThu = vVanThu.NgayVanThu.HasValue
                        ? vVanThu.NgayVanThu.Value.ToString("dd/MM/yyyy")
                        : "";
                    list.Add(vanthu);
                }
                var rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ReportVanThu.rpt"));
                rd.SetDataSource(list);
                rd.SetParameterValue("loaivanban", "Tình trạng: " + trangthai);
                rd.SetParameterValue("trangthai", loaivanthu);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                if (Request["dataType"] == "Excel")
                {
                    var stream = rd.ExportToStream(ExportFormatType.ExcelWorkbook);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/xlsx",
                        string.Format("vanthu_report_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Pdf")
                {
                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf",
                        string.Format("vanthu_report_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Doc")
                {
                    var stream = rd.ExportToStream(ExportFormatType.WordForWindows);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/doc",
                        string.Format("vanthu_report_{0}.doc", DateTime.Now.ToString("dd_MM_yyyy")));
                }

                #endregion

                ViewBag.LoaiVanThu = ViewData["loaivanthu"];
            }
            if (Request["btnXemTruoc"] == "Xem trước")
            {
                return RedirectToAction("TestPdf");
            }
            ViewData["orders"] = vanthus;
            return View();
        }

        public ActionResult SearchVanThu()
        {
            GetDefaultValue();
            return View();
        }

        [HttpPost]
        public ActionResult SearchVanThu(FormCollection frm)
        {
            GetDefaultValue();
            ViewBag.LoaiVanThu = ViewData["loaivanthu"];
            if (Request["btnXuat"] == "Xuất")
            {
                #region Export Pdf And Excel
                var keyword = frm["keyword"];
                var list = GetSearchVanThuToList(keyword).ToList();

                var rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "VanThuReport.rpt"));
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
                        string.Format("searchvanthu_report_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Pdf")
                {
                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf",
                        string.Format("searchvanthu_report_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Doc")
                {
                    var stream = rd.ExportToStream(ExportFormatType.WordForWindows);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/doc",
                        string.Format("searchvanthu_report_{0}.doc", DateTime.Now.ToString("dd_MM_yyyy")));
                }

                #endregion

                
            }
            return View();
        }

        public ActionResult VanThuReport()
        {
            GetDefaultValue();
            ViewBag.PhanLoaiVanThu = db.PhanLoaiVanThus.Where(a => a.CapVanThu == null).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult VanThuReport(FormCollection frm)
        {
            GetDefaultValue();
            ViewBag.PhanLoaiVanThu = db.PhanLoaiVanThus.Where(a => a.CapVanThu == null).ToList();
            if (Request["btnXuat"] == "Xuất")
            {
                #region Export Pdf And Excel
                var keyword = frm["veviec"];
                var list = GetSearchVanThuToList(keyword).ToList();
               
                var rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "NhungVanDeCanHop.rpt"));
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
                        string.Format("searchvanthu_report_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Pdf")
                {
                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf",
                        string.Format("searchvanthu_report_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy")));
                }
                if (Request["dataType"] == "Doc")
                {
                    var stream = rd.ExportToStream(ExportFormatType.WordForWindows);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/doc",
                        string.Format("searchvanthu_report_{0}.doc", DateTime.Now.ToString("dd_MM_yyyy")));
                }

                #endregion


            }
            return View();
        }

        #region GetDataForReport

        public ActionResult _SearchVanThu(string keyword, DataSourceRequest request)
        {
            return Json(GetSearchVanThu(keyword).ToDataSourceResult(request));

        }
        private IEnumerable<VanThuDBO> GetSearchVanThu(string keyword)
        {
            var result = string.IsNullOrEmpty(keyword) ? db.VanThus.ToList() : db.VanThus.Where(a => a.TieuDe.Contains(keyword)).ToList();
            return result.Select(Mapper.DynamicMap<VanThu, VanThuDBO>).ToList();
        }

        private IEnumerable<v_vanThu> GetSearchVanThuToList(string keyword)
        {
            var result = new List<v_vanThu>();
            foreach (var id in keyword.Split(CommonFunction._delimiterPost))
            {
                var id1 = CommonFunction.ConvertInt(id);
                var list = db.v_vanThu.Where(a => a.PhanLoaiVanThu_Id == id1);
                result.AddRange(list);
            }
            //var result = string.IsNullOrEmpty(keyword) ?: db.v_vanThu.Where(a => a.TieuDe.Contains(keyword)).ToList();
            return result;
        }
        public ActionResult _VanThuReport(string keyword, DataSourceRequest request)
        {
            return Json(GetVanThuReport(keyword).ToDataSourceResult(request));

        }
        private IEnumerable<VanThuDBO> GetVanThuReport(string keyword)
        {
            var result = new List<v_vanThu>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                foreach (var id in keyword.Split(CommonFunction._delimiterPost))
                {
                    var id1 = CommonFunction.ConvertInt(id);
                    var list = db.v_vanThu.Where(a => a.PhanLoaiVanThu_Id == id1);
                    result.AddRange(list);
                }
            }
            else
            {
                result = db.v_vanThu.ToList();
            }
            

            return result.Select(Mapper.DynamicMap<v_vanThu, VanThuDBO>).ToList();
        }
        #endregion
        //Nhận văn thừ từ nhân viên văn phòng
        public ActionResult VanThuDen(int? id)
        {
            GetDefaultValue();
            ViewBag.VanThuId = id;
            var vanthu = new VanThu();
            if (id.HasValue)
            {
                vanthu = db.VanThus.FirstOrDefault(a => a.id == id);
                ViewBag.LoaiVanThu = vanthu.LoaiVanThu;
            }
            ViewBag.PhanLoaiVanThu = db.PhanLoaiVanThus.Where(a => a.CapVanThu == null).ToList();
            var result = Mapper.DynamicMap<VanThu, VanThuDBO>(vanthu);
            return View(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VanThuDen(FormCollection frm)
        {
            GetDefaultValue();
            var vanthuId = CommonFunction.ConvertInt(frm["vanthuId"]);
            var vanthu =new VanThu();
            if (vanthuId > 0)
            {
                vanthu = db.VanThus.FirstOrDefault(a => a.id == vanthuId) ?? new VanThu();
            }
            ViewBag.PhanLoaiVanThu = db.PhanLoaiVanThus.Where(a => a.CapVanThu == null).ToList();
            vanthu.NoiDung = frm["NoiDung"];
            vanthu.TieuDe = frm["veviec"];
            vanthu.NgayVanThu = CommonFunction.ConvertDateTime(frm["ngayvanthu"]);
            vanthu.TinhTrang = frm["tinhtrang"];
            //vanthu.LoaiVanDe = frm["LoaiVanDe"];
            vanthu.NguoiGui = frm["nguoigui"];
            vanthu.NguoiNhan = frm["nguoinhan"];
            vanthu.GhiChu = frm["GhiChu"];
            vanthu.LoaiVanThu = frm["LoaiVanThu"];
            vanthu.LoaiVanDe = frm["PhanLoai"];
            vanthu.TrangThaiPheDuyet = frm["TrangThaiPheDuyet"] == "on";
            var erros = ValidateFunction.ValidateVanThu(vanthu);
           
            if (erros.Count==0)
            {
                var fileRequest = string.Format("ChungChi");
                var file = Request.Files[fileRequest];
                if (file != null && file.FileName.Length > 0)
                {
                    var systemLocation = "";
                    var importPath = Path.Combine(Constant.Document_VanThu, vanthu.id.ToString(CultureInfo.InvariantCulture));
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
                    vanthu.VanBan = systemLocation;
                }
                else if (string.IsNullOrEmpty(Request["txtChungChi"]))
                {
                    vanthu.VanBan = string.Empty;
                }
                if (vanthu.id == 0)
                    db.VanThus.Add(vanthu);
                else
                {
                    db.Entry(vanthu).State = System.Data.Entity.EntityState.Modified;
                }
                    
                db.SaveChanges();
                Response.Redirect("~/VanThu/");
            }
            ViewBag.Errors = erros;
            var result = Mapper.DynamicMap<VanThu, VanThuDBO>(vanthu);
            return View(result);
        }

        //Nhận văn thừ từ nhân viên văn phòng
        public ActionResult ViewVanThu(int? id)
        {
            GetDefaultValue();
            ViewBag.VanThuId = id;
            var vanthu = new VanThu();
            if (id.HasValue)
            {
                vanthu = db.VanThus.FirstOrDefault(a => a.id == id);
                ViewBag.LoaiVanThu = vanthu.LoaiVanThu;
                int loaiVanDe = CommonFunction.ConvertInt(vanthu.LoaiVanDe);
                var phanLoaiVanThu = db.PhanLoaiVanThus.FirstOrDefault(a => a.Id == loaiVanDe);
                if (phanLoaiVanThu != null)
                    ViewBag.PhanLoaiVanThu = phanLoaiVanThu.Description;
            }
            
            var result = Mapper.DynamicMap<VanThu, VanThuDBO>(vanthu);
            return View(result);
        }
        

        public ActionResult Delete(int? id)
        {
            GetDefaultValue();
            ViewBag.VanThuId = id;
            ViewBag.LoaiVanThu = db.PhanLoaiVanThus.Where(a => a.CapVanThu == null).ToList();
            VanThu vanthu = new VanThu();
            if (id.HasValue)
            {
                vanthu = db.VanThus.FirstOrDefault(a => a.id == id);
                ViewBag.LoaiVanThu = vanthu.LoaiVanThu;
            }
            var result = Mapper.DynamicMap<VanThu, VanThuDBO>(vanthu);
            return View(result);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection frm)
        {
            GetDefaultValue();
            if (frm["VanThuId"] != "0")
            {
                var id = CommonFunction.ConvertInt(frm["vanthuId"]);
                var vanthu = db.VanThus.FirstOrDefault(a => a.id == id);
                if (vanthu != null)
                {
                    db.VanThus.Remove(vanthu);
                    db.SaveChanges();
                }
                return RedirectToAction("Index"); //Response.Redirect("~/VanThu/");
            }

            return View();
        }
        

    }
}
