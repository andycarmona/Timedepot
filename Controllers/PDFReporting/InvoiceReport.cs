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
using System.Text;

namespace TimelyDepotMVC.PDFReporting
{
    public class InvoiceReport
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        IQueryable<InvoiceDetail> qryDetails = null;


        public IPdfReportData CreateReport02(string szReportFolderPath, HttpResponseBase Response, int Invoiceid = 0)
        {
            decimal dUnitPrice = 0;
            decimal dQty = 0;
            decimal dAmount = 0;
            string szConnectionString = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ToString();
            string szProviderName = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ProviderName;
            string szSql = "";
            string szFilePath = "";

            szSql = string.Format("SELECT TOP (100) PERCENT InvoiceId, Quantity, ShipQuantity, BackOrderQuantity, ItemID, Sub_ItemID, Description, Tax, UnitPrice, ItemPosition, ItemOrder " +
                "FROM  InvoiceDetail WHERE (InvoiceId = {0}) ORDER BY ItemPosition, ItemOrder", Invoiceid.ToString());


            var report = new PdfReport();

            //Document preferences
            report.DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vios", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Invoice Report", Title = "Invoice" });
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
                string szFontsPath = System.IO.Path.Combine(szReportFolderPath, "fonts\\arial.ttf");
                string szFontsPath2 = System.IO.Path.Combine(szReportFolderPath, "fonts\\verdana.ttf");
                fonts.Path(szFontsPath, szFontsPath2);

                //fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                //           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));

                fonts.Size(9);
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
                        return createHeader(header, Invoiceid);
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
                //var listOfRows = new List<User>();
                //for (int i = 0; i < 10; i++)
                //{
                //    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                //}

                //var listOfRows = new List<InvoiceDetail>();
                //qryDetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == Invoiceid);
                //if (qryDetails.Count() > 0)
                //{
                //    foreach (var item in qryDetails)
                //    {
                //        listOfRows.Add(item);
                //    }
                //}
                //dataSource.StronglyTypedList(listOfRows);

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
                    column.PropertyName("InvoiceId");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell("Invoice Id");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Quantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(2);
                    column.Width(0.75f);
                    column.HeaderCell("Req. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ShipQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(0.75f);
                    column.HeaderCell("Ship Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("BackOrderQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(4);
                    column.Width(0.75f);
                    column.HeaderCell("B.O. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(5);
                    column.Width(1);
                    column.HeaderCell("ItemID");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Sub_ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(6);
                    column.Width(2);
                    column.HeaderCell("ItemID No.");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Description");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(7);
                    column.Width(4);
                    column.HeaderCell("Description");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Tax");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(8);
                    column.Width(0.5f);
                    column.HeaderCell("Tax");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("UnitPrice");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                    column.Order(9);
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

                    column.Order(10);
                    column.IsVisible(true);
                });


            });

            //report.MainTableColumns(columns =>
            //{
            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName("rowNo");
            //        column.IsRowNumber(true);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(0);
            //        column.Width(1);
            //        column.HeaderCell("#");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(1);
            //        column.Width(2);
            //        column.HeaderCell("Id");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Name);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(2);
            //        column.Width(3);
            //        column.HeaderCell("Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.LastName);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(3);
            //        column.Width(3);
            //        column.HeaderCell("Last Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Balance);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(4);
            //        column.Width(2);
            //        column.HeaderCell("Balance");
            //        column.ColumnItemsTemplate(template =>
            //        {
            //            template.TextBlock();
            //            template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //        column.AggregateFunction(aggregateFunction =>
            //        {
            //            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            //            aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(5);
            //        column.Width(2);
            //        column.HeaderCell("QRCode");
            //        column.ColumnItemsTemplate(itemsTemplate =>
            //        {
            //            itemsTemplate.InlineField(inlineField =>
            //            {
            //                inlineField.RenderCell(cellData =>
            //                {
            //                    var data = cellData.Attributes.RowData.TableRowData;
            //                    var id = data.GetSafeStringValueOf<User>(x => x.Id);

            //                    var qrcode = new BarcodeQRCode(id, 1, 1, null);
            //                    var image = qrcode.GetImage();
            //                    var mask = qrcode.GetImage();
            //                    mask.MakeMask();
            //                    image.ImageMask = mask; // making the background color transparent
            //                    var pdfCell = new PdfPCell(image, fit: false);

            //                    return pdfCell;
            //                });
            //            });
            //        });
            //    });
            //});


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

                    PdfPTable salesamounttb = GetSalesAmountData(Invoiceid);
                    args.PdfDoc.Add(salesamounttb);
                });

            });

            //Export report
            report.Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            });


            //szFilePath = string.Format("{0}\\InvoiceReport-{1}.pdf", szReportFolderPath, Invoiceid.ToString());
            //var szHlp = AppPath.ApplicationPath;

            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));

            //var reportHlp = report.Generate(data => data.AsPdfFile(szFilePath));

            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));

            //var reportHlp = report.Generate02(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())), Response);
            var reportHlp = report.Generate02(null, Response);

            return reportHlp;
        }


        public IPdfReportData CreateReport01(string szReportFolderPath, int Invoiceid = 0)
        {
            decimal dUnitPrice = 0;
            decimal dQty = 0;
            decimal dAmount = 0;
            string szConnectionString = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ToString();
            string szProviderName = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ProviderName;
            string szSql = "";
            string szFilePath = "";

            szSql = string.Format("SELECT TOP (100) PERCENT InvoiceId, Quantity, ShipQuantity, BackOrderQuantity, ItemID, Sub_ItemID, Description, Tax, UnitPrice, ItemPosition, ItemOrder " +
                "FROM  InvoiceDetail WHERE (InvoiceId = {0}) ORDER BY ItemPosition, ItemOrder", Invoiceid.ToString());

            if (HttpContext.Current == null)
            {
                return null;
            }
            var fileName = string.Format("MemoryInvoiceReport-{0}.pdf", Invoiceid.ToString());


            var report = new PdfReport();

            //Document preferences
            report.DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vios", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Invoice Report", Title = "Invoice" });
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
                string szFontsPath = System.IO.Path.Combine(szReportFolderPath, "fonts\\arial.ttf");
                string szFontsPath2 = System.IO.Path.Combine(szReportFolderPath, "fonts\\verdana.ttf");
                fonts.Path(szFontsPath, szFontsPath2);

                //fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                //           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));

                fonts.Size(9);
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
                        return createHeader(header, Invoiceid);
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
                //var listOfRows = new List<User>();
                //for (int i = 0; i < 10; i++)
                //{
                //    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                //}

                //var listOfRows = new List<InvoiceDetail>();
                //qryDetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == Invoiceid);
                //if (qryDetails.Count() > 0)
                //{
                //    foreach (var item in qryDetails)
                //    {
                //        listOfRows.Add(item);
                //    }
                //}
                //dataSource.StronglyTypedList(listOfRows);

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
                    column.PropertyName("InvoiceId");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell("Invoice Id");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Quantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(2);
                    column.Width(0.75f);
                    column.HeaderCell("Req. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ShipQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(0.75f);
                    column.HeaderCell("Ship Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("BackOrderQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(4);
                    column.Width(0.75f);
                    column.HeaderCell("B.O. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(5);
                    column.Width(1);
                    column.HeaderCell("ItemID");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Sub_ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(6);
                    column.Width(2);
                    column.HeaderCell("ItemID No.");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Description");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(7);
                    column.Width(4);
                    column.HeaderCell("Description");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Tax");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(8);
                    column.Width(0.5f);
                    column.HeaderCell("Tax");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("UnitPrice");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                    column.Order(9);
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

                    column.Order(10);
                    column.IsVisible(true);
                });


            });

            //report.MainTableColumns(columns =>
            //{
            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName("rowNo");
            //        column.IsRowNumber(true);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(0);
            //        column.Width(1);
            //        column.HeaderCell("#");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(1);
            //        column.Width(2);
            //        column.HeaderCell("Id");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Name);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(2);
            //        column.Width(3);
            //        column.HeaderCell("Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.LastName);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(3);
            //        column.Width(3);
            //        column.HeaderCell("Last Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Balance);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(4);
            //        column.Width(2);
            //        column.HeaderCell("Balance");
            //        column.ColumnItemsTemplate(template =>
            //        {
            //            template.TextBlock();
            //            template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //        column.AggregateFunction(aggregateFunction =>
            //        {
            //            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            //            aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(5);
            //        column.Width(2);
            //        column.HeaderCell("QRCode");
            //        column.ColumnItemsTemplate(itemsTemplate =>
            //        {
            //            itemsTemplate.InlineField(inlineField =>
            //            {
            //                inlineField.RenderCell(cellData =>
            //                {
            //                    var data = cellData.Attributes.RowData.TableRowData;
            //                    var id = data.GetSafeStringValueOf<User>(x => x.Id);

            //                    var qrcode = new BarcodeQRCode(id, 1, 1, null);
            //                    var image = qrcode.GetImage();
            //                    var mask = qrcode.GetImage();
            //                    mask.MakeMask();
            //                    image.ImageMask = mask; // making the background color transparent
            //                    var pdfCell = new PdfPCell(image, fit: false);

            //                    return pdfCell;
            //                });
            //            });
            //        });
            //    });
            //});


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

                    PdfPTable salesamounttb = GetSalesAmountData(Invoiceid);
                    args.PdfDoc.Add(salesamounttb);
                });

            });

            //Export report
            report.Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            });


            //szFilePath = string.Format("{0}\\InvoiceReport-{1}.pdf", szReportFolderPath, Invoiceid.ToString());
            //var szHlp = AppPath.ApplicationPath;

            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));

            //var reportHlp = report.Generate(data => data.AsPdfFile(szFilePath));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));
            
            var reportMemHlp = report.Generate(data => 
            {
                fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                data.FlushInBrowser(fileName, FlushType.Inline);
            });

            return reportMemHlp;
        }


        public IPdfReportData CreateReport(string szReportFolderPath, int Invoiceid = 0 )
        {
            decimal dUnitPrice = 0;
            decimal dQty = 0;
            decimal dAmount = 0;
            string szConnectionString = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ToString();
            string szProviderName = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ProviderName;
            string szSql = "";
            string szFilePath = "";

            szSql = string.Format("SELECT TOP (100) PERCENT InvoiceId, Quantity, ShipQuantity, BackOrderQuantity, ItemID, Sub_ItemID, Description, Tax, UnitPrice, ItemPosition, ItemOrder " +
                "FROM  InvoiceDetail WHERE (InvoiceId = {0}) ORDER BY ItemPosition, ItemOrder", Invoiceid.ToString());


            var report = new PdfReport();

            //Document preferences
            report.DocumentPreferences(doc =>
                {
                    doc.RunDirection(PdfRunDirection.LeftToRight);
                    doc.Orientation(PageOrientation.Portrait);
                    doc.PageSize(PdfPageSize.A4);
                    doc.DocumentMetadata(new DocumentMetadata { Author = "Vios", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Invoice Report", Title = "Invoice" });
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
                string szFontsPath = System.IO.Path.Combine(szReportFolderPath, "fonts\\arial.ttf");
                string szFontsPath2 = System.IO.Path.Combine(szReportFolderPath, "fonts\\verdana.ttf");
                fonts.Path(szFontsPath, szFontsPath2);

                //fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                //           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));

                fonts.Size(9);
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
                        return createHeader(header, Invoiceid);
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
                //var listOfRows = new List<User>();
                //for (int i = 0; i < 10; i++)
                //{
                //    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                //}

                //var listOfRows = new List<InvoiceDetail>();
                //qryDetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == Invoiceid);
                //if (qryDetails.Count() > 0)
                //{
                //    foreach (var item in qryDetails)
                //    {
                //        listOfRows.Add(item);
                //    }
                //}
                //dataSource.StronglyTypedList(listOfRows);

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
                    column.PropertyName("InvoiceId");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell("Invoice Id");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Quantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(2);
                    column.Width(0.75f);
                    column.HeaderCell("Req. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ShipQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(0.75f);
                    column.HeaderCell("Ship Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("BackOrderQuantity");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(4);
                    column.Width(0.75f);
                    column.HeaderCell("B.O. Qty");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(5);
                    column.Width(1);
                    column.HeaderCell("ItemID");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Sub_ItemID");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(6);
                    column.Width(2);
                    column.HeaderCell("ItemID No.");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Description");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(7);
                    column.Width(4);
                    column.HeaderCell("Description");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Tax");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(8);
                    column.Width(0.5f);
                    column.HeaderCell("Tax");
                    column.IsVisible(false);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("UnitPrice");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                    column.Order(9);
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

                    column.Order(10);
                    column.IsVisible(true);
                });


            });

            //report.MainTableColumns(columns =>
            //{
            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName("rowNo");
            //        column.IsRowNumber(true);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(0);
            //        column.Width(1);
            //        column.HeaderCell("#");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(1);
            //        column.Width(2);
            //        column.HeaderCell("Id");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Name);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(2);
            //        column.Width(3);
            //        column.HeaderCell("Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.LastName);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(3);
            //        column.Width(3);
            //        column.HeaderCell("Last Name");
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Balance);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(4);
            //        column.Width(2);
            //        column.HeaderCell("Balance");
            //        column.ColumnItemsTemplate(template =>
            //        {
            //            template.TextBlock();
            //            template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //        column.AggregateFunction(aggregateFunction =>
            //        {
            //            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            //            aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
            //        });
            //    });

            //    columns.AddColumn(column =>
            //    {
            //        column.PropertyName<User>(x => x.Id);
            //        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
            //        column.IsVisible(true);
            //        column.Order(5);
            //        column.Width(2);
            //        column.HeaderCell("QRCode");
            //        column.ColumnItemsTemplate(itemsTemplate =>
            //        {
            //            itemsTemplate.InlineField(inlineField =>
            //            {
            //                inlineField.RenderCell(cellData =>
            //                {
            //                    var data = cellData.Attributes.RowData.TableRowData;
            //                    var id = data.GetSafeStringValueOf<User>(x => x.Id);

            //                    var qrcode = new BarcodeQRCode(id, 1, 1, null);
            //                    var image = qrcode.GetImage();
            //                    var mask = qrcode.GetImage();
            //                    mask.MakeMask();
            //                    image.ImageMask = mask; // making the background color transparent
            //                    var pdfCell = new PdfPCell(image, fit: false);

            //                    return pdfCell;
            //                });
            //            });
            //        });
            //    });
            //});


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

                    PdfPTable salesamounttb = GetSalesAmountData(Invoiceid);
                    args.PdfDoc.Add(salesamounttb);
                });

            });

            //Export report
            report.Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            });


            //szFilePath = string.Format("{0}\\InvoiceReport-{1}.pdf", szReportFolderPath, Invoiceid.ToString());
            //var szHlp = AppPath.ApplicationPath;

            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));
            //var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));

            //var reportHlp = report.Generate(data => data.AsPdfFile(szFilePath));

            var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InvoiceReport-{1}.pdf", AppPath.ApplicationPath, Invoiceid.ToString())));

            return reportHlp;
        }


        private PdfPTable GetSalesAmountData(int Invoiceid = 0)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            decimal dPayment = 0;
            string szNotes = "The note";
            string szShipping = "";
            string szPayment = "";

            string szMsg = "";
            Paragraph title = null;

            //Get the invoice data
            Invoice invoice = null;
            invoice = db.Invoices.Find(Invoiceid);
            if (invoice == null)
            {
                return null;
            }
            szNotes = invoice.Note;
            szShipping = Convert.ToDecimal(invoice.ShippingHandling).ToString("C");

            if (invoice.PaymentAmount == null)
            {
                dPayment = 0;
            }
            else
            {
                dPayment = Convert.ToDecimal(invoice.PaymentAmount);
            }
            szPayment = dPayment.ToString("C");

            //Get the totals
            TimelyDepotMVC.Controllers.InvoiceController invoiceCtrl = new Controllers.InvoiceController();
            invoiceCtrl.GetInvoiceTotals(invoice.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            string szSalesAmount = dSalesAmount.ToString("C");
            string szTotalTax = dTotalTax.ToString("C");
            string szTax = dTax.ToString("F2");
            string szTotalAmount = dTotalAmount.ToString("C");
            string szBalanceDue = dBalanceDue.ToString("C");


            Font times01 = FontFactory.GetFont("helvetica-bold", 10, BaseColor.BLACK);
            Font times02 = FontFactory.GetFont("helvetica-bold", 10, Font.ITALIC, BaseColor.BLACK);
            Font times03 = FontFactory.GetFont("helvetica-bold", 10, Font.UNDERLINE, BaseColor.BLACK);
            Font times04 = FontFactory.GetFont("helvetica", 10, BaseColor.BLACK);
            Font times05 = FontFactory.GetFont("helvetica", 10, Font.UNDERLINE, BaseColor.BLACK);
            Font times06 = FontFactory.GetFont("helvetica", 8, BaseColor.BLACK);
            Font times07 = FontFactory.GetFont("helvetica", 10, Font.ITALIC, BaseColor.BLACK);

            PdfPTable infotable = new PdfPTable(numColumns: 4);
            infotable.WidthPercentage = 100;
            infotable.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            infotable.SpacingBefore = 25;

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;

            //First Row Sales Amount
            PdfPTable nested = new PdfPTable(numColumns: 3);
            nested.SetTotalWidth(new float[] { 350.6f, 123.6f, 64.2f });
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

            szMsg = string.Format("{0}", szSalesAmount);
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
            infotable.AddCell(nestingcell);


            //Second Row Sales Amount
            PdfPTable nested01 = new PdfPTable(numColumns: 2);
            nested01.SetTotalWidth(new float[] { 350.6f, 187.8f });
            nested01.LockedWidth = true;

            szMsg = string.Format("{0}", szNotes);
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
            nested02.SetTotalWidth(new float[] { 123.6f, 64.2f });
            nested02.LockedWidth = true;

            szMsg = string.Format("Tax {0} %:", szTax);
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

            szMsg = string.Format("{0}", szTotalTax);
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

            szMsg = string.Format("{0}", szShipping);
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

            szMsg = string.Format("Total Amount:", " ");
            title = new Paragraph(szMsg, times01);
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

            szMsg = string.Format("{0}", szTotalAmount);
            title = new Paragraph(szMsg, times01);
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


            szMsg = string.Format("Payment:", " ");
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

            szMsg = string.Format("{0}", szPayment);
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


            //
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
            infotable.AddCell(nestingcell);

            //Last Row Sales Amount
            PdfPTable nested03 = new PdfPTable(numColumns: 3);
            nested03.SetTotalWidth(new float[] { 350.6f, 123.6f, 64.2f });
            nested03.LockedWidth = true;

            times07.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("C/C#: {0}", "******8286");
            title = new Paragraph(szMsg, times07);
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
            nested03.AddCell(hlpCel);

            times02.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("Balance Due:");
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
            nested03.AddCell(hlpCel);

            szMsg = string.Format("{0}", szBalanceDue);
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
            nested03.AddCell(hlpCel);

            nestingcell = new PdfPCell(nested03);
            nestingcell.Colspan = 4;
            nestingcell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            nestingcell.BorderWidthLeft = 0;
            nestingcell.BorderWidthRight = 0;
            nestingcell.BorderWidthTop = 0;
            nestingcell.BorderWidthBottom = 0;
            nestingcell.Padding = 0;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            nestingcell.HorizontalAlignment = Element.ALIGN_CENTER;
            infotable.AddCell(nestingcell);

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

        private PdfGrid createHeader(PagesHeaderBuilder header, int Invoiceid)
        {
            string szMsg = "";
            string szInvoiceNo = "123456";
            string szCustomerNo = "810901";
            string szTrade = "The Essence of Time";
            string szASI = "52715";
            string szSage = "52534";
            string szTrade01 = "2508 Merced Ave Unit # A";
            string szCity = "";
            string szState = "";
            string szZip = "";
            string szCountry = "";
            string szTel = "";
            string szFax = "";
            string szWebSiteTrade = "";
            string szEmailTrade = "";
            string szAsiTrade = "";
            string szSageTrade = "";
            string szPpaiTrade = "";
            string szTrade02 = "www.timessence.com";
            string szTrade03 = "South El Monte, CA 91733";
            string szTrade04 = "asi@timessence.com";
            string szTrade05 = "626-527-3877";
            string szTrade06 = "626-527-3898";
            string szSoldto01 = "GINNY COLLINS";
            string szSoldto02 = "ADSTREET";
            string szSoldto03 = "120 HARDING STREET";
            string szSoldto04 = "CHAPIN, SC 29036";
            string szSoldto05 = " ";
            string szSoldto06 = " ";
            string szSoldto07 = "803-345-3208";
            string szSoldto08 = "803-932-0669";
            string szShipTo01 = "GINNY COLLINS";
            string szShipTo02 = "ADSTREET";
            string szShipTo03 = "120 HARDING STREET";
            string szShipTo04 = "CHAPIN, SC 29036";
            string szShipTo05 = " ";
            string szShipTo06 = " ";
            string szShipTo07 = "803-345-3208";
            string szShipTo08 = "803-932-0669";

            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";

            CustomersContactAddress soldto = null;
            CustomersBillingDept billto = null;
            CustomersShipAddress shipto = null;
            VendorsContactAddress venaddress = null;

            IQueryable<CustomersContactAddress> qryAddress = null;
            IQueryable<VendorsContactAddress> qryVenAddres = null;
            IQueryable<CustomersShipAddress> qryshipto = null;
            IQueryable<CustomersBillingDept> qryBill = null;


            //Get the invoice data
            Invoice invoice = null;
            invoice = db.Invoices.Find(Invoiceid);
            if (invoice == null)
            {
                return null;
            }
            szInvoiceNo = invoice.InvoiceNo;

            TimelyDepotMVC.Controllers.SalesOrderController.GetCustomerData01(db, ref szASI, ref szSage, ref szTrade02, ref szTrade04, ref szCustomerNo, Convert.ToInt32(invoice.CustomerId));
            TimelyDepotMVC.Controllers.SalesOrderController.GetTradeData(db, ref szTrade01, ref szCity, ref szState, ref szZip, ref szCountry, ref szTel, ref szFax,
                ref szTrade02, ref szTrade04, ref szTrade, ref szAsiTrade, ref szSageTrade, ref szPpaiTrade, Convert.ToInt32(invoice.TradeId));
            telHlp = Convert.ToInt64(szTel);
            szTel = telHlp.ToString(telfmt);
            telHlp = Convert.ToInt64(szFax);
            szFax = telHlp.ToString(telfmt);

            //Get the SolTo Data
            qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == invoice.CustomerId);
            if (qryAddress.Count() > 0)
            {
                soldto = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (soldto != null)
                {

                }
            }

            //Get the Bill to data
            qryBill = db.CustomersBillingDepts.Where(ctbi => ctbi.CustomerId == invoice.CustomerId);
            if (qryBill.Count() > 0)
            {
                billto = qryBill.FirstOrDefault<CustomersBillingDept>();
                if (billto != null)
                {
                }
            }

            //Get the ship to data
            qryshipto = db.CustomersShipAddresses.Where(ctsp => ctsp.Id == invoice.CustomerShiptoId);
            if (qryshipto.Count() > 0)
            {
                shipto = qryshipto.FirstOrDefault<CustomersShipAddress>();
                if (shipto != null)
                {
                }
            }

            //Get the Vendor address data
            qryVenAddres = db.VendorsContactAddresses.Where(vnad => vnad.VendorId == invoice.VendorId);
            if (qryVenAddres.Count() > 0)
            {
                venaddress = qryVenAddres.FirstOrDefault<VendorsContactAddress>();
                if (venaddress != null)
                {
                }
            }

            //Set the address
            szSoldto01 = string.Format("{0} {1}", soldto.FirstName, soldto.LastName);
            szSoldto02 = string.Format("{0}", soldto.CompanyName);
            szSoldto03 = string.Format("{0}", soldto.Address);
            szSoldto04 = string.Format("{0}, {1} {2}", soldto.City, soldto.State, soldto.Zip);
            if (string.IsNullOrEmpty(soldto.Tel))
            {
                szSoldto07 = "0";
            }
            else
            {
                szSoldto07 = soldto.Tel;
            }
            if (string.IsNullOrEmpty(soldto.Fax))
            {
                szSoldto08 = "0";
            }
            else
            {
                szSoldto08 = soldto.Fax;
            }

            telHlp = Convert.ToInt64(szSoldto07);
            szSoldto07 = telHlp.ToString(telfmt);
            telHlp = Convert.ToInt64(szSoldto08);
            szSoldto08 = telHlp.ToString(telfmt);

            if (invoice.IsBlindShip)
            {

            }
            else
            {
                //szFirstName = szTradeName;
                //szAddressHlp2 = szAddress;
                //szAddressHlp3 = string.Format("{0} {1} {2}", szCity, szState, szZip);

                //Ship to data
                //szFirstName2 = shipto.FirstName;
                //szLastName2 = shipto.LastName;
                //szCompany2 = soldto.CompanyName;
                //szAddressHlp4 = string.Format("{0}", shipto.Address1);
                //szAddressHlp5 = string.Format("{0} {1} {2}", shipto.City, shipto.State, shipto.Zip);
                if (shipto != null)
                {
                    szShipTo01 = string.Format("{0} {1}", shipto.FirstName, shipto.LastName);
                    szShipTo02 = string.Format("{0}", soldto.CompanyName);
                    szShipTo03 = string.Format("{0}", shipto.Address1);
                    szShipTo04 = string.Format("{0} {1} {2}", shipto.City, shipto.State, shipto.Zip);

                    if (string.IsNullOrEmpty(shipto.Tel))
                    {
                        szShipTo07 = "0";
                    }
                    else
                    {
                        szShipTo07 = shipto.Tel;
                    }
                    if (string.IsNullOrEmpty(shipto.Fax))
                    {
                        szShipTo08 = "0";
                    }
                    else
                    {
                        szShipTo08 = shipto.Fax;
                    }


                    telHlp = Convert.ToInt64(szShipTo07);
                    szShipTo07 = telHlp.ToString(telfmt);
                    telHlp = Convert.ToInt64(szShipTo08);
                    szShipTo08 = telHlp.ToString(telfmt);

                }
                else
                {
                    szShipTo01 = string.Format("{0} {1}", string.Empty, string.Empty);
                    szShipTo02 = string.Format("{0}", string.Empty);
                    szShipTo03 = string.Format("{0}", string.Empty);
                    szShipTo04 = string.Format("{0} {1} {2}", string.Empty, string.Empty, string.Empty);
                    szShipTo07 = string.Format("{0}", string.Empty);
                    szShipTo08 = string.Format("{0}", string.Empty);
                }
            }


            //Create the table for the headr
            var table = new PdfGrid(numColumns: 2);
            table.SetTotalWidth(new float[] { 372.5f, 180.5f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 7;


            //Paragraph title = new Paragraph("I N V O I C E", times);

            //List al available fonts in the sistem
            //foreach (var item in FontFactory.RegisteredFonts)
            //{

            //}

            // Variety of ways to use the GetFont() method:
            //Font arial = FontFactory.GetFont("Arial", 28, BaseColor.GRAY);
            //Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLDITALIC, new BaseColor(125, 88, 15));
            //Font palatino = FontFactory.GetFont(
            //     "palatino linotype italique",
            //      BaseFont.CP1252,
            //      BaseFont.EMBEDDED,
            //      10,
            //      Font.ITALIC,
            //      BaseColor.GREEN
            //      );
            //Font smallfont = FontFactory.GetFont("Arial", 7);
            //Font xFont = FontFactory.GetFont("nina fett");
            //xFont.Size = 10;
            //xFont.SetStyle("Italic");
            //xFont.SetColor(100, 50, 200);

            //var title = header.PdfFont.FontSelector.Process("I N V O I C E");
            //Font times = FontFactory.GetFont("helvetica-bold");
            //times.Size = 16;
            //times.SetStyle("Italic");
            //times.SetColor(0, 0, 0);

            PdfPCell hlpCell = null;
            Paragraph hlpPar = null;

            BaseFont bftimes = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true);
            Font times = new Font(bftimes, 18, Font.ITALIC, BaseColor.BLACK);
            Paragraph title = new Paragraph("I N V O I C E", times);

            //Title
            var pdfCell = new PdfPCell(title);
            pdfCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            pdfCell.BorderWidthTop = 0;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.BorderWidthBottom = 0;
            pdfCell.Padding = 4;
            pdfCell.BorderColorBottom = new BaseColor(System.Drawing.Color.LightGray);
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(pdfCell);

            //Invoice and customer 
            PdfPTable nested = new PdfPTable(2);
            nested.SetTotalWidth(new float[] { 97.65f, 62.85f });
            nested.LockedWidth = true;

            Font times01 = FontFactory.GetFont("helvetica-bold", 14, BaseColor.BLACK);
            szMsg = string.Format("Invoice No.:");
            title = new Paragraph(szMsg, times01);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested.AddCell(hlpCell);

            szMsg = string.Format("{0}", szInvoiceNo);
            title = new Paragraph(szMsg, times01);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCell);

            Font times02 = FontFactory.GetFont("helvetica-bold", 12, BaseColor.BLACK);
            szMsg = string.Format("Customer No:");
            title = new Paragraph(szMsg, times02);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested.AddCell(hlpCell);


            Font times03 = FontFactory.GetFont("helvetica", 12, BaseColor.BLACK);
            szMsg = string.Format("{0}", szCustomerNo);
            title = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCell);

            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.BorderWidth = 0;
            nesthousing.Padding = 0f;
            table.AddCell(nesthousing);

            szMsg = string.Format("{0}", szTrade);
            hlpPar = new Paragraph(szMsg, times01);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCell);

            PdfPTable nested01 = new PdfPTable(4);
            nested01.SetTotalWidth(new float[] { 46.5f, 44f, 46f, 44f });
            nested01.LockedWidth = true;

            hlpPar = new Paragraph("ASI:", times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested01.AddCell(hlpCell);

            szMsg = string.Format("{0}", szASI);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested01.AddCell(hlpCell);

            hlpPar = new Paragraph("SAGE:", times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested01.AddCell(hlpCell);

            szMsg = string.Format("{0}", szSage);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested01.AddCell(hlpCell);

            PdfPCell nesthousing01 = new PdfPCell(nested01);
            nesthousing01.BorderWidth = 0;
            nesthousing01.Padding = 0f;
            table.AddCell(nesthousing01);

            //
            szMsg = string.Format("{0}", szTrade01);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCell);

            szMsg = string.Format("{0}", szTrade02);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCell);

            //
            szTrade03 = string.Format("{0}, {1} {2}", szCity, szState, szZip);
            szMsg = string.Format("{0}", szTrade03);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCell);

            szMsg = string.Format("{0}", szTrade04);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCell);

            //
            szMsg = string.Format("Tel: {0}", szTrade05);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCell);


            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCell);

            //
            szMsg = string.Format("Fax: {0}", szTrade06);
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCell);


            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times03);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCell);

            // Addresses
            Font times04 = FontFactory.GetFont("helvetica", 10, BaseColor.BLACK);
            Font times05 = FontFactory.GetFont("helvetica-bold", 10, BaseColor.BLACK);

            PdfPTable nested02 = new PdfPTable(2);
            nested02.SetTotalWidth(new float[] { 259.47f, 259.47f });
            nested02.LockedWidth = true;

            szMsg = string.Format("{0}", "Sold to");
            hlpPar = new Paragraph(szMsg, times04);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 3;
            hlpCell.PaddingLeft = 12;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 6;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested02.AddCell(hlpCell);

            szMsg = string.Format("{0}", "Ship to");
            hlpPar = new Paragraph(szMsg, times04);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 3;
            hlpCell.PaddingLeft = 8;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 3;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested02.AddCell(hlpCell);

            PdfPCell nesthousing02 = new PdfPCell(nested02);
            nesthousing02.Colspan = 2;
            nesthousing02.BorderWidth = 0;
            nesthousing02.Padding = 0f;
            table.AddCell(nesthousing02);

            //Sold  table
            PdfPTable soldtotbl = new PdfPTable(numColumns: 3);
            soldtotbl.SetTotalWidth(new float[] { 30.1f, 113f, 98f });
            soldtotbl.LockedWidth = true;

            szMsg = string.Format("{0}", "Attn:");
            hlpPar = new Paragraph(szMsg, times04);
            PdfPCell hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0.5f;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto01);
            hlpPar = new Paragraph(szMsg, times05);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.Colspan = 2;
            hlpslto.BorderWidthTop = 0.5f;
            hlpslto.BorderWidthLeft = 0;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto02);
            hlpPar = new Paragraph(szMsg, times05);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.Colspan = 2;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0f;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto03);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.Colspan = 2;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0f;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto04);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.Colspan = 2;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0f;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto05);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("{0}", szSoldto06);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.Colspan = 2;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0f;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 1;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);


            szMsg = string.Format("Tel: {0}", szSoldto07);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0.5f;
            hlpslto.BorderWidthRight = 0;
            hlpslto.BorderWidthBottom = 0.5f;
            hlpslto.Colspan = 2;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 8;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 3;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            szMsg = string.Format("Fax: {0}", szSoldto08);
            hlpPar = new Paragraph(szMsg, times04);
            hlpslto = new PdfPCell(hlpPar);
            hlpslto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpslto.BorderWidthTop = 0;
            hlpslto.BorderWidthLeft = 0f;
            hlpslto.BorderWidthRight = 0.5f;
            hlpslto.BorderWidthBottom = 0.5f;
            hlpslto.PaddingTop = 1;
            hlpslto.PaddingLeft = 1;
            hlpslto.PaddingRight = 1;
            hlpslto.PaddingBottom = 3;
            hlpslto.HorizontalAlignment = Element.ALIGN_LEFT;
            soldtotbl.AddCell(hlpslto);

            //Ship to table
            PdfPTable shiptotbl = new PdfPTable(numColumns: 3);
            shiptotbl.SetTotalWidth(new float[] { 30.1f, 113f, 98f });
            shiptotbl.LockedWidth = true;

            szMsg = string.Format("{0}", "Attn:");
            hlpPar = new Paragraph(szMsg, times04);
            PdfPCell hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0.5f;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo01);
            hlpPar = new Paragraph(szMsg, times05);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.Colspan = 2;
            hlpspto.BorderWidthTop = 0.5f;
            hlpspto.BorderWidthLeft = 0;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo02);
            hlpPar = new Paragraph(szMsg, times05);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.Colspan = 2;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0f;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo03);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.Colspan = 2;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0f;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo04);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.Colspan = 2;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0f;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo05);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("{0}", szShipTo06);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.Colspan = 2;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0f;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 1;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);


            szMsg = string.Format("Tel: {0}", szShipTo07);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0.5f;
            hlpspto.BorderWidthRight = 0;
            hlpspto.BorderWidthBottom = 0.5f;
            hlpspto.Colspan = 2;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 8;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 3;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);

            szMsg = string.Format("Fax: {0}", szShipTo08);
            hlpPar = new Paragraph(szMsg, times04);
            hlpspto = new PdfPCell(hlpPar);
            hlpspto.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpspto.BorderWidthTop = 0;
            hlpspto.BorderWidthLeft = 0f;
            hlpspto.BorderWidthRight = 0.5f;
            hlpspto.BorderWidthBottom = 0.5f;
            hlpspto.PaddingTop = 1;
            hlpspto.PaddingLeft = 1;
            hlpspto.PaddingRight = 1;
            hlpspto.PaddingBottom = 3;
            hlpspto.HorizontalAlignment = Element.ALIGN_LEFT;
            shiptotbl.AddCell(hlpspto);


            //The Address
            PdfPTable nested03 = new PdfPTable(2);
            nested03.SetTotalWidth(new float[] { 259.47f, 259.47f });
            nested03.LockedWidth = true;

            szMsg = string.Format("{0}", "Sold tox");
            hlpPar = new Paragraph(szMsg, times04);
            //hlpCell = new PdfPCell(hlpPar);

            hlpCell = new PdfPCell(soldtotbl);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 0;
            hlpCell.PaddingLeft = 8;
            hlpCell.PaddingRight = 0;
            hlpCell.PaddingBottom = 0;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested03.AddCell(hlpCell);

            szMsg = string.Format("{0}", "Ship tox");
            hlpPar = new Paragraph(szMsg, times04);
            //hlpCell = new PdfPCell(hlpPar);

            hlpCell = new PdfPCell(shiptotbl);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested03.AddCell(hlpCell);

            PdfPCell nesthousing03 = new PdfPCell(nested03);
            nesthousing03.Colspan = 2;
            nesthousing03.BorderWidth = 0;
            nesthousing03.Padding = 0f;
            table.AddCell(nesthousing03);

            //Invoice data
            szMsg = string.Format("{0}", "Invoice Data");
            hlpPar = new Paragraph(szMsg, times04);
            hlpCell = new PdfPCell(hlpPar);

            PdfPTable invoicedatatbl = GetInvoiceData(times04, times05, invoice);
            hlpCell = new PdfPCell(invoicedatatbl);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 3;
            hlpCell.PaddingLeft = 0;
            hlpCell.PaddingRight = 0;
            hlpCell.PaddingBottom = 0;
            hlpCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell nesthousing04 = new PdfPCell(hlpCell);
            nesthousing04.Colspan = 2;
            nesthousing04.BorderWidth = 0;
            hlpCell.Padding = 0f;
            table.AddCell(nesthousing04);

            return table;
        }

        private PdfPTable GetInvoiceData(Font times04, Font times05, Invoice invoice)
        {
            string szMsg = "";
            string szData01 = "09/20/2013";
            string szData02 = "UPS GROUND";
            string szData03 = " ";
            string szData04 = "PAID BY VISA";
            string szData05 = "5586";
            string szData06 = "9/10/2013";
            string szData07 = "AD";
            string szData08 = "99067";
            DateTime dDate = DateTime.Now;

            PurchaseOrders purchaseorder = db.PurchaseOrders.Where(pror => pror.PurchaseOrderNo == invoice.PurchaseOrderNo).FirstOrDefault<PurchaseOrders>();
            if (purchaseorder == null)
            {
                szData06 = string.Empty;
            }

            PdfPTable invoicedatatbl = new PdfPTable(numColumns: 4);
            invoicedatatbl.SetTotalWidth(new float[] { 138.105f, 138.105f, 138.105f, 138.105f });
            invoicedatatbl.LockedWidth = true;

            Paragraph hlpPar = null;
            PdfPCell hlpinv = null;

            //First Row
            szMsg = string.Format("{0}", "Date:");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Ship Via");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Tracking No.");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Terms");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            //Second Row
            dDate = Convert.ToDateTime(invoice.InvoiceDate);
            szData01 = dDate.ToString("MM/dd/yyyy");
            szMsg = string.Format("{0}", szData01);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szData02 = invoice.ShipVia;
            szMsg = string.Format("{0}", szData02);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szData03 = invoice.TrackingNo;
            szMsg = string.Format("{0}", szData03);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szData04 = invoice.PaymentTerms;
            szMsg = string.Format("{0}", szData04);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            //Third Row
            szMsg = string.Format("{0}", "P/O No.");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Order Date");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Sales Rep.");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szMsg = string.Format("{0}", "Sales Order No.");
            hlpPar = new Paragraph(szMsg, times05);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0f;
            hlpinv.BorderWidthBottom = 0f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            //Fourth Row
            if (string.IsNullOrEmpty(invoice.PurchaseOrderNo))
            {
                szData06 = string.Empty;
            }
            else
            {
                dDate = Convert.ToDateTime(purchaseorder.PODate);
                szData06 = dDate.ToString("MM/dd/yyyy");
            }
            szData05 = invoice.PurchaseOrderNo;
            szMsg = string.Format("{0}", szData05);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0.5f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);


            szMsg = string.Format("{0}", szData06);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0.5f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szData07 = invoice.SalesRep;
            szMsg = string.Format("{0}", szData07);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0.5f;
            hlpinv.BorderWidthBottom = 0.5f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            szData08 = invoice.SalesOrderNo;
            szMsg = string.Format("{0}", szData08);
            hlpPar = new Paragraph(szMsg, times04);
            hlpinv = new PdfPCell(hlpPar);
            hlpinv.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpinv.BorderWidthTop = 0.5f;
            hlpinv.BorderWidthLeft = 0f;
            hlpinv.BorderWidthRight = 0f;
            hlpinv.BorderWidthBottom = 0.5f;
            hlpinv.PaddingTop = 1;
            hlpinv.PaddingLeft = 1;
            hlpinv.PaddingRight = 1;
            hlpinv.PaddingBottom = 3;
            hlpinv.HorizontalAlignment = Element.ALIGN_CENTER;
            invoicedatatbl.AddCell(hlpinv);

            return invoicedatatbl;
        }


    }
}