﻿using System;
using System.Collections.Generic;
using PdfReportSamples.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.QuestionsAcroForm
{
    public class QuestionsAcroFormPdfReport
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
                 footer.DefaultFooter(printDate: DateTime.Now.ToString("MM/dd/yyyy"));
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
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<Question>();
                 for (int i = 1; i <= 20; i++)
                 {
                     listOfRows.Add(new Question
                     {
                         Id = i,
                         QuestionText = "A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. " + i,
                         Answer1 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         Answer2 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         Answer3 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         Answer4 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         PicturePath = System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png")
                     });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableColumns(columns =>
             {
                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Question>(x => x.Id);
                     column.HeaderCell(caption: "Questions");
                     column.Width(1);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.ColumnItemsTemplate(template =>
                     {
                         template.PdfTemplate(
                                 pdfTemplatePath: System.IO.Path.Combine(AppPath.ApplicationPath, "data\\questionTemplate.pdf"),
                                 onFillAcroForm: (data, form, pdfStamper) =>
                                 {
                                     var id = data.GetSafeStringValueOf<Question>(x => x.Id);
                                     form.SetField("txtNumber", id);

                                     var questionText = data.GetSafeStringValueOf<Question>(x => x.QuestionText);
                                     form.SetField("txtQuestion", questionText);

                                     var answer1 = data.GetSafeStringValueOf<Question>(x => x.Answer1);
                                     form.SetField("txtItem1", answer1);

                                     var answer2 = data.GetSafeStringValueOf<Question>(x => x.Answer2);
                                     form.SetField("txtItem2", answer2);

                                     var answer3 = data.GetSafeStringValueOf<Question>(x => x.Answer3);
                                     form.SetField("txtItem3", answer3);

                                     var answer4 = data.GetSafeStringValueOf<Question>(x => x.Answer4);
                                     form.SetField("txtItem4", answer4);

                                     //Create a standard iTextSharp image and scale it to the form field's dimensions and place it where the field is
                                     var picturePath = data.GetSafeStringValueOf<Question>(x => x.PicturePath);
                                     var rect = form.GetFieldPositions("txtImage")[0].position;
                                     var img = PdfImageHelper.GetITextSharpImageFromImageFile(picturePath);
                                     //Scale it
                                     img.ScaleAbsolute(rect.Width, rect.Height);
                                     //Position it
                                     img.SetAbsolutePosition(rect.Left, rect.Bottom);
                                     //Add it to page 1 of the document
                                     pdfStamper.GetOverContent(1).AddImage(img);
                                 });
                     });
                 });
             })
             .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\QuestionsAcroRpt-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))), debugMode: true);
        }
    }
}
