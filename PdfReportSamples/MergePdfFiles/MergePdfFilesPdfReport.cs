﻿using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfReportSamples.HexDump;
using PdfReportSamples.IList;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfReportSamples.MergePdfFiles
{
    public class MergePdfFilesPdfReport
    {
        public string CreatePdfReport()
        {
            return mergeMultipleReports();
        }

        private string mergeMultipleReports()
        {
            var rpt1 = new IListPdfReport().CreatePdfReport();
            var rpt2 = new HexDumpPdfReport().CreatePdfReport();

            var finalMergedFile = System.IO.Path.Combine(AppPath.ApplicationPath, "Pdf\\mergedFile.pdf");

            new MergePdfDocuments
            {
                DocumentMetadata = new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "MergePdfFiles Rpt.", Title = "Test" },
                InputFileStreams = new[] 
                     {
                       new FileStream(rpt1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read),
                       new FileStream(rpt2.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                     },
                OutputFileStream = new FileStream(finalMergedFile, FileMode.Create),
                AttachmentsBookmarkLabel = "Attachment(s) ",
                WriterCustomizer = importedPageInfo =>
                {
                    addNewPageNumbersToFinalMergedFile(importedPageInfo);
                }
            }
            .PerformMerge();

            return finalMergedFile;
        }

        private void addNewPageNumbersToFinalMergedFile(ImportedPageInfo importedPageInfo)
        {
            var bottomMargin = importedPageInfo.PdfDocument.BottomMargin;
            var pageSize = importedPageInfo.PageSize;
            var contentByte = importedPageInfo.Stamp.GetOverContent();

            // hide the old footer
            contentByte.SaveState();
            contentByte.SetColorFill(BaseColor.WHITE);
            contentByte.Rectangle(0, 0, pageSize.Width, bottomMargin);
            contentByte.Fill();
            contentByte.RestoreState();

            // write the new page numbers
            var center = (pageSize.Left + pageSize.Right) / 2;
            ColumnText.ShowTextAligned(
                canvas: contentByte,
                alignment: Element.ALIGN_CENTER,
                phrase: new Phrase("Page " + importedPageInfo.CurrentPageNumber + "/" + importedPageInfo.TotalNumberOfPages),
                x: center,
                y: pageSize.GetBottom(25),
                rotation: 0,
                runDirection: PdfWriter.RUN_DIRECTION_LTR,
                arabicOptions: 0);
        }
    }
}
