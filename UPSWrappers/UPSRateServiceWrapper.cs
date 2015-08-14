using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimelyDepotMVC.UPSRateService;

namespace TimelyDepotMVC.UPSWrappers
{
    using TimelyDepotMVC.Models.Admin;
    using TimelyDepotMVC.ModelsView;

    public class UPSRateServiceWrapper
    {

        public string UserName { get; set; }
        public string Pasword { get; set; }
        public string AccessLicenseNumber { get; set; }
        public string ShipperNumber { get; set; }
        public string ShipperName { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeDescription { get; set; }
        public string ShipperAddressLine { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPostalCode { get; set; }
        public string ShipperStateProvinceCode { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipToCompany { get; set; }
        public string ShipToPostalCode { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddressLine { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToStateProvinceCode { get; set; }
        public string ShipperCompany { get; set; }

        public string ShipFromAddressLine { get; set; }
        public string ShipFromCity { get; set; }
        public string ShipFromPostalCode { get; set; }
        public string ShipFromStateProvinceCode { get; set; }
        public string ShipFromCountryCode { get; set; }
        public string ShipFromName { get; set; }
        public string BillShipperAccountNumber { get; set; }
        public string PackagingTypeCode { get; set; }

        public UPSRateServiceWrapper(string userName, string password, string accessLicenseNumber,
            string shipperNumber, string shipperName, string customerTypeCode, string customerTypeDescription,
            string shipperAddressLine, string shipperCity, string shipperPostalCode, string shipperStateProvinceCode,
            string shipperCountryCode, string shipToPostalCode, string shipToCountryCode, string shipToName,
            string shipToAddressLine, string shipToCity, string shipToStateProvinceCode, string shipFromAddressLine,
            string shipFromCity, string shipFromPostalCode, string shipFromStateProvinceCode, string shipFromCountryCode,
            string shipFromName, string billShipperAccountNumber, string packagingTypeCode)
        {
            UserName = userName;
            Pasword = password;
            AccessLicenseNumber = accessLicenseNumber;
            ShipperNumber = shipperNumber;
            ShipperName = shipperName;
            CustomerTypeCode = customerTypeCode;
            CustomerTypeDescription = customerTypeDescription;
            ShipperAddressLine = shipperAddressLine;
            ShipperCity = shipperCity;
            ShipperPostalCode = shipperPostalCode;
            ShipperStateProvinceCode = shipperStateProvinceCode;
            ShipperCountryCode = shipperCountryCode;
            ShipToPostalCode = shipToPostalCode;
            ShipToCountryCode = shipToCountryCode;
            ShipToName = shipToName;
            ShipToAddressLine = shipToAddressLine;
            ShipToCity = shipToCity;
            ShipToStateProvinceCode = shipToStateProvinceCode;
            ShipFromAddressLine = shipFromAddressLine;
            ShipFromCity = shipFromCity;
            ShipFromPostalCode = shipFromPostalCode;
            ShipFromStateProvinceCode = shipFromStateProvinceCode;
            ShipFromCountryCode = shipFromCountryCode;
            ShipFromName = shipFromName;
            BillShipperAccountNumber = billShipperAccountNumber;
            PackagingTypeCode = packagingTypeCode;
        }

        public UPSRateServiceWrapper(ShipmentRequestView shipmentRequestModel)
        {
            UserName = shipmentRequestModel.userName;
            Pasword = shipmentRequestModel.password;
            AccessLicenseNumber = shipmentRequestModel.accessLicenseNumber;
            ShipperNumber = shipmentRequestModel.shipperNumber;
            ShipperName = shipmentRequestModel.FromName;

            ShipFromAddressLine = shipmentRequestModel.FromAddress1;
            ShipperAddressLine = shipmentRequestModel.FromAddress1;

            ShipFromCity = shipmentRequestModel.FromCity;
            ShipperCity = shipmentRequestModel.FromCity;

            ShipperPostalCode = shipmentRequestModel.FromZip;
            ShipFromPostalCode = shipmentRequestModel.FromZip;

            ShipFromStateProvinceCode = shipmentRequestModel.Fromstate;
            ShipperStateProvinceCode = shipmentRequestModel.Fromstate;

            ShipFromCountryCode = shipmentRequestModel.FromCountry;
            ShipperCountryCode = shipmentRequestModel.FromCountry;

            ShipperCompany = shipmentRequestModel.FromCompany;
            ShipToCompany = shipmentRequestModel.ToCompany;

            ShipToPostalCode = shipmentRequestModel.ToZip;
            ShipToCountryCode = shipmentRequestModel.ToCountry;
            ShipToName = shipmentRequestModel.ToName;
            ShipToAddressLine = shipmentRequestModel.ToAddress1;
            ShipToCity = shipmentRequestModel.ToCity;
            ShipToStateProvinceCode = shipmentRequestModel.ToState;
            BillShipperAccountNumber = shipmentRequestModel.billShipperAccountNumber;
            PackagingTypeCode = shipmentRequestModel.packagingTypeCode;
            
        }
        #region Private

        private void AddCustomerClassification(RateRequest rateRequest)
        {
            var customerType = new CodeDescriptionType();
            customerType.Code = CustomerTypeCode;
            customerType.Description = CustomerTypeDescription;
            rateRequest.CustomerClassification = customerType;
        }

        private void AddUpsSecurity(RateService upsService)
        {
            var upss = new UPSRateService.UPSSecurity();
            AddUpsServiceAccessToken(upss);
            AddUserNameToken(upss);
            upsService.UPSSecurityValue = upss;
        }

        private void AddUpsServiceAccessToken(UPSRateService.UPSSecurity upss)
        {
            var upssSvcAccessToken = new UPSRateService.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = AccessLicenseNumber;
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private void AddUserNameToken(UPSRateService.UPSSecurity upss)
        {
            var upssUsrNameToken = new UPSRateService.UPSSecurityUsernameToken();
            upssUsrNameToken.Username = UserName;
            upssUsrNameToken.Password = Pasword;
            upss.UsernameToken = upssUsrNameToken;
        }

        private void AddShipper(UPSRateService.ShipmentType shipment)
        {
            var shipper = new UPSRateService.ShipperType();
            shipper.ShipperNumber = ShipperNumber;
            AddShipperAddress(shipper);
            shipment.Shipper = shipper;
        }

        private void AddShipperAddress(UPSRateService.ShipperType shipper)
        {
            var shipperAddress = new UPSRateService.AddressType();
            shipperAddress.AddressLine = new String[] { ShipperAddressLine };
            shipperAddress.City = ShipperCity;
            shipperAddress.PostalCode = ShipperPostalCode;
            shipperAddress.StateProvinceCode = ShipperStateProvinceCode;
            shipperAddress.CountryCode = ShipperCountryCode;
            shipper.Address = shipperAddress;
        }

        private void AddInvoiceTotalType(int Qty, decimal unitPrice, UPSRateService.ShipmentType shipment)
        {
            var invoiceType = new UPSRateService.InvoiceLineTotalType();
            invoiceType.CurrencyCode = "USD";
            int total = (int)(Qty * unitPrice);
            if (total % 100 > 0)
                total = total + (100 - total % 100);
            invoiceType.MonetaryValue = total.ToString();
            shipment.InvoiceLineTotal = invoiceType;
        }

        private void AddPackageArray(int nrBoxes, int itemsInLastBox, string fullBoxWeight, string partialBoxWeight, int valuePerFullBox, int valuePerPartialBox, inv_detl details, string packagingTypeCode, string currencyCode, UPSRateService.ShipmentType shipment)
        {
            UPSRateService.PackageType[] pkgArray;
            //if (itemsInLastBox > 0)
            //    pkgArray = new UPSRateService.PackageType[nrBoxes];
            //else
            pkgArray = new UPSRateService.PackageType[nrBoxes];

            int tempItemCount = nrBoxes;
            if (itemsInLastBox > 0)
                tempItemCount = tempItemCount - 1;
            for (int i = 0; i < tempItemCount; i++)
            {
                AddFullPackage(fullBoxWeight, valuePerFullBox, details, packagingTypeCode, currencyCode, pkgArray, i);
            }
            if (itemsInLastBox > 0 && !string.IsNullOrEmpty(partialBoxWeight))
                AddPartialPackage(nrBoxes, partialBoxWeight, valuePerPartialBox, details, packagingTypeCode, currencyCode, pkgArray);

            shipment.Package = pkgArray;
        }

        private void AddShipToAddress(UPSRateService.ShipmentType shipment, bool isResidentialAddress)
        {
            var shipTo = new UPSRateService.ShipToType();
            var shipToAddress = new UPSRateService.ShipToAddressType();
            if (!string.IsNullOrEmpty(ShipToAddressLine))
                shipToAddress.AddressLine = new String[] { ShipToAddressLine };
            if (!string.IsNullOrEmpty(ShipToCity))
                shipToAddress.City = ShipToCity;
            if (!string.IsNullOrEmpty(ShipToPostalCode))
                shipToAddress.PostalCode = ShipToPostalCode;
            if (!string.IsNullOrEmpty(ShipToStateProvinceCode))
                shipToAddress.StateProvinceCode = ShipToStateProvinceCode;
            if (!string.IsNullOrEmpty(ShipToCountryCode))
                shipToAddress.CountryCode = ShipToCountryCode;
            if (isResidentialAddress)
                shipToAddress.ResidentialAddressIndicator = "true";
            shipTo.Address = shipToAddress;
            shipment.ShipTo = shipTo;
        }

        private void AddShipFromAddress(UPSRateService.ShipmentType shipment)
        {
            var shipFrom = new UPSRateService.ShipFromType();
            var shipFromAddress = new UPSRateService.AddressType();
            if (!string.IsNullOrEmpty(ShipFromAddressLine))
                shipFromAddress.AddressLine = new String[] { ShipFromAddressLine };
            if (!string.IsNullOrEmpty(ShipFromCity))
                shipFromAddress.City = ShipFromCity;
            if (!string.IsNullOrEmpty(ShipFromPostalCode))
                shipFromAddress.PostalCode = ShipFromPostalCode;
            if (!string.IsNullOrEmpty(ShipFromStateProvinceCode))
                shipFromAddress.StateProvinceCode = ShipFromStateProvinceCode;
            if (!string.IsNullOrEmpty(ShipFromCountryCode))
                shipFromAddress.CountryCode = ShipFromCountryCode;
            shipFrom.Address = shipFromAddress;
            shipment.ShipFrom = shipFrom;
        }

        private void AddFullPackage(string fullBoxWeight, int valuePerFullBox, inv_detl details, string packagingTypeCode, string currencyCode, UPSRateService.PackageType[] pkgArray, int pos)
        {
            var package = new UPSRateService.PackageType();
            var packageWeight = new UPSRateService.PackageWeightType();
            packageWeight.Weight = fullBoxWeight;
            var uom = new UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            var packageDimensions = new UPSRateService.DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();
            var packDimType = new UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            var packageServiceOptions = new UPSRateService.PackageServiceOptionsType();
            var insuredValue = new UPSRateService.InsuredValueType();
            insuredValue.CurrencyCode = currencyCode;
            insuredValue.MonetaryValue = valuePerFullBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            var packType = new UPSRateService.CodeDescriptionType();
            packType.Code = packagingTypeCode;
            package.PackagingType = packType;
            pkgArray[pos] = package;
        }

        private void AddPartialPackage(int nrBoxes, string partialBoxWeight, int valuePerPartialBox, inv_detl details, string packagingTypeCode, string currencyCode, UPSRateService.PackageType[] pkgArray)
        {
            var package = new UPSRateService.PackageType();
            var packageWeight = new UPSRateService.PackageWeightType();
            packageWeight.Weight = partialBoxWeight;
            var uom = new UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            var packageDimensions = new UPSRateService.DimensionsType();
            //packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            //packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            //packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();

            packageDimensions.Height = "0";
            packageDimensions.Length = "0";
            packageDimensions.Width = "0";

            var packDimType = new UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            var packageServiceOptions = new UPSRateService.PackageServiceOptionsType();
            var insuredValue = new UPSRateService.InsuredValueType();
            insuredValue.CurrencyCode = currencyCode;
            insuredValue.MonetaryValue = valuePerPartialBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            var packType = new UPSRateService.CodeDescriptionType();
            packType.Code = packagingTypeCode;
            package.PackagingType = packType;
            pkgArray[nrBoxes - 1] = package;
        }

        #endregion

        public RateResponse CallUPSRateRequest(string serviceCode, int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, string packagingTypeCode, string currencyCode, decimal unitPrice, bool isResidentialAddress)//, out string requestXML)
        {
            var upsService = new RateService();
            var rateResponse = new RateResponse();
            var rateRequest = new RateRequest();
            AddCustomerClassification(rateRequest);
            AddUpsSecurity(upsService);
            var request = new RequestType();
            String[] requestOption = { "Rate" };
            request.RequestOption = requestOption; //this can be Rate or Shop
            rateRequest.Request = request;
            var shipment = new UPSRateService.ShipmentType();
            AddShipper(shipment);
            AddShipFromAddress(shipment);
            AddShipToAddress(shipment, isResidentialAddress);
            var service = new CodeDescriptionType();
            service.Code = serviceCode;
            shipment.Service = service;
            var optype = new UPSRateService.ShipmentRatingOptionsType();
            optype.NegotiatedRatesIndicator = string.Empty;
            shipment.ShipmentRatingOptions = optype;
            AddPackageArray(nrBoxes, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, packagingTypeCode, currencyCode, shipment);
            AddInvoiceTotalType(Qty, unitPrice, shipment);
            rateRequest.Shipment = shipment;
            //var serializer = new XmlSerializer(typeof(RateRequest));
            //using (var writer = new StringWriter())
            //{
            //  serializer.Serialize(writer, rateRequest);
            //  requestXML = writer.ToString();
            //}
            rateResponse = upsService.ProcessRate(rateRequest);
            return rateResponse;
        }
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
}