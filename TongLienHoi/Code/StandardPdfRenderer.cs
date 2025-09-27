using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using HTTLVN.QLTLH.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;

namespace HTTLVN.QLTLH.Code
{
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 20;
        private const int VerticalMargin = 20;
        private BaseFont _baseFont;


        //public byte[] Render(string htmlText, string pageTitle)
        //{
        //    byte[] renderedBuffer;

        //    using (var outputMemoryStream = new MemoryStream())
        //    {
        //        using (
        //            var pdfDocument = new Document(new Rectangle(288f, 144f), HorizontalMargin, HorizontalMargin, 45, 45))
        //        {
        //            pdfDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
        //            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
        //            pdfWriter.CloseStream = false;
        //            pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
        //            pdfDocument.Open();
        //            string path = HttpContext.Current.Server.MapPath("../Content/fonts/");
        //            _baseFont = BaseFont.CreateFont(path + "TIMES.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //            var fontTopic = new Font(_baseFont, 16f, Font.NORMAL, BaseColor.BLACK);
        //            var fontHeader = new Font(_baseFont, 14f, Font.NORMAL, BaseColor.BLACK);
        //            var fontDetail = new Font(_baseFont, 10f, Font.NORMAL, BaseColor.BLACK);
        //            var table = new PdfPTable(6);

        //            var topic = new PdfPCell(new Phrase("VĂN THƯ ĐẾN", fontTopic))
        //            {
        //                Colspan = 6,
        //                Border = 0,
        //                HorizontalAlignment = 3
        //            };
        //            table.AddCell(topic);
        //            var space = new PdfPCell(new Phrase(" ", fontTopic))
        //            {
        //                Colspan = 6,
        //                Border = 0,
        //                HorizontalAlignment = 3
        //            };
        //            table.AddCell(space);
        //            var status = new PdfPCell(new Phrase("Tình Trạng: ", fontTopic))
        //            {
        //                Colspan = 6,
        //                Border = 0,
        //                HorizontalAlignment = 3
        //            };
        //            table.AddCell(status);

        //            var space1 = new PdfPCell(new Phrase(" ", fontTopic))
        //            {
        //                Colspan = 6,
        //                Border = 0,
        //                HorizontalAlignment = 3
        //            };
        //            table.AddCell(space1);
        //            //add header of pdf
        //            table.AddCell(new Phrase("TT", fontHeader));
        //            table.AddCell(new Phrase("Ngày gửi", fontHeader));
        //            table.AddCell(new Phrase("Người gửi", fontHeader));
        //            table.AddCell(new Phrase("Về việc", fontHeader));
        //            table.AddCell(new Phrase("Nội dung", fontHeader));
        //            table.AddCell(new Phrase("Ghi chú", fontHeader));
        //            //add content render

        //            table.AddCell(new Phrase("TT", fontDetail));
        //            table.AddCell(new Phrase("Ngày gửi", fontDetail));
        //            table.AddCell(new Phrase("Người gửi", fontDetail));
        //            table.AddCell(new Phrase("Về việc", fontDetail));
        //            table.AddCell(new Phrase("Nội dung", fontDetail));
        //            table.AddCell(new Phrase("Ghi chú", fontDetail));
        //            pdfDocument.Add(table);
        //        }
        //        renderedBuffer = new byte[outputMemoryStream.Position];
        //        outputMemoryStream.Position = 0;
        //        outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
        //    }

        //    return renderedBuffer;
        //}

        public byte[] Render(IEnumerable<Models.v_vanThu> model,string tinhtrang,string loaivanthu)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                using (
                    var pdfDocument = new Document(new Rectangle(288f, 144f), HorizontalMargin, HorizontalMargin, 45, 45))
                {
                    pdfDocument.SetPageSize(PageSize.A4.Rotate());
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = ""};
                    pdfDocument.Open();
                    string path = HttpContext.Current.Server.MapPath("../Content/fonts/");

                    #region Pdf Generate Header
                    _baseFont = BaseFont.CreateFont(path + "TIMES.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var fontTopic = new Font(_baseFont, 16f, Font.NORMAL, BaseColor.BLACK);
                    var fontHeader = new Font(_baseFont, 10f, Font.BOLD, BaseColor.BLACK);
                    var fontDetail = new Font(_baseFont, 10f, Font.NORMAL, BaseColor.BLACK);

                    var para = new Paragraph(loaivanthu, fontTopic)
                    {
                        SpacingAfter = 9f,
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDocument.Add(para);

                    var status = new Paragraph("Tình Trạng: " + tinhtrang, fontTopic)
                    {
                        SpacingAfter = 9f,
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDocument.Add(status);
                    var table = new PdfPTable(6);

                    //var topic = new PdfPCell(new Phrase(500, "", fontTopic))
                    //{
                    //    Colspan = 6,
                    //    Border = 1,
                    //    BackgroundColor = BaseColor.RED,
                    //    VerticalAlignment = 0,
                    //    HorizontalAlignment = 6
                    //};
                    //topic.Colspan = 6;
                    
                    //topic.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    //topic.VerticalAlignment = Element.ALIGN_TOP;
                    //table.AddCell(topic);
                    
                    
                    var space1 = new PdfPCell(new Phrase(" ", fontTopic))
                    {
                        Colspan = 6,
                        Border = 0,
                        HorizontalAlignment = 3
                    };
                    table.AddCell(space1);
                    //add header of pdf
                    var thutucolumn = new PdfPCell(new Phrase("TT", fontHeader));
                    table.SetWidths(new int[] {40, 80,80,100,300,200 });
                    table.AddCell(thutucolumn);
                    table.AddCell(new Phrase("Ngày gửi", fontHeader));
                    table.AddCell(new Phrase("Người gửi", fontHeader));
                    table.AddCell(new Phrase("Về việc", fontHeader));
                    table.AddCell(new Phrase("Nội dung", fontHeader));
                    table.AddCell(new Phrase("Ghi chú", fontHeader));
                    #endregion
                    //add content render
                    var vVanThus = model.ToList();
                    for (int i = 0; i < vVanThus.Count; i++)
                    {
                        var vVanThu = vVanThus[i];
                        var index = i + 1;
                        table.AddCell(new Phrase(index.ToString(CultureInfo.InvariantCulture), fontDetail));
                        table.AddCell(new Phrase(vVanThu.NgayVanThu.HasValue?vVanThu.NgayVanThu.Value.ToString("dd/MM/yyyy"):"", fontDetail));
                        table.AddCell(new Phrase(vVanThu.NguoiGui, fontDetail));
                        table.AddCell(new Phrase(vVanThu.TieuDe, fontDetail));
                        table.AddCell(new Phrase(vVanThu.NoiDung.Length>200? vVanThu.NoiDung.Substring(0,200):vVanThu.NoiDung, fontDetail));
                        table.AddCell(new Phrase(vVanThu.GhiChu, fontDetail));
                    }

                    pdfDocument.Add(table);
                }
                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }
    }

}