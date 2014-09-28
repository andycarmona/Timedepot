using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfReportSamples.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using PdfReportSamples;

namespace PDFMVCTest.TimelyDepotReports
{
    public class TDTestReport
    {
        public IPdfReportData CreatePdfReport()
        {
            var report = new PdfReport();

            report.DocumentPreferences(doc =>
                {
                    doc.RunDirection(PdfRunDirection.LeftToRight);
                    doc.Orientation(PageOrientation.Portrait);
                    doc.PageSize(PdfPageSize.A4);
                    doc.DocumentMetadata(new DocumentMetadata { Author = "Vios", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Test Rpt", Title = "Test" });
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

            report.DefaultFonts(fonts =>
            {
                fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);

            });

            report.PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            });

            report.PagesHeader(header =>
            {
                header.DefaultHeader(defaultHeader =>
                {
                    defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                    defaultHeader.ImagePath(System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png"));
                    defaultHeader.Message("Our new rpt.");
                });
            });



            report.MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.ClassicTemplate);
            });

            report.MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.NumberOfDataRowsPerPage(15);
            });

            report.MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<User>();
                for (int i = 0; i < 200; i++)
                {
                    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                }
                dataSource.StronglyTypedList(listOfRows);

            });

            report.MainTableSummarySettings(summarySettings => 
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");

            });

            report.MainTableColumns(columns => 
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
                    column.HeaderCell("Name", horizontalAlignment: HorizontalAlignment.Left);
                    column.Font(font =>
                    {
                        font.Size(10);
                        font.Color(System.Drawing.Color.Brown);
                    });
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

            });

            report.MainTableEvents(events => 
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            });

            report.Export(export => 
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            });

            var reportHlp = report.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\RptTDSample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));

            //return report as IPdfReportData;
            return reportHlp;
        }
    }
}