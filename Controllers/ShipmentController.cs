using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;
using PagedList;
using TimelyDepotMVC.CommonCode;
using System.Data;

using TimelyDepotMVC.XAVService;
using TimelyDepotMVC.UPSWrappers;
using TimelyDepotMVC.Properties;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TimelyDepotMVC.Controllers
{
    using System.Data.Entity;
    using System.Globalization;
    using System.Reflection.Emit;
    using System.Threading.Tasks;
    using System.Web.Routing;

    using AutoMapper;

    using PdfReportSamples.Models;

    using TimelyDepotMVC.Helpers;
    using TimelyDepotMVC.Models;
    using TimelyDepotMVC.ModelsView;
    using TimelyDepotMVC.UPSRateService;
    using TimelyDepotMVC.UPSShipService;

    public class ShipmentController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

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

            IQueryable<Shipment> qryShipment = null;

            List<Shipment> ShipmentList = new List<Shipment>();
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
                        foreach (var item in qryShipment)
                        {
                            ShipmentList.Add(item);
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

        public PartialViewResult ProcessShipmentInformation(string invoiceId)
        {

            string szShipperNumber = string.Empty;
            List<ShipmentDetails> listShipmentDetails = null;
            szShipperNumber = Settings.Default.UPSShipperNumber;
            ViewBag.ShipperNumber = szShipperNumber;
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
            listSelector.Add(new KeyValuePair<string, string>("01", "Next Day Air"));
            listSelector.Add(new KeyValuePair<string, string>("02", "2nd Day Air"));
            listSelector.Add(new KeyValuePair<string, string>("03", "Ground"));
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
            listSelector.Add(new KeyValuePair<string, string>("00", "Unknown"));
            listSelector.Add(new KeyValuePair<string, string>("01", "UPS Letter"));
            listSelector.Add(new KeyValuePair<string, string>("02", "Package/customer supplied"));
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

        public ActionResult ProcessShipment(string serviceCode, int shipmentId, string invoiceNo)
        {
            ShipmentResponse rateResponse = null;
            string szError = null;
            var selectedInvoice = db.Invoices.SingleOrDefault(x => x.InvoiceNo == invoiceNo);

            if (selectedInvoice != null)
            {
                var shipmentRequestDto = Mapper.Map<ShipmentRequestView>(selectedInvoice);
                shipmentRequestDto.userName = UPSConstants.UpsUserName;
                shipmentRequestDto.password = UPSConstants.UpsPasword;
                shipmentRequestDto.accessLicenseNumber = UPSConstants.UpsAccessLicenseNumber;
                shipmentRequestDto.shipperNumber = UPSConstants.UpsShipperNumber;
                shipmentRequestDto.packagingTypeCode = UPSConstants.UpsPackagingType;
                shipmentRequestDto.shipmentChargeType = UPSConstants.UpsShipmentChargeType;
                shipmentRequestDto.billShipperAccountNumber = UPSConstants.UpsShipperNumber;

                var shipServiceWrapper = new UPSShipServiceWrapper(shipmentRequestDto);

                rateResponse = shipServiceWrapper.CallUPSShipmentRequest(serviceCode, shipmentId, ref szError);

                var result = this.AddUpsDataToShipmentDetail(shipmentId, rateResponse.ShipmentResults.PackageResults);
            }

            if (string.IsNullOrEmpty(szError))
            {
                return Json(rateResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(szError, JsonRequestBehavior.AllowGet);
            }

        }

        public Task<int> AddUpsDataToShipmentDetail(int shipmentId, PackageResultsType[] packageList)
        {
            var aShipmentDetailList =
               db.ShipmentDetails.Where(x => x.ShipmentId == shipmentId).ToList();
            if (!aShipmentDetailList.Any())
            {
                return this.db.SaveChangesAsync();
            }

            for (var packageIndex = 0; packageIndex < packageList.Count(); packageIndex++)
            {
                var aLabel = packageList[packageIndex].ShippingLabel.GraphicImage;
                var trackId = packageList[packageIndex].TrackingNumber;
                aShipmentDetailList[packageIndex].ShipmentLabel = "data:image/jpg;base64," + aLabel;
                aShipmentDetailList[packageIndex].trackId = trackId;
                aShipmentDetailList[packageIndex].Shipped = true;
                this.db.Entry(aShipmentDetailList[packageIndex]).State = EntityState.Modified;
            }

            return this.db.SaveChangesAsync();

        }

        public JsonResult ValidateUPSAccount(string accountNumber, string invoiceNo)
        {
            var selectedInvoice = db.Invoices.SingleOrDefault(x => x.InvoiceNo == invoiceNo);
            string result = "Not a valid UPS Account.";
            try
            {
                UPSWrappers.inv_detl inv_detl = new UPSWrappers.inv_detl();
                inv_detl.CASE_HI = 24;
                inv_detl.CASE_LEN = 24;
                inv_detl.CASE_WI = 42;
                inv_detl.CASE_WT = 19;
                inv_detl.UT_WT = Convert.ToDecimal(2.1);

                if (selectedInvoice != null)
                {
                    var shipmentRequestDto = Mapper.Map<ShipmentRequestView>(selectedInvoice);
                    shipmentRequestDto.userName = UPSConstants.UpsUserName;
                    shipmentRequestDto.password = UPSConstants.UpsPasword;
                    shipmentRequestDto.accessLicenseNumber = UPSConstants.UpsAccessLicenseNumber;
                    shipmentRequestDto.shipperNumber = UPSConstants.UpsShipperNumber;
                    shipmentRequestDto.packagingTypeCode = UPSConstants.UpsPackagingType;
                    shipmentRequestDto.shipmentChargeType = UPSConstants.UpsShipmentChargeType;
                    shipmentRequestDto.billShipperAccountNumber = UPSConstants.UpsShipperNumber;
                    var rateServiceWrapper = new UPSRateServiceWrapper(shipmentRequestDto);

                    var transitTimeWrapper = new UPSTimeInTransitWrapper(UPSConstants.UpsUserName, UPSConstants.UpsPasword,
                        UPSConstants.UpsAccessLicenseNumber,
                        UPSConstants.UpsCustomerTypeCode, UPSConstants.UpsCustomerTypeDescription, "94306", "US", null, null, null,
                        UPSConstants.UpsShipFromCity, UPSConstants.UpsShipFromPostalCode, UPSConstants.UpsShipFromStateProvinceCode,
                        UPSConstants.UpsShipFromCountryCode, UPSConstants.UpsShipFromName);


                    var rateResponse = rateServiceWrapper.CallUPSRateRequest("03", 25, 2, 5, "42", 400, 100, "11", inv_detl, "02", "USD", Convert.ToDecimal(26.5), true);//,out requestXML);

                    result = "This is a valid UPS Account.";
                }
            }
            catch
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET:/Shipment/GetUPSRateTE
        //public JsonResult GetUPSRateTE(int invoiceId, string ValidateAddresResult, string itemNo, string quantity, string unitPriceValue, string customerType, string serviceType, string packageType)
        public PartialViewResult GetUPSRateTE(string ItemId, string quantity, string shipToPostalCode)
        {
            //Invoice invoice = db.Invoices.Find(invoiceId);
            List<ResultData> resultData = new List<ResultData>();
            try
            {
                int unitPerCase = 1;
                //string itemID = Request.QueryString["ItemID"] != null ? Convert.ToString(Request.QueryString["ItemID"]) : string.Empty;
                //DataSet ds = new DataSet();
                //Connection obj = new Connection();
                int BOX_CASE = 0;
                decimal CASE_WI = 0;
                decimal UT_WT = 0;
                decimal unitPrice = 0;//TODO:// need to get from db unitprice
                int Qty;
                int.TryParse(quantity, out Qty);
                //ds = obj.Getdataset("select a.* from (Select *,LTRIM(RTRIM(STR(Price,10,2))) as PriceNew,case PriceType when 'DEFAULT' then 0 when 'Default' then 0 else 0 end as seq From PRICE Where Item='" + itemID + "')a where a.seq='0'  order by    a.Price   desc  ");
                var ds = db.PRICEs.Where(i => i.Item == ItemId).OrderByDescending(i => i.thePrice).ToList();
                //ds = obj.Getdataset("select a.* from (Select *,LTRIM(RTRIM(STR(thePrice,10,2))) as PriceNew,case PriceType when 'DEFAULT' then 0 when 'Default' then 0 else 0 end as seq From PRICE Where Item='" + itemID + "')a where a.seq='0'  order by    a.thePrice   desc  ");
                //if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                if (ds != null && ds.Count > 0)
                {
                    int j = 0;
                    bool hasValue = false;
                    //for (Int32 i = 0; i < ds.Tables[0].Rows.Count; i++)
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
                        j = ds.Count - 1;
                    var dsArray = ds.ToArray();
                    unitPrice = Convert.ToDecimal(dsArray[j].thePrice);
                }

                UPSWrappers.inv_detl details = new UPSWrappers.inv_detl();
                details.CASE_HI = 0;
                details.CASE_LEN = 0;
                details.CASE_WI = 0;
                details.CASE_WT = 0;
                //ds = new DataSet();
                //ds = obj.Getdataset("SELECT [UnitPerCase],[UnitWeight],[CaseWeight],[DimensionH],[DimensionL],[DimensionD] FROM [dbo].[ITEM] where itemid='" + itemID + "'");
                var itemList = db.ITEMs.Where(i => i.ItemID == ItemId).ToList();
                if (itemList.Count > 0)
                {
                    int j = 0;
                    bool hasValue = false;
                    var i = 0;
                    //for (Int32 i = 0; i < ds.Tables[0].Rows.Count; i++)
                    foreach (var item in itemList)
                    {

                        if (item.UnitPerCase != null)
                            BOX_CASE = Convert.ToInt16(item.UnitPerCase);
                        if (item.CaseWeight != null)
                            CASE_WI = Convert.ToDecimal(item.CaseWeight);
                        if (item.UnitWeight != null)
                            UT_WT = Convert.ToDecimal(item.UnitWeight);
                        if (item.DimensionH != null)
                            details.CASE_HI = Convert.ToInt16(item.DimensionH);
                        if (item.DimensionL != null)
                            details.CASE_LEN = Convert.ToInt16(item.DimensionL);
                        if (item.DimensionD != null)
                            details.CASE_WT = Convert.ToInt16(item.DimensionD);
                        i++;
                    }
                }

                details.UT_WT = UT_WT;
                details.CASE_WI = CASE_WI;
                if (BOX_CASE == 0)
                    BOX_CASE = unitPerCase;
                var qty = Convert.ToInt32(quantity);
                int nrBoxes = qty / BOX_CASE;
                if ((qty % BOX_CASE) > 0)
                    nrBoxes += 1;
                if (nrBoxes < 1)
                    nrBoxes = 1;
                int itemsInLastBox = qty % BOX_CASE;
                string fullBoxWeight = "0";
                if ((qty / BOX_CASE) > 0)
                    fullBoxWeight = Math.Ceiling(BOX_CASE * UT_WT).ToString();
                string partialBoxWeight = "0";
                if (itemsInLastBox > 0)
                    partialBoxWeight = Math.Ceiling(itemsInLastBox * UT_WT).ToString();


                int valuePerFullBox = qty >= BOX_CASE ? (int)(BOX_CASE * (unitPrice * ((decimal)0.60))) : 0;
                int diff = valuePerFullBox % 100;
                if (diff > 0)
                    valuePerFullBox = valuePerFullBox + (100 - diff);
                int valuePerPartialBox = (int)(itemsInLastBox * (unitPrice * ((decimal)0.60)));
                diff = valuePerPartialBox % 100;
                if (diff > 0)
                    valuePerPartialBox = valuePerPartialBox + (100 - diff);

                var resultString = string.Empty;
                //if (Request.IsAuthenticated)
                //    if (Roles.IsUserInRole("Admin"))
                //    {
                resultString = "BOX_CASE : " + BOX_CASE;
                resultString += "<br /> unitPrice : " + unitPrice;
                resultString += "<br /> CASE_WI : " + CASE_WI;
                resultString += "<br /> UT_WT : " + UT_WT;
                resultString += "<br /> CASE_HI : " + details.CASE_HI;
                resultString += "<br /> CASE_LEN : " + details.CASE_LEN;
                resultString += "<br /> CASE_WT : " + details.CASE_WT;
                resultString += "<br /> valuePerFullBox : " + valuePerFullBox;
                resultString += "<br /> valuePerPartialBox : " + valuePerPartialBox;
                resultString += "<br /> fullBoxWeight : " + fullBoxWeight;
                resultString += "<br /> partialBoxWeight : " + partialBoxWeight;
                resultString += "<br /> nrBoxes : " + nrBoxes;
                resultString += "<br /> qty : " + qty;
                //}

                //List<ResultData> lst = new List<ResultData>();
                //lst.Add(new ResultData { service = "BOX_CASE", cost = BOX_CASE.ToString("#.##") });

                resultData = GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, shipToPostalCode);

            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message + ex.StackTrace);

            }

            // XML

            string szResult = "The rate transaction was a Success;Total Shipment Charges: 41.20USD;Total Shipment Negociated Charges: 39.32USD days;Delivery time: 2 days";

            return PartialView(resultData);
            //return RedirectToAction("Index", "Shipment", new { id = invoiceId, addressresult = ValidateAddresResult });
        }

        #region UPS RATE SERVICE API

        private List<ResultData> GetRateFromUPS(int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, UPSWrappers.inv_detl details, decimal unitPrice, string shipToPostalCode)
        {
            try
            {
                List<ResultData> lst = new List<ResultData>();


                lst.Add(new ResultData() { service = "UPS Ground", code = "03" });
                lst.Add(new ResultData() { service = "UPS Three-Day Select®", code = "12" });
                lst.Add(new ResultData() { service = "UPS Second Day Air®", code = "02" });
                lst.Add(new ResultData() { service = "UPS Next Day Air®", code = "01" });



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
                        //var rateResponse = CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, upsService,true);

                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                var rate = Math.Round
                                    (decimal.Parse(rshipment.TotalCharges.MonetaryValue) + decimal.Parse(rshipment.TotalCharges.MonetaryValue) * 0.15m);
                                r.cost = rate + " " + rshipment.TotalCharges.CurrencyCode;

                                r.Publishedcost = rshipment.TotalCharges.MonetaryValue + " " + rshipment.TotalCharges.CurrencyCode;


                                r.Negcost = rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue + " " + rshipment.NegotiatedRateCharges.TotalCharge.CurrencyCode;


                                //r.cost = string.Format("Total Charges:{0} {1} | Negociated Rate:{2} {3}", rshipment.TotalCharges.MonetaryValue, rshipment.TotalCharges.CurrencyCode, rshipment.TotalCharges.MonetaryValue, rshipment.TotalCharges.CurrencyCode);
                                if (rshipment.GuaranteedDelivery != null && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                                //if (rshipment.GuaranteedDelivery != null && !String.IsNullOrEmpty(rshipment.GuaranteedDelivery.DeliveryByTime))
                                //    r.time = r.time + " " + rshipment.GuaranteedDelivery.DeliveryByTime + "time";
                            }


                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
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
                }

                foreach (ResultData r in lst)
                {
                    RateService upsService = new RateService();
                    try
                    {
                        var rateResponse = rateServiceWrapper.CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, "02", "USD", unitPrice, false);//,out requestXML);
                        //RateResponse rateResponse = CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, upsService,false);
                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                var rate = Math.Round
                                    (decimal.Parse(rshipment.TotalCharges.MonetaryValue) + decimal.Parse(rshipment.TotalCharges.MonetaryValue) * 0.15m);
                                r.cost = rate + " " + rshipment.TotalCharges.CurrencyCode;


                                r.Publishedcost = rshipment.TotalCharges.MonetaryValue + " " + rshipment.TotalCharges.CurrencyCode;


                                r.Negcost = rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue + " " + rshipment.NegotiatedRateCharges.TotalCharge.CurrencyCode;

                                //r.cost = string.Format("Total Charges:{0} {1} | Negociated Rate:{2} {3}", rshipment.TotalCharges.MonetaryValue, rshipment.TotalCharges.CurrencyCode, rshipment.TotalCharges.MonetaryValue, rshipment.TotalCharges.CurrencyCode);
                                if (rshipment.GuaranteedDelivery != null && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                                //if (rshipment.GuaranteedDelivery != null && !String.IsNullOrEmpty(rshipment.GuaranteedDelivery.DeliveryByTime))
                                //    r.time = r.time + " " + rshipment.GuaranteedDelivery.DeliveryByTime + "time";
                            }
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                    }
                }
                try
                {
                    var titResponse = transitTimeWrapper.CallUPSTimeInTransitRequest(Qty, nrBoxes, fullBoxWeight, partialBoxWeight, "USD", unitPrice, false);
                    var groundService =
                       ((UPSTimeInTransit.TransitResponseType)titResponse.Item).ServiceSummary.FirstOrDefault(
                           i => i.Service.Code == "GND");
                    var groundResult = lst.FirstOrDefault(i => i.code == "03");
                    groundResult.time = groundService.EstimatedArrival.BusinessDaysInTransit + " days";
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                }



                return lst;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
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


        //
        // GET:/Shipment/ShipAll
        public ActionResult ShipAll(int invoiceId)
        {
            string szError = string.Empty;

            Shipment shipment = null;
            ShipmentDetails details = null;
            IQueryable<InvoiceDetail> qryInvDetail = null;

            //Get the invoice
            Invoice invoice = db.Invoices.Where(inv => inv.InvoiceId == invoiceId).FirstOrDefault<Invoice>();
            if (invoice != null)
            {

                shipment = new Shipment();
                shipment.ShipmentDate = DateTime.Now;
                shipment.InvoiceId = invoice.InvoiceId;
                shipment.InvoiceNo = invoice.InvoiceNo;
                db.Shipments.Add(shipment);
                db.SaveChanges();


                qryInvDetail = db.InvoiceDetails.Where(invdtl => invdtl.InvoiceId == invoiceId);
                if (qryInvDetail.Count() > 0)
                {
                    //Ship all invoice details
                    foreach (var item in qryInvDetail)
                    {
                        //Create the shipment detail
                        details = CreateShipmentDetail(shipment, invoice, item, ref szError);
                    }
                }
            }


            return RedirectToAction("Index", "Shipment", new { id = invoiceId });
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
            else
            {
                nInvoiceId = invoice.InvoiceId;
                szInvoiceNo = invoice.InvoiceNo;

            }

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
            details = CreateShipmentDetail(shipment, invoice, invDetails, ref szError);
            ViewBag.ShipmentId = shipment.ShipmentId;
            return PartialView(details);
        }

        //
        // POST: /Shipment/AddDetail/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetail(ShipmentDetails shipmentdetails)
        {

            var shipmentDetail = db.ShipmentDetails.FirstOrDefault(shp => shp.ShipmentDetailID == shipmentdetails.ShipmentDetailID);
            var invoiceId = 0;
            var shipment = db.Shipments.FirstOrDefault(shp => shp.ShipmentId == shipmentdetails.ShipmentId);

            if (shipment != null)
            {
                invoiceId = Convert.ToInt32(shipment.InvoiceId);
                ViewBag.ShipmentId = shipment.ShipmentId;

                if (shipmentDetail != null)
                {
                    if (!this.ModelState.IsValid)
                    {
                        return this.RedirectToAction("Index", "Shipment", new { id = invoiceId });
                    }
                    this.db.Entry(shipmentdetails).State = EntityState.Modified;
                    this.db.SaveChanges();
                }
                else
                {

                    db.ShipmentDetails.Add(shipmentdetails);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Shipment", new { id = invoiceId });
        }

        //
        // GET:/Shipment/Delete
        public ActionResult Delete(int id = 0)
        {
            int invoiceId = 0;
            Shipment shipment = null;

            ShipmentDetails details = db.ShipmentDetails.Find(id);
            if (details != null)
            {
                shipment = db.Shipments.Find(details.ShipmentId);
                if (shipment != null)
                {
                    invoiceId = Convert.ToInt32(shipment.InvoiceId);
                }

                db.ShipmentDetails.Remove(details);
                db.SaveChanges();

                if (invoiceId != 0)
                {
                    return RedirectToAction("Index", "Shipment", new { id = invoiceId });
                }
            }

            return RedirectToAction("Index", "Shipment");
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


            //IQueryable<Shipment> qryShipment = null;
            //qryShipment = db.Shipments.Where(shp => shp.InvoiceId == invoiceid);
            //if (qryShipment.Count() > 0)
            //{

            //}

            if (!string.IsNullOrEmpty(shipmentLog))
            {
                ViewBag.ShipmentLogDetails = true;
            }

            Shipment shipment = null;
            List<ShipmentDetails> ShipmentDetailsList = new List<ShipmentDetails>();

            //qryShipmentDetails = db.ShipmentDetails.Where(shpdtl => shpdtl.);
            var qryShipmentDetails = db.ShipmentDetails.Join(db.Shipments, dtl => dtl.ShipmentId, shp => shp.ShipmentId, (dtl, shp)
                => new { dtl, shp }).Where(NData => NData.shp.InvoiceId == invoiceid).OrderBy(NData => NData.dtl.Sub_ItemID);
            if (qryShipmentDetails.Count() > 0)
            {
                foreach (var item in qryShipmentDetails)
                {
                    if (nShipmentId == 0)
                    {
                        nShipmentId = item.shp.ShipmentId;
                    }
                    ShipmentDetailsList.Add(item.dtl);
                }
            }



            //Verify the shipment only
            ViewBag.ShipmentTitle = " ";
            if (nShipmentId == 0)
            {
                shipment = db.Shipments.Where(shp => shp.InvoiceId == invoiceid).FirstOrDefault<Shipment>();
                if (shipment != null)
                {
                    nShipmentId = shipment.ShipmentId;
                    szInvoiceNo = shipment.InvoiceNo;
                    szRate = shipment.RateResults;
                    dDate = Convert.ToDateTime(shipment.ShipmentDate);
                    szDate = dDate.ToShortDateString();
                    ViewBag.ShipmentTitle = string.Format("Invoice No: {0} Date: {1} Rate Results: {2}", szInvoiceNo, szDate, szRate);
                }
            }
            else
            {
                shipment = db.Shipments.Where(shp => shp.InvoiceId == invoiceid).FirstOrDefault<Shipment>();
                if (shipment != null)
                {
                    nShipmentId = shipment.ShipmentId;
                    szInvoiceNo = shipment.InvoiceNo;
                    szRate = shipment.RateResults;
                    dDate = Convert.ToDateTime(shipment.ShipmentDate);
                    szDate = dDate.ToShortDateString();
                    ViewBag.ShipmentTitle = string.Format("Invoice No: {0} Date: {1} Rate Results: {2}", szInvoiceNo, szDate, szRate);
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


            var onePageOfData = ShipmentDetailsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            ViewBag.ShipmentId = ShipmentDetailsList[0].ShipmentId;
            return PartialView(ShipmentDetailsList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Shipment/ShipItem
        public ActionResult ShipItem(string salesorderid, int id = 0)
        {
            string szError = string.Empty;

            Invoice invoice = null;
            InvoiceDetail invDetails = null;
            Shipment shipment = null;
            ShipmentDetails details = null;

            // Get Invoice details
            invDetails = db.InvoiceDetails.Find(id);
            if (invDetails != null)
            {
                //Get the invoice
                invoice = db.Invoices.Where(inv => inv.InvoiceId == invDetails.InvoiceId).FirstOrDefault<Invoice>();
                if (invoice != null)
                {
                    //Create the shimpent if it does not exit
                    shipment = db.Shipments.Where(shpm => shpm.InvoiceId == invDetails.InvoiceId).FirstOrDefault<Shipment>();
                    if (shipment == null)
                    {
                        shipment = new Shipment();
                        shipment.ShipmentDate = DateTime.Now;
                        shipment.InvoiceId = invDetails.InvoiceId;
                        shipment.InvoiceNo = invoice.InvoiceNo;
                        db.Shipments.Add(shipment);
                        db.SaveChanges();
                    }

                    //Create the shipment detail
                    details = CreateShipmentDetail(shipment, invoice, invDetails, ref szError);
                }
            }

            return RedirectToAction("Index", "Shipment", new { id = salesorderid });
        }

        private ShipmentDetails CreateShipmentDetail(Shipment shipment, Invoice invoice, InvoiceDetail invDetails, ref string szError)
        {
            int nrBoxes = 0;
            int itemsInLastBox = 0;
            int nValuePerFullBox = 0;
            int diff = 0;
            int nValuePerPartialBox = 0;
            int nPartialBoxWeight = 0;
            int nUnitperCase = 0;
            decimal dUnitWeigth = 0;
            decimal dfullBoxWeight = 0;
            decimal dHlp = 0;
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

                //Do not allow duplicates
                shipmentDetails = db01.ShipmentDetails.Where(shpdtl => shpdtl.ShipmentId == shipment.ShipmentId && shpdtl.DetailId == invDetails.Id).FirstOrDefault<ShipmentDetails>();
                if (shipmentDetails != null)
                {
                    szError = string.Format("Shipment Detail exist for item {0}", invDetails.Sub_ItemID);
                    return shipmentDetails;
                }

                if (invDetails.Id == 0)
                {
                    //Create the shipment detail
                    shipmentDetails = new ShipmentDetails();
                    shipmentDetails.DetailId = invDetails.Id;

                    shipmentDetails.DimensionD = 0;

                    shipmentDetails.DimensionH = 0;

                    shipmentDetails.DimensionL = 0;

                    shipmentDetails.BoxNo = string.Empty;
                    shipmentDetails.Sub_ItemID = invDetails.Sub_ItemID;
                    shipmentDetails.Quantity = invDetails.Quantity;
                    shipmentDetails.Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo);
                    shipmentDetails.Reference2 = string.Format("{0} {1}", invDetails.Sub_ItemID, invDetails.Quantity.ToString());
                    shipmentDetails.ShipmentId = shipment.ShipmentId;
                    shipmentDetails.UnitPrice = invDetails.UnitPrice;
                    shipmentDetails.UnitWeight = 0;

                    db01.ShipmentDetails.Add(shipmentDetails);
                    db01.SaveChanges();

                    db01.Dispose();
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
                                    nrBoxes = Convert.ToInt32(invDetails.Quantity) / nUnitperCase;
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
                            itemsInLastBox = Convert.ToInt32(invDetails.Quantity) % nUnitperCase;
                        }

                        //Box weigth
                        if (string.IsNullOrEmpty(item.CaseWeight))
                        {
                            fullBoxWeight = string.Empty;
                            dfullBoxWeight = 0;
                        }
                        else
                        {
                            fullBoxWeight = item.CaseWeight;
                            dfullBoxWeight = Convert.ToDecimal(item.CaseWeight);
                        }

                        //Last Box weigth
                        if (itemsInLastBox > 0)
                        {
                            if (string.IsNullOrEmpty(item.UnitWeight))
                            {
                                dUnitWeigth = 0;
                            }
                            else
                            {
                                dUnitWeigth = Convert.ToDecimal(item.UnitWeight);
                            }
                            nPartialBoxWeight = itemsInLastBox * Convert.ToInt32(dUnitWeigth);
                            partialBoxWeight = nPartialBoxWeight.ToString();
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
                                shipmentDetails.DimensionD = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionW);
                                shipmentDetails.DimensionD = Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionH == null)
                            {
                                shipmentDetails.DimensionH = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionH);
                                shipmentDetails.DimensionH = Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionL == null)
                            {
                                shipmentDetails.DimensionL = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionL);
                                shipmentDetails.DimensionL = Convert.ToInt32(dHlp); ;
                            }

                            shipmentDetails.BoxNo = string.Format("Box {0}", (i + 1).ToString());
                            shipmentDetails.Sub_ItemID = invDetails.Sub_ItemID;
                            shipmentDetails.Quantity = nUnitperCase;
                            shipmentDetails.Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo);
                            shipmentDetails.Reference2 = string.Format("{0} {1}", invDetails.Sub_ItemID, nUnitperCase.ToString());
                            shipmentDetails.ShipmentId = shipment.ShipmentId;
                            shipmentDetails.UnitPrice = invDetails.UnitPrice;

                            try
                            {
                                shipmentDetails.UnitWeight = Convert.ToInt32(dfullBoxWeight);
                            }
                            catch (Exception err01)
                            {
                                szError01 = err01.Message;
                                //szMsg = item.UnitWeight.Replace(".", ",");
                                //shipmentDetails.UnitWeight = Convert.ToInt32(szMsg);
                                shipmentDetails.UnitWeight = 0;
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
                                shipmentDetails.DimensionD = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionW);
                                shipmentDetails.DimensionD = Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionH == null)
                            {
                                shipmentDetails.DimensionH = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionH);
                                shipmentDetails.DimensionH = Convert.ToInt32(dHlp);
                            }

                            if (item.CaseDimensionL == null)
                            {
                                shipmentDetails.DimensionL = 0;
                            }
                            else
                            {
                                dHlp = Convert.ToDecimal(item.CaseDimensionL);
                                shipmentDetails.DimensionL = Convert.ToInt32(dHlp); ;
                            }

                            shipmentDetails.BoxNo = string.Format("Box {0}", (nrBoxes + 1).ToString());
                            shipmentDetails.Sub_ItemID = invDetails.Sub_ItemID;
                            shipmentDetails.Quantity = itemsInLastBox;
                            shipmentDetails.Reference1 = string.Format("{0} {1}", invoice.SalesOrderNo, szCustomerNo);
                            shipmentDetails.Reference2 = string.Format("{0} {1}", invDetails.Sub_ItemID, itemsInLastBox.ToString());
                            shipmentDetails.ShipmentId = shipment.ShipmentId;
                            shipmentDetails.UnitPrice = invDetails.UnitPrice;

                            try
                            {
                                shipmentDetails.UnitWeight = Convert.ToInt32(nPartialBoxWeight);
                            }
                            catch (Exception err01)
                            {
                                szError01 = err01.Message;
                                //szMsg = item.UnitWeight.Replace(".", ",");
                                //shipmentDetails.UnitWeight = Convert.ToInt32(szMsg);
                                shipmentDetails.UnitWeight = 0;
                            }

                            db01.ShipmentDetails.Add(shipmentDetails);
                        }

                        db01.SaveChanges();

                        db01.Dispose();
                    }
                }

            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            return shipmentDetails;
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

            qryShipment = db.Shipments.Where(shp => shp.InvoiceId == id);
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
        public RedirectToRouteResult Edit(Invoice invoice)
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

            return RedirectToAction("Index", "Shipment", new { id = invoice.InvoiceId });
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

                if (searchItem == "0")
                {
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
