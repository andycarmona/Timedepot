using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// Collects XMLWorker's parsed elements.
    /// </summary>
    public class SimpleElementsCollector : IElementHandler
    {
        private readonly Paragraph _paragraph;

        /// <summary>
        /// Collects XMLWorker's parsed elements.
        /// </summary>
        public SimpleElementsCollector()
        {
            _paragraph = new Paragraph();
        }

        /// <summary>
        /// This Paragraph contains all of the parsed elements.
        /// </summary>
        public Paragraph Paragraph
        {
            get { return _paragraph; }
        }

        /// <summary>
        /// Intercepting the XMLWorker's parser and collecting its interpreted pdf elements.
        /// </summary>        
        public void Add(IWritable htmlElement)
        {
            var writableElement = htmlElement as WritableElement;
            if (writableElement == null)
                return;

            foreach (var element in writableElement.Elements())
            {
                _paragraph.Add(element);
            }
        }
    }
}