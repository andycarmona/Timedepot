﻿using System;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text.pdf;
using PdfReportSamples.Models;
using PdfRpt;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.ProgressReport
{
    public class ProgressReportPdfReport
    {
        private IPdfFont getWatermarkFont()
        {
            var watermarkFont = new GenericFontProvider(
                                        System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                                        System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
            watermarkFont.Color = new GrayColor(0.75f);
            watermarkFont.Size = 50;
            return watermarkFont;
        }

        public IPdfReportData CreatePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.DiagonalWatermark(new DiagonalWatermark
                {
                    Text = "Diagonal Watermark",
                    RunDirection = PdfRunDirection.LeftToRight,
                    Font = getWatermarkFont(),
                    FillOpacity = 0.6f,
                    StrokeOpacity = 1
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
                template.BasicTemplate(BasicTemplate.SilverTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.MultipleColumnsPerPage(new MultipleColumnsPerPage
                {
                    ColumnsGap = 20,
                    ColumnsPerPage = 2,
                    ColumnsWidth = 250,
                    IsRightToLeft = false,
                    TopMargin = 7
                });
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<Task>();
                var rnd = new Random();
                for (int i = 0; i < 410; i++)
                {
                    listOfRows.Add(new Task
                    {
                        Id = rnd.Next(1000, 10000),
                        Name = "Task" + i,
                        PercentCompleted = rnd.Next(1, 100),
                        IsActive = rnd.Next(0, 2) == 1 ? true : false
                    });
                }
                dataSource.StronglyTypedList<Task>(listOfRows);
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
                    column.HeaderCell("#", captionRotation: 90);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.Id);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Id");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.Name);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.PercentCompleted);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Percent Completed");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.ProgressBar(progressBarColor: Color.SkyBlue, showPercentText: true);
                        template.DisplayFormatFormula(obj =>
                        {
                            if (obj == null) return "% 0";
                            return "% " + obj.ToString();
                        });
                    });
                    column.Font(font =>
                    {
                        font.Size(9);
                        font.Color(Color.Brown);
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.IsActive);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Active");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.Checkmark(checkmarkFillColor: Color.Green, crossSignFillColor: Color.DarkRed);
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
            .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\ProgressReportSample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
        }
    }
}
