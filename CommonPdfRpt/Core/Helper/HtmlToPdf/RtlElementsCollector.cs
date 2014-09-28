using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// XMLWorker does not support RTL by default. so we need to collect the parsed elements first and
    /// then wrap them with a RTL table.
    /// </summary>
    public class RtlElementsCollector : IElementHandler
    {
        private readonly Paragraph _paragraph;

        /// <summary>
        /// XMLWorker does not support RTL by default. so we need to collect the parsed elements first and
        /// then wrap them with a RTL table.
        /// </summary>
        public RtlElementsCollector()
        {
            _paragraph = new Paragraph
            {
                Alignment = Element.ALIGN_LEFT
            };
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
                fixNestedTablesRunDirection(element);
                _paragraph.Add(element);
            }
        }

        /// <summary>
        /// It's necessary to fix the RTL of the final PdfPTables.
        /// </summary>        
        private void fixNestedTablesRunDirection(IElement element)
        {
            var table = element as PdfPTable;
            if (table == null)
                return;

            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            foreach (var row in table.Rows)
            {
                foreach (var cell in row.GetCells())
                {
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                    if (cell.CompositeElements == null)
                        continue;

                    foreach (var item in cell.CompositeElements)
                    {
                        fixNestedTablesRunDirection(item);
                    }
                }
            }
        }
    }
}