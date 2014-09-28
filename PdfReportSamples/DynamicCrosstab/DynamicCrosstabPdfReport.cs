﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PdfReportSamples.Models;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.DataSources;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.DynamicCrosstab
{
    public class DynamicCrosstabPdfReport
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
                 fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\tahoma.ttf"),
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
                     defaultHeader.ImagePath(System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png"));
                     defaultHeader.Message("Students Rpt.");
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
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
                 var result = new List<StudentStat>();
                 var rnd = new Random();

                 for (int day = 1; day < 31; day++)
                 {
                     for (int student = 1; student < 21; student++)
                     {
                         result.Add(new StudentStat
                         {
                             Id = student,
                             Date = new DateTime(2011, 11, day),
                             IsPresent = rnd.Next(-1, 1) == 0 ? true : false,
                             Name = "Student " + student
                         });
                     }
                 }

                 var list = result.Where(x => x.Date.Day > 13 && x.Date.Day < 19);
                 var crossTabList = list.Pivot(x =>
                                                  new
                                                  {
                                                      x.Id,
                                                      x.Name
                                                  },
                                                  x1 => "Day " + x1.Date.Day,
                                                  x2 => x2.First().IsPresent,
                                                  x3 =>
                                                  new
                                                  {
                                                      IsPresent = x3.Count(x4 => x4.IsPresent),
                                                      IsNotPresent = x3.Count(x4 => !x4.IsPresent)
                                                  });
                 dataSource.Crosstab(crossTabList);
             })
             .MainTableEvents(events =>
             {
                 // How to change the captions of the HeaderRow
                 events.RowStarted(args =>
                     {
                         if (args.RowType == RowType.HeaderRow)
                         {
                             foreach (var item in args.PdfColumnsAttributes)
                             {
                                 switch (item.HeaderCell.Caption)
                                 {
                                     case "IsPresent":
                                         item.HeaderCell.Caption = "Present";
                                         break;

                                     case "IsNotPresent":
                                         item.HeaderCell.Caption = "Absent";
                                         break;
                                 }
                             }
                         }
                     });

                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableAdHocColumnsConventions(adHocColumnsConventions =>
             {
                 adHocColumnsConventions.ShowRowNumberColumn(true);
                 adHocColumnsConventions.RowNumberColumnCaption("#");
                 adHocColumnsConventions.AddTypeColumnItemsTemplate(
                     typeof(bool),
                     new CheckmarkField { CheckmarkFillColor = Color.Green, CrossSignFillColor = Color.DarkRed }
                     );
             })
             .Export(export =>
             {
                 export.ToExcel();
                 export.ToXml();
             })
             .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\DynamicCrosstabSampleRpt-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
        }
    }
}
