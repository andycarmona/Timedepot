using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Allows catching several document events.
    /// </summary>
    public class PageEvents : PdfPageEventHelper
    {
        #region Fields (1)

        BackgroundImageTemplate _backgroundImageTemplate;
        HeaderFooterManager _headerFooterManager;
        DiagonalWatermarkManager _diagonalWatermarkManager;

        #endregion Fields

        #region Properties (5)

        /// <summary>
        /// Rows summaries data
        /// </summary>
        public IList<SummaryCellData> ColumnSummaryCellsData { set; get; }

        /// <summary>
        /// Stores the last rendered row's data
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Document settings
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Optional custom footer of the pages.
        /// </summary>
        public IPageFooter PdfRptFooter { get; set; }

        /// <summary>
        /// Optional custom header of the pages.
        /// </summary>
        public IPageHeader PdfRptHeader { set; get; }

        #endregion Properties

        #region Methods (11)

        // Public Methods (4) 

        /// <summary>
        /// Fires when the document is closed.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            _backgroundImageTemplate.ApplyBackgroundImage(document);
            _headerFooterManager.ApplyFooter(writer, document, ColumnSummaryCellsData);
            _diagonalWatermarkManager.ApplyWatermark(document);
        }

        /// <summary>
        /// Fires when a page is finished, just before being written to the document.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            _headerFooterManager.AddFooter(writer, document, ColumnSummaryCellsData);
            _diagonalWatermarkManager.ReserveWatermarkSpace(writer);
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            initBackgroundImageTemplate(writer, document);
            initHeaderFooterManager(writer);
            initDiagonalWatermarkManager(writer, document);
        }

        private void initDiagonalWatermarkManager(PdfWriter writer, Document document)
        {
            _diagonalWatermarkManager = new DiagonalWatermarkManager { PageSetup = PageSetup };
            _diagonalWatermarkManager.InitWatermarkTemplate(writer, document);
        }

        private void initBackgroundImageTemplate(PdfWriter writer, Document document)
        {
            _backgroundImageTemplate = new BackgroundImageTemplate { PageSetup = PageSetup };
            _backgroundImageTemplate.InitBackgroundImageTemplate(writer, document);
        }

        private void initHeaderFooterManager(PdfWriter writer)
        {
            _headerFooterManager = new HeaderFooterManager
            {
                ColumnSummaryCellsData = ColumnSummaryCellsData,
                CurrentRowInfoData = CurrentRowInfoData,
                PdfRptFooter = PdfRptFooter,
                PdfRptHeader = PdfRptHeader,
                CacheHeader = PageSetup.PagePreferences.CacheHeader
            };
            _headerFooterManager.InitFooter(writer, ColumnSummaryCellsData);
        }

        /// <summary>
        /// Fires when a page is initialized.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            _headerFooterManager.AddHeader(writer, document);
            _backgroundImageTemplate.ReserveBackgroundImageSpace(writer);            
        }
        // Private Methods (7) 

        #endregion Methods
    }
}
