using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iTextSharp.text;
using iTextSharp.text.pdf;

using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using System.IO;
using TimelyDepotMVC.PDFReporting;

namespace TimelyDepotMVC.Controllers.PDFReporting
{
    public class TableHeader : IPdfPageEvent
    {
        string _header = "";
        PdfTemplate total = null;

        /// <summary>
        /// Allows us to change the content of the header.
        /// </summary>
        /// <param name="header"></param>
        public void setHeader(string header)
        {
            _header = header;
        }

        public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
        {
            throw new NotImplementedException("NotImplementedException: OnChapter");
        }

        public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            throw new NotImplementedException("NotImplementedException: OnChapterEnd");
        }

        /// <summary>
        /// Fills out the total number of pages before the document is closed
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public void OnCloseDocument(PdfWriter writer, Document document)
        {
            string szMsg = "";
            int nPage = writer.PageNumber - 1;
            szMsg = string.Format("{0}", nPage.ToString());
            Phrase phrase = new Phrase(szMsg);
            ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, phrase, 2, 2, 0);

            //ColumnText.showTextAligned(total, Element.ALIGN_LEFT,
            //        new Phrase(String.valueOf(writer.getPageNumber() - 1)),
            //        2, 2, 0);
        }

        /// <summary>
        /// Adds a header to every page
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable table = null;
            try
            {
                table = new PdfPTable(3);
                table.SetWidths(new int[] { 24, 24, 2 });
                table.TotalWidth = 527;
                table.LockedWidth = true;
                table.DefaultCell.FixedHeight = 20;
                table.DefaultCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                table.AddCell(_header);

                table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(string.Format("Page {0} of", writer.PageNumber.ToString()));

                PdfPCell cell = new PdfPCell(iTextSharp.text.Image.GetInstance(total));
                cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, 34, 34, writer.DirectContent);
            }
            catch (DocumentException de)
            {
                throw new Exception(de.Message, de.InnerException);
            }
        }

        public void OnGenericTag(PdfWriter writer, Document document, iTextSharp.text.Rectangle rect, string text)
        {
            throw new NotImplementedException("NotImplementedException: OnGenericTag");
        }

        /// <summary>
        /// Creates the PdfTemplate that will hold the total number of 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(30, 16);
        }

        public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
            throw new NotImplementedException("NotImplementedException: OnParagraph");
        }

        public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            throw new NotImplementedException("NotImplementedException: OnParagraphEnd");
        }

        public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
        {
            throw new NotImplementedException("NotImplementedException: OnSection");
        }

        public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            throw new NotImplementedException("NotImplementedException: OnSectionEnd");
        }

        public void OnStartPage(PdfWriter writer, Document document)
        {
            //throw new NotImplementedException("NotImplementedException: OnStartPage");
        }
    }

    public class ITextReportsController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /ITextReports/SalesOrderReport
        public ActionResult SalesOrderReport(int SalesOrderId = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data
            //Get the first invoice
            if (SalesOrderId == 0)
            {
                SalesOrder invoiceHlp = db.SalesOrders.FirstOrDefault<SalesOrder>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                SalesOrderId = invoiceHlp.SalesOrderId;
            }

            //Get and verify resource for the Report
            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //The fonts 
            //Default font Helvetica 12 pt can not be changed use factory class to produce object with the required font
            iTextSharp.text.Font timesHB_I_18 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12 = null;
            iTextSharp.text.Font timesHB_N_10 = null;
            iTextSharp.text.Font timesHB_I_10 = null;
            iTextSharp.text.Font timesHB_I_10_U = null;

            iTextSharp.text.Font timesH_N_12 = null;
            iTextSharp.text.Font timesH_N_10 = null;
            iTextSharp.text.Font timesH_I_10 = null;
            iTextSharp.text.Font timesH_N_10_U = null;
            iTextSharp.text.Font timesH_N_8 = null;

            iTextSharp.text.Font timesArial_N_18 = null;

            timesHB_I_18 = CreateFont("HELVETICA_BOLD", 18f, iTextSharp.text.Font.ITALIC);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12 = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesHB_I_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.ITALIC);
            timesHB_I_10_U = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_12 = CreateFont("HELVETICA", 12f, iTextSharp.text.Font.NORMAL);
            timesH_N_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_I_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.ITALIC);
            timesH_N_10_U = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_8 = CreateFont("HELVETICA", 8f, iTextSharp.text.Font.NORMAL);
            timesArial_N_18 = CreateFont("Arial.ttf", 18f, iTextSharp.text.Font.NORMAL);

            //Underline the required fonts
            timesHB_I_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);
            timesH_N_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create in memory the PDF document, to allow a second pass and add the page number footer
            //MemoryStream baos = new MemoryStream();


            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {
                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                //writer = PdfWriter.GetInstance(doc, baos);
                //writer.CloseStream = false;

                writer.PageEvent = new TableHeader();

                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //
                PdfPTable table = GetTableSalesOrder(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_12, timesH_N_10, timesH_I_10, timesH_N_10_U, timesH_N_8, SalesOrderId);
                doc.Add(table);

                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }

            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }

        private PdfPTable GetTableSalesOrder(iTextSharp.text.Font timesHB_I_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesHB_I_10, iTextSharp.text.Font timesHB_I_10_U, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_I_10, iTextSharp.text.Font timesH_N_10_U, iTextSharp.text.Font timesH_N_8, int SalesOrderId)
        {
            double dHlp = 0;
            double dQty = 0;
            double dAmount = 0;
            string szMsg = "";

            IQueryable<SalesOrderDetail> qryPrice = db.SalesOrderDetails.Where(prc => prc.SalesOrderId == SalesOrderId).OrderBy(prc => prc.ItemPosition).ThenBy(prc => prc.ItemOrder);

            Phrase phrase = null;
            PdfPCell cell = null;

            TableHeader tableheader = new TableHeader();
            //tableheader.setHeader("Hola Vios");

            PdfPTable table = new PdfPTable(numColumns: 5);
            table.SetTotalWidth(new float[] { 119.54f, 44.83f, 235.56f, 59.77f, 63.3f });
            table.LockedWidth = true;

            // Add the first header row
            PdfPTable headertbl = CreateSalesOrderHeader(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesH_N_12, timesH_N_10, timesH_N_8, SalesOrderId);
            if (headertbl != null)
            {
                cell = new PdfPCell(headertbl);
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                table.AddCell(cell);

            }
            // Add the second header row 

            szMsg = "ItemID No.";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Quantity";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Description";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            //szMsg = "Tax";
            //phrase = new Phrase(szMsg, timesH_N_10);
            //cell = new PdfPCell(phrase);
            ////cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.BorderWidthTop = 0f;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0.75f;
            //cell.BorderWidthBottom = 0.75f;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //table.AddCell(cell);

            szMsg = "Unit Price";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            szMsg = "Ext. Amount";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            // Add the footer
            PdfPTable footertbl = GetSalesOrderAmountData(SalesOrderId, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_10, timesH_N_10_U, timesH_I_10, timesH_N_8);
            if (footertbl != null)
            {
                cell = new PdfPCell(footertbl);
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                table.AddCell(cell);

            }
            // There are three special rows
            table.HeaderRows = 3;
            // One of them is a footer
            table.FooterRows = 1;

            // Now let's loop over the data
            if (qryPrice.Count() > 0)
            {
                foreach (var item in qryPrice)
                {

                    szMsg = item.Sub_ItemID;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.Quantity);
                    dQty = dHlp;
                    szMsg = dHlp.ToString("N0");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    szMsg = item.Description;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    //dHlp = Convert.ToDouble(item.Tax);
                    //szMsg = dHlp.ToString("N2");
                    //phrase = new Phrase(szMsg, timesH_N_10);
                    //cell = new PdfPCell(phrase);
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BorderWidthTop = 0;
                    //cell.BorderWidthLeft = 0;
                    //cell.BorderWidthRight = 0.75f;
                    //cell.BorderWidthBottom = 0.75f;
                    //table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    szMsg = dHlp.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    dAmount = dQty * dHlp;
                    szMsg = dAmount.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);
                }
            }

            return table;
        }

        private PdfPTable CreateSalesOrderHeader(iTextSharp.text.Font timesHB_N_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_N_8, int SalesOrderId)
        {
            string szMsg = "";
            string szInvoiceNo = "";
            string szSoldto07 = "";
            string szSoldto08 = "";
            string szFirstName2 = "";
            string szLastName2 = "";
            string szCompany2 = "";
            string szAddressHlp4 = "";
            string szAddressHlp5 = "";
            string szAddressHlp6 = "";
            string szCountry2 = "";
            string szTel2 = "";

            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";

            //Get the sales order data
            SalesOrder invoice = null;
            invoice = db.SalesOrders.Find(SalesOrderId);
            if (invoice == null)
            {
                return null;
            }
            szInvoiceNo = invoice.SalesOrderNo;

            //Get the SolTo Data
            CustomersContactAddress soldto = null;

            IQueryable<CustomersContactAddress> qryAddress = null;

            qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == invoice.CustomerId);
            if (qryAddress.Count() > 0)
            {
                soldto = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (soldto != null)
                {
                    ViewBag.SoldTo = soldto;
                    if (string.IsNullOrEmpty(soldto.Tel))
                    {
                        szSoldto07 = "0";
                    }
                    else
                    {
                        szSoldto07 = soldto.Tel;
                    }
                    telHlp = Convert.ToInt64(szSoldto07);
                    szSoldto07 = telHlp.ToString(telfmt);
                    if (string.IsNullOrEmpty(soldto.Fax))
                    {
                        szSoldto08 = "0";
                    }
                    else
                    {
                        szSoldto08 = soldto.Fax;
                    }
                    telHlp = Convert.ToInt64(szSoldto08);
                    szSoldto08 = telHlp.ToString(telfmt);

                }
            }


            //Get the ship to data
            IQueryable<CustomersShipAddress> qryshipto = null;
            CustomersShipAddress shipto = null;
            qryshipto = db.CustomersShipAddresses.Where(ctsp => ctsp.Id == invoice.CustomerShiptoId);
            if (qryshipto.Count() > 0)
            {
                shipto = qryshipto.FirstOrDefault<CustomersShipAddress>();
                if (shipto != null)
                {
                    ViewBag.ShipTo = shipto;
                }
            }

            //Get the blind ship addres
            SalesOrderBlindShip salesblind = null;
            IQueryable<SalesOrderBlindShip> qryBlind = null;
            qryBlind = db.SalesOrderBlindShips.Where(slbd => slbd.SalesOrderId == invoice.SalesOrderId);
            if (qryBlind.Count() > 0)
            {
                salesblind = qryBlind.FirstOrDefault<SalesOrderBlindShip>();
                if (salesblind != null)
                {
                    ViewBag.BlindShip = salesblind;
                }
            }


            if (invoice.IsBlindShip)
            {

                szFirstName2 = salesblind.FirstName;
                szLastName2 = salesblind.LastName;
                szCompany2 = salesblind.Title;
                szAddressHlp4 = string.Format("{0}", salesblind.Address1, salesblind.Address2);
                szAddressHlp6 = string.Format("{0}", salesblind.Address2);
                szAddressHlp5 = string.Format("{0} {1} {2}", salesblind.City, salesblind.State, salesblind.Zip);
                szCountry2 = salesblind.Country;
                telHlp = Convert.ToInt64(salesblind.Tel);
                szTel2 = telHlp.ToString(telfmt); 
            }
            else
            {
                szFirstName2 = shipto.FirstName;
                szLastName2 = shipto.LastName;
                szCompany2 = soldto.CompanyName;
                szAddressHlp4 = string.Format("{0}", shipto.Address1);
                szAddressHlp5 = string.Format("{0} {1} {2}", shipto.City, shipto.State, shipto.Zip);
            }

            //Create the table for the header
            var table = new PdfPTable(numColumns: 2);
            table.SetTotalWidth(new float[] { 412.85f, 110.55f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 3;


            PdfPCell hlpCell = null;
            Paragraph hlpPar = null;

            Paragraph title = new Paragraph("I N V O I C E", timesHB_N_18);

            //Title
            PdfPTable titletbl = GetSalesOrderTitle(invoice, timesHB_N_18, timesHB_N_14, timesHB_N_12, timesH_N_10, timesH_N_8);
            hlpCell = new PdfPCell(titletbl);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.Colspan = 2;
            hlpCell.BorderWidth = 0;
            hlpCell.Padding = 0;
            hlpCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCell);


            // Customer No, Terms, etc
            PdfPTable customertermostbl = GetSalesOrderCustomerTerms(invoice, timesHB_N_10, timesH_N_10);
            hlpCell = new PdfPCell(customertermostbl);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.Colspan = 2;
            hlpCell.BorderWidth = 0;
            hlpCell.Padding = 0;
            hlpCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCell);

            // Addresses
            PdfPTable nested02 = new PdfPTable(2);
            nested02.SetTotalWidth(new float[] { 259.47f, 259.47f });
            nested02.LockedWidth = true;

            szMsg = string.Format("{0}", "Sold to");
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0} {1}", soldto.FirstName, soldto.LastName);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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

            szMsg = string.Format("{0}", soldto.CompanyName);
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", soldto.Address);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", soldto.Note);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}, {1} {2}", soldto.City, soldto.State, soldto.Zip);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0} {1}", szFirstName2, szLastName2);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", szCompany2);
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", szAddressHlp4);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", szAddressHlp5);
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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


            //szMsg = string.Format("Tel: {0}", szShipTo07);
            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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

            //szMsg = string.Format("Fax: {0}", szShipTo08);
            szMsg = string.Format("{0}", " ");
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
            hlpCell = new PdfPCell(hlpPar);

            PdfPTable invoicedatatbl = GetSalesOrderData(timesH_N_10, timesHB_N_10, invoice);
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
            nesthousing04.BorderWidthTop = 0;
            nesthousing04.BorderWidthLeft = 0;
            nesthousing04.BorderWidthRight = 0;
            nesthousing04.BorderWidthBottom = 1;
            hlpCell.Padding = 0f;
            table.AddCell(nesthousing04);

            return table;
        }

        private PdfPTable GetSalesOrderTitle(SalesOrder invoice, iTextSharp.text.Font timesHB_N_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_N_8)
        {
            string szMsg = "";
            string szAddress = "";
            string szAddressHlp = "";
            string szAddressHlp1 = "";
            string szAddressHlp2 = "";
            string szAddressHlp3 = "";
            string szCompany = "";
            string szAddressHlp4 = "";
            string szAddressHlp5 = "";
            string szAddressHlp6 = "";
            string szCompany2 = "";
            string szCity = "";
            string szState = "";
            string szZip = "";
            string szCountry = "";
            string szCountry2 = "";
            string szTel2 = "";
            string szTel = "";
            string szFax = "";
            string szWebSiteTrade = "";
            string szEmailTrade = "";
            string szFirstName = "";
            string szLastName = "";
            string szFirstName2 = "";
            string szLastName2 = "";
            string szTradeName = "";
            string szAsiTrade = "";
            string szSageTrade = "";
            string szPpaiTrade = "";

            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";

            TimelyDepotMVC.Controllers.SalesOrderController.GetTradeData(db, ref szAddress, ref szCity, ref szState, ref szZip, ref szCountry, ref szTel, ref szFax,
                ref szWebSiteTrade, ref szEmailTrade, ref szTradeName, ref szAsiTrade, ref szSageTrade, ref szPpaiTrade, Convert.ToInt32(invoice.TradeId));
            if (string.IsNullOrEmpty(szTel))
            {
                szTel = "0";
            }
            telHlp = Convert.ToInt64(szTel);
            szTel = telHlp.ToString(telfmt);
            if (string.IsNullOrEmpty(szFax))
            {
                szFax = "0";
            }
            telHlp = Convert.ToInt64(szFax);
            szFax = telHlp.ToString(telfmt);

            Paragraph title = null;
            PdfPCell hlpCel = null;

            PdfPTable table = new PdfPTable(numColumns: 2);
            table.SetTotalWidth(new float[] { 412.85f, 110.55f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 2;

            szMsg = string.Format("{0}", szTradeName);
            title = new Paragraph(szMsg, timesHB_N_14);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("ASI: {0}  SAGE: {1}", szAsiTrade, szSageTrade);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", szWebSiteTrade);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", szEmailTrade);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("*{0}*", invoice.SalesOrderNo);
            title = new Paragraph(szMsg, timesHB_N_18);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.Rowspan = 3;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 18;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}, {1}, {2} {3}", szAddress, szCity, szState, szZip);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("Tel: {0}        Fax: {1}", szTel, szFax);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            //table.CompleteRow();

            szMsg = string.Format("Customer Sales Order");
            title = new Paragraph(szMsg, timesHB_N_18);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);

            szMsg = string.Format("S/O No.: {0}", invoice.SalesOrderNo);
            title = new Paragraph(szMsg, timesHB_N_12);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.PaddingTop = 6;
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(hlpCel);


            return table;
        }


        private PdfPTable GetSalesOrderCustomerTerms(SalesOrder invoice, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesH_N_10)
        {
            string szMsg = "";
            string szCustomerNo = "";

            Customers customer = db.Customers.Where(cstr => cstr.Id == invoice.CustomerId).FirstOrDefault<Customers>();
            if (customer != null)
            {
                szCustomerNo = customer.CustomerNo;
            }

            Paragraph title = null;
            PdfPCell hlpCel = null;

            PdfPTable table = new PdfPTable(numColumns: 8);
            table.SetTotalWidth(new float[] { 72.38f, 58.38f, 65.38f, 65.38f, 65.38f, 65.38f, 65.38f, 65.34f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingBefore = 3;

            szMsg = string.Format("Customer No.:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", szCustomerNo);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("Terms:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", invoice.PaymentTerms);
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            szMsg = string.Format("Depto No.:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);


            szMsg = string.Format("Cases:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(hlpCel);

            szMsg = string.Format("{0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0.5f;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0.5f;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(hlpCel);

            return table;
        }

        private PdfPTable GetSalesOrderData(iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesHB_N_10, SalesOrder invoice)
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

            iTextSharp.text.Font timesHB_N_8 = null;
            iTextSharp.text.Font timesH_N_8 = null;

            timesHB_N_8 = CreateFont("HELVETICA_BOLD", 8f, iTextSharp.text.Font.NORMAL);
            timesH_N_8 = CreateFont("HELVETICA", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable invoicedatatbl = new PdfPTable(numColumns: 8);
            invoicedatatbl.SetTotalWidth(new float[] { 47.31f, 87.24f, 46.04f, 87.73f, 47.31f, 86.47f, 60.45f, 60.45f });
            invoicedatatbl.LockedWidth = true;

            Paragraph hlpPar = null;
            PdfPCell hlpinv = null;

            //First Row
            szMsg = string.Format("{0}", "S/O Date:");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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

            szMsg = string.Format("{0}", "P/O No.");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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

            szMsg = string.Format("{0}", "F.O.B.");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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

            szMsg = string.Format("{0}", "Ship Date");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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

            szMsg = string.Format("{0}", "Cancel Date");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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

            szMsg = string.Format("{0}", "Order By");
            hlpPar = new Paragraph(szMsg, timesHB_N_8);
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
            dDate = Convert.ToDateTime(invoice.SODate);
            szData01 = dDate.ToString("MM/dd/yyyy");
            szMsg = string.Format("{0}", szData01);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            szData02 = invoice.PurchaseOrderNo;
            szMsg = string.Format("{0}", szData02);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            szData03 = invoice.SalesRep;
            szMsg = string.Format("{0}", szData03);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            szData04 = " ";
            szMsg = string.Format("{0}", szData04);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            if (string.IsNullOrEmpty(invoice.PurchaseOrderNo))
            {
                szData06 = string.Empty;
            }
            else
            {
                if (purchaseorder != null)
                {
                    dDate = Convert.ToDateTime(purchaseorder.ShipDate);
                    szData06 = dDate.ToString("MM/dd/yyyy");                    
                }
                else
                {
                    szData06 = string.Empty;
                }
            }
            szMsg = string.Format("{0}", szData06);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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


            szMsg = string.Format("{0}", invoice.ShipVia);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            szMsg = string.Format("{0}", "  /  /  ");
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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

            szData08 = "  ";
            szMsg = string.Format("{0}", szData08);
            hlpPar = new Paragraph(szMsg, timesH_N_8);
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


        private PdfPTable GetSalesOrderAmountData(int SalesOrderId, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesHB_I_10, iTextSharp.text.Font timesHB_I_10_U, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_N_10_U, iTextSharp.text.Font timesH_I_10, iTextSharp.text.Font timesH_N_8)
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
            SalesOrder invoice = null;
            invoice = db.SalesOrders.Find(SalesOrderId);
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
            TimelyDepotMVC.Controllers.SalesOrderController invoiceCtrl = new Controllers.SalesOrderController();
            invoiceCtrl.GetSalesOrderTotals(invoice.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            string szSalesAmount = dSalesAmount.ToString("C");
            string szTotalTax = dTotalTax.ToString("C");
            string szTax = dTax.ToString("F2");
            string szTotalAmount = dTotalAmount.ToString("C");
            string szBalanceDue = dBalanceDue.ToString("C");


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
            title = new Paragraph(szMsg, timesH_N_8);
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

            szMsg = string.Format("Order Amount:");
            title = new Paragraph(szMsg, timesHB_I_10);
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
            title = new Paragraph(szMsg, timesHB_I_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("Sales Tax {0} %:", szTax);
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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

            szMsg = string.Format("Freight Fee & Handling:", " ");
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesHB_N_10);
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
            title = new Paragraph(szMsg, timesHB_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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

            //times07.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("Order Taken: {0}", " ");
            title = new Paragraph(szMsg, timesH_N_10);
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

            //times02.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("Balance Due:");
            title = new Paragraph(szMsg, timesHB_I_10_U);
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
            title = new Paragraph(szMsg, timesHB_I_10);
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

            //Signatures
            PdfPTable signaturestbl = GetSalesOrderSignatures(timesH_N_10, timesH_N_10_U);

            hlpCel = new PdfPCell(signaturestbl);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.PaddingTop = 0;
            hlpCel.PaddingLeft = 0;
            hlpCel.PaddingRight = 0;
            hlpCel.PaddingBottom = 0;
            //BorderColorTop = new BaseColor(System.Drawing.Color.LightGray),
            hlpCel.HorizontalAlignment = Element.ALIGN_CENTER;
            hlpCel.Colspan = 4;
            infotable.AddCell(hlpCel);

            return infotable;

        }

        private PdfPTable GetSalesOrderSignatures(iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_N_10_U)
        {
            string szMsg = "";
            Paragraph title = null;
            PdfPCell hlpCel = null;

            PdfPTable signaturestbl = new PdfPTable(numColumns: 6);
            signaturestbl.SetTotalWidth(new float[] { 87.16f, 87.16f, 87.16f, 87.16f, 87.16f, 87.2f });
            signaturestbl.LockedWidth = true;
            signaturestbl.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            signaturestbl.SpacingBefore = 5;

            //First signatures row
            szMsg = string.Format("Sales Person:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format("Price Change Approved by:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);


            szMsg = string.Format("Credit Approved by:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);


            //Second signatures row
            szMsg = string.Format("Pricing Approved By::");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);


            szMsg = string.Format("Shipping Approved by:");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 0;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            szMsg = string.Format(" ");
            title = new Paragraph(szMsg, timesH_N_10);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidthTop = 0;
            hlpCel.BorderWidthLeft = 0;
            hlpCel.BorderWidthRight = 0;
            hlpCel.BorderWidthBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_RIGHT;
            signaturestbl.AddCell(hlpCel);

            return signaturestbl;
        }


        //
        // GET: /ITextReports/POReport
        public ActionResult POReport(int id = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data
            //Get the first invoice
            if (id == 0)
            {
                PurchaseOrders invoiceHlp = db.PurchaseOrders.FirstOrDefault<PurchaseOrders>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                id = invoiceHlp.PurchaseOrderId;
            }

            //Get and verify resource for the Report
            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //The fonts 
            //Default font Helvetica 12 pt can not be changed use factory class to produce object with the required font
            iTextSharp.text.Font timesHB_I_18 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12 = null;
            iTextSharp.text.Font timesHB_N_10 = null;
            iTextSharp.text.Font timesHB_I_10 = null;
            iTextSharp.text.Font timesHB_I_10_U = null;

            iTextSharp.text.Font timesH_N_12 = null;
            iTextSharp.text.Font timesH_N_10 = null;
            iTextSharp.text.Font timesH_I_10 = null;
            iTextSharp.text.Font timesH_N_10_U = null;
            iTextSharp.text.Font timesH_N_8 = null;

            iTextSharp.text.Font timesArial_N_18 = null;

            timesHB_I_18 = CreateFont("HELVETICA_BOLD", 18f, iTextSharp.text.Font.ITALIC);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12 = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesHB_I_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.ITALIC);
            timesHB_I_10_U = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_12 = CreateFont("HELVETICA", 12f, iTextSharp.text.Font.NORMAL);
            timesH_N_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_I_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.ITALIC);
            timesH_N_10_U = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_8 = CreateFont("HELVETICA", 8f, iTextSharp.text.Font.NORMAL);
            timesArial_N_18 = CreateFont("Arial.ttf", 18f, iTextSharp.text.Font.NORMAL);

            //Underline the required fonts
            timesHB_I_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);
            timesH_N_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create in memory the PDF document, to allow a second pass and add the page number footer
            //MemoryStream baos = new MemoryStream();


            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {
                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                //writer = PdfWriter.GetInstance(doc, baos);
                //writer.CloseStream = false;

                writer.PageEvent = new TableHeader();

                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //
                PdfPTable table = GetTablePO(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_12, timesH_N_10, timesH_I_10, timesH_N_10_U, timesH_N_8, id);
                doc.Add(table);

                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }
            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }

        private PdfPTable GetTablePO(iTextSharp.text.Font timesHB_I_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesHB_I_10, iTextSharp.text.Font timesHB_I_10_U, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_I_10, iTextSharp.text.Font timesH_N_10_U, iTextSharp.text.Font timesH_N_8, int purchaseorderId)
        {
            double dHlp = 0;
            double dQty = 0;
            double dAmount = 0;
            string szMsg = "";

            IQueryable<PurchasOrderDetail> qryPrice = db.PurchasOrderDetails.Where(prc => prc.PurchaseOrderId == purchaseorderId).OrderBy(prc => prc.ItemPosition).ThenBy(prc => prc.ItemOrder);

            Phrase phrase = null;
            PdfPCell cell = null;

            TableHeader tableheader = new TableHeader();
            //tableheader.setHeader("Hola Vios");

            PdfPTable table = new PdfPTable(numColumns: 5);
            table.SetTotalWidth(new float[] { 149.43f, 149.43f, 74.71f, 74.71f, 74.72f });
            table.LockedWidth = true;

            // Add the first header row
            PdfPTable headertbl = CreatePOHeader(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesH_N_12, timesH_N_10, purchaseorderId);
            cell = new PdfPCell(headertbl);
            cell.Colspan = 5;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            // Add the second header row 
            szMsg = "Our Item #";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Your Item";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Qty";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Unit Price";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            szMsg = "Ext. Amount";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            // Add the footer
            PdfPTable footertbl = GetPOAmountData(purchaseorderId, timesHB_N_12, timesH_N_12, timesH_N_10);
            if (footertbl != null)
            {
                cell = new PdfPCell(footertbl);
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                table.AddCell(cell);

            }

            // There are three special rows
            table.HeaderRows = 3;
            // One of them is a footer
            table.FooterRows = 1;

            // Now let's loop over the data
            if (qryPrice.Count() > 0)
            {
                foreach (var item in qryPrice)
                {

                    szMsg = item.Sub_ItemID;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    szMsg = item.VendorReference;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.Quantity);
                    dQty = dHlp;
                    szMsg = dHlp.ToString("N0");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    szMsg = dHlp.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    dAmount = dQty * dHlp;
                    szMsg = dAmount.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);
                }
            }

            return table;
        }

        private PdfPTable GetPOAmountData(int purchaseorderId, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10)
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
            iTextSharp.text.Font timesHB_N_16 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12_U = null;
            iTextSharp.text.Font timesHB_N_10 = null;

            timesHB_N_16 = CreateFont("HELVETICA_BOLD", 16f, iTextSharp.text.Font.NORMAL);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12_U = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);

            //Underline font
            timesHB_N_12_U.SetStyle(iTextSharp.text.Font.UNDERLINE);

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;
            Paragraph title = null;

            PdfPTable infotable = new PdfPTable(numColumns: 1);
            infotable.WidthPercentage = 100;
            infotable.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            infotable.SpacingBefore = 25;

            szMsg = string.Format("Total {0}", dTotal.ToString("N2"));
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_16);
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
            title = new Paragraph(szMsg, timesHB_N_16);
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
            title = new Paragraph(szMsg, timesHB_N_12_U);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12_U);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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

        private PdfPTable CreatePOHeader(iTextSharp.text.Font timesHB_I_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, int purchaseorderId)
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

            PdfPCell nestingcell = null;
            PdfPCell hlpCel = null;
            Paragraph title = null;

            //Create the table for the headr
            PdfPTable table = new PdfPTable(numColumns: 1);
            table.SetTotalWidth(new float[] { 553f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 7;


            szMsg = string.Format("{0}", szTradeName);
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
            hlpCel = new PdfPCell(title);
            hlpCel.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCel.BorderWidth = 0;
            hlpCel.PaddingTop = 1;
            hlpCel.PaddingLeft = 4;
            hlpCel.PaddingRight = 1;
            hlpCel.PaddingBottom = 1;
            hlpCel.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCel);

            timesHB_I_18.SetStyle(iTextSharp.text.Font.NORMAL);
            szMsg = string.Format("Ref: {0}", szPORef);
            title = new Paragraph(szMsg, timesHB_I_18);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesH_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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
            title = new Paragraph(szMsg, timesHB_N_12);
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


        //
        // GET: /ITextReports/InvoiceReport
        public ActionResult InvoiceReport(int Invoiceid = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data
            //Get the first invoice
            if (Invoiceid == 0)
            {
                Invoice invoiceHlp = db.Invoices.FirstOrDefault<Invoice>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                Invoiceid = invoiceHlp.InvoiceId;
            }

            //Get and verify resource for the Report
            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //The fonts 
            //Default font Helvetica 12 pt can not be changed use factory class to produce object with the required font
            iTextSharp.text.Font timesHB_I_18 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12 = null;
            iTextSharp.text.Font timesHB_N_10 = null;
            iTextSharp.text.Font timesHB_I_10 = null;
            iTextSharp.text.Font timesHB_I_10_U = null;

            iTextSharp.text.Font timesH_N_12 = null;
            iTextSharp.text.Font timesH_N_10 = null;
            iTextSharp.text.Font timesH_I_10 = null;
            iTextSharp.text.Font timesH_N_10_U = null;
            iTextSharp.text.Font timesH_N_8 = null;

            iTextSharp.text.Font timesArial_N_18 = null;

            timesHB_I_18 = CreateFont("HELVETICA_BOLD", 18f, iTextSharp.text.Font.ITALIC);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12 = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesHB_I_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.ITALIC);
            timesHB_I_10_U = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_12 = CreateFont("HELVETICA", 12f, iTextSharp.text.Font.NORMAL);
            timesH_N_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_I_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.ITALIC);
            timesH_N_10_U = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_8 = CreateFont("HELVETICA", 8f, iTextSharp.text.Font.NORMAL);
            timesArial_N_18 = CreateFont("Arial.ttf", 18f, iTextSharp.text.Font.NORMAL);

            //Underline the required fonts
            timesHB_I_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);
            timesH_N_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create in memory the PDF document, to allow a second pass and add the page number footer
            //MemoryStream baos = new MemoryStream();


            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {
                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                //writer = PdfWriter.GetInstance(doc, baos);
                //writer.CloseStream = false;

                writer.PageEvent = new TableHeader();

                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //
                PdfPTable table = GetTablePrice(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_12, timesH_N_10, timesH_I_10, timesH_N_10_U, timesH_N_8, Invoiceid);
                doc.Add(table);

                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }

            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }


        //
        // GET: /ITextReports/InvoiceiText
        public ActionResult InvoiceiText(int Invoiceid = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data
            //Get the first invoice
            if (Invoiceid == 0)
            {
                Invoice invoiceHlp = db.Invoices.FirstOrDefault<Invoice>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                Invoiceid = invoiceHlp.InvoiceId;
            }

            //Get and verify resource for the Report
            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //The fonts 
            //Default font Helvetica 12 pt can not be changed use factory class to produce object with the required font
            iTextSharp.text.Font timesHB_I_18 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12 = null;
            iTextSharp.text.Font timesHB_N_10 = null;
            iTextSharp.text.Font timesHB_I_10 = null;
            iTextSharp.text.Font timesHB_I_10_U = null;

            iTextSharp.text.Font timesH_N_12 = null;
            iTextSharp.text.Font timesH_N_10 = null;
            iTextSharp.text.Font timesH_I_10 = null;
            iTextSharp.text.Font timesH_N_10_U = null;
            iTextSharp.text.Font timesH_N_8 = null;

            iTextSharp.text.Font timesArial_N_18 = null;

            timesHB_I_18 = CreateFont("HELVETICA_BOLD", 18f, iTextSharp.text.Font.ITALIC);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12 = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesHB_I_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.ITALIC);
            timesHB_I_10_U = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_12 = CreateFont("HELVETICA", 12f, iTextSharp.text.Font.NORMAL);
            timesH_N_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_I_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.ITALIC);
            timesH_N_10_U = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_8 = CreateFont("HELVETICA", 8f, iTextSharp.text.Font.NORMAL);
            timesArial_N_18 = CreateFont("Arial.ttf", 18f, iTextSharp.text.Font.NORMAL);

            //Underline the required fonts
            timesHB_I_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);
            timesH_N_10_U.SetStyle(iTextSharp.text.Font.UNDERLINE);

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {
                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);


                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //
                PdfPTable table = GetTablePrice(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_12, timesH_N_10, timesH_I_10, timesH_N_10_U, timesH_N_8, Invoiceid);
                doc.Add(table);


                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }
            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }

        private PdfPTable GetTablePrice(iTextSharp.text.Font timesHB_I_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesHB_I_10, iTextSharp.text.Font timesHB_I_10_U, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_I_10, iTextSharp.text.Font timesH_N_10_U, iTextSharp.text.Font timesH_N_8, int Invoiceid)
        {
            double dHlp = 0;
            double dQty = 0;
            double dAmount = 0;
            string szMsg = "";

            IQueryable<InvoiceDetail> qryPrice = db.InvoiceDetails.Where(prc => prc.InvoiceId == Invoiceid);

            Phrase phrase = null;
            PdfPCell cell = null;

            TableHeader tableheader = new TableHeader();
            //tableheader.setHeader("Hola Vios");

            PdfPTable table = new PdfPTable(numColumns: 7);
            table.SetTotalWidth(new float[] { 38.27f, 38.27f, 38.27f, 102.04f, 201.1f, 51.02f, 54.03f });
            table.LockedWidth = true;

            // Add the first header row
            PdfPTable headertbl = CreateHeader(timesHB_I_18, timesHB_N_14, timesHB_N_12, timesHB_N_10, timesH_N_12, timesH_N_10, Invoiceid);
            cell = new PdfPCell(headertbl);
            cell.Colspan = 7;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            // Add the second header row 
            szMsg = "Req. Qty";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Ship Qty";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "B.O. Qty";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "ItemID No.";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            szMsg = "Description";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            //szMsg = "Tax";
            //phrase = new Phrase(szMsg, timesH_N_10);
            //cell = new PdfPCell(phrase);
            ////cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.BorderWidthTop = 0f;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0.75f;
            //cell.BorderWidthBottom = 0.75f;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //table.AddCell(cell);

            szMsg = "Unit Price";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0.75f;
            cell.BorderWidthBottom = 0.75f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            szMsg = "Ext. Amount";
            phrase = new Phrase(szMsg, timesH_N_10);
            cell = new PdfPCell(phrase);
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0.75f;
            table.AddCell(cell);

            // Add the footer
            PdfPTable footertbl = GetSalesAmountData(Invoiceid, timesHB_N_10, timesHB_I_10, timesHB_I_10_U, timesH_N_10, timesH_N_10_U, timesH_I_10, timesH_N_8);
            cell = new PdfPCell(footertbl);
            cell.Colspan = 7;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            // There are three special rows
            table.HeaderRows = 3;
            // One of them is a footer
            table.FooterRows = 1;

            // Now let's loop over the data
            if (qryPrice.Count() > 0)
            {
                foreach (var item in qryPrice)
                {
                    dHlp = Convert.ToDouble(item.Quantity);
                    dQty = dHlp;
                    szMsg = dHlp.ToString("N0");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.ShipQuantity);
                    szMsg = dHlp.ToString("N0");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.BackOrderQuantity);
                    szMsg = dHlp.ToString("N0");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    szMsg = item.Sub_ItemID;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    szMsg = item.Description;
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    //dHlp = Convert.ToDouble(item.Tax);
                    //szMsg = dHlp.ToString("N2");
                    //phrase = new Phrase(szMsg, timesH_N_10);
                    //cell = new PdfPCell(phrase);
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BorderWidthTop = 0;
                    //cell.BorderWidthLeft = 0;
                    //cell.BorderWidthRight = 0.75f;
                    //cell.BorderWidthBottom = 0.75f;
                    //table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    szMsg = dHlp.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.75f;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);

                    dHlp = Convert.ToDouble(item.UnitPrice);
                    dAmount = dQty * dHlp;
                    szMsg = dAmount.ToString("N2");
                    phrase = new Phrase(szMsg, timesH_N_10);
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 0.75f;
                    table.AddCell(cell);
                }
            }

            return table;
        }

        private PdfPTable GetSalesAmountData(int Invoiceid, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesHB_I_10, iTextSharp.text.Font timesHB_I_10_U, iTextSharp.text.Font timesH_N_10, iTextSharp.text.Font timesH_N_10_U, iTextSharp.text.Font timesH_I_10, iTextSharp.text.Font timesH_N_8)
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
            title = new Paragraph(szMsg, timesH_N_8);
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
            title = new Paragraph(szMsg, timesHB_I_10);
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
            title = new Paragraph(szMsg, timesHB_I_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesHB_N_10);
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
            title = new Paragraph(szMsg, timesHB_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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
            title = new Paragraph(szMsg, timesH_N_10);
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

            //times07.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("C/C#: {0}", "******8286");
            title = new Paragraph(szMsg, timesH_N_10_U);
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

            //times02.SetStyle(Font.UNDERLINE);
            szMsg = string.Format("Balance Due:");
            title = new Paragraph(szMsg, timesHB_I_10_U);
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
            title = new Paragraph(szMsg, timesHB_I_10);
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


        private PdfPTable CreateHeader(iTextSharp.text.Font timesHB_N_18, iTextSharp.text.Font timesHB_N_14, iTextSharp.text.Font timesHB_N_12, iTextSharp.text.Font timesHB_N_10, iTextSharp.text.Font timesH_N_12, iTextSharp.text.Font timesH_N_10, int Invoiceid)
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
            var table = new PdfPTable(numColumns: 2);
            table.SetTotalWidth(new float[] { 372.5f, 180.5f });
            table.LockedWidth = true;
            table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            table.SpacingAfter = 7;


            PdfPCell hlpCell = null;
            Paragraph hlpPar = null;

            Paragraph title = new Paragraph("I N V O I C E", timesHB_N_18);

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

            szMsg = string.Format("Invoice No.:");
            title = new Paragraph(szMsg, timesHB_N_14);
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
            title = new Paragraph(szMsg, timesH_N_12);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested.AddCell(hlpCell);

            szMsg = string.Format("Customer No:");
            title = new Paragraph(szMsg, timesHB_N_12);
            hlpCell = new PdfPCell(title);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nested.AddCell(hlpCell);


            szMsg = string.Format("{0}", szCustomerNo);
            title = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_14);
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

            hlpPar = new Paragraph("ASI:", timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
            hlpCell = new PdfPCell(hlpPar);
            hlpCell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
            hlpCell.BorderWidth = 0;
            hlpCell.PaddingTop = 1;
            hlpCell.PaddingLeft = 4;
            hlpCell.PaddingRight = 1;
            hlpCell.PaddingBottom = 1;
            hlpCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nested01.AddCell(hlpCell);

            hlpPar = new Paragraph("SAGE:", timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            hlpPar = new Paragraph(szMsg, timesH_N_12);
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
            PdfPTable nested02 = new PdfPTable(2);
            nested02.SetTotalWidth(new float[] { 259.47f, 259.47f });
            nested02.LockedWidth = true;

            szMsg = string.Format("{0}", "Sold to");
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesHB_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
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
            hlpPar = new Paragraph(szMsg, timesH_N_10);
            hlpCell = new PdfPCell(hlpPar);

            PdfPTable invoicedatatbl = GetInvoiceData(timesH_N_10, timesHB_N_10, invoice);
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

        private PdfPTable GetInvoiceData(iTextSharp.text.Font times04, iTextSharp.text.Font times05, Invoice invoice)
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

            szMsg = string.Format("{0}", "Sales Order Date");
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

        //Create and embedd the font
        private iTextSharp.text.Font CreateFont(string szFontName, float fFontSize, int nfontStyle)
        {

            string szReportFolderPath = string.Format("~/Pdf");
            string szReportFontFolderPath = string.Format("~/Pdf/fonts");
            string szFontFolderPath = "";

            //Verify font folders
            szReportFolderPath = Server.MapPath(szReportFolderPath);
            if (!Directory.Exists(szReportFolderPath))
            {
                Directory.CreateDirectory(szReportFolderPath);
            }
            szReportFontFolderPath = Server.MapPath(szReportFontFolderPath);
            if (!Directory.Exists(szReportFontFolderPath))
            {
                Directory.CreateDirectory(szReportFontFolderPath);
            }


            BaseFont bf00 = null;

            switch (szFontName)
            {
                case "HELVETICA":
                    bf00 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    break;
                case "HELVETICA_BOLD":
                    bf00 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    break;
                default:
                    szFontFolderPath = string.Format("{0}\\{1}", szReportFontFolderPath, szFontName);
                    bf00 = BaseFont.CreateFont(szFontFolderPath, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    break;
            }

            //Return the font
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf00, fFontSize, nfontStyle);

            return font;
        }

        //
        // GET: /ITextReports/InvoiceiText00
        public ActionResult InvoiceiText00(int Invoiceid = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data

            //Get and verify resource for the Report

            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {


                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);


                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //

                //Create Document header; shows GMT time when document was created
                // HeaderFooter class removed in iText 5.0.0, so we instead write content to an 'absolute' position on the document
                iTextSharp.text.Rectangle page = doc.PageSize;
                PdfPTable head = new PdfPTable(numColumns: 1);
                head.TotalWidth = page.Width;

                szMsg = string.Format("{0} GMT", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                Phrase phrase = new Phrase(szMsg, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10));
                PdfPCell c = new PdfPCell(phrase);
                c.Border = iTextSharp.text.Rectangle.NO_BORDER;
                c.VerticalAlignment = Element.ALIGN_TOP;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                head.AddCell(c);
                head.WriteSelectedRows(
                    //first/last row row; -1 writes all rows
                    0, -1,
                    // left offsets
                    0,
                    // ** bottom ** yPos of the table
                    page.Height - doc.TopMargin + head.TotalHeight + 10,
                    writer.DirectContent
                    );

                // add image to the document
                szMsg = string.Format("~/Images/heroAccent.png");
                szMsg = Request.MapPath(szMsg);
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(szMsg);
                gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                gif.ScalePercent(150f);
                doc.Add(gif);

                //add tabular data
                Paragraph p = new Paragraph("Parameters for the web site");
                p.Alignment = 1;
                doc.Add(p);

                PdfPTable tabulartb = _stateTable();
                doc.Add(tabulartb);

                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }
            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }

        //
        // GET: /ITextReports/InvoiceiText01
        public ActionResult InvoiceiText01(int Invoiceid = 0)
        {
            string szMsg = "";
            string szError = "";

            //Get the report data

            //Get and verify resource for the Report
            float fWidth = Utilities.PointsToMillimeters(523);
            float fHeight = Utilities.PointsToMillimeters(770);

            //The fonts 
            iTextSharp.text.Font timesHB_I_18 = null;
            iTextSharp.text.Font timesHB_N_14 = null;
            iTextSharp.text.Font timesHB_N_12 = null;
            iTextSharp.text.Font timesHB_N_10 = null;
            iTextSharp.text.Font timesH_N_12 = null;
            iTextSharp.text.Font timesH_N_10 = null;
            iTextSharp.text.Font timesArial_N_18 = null;

            timesHB_I_18 = CreateFont("HELVETICA_BOLD", 18f, iTextSharp.text.Font.ITALIC);
            timesHB_N_14 = CreateFont("HELVETICA_BOLD", 14f, iTextSharp.text.Font.NORMAL);
            timesHB_N_12 = CreateFont("HELVETICA_BOLD", 12f, iTextSharp.text.Font.NORMAL);
            timesHB_N_10 = CreateFont("HELVETICA_BOLD", 10f, iTextSharp.text.Font.NORMAL);
            timesH_N_12 = CreateFont("HELVETICA", 12f, iTextSharp.text.Font.NORMAL);
            timesH_N_10 = CreateFont("HELVETICA", 10f, iTextSharp.text.Font.NORMAL);
            timesArial_N_18 = CreateFont("Arial.ttf", 18f, iTextSharp.text.Font.NORMAL);

            //Default font Helvetica 12 pt can not be changed use factory class to produce object with the required font

            //Generate and display the report
            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create the PDF object
            Document doc = null;
            PdfWriter writer = null;

            try
            {


                //
                //Setp 1 Instantiate a Document object
                // Default page size: A4 595 x 842 pt, margins: top, bottom, keft , right 36 pt
                // Working area: 523 x 770 pt = 184.5 x 271.64 mm
                // 1 in = 2.54 cm = 72 pt
                // Default orientation: portrait
                doc = new Document();

                //
                // Step 2 instantiate a PdfWriter
                //
                writer = PdfWriter.GetInstance(doc, Response.OutputStream);


                // Step 3 Open the document instance
                if (!doc.IsOpen())
                {
                    doc.Open();
                }

                //
                // Step 4 Add Content to the document
                //

                //Create Document header; shows GMT time when document was created
                // HeaderFooter class removed in iText 5.0.0, so we instead write content to an 'absolute' position on the document
                iTextSharp.text.Rectangle page = doc.PageSize;
                PdfPTable head = new PdfPTable(numColumns: 1);
                head.TotalWidth = page.Width;

                szMsg = string.Format("{0} GMT", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                Phrase phrase = new Phrase(szMsg, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10));
                PdfPCell c = new PdfPCell(phrase);
                c.Border = iTextSharp.text.Rectangle.NO_BORDER;
                c.VerticalAlignment = Element.ALIGN_TOP;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                head.AddCell(c);
                head.WriteSelectedRows(
                    //first/last row row; -1 writes all rows
                    0, -1,
                    // left offsets
                    0,
                    // ** bottom ** yPos of the table
                    page.Height - doc.TopMargin + head.TotalHeight + 10,
                    writer.DirectContent
                    );

                // add image to the document
                szMsg = string.Format("~/Images/heroAccent.png");
                szMsg = Request.MapPath(szMsg);
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(szMsg);
                gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                gif.ScalePercent(150f);
                doc.Add(gif);

                //add tabular data
                Paragraph p = new Paragraph("Parameters for the web site");
                p.Alignment = 1;
                doc.Add(p);

                PdfPTable tabulartb = _stateTable();
                doc.Add(tabulartb);


                //
                // Step 5 Close the document
                //
                if (doc.IsOpen())
                {
                    //Verify that the document has at least a page (try/catch/throw)
                    doc.Close();
                }
            }
            catch (DocumentException docerr)
            {
                szError = docerr.Message;
            }
            catch (Exception err)
            {
                szError = string.Format("{0} {1}", szError, err.Message);

                TempData["PdfErrorMessage"] = szError;

                return RedirectToAction("PdfError");
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }


        //
        // GET: /ITextReports/PdfError
        public ActionResult PdfError()
        {
            if (TempData["PdfErrorMessage"] != null)
            {
                ViewBag.PdfError = TempData["PdfErrorMessage"].ToString();
            }
            return View();
        }

        //
        // GET: /ITextReports/Invoice
        public ActionResult Invoice()
        {
            int Invoiceid = 0;
            string szOutputFilePath = "";
            string szOutputFilePathHlp = "";
            string szMsg = "";

            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Verify the required folder
            string szReportFolderPath = string.Format("~/Pdf");
            szReportFolderPath = Server.MapPath(szReportFolderPath);
            if (!Directory.Exists(szReportFolderPath))
            {
                Directory.CreateDirectory(szReportFolderPath);
            }

            //Save the report folder path
            Parameters parameter = db.Parameters.Where(prmt => prmt.Parameter == "PdfFolderPath").FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = "PdfFolderPath";
                parameter.ParameterValue = szReportFolderPath;
                db.Parameters.Add(parameter);
                db.SaveChanges();
            }
            else
            {
                szReportFolderPath = parameter.ParameterValue;
            }

            //Get the first invoice
            if (Invoiceid == 0)
            {
                Invoice invoiceHlp = db.Invoices.FirstOrDefault<Invoice>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                Invoiceid = invoiceHlp.InvoiceId;
                //id = Invoiceid;
            }

            var rpt = new InvoiceReport().CreateReport02(szReportFolderPath, Response, Invoiceid);

            // Send the output to the client
            Response.Flush();
            return View();

            //szOutputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);  //This fails. does not found the pdf file !!
            //var rpt = new InvoiceReport().CreateReport(szReportFolderPath, Invoiceid);
            //szOutputFilePathHlp = string.Format("~/Pdf/InvoiceReport-{0}.pdf", Invoiceid.ToString());
            //return Redirect(szOutputFilePathHlp);

        }

        // GET: /ITextReports/Samplekdi
        public ActionResult Samplekdi()
        {
            string szMsg = "";

            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "application/pdf";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;


            //Setp 1 Instantiate a Document object
            Document doc = new Document();

            // Step 2 instantiate a PdfWriter
            PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);


            // Step 3 Open the document instance
            if (!doc.IsOpen())
            {
                doc.Open();
            }


            // Step 4 Add Content to the document
            //Create Document header; shows GMT time when document was created
            // HeaderFooter class removed in iText 5.0.0, so we instead write content to an 'absolute' position on the document
            iTextSharp.text.Rectangle page = doc.PageSize;
            PdfPTable head = new PdfPTable(numColumns: 1);
            head.TotalWidth = page.Width;

            szMsg = string.Format("{0} GMT", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            Phrase phrase = new Phrase(szMsg, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10));
            PdfPCell c = new PdfPCell(phrase);
            c.Border = iTextSharp.text.Rectangle.NO_BORDER;
            c.VerticalAlignment = Element.ALIGN_TOP;
            c.HorizontalAlignment = Element.ALIGN_CENTER;
            head.AddCell(c);
            head.WriteSelectedRows(
                //first/last row row; -1 writes all rows
                0, -1,
                // left offsets
                0,
                // ** bottom ** yPos of the table
                page.Height - doc.TopMargin + head.TotalHeight + 10,
                writer.DirectContent
                );

            // add image to the document
            szMsg = string.Format("~/Images/heroAccent.png");
            szMsg = Request.MapPath(szMsg);
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(szMsg);
            gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            gif.ScalePercent(150f);
            doc.Add(gif);

            //add tabular data
            Paragraph p = new Paragraph("Parameters for the web site");
            p.Alignment = 1;
            doc.Add(p);

            PdfPTable tabulartb = _stateTable();
            doc.Add(tabulartb);

            // Step 5 Close the document
            if (doc.IsOpen())
            {
                //Verify that the document has at least a page (try/catch/throw)
                doc.Close();
            }


            // Send the output to the client
            Response.Flush();

            return View();
        }

        private PdfPTable _stateTable()
        {
            IQueryable<Parameters> qryParm = db.Parameters.OrderBy(prmt => prmt.Parameter);

            PdfPTable table = new PdfPTable(numColumns: 2);

            // Default table witdh 80%
            table.WidthPercentage = 100;

            //Set the columns relative widths
            table.SetWidths(new Single[] { 2, 4 });

            // by default tables 'collapse' on surrounding elements, you need to explicity add spacing
            table.SpacingBefore = 10;

            // Set the table header
            Phrase phrase = new Phrase("Parameter");
            PdfPCell cell = new PdfPCell(phrase);
            cell.BackgroundColor = new BaseColor(204, 204, 204);
            table.AddCell(cell);

            phrase = new Phrase("Value");
            cell = new PdfPCell(phrase);
            cell.BackgroundColor = new BaseColor(204, 204, 204);
            table.AddCell(cell);

            //Displat tha database data
            if (qryParm.Count() > 0)
            {
                foreach (var item in qryParm)
                {
                    phrase = new Phrase(item.Parameter);
                    cell = new PdfPCell(phrase);
                    table.AddCell(cell);

                    phrase = new Phrase(item.ParameterValue);
                    cell = new PdfPCell(phrase);
                    table.AddCell(cell);
                }
            }

            return table;

        }

        //
        // GET: /ITextReports/OutputStream
        public ActionResult OutputStream()
        {
            string szMsg = "";

            szMsg = Response.ContentType;

            //Set the page's content type to jpeg files and clears all content from the buffer stream
            Response.ContentType = "image/jpeg";
            Response.Clear();

            //Buffer respose so that page is sent after processing of the page is complete
            Response.BufferOutput = true;

            //Create a Font style
            System.Drawing.Font rectangleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);


            //Create integer variables
            int height = 100;
            int width = 200;

            //Create a random number generator and create variable values based on it
            Random r = new Random();
            int x = r.Next(75);
            int a = r.Next(155);
            int x1 = r.Next(100);

            //Create a bitmap and use it to create a graphic object
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);

            // Use the Graphics to draw three rectangles
            g.DrawRectangle(Pens.White, 1, 1, width - 3, height - 3);
            g.DrawRectangle(Pens.Aquamarine, 2, 2, width - 3, height - 3);
            g.DrawRectangle(Pens.CornflowerBlue, 0, 0, width, height);

            // Use the graphics object to write a string on the rectangle
            g.DrawString("ASP.NET Pdf sample", rectangleFont, SystemBrushes.WindowText, new PointF(10, 40));

            // Apply color to two of the rectangles
            g.FillRectangle(new SolidBrush(Color.FromArgb(a, 255, 128, 255)), x, 20, 100, 50);

            g.FillRectangle(
                new LinearGradientBrush(
                    new Point(x, 10),
                    new Point(x1 + 75, 50 + 30),
                    Color.FromArgb(128, 0, 0, 128),
                    Color.FromArgb(255, 255, 255, 240)
                    ),
                x1, 50, 75, 30
                );

            //Save bitmap to the response steam and convert it to jpeg format
            bmp.Save(Response.OutputStream, ImageFormat.Jpeg);

            // Release memory used by the Graphic object and the bitmap
            g.Dispose();
            bmp.Dispose();

            // Send the output to the client
            Response.Flush();

            return View();
        }
        //
        // GET: /ITextReports/

        public ActionResult Index()
        {
            return View();
        }

    }
}
