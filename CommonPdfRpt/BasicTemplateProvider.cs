using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using PdfRpt.Core.Contracts;

namespace PdfRpt
{
    /// <summary>
    ///A set of a predefined main table's templates.
    /// </summary>
    public class BasicTemplateProvider : ITableTemplate
    {
        #region Fields (15)

        readonly IDictionary<BasicTemplate, BaseColor> _alternatingRowBackgroundColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("PaleGoldenrod"))},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.WhiteSmoke)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(Color.White)   },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(ColorTranslator.FromHtml("#CCCCCC"))},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(ColorTranslator.FromHtml("#CCCCCC"))},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#DEBA84"))},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.White)}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _alternatingRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#003399"))},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#284775"))},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#4A3C8C"))},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#000066"))},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(ColorTranslator.FromHtml("#330099"))    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#8C4510"))},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#333333"))},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#333333"))}
            };
        readonly BasicTemplate _basicTemplate;

        readonly IDictionary<BasicTemplate, BaseColor> _cellBorderColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(ColorTranslator.FromHtml("#336666")) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.MochaTemplate, new BaseColor(ColorTranslator.FromHtml("#DEDFDE"))},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(ColorTranslator.FromHtml("#999999"))},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("Tan"))},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF"))},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCCC"))},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCCC"))},
                {  BasicTemplate.AutumnTemplate, new BaseColor(Color.LightGray)     },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(ColorTranslator.FromHtml("#999999"))},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(ColorTranslator.FromHtml("#999999"))},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.LightGray)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.LightGray)}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _headerBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#336666")), new BaseColor(ColorTranslator.FromHtml("#246d6d")) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#4A3C8C")), new BaseColor(ColorTranslator.FromHtml("#56489c")) } },
                {  BasicTemplate.MochaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#6B696B")), new BaseColor(ColorTranslator.FromHtml("#5b5a5b"))} },
                {  BasicTemplate.NullTemplate, new List<BaseColor> { null} },
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#003399")), new BaseColor(ColorTranslator.FromHtml("#06399f")) } },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#5D7B9D")), new BaseColor(ColorTranslator.FromHtml("#4d7199"))} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#000084")), new BaseColor(ColorTranslator.FromHtml("#020291")) } },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor> { new BaseColor(Color.FromName("Tan"))} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor> { new BaseColor(Color.LightGray) , new BaseColor(Color.DarkGray)} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#1C5E55")), new BaseColor(ColorTranslator.FromHtml("#16574e"))} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#4A3C8C")), new BaseColor(ColorTranslator.FromHtml("#3e317e"))} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#006699")), new BaseColor(ColorTranslator.FromHtml("#025c89"))} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#333333")), new BaseColor(ColorTranslator.FromHtml("#454545"))} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#990000")) , new BaseColor(ColorTranslator.FromHtml("#e80000")) } },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor> { new BaseColor(Color.Black)} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor> { new BaseColor(Color.Black)} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#A55129")), new BaseColor(ColorTranslator.FromHtml("#b05327"))} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#507CD1")), new BaseColor(ColorTranslator.FromHtml("#2f5fba")) } },
                {  BasicTemplate.TimelyDepoTemplate, new List<BaseColor> { new BaseColor(Color.White), new BaseColor(Color.White) } },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#990000")), new BaseColor(ColorTranslator.FromHtml("#e80000")) } }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _headerFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF"))},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCFF"))},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F7F7"))},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(ColorTranslator.FromHtml("#FFFFCC"))    },
                {  BasicTemplate.BlackAndBlue1Template , new BaseColor(Color.White)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.White)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.White)}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _pageSummaryRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7")) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.NullTemplate, new List<BaseColor>{ null} },
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.TimelyDepoTemplate, new List<BaseColor>{ new BaseColor(Color.White), new BaseColor(Color.White)} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _pageSummaryRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SnowyPineTemplate,  new BaseColor(Color.Black)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(Color.Black)   },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black)}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _remainingRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.NullTemplate, new List<BaseColor> { null }},
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7")) } },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} },
                {  BasicTemplate.TimelyDepoTemplate, new List<BaseColor> { new BaseColor(Color.White), new BaseColor(Color.White)} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3")), new BaseColor(ColorTranslator.FromHtml("#ccd5e7"))} }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _remainingRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.Black)    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black)}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _rowBackgroundColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(ColorTranslator.FromHtml("#DEDFDE"))},
                {  BasicTemplate.MochaTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F7DE"))},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F6F3"))},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(ColorTranslator.FromHtml("#EEEEEE"))},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("LightGoldenrodYellow"))},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(ColorTranslator.FromHtml("#E3EAEB"))},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF"))},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White)} ,
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.White)    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.White)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.White)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#FFF7E7"))},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#EFF3FB"))},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.White)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#FFFBD6"))}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _rowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(ColorTranslator.FromHtml("#333333")) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#003399"))},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#333333"))},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#4A3C8C"))},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#000066"))},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(ColorTranslator.FromHtml("#330099"))   },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#8C4510"))},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#333333"))},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#333333"))}
            };

        readonly IDictionary<BasicTemplate, bool> _showGridLines =
            new Dictionary<BasicTemplate, bool>
            {
                {  BasicTemplate.CoverFieldTemplate, true },
                {  BasicTemplate.LiliacsInMistTemplate, false},
                {  BasicTemplate.MochaTemplate, true},
                {  BasicTemplate.NullTemplate, false},
                {  BasicTemplate.OceanicaTemplate, true},
                {  BasicTemplate.ProfessionalTemplate, false},
                {  BasicTemplate.RainyDayTemplate, false},
                {  BasicTemplate.SandAndSkyTemplate, true},
                {  BasicTemplate.SilverTemplate, true},
                {  BasicTemplate.SimpleTemplate, false},
                {  BasicTemplate.SlateTemplate, false},
                {  BasicTemplate.SnowyPineTemplate, true},
                {  BasicTemplate.AppleOrchardTemplate, true},
                {  BasicTemplate.AutumnTemplate,   true   },
                {  BasicTemplate.BlackAndBlue1Template, false},
                {  BasicTemplate.BlackAndBlue2Template, true},
                {  BasicTemplate.BrownSugarTemplate, true},
                {  BasicTemplate.ClassicTemplate, false},
                {  BasicTemplate.TimelyDepoTemplate, true},
                {  BasicTemplate.ColorfulTemplate, false}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _summaryRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653")) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.NullTemplate, new List<BaseColor>{ null }},
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.AutumnTemplate,  new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653"))} },
                {  BasicTemplate.TimelyDepoTemplate, new List<BaseColor>{ new BaseColor(Color.White), new BaseColor(Color.White)} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9")), new BaseColor(ColorTranslator.FromHtml("#b8c653")) }}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _summaryRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.Black)    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black)},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.TimelyDepoTemplate, new BaseColor(Color.Black)},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black)}
            };


        readonly IDictionary<BasicTemplate, HorizontalAlignment> _headerHorizontalAlignment =
            new Dictionary<BasicTemplate, HorizontalAlignment>
            {
                {  BasicTemplate.CoverFieldTemplate, HorizontalAlignment.Center },
                {  BasicTemplate.LiliacsInMistTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.MochaTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.NullTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.OceanicaTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ProfessionalTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.RainyDayTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SandAndSkyTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SilverTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SimpleTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SlateTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SnowyPineTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.AppleOrchardTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.AutumnTemplate,  HorizontalAlignment.Center    },
                {  BasicTemplate.BlackAndBlue1Template, HorizontalAlignment.Center},
                {  BasicTemplate.BlackAndBlue2Template, HorizontalAlignment.Center},
                {  BasicTemplate.BrownSugarTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ClassicTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.TimelyDepoTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ColorfulTemplate, HorizontalAlignment.Center}
            };
        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Main table's template selector
        /// </summary>
        /// <param name="basicTemplate">template's name</param>
        public BasicTemplateProvider(BasicTemplate basicTemplate)
        {
            _basicTemplate = basicTemplate;
        }

        #endregion Constructors

        #region Properties (14)

        /// <summary>
        /// Alternating rows background color value
        /// </summary>
        public BaseColor AlternatingRowBackgroundColor
        {
            get { return _alternatingRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Alternating rows font color value
        /// </summary>
        public BaseColor AlternatingRowFontColor
        {
            get { return _alternatingRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Cells border color value
        /// </summary>
        public BaseColor CellBorderColor
        {
            get { return _cellBorderColor[_basicTemplate]; }
        }

        /// <summary>
        /// Main table's header background color value
        /// </summary>
        public IList<BaseColor> HeaderBackgroundColor
        {
            get { return _headerBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Main table's headers font color value
        /// </summary>
        public BaseColor HeaderFontColor
        {
            get { return _headerFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Pages summary row background color value
        /// </summary>
        public IList<BaseColor> PageSummaryRowBackgroundColor
        {
            get { return _pageSummaryRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Pages summary rows font color value
        /// </summary>
        public BaseColor PageSummaryRowFontColor
        {
            get { return _pageSummaryRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Remaining rows background color value
        /// </summary>
        public IList<BaseColor> PreviousPageSummaryRowBackgroundColor
        {
            get { return _remainingRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Remaining rows font color value
        /// </summary>
        public BaseColor PreviousPageSummaryRowFontColor
        {
            get { return _remainingRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows background color value
        /// </summary>
        public BaseColor RowBackgroundColor
        {
            get { return _rowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows font color value
        /// </summary>
        public BaseColor RowFontColor
        {
            get { return _rowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Sets visibility of the main table's grid lines
        /// </summary>
        public bool ShowGridLines
        {
            get { return _showGridLines[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows background color value
        /// </summary>
        public IList<BaseColor> SummaryRowBackgroundColor
        {
            get { return _summaryRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows font color value
        /// </summary>
        public BaseColor SummaryRowFontColor
        {
            get { return _summaryRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Header's caption horizontal alignment
        /// </summary>
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return _headerHorizontalAlignment[_basicTemplate]; }
        }

        #endregion Properties
    }
}
