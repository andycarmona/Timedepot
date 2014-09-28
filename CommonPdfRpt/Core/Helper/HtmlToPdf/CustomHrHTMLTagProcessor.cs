using System.Collections.Generic;
using iTextSharp.text.html.simpleparser;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// Currently HTML Worker does not support the HR tag. 
    /// Here is the do nothing tag processor to suppress null reference exceptions.
    /// </summary>
    public class CustomHrHTMLTagProcessor : IHTMLTagProcessor
    {
        /// <summary>
        /// Tells the HTMLWorker what to do when a close tag is encountered.
        /// </summary>
        public void EndElement(HTMLWorker worker, string tag)
        {
        }

        /// <summary>
        /// Tells the HTMLWorker what to do when an open tag is encountered.
        /// </summary>
        public void StartElement(HTMLWorker worker, string tag, IDictionary<string, string> attrs)
        {
            worker.CarriageReturn();
            worker.PushToStack(new ElementFactory().CreateLineSeparator(attrs, 0));
        }
    }
}