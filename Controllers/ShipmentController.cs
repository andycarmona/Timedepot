using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;
using PagedList;
using TimelyDepotMVC.CommonCode;
using System.Data;

using TimelyDepotMVC.ModelsView;
using TimelyDepotMVC.UPSShipService;
using TimelyDepotMVC.XAVService;
using TimelyDepotMVC.UPSWrappers;
using TimelyDepotMVC.Properties;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TimelyDepotMVC.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Globalization;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Web.Routing;
    using System.Web.Script.Serialization;

    using AutoMapper;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    using Newtonsoft.Json;

    using PayPal.Platform.SDK;

    using PdfReportSamples.Models;

    using TimelyDepotMVC.Helpers;
    using TimelyDepotMVC.Models;
    using TimelyDepotMVC.ModelsView;
    using TimelyDepotMVC.UPSRateService;
    using TimelyDepotMVC.UPSShipService;

    public class ShipmentController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //
        // GET: /Shipment/DetailsShipment
        [NoCache]
        public PartialViewResult DetailsShipment(int id)
        {


            return PartialView();
        }

        //
        // GET: /Shipment/DeleteShipment
        public ActionResult DeleteShipment(int id)
        {
            int nInvoiceid = 0;

            IQueryable<ShipmentDetails> qryDetails = null;

            Shipment shipment = db.Shipments.Find(id);
            if (shipment != null)
            {
                nInvoiceid = Convert.ToInt32(shipment.InvoiceId);

                qryDetails = db.ShipmentDetails.Where(shpdtl => shpdtl.ShipmentId == shipment.ShipmentId);
                if (qryDetails.Count() > 0)
                {
                    foreach (var item in qryDetails)
                    {
                        db.ShipmentDetails.Remove(item);
                    }
                }

                db.Shipments.Remove(shipment);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Shipment", new { id = nInvoiceid });
        }

        //
        // GET: //Invoice/
        [NoCache]
        public PartialViewResult ShipmentLog(int? page, string searchItemLog, string ckCriteriaLog)
        {
            bool bHasData = true;
            int nShipmentId = 0;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            string[] szFecha = null;
            DateTime dFecha = DateTime.Now;
            Mapper.CreateMap<Shipment, ShipmentLogView>();

            IQueryable<Shipment> qryShipment = null;

            List<ShipmentLogView> ShipmentList = new List<ShipmentLogView>();


            if (string.IsNullOrEmpty(searchItemLog) || searchItemLog == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "invoice";
                ViewBag.CurrentDateLog = dFecha.ToString("yyyy/MM/dd");
                bHasData = false;

                if (string.IsNullOrEmpty(searchItemLog) || searchItemLog == "0")
                {
                    ViewBag.SearchItemLog = searchItemLog;
                    bHasData = true;

                    if (ckCriteriaLog.Trim() == "invoice")
                    {
                        qryShipment = db.Shipments.OrderByDescending(vd => vd.InvoiceNo);
                    }
                }
            }
            else
            {
                ViewBag.SearchItem = searchItemLog;
                ViewBag.ckCriteriaHlp = ckCriteriaLog;

                if (ckCriteriaLog.Trim() == "invoice")
                {
                    qryShipment = db.Shipments.Where(vd => vd.InvoiceNo.StartsWith(searchItemLog)).OrderBy(vd => vd.InvoiceNo);
                }


                if (ckCriteriaLog.Trim() == "shipmentrecord")
                {
                    nShipmentId = Convert.ToInt32(searchItemLog);
                    qryShipment = db.Shipments.Where(vd => vd.ShipmentId == nShipmentId).OrderBy(vd => vd.InvoiceNo);
                }
            }

            if (bHasData)
            {
                if (qryShipment != null)
                {
                    if (qryShipment.Count() > 0)
                    {
                        List<ShipmentLogView> shipmentLogList = Mapper.Map<IQueryable<Shipment>, List<ShipmentLogView>>(qryShipment);
                        foreach (var item in shipmentLogList)
                        {
                            if (item.Shipped)
                            {
                                var actualInvoice = db.Invoices.FirstOrDefault(x => x.InvoiceId == item.InvoiceId);
                                var customer = db.CustomersContactAddresses.FirstOrDefault(c => c.Id == actualInvoice.CustomerId);
                                item.CompanyName = customer.CompanyName;
                                ShipmentList.Add(item);
                            }
                        }
                    }
                }

            }
            
            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = ShipmentList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(ShipmentList.ToPagedList(pageIndex, pageSize));

            //return PartialView();
        }

        public ActionResult OpenShipmentLogFile()
        {
            return View();
        }

        public PartialViewResult ProcessShipmentInformation(string invoiceId, string upsNumber)
        {


            List<ShipmentDetails> listShipmentDetails = null;

            ViewBag.ShipperNumber = upsNumber.Equals("undefined")? Settings.Default.UPSShipperNumber:upsNumber ;
            int parsedInvoiceId = Int16.Parse(invoiceId);

            var shipmentByInvoice = db.Shipments.FirstOrDefault(z => z.InvoiceId == parsedInvoiceId);
            if (shipmentByInvoice != null)
            {
                listShipmentDetails =
                    db.ShipmentDetails.Where(x => x.ShipmentId == shipmentByInvoice.ShipmentId).ToList();
                ViewBag.ActualShipmentId = shipmentByInvoice.ShipmentId;
            }
            // Shipment boxes
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            listSelector = new List<KeyValuePair<string, string>>();
            if (listShipmentDetails != null)
            {
                foreach (var shipmentDetail in listShipmentDetails)
                {
                    listSelector.Add(
                        new KeyValuePair<string, string>(
                            shipmentDetail.ShipmentDetailID.ToString(CultureInfo.InvariantCulture),
                            "Box " + shipmentDetail.BoxNo + " - " + shipmentDetail.Reference1));
                }
            }

            SelectList shipmentBoxOptionlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ShipmentBoxesOptionlist = shipmentBoxOptionlist;

            //Request option
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Rate", "Rate"));
            listSelector.Add(new KeyValuePair<string, string>("Shop", "Shop"));
            SelectList requestOptionlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.RequestOptionlist = requestOptionlist;

            // Bill To Option
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Shipper", "Shipper"));
            listSelector.Add(new KeyValuePair<string, string>("Sender", "Sender"));
            listSelector.Add(new KeyValuePair<string, string>("Third Party", "Third Party"));
            SelectList billToOptionlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.BillToOptionlist = billToOptionlist;



            //Customer type
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("00", "Rates Associated with Shipper Number"));
            listSelector.Add(new KeyValuePair<string, string>("01", "Daily Rates"));
            listSelector.Add(new KeyValuePair<string, string>("04", "Retail Rates"));
            listSelector.Add(new KeyValuePair<string, string>("53", "Standard List Rates"));
            SelectList customerTypelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.CustomerTypeList = customerTypelist;

            //UPS Service type
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("03", "Ground"));
            listSelector.Add(new KeyValuePair<string, string>("01", "Next Day Air"));
            listSelector.Add(new KeyValuePair<string, string>("02", "2nd Day Air"));
            listSelector.Add(new KeyValuePair<string, string>("12", "3 Day Select"));
            listSelector.Add(new KeyValuePair<string, string>("13", "Next Day Air Saver"));
            listSelector.Add(new KeyValuePair<string, string>("14", "Next Day Air Early AM"));
            listSelector.Add(new KeyValuePair<string, string>("59", "2nd Day Air AM"));
            listSelector.Add(new KeyValuePair<string, string>("07", "Worldwide Express"));
            listSelector.Add(new KeyValuePair<string, string>("08", "Worldwide Expedited"));
            listSelector.Add(new KeyValuePair<string, string>("11", "Standard"));
            listSelector.Add(new KeyValuePair<string, string>("54", "Worldwide Express Plus"));
            listSelector.Add(new KeyValuePair<string, string>("65", "UPS Saver"));
            listSelector.Add(new KeyValuePair<string, string>("82", "UPS Today Standard"));
            listSelector.Add(new KeyValuePair<string, string>("83", "UPS Today Dedicated Courier"));
            listSelector.Add(new KeyValuePair<string, string>("84", "UPS Today Intercity"));
            listSelector.Add(new KeyValuePair<string, string>("85", "UPS Today Express"));
            listSelector.Add(new KeyValuePair<string, string>("86", "UPS Today Express Saver"));
            SelectList upsServiceTypelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.UpsServiceTypelist = upsServiceTypelist;

            //Package pack type
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("02", "Package/customer supplied"));
            listSelector.Add(new KeyValuePair<string, string>("00", "Unknown"));
            listSelector.Add(new KeyValuePair<string, string>("01", "UPS Letter"));
            listSelector.Add(new KeyValuePair<string, string>("03", "UPS Tube"));
            listSelector.Add(new KeyValuePair<string, string>("04", "UPS Pack"));
            listSelector.Add(new KeyValuePair<string, string>("21", "Express Box"));
            listSelector.Add(new KeyValuePair<string, string>("24", "25KG Box"));
            listSelector.Add(new KeyValuePair<string, string>("25", "10KG Box"));
            listSelector.Add(new KeyValuePair<string, string>("30", "Pallet"));
            listSelector.Add(new KeyValuePair<string, string>("2a", "Small Express Box"));
            listSelector.Add(new KeyValuePair<string, string>("2b", "Medium Express Box"));
            listSelector.Add(new KeyValuePair<string, string>("2c", "Large Express Box"));
            SelectList packageTypelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.packageTypelist = packageTypelist;

            return this.PartialView();
        }

        public ActionResult ProcessShipmentConfirmation(string serviceCode, int shipmentId, string invoiceNo, string upsShipperNumber)
        {

            ShipConfirmResponse rateResponse = null;
            string szError = null;
            var selectedInvoice = db.Invoices.SingleOrDefault(x => x.InvoiceNo == invoiceNo);

            if (selectedInvoice != null)
            {
                var shipmentRequestDto = Mapper.Map<ShipmentRequestView>(selectedInvoice);
                shipmentRequestDto.userName = UPSConstants.UpsUserName;
                shipmentRequestDto.password = UPSConstants.UpsPasword;
                shipmentRequestDto.accessLicenseNumber = UPSConstants.UpsAccessLicenseNumber;
                shipmentRequestDto.shipperNumber = upsShipperNumber;
                shipmentRequestDto.packagingTypeCode = UPSConstants.UpsPackagingType;
                shipmentRequestDto.shipmentChargeType = UPSConstants.UpsShipmentChargeType;
                shipmentRequestDto.billShipperAccountNumber = upsShipperNumber;

                var shipServiceWrapper = new UPSShipServiceWrapper(shipmentRequestDto);
                log.Debug("ShipmentId: " + shipmentId.ToString() + " for invoiceNo: " +invoiceNo);                
                rateResponse = shipServiceWrapper.CallUPSShipmentConfirmationRequest(serviceCode, shipmentId, ref szError);

            }

            return string.IsNullOrEmpty(szError) ? this.Json(rateResponse, JsonRequestBehavior.AllowGet) : this.Json(szError, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetShipmentDetailByInvoice(int invoiceId)
        {
            List<int> idArray = this.db.InvoiceDetails.Select(inv => inv.Id).ToList();
            var listOfShipId = JsonConvert.SerializeObject(idArray);
            return this.Json(listOfShipId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcessShipment(string serviceCode, int shipmentId, string invoiceNo, string upsShipperNumber)
        {

            ShipmentResponse rateResponse = null;
            string szError = null;
            var selectedInvoice = db.Invoices.SingleOrDefault(x => x.InvoiceNo == invoiceNo);
            var actualShipment = db.Shipments.FirstOrDefault(y => y.ShipmentId == shipmentId);

            if ((selectedInvoice == null) || (actualShipment == null))
            {
                return this.Json("Error.No invoice selected.", JsonRequestBehavior.AllowGet);
            }

            actualShipment.UpsNumber = upsShipperNumber;
            var shipmentRequestDto = Mapper.Map<ShipmentRequestView>(selectedInvoice);
            shipmentRequestDto.userName = UPSConstants.UpsUserName;
            shipmentRequestDto.password = UPSConstants.UpsPasword;
            shipmentRequestDto.accessLicenseNumber = UPSConstants.UpsAccessLicenseNumber;
            shipmentRequestDto.shipperNumber = upsShipperNumber;
            shipmentRequestDto.packagingTypeCode = UPSConstants.UpsPackagingType;
            shipmentRequestDto.shipmentChargeType = UPSConstants.UpsShipmentChargeType;
            shipmentRequestDto.billShipperAccountNumber = upsShipperNumber;

            var shipServiceWrapper = new UPSShipServiceWrapper(shipmentRequestDto);

            rateResponse = shipServiceWrapper.CallUPSShipmentRequest(serviceCode, shipmentId, ref szError);
            if (!string.IsNullOrEmpty(szError))
            {
                return this.Json(szError, JsonRequestBehavior.AllowGet);
            }

            this.AddUpsDataToShipmentDetail(shipmentId, rateResponse.ShipmentResults.PackageResults);
            this.ResetQuantityOfInvoiceDetails(selectedInvoice);
            return this.Json(rateResponse, JsonRequestBehavior.AllowGet);
        }

        public void ResetQuantityOfInvoiceDetails(Invoice selectedInvoice)
        {
            var listInvoiceDetails =
                db.InvoiceDetails.Where(x => x.InvoiceId == selectedInvoice.InvoiceId).ToList();
            foreach (var invoiceDetail in listInvoiceDetails)
            {
                invoiceDetail.ShipQuantity = 0;
                this.db.Entry(invoiceDetail).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void UpdateInvoiceQuantity(Invoice selectedInvoice, int totalShipped, int id)
        {
            var invoiceDetail = db.InvoiceDetails.SingleOrDefault(x => x.InvoiceId == selectedInvoice.InvoiceId && x.Id == id);

            if (invoiceDetail != null)
            {
                var actualQuantity = (int)(invoiceDetail.ShipQuantity - totalShipped);
                if (actualQuantity >= 0)
                {
                    // invoiceDetail.Quantity = actualQuantity;
                    invoiceDetail.ShipQuantity = actualQuantity;
                }
            }

            this.db.Entry(invoiceDetail).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddUpsDataToShipmentDetail(int shipmentId, PackageResultsType[] packageList)
        {
            var aShipmentDetailList = db.ShipmentDetails.Where(x => x.ShipmentId == shipmentId).ToList();
            for (var packageIndex = 0; packageIndex < packageList.Count(); packageIndex++)
            {
                var aLabel = packageList[packageIndex].ShippingLabel.GraphicImage;
                var trackId = packageList[packageIndex].TrackingNumber;
                aShipmentDetailList[packageIndex].ShipmentLabel = "data:image/jpg;base64," + aLabel;
                aShipmentDetailList[packageIndex].trackId = trackId;
                aShipmentDetailList[packageIndex].Shipped = true;
                this.db.Entry(aShipmentDetailList[packageIndex]).State = EntityState.Modified;
            }
            var actualShipment = db.Shipments.SingleOrDefault(x => x.ShipmentId == shipmentId);
            actualShipment.Shipped = true;
            this.db.Entry(actualShipment).State = EntityState.Modified;

        }


        public PartialViewResult GetUPSRateTE(int? invoiceId, string shipToPostalCode)
        {
            var resultData = new List<ResultData>();
            var resultDataViewList = new List<ResultDataView>();
            var currency = "US";
            try
            {
                decimal unitPrice = 0;
                int nrBoxes = 0;
                int itemsInLastBox = 0;
                string fullBoxWeight = null;
                string partialBoxWeight = null;
                int valuePerFullBox = 0;
                int valuePerPartialBox = 0;
                UPSWrappers.inv_detl details = null;
                string errorMessage = "";


                resultData.Add(new ResultData() { service = "UPS Ground", code = "03", cost = 0, Negcost = 0, Publishedcost = 0 });
                resultData.Add(new ResultData() { service = "UPS Three-Day Select®", code = "12" });
                resultData.Add(new ResultData() { service = "UPS Second Day Air®", code = "02" });
                resultData.Add(new ResultData() { service = "UPS Next Day Air®", code = "01" });

                var invoiceDetailList = this.db.InvoiceDetails.Where(inv => inv.InvoiceId == invoiceId);
                foreach (var anInvoiceList in invoiceDetailList)
                {
                    if (anInvoiceList.ShipQuantity != null)
                    {
                        details = this.GetBoxInformationDetails(
                            anInvoiceList.ItemID,
                            (double)anInvoiceList.ShipQuantity,
                            ref unitPrice,
                            out nrBoxes,
                            out itemsInLastBox,
                            out fullBoxWeight,
                            out partialBoxWeight,
                            out valuePerFullBox,
                            out valuePerPartialBox);
                    }

                    if (anInvoiceList.ShipQuantity != null)
                    {

                        resultData = this.GetRateFromUPS(
                            (int)anInvoiceList.ShipQuantity,
                            nrBoxes,
                            itemsInLastBox,
                            fullBoxWeight,
                            valuePerFullBox,
                            valuePerPartialBox,
                            partialBoxWeight,
                            details,
                            unitPrice,
                            shipToPostalCode,
                            resultData,
                            out currency,
                            out errorMessage);
                    }
                }

                foreach (var rateData in resultData)
                {
                    if (rateData.cost == 0)
                    {
                        rateData.errorMessage = "Missing data to be able to calculate price.";
                    }
                    else
                    {
                        var aresultData = new ResultDataView();
                        aresultData.cost = rateData.cost.ToString(CultureInfo.InvariantCulture) + " " + currency;
                        aresultData.Negcost = rateData.Negcost.ToString(CultureInfo.InvariantCulture) + " " + currency;
                        aresultData.Publishedcost = rateData.Publishedcost.ToString(CultureInfo.InvariantCulture) + " " + currency;
                        aresultData.code = rateData.code;
                        aresultData.errorMessage = rateData.errorMessage;
                        aresultData.service = rateData.service;
                        aresultData.time = rateData.time;
                        resultDataViewList.Add(aresultData);
                    }
                }
            }
            catch (Exception ex)
            {
                resultDataViewList[0].errorMessage = ex.Message;

            }

            return PartialView(resultDataViewList);

        }

        private UPSWrappers.inv_detl GetBoxInformationDetails(
            string ItemId,
            double Qty,
            ref decimal unitPrice,
            out int nrBoxes,
            out int itemsInLastBox,
            out string fullBoxWeight,
            out string partialBoxWeight,
            out int valuePerFullBox,
            out int valuePerPartialBox)
        {
            int unitPerCase = 1;
            TimelyDepotContext dbAux = new TimelyDepotContext();
            int BOX_CASE = 0;
            decimal CASE_WI = 0;
            decimal UT_WT = 0;
            var ds = dbAux.PRICEs.Where(i => i.Item == ItemId).OrderByDescending(i => i.thePrice).ToList();

            if (ds != null && ds.Count > 0)
            {
                int j = 0;
                bool hasValue = false;

                var i = 0;
                foreach (var item in ds)
                {
                    if (Convert.ToInt16(item.Qty) > Qty)
                    {
                        hasValue = true;
                        j = i == 0 ? 0 : i - 1;
                        break;
                    }
                    i++;
                }
                if (!hasValue)
                {
                    j = ds.Count - 1;
                }
                var dsArray = ds.ToArray();
                unitPrice = Convert.ToDecimal(dsArray[j].thePrice);
            }

            UPSWrappers.inv_detl details = new UPSWrappers.inv_detl();
            details.CASE_HI = 0;
            details.CASE_LEN = 0;
            details.CASE_WI = 0;
            details.CASE_WT = 0;
            var itemList = dbAux.ITEMs.Where(i => i.ItemID == ItemId).ToList();
            if (itemList.Count > 0)
            {
                int j = 0;
                bool hasValue = false;
                var i = 0;
                foreach (var item in itemList)
                {
                    if (item.UnitPerCase != null)
                    {
                        BOX_CASE = Convert.ToInt16(item.UnitPerCase);
                    }
                    if (item.CaseWeight != null)
                    {
                        CASE_WI = Convert.ToDecimal(item.CaseWeight, CultureInfo.InvariantCulture);
                    }
                    if (item.UnitWeight != null)
                    {
                        UT_WT = Convert.ToDecimal(item.UnitWeight, CultureInfo.InvariantCulture);
                    }
                    if (item.DimensionH != null)
                    {
                        details.CASE_HI = Convert.ToInt16(item.DimensionH);
                    }
                    if (item.DimensionL != null)
                    {
                        details.CASE_LEN = Convert.ToInt16(item.DimensionL);
                    }
                    if (item.DimensionD != null)
                    {
                        details.CASE_WT = Convert.ToInt16(item.DimensionD);
                    }
                    i++;
                }
            }

            details.UT_WT = UT_WT;
            details.CASE_WI = CASE_WI;
            if (BOX_CASE == 0)
            {
                BOX_CASE = unitPerCase;
            }
            var qty = Qty;
            nrBoxes = (int)(qty / BOX_CASE);
            if ((qty % BOX_CASE) > 0)
            {
                nrBoxes += 1;
            }
            if (nrBoxes < 1)
            {
                nrBoxes = 1;
            }
            itemsInLastBox = (int)(qty % BOX_CASE);
            fullBoxWeight = "1";
            if ((qty / BOX_CASE) > 0)
            {
                fullBoxWeight = Math.Ceiling(BOX_CASE * UT_WT).ToString();
            }
            partialBoxWeight = "1";
            if (itemsInLastBox > 0)
            {
                partialBoxWeight = Math.Ceiling(itemsInLastBox * UT_WT).ToString();
            }

            valuePerFullBox = qty >= BOX_CASE ? (int)(BOX_CASE * (unitPrice * ((decimal)0.60))) : 0;
            int diff = valuePerFullBox % 100;
            if (diff > 0)
            {
                valuePerFullBox = valuePerFullBox + (100 - diff);
            }
            valuePerPartialBox = (int)(itemsInLastBox * (unitPrice * ((decimal)0.60)));
            diff = valuePerPartialBox % 100;
            if (diff > 0)
            {
                valuePerPartialBox = valuePerPartialBox + (100 - diff);
            }
            return details;
        }

        #region UPS RATE SERVICE API

        private List<ResultData> GetRateFromUPS(int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, UPSWrappers.inv_detl details, decimal unitPrice, string shipToPostalCode, List<ResultData> lst, out string currency, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var rateServiceWrapper = new UPSRateServiceWrapper(UPSConstants.UpsUserName, UPSConstants.UpsPasword,
                    UPSConstants.UpsAccessLicenseNumber, UPSConstants.UpsShipperNumber, UPSConstants.UpsShipperName,
                    UPSConstants.UpsCustomerTypeCode, UPSConstants.UpsCustomerTypeDescription, UPSConstants.UpsShipperAddressLine,
                    UPSConstants.UpsShipperCity, UPSConstants.UpsShipperPostalCode, UPSConstants.UpsShipperStateProvinceCode,
                    UPSConstants.UpsShipperCountryCode, shipToPostalCode, "US", null, null, null, null, UPSConstants.UpsShipFromAddressLine,
                    UPSConstants.UpsShipFromCity, UPSConstants.UpsShipFromPostalCode, UPSConstants.UpsShipFromStateProvinceCode,
                    UPSConstants.UpsShipFromCountryCode, UPSConstants.UpsShipFromName,
                    UPSConstants.UpsShipperNumber, UPSConstants.UpsPackagingType);
                var transitTimeWrapper = new UPSTimeInTransitWrapper(UPSConstants.UpsUserName, UPSConstants.UpsPasword,
                    UPSConstants.UpsAccessLicenseNumber,
                    UPSConstants.UpsCustomerTypeCode, UPSConstants.UpsCustomerTypeDescription, shipToPostalCode, "US", null, null, null,
                    UPSConstants.UpsShipFromCity, UPSConstants.UpsShipFromPostalCode, UPSConstants.UpsShipFromStateProvinceCode,
                    UPSConstants.UpsShipFromCountryCode, UPSConstants.UpsShipFromName);

                foreach (ResultData r in lst)
                {
                    try
                    {
                        var rateResponse = rateServiceWrapper.CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, "02", "USD", unitPrice, true);//,out requestXML);

                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                currency = " " + rshipment.TotalCharges.CurrencyCode;
                                var rate = Math.Round
                                    (decimal.Parse(rshipment.TotalCharges.MonetaryValue, CultureInfo.InvariantCulture) + decimal.Parse(rshipment.TotalCharges.MonetaryValue, CultureInfo.InvariantCulture) * 0.15m);
                                r.cost += rate;

                                r.Publishedcost += decimal.Parse(rshipment.TotalCharges.MonetaryValue, CultureInfo.InvariantCulture);


                                r.Negcost += decimal.Parse(rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue, CultureInfo.InvariantCulture);

                                if (rshipment.GuaranteedDelivery != null
                                    && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                {
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                                }
                            }
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {

                        errorMessage += ex.Detail.InnerText;
                    }
                }
                try
                {
                    var titResponse = transitTimeWrapper.CallUPSTimeInTransitRequest(Qty, nrBoxes, fullBoxWeight, partialBoxWeight, "USD", unitPrice, true);
                    var groundService =
                       ((UPSTimeInTransit.TransitResponseType)titResponse.Item).ServiceSummary.FirstOrDefault(
                           i => i.Service.Code == "GND");
                    var groundResult = lst.FirstOrDefault(i => i.code == "03");
                    groundResult.time = groundService.EstimatedArrival.BusinessDaysInTransit + " days";
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    errorMessage += ex.Detail.InnerText;
                }

                foreach (ResultData r in lst)
                {
                    var upsService = new RateService();
                    try
                    {
                        var rateResponse = rateServiceWrapper.CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, "02", "USD", unitPrice, false);
                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                currency = " " + rshipment.TotalCharges.CurrencyCode;
                                var rate =
                                    Math.Round(
                                        decimal.Parse(
                                            rshipment.TotalCharges.MonetaryValue,
                                            CultureInfo.InvariantCulture)
                                        + decimal.Parse(
                                            rshipment.TotalCharges.MonetaryValue,
                                            CultureInfo.InvariantCulture) * 0.15m);
                                r.cost += rate;

                                r.Publishedcost += decimal.Parse(rshipment.TotalCharges.MonetaryValue, CultureInfo.InvariantCulture);

                                r.Negcost += decimal.Parse(rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue, CultureInfo.InvariantCulture);

                                if (rshipment.GuaranteedDelivery != null
                                    && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                {
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                                }
                            }
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                        errorMessage += ex.Detail.InnerText;
                    }
                }
                try
                {
                    var titResponse = transitTimeWrapper.CallUPSTimeInTransitRequest(Qty, nrBoxes, fullBoxWeight, partialBoxWeight, "USD", unitPrice, false);
                    var groundService =
                       ((UPSTimeInTransit.TransitResponseType)titResponse.Item).ServiceSummary.FirstOrDefault(
                           i => i.Service.Code == "GND");
                    var groundResult = lst.FirstOrDefault(i => i.code == "03");
                    if (groundResult != null)
                    {
                        if (groundService != null)
                        {
                            groundResult.time = groundService.EstimatedArrival.BusinessDaysInTransit + " days";
                        }
                    }
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                }

                currency = "US";
                return lst;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                currency = "US";
                return null;
            }
        }



        #endregion

        private void ValidateAddress(Invoice invoice, ref string szError, ref string ValidateAddresResult, ref string txtPShipToCountryCode, ref string txtPShipToStateCode, ref string txtPShipToCity)
        {
            try
            {
                XAVService.XAVService xavService = new XAVService.XAVService();

                XAVResponse xavResponse = CallUPSXAVRequest(xavService, invoice);
                if (xavResponse.Candidate != null && xavResponse.Candidate[0] != null && xavResponse.Candidate[0].AddressClassification != null && xavResponse.Candidate[0].AddressClassification.Code != null)
                {
                    switch (xavResponse.Candidate[0].AddressClassification.Code)
                    {
                        case "0":
                            //lblValidateAddressResult.Text = "Unclassified";
                            ValidateAddresResult = "Unclassified";
                            break;
                        case "1":
                            //lblValidateAddressResult.Text = "Commercial";
                            ValidateAddresResult = "Commercial";
                            break;
                        case "2":
                            //lblValidateAddressResult.Text = "Residential";
                            ValidateAddresResult = "Residential";
                            break;
                    }
                    if (xavResponse.Candidate[0].AddressKeyFormat != null)
                    {
                        if (xavResponse.Candidate[0].AddressKeyFormat.CountryCode != null)
                        {
                            //txtPShipToCountryCode.Text = xavResponse.Candidate[0].AddressKeyFormat.CountryCode;
                            txtPShipToCountryCode = xavResponse.Candidate[0].AddressKeyFormat.CountryCode;
                        }
                        if (xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision1 != null)
                        {
                            //txtPShipToStateCode.Text = xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision1;
                            txtPShipToStateCode = xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision1;
                        }
                        if (xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision2 != null)
                        {
                            //txtPShipToCity.Text = xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision2;
                            txtPShipToCity = xavResponse.Candidate[0].AddressKeyFormat.PoliticalDivision2;
                        }
                    }
                }
                else
                    //MessageBox.Show("Error processing API Validate Address call: invalid address", "UPS API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    szError = string.Format("Error processing API Validate Address call: invalid address");
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                string message = string.Empty;
                message = message + Environment.NewLine + "SoapException Message= " + ex.Message;
                message = message + Environment.NewLine + "SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText;
                message = message + Environment.NewLine + "SoapException XML String for all= " + ex.Detail.LastChild.OuterXml;
                message = message + Environment.NewLine + "SoapException StackTrace= " + ex.StackTrace;
                szError = string.Format("Error processing API Validate Address call (webservice error): {0} UPS API Error: {1}", ex.Message, ex.Detail.LastChild.InnerText);
                //szError = string.Format("Error processing API Validate Address call (webservice error): {0} UPS API Error", message);
                //MessageBox.Show("Error processing API Validate Address call (webservice error): " + message, "UPS API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Log("Error : " + message);
            }
        }
        private static void AddUPSSecurity(XAVService.XAVService xavSvc)
        {
            XAVService.UPSSecurity upss = new XAVService.UPSSecurity();
            AddUPSServiceAccessToken(upss);
            AddUPSUsernameToken(upss);
            xavSvc.UPSSecurityValue = upss;
        }

        private static void AddUPSUsernameToken(XAVService.UPSSecurity upss)
        {
            XAVService.UPSSecurityUsernameToken upssUsrNameToken = new XAVService.UPSSecurityUsernameToken();
            upssUsrNameToken.Username = Settings.Default.UPSUserName;   //young55961
            upssUsrNameToken.Password = Settings.Default.UPSPassword;   //Merced88
            upss.UsernameToken = upssUsrNameToken;
        }

        private static void AddUPSServiceAccessToken(XAVService.UPSSecurity upss)
        {
            XAVService.UPSSecurityServiceAccessToken upssSvcAccessToken = new XAVService.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = Settings.Default.UPSApiKey;    //FCBD8E914895FF36
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private XAVResponse CallUPSXAVRequest(XAVService.XAVService xavService, Invoice invoice)
        {
            string szToCountry = string.Empty;

            XAVRequest xavRequest = new XAVRequest();

            AddUPSSecurity(xavService);

            XAVService.RequestType request = new XAVService.RequestType();
            String[] requestOption = { "3" };
            request.RequestOption = requestOption;
            xavRequest.Request = request;

            AddressKeyFormatType addressKeyFormat = new AddressKeyFormatType();

            //String[] addressKeyFormatItems = { "CA", "Cumming", "95827" };
            //addressKeyFormat.Items = addressKeyFormatItems;

            //addressKeyFormat.AddressLine = new String[] { txtPShipToAddress.Text };
            //addressKeyFormat.PoliticalDivision1 = txtPShipToStateCode.Text;
            //addressKeyFormat.PoliticalDivision2 = txtPShipToCity.Text;
            //addressKeyFormat.PostcodePrimaryLow = txtPShipToPostalCode.Text;

            addressKeyFormat.AddressLine = new String[] { invoice.ToAddress1 };
            addressKeyFormat.PoliticalDivision1 = invoice.ToState;
            addressKeyFormat.PoliticalDivision2 = invoice.ToCity;
            addressKeyFormat.PostcodePrimaryLow = invoice.ToZip;
            szToCountry = invoice.ToCountry;
            szToCountry = szToCountry.Replace("USA", "US");
            addressKeyFormat.CountryCode = szToCountry;


            //addressKeyFormat.Urbanization = txtPShipToCity.Text + " , " + txtPShipToStateCode.Text + " , " + txtPShipToPostalCode.Text;
            addressKeyFormat.ConsigneeName = "Some Consignee";
            //addressKeyFormat.CountryCode = txtPShipFromCountryCode.Text;
            //addressKeyFormat.CountryCode = invoice.FromCountry;
            xavRequest.AddressKeyFormat = addressKeyFormat;

            ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;

            XAVResponse xavResponse = xavService.ProcessXAV(xavRequest);
            return xavResponse;
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        //
        // GET: //ValidateAddress
        public ActionResult ValidateAddressTE(int invoiceId)
        {
            string ValidateAddresResult = string.Empty;
            string txtPShipToCountryCode = string.Empty;
            string txtPShipToStateCode = string.Empty;
            string txtPShipToCity = string.Empty;
            string szError = string.Empty;

            Invoice invoice = db.Invoices.Find(invoiceId);
            if (invoice != null)
            {
                //Validate the address
                //Verify empty fields
                if (string.IsNullOrEmpty(invoice.ToAddress1) || string.IsNullOrEmpty(invoice.ToCity) || string.IsNullOrEmpty(invoice.ToState) || string.IsNullOrEmpty(invoice.ToZip) || string.IsNullOrEmpty(invoice.ToCountry))
                {
                    ValidateAddresResult = string.Format("Error: Address data is missing.");
                }
                else
                {
                    //ValidateAddresResult = "Valid Address";
                    //ValidateAddress(ref szError, ref ValidateAddresResult, ref txtPShipToCountryCode, ref txtPShipToStateCode, ref txtPShipToCity);
                    ValidateAddress(invoice, ref szError, ref ValidateAddresResult, ref txtPShipToCountryCode, ref txtPShipToStateCode, ref txtPShipToCity);

                    if (!string.IsNullOrEmpty(szError))
                    {
                        ValidateAddresResult = string.Format("Error: ");
                        TempData["UPSError"] = szError;
                    }
                }


            }

            return RedirectToAction("Index", "Shipment", new { id = invoiceId, addressresult = ValidateAddresResult });
        }

        public bool CheckExistingShipments(int invoiceId)
        {
            var shipmentsByInvoice = db.Shipments.Where(inv => inv.InvoiceId == invoiceId && inv.Shipped == false);
            var existShipment = shipmentsByInvoice.Any();
            return existShipment;
        }

        public T Clone<T>(T source)
        {
            var dcs = new DataContractSerializer(typeof(T));
            using (var ms = new System.IO.MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }

        [HttpPost]
        public ActionResult DuplicateDetailBox(int invoiceNoId, string itemId)
        {
            var invoiceId = 0;
            try
            {
                var invoiceNo = invoiceNoId.ToString(CultureInfo.InvariantCulture);
                invoiceId = this.db.Invoices.FirstOrDefault(inv => inv.InvoiceNo == invoiceNo).InvoiceId;
                var aShipmentDetail =
                    this.db.ShipmentDetails.FirstOrDefault(shpDetail => shpDetail.Sub_ItemID == itemId);
                var cloneShpDetail = Clone(aShipmentDetail);
                var anInvoiceDetail = this.db.InvoiceDetails.SingleOrDefault(s => s.InvoiceId == invoiceId && s.ItemID == itemId);
                if (anInvoiceDetail != null)
                {
                    var availableItems = anInvoiceDetail.Quantity - anInvoiceDetail.ShipQuantity;
                    if (availableItems > 1)
                    {
                        cloneShpDetail.Quantity = 1;
                        anInvoiceDetail.ShipQuantity = anInvoiceDetail.ShipQuantity + 1;
                        this.db.ShipmentDetails.Add(cloneShpDetail);
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                var message = e.Message;
            }

            return RedirectToAction("GetShipmenDetails", new { invoiceid = invoiceId });
        }

        //
        // GET: /Shipment/AddDetail
        public PartialViewResult AddDetail(string invoiceNoid, int shipmenid = 0)
        {
            int nInvoiceId = 0;
            string szInvoiceNo = string.Empty;
            string szError = string.Empty;

            Shipment shipment = null;
            ShipmentDetails details = null;
            Invoice invoice = null;
            InvoiceDetail invDetails = new InvoiceDetail();

            invoice = db.Invoices.Where(inv => inv.InvoiceNo == invoiceNoid).FirstOrDefault<Invoice>();
            if (invoice == null)
            {
                invoice = new Invoice();
            }

            nInvoiceId = invoice.InvoiceId;
            szInvoiceNo = invoice.InvoiceNo;

            shipment = db.Shipments.Find(shipmenid);
            if (shipment == null)
            {
                shipment = new Shipment();
                shipment.ShipmentDate = DateTime.Now;
                shipment.InvoiceId = nInvoiceId;
                shipment.InvoiceNo = szInvoiceNo;
                db.Shipments.Add(shipment);
                db.SaveChanges();
            }

            //Create the shipment detail
            details = CreateShipmentDetail(shipment, invoice, invDetails, 0, ref szError);
            ViewBag.ShipmentId = shipment.ShipmentId;
            return PartialView(details);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetail(ShipmentDetails shipmentdetails)
        {
            var shipmentDetail = db.ShipmentDetails.FirstOrDefault(shp => shp.ShipmentDetailID == shipmentdetails.ShipmentDetailID);
            var invoiceId = 0;
            var shipment = db.Shipments.FirstOrDefault(shp => shp.ShipmentId == shipmentdetails.ShipmentId);

            if (shipment == null)
            {
                return this.RedirectToAction("GetShipmenDetails", new { invoiceid = invoiceId });
            }

            this.ViewBag.ShipmentId = shipment.ShipmentId;

            invoiceId = Convert.ToInt32(shipment.InvoiceId);

            if (shipmentDetail != null)
            {
                this.db.Entry(shipmentdetails).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            else
            {
                this.db.ShipmentDetails.Add(shipmentdetails);
                this.db.SaveChanges();
            }

            return RedirectToAction("GetShipmenDetails", new { invoiceid = invoiceId });
        }

        private static Dictionary<string, int> GetItemsTotalSum(List<ShipmentDetails> shipmentDetailForm)
        {
            var totalSumByItem = new Dictionary<string, int>();
            foreach (var shpDetail in shipmentDetailForm)
            {
                if (totalSumByItem.ContainsKey(shpDetail.Sub_ItemID))
                {
                    continue;
                }

                var totalSum = shipmentDetailForm.Where(x => x.Sub_ItemID == shpDetail.Sub_ItemID).Sum(t => t.Quantity);
                if (totalSum == null)
                {
                    continue;
                }

                totalSumByItem.Add(shpDetail.Sub_ItemID, (int)totalSum);
            }

            return totalSumByItem;
        }

        [HttpPost]
        public ActionResult UpdateDetail(List<ShipmentDetails> shipmentDetailForm)
        {
            var fullErrorMessage = string.Empty;
            Shipment actualShipment = null;
            var errorItems = new HashSet<string>();

            if (shipmentDetailForm.Any())
            {
                var actualShipmentId = shipmentDetailForm[0].ShipmentId;
                actualShipment = this.db.Shipments.SingleOrDefault(x => x.ShipmentId == actualShipmentId);
            }

            try
            {
                var totalSum = GetItemsTotalSum(shipmentDetailForm);
                foreach (var shipmentDetail in shipmentDetailForm)
                {
                    int qtyForItem;
                    int? qtydiff = 0;
                    var actualInvoiceDetail =
                        db.InvoiceDetails.FirstOrDefault(x => x.ItemID == shipmentDetail.Sub_ItemID);

                    totalSum.TryGetValue(shipmentDetail.Sub_ItemID, out qtyForItem);



                    if (actualInvoiceDetail == null || !(actualInvoiceDetail.Quantity >= qtyForItem))
                    {
                        errorItems.Add(shipmentDetail.Sub_ItemID);
                        continue;
                    }

                    actualInvoiceDetail.ShipQuantity = qtyForItem;
                    this.db.Entry(shipmentDetail).State = EntityState.Modified;
                    this.db.Entry(actualInvoiceDetail).State = EntityState.Modified;
                }

                this.db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorMessage = dbEx.EntityValidationErrors;

                foreach (var validationResult in errorMessage)
                {
                    var entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (var error in validationResult.ValidationErrors)
                    {
                        fullErrorMessage += entityName + "." + error.PropertyName + ": " + error.ErrorMessage + Environment.NewLine;
                    }
                }
            }

            if (errorItems.Any())
            {
                fullErrorMessage = "Please.Choose less quantity for items: " + string.Join<string>(",", errorItems);
            }

            TempData["ErrorMessages"] = fullErrorMessage;
            return RedirectToAction("GetShipmenDetails", new { invoiceid = actualShipment == null ? 0 : actualShipment.InvoiceId });
        }


        [HttpPost]
        public ActionResult Delete(int shipmentDetailId = 0)
        {
            int invoiceId = 0;

            Shipment shipment = null;

            var details = db.ShipmentDetails.Find(shipmentDetailId);

            if (details == null)
            {
                return this.RedirectToAction("GetShipmenDetails", new { invoiceid = invoiceId });
            }

            shipment = this.db.Shipments.Find(details.ShipmentId);
            if (shipment != null)
            {
                invoiceId = Convert.ToInt32(shipment.InvoiceId);
                var anInvoice = db.Invoices.SingleOrDefault(x => x.InvoiceId == invoiceId);
                var minusQuantity = details.Quantity;
                if (details.DetailId != null)
                {
                    this.UpdateInvoiceQuantity(anInvoice, (int)minusQuantity, (int)details.DetailId);
                }
            }

            this.db.ShipmentDetails.Remove(details);
            this.db.SaveChanges();
            this.ReorderBoxesNames(details.ShipmentId);

            return RedirectToAction("GetShipmenDetails", new { invoiceid = invoiceId });
        }

        public JsonResult GetInvoiceRemainingQuantity(int invDetailId)
        {
            var remainingQuantity = -1;
            var invDetail = this.db.InvoiceDetails.FirstOrDefault(x => x.InvoiceId == invDetailId);

            if (invDetail == null)
            {
                var invoiceNo = invDetailId.ToString(CultureInfo.InvariantCulture);
                var firstOrDefaultInvoice = this.db.Invoices.FirstOrDefault(inv => inv.InvoiceNo == invoiceNo);
                if (firstOrDefaultInvoice != null)
                {
                    var invoiceId = firstOrDefaultInvoice.InvoiceId;
                    invDetail = db.InvoiceDetails.FirstOrDefault(inv => inv.InvoiceId == invoiceId);
                }
            }

            if (invDetail == null)
            {
                invDetail = db.InvoiceDetails.FirstOrDefault(inv => inv.Id == invDetailId);
            }

            if (invDetail != null)
            {
                remainingQuantity = (int)(invDetail.Quantity - invDetail.ShipQuantity);
            }

            return this.Json(remainingQuantity, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultipleInvoiceRemainingQuantity(string shipDetailIdList)
        {
            var listRemainingQuantity = string.Empty;
            var listShipId = JsonConvert.DeserializeObject<List<string>>(shipDetailIdList);
            var quantiyOfShpDetail = new Dictionary<int, string>();
            try
            {
                if (listShipId.Any())
                {
                    for (var index = 0; index < listShipId.Count(); index++)
                    {
                        var tempShipmenDetailId = int.Parse(listShipId[index]);

                        var invDetail = this.db.InvoiceDetails.FirstOrDefault(x => x.Id == tempShipmenDetailId);

                        if (invDetail != null)
                        {
                            if (invDetail.ShipQuantity != null)
                            {
                                int? remainingQty = invDetail.Quantity - (int)invDetail.ShipQuantity;
                                quantiyOfShpDetail.Add(
                                    tempShipmenDetailId,
                                    remainingQty.ToString());
                            }
                        }
                    }
                }

                listRemainingQuantity = JsonConvert.SerializeObject(quantiyOfShpDetail);
            }
            catch (Exception e)
            {
                var mssg = e.Message;
            }

            return this.Json(listRemainingQuantity, JsonRequestBehavior.AllowGet);
        }

        private void ReorderBoxesNames(int shipmentId)
        {
            var detailIndex = 1;

            foreach (var aDetail in this.db.ShipmentDetails.Where(x => x.ShipmentId == shipmentId).ToList())
            {
                aDetail.BoxNo = "Box " + detailIndex++;
            }

            db.SaveChanges();
        }

        private void RedistributeBoxes(int shipmentId, string itemId)
        {
            try
            {
                var shipmentDetailList =
                    db.ShipmentDetails.Where(
                        shpDetail =>
                        shpDetail.ShipmentId == shipmentId
                        && shpDetail.Sub_ItemID == itemId).ToArray();
                ITEM aItem = this.db.ITEMs.SingleOrDefault(aitem => aitem.ItemID == itemId);

                if ((aItem == null) || (shipmentDetailList.Count() <= 1))
                {
                    return;
                }

                var itemsInABox = int.Parse(aItem.UnitPerCase);
                for (var index = 0; index < shipmentDetailList.Count(); index++)
                {
                    if (shipmentDetailList[index].Quantity == 0)
                    {
                        continue;
                    }

                    for (var subindex = 0; subindex < index; subindex++)
                    {
                        var restTocomplete = itemsInABox - shipmentDetailList[index].Quantity;
                        if (!(shipmentDetailList[subindex].Quantity <= restTocomplete))
                        {
                            continue;
                        }

                        shipmentDetailList[index].Quantity = shipmentDetailList[index].Quantity
                                                             + shipmentDetailList[subindex].Quantity;
                        shipmentDetailList[subindex].Quantity = 0;
                        this.db.ShipmentDetails.Remove(shipmentDetailList[subindex]);
                    }
                }

                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                var errorMessage = e.Message;
            }
        }

        //
        // GET:/Shipment/GetShipmenDetails
        [NoCache]
        public PartialViewResult GetShipmenDetails(int? page, string shipmentLog, int invoiceid = 0)
        {

            int nShipmentId = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            string szInvoiceNo = string.Empty;
            string szRate = string.Empty;
            string szDate = string.Empty;
            DateTime dDate = DateTime.Now;
            IPagedList<ShipmentDetails> onePageOfData = null;

            pageIndex = page == null ? 1 : Convert.ToInt32(page);

            if (!string.IsNullOrEmpty(shipmentLog))
            {
                ViewBag.ShipmentLogDetails = true;
            }

            Shipment shipment = null;
            var ShipmentDetailsList = new List<ShipmentDetails>();

            var qryShipmentDetails =
                db.ShipmentDetails.Join(
                    db.Shipments,
                    dtl => dtl.ShipmentId,
                    shp => shp.ShipmentId,
                    (dtl, shp) => new { dtl, shp })
                    .Where(NData => NData.shp.InvoiceId == invoiceid)
                    .OrderBy(NData => NData.dtl.Sub_ItemID);

            if (qryShipmentDetails.Any())
            {
                ShipmentDetailsList.AddRange(from item in qryShipmentDetails where !item.dtl.Shipped select item.dtl);
                var result = new List<string>(ShipmentDetailsList.Select(x => x.Sub_ItemID).ToList().Distinct());
                ViewBag.AvailableDetailShipId = result;
            }

            shipment = db.Shipments.FirstOrDefault(shp => shp.InvoiceId == invoiceid && shp.Shipped == false);
            if ((shipment != null) && ShipmentDetailsList.Any())
            {
                nShipmentId = shipment.ShipmentId;
                szInvoiceNo = shipment.InvoiceNo;
                szRate = shipment.RateResults;
                dDate = Convert.ToDateTime(shipment.ShipmentDate);
                szDate = dDate.ToShortDateString();
                ViewBag.ShipmentTitle = string.Format("Invoice No: {0} shipment Id: {1} Date: {2} Rate Results: {3}", szInvoiceNo, nShipmentId, szDate, szRate);

                if (!shipment.Shipped)
                {
                    onePageOfData = ShipmentDetailsList.ToPagedList(pageIndex, pageSize);
                }

                this.ReorderBoxesNames(shipment.ShipmentId);

            }

            ViewBag.ShipmentTitle = " ";
            ViewBag.OnePageOfData = onePageOfData;
            ViewBag.ShipmentId = nShipmentId;
            ViewBag.InvoiceId = invoiceid;
            return PartialView(onePageOfData);
        }

        [HttpPost]
        public ActionResult ShipItem(string salesorderid, int shipqty, int id = 0)
        {
            var szError = string.Empty;
            Invoice invoice = null;
            InvoiceDetail invDetails;
            var actualInvoiceId = Convert.ToInt32(salesorderid);
            var shipment = this.ShipAll(actualInvoiceId, out invoice, out invDetails, id);

            if (invDetails == null)
            {
                return this.RedirectToAction(
                    "GetShipmenDetails",
                    new { invoiceid = shipment != null ? shipment.InvoiceId : 0 });
            }

            var details = this.CreateShipmentDetail(shipment, invoice, invDetails, shipqty, ref szError);

            if ((details != null) && string.IsNullOrEmpty(szError))
            {
                this.UpdateInvoiceQuantity(invoice, shipqty * -1, id);
                RedistributeBoxes(shipment.ShipmentId, details.Sub_ItemID);
            }


            return RedirectToAction("GetShipmenDetails", new { invoiceid = shipment != null ? shipment.InvoiceId : 0 });
        }


        // GET:/Shipment/ShipAll
        public Shipment ShipAll(int invoiceId, out Invoice invoice, out InvoiceDetail qryInvDetail, int id)
        {
            Shipment shipment = null;
            var existActiveShipments = this.CheckExistingShipments(invoiceId);
            invoice = db.Invoices.FirstOrDefault(inv => inv.InvoiceId == invoiceId);
            if (invoice != null)
            {
                if (!existActiveShipments)
                {
                    shipment = new Shipment
                                   {
                                       ShipmentDate = DateTime.Now,
                                       InvoiceId = invoice.InvoiceId,
                                       InvoiceNo = invoice.InvoiceNo

                                   };

                    db.Shipments.Add(shipment);
                    db.SaveChanges();
                }
                else
                {
                    shipment = db.Shipments.SingleOrDefault(c => c.InvoiceId == invoiceId && c.Shipped != true);
                    if (shipment != null)
                    {
                        this.ReorderBoxesNames(shipment.ShipmentId);
                        qryInvDetail =
                            db.InvoiceDetails.SingleOrDefault(
                                invdtl => invdtl.InvoiceId == invoiceId && invdtl.Id == id);
                        return shipment;
                    }
                }
            }

            qryInvDetail = null;
            return shipment;
        }


        private ShipmentDetails CreateShipmentDetail(Shipment shipment, Invoice invoice, InvoiceDetail invDetails, int quantityShipped, ref string szError)
        {
            int nrBoxes = 0;
            int itemsInLastBox = 0;
            int nValuePerFullBox = 0;
            int diff = 0;
            int nValuePerPartialBox = 0;
            int nPartialBoxWeight = 0;
            int nUnitperCase = 0;
            decimal dUnitWeigth = 1;
            decimal dfullBoxWeight = 1;
            decimal dHlp = 1;
            string fullBoxWeight = string.Empty;
            string partialBoxWeight = string.Empty;
            string szError01 = string.Empty;
            string szCustomerNo = string.Empty;
            string szError02 = string.Empty;
            ITEM item = null;
            ShipmentDetails shipmentDetails = null;

            TimelyDepotContext db01 = new TimelyDepotContext();

            try
            {
                Customers customer = db01.Customers.Find(invoice.CustomerId);
                if (customer == null)
                {
                    szCustomerNo = string.Empty;
                }
                else
                {
                    szCustomerNo = customer.CustomerNo;
                }

                if (invDetails.Id == 0)
                {
                    var shipmentDetailList =
                        db01.ShipmentDetails.Where(cf => cf.ShipmentId == shipment.ShipmentId);
                    var shDetailCount = shipmentDetailList.Count() + 1;
                    shipmentDetails = new ShipmentDetails
                                          {
                                              DetailId = invDetails.Id,
                                              DimensionD = 1,
                                              DimensionH = 1,
                                              DimensionL = 1,
                                              BoxNo = "Box " + shDetailCount,
                                              Sub_ItemID = invDetails.ItemID,
                                              Quantity = quantityShipped,
                                              Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo),
                                              Reference2 = string.Format("{0} {1}", invDetails.Sub_ItemID, quantityShipped),
                                              ShipmentId = shipment.ShipmentId,
                                              UnitPrice = invDetails.UnitPrice,
                                              UnitWeight = 1,
                                              DeclaredValue = 1
                                          };
                }
                else
                {
                    //Get SubItem data
                    item = db01.ITEMs.Where(itm => itm.ItemID == invDetails.ItemID).FirstOrDefault<ITEM>();
                    if (item != null)
                    {
                        //Calculate the shimpment boxes
                        //Number of full boxes
                        if (string.IsNullOrEmpty(item.UnitPerCase))
                        {
                            nrBoxes = 0;
                            nUnitperCase = 0;
                        }
                        else
                        {
                            try
                            {
                                nUnitperCase = Convert.ToInt32(item.UnitPerCase);
                                if (nUnitperCase == 0)
                                {
                                    nrBoxes = 0;
                                }
                                else
                                {
                                    nrBoxes = quantityShipped != 0 ? Convert.ToInt32(quantityShipped) / nUnitperCase : 0;
                                }
                            }
                            catch (Exception err02)
                            {
                                nUnitperCase = 0;
                                nrBoxes = 0;
                                szError02 = err02.Message;
                            }
                        }

                        //Last Box
                        if (nUnitperCase == 0)
                        {
                            itemsInLastBox = 0;
                        }
                        else
                        {
                            itemsInLastBox = quantityShipped != 0 ? Convert.ToInt32(quantityShipped) % nUnitperCase : 0;
                        }

                        //Box weigth
                        if (string.IsNullOrEmpty(item.CaseWeight))
                        {
                            fullBoxWeight = string.Empty;
                            dfullBoxWeight = 1;
                        }
                        else
                        {
                            fullBoxWeight = item.CaseWeight;
                            dfullBoxWeight = Convert.ToDecimal(item.CaseWeight, CultureInfo.InvariantCulture);
                        }

                        //Last Box weigth
                        if (itemsInLastBox > 0)
                        {
                            if (string.IsNullOrEmpty(item.UnitWeight))
                            {
                                dUnitWeigth = 1;
                            }
                            else
                            {
                                dUnitWeigth = Convert.ToDecimal(item.UnitWeight, CultureInfo.InvariantCulture);
                            }
                            nPartialBoxWeight = itemsInLastBox * Convert.ToInt32(dUnitWeigth);
                            partialBoxWeight = nPartialBoxWeight.ToString(CultureInfo.InvariantCulture);
                        }

                        //Declared value
                        nValuePerFullBox = nUnitperCase * Convert.ToInt32(invDetails.UnitPrice);
                        diff = nValuePerFullBox % 100;
                        if (diff > 0)
                        {
                            nValuePerFullBox = nValuePerFullBox + (100 - diff);
                        }

                        nValuePerPartialBox = itemsInLastBox * Convert.ToInt32(invDetails.UnitPrice);
                        diff = nValuePerPartialBox % 100;
                        if (diff > 0)
                        {
                            nValuePerPartialBox = nValuePerPartialBox + (100 - diff);
                        }

                        //Full boxes
                        for (int i = 0; i < nrBoxes; i++)
                        {
                            //Create the shipment detail
                            shipmentDetails = new ShipmentDetails();
                            shipmentDetails.DetailId = invDetails.Id;

                            if (item.CaseDimensionW == null)
                            {
                                shipmentDetails.DimensionD = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionW, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionD = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionH == null)
                            {
                                shipmentDetails.DimensionH = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionH, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionH = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionL == null)
                            {
                                shipmentDetails.DimensionL = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionL, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionL = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            shipmentDetails.BoxNo = string.Format("Box {0}", i + 1);
                            shipmentDetails.Sub_ItemID = invDetails.ItemID;
                            shipmentDetails.Quantity = nUnitperCase;
                            shipmentDetails.Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo);
                            shipmentDetails.Reference2 = string.Format(
                                "{0} {1}",
                                invDetails.Sub_ItemID,
                                nUnitperCase);
                            shipmentDetails.ShipmentId = shipment.ShipmentId;
                            shipmentDetails.UnitPrice = invDetails.UnitPrice;
                            shipmentDetails.DeclaredValue = GetRoundedDeclaredValue(invDetails, nUnitperCase);


                            try
                            {
                                shipmentDetails.UnitWeight = Convert.ToInt32(dfullBoxWeight);
                            }
                            catch (Exception err01)
                            {
                                szError01 = err01.Message;
                                shipmentDetails.UnitWeight = 1;
                            }

                            db01.ShipmentDetails.Add(shipmentDetails);
                        }

                        //The last Box
                        if (itemsInLastBox > 0 && !string.IsNullOrEmpty(partialBoxWeight))
                        {
                            //Create the shipment detail
                            shipmentDetails = new ShipmentDetails();
                            shipmentDetails.DetailId = invDetails.Id;

                            if (item.CaseDimensionW == null)
                            {
                                shipmentDetails.DimensionD = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionW, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionD = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionH == null)
                            {
                                shipmentDetails.DimensionH = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionH, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionH = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionL == null)
                            {
                                shipmentDetails.DimensionL = 1;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionL, CultureInfo.InvariantCulture);
                                shipmentDetails.DimensionL = dHlp == 0 ? 1 : Convert.ToInt32(dHlp);
                            }

                            shipmentDetails.BoxNo = string.Format("Box {0}", (nrBoxes + 1).ToString());
                            shipmentDetails.Sub_ItemID = invDetails.ItemID;
                            shipmentDetails.Quantity = itemsInLastBox;
                            shipmentDetails.Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo);
                            shipmentDetails.Reference2 = string.Format(
                                "{0} {1}",
                                invDetails.Sub_ItemID,
                                itemsInLastBox.ToString());
                            shipmentDetails.ShipmentId = shipment.ShipmentId;
                            shipmentDetails.UnitPrice = invDetails.UnitPrice;
                            shipmentDetails.DeclaredValue = GetRoundedDeclaredValue(invDetails, itemsInLastBox);
                            try
                            {
                                shipmentDetails.UnitWeight = nPartialBoxWeight == 0
                                                                 ? 1
                                                                 : Convert.ToInt32(nPartialBoxWeight);
                            }
                            catch (Exception err01)
                            {
                                szError += err01.Message;
                                shipmentDetails.UnitWeight = 1;
                            }

                            db01.ShipmentDetails.Add(shipmentDetails);
                        }

                        db01.SaveChanges();

                        db01.Dispose();
                    }
                    else
                    {
                        szError += "Sorry, Couln't find that item on DB.";
                    }
                }

            }
            catch (Exception err)
            {
                szError += err.Message;
            }

            return shipmentDetails;
        }

        private static int GetRoundedDeclaredValue(InvoiceDetail invDetails, int quantityShipped)
        {
            decimal? roundedDeclaredValue = (quantityShipped * invDetails.UnitPrice);
            var roundedToHundred = Math.Ceiling((decimal)(roundedDeclaredValue / (decimal)100.0)) * 100;
            var roundedResult = (int)roundedToHundred;

            return roundedResult;
        }

        //
        // GET: /Invoice/Edit/5
        [NoCache]
        public PartialViewResult Edit(int id = 0)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            DateTime dPODate = DateTime.Now;
            string szMsg = string.Empty;

            Customers customer = null;
            CustomersContactAddress soldto = null;
            CustomersBillingDept billto = null;
            CustomersShipAddress shipto = null;
            VendorsContactAddress venaddress = null;
            VendorsSalesContact vendorsalescontact = null;
            SalesOrderBlindShip salesblind = null;
            PurchaseOrders purchaseorder = null;
            SalesOrder salesorder = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            List<SalesOrderDetail> salesdetailList = new List<SalesOrderDetail>();

            IQueryable<Trade> qryTrade = null;
            IQueryable<CustomersContactAddress> qryAddress = null;
            IQueryable<VendorsContactAddress> qryVenAddres = null;
            IQueryable<CustomersShipAddress> qryshipto = null;
            IQueryable<CustomersBillingDept> qryBill = null;
            IQueryable<VendorsSalesContact> qrysalescontact = null;
            IQueryable<SalesOrderDetail> qrysalesdetail = null;
            IQueryable<SalesOrderBlindShip> qryBlind = null;
            IQueryable<CustomersSalesContact> qryCusSal = null;
            IQueryable<Warehouses> qryWarehouse = null;
            IQueryable<Terms> qryTerms = null;

            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return null;
            }

            //Get the dropdown data
            qryTrade = db.Trades.OrderBy(trd => trd.TradeName);
            if (qryTrade.Count() > 0)
            {
                foreach (var item in qryTrade)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.TradeId.ToString(), item.TradeName));
                }
            }
            SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TradeList = tradeselectorlist;


            //Get the SolTo Data
            listSelector = new List<KeyValuePair<string, string>>();
            qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == invoice.CustomerId);
            if (qryAddress.Count() > 0)
            {
                soldto = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (soldto != null)
                {
                    ViewBag.SoldTo = soldto;
                }
            }
            SelectList mainContactlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.MainContactList = mainContactlist;

            //Get the CustomerNo
            if (soldto != null)
            {
                customer = db.Customers.Where(cust => cust.Id == soldto.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    ViewBag.Customer = customer;

                    //Update payment terms
                    if (string.IsNullOrEmpty(invoice.PaymentTerms))
                    {
                        invoice.PaymentTerms = customer.PaymentTerms;
                    }

                    listSelector = new List<KeyValuePair<string, string>>();
                    qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customer.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
                    if (qryCusSal.Count() > 0)
                    {
                        foreach (var item in qryCusSal)
                        {
                            szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                            listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                        }
                    }
                    SelectList cusdefaultlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.SalesContactList = cusdefaultlist;

                    if (string.IsNullOrEmpty(invoice.SalesRep))
                    {
                        invoice.SalesRep = customer.SalesPerson;
                    }
                }
            }
            else
            {
                customer = db.Customers.Where(cust => cust.Id == invoice.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    ViewBag.Customer = customer;

                    //Update payment terms
                    if (string.IsNullOrEmpty(invoice.PaymentTerms))
                    {
                        invoice.PaymentTerms = customer.PaymentTerms;
                    }

                    listSelector = new List<KeyValuePair<string, string>>();
                    qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customer.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
                    if (qryCusSal.Count() > 0)
                    {
                        foreach (var item in qryCusSal)
                        {
                            szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                            listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                        }
                    }
                    SelectList cusdefaultlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.SalesContactList = cusdefaultlist;

                    if (string.IsNullOrEmpty(invoice.SalesRep))
                    {
                        invoice.SalesRep = customer.SalesPerson;
                    }
                }

            }

            listSelector = new List<KeyValuePair<string, string>>();
            qryWarehouse = db.Warehouses.OrderBy(csp => csp.Warehouse);
            if (qryWarehouse.Count() > 0)
            {
                foreach (var item in qryWarehouse)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Warehouse, item.Warehouse));
                }
            }
            SelectList warehouselist = new SelectList(listSelector, "Key", "Value");
            ViewBag.WarehouseList = warehouselist;

            //Get the Bill to data
            listSelector = new List<KeyValuePair<string, string>>();
            qryBill = db.CustomersBillingDepts.Where(ctbi => ctbi.CustomerId == invoice.CustomerId);
            if (qryBill.Count() > 0)
            {
                //billto = qryBill.FirstOrDefault<CustomersBillingDept>();
                //if (billto != null)
                //{
                //    ViewBag.BillTo = billto;
                //}
                foreach (var item in qryBill)
                {
                    if (billto == null)
                    {
                        //billto = qryBill.FirstOrDefault<CustomersBillingDept>();
                        billto = item;
                        ViewBag.BillTo = billto;
                    }

                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                }
            }
            SelectList billtolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.BillToList = billtolist;

            //Get the ship to data
            listSelector = new List<KeyValuePair<string, string>>();
            qryshipto = db.CustomersShipAddresses.Where(ctsp => ctsp.Id == invoice.CustomerId);
            if (qryshipto.Count() > 0)
            {
                //shipto = qryshipto.FirstOrDefault<CustomersShipAddress>();
                //if (shipto != null)
                //{
                //    ViewBag.ShipTo = shipto;
                //}
                foreach (var item in qryshipto)
                {
                    if (shipto == null)
                    {
                        shipto = item;
                        ViewBag.ShipTo = shipto;
                    }
                    if (!string.IsNullOrEmpty(item.ShippingPreference))
                    {
                        listSelector.Add(new KeyValuePair<string, string>(item.ShippingPreference, item.ShippingPreference));
                    }
                }
                SelectList shipvialist = new SelectList(listSelector, "Key", "Value");
                ViewBag.ShipViaList = shipvialist;

            }

            //Get the Vendor address data
            qryVenAddres = db.VendorsContactAddresses.Where(vnad => vnad.VendorId == invoice.VendorId);
            if (qryVenAddres.Count() > 0)
            {
                venaddress = qryVenAddres.FirstOrDefault<VendorsContactAddress>();
                if (venaddress != null)
                {
                    ViewBag.VendorAddress = venaddress;
                }
            }

            //Get the sales contact
            qrysalescontact = db.VendorsSalesContacts.Where(vdsc => vdsc.Id == invoice.VendorId);
            if (qrysalescontact.Count() > 0)
            {
                vendorsalescontact = qrysalescontact.FirstOrDefault<VendorsSalesContact>();
                if (vendorsalescontact != null)
                {
                    ViewBag.VendorSalesContact = vendorsalescontact;
                }
            }

            //Get the blind ship addres
            qryBlind = db.SalesOrderBlindShips.Where(slbd => slbd.SalesOrderId == invoice.SalesOrderId);
            if (qryBlind.Count() > 0)
            {
                salesblind = qryBlind.FirstOrDefault<SalesOrderBlindShip>();
                if (salesblind != null)
                {
                    ViewBag.BlindShip = salesblind;
                }
            }

            //Get the Purchase order
            if (string.IsNullOrEmpty(invoice.PurchaseOrderNo))
            {
                purchaseorder = db.PurchaseOrders.Where(po => po.SalesOrderNo == invoice.SalesOrderNo).FirstOrDefault<PurchaseOrders>();
                if (purchaseorder != null)
                {
                    invoice.PurchaseOrderNo = purchaseorder.PurchaseOrderNo;
                    if (purchaseorder.PODate != null)
                    {
                        dPODate = Convert.ToDateTime(purchaseorder.PODate);
                        ViewBag.PODate = dPODate.ToString("MM/dd/yyyy");
                    }
                }
                else
                {
                    ViewBag.PODate = string.Empty;
                }
            }

            //Get the sales order
            if (!string.IsNullOrEmpty(invoice.SalesOrderNo))
            {
                salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == invoice.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                    ViewBag.SalesOrderDate = Convert.ToDateTime(salesorder.SODate).ToString("MM/dd/yyyy");
                }
                else
                {
                    ViewBag.SalesOrderDate = string.Empty;
                }
            }
            else
            {
                ViewBag.SalesOrderDate = string.Empty;
            }

            //Get the totals
            //GetInvoiceTotals(invoice.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            ViewBag.SalesAmount = dSalesAmount.ToString("C");
            ViewBag.TotalTax = dTotalTax.ToString("C");
            ViewBag.Tax = dTax.ToString("F2");
            ViewBag.TotalAmount = dTotalAmount.ToString("C");
            ViewBag.BalanceDue = dBalanceDue.ToString("C");

            //Get the terms data
            listSelector = new List<KeyValuePair<string, string>>();
            qryTerms = db.Terms.OrderBy(trm => trm.Term);
            if (qryTerms.Count() > 0)
            {
                foreach (var item in qryTerms)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Term, item.Term));
                }
                SelectList termslist = new SelectList(listSelector, "Key", "Value");
                ViewBag.TermsList = termslist;
            }

            ViewBag.HayShipments = "0";
            IQueryable<Shipment> qryShipment = null;

            qryShipment = db.Shipments.Where(shp => shp.InvoiceId == id && shp.Shipped != true);
            if (qryShipment.Count() > 0)
            {
                ViewBag.HayShipments = id;
            }

            invoice.CustomerShipLocation = ViewBag.SoldTo.CompanyName;

            return PartialView(invoice);
        }


        //
        // POST: /Invoice/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Edit(Invoice invoice)
        {
            var msgResult = "Success";

            if (ModelState.IsValid)
            {
                try
                {

                    db.Entry(invoice).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    msgResult = e.Message;
                }
            }

            return PartialView(invoice);
        }
        //
        // GET: /Invoice/Edit0
        public ActionResult Edit0(string errorMsg)
        {
            ViewBag.ErrorMsg = errorMsg;

            return View();
        }


        //
        // GET: /Invoice/

        public PartialViewResult SelectInvoice(int? page, string searchItem, string ckActive, string ckCriteria, string sortOrder)
        {
            bool bHasData = true;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            string[] szFecha = null;
            DateTime dFecha = DateTime.Now;
            IQueryable<Invoice> qryInvoice = null;

            List<Invoice> InvoiceList = new List<Invoice>();
            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "invoice";
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");
                bHasData = false;
                ViewBag.SearchItem = searchItem;
                bHasData = true;

                if (ckCriteria == "invoice")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.OrderBy(vd => vd.InvoiceNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.OrderBy(vd => vd.InvoiceNo);
                    }
                }

            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;

                if (ckCriteria == "invoice")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.InvoiceNo.StartsWith(searchItem)).OrderBy(vd => vd.InvoiceNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.InvoiceNo.StartsWith(searchItem)).OrderBy(vd => vd.InvoiceNo);
                    }
                }

                if (ckCriteria == "salesorder")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.SalesOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.SalesOrderNo);
                    }
                }

                if (ckCriteria == "shippeddate")
                {
                    szFecha = searchItem.Split('/');
                    if (szFecha != null)
                    {
                        nYear = Convert.ToInt32(szFecha[0]);
                        nMonth = Convert.ToInt32(szFecha[1]);
                        nDay = Convert.ToInt32(szFecha[2]);
                        dFecha = new DateTime(nYear, nMonth, nDay);
                    }

                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.SalesOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.SalesOrderNo);
                    }
                }

                if (ckCriteria == "customerpo")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "customername")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Invoices.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                InvoiceList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Invoices.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                InvoiceList.Add(item.ctc);
                            }
                        }
                    }
                    bHasData = false;
                }
            }



            if (bHasData)
            {
                if (qryInvoice != null)
                {
                    if (qryInvoice.Count() > 0)
                    {
                        foreach (var item in qryInvoice)
                        {
                            InvoiceList.Add(item);
                        }
                    }

                }
            }
            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }

            //Sorting.
            ViewBag.InvoiceNoSortOrder = (string.IsNullOrEmpty(sortOrder) || sortOrder == "InvoiceNoAsc") ? "InvoiceNoDesc" : "InvoiceNoAsc";
            ViewBag.SalesOrderNoSortOrder = (sortOrder == "SalesOrderNoAsc") ? "SalesOrderNoDesc" : "SalesOrderNoAsc";
            ViewBag.CustomerNameSortOrder = (sortOrder == "CustomerNameAsc") ? "CustomerNameDesc" : "CustomerNameAsc";
            switch (sortOrder)
            {
                case "InvoiceNoDesc":
                    InvoiceList = InvoiceList.OrderByDescending(s => s.InvoiceNoSortData).ToList();
                    break;
                case "InvoiceNoAsc":
                    InvoiceList = InvoiceList.OrderBy(s => s.InvoiceNoSortData).ToList();
                    break;
                case "SalesOrderNoDesc":
                    InvoiceList = InvoiceList.OrderByDescending(s => s.SalesOrderNo).ToList();
                    break;
                case "SalesOrderNoAsc":
                    InvoiceList = InvoiceList.OrderBy(s => s.SalesOrderNo).ToList();
                    break;
                case "CustomerNameDesc":
                    InvoiceList = InvoiceList.OrderByDescending(s => s.CustomerId).ToList();
                    break;
                case "CustomerNameAsc":
                    InvoiceList = InvoiceList.OrderBy(s => s.CustomerId).ToList();
                    break;
                default:
                    InvoiceList = InvoiceList.OrderBy(s => s.InvoiceNoSortData).ToList();
                    break;
            }

            var onePageOfData = InvoiceList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(InvoiceList.ToPagedList(pageIndex, pageSize));

        }

        //
        // GET: /Shipment/

        public ActionResult Index(string id, string addressresult, string ckCriteriaLog)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var parsedId = int.Parse(id);

                var aShipmentDetail = this.db.ShipmentDetails.Join(this.db.Shipments, dtl => dtl.ShipmentId, shp => shp.ShipmentId, (dtl, shp) => new { dtl, shp }).Where(NData => NData.shp.InvoiceId == parsedId && NData.dtl.Shipped == false).ToList();

                if (aShipmentDetail.Any())
                {
                    this.ViewBag.ActualShipmentId = aShipmentDetail[0].dtl.ShipmentId;
                }
            }

            var dFecha = DateTime.Now;

            if (!string.IsNullOrEmpty(addressresult))
            {
                ViewBag.AddressResult = addressresult;
            }
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.LoadInvoiceNo = id;
            }
            if (TempData["UPSError"] != null)
            {
                ViewBag.UPSError = TempData["UPSError"].ToString();
            }

            if (string.IsNullOrEmpty(ckCriteriaLog))
            {
                ViewBag.SearchItemLog = "0";
                ViewBag.ckCriteriaHlpLog = "invoice";
            }


            ViewBag.CurrentDateLog = dFecha.ToString("yyyy/MM/dd");
            return View();
        }

    }
}
