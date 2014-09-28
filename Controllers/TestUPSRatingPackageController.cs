using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPS_Shipping_Rate.UPSRateService;

using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;

namespace TimelyDepotMVC.Controllers
{
    public class Result
    {
        public string code { get; set; }
        public string service { get; set; }
        public string cost { get; set; }
        public string time { get; set; }
        public string billingweight { get; set; }

    }


    public partial class inv_detl
    {
        public global::System.String PROD_CD
        {
            get
            {
                return _PROD_CD;
            }
            set
            {
                if (_PROD_CD != value)
                {
                    _PROD_CD = value;
                }
            }
        }
        private global::System.String _PROD_CD;
        public Nullable<global::System.Decimal> CASE_LEN
        {
            get
            {
                return _CASE_LEN;
            }
            set
            {
                _CASE_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_LEN;
        public Nullable<global::System.Decimal> CASE_HI
        {
            get
            {
                return _CASE_HI;
            }
            set
            {
                _CASE_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_HI;

        public Nullable<global::System.Decimal> CASE_WI
        {
            get
            {
                return _CASE_WI;
            }
            set
            {
                _CASE_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_WI;

        public Nullable<global::System.Decimal> CASE_WT
        {
            get
            {
                return _CASE_WT;
            }
            set
            {
                _CASE_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_WT;
        public Nullable<global::System.Decimal> BOX_LEN
        {
            get
            {
                return _BOX_LEN;
            }
            set
            {
                _BOX_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_LEN;
        public Nullable<global::System.Decimal> BOX_HI
        {
            get
            {
                return _BOX_HI;
            }
            set
            {
                _BOX_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_HI;
        public Nullable<global::System.Decimal> BOX_WI
        {
            get
            {
                return _BOX_WI;
            }
            set
            {
                _BOX_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_WI;
        public Nullable<global::System.Decimal> BOX_WT
        {
            get
            {
                return _BOX_WT;
            }
            set
            {
                _BOX_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_WT;
        public Nullable<global::System.Decimal> UT_LEN
        {
            get
            {
                return _UT_LEN;
            }
            set
            {
                _UT_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_LEN;
        public Nullable<global::System.Decimal> UT_HI
        {
            get
            {
                return _UT_HI;
            }
            set
            {
                _UT_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_HI;
        public Nullable<global::System.Decimal> UT_WI
        {
            get
            {
                return _UT_WI;
            }
            set
            {
                _UT_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_WI;
        public Nullable<global::System.Decimal> UT_WT
        {
            get
            {
                return _UT_WT;
            }
            set
            {
                _UT_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_WT;
        public Nullable<global::System.Byte> PD_KG
        {
            get
            {
                return _PD_KG;
            }
            set
            {
                _PD_KG = value;
            }
        }
        private Nullable<global::System.Byte> _PD_KG;
        public global::System.String ORIGEN
        {
            get
            {
                return _ORIGEN;
            }
            set
            {
                _ORIGEN = value;
            }
        }
        private global::System.String _ORIGEN;
    }

    public class TestUPSRatingPackageController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        private void AddCustomerClassification(RateRequest rateRequest)
        {
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType customerType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            customerType.Code = "53";
            customerType.Description = "";
            rateRequest.CustomerClassification = customerType;
        }

        private static void AddUpsSecurity(RateService upsService)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurity upss = new UPS_Shipping_Rate.UPSRateService.UPSSecurity();
            AddUpsServiceAccessToken(upss);
            AddUserNameToken(upss);
            upsService.UPSSecurityValue = upss;
        }

        private static void AddUpsServiceAccessToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken upssSvcAccessToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = "FCBD8E914895FF36";
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private static void AddUserNameToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken upssUsrNameToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken();
            //upssUsrNameToken.Username = "young55961";
            //upssUsrNameToken.Password = "Merced88";
            upssUsrNameToken.Username = "young55961";
            upssUsrNameToken.Password = "Merced88";
            upss.UsernameToken = upssUsrNameToken;
        }


        private void AddShipper(ShipmentType shipment)
        {
            ShipperType shipper = new ShipperType();
            AddShipperAddress(shipper);

            shipment.Shipper = shipper;
        }

        private void AddShipperAddress(ShipperType shipper)
        {
            AddressType shipperAddress = new AddressType();
            shipperAddress.City = "South El Monte";
            shipperAddress.PostalCode = "91733";
            shipperAddress.CountryCode = "US";
            shipper.ShipperNumber = "A3024V";
            shipper.Address = shipperAddress;
        }

        private static void AddInvoiceTotalType(int Qty, decimal unitPrice, ShipmentType shipment)
        {
            InvoiceLineTotalType invoiceType = new InvoiceLineTotalType();
            invoiceType.CurrencyCode = "USD";
            int total = (int)(Qty * unitPrice);
            if (total % 100 > 0)
                total = total + (100 - total % 100);
            invoiceType.MonetaryValue = total.ToString();
            shipment.InvoiceLineTotal = invoiceType;
        }


        private void AddPackageArray(int nrBoxes, int itemsInLastBox, string fullBoxWeight, string partialBoxWeight, int valuePerFullBox, int valuePerPartialBox, inv_detl details, ShipmentType shipment)
        {
            PackageType[] pkgArray;
            if (itemsInLastBox > 0)
                pkgArray = new PackageType[nrBoxes + 1];
            else
                pkgArray = new PackageType[nrBoxes];

            for (int i = 0; i < nrBoxes; i++)
            {
                AddFullPackage(fullBoxWeight, valuePerFullBox, details, pkgArray, i);
            }
            if (itemsInLastBox > 0 && !string.IsNullOrEmpty(partialBoxWeight))
                AddPartialPackage(nrBoxes, partialBoxWeight, valuePerPartialBox, details, pkgArray);

            shipment.Package = pkgArray;
        }
        private void AddFullPackage(string fullBoxWeight, int valuePerFullBox, inv_detl details, PackageType[] pkgArray, int pos)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = fullBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerFullBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[pos] = package;
        }

        private void AddPartialPackage(int nrBoxes, string partialBoxWeight, int valuePerPartialBox, inv_detl details, PackageType[] pkgArray)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = partialBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerPartialBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[nrBoxes] = package;
        }


        private void AddShipToAddress(ShipmentType shipment)
        {
            ShipToType shipTo = new ShipToType();
            ShipToAddressType shipToAddress = new ShipToAddressType();
            //shipToAddress.PostalCode = txtZipcode.Text;
            shipToAddress.PostalCode = "19850";
            shipToAddress.CountryCode = "US";
            shipTo.Address = shipToAddress;
            shipment.ShipTo = shipTo;
        }

        private void AddShipFromAddress(ShipmentType shipment)
        {
            ShipFromType shipFrom = new ShipFromType();
            AddressType shipFromAddress = new AddressType();
            //  shipFromAddress.AddressLine = new String[] { txtPShipFromAddress.Text };
            shipFromAddress.City = "South El Monte";
            shipFromAddress.PostalCode = "91733";
            shipFromAddress.StateProvinceCode = "CA";
            shipFromAddress.CountryCode = "US";
            shipFrom.Address = shipFromAddress;
            shipment.ShipFrom = shipFrom;
        }
        private RateResponse CallUPSRateRequest(string scode, int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, RateService upsService)
        {
            RateResponse rateResponse = new RateResponse();
            try
            {
                RateRequest rateRequest = new RateRequest();
                AddCustomerClassification(rateRequest);
                AddUpsSecurity(upsService);
                UPS_Shipping_Rate.UPSRateService.RequestType request = new RequestType();
                String[] requestOption = { "Rate" };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                AddShipper(shipment);
                AddShipFromAddress(shipment);
                AddShipToAddress(shipment);
                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = scode.ToString();
                shipment.Service = service;
                ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                optype.NegotiatedRatesIndicator = string.Empty;
                shipment.ShipmentRatingOptions = optype;
                AddPackageArray(nrBoxes, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, shipment);
                AddInvoiceTotalType(Qty, unitPrice, shipment);
                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                rateResponse = upsService.ProcessRate(rateRequest);
            }
            catch (Exception ex)
            {

            }
            return rateResponse;
        }
        private void GetRateFromUPS(int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice)
        {
            try
            {
                List<Result> lst = new List<Result>();
                lst.Add(new Result() { service = "UPS Next Day Air®", code = "01" });
                lst.Add(new Result() { service = "UPS Second Day Air®", code = "02" });
                lst.Add(new Result() { service = "UPS Ground", code = "03" });
                lst.Add(new Result() { service = "UPS Express", code = "07" });
                lst.Add(new Result() { service = "UPS ExpeditedSM", code = "08" });
                lst.Add(new Result() { service = "UPS Standard", code = "11" });
                lst.Add(new Result() { service = "UPS Three-Day Select®", code = "12" });
                lst.Add(new Result() { service = "UPS Next Day Air Saver®", code = "13" });
                lst.Add(new Result() { service = "UPS Next Day Air® Early A.M. SM", code = "14" });

                foreach (Result r in lst)
                {
                    RateService upsService = new RateService();
                    try
                    {
                        RateResponse rateResponse = CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, upsService);
                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                r.cost = rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue + " " + rshipment.NegotiatedRateCharges.TotalCharge.CurrencyCode;
                                if (rshipment.GuaranteedDelivery != null && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                            }
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {

                    }
                }
                //lstView.DataSource = lst;
                //lstView.DataBind();

                //Display the response data
                ViewBag.ResultList = lst;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {

            }
        }


        //
        // POST: //GetCost
        [HttpPost]
        public ActionResult GetCost(string qty, string zipcode)
        {
            int BOX_CASE = 5;
            int CASE_WT = 5;
            int UT_WT = 1;
            int Qty = 100;
            int diff = 0;

            int nrBoxes = 0;
            int itemsInLastBox = 0;
            int valuePerFullBox = 0;
            int valuePerPartialBox = 0;


            decimal unitPrice = 14;

            string szError = "";
            string szMsg = "";
            string szItemId = "DF4011";


            string fullBoxWeight = "";
            string partialBoxWeight = "";

            //UPSinv_detl details = null;
            inv_detl details = null;

            ITEM item = null;
            PRICE price = null;
            try
            {
                if (string.IsNullOrEmpty(qty))
                {
                    Qty = 50;
                }
                else
                {
                    Qty = Convert.ToInt32(qty);
                }

                //details = new UPSinv_detl();
                details = new inv_detl();
                details.CASE_HI = 5;
                details.CASE_LEN = 5;
                details.CASE_WI = 5;

                price = db.PRICEs.Where(prc => prc.Item == szItemId && prc.Qty == Qty).FirstOrDefault<PRICE>();
                if (price != null)
                {
                    unitPrice = price.thePrice;
                }

                item = db.ITEMs.Where(itm => itm.ItemID == szItemId).FirstOrDefault<ITEM>();
                if (item != null)
                {
                    BOX_CASE = Convert.ToInt16(item.UnitPerCase);
                    CASE_WT = Convert.ToInt32(item.CaseWeight);
                    UT_WT = Convert.ToInt32(item.UnitWeight);

                    details.CASE_HI = Convert.ToDecimal(item.CaseDimensionH);
                    details.CASE_LEN = Convert.ToDecimal(item.CaseDimensionL);
                    details.CASE_WT = Convert.ToDecimal(item.CaseDimensionW);   //Sample uses details.CASE_WT
                }

                nrBoxes = Qty / BOX_CASE;
                itemsInLastBox = Qty % BOX_CASE;

                fullBoxWeight = CASE_WT.ToString();
                if (itemsInLastBox > 0)
                {
                    partialBoxWeight = (itemsInLastBox * UT_WT).ToString();
                }

                valuePerFullBox = BOX_CASE * Convert.ToInt32(unitPrice);
                diff = valuePerFullBox % 100;
                if (diff > 0)
                {
                    valuePerFullBox = valuePerFullBox + (100 - diff);
                }

                valuePerPartialBox = itemsInLastBox * Convert.ToInt32(unitPrice);
                diff = valuePerPartialBox % 100;
                if (diff > 0)
                {
                    valuePerPartialBox = valuePerPartialBox + (100 - diff);
                }

                //GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice);
                GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice);

            }
            catch (Exception err)
            {
                szMsg = string.Format("{0} {1}", err.Message, err.StackTrace);
            }
            return View();
        }

        //
        // GET: /TestUPSRatingPackage/

        public ActionResult Index()
        {
            return View();
        }

    }
}
