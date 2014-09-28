using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// Using iTextSharp's limited HTML to PDF capabilities.
    /// </summary>    
    public class HtmlWorkerHelper
    {
        /// <summary>
        /// Custom HTML Element.
        /// </summary>
        public Image PdfElement;

        /// <summary>
        /// The HTML to show.
        /// </summary>
        public string Html { set; get; }

        /// <summary>
        /// Defines styles for HTMLWorker.
        /// </summary>
        public StyleSheet StyleSheet { set; get; }

        /// <summary>
        /// Run direction, left-to-right or right-to-left.
        /// </summary>
        public PdfRunDirection RunDirection { set; get; }

        /// <summary>
        /// Cells horizontal alignment value
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { set; get; }

        /// <summary>
        /// Custom font's definitions
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Using iTextSharp's limited HTML to PDF capabilities.
        /// </summary>
        public PdfPCell RenderHtml()
        {
            var pdfCell = new PdfPCell
            {
                UseAscender = true,
                UseDescender = true,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            applyStyleSheet();

            var tags = setCustomTags();

            using (var reader = new StringReader(Html))
            {
                var parsedHtmlElements = HTMLWorker.ParseToList(reader, StyleSheet, tags, null);

                foreach (var htmlElement in parsedHtmlElements)
                {
                    applyRtlRunDirection(htmlElement);
                    pdfCell.AddElement(htmlElement);
                }

                return pdfCell;
            }
        }

        private HTMLTagProcessors setCustomTags()
        {
            var tags = new HTMLTagProcessors
            {
                { "totalpagesnumber", new TotalPagesNumberHTMLTagProcessor(this.PdfElement) }
            };
            tags[HtmlTags.HR] = new CustomHrHTMLTagProcessor();
            return tags;
        }

        private void applyRtlRunDirection(IElement htmlElement)
        {
            if (RunDirection != PdfRunDirection.RightToLeft) return;

            var table = htmlElement as PdfPTable;
            if (table == null) return;

            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            foreach (var cell in table.Rows.SelectMany(row => row.GetCells()))
            {
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            }
        }

        private void applyStyleSheet()
        {
            if (StyleSheet == null) StyleSheet = new StyleSheet();
            StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONTFAMILY, PdfFont.Fonts[0].Familyname);
            StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONTSIZE, string.Format("{0}pt", PdfFont.Fonts[0].Size));
            StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, "Identity-H");

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    StyleSheet.LoadTagStyle(HtmlTags.IMG, HtmlTags.ALIGN, HtmlTags.ALIGN_CENTER);
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_CENTER);
                    break;
                case HorizontalAlignment.Justified:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_JUSTIFY);
                    break;
                case HorizontalAlignment.JustifiedAll:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_JUSTIFIED_ALL);
                    break;
                case HorizontalAlignment.Left:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_LEFT);
                    break;
                case HorizontalAlignment.Right:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_RIGHT);
                    break;
                default:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_LEFT);
                    break;
            }
        }
    }
}