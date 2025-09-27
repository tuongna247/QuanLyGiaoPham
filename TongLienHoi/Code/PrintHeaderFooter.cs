using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HTTLVN.QLTLH.Code
{
    public class PrintHeaderFooter : PdfPageEventHelper
    {
        private PdfContentByte _pdfContent;
        private PdfTemplate _pageNumberTemplate;
        private BaseFont _baseFont;
        private DateTime _printTime;

        public string Title { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            _printTime = DateTime.Now;
            string path = HttpContext.Current.Server.MapPath("../Content/fonts/");
            _baseFont = BaseFont.CreateFont(path + "TIMES.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            _pdfContent = writer.DirectContent;
            _pdfContent.SetFontAndSize(_baseFont, 11);
            _pageNumberTemplate = _pdfContent.CreateTemplate(50, 50);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;

            if (Title != string.Empty)
            {
                _pdfContent.BeginText();
                _pdfContent.SetFontAndSize(_baseFont, 12);
                _pdfContent.SetRGBColorFill(0, 0, 0);
                _pdfContent.SetTextMatrix(pageSize.GetLeft(20), pageSize.GetTop(10));
                _pdfContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Title, pageSize.Width / 2, 800, 0);
                //_pdfContent.ShowText(Title);
                _pdfContent.EndText();
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            string text = "Trang " + pageN + " của ";

            Rectangle pageSize = document.PageSize;
            _pdfContent = writer.DirectContent;
            _pdfContent.SetRGBColorFill(0, 0, 0); ;

            _pdfContent.BeginText();
            _pdfContent.SetFontAndSize(_baseFont, 12);
            _pdfContent.SetTextMatrix(40, pageSize.GetBottom(30));
            _pdfContent.EndText();


            _pdfContent.BeginText();
            _pdfContent.SetFontAndSize(_baseFont, 8);
            _pdfContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(50), pageSize.GetBottom(30), 0);
            _pdfContent.EndText();

            _pdfContent.AddTemplate(_pageNumberTemplate, pageSize.GetRight(50), pageSize.GetBottom(30));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _pageNumberTemplate.BeginText();
            _pageNumberTemplate.SetFontAndSize(_baseFont, 8);
            _pageNumberTemplate.SetTextMatrix(0, 0);
            _pageNumberTemplate.ShowText(string.Empty + (writer.PageNumber - 1));
            _pageNumberTemplate.EndText();
        }
    }
}