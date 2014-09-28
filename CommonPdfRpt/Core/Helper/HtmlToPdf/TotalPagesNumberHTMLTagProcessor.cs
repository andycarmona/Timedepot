using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// Our custom HTML Tag to add an IElement.
    /// </summary>
    public class TotalPagesNumberHTMLTagProcessor : IHTMLTagProcessor
    {
        readonly Image _image;
        /// <summary>
        /// ctor.
        /// </summary>        
        public TotalPagesNumberHTMLTagProcessor(Image image)
        {
            _image = image;
        }

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
            worker.UpdateChain(tag, attrs);
            worker.ProcessImage(_image, attrs);
            worker.UpdateChain(tag);
        }
    }
}
