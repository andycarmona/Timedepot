using System;
using System.Collections.Generic;
using PdfReportSamples.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.AccountingBalanceColumn
{
    public class AccountingBalanceColumnPdfReport
    {
        public IPdfReportData CreatePdfReport()
        {
            // Balance of each row = Balance of the previous row + current row's credit - current row's debit
            long lastBalance = 0;

            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
             .DefaultFonts(fonts =>
             {
                 fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"),
                            System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\tahoma.ttf"));
                 fonts.Size(9);
                 fonts.Color(System.Drawing.Color.Black);
             })
             .PagesFooter(footer =>
             {
                 footer.DefaultFooter(printDate: DateTime.Now.ToString("MM/dd/yyyy"));
             })
             .PagesHeader(header =>
             {
                 header.DefaultHeader(defaultHeader =>
                 {
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                     defaultHeader.ImagePath(System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png"));
                     defaultHeader.Message(
                         "Accounting Rpt.\n" +
                         "Balance of each row = Balance of the previous row + current row's credit - current row's debit");
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<Payment>();
                 for (int i = 1; i < 20; i++)
                 {
                     listOfRows.Add(new Payment
                     {
                         Id = i,
                         Credit = (i % 2 == 0) ? (1000 * i) : 0,
                         Debit = (i % 2 == 0) ? 0 : (2000 * i),
                         UnitPrice = 3000 + i
                     });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Summary");
                 summary.PreviousPageSummarySettings("Previous Page Summary");
                 summary.PageSummarySettings("Page Summary");
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
                     column.PropertyName<Payment>(x => x.UnitPrice);
                     column.HeaderCell("UnitPrice ($)");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Payment>(x => x.Credit);
                     column.HeaderCell("Credit ($)");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Payment>(x => x.Debit);
                     column.HeaderCell("Debit ($)");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                 });


                 columns.AddColumn(column =>
                 {
                     column.PropertyName("C.F.1");
                     column.HeaderCell("Balance ($)");
                     column.Width(3);
                     column.AggregateFunction(aggregateFunction =>
                     {
                         aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                         aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.CalculatedField(
                         list =>
                         {
                             if (list == null) return string.Empty;
                             var currentRowCredit = (long)list.GetValueOf<Payment>(x => x.Credit);
                             var currentRowDebit = (long)list.GetValueOf<Payment>(x => x.Debit);
                             lastBalance = lastBalance + currentRowCredit - currentRowDebit;
                             return lastBalance;
                         });
                     column.IsVisible(true);
                     column.Order(4);
                 });
             })
             .Export(export =>
             {
                 export.ToExcel();
             })
             .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\AccountingBalanceSample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
        }
    }
}
