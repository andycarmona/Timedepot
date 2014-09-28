using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// XmlWorker's Unicode Font Provider class.
    /// </summary>
    public class UnicodeFontProvider : FontFactoryImp
    {
        /// <summary>
        /// Provides a font with BaseFont.IDENTITY_H encoding.
        /// </summary>
        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
        {
            if (string.IsNullOrEmpty(fontname))
                return new Font(Font.FontFamily.UNDEFINED, size, style, color);
            return FontFactory.GetFont(fontname, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style, color);
        }
    }
}