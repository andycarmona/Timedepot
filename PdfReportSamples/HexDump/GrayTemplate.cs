using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using PdfRpt.Core.Contracts;

namespace PdfReportSamples.HexDump
{
    public class GrayTemplate : ITableTemplate
    {
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return HorizontalAlignment.Center; }
        }

        public BaseColor AlternatingRowBackgroundColor
        {
            get { return new BaseColor(Color.WhiteSmoke); }
        }

        public BaseColor CellBorderColor
        {
            get { return new BaseColor(Color.LightGray); }
        }

        public IList<BaseColor> HeaderBackgroundColor
        {
            get { return new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#990000")), new BaseColor(ColorTranslator.FromHtml("#e80000")) }; }
        }

        public BaseColor RowBackgroundColor
        {
            get { return null; }
        }

        public IList<BaseColor> PreviousPageSummaryRowBackgroundColor
        {
            get { return new List<BaseColor> { new BaseColor(Color.LightSkyBlue) }; }
        }

        public IList<BaseColor> SummaryRowBackgroundColor
        {
            get { return new List<BaseColor> { new BaseColor(Color.LightSteelBlue) }; }
        }

        public IList<BaseColor> PageSummaryRowBackgroundColor
        {
            get { return new List<BaseColor> { new BaseColor(Color.Yellow) }; }
        }

        public BaseColor AlternatingRowFontColor
        {
            get { return new BaseColor(ColorTranslator.FromHtml("#333333")); }
        }

        public BaseColor HeaderFontColor
        {
            get { return new BaseColor(Color.White); }
        }

        public BaseColor RowFontColor
        {
            get { return new BaseColor(ColorTranslator.FromHtml("#333333")); }
        }

        public BaseColor PreviousPageSummaryRowFontColor
        {
            get { return new BaseColor(Color.Black); }
        }

        public BaseColor SummaryRowFontColor
        {
            get { return new BaseColor(Color.Black); }
        }

        public BaseColor PageSummaryRowFontColor
        {
            get { return new BaseColor(Color.Black); }
        }

        public bool ShowGridLines
        {
            get { return true; }
        }
    }
}
