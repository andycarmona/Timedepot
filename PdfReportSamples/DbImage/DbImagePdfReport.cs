﻿using System;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.DbImage
{
    public class DbImagePdfReport
    {
        public IPdfReportData CreatePdfReport()
        {
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
                 fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                            System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                 fonts.Size(9);
                 fonts.Color(System.Drawing.Color.Black);
             })
             .PagesFooter(footer =>
             {
                 footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
             })
             .PagesHeader(header =>
             {
                 header.DefaultHeader(defaultHeader =>
                 {
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                     defaultHeader.ImagePath(System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png"));
                     defaultHeader.Message("Our new rpt.");
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.AutumnTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.GenericDataReader(
                   providerName: "System.Data.SQLite",
                   connectionString: "Data Source=" + System.IO.Path.Combine(AppPath.ApplicationPath, "data\\blogs.sqlite"),
                   sql: @"SELECT [url], [name], [NumberOfPosts], [AddDate], [thumbnail]
                               FROM [tblBlogs]
                               WHERE [NumberOfPosts]>=@p1",
                   parametersValues: new object[] { 10 }
               );
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
                     column.HeaderCell("#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("url");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.HeaderCell("Url");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("name");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.HeaderCell("Name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("NumberOfPosts");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.HeaderCell("Posts");
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
                     column.PropertyName("AddDate");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.HeaderCell("Add Date");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null ? string.Empty : ((DateTime)obj).ToString("dd/MM/yyyy HH:mm"));
                     });
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("thumbnail");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(5);
                     column.HeaderCell("Thumbnail");
                     column.ColumnItemsTemplate(t => t.ByteArrayImage(defaultImageFilePath: string.Empty, fitImages: false));
                 });
             })
             .Export(export =>
             {
                 export.ToXml();
                 export.ToExcel();
             })
             .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\RptDbImageSample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
        }
    }
}
