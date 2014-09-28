using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PdfReportSamples.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using PdfReportSamples;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using System.Configuration;


namespace TimelyDepotMVC.Controllers.PDFReporting
{
    public class PurchaseOrderReport
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        IQueryable<InvoiceDetail> qryDetails = null;

        public IPdfReportData CreateReport(int purchaseorderId = 0)
        {
            //int purchaseorderIdHlp = 4003;

            decimal dUnitPrice = 0;
            decimal dQty = 0;
            decimal dAmount = 0;
            string szConnectionString = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ToString();
            string szProviderName = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ProviderName;
            string szSql = "";

            //szSql = string.Format("SELECT TOP (100) PERCENT InvoiceId, Quantity, ShipQuantity, BackOrderQuantity, ItemID, Sub_ItemID, Description, Tax, UnitPrice, ItemPosition, ItemOrder " +
            //    "FROM  InvoiceDetail WHERE (InvoiceId = {0}) ORDER BY ItemPosition, ItemOrder", purchaseorderId.ToString());
            szSql = string.Format("SELECT TOP (100) PERCENT PurchaseOrderId, ItemID, Sub_ItemID, Description, Quantity, UnitPrice, VendorReference, ItemPosition, ItemOrder " +
                "FROM PurchasOrderDetail WHERE (PurchaseOrderId = {0}) ORDER BY ItemPosition, ItemOrder", purchaseorderId.ToString());

            var report = new PdfReport();

            //Document preferences
            report.DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vios", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Purchase Order Report", Title = "Invoice" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
                doc.PrintingPreferences(new PrintingPreferences
                {
                    ShowPrintDialogAutomatically = true
                });
            }
            );

            //Fonts
            report.DefaultFonts(fonts =>
            {
                fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                fonts.Size(10);
                fonts.Color(System.Drawing.Color.Black);

            });

            //The footer
            report.PagesFooter(footer =>
            {
                var date = DateTime.Now.ToString("MM/dd/yyyy");
                footer.InlineFooter(inlineFooter =>
                {
                    inlineFooter.FooterProperties(new FooterBasicProperties
                    {
                        PdfFont = footer.PdfFont,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        RunDirection = PdfRunDirection.LeftToRight,
                        SpacingBeforeTable = 30,
                        TotalPagesCountTemplateHeight = 9,
                        TotalPagesCountTemplateWidth = 50
                    });

                    inlineFooter.AddPageFooter(data =>
                    {
                        return createFooter(footer, date, data);
                    });
                });

            });

            // The Header
            report.PagesHeader(header =>
            {
                header.InlineHeader(inlineheader =>
                {
                    inlineheader.AddPageHeader(data =>
                    {
                        return createPOHeader(header, purchaseorderId);
                    });
                });
            });

            //Main table
            report.MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.TimelyDepoTemplate);
            });

            report.MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.NumberOfDataRowsPerPage(15);
            });

            //Main data
            report.MainTableDataSource(dataSource =>
            {

                {
                    dataSource.GenericDataReader(
                       providerName: szProviderName,
                       connectionString: szConnectionString,
                       sql: szSql
                   );
                }
            });

            //Summary main Data
            //report.MainTableSummarySettings(summarySettings =>
            //{
            //    summarySettings.OverallSummarySettings("Summary");
            //    summarySettings.PreviousPageSummarySettings("Previous Page Summary");
            //    summarySettings.PageSummarySettings("Page Summary");

            //});

            // Required Columns and formats
            report.MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("rowNo");
                    column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.HeaderCell("#");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("PurchaseOrderId");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell("Purchase Order Id");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(2);
                    column.Width(1);
                    column.HeaderCell("ItemID");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Sub_ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(2);
                    column.HeaderCell("Our Item #");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Description");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(6);
                    column.HeaderCell("Description");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("VendorReference");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("Your Item");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Quantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(6);
                    column.Width(0.75f);
                    column.HeaderCell("Req. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("UnitPrice");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                    column.Order(7);
                    column.Width(1);
                    column.HeaderCell("Unit Price");
                    column.IsVisible(true);
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n2}", obj));
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("CF1");
                    column.HeaderCell("Ext. Amount");
                    column.Width(1);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Right);

                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n2}", obj));
                    });

                    column.CalculatedField(list =>
                    {
                        if (list == null)
                        {
                            return string.Empty;
                        }

                        if (list.GetValueOf("UnitPrice") == null)
                        {
                            dUnitPrice = 0;
                        }
                        else
                        {
                            dUnitPrice = Convert.ToDecimal(list.GetValueOf("UnitPrice"));
                        }

                        if (list.GetValueOf("Quantity") == null)
                        {
                            dQty = 0;
                        }
                        else
                        {
                            dQty = Convert.ToDecimal(list.GetValueOf("Quantity"));
                        }

                        dAmount = dQty * dUnitPrice;
                        return dAmount;
                    });

                    //column.AggregateFunction(aggregateFunction =>
                    //{
                    //    aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                    //    aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n2}", obj));
                    //});

                    column.Order(8);
                    column.IsVisible(true);
                });


            });



            //Main table Events
            report.MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");

                events.MainTableAdded(args =>
                {
                    //var data = args.LastOverallAggregateValueOf<Order>(y => y.Price);
                    //var msg = "Total: " + data + ", " + long.Parse(data, NumberStyles.AllowThousands, CultureInfo.InvariantCulture).NumberToText(Language.English);
                    //var infoTable = new PdfGrid(numColumns: 1)
                    //{
                    //    WidthPercentage = 100
                    //};
                    //infoTable.AddSimpleRow(
                    //     (cellData, properties) =>
                    //     {
                    //         cellData.Value = "Show data after the main table ...";
                    //         properties.PdfFont = events.PdfFont;
                    //         properties.RunDirection = PdfRunDirection.LeftToRight;
                    //     });
                    //infoTable.AddSimpleRow(
                    //     (cellData, properties) =>
                    //     {
                    //         cellData.Value = msg;
                    //         properties.PdfFont = events.PdfFont;
                    //         properties.RunDirection = PdfRunDirection.LeftToRight;
                    //     });
                    //args.PdfDoc.Add(infoTable.AddBorderToTable(borderColor: BaseColor.LIGHT_GRAY, spacingBefore: 25f));

                    //PdfPTable salesamounttb = GetSalesAmountData(purchaseorderIdHlp);
                    PdfPTable POamounttb = GetPOAmountData(purchaseorderId);
                    args.PdfDoc.Add(POamounttb);
                });

                //events.PageTableAdded(args => 
                //{
                //    PdfPTable salesamounttb = GetSalesAmountData(purchaseorderIdHlp);
                //    args.PdfDoc.Add(salesamounttb);

                //});

            });

            //Export report
            report.Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            });



            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
            var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\POReport-{1}.pdf", AppPath.ApplicationPath, purchaseorderId.ToString())));

            return reportHlp;
        }



        private PdfPTable GetPOAmountData(int purchaseorderId)
        {
            decimal dTotal = 0;
            string szMsg = "";
            string szLogo = "HILTON GARDEN INN.";
            string szImprintColors = "Process blue & ...";
            string szBlindShip = "Blind Ship";
            string szStars = "**************";

            string szFromTitle = "";
            string szFromCompany = "";
            string szFromAddress1 = "";
            string szFromAddress2 = "";
            string szFromCity = "";
            string szFromState = "";
            string szFromZip = "";
            string szFromCountry = "";
            string szFromEmail = "";
            string szFromTel = "";
            string szFromFax = "";
            string szFromName = "";

            string szToTitle = "";
            string szToCompany = "";
            string szToAddress1 = "";
            string szToAddress2 = "";
            string szToCity = "";
            string szToState = "";
            string szToZip = "";
            string szToCountry = "";
            string szToEmail = "";
            string szToTel = "";
            string szToFax = "";
            string szToName = "";

            PurchaseOrders purchaseorder = db.PurchaseOrders.Find(purchaseorderId);
            if (purchaseorder != null)
            {
                szLogo = purchaseorder.Logo;
                szImprintColors = purchaseorder.ImprintColor;

                if (purchaseorder.IsBlindShip)
                {
                    szBlindShip = "Blind Ship";
                }
                else
                {
                    szBlindShip = string.Empty;
                }

                szFromAddress1 = purchaseorder.FromAddress1;
                szFromAddress2 = purchaseorder.FromAddress2;
                szFromCity = purchaseorder.FromCity;
                szFromCompany = purchaseorder.FromCompany;
                szFromCountry = purchaseorder.FromCountry;
                szFromEmail = purchaseorder.FromEmail;
                szFromFax = purchaseorder.FromFax;
                szFromState = purchaseorder.FromState;
                szFromTel = purchaseorder.FromTel;
                szFromTitle = purchaseorder.FromTitle;
                szFromName = purchaseorder.FromName;
                szFromZip = purchaseorder.FromZip;

                szToAddress1 = purchaseorder.ToAddress1;
                szToAddress2 = purchaseorder.ToAddress2;
                szToCity = purchaseorder.ToCity;
                szToCompany = purchaseorder.ToCompany;
                szToCountry = purchaseorder.ToCountry;
                szToEmail = purchaseorder.ToEmail;
                szToFax = purchaseorder.ToFax;
                szToState = purchaseorder.ToState;
                szToTel = purchaseorder.ToTel;
                szToTitle = purchaseorder.ToTitle;
                szToName = purchaseorder.ToName;
                szToZip = purchaseorder.ToZip;

            }

            //The report data
            //Get the total
            TimelyDepotMVC.Controllers.PurchaseOrderController poctrl = new PurchaseOrderController();
            dTotal = poctrl.GetTotalPO(purchaseorderId);

            //The report layout
            Font times01 = FontFactory.GetFont("helvetica-bold", 12, BaseColor.BLACK);
            Font times02 = FontFactory.GetFont("helvetica-bold", 12, Font.ITALIC, BaseColor.BLACK);
            Font times03 = FontFactory.GetFont("helvetica-bold", 12, Font.UNDERLINE, BaseColor.BLACK);
            Font times04 = FontFactory.GetFont("helvetica", 12, BaseColor.BLACK);
            Font times05 = FontFactory.GetFont("helvetica", 12, Font.UNDERLINE, BaseColor.BLACK);
            Font times06 = FontFactory.GetFont("helvetica", 10, BaseColor.BLACK);
            Font times07 = FontFactory.GetFont("helvetica", 12, Font.ITALIC, BaseColor.BLACK);
            Font times08 = FontFactory.GetFont("helvetica-bold", 16, BaseColor.BLACK);

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;
            Paragraph title = null;

            PdfPTable infotable = new PdfPTable(numColumns: 1);
            infotable.WidthPercentage = 100;
            infotable.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            infotable.SpacingBefore = 25;

            szMsg = string.Format("Total {0}", dTotal.ToString("N2"));
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("Logo: {0}", szLogo);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("Imprint colors: {0}", szImprintColors);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szBlindShip);
            title = new Paragraph(szMsg, times08);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 10;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szStars);
            title = new Paragraph(szMsg, times08);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("From:");
            title = new Paragraph(szMsg, times03);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szFromCompany);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szFromName);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szFromAddress1);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szFromAddress2);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}, {1} {2}", szFromCity, szFromState, szFromZip);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);


            szMsg = string.Format("Ship to:");
            title = new Paragraph(szMsg, times03);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 20;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szToCompany);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szToName);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szToAddress1);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}", szToAddress2);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            szMsg = string.Format("{0}, {1} {2}", szToCity, szToState, szToZip);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            infotable.AddCell(hlpCel);

            return infotable;
        }


        private static PdfGrid createFooter(PagesFooterBuilder footer, string date, FooterData data)
        {

            var table = new PdfGrid(numColumns: 4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR
            };


            //Page counter
            var datePhrase = footer.PdfFont.FontSelector.Process(date);
            var datePhrase01 = footer.PdfFont.FontSelector.Process(" ");
            var datePdfCell = new PdfPCell(datePhrase01)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var nullPdfCell = new PdfPCell
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var pageNumberPhrase = footer.PdfFont.FontSelector.Process("Page " + data.CurrentPageNumber + " of ");
            var pageNumberPdfCell = new PdfPCell(pageNumberPhrase)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var totalPagesNumberImagePdfCell = new PdfPCell(data.TotalPagesCountImage)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                PaddingLeft = 0,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            table.AddCell(datePdfCell);
            table.AddCell(nullPdfCell);
            table.AddCell(pageNumberPdfCell);
            table.AddCell(totalPagesNumberImagePdfCell);
            return table;
        }

        private static PdfGrid createFooter01(PagesFooterBuilder footer, string date, FooterData data)
        {
            string szMsg = "";
            Paragraph title = null;

            Font times01 = FontFactory.GetFont("helvetica-bold", 10, BaseColor.BLACK);
            Font times02 = FontFactory.GetFont("helvetica-bold", 10, Font.ITALIC, BaseColor.BLACK);
            Font times03 = FontFactory.GetFont("helvetica-bold", 10, Font.UNDERLINE, BaseColor.BLACK);
            Font times04 = FontFactory.GetFont("helvetica", 10, BaseColor.BLACK);
            Font times05 = FontFactory.GetFont("helvetica", 10, Font.UNDERLINE, BaseColor.BLACK);
            Font times06 = FontFactory.GetFont("helvetica", 8, BaseColor.BLACK);


            var table = new PdfGrid(numColumns: 4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR
            };

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;

            //First Row Sales Amount
            PdfPTable nested = new PdfPTable(numColumns: 3);
            nested.SetTotalWidth(new float[] { 390.6f, 83.6f, 64.2f });
            nested.LockedWidth = true;

            szMsg = string.Format("Notes:");
            title = new Paragraph(szMsg, times06);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            szMsg = string.Format("Sales Amount:");
            title = new Paragraph(szMsg, times02);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            szMsg = string.Format("{0}", "96.85");
            title = new Paragraph(szMsg, times02);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested.AddCell(hlpCel);


            nestingcell = new PdfPCell(nested);
            nestingcell.Colspan = 4;
            nestingcell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            nestingcell.BorderWidthLeft = 0;
            nestingcell.BorderWidthRight = 0;
            nestingcell.BorderWidthTop = 0;
            nestingcell.BorderWidthBottom = 0;
            nestingcell.Padding = 0;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            nestingcell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(nestingcell);

            //Second Row Sales Amount
            PdfPTable nested01 = new PdfPTable(numColumns: 2);
            nested01.SetTotalWidth(new float[] { 390.6f, 147.8f });
            nested01.LockedWidth = true;

            szMsg = string.Format("{0}", "The note goes here !! (Use 4 rows)");
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested01.AddCell(hlpCel);


            PdfPTable nested02 = new PdfPTable(numColumns: 2);
            nested02.SetTotalWidth(new float[] { 83.6f, 64.2f });
            nested02.LockedWidth = true;

            szMsg = string.Format("Tax {0} %:", " ");
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested02.AddCell(hlpCel);

            szMsg = string.Format("{0}", "0.00");
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested02.AddCell(hlpCel);

            szMsg = string.Format("Shipping & Handling:", " ");
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested02.AddCell(hlpCel);

            szMsg = string.Format("{0}", "21.00");
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested02.AddCell(hlpCel);


            hlpCel = new PdfPCell(nested02);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            nested01.AddCell(hlpCel);



            nestingcell = new PdfPCell(nested01);
            nestingcell.Colspan = 4;
            nestingcell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            nestingcell.BorderWidthLeft = 0;
            nestingcell.BorderWidthRight = 0;
            nestingcell.BorderWidthTop = 0;
            nestingcell.BorderWidthBottom = 0;
            nestingcell.Padding = 0;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            nestingcell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(nestingcell);

            //Page counter
            var datePhrase = footer.PdfFont.FontSelector.Process(date);
            var datePhrase01 = footer.PdfFont.FontSelector.Process(" ");
            var datePdfCell = new PdfPCell(datePhrase01)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var nullPdfCell = new PdfPCell
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var pageNumberPhrase = footer.PdfFont.FontSelector.Process("Page " + data.CurrentPageNumber + " of ");
            var pageNumberPdfCell = new PdfPCell(pageNumberPhrase)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var totalPagesNumberImagePdfCell = new PdfPCell(data.TotalPagesCountImage)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 1,
                BorderWidthBottom = 0,
                Padding = 4,
                PaddingLeft = 0,
                BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            table.AddCell(datePdfCell);
            table.AddCell(nullPdfCell);
            table.AddCell(pageNumberPdfCell);
            table.AddCell(totalPagesNumberImagePdfCell);
            return table;
        }

        private PdfGrid createPOHeader(PagesHeaderBuilder header, int purchaseorderId)
        {
            int nTradeId = 0;
            string szMsg = "";
            string szAddress = "";
            string szCity = "";
            string szState = "";
            string szZip = "";
            string szCountry = "";
            string szTel = "";
            string szFax = "";
            string szWebSiteTrade = "";
            string szEmailTrade = "";
            string szTradeName = "";
            string szAsiTrade = "";
            string szSageTrade = "";
            string szPpaiTrade = "";

            string szPODate = "";
            string szPORef = "";
            string szPONo = "";
            string szPOShipDate = "";
            string szPOBy = "";
            string szPOBlindDrop = "";

            string szVendorCompanyName = "";
            string szVendorFirstName = "";
            string szVendorLastName = "";
            string szVendorTitle = "";
            string szVendorAddress1 = "";
            string szVendorAddress2 = "";
            string szVendorAddress3 = "";
            string szVendorCity = "";
            string szVendorState = "";
            string szVendorZip = "";
            string szVendorCountry = "";
            string szVendorTel = "";
            string szVendorFax = "";
            string szVendorEmail = "";
            string szVendorWebsite = "";
            string szVendorTel1 = "";
            string szVendorTel2 = "";

            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";

            DateTime dDate = DateTime.Now;


            //The report data
            PurchaseOrders purchaseorder = db.PurchaseOrders.Find(purchaseorderId);
            if (purchaseorder != null)
            {
                nTradeId = Convert.ToInt32(purchaseorder.TradeId);
                dDate = Convert.ToDateTime(purchaseorder.PODate);
                szPODate = dDate.ToString("MM/dd/yyyy");
                szPORef = purchaseorder.PurchaseOrderReference;
                szPONo = purchaseorder.PurchaseOrderNo;
                szPOBy = purchaseorder.PaidBy;
                szPOBlindDrop = purchaseorder.BlindDrop;
                if (purchaseorder.ShipDate == null)
                {
                    szPOShipDate = string.Empty;
                }
                else
                {
                    dDate = Convert.ToDateTime(purchaseorder.ShipDate);
                    szPOShipDate = dDate.ToString("MM/dd/yyyy");

                }
            }
            TimelyDepotMVC.Controllers.SalesOrderController.GetTradeData(db, ref szAddress, ref szCity, ref szState, ref szZip, ref szCountry, ref szTel, ref szFax,
                ref szWebSiteTrade, ref szEmailTrade, ref szTradeName, ref szAsiTrade, ref szSageTrade, ref szPpaiTrade, nTradeId);
            if (string.IsNullOrEmpty(szTel))
            {
                szTel = "0";
            }
            if (string.IsNullOrEmpty(szFax))
            {
                szFax = "0";
            }

            telHlp = Convert.ToInt64(szTel);
            szTel = telHlp.ToString(telfmt);
            telHlp = Convert.ToInt64(szFax);
            szFax = telHlp.ToString(telfmt);

            TimelyDepotMVC.Controllers.PurchaseOrderController.GetVendorData(db, purchaseorder.VendorId, ref szVendorCompanyName, ref szVendorFirstName, ref szVendorLastName,
                ref szVendorTitle, ref szVendorAddress1, ref szVendorAddress2, ref szVendorAddress3, ref szVendorCity, ref szVendorState, ref szVendorZip, ref szVendorCountry,
                ref szVendorTel, ref szVendorFax, ref szVendorEmail, ref szVendorWebsite, ref szVendorTel1, ref szVendorTel2);
            if (string.IsNullOrEmpty(szVendorTel1))
            {
                szVendorTel1 = "0";
            }
            if (string.IsNullOrEmpty(szVendorTel2))
            {
                szVendorTel2 = "0";
            }
            if (string.IsNullOrEmpty(szVendorTel))
            {
                szVendorTel = "0";
            }

            telHlp = Convert.ToInt64(szVendorTel1);
            szVendorTel1 = telHlp.ToString(telfmt);
            telHlp = Convert.ToInt64(szVendorTel2);
            szVendorTel2 = telHlp.ToString(telfmt);
            telHlp = Convert.ToInt64(szVendorTel);
            szVendorTel = telHlp.ToString(telfmt);

            //The report layout
            Font times01 = FontFactory.GetFont("helvetica-bold", 12, BaseColor.BLACK);
            Font times02 = FontFactory.GetFont("helvetica-bold", 12, Font.ITALIC, BaseColor.BLACK);
            Font times03 = FontFactory.GetFont("helvetica-bold", 12, Font.UNDERLINE, BaseColor.BLACK);
            Font times04 = FontFactory.GetFont("helvetica", 12, BaseColor.BLACK);
            Font times05 = FontFactory.GetFont("helvetica", 12, Font.UNDERLINE, BaseColor.BLACK);
            Font times06 = FontFactory.GetFont("helvetica", 10, BaseColor.BLACK);
            Font times07 = FontFactory.GetFont("helvetica", 12, Font.ITALIC, BaseColor.BLACK);
            Font times08 = FontFactory.GetFont("helvetica-bold", 16, BaseColor.BLACK);

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;
            Paragraph title = null;

            //Create the table for the headr
            var table = new PdfGrid(numColumns: 1);
            table.SetTotalWidth(new float[] { 553f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 7;


            szMsg = string.Format("{0}", szTradeName);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 4;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", szAddress);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}, {1} {2}", szCity, szState, szZip);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("Tel: {0}", szTel);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("Fax: {0}", szFax);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("Email: {0}", szEmailTrade);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            //
            szMsg = string.Format("{0}", szPODate);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 10;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 1;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            PdfPTable nested = new PdfPTable(2);
            nested.SetTotalWidth(new float[] { 332f, 221f });
            nested.LockedWidth = true;

            szMsg = string.Format("Attn: {0} {1}", szVendorFirstName, szVendorLastName);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            szMsg = string.Format("Ref: {0}", szPORef);
            title = new Paragraph(szMsg, times08);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);


            szMsg = string.Format("{0}", szVendorCompanyName.ToUpper());
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            szMsg = string.Format("Purchase Order No.: {0}", szPONo);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.BorderWidth = 0;
            nesthousing.Padding = 0f;
            table.AddCell(nesthousing);

            szMsg = string.Format("{0} {1} {2}", szVendorAddress1, szVendorAddress2, szVendorAddress3);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}, {1} {2}", szVendorCity, szVendorState, szVendorZip);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("Tel: {0} {1} {2}", szVendorTel1, szVendorTel2, szVendorTel);
            title = new Paragraph(szMsg, times04);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("Ship date: {0}", szPOShipDate);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 20;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("By: {0}", szPOBy);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("Blind Drop by: {0}", szPOBlindDrop);
            title = new Paragraph(szMsg, times01);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 6;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            return table;
        }



    }
}