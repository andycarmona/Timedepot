﻿using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfReportSamples.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.InlineProviders
{
    public class InlineProvidersPdfReport
    {
        public IPdfReportData CreatePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata
                {
                    Author = "Vahid",
                    Application = "PdfRpt",
                    Keywords = "IList Rpt.",
                    Subject = "Test Rpt",
                    Title = "Test"
                });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
            .DefaultFonts(fonts =>
            {
                fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesFooter(footer =>
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
            })
            .PagesHeader(header =>
            {
                header.InlineHeader(inlineHeader =>
                {
                    inlineHeader.AddPageHeader(data =>
                    {
                        return createHeader(header);
                    });
                });
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.ClassicTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<User>();
                for (int i = 0; i < 200; i++)
                {
                    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                }
                dataSource.StronglyTypedList(listOfRows);
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("rowNo");
                    column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.HeaderCell("#");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Id);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Id");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Name);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.LastName);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Last Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Balance);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Balance");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                    });
                    column.AggregateFunction(aggregateFunction =>
                    {
                        aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                        aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Id);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("QRCode");
                    column.ColumnItemsTemplate(itemsTemplate =>
                    {
                        itemsTemplate.InlineField(inlineField =>
                        {
                            inlineField.RenderCell(cellData =>
                            {
                                var data = cellData.Attributes.RowData.TableRowData;
                                var id = data.GetSafeStringValueOf<User>(x => x.Id);

                                var qrcode = new BarcodeQRCode(id, 1, 1, null);
                                var image = qrcode.GetImage();
                                var mask = qrcode.GetImage();
                                mask.MakeMask();
                                image.ImageMask = mask; // making the background color transparent
                                var pdfCell = new PdfPCell(image, fit: false);

                                return pdfCell;
                            });
                        });
                    });
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Export(export =>
            {
                export.ToExcel();
            })
            .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\InlineProvidersPdfReport-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))), debugMode: true);
        }

        private static PdfGrid createHeader(PagesHeaderBuilder header)
        {
            var table = new PdfGrid(numColumns: 1)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                SpacingAfter = 7
            };

            var title = header.PdfFont.FontSelector.Process("Our new rpt.");
            var pdfCell = new PdfPCell(title)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 0,
                BorderWidthBottom = 1,
                Padding = 4,
                BorderColorBottom = new BaseColor(System.Drawing.Color.LightGray),
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            table.AddCell(pdfCell);
            return table;
        }

        private static PdfGrid createFooter(PagesFooterBuilder footer, string date, FooterData data)
        {
            var table = new PdfGrid(numColumns: 4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR
            };

            var datePhrase = footer.PdfFont.FontSelector.Process(date);
            var datePdfCell = new PdfPCell(datePhrase)
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
    }
}
