﻿using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.Core.Helper.HtmlToPdf;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Using iTextSharp's limited HTML to PDF capabilities.
    /// This class uses the HTMLWorker class of iTextSharp which is deprecated now and you should switch to the XHtmlField.
    /// </summary>
    [Obsolete("HtmlField uses the HTMLWorker class of iTextSharp which is deprecated now and you should replace it with XHtmlField.")]
    public class HtmlField : IColumnItemsTemplate
    {
        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, it will be filled by using current template's settings internally.
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values. 
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
        }

        /// <summary>
        /// Defines styles for HTMLWorker.
        /// </summary>
        public StyleSheet StyleSheet { set; get; }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            var html = FuncHelper.ApplyFormula(attributes.BasicProperties.DisplayFormatFormula, attributes.RowData.Value);
            attributes.RowData.FormattedValue = html;
            var cell = new HtmlWorkerHelper
            {
                PdfFont = attributes.BasicProperties.PdfFont,
                HorizontalAlignment = attributes.BasicProperties.HorizontalAlignment.Value,
                Html = html,
                RunDirection = attributes.BasicProperties.RunDirection.Value,
                StyleSheet = StyleSheet
            }.RenderHtml();
            return cell;
        }
    }
}
