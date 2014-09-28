using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// This class allows accessing cell's canvas after finishing its rendering to add additional text or graphics.
    /// At this point we can add summary cells data.
    /// </summary>
    public class MainTableCellsEvent : IPdfPCellEvent
    {
        #region Fields (1)

        readonly CellAttributes _pdfRptCell;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Allows accessing cell's canvas after finishing its rendering to add additional text or graphics
        /// </summary>
        /// <param name="pdfRptCell">Related cell's attributes</param>
        public MainTableCellsEvent(CellAttributes pdfRptCell)
        {
            _pdfRptCell = pdfRptCell;
        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// List of the SummaryCells Data
        /// </summary>
        public IList<SummaryCellData> SummaryCellsData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Indicates grouping is enabled or not
        /// </summary>
        public bool IsGroupingEnabled { set; get; }

        #endregion Properties

        #region Methods (7)

        // Public Methods (1) 

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            setLastRenderedRowNumber();

            applyCustomCellLayout(cell, position, canvases);
            applyGradientBackground(position, canvases);

            if (_pdfRptCell.SharedData.PdfColumnAttributes == null) return;
            if (_pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction == null) return;
            if (_pdfRptCell.SharedData.PdfColumnAttributes.IsRowNumber) return;

            if (_pdfRptCell.RowData.PdfRowType == RowType.PreviousPageSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.AllGroupsSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.PageSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.SummaryRow)
            {
                setPagesInfo();
                printSummary(position, canvases);
            }
        }

        private void applyGradientBackground(Rectangle position, PdfContentByte[] canvases)
        {
            GradientBackground.ApplyGradientBackground(
                _pdfRptCell.RowData.PdfRowType,
                _pdfRptCell.SharedData,                
                position,
                canvases);
        }

        private void applyCustomCellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            if (_pdfRptCell.ItemTemplate == null) return;
            _pdfRptCell.ItemTemplate.CellRendered(cell, position, canvases, _pdfRptCell);
        }
        // Private Methods (6) 

        private string getRowSummaryData()
        {
            return _pdfRptCell.RowData.PdfRowType == RowType.PageSummaryRow ? thisPageSummary() : getTotalSummaries();
        }

        private string getTotalSummaries()
        {
            var row = CurrentRowInfoData.LastRenderedRowNumber;
            var propertyName = _pdfRptCell.SharedData.PdfColumnAttributes.PropertyName;

            var result = SummaryCellsData.FirstOrDefault(x => x.CellData.PropertyName == propertyName &&
                                                              x.OverallRowNumber == row);

            if (result == null)
            {
                return string.Empty;
            }

            object data;
            if (IsGroupingEnabled &&
                (_pdfRptCell.RowData.PdfRowType == RowType.SummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.PreviousPageSummaryRow))
            {
                data = result.GroupAggregateValue;
            }
            else
            {
                data = result.OverallAggregateValue;
            }

            return FuncHelper.ApplyFormula(_pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction.DisplayFormatFormula, data);
        }

        private void printSummary(Rectangle position, PdfContentByte[] canvases)
        {
            var rowSummaryData = getRowSummaryData();
            if (string.IsNullOrEmpty(rowSummaryData)) return;

            var pdfContentByte = canvases[PdfPTable.TEXTCANVAS];
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(_pdfRptCell.BasicProperties.PdfFont.Fonts[0].BaseFont,
                                          _pdfRptCell.BasicProperties.PdfFont.Fonts[0].Size);
            pdfContentByte.ShowTextAligned(
                PdfContentByte.ALIGN_CENTER,
                rowSummaryData,
                (position.Left + position.Right) / 2,
                ((position.Bottom + position.Top) / 2) - 4,
                0);
            pdfContentByte.EndText();
        }

        private void setLastRenderedRowNumber()
        {
            if (_pdfRptCell.SharedData.DataRowNumber <= 0) return;
            if (_pdfRptCell.SharedData.DataRowNumber <= CurrentRowInfoData.LastRenderedRowNumber) return;

            CurrentRowInfoData.LastRenderedRowNumber = _pdfRptCell.SharedData.DataRowNumber;
            CurrentRowInfoData.LastRenderedGroupNumber = _pdfRptCell.SharedData.GroupNumber;
        }

        private void setPagesInfo()
        {
            if (CurrentRowInfoData.PagesBoundaries == null)
            {
                CurrentRowInfoData.PagesBoundaries = new List<int>();
            }

            if (!CurrentRowInfoData.PagesBoundaries.Contains(CurrentRowInfoData.LastRenderedRowNumber))
            {
                CurrentRowInfoData.PagesBoundaries.Add(CurrentRowInfoData.LastRenderedRowNumber);
            }
        }

        private string thisPageSummary()
        {
            var pageBoundary = CurrentRowInfoData.PagesBoundaries.OrderByDescending(x => x).Take(2).ToList();
            if (!pageBoundary.Any()) return string.Empty;

            int firstRowOfThePage, lastRowOfThePage;
            if (pageBoundary.Count == 1)
            {
                firstRowOfThePage = 1;
                lastRowOfThePage = pageBoundary[0];
            }
            else
            {
                firstRowOfThePage = pageBoundary[1] + 1;
                lastRowOfThePage = pageBoundary[0];
            }

            var propertyName = _pdfRptCell.SharedData.PdfColumnAttributes.PropertyName;
            var list = SummaryCellsData.Where(x => x.CellData.PropertyName == propertyName
                                           && x.OverallRowNumber >= firstRowOfThePage && x.OverallRowNumber <= lastRowOfThePage)
                                       .ToList();
            var result = _pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction.ProcessingBoundary(list);
            return FuncHelper.ApplyFormula(_pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction.DisplayFormatFormula, result);
        }

        #endregion Methods
    }
}