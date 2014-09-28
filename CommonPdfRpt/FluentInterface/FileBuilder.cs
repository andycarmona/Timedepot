﻿using System.IO;
using PdfRpt.Core.Helper;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Pdf RptFile Builder Class.
    /// </summary>
    public class FileBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// Flushes the fileData into the user's browser.
        /// </summary>
        internal bool FlushReportDataInBrowser { get; private set; }

        /// <summary>
        /// How to flush an in memory PDF file.
        /// </summary>
        internal FlushType FlushType { get; private set; }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public FileBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets produced PDF file's path and name.
        /// It can be null if you are using an in memory stream.
        /// </summary>
        /// <param name="fileName">produced PDF file's path and name</param>
        public void AsPdfFile(string fileName)
        {
            fileName.CheckDirectoryExists();
            _pdfReport.DataBuilder.SetFileName(fileName);
        }

        /// <summary>
        /// Sets the PDF file's stream.
        /// It can be null. In this case a new FileStream will be used automatically and you need to provide the FileName.
        /// </summary>
        /// <param name="pdfStreamOutput">the PDF file's stream</param>
        public void AsPdfStream(Stream pdfStreamOutput)
        {
            _pdfReport.DataBuilder.SetStreamOutput(pdfStreamOutput);
        }

        /// <summary>
        /// Flushes the fileData into the user's browser.
        /// It's designed for the ASP.NET Applications.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="flushType">How to flush an in memory PDF file</param>
        public void FlushInBrowser(string fileName = "report.pdf", FlushType flushType = FlushType.Attachment)
        {
            _pdfReport.DataBuilder.SetFileName(fileName);
            FlushReportDataInBrowser = true;
            FlushType = flushType;
        }
    }
}