using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimelyDepotMVC.UPSShipService;

namespace TimelyDepotMVC.UPSWrappers
{
    using System.Globalization;

    using PayPal.AdaptivePayments;

    using TimelyDepotMVC.ModelsView;
    using TimelyDepotMVC.UPSRateService;

    using DimensionsType = TimelyDepotMVC.UPSShipService.DimensionsType;
    using PackageServiceOptionsType = TimelyDepotMVC.UPSShipService.PackageServiceOptionsType;
    using PackageType = TimelyDepotMVC.UPSShipService.PackageType;
    using PackageWeightType = TimelyDepotMVC.UPSShipService.PackageWeightType;
    using RequestType = TimelyDepotMVC.UPSShipService.RequestType;
    using ShipFromType = TimelyDepotMVC.UPSShipService.ShipFromType;
    using ShipmentType = TimelyDepotMVC.UPSShipService.ShipmentType;
    using ShipperType = TimelyDepotMVC.UPSShipService.ShipperType;
    using ShipToAddressType = TimelyDepotMVC.UPSShipService.ShipToAddressType;
    using ShipToType = TimelyDepotMVC.UPSShipService.ShipToType;
    using UPSSecurity = TimelyDepotMVC.UPSShipService.UPSSecurity;
    using UPSSecurityServiceAccessToken = TimelyDepotMVC.UPSShipService.UPSSecurityServiceAccessToken;
    using UPSSecurityUsernameToken = TimelyDepotMVC.UPSShipService.UPSSecurityUsernameToken;

    public class UPSShipServiceWrapper
    {
        public UPSShipServiceWrapper(
           string userName,
           string password,
           string accessLicenseNumber,
           string shipperNumber,
           string shipperName,
           string shipperAddressLine,
           string shipperCity,
           string shipperPostalCode,
           string shipperStateProvinceCode,
           string shipperCountryCode,
           string shipToPostalCode,
           string shipToCountryCode,
           string shipToName,
           string shipToAddressLine,
           string shipToCity,
           string shipToStateProvinceCode,
           string shipFromAddressLine,
           string shipFromCity,
           string shipFromPostalCode,
           string shipFromStateProvinceCode,
           string shipFromCountryCode,
           string shipFromName,
           string billShipperAccountNumber,
           string packagingTypeCode,
           string shipmentChargeType)
        {
            UserName = userName;
            Pasword = password;
            AccessLicenseNumber = accessLicenseNumber;
            ShipperNumber = shipperNumber;
            ShipperName = shipperName;
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
            ShipmentChargeType = shipmentChargeType;
            //FreightBillingOption = freightBillingOption;
        }

        public UPSShipServiceWrapper(ShipmentRequestView shipmentRequestModel)
        {
            UserName = shipmentRequestModel.userName;
            Pasword = shipmentRequestModel.password;
            AccessLicenseNumber = shipmentRequestModel.accessLicenseNumber;
            ShipperNumber = shipmentRequestModel.shipperNumber;
            ShipperName = shipmentRequestModel.FromName;
            ShipperAddressLine = shipmentRequestModel.FromAddress1;
            ShipperCity = shipmentRequestModel.FromCity;
            ShipperPostalCode = shipmentRequestModel.FromZip;
            ShipperStateProvinceCode = shipmentRequestModel.Fromstate;
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
            ShipmentChargeType = shipmentRequestModel.shipmentChargeType;
        }

        public UPSShipServiceWrapper(string userName, string password, string accessLicenseNumber)
        {
            UserName = userName;
            Pasword = password;
            AccessLicenseNumber = accessLicenseNumber;
        }

        public string UserName { get; set; }

        public string Pasword { get; set; }

        public string AccessLicenseNumber { get; set; }

        public string ShipperNumber { get; set; }

        public string ShipperName { get; set; }

        public string ShipperAddressLine { get; set; }

        public string ShipperCity { get; set; }

        public string ShipperCompany { get; set; }

        public string ShipperPostalCode { get; set; }

        public string ShipperStateProvinceCode { get; set; }

        public string ShipperCountryCode { get; set; }

        public string ShipToPostalCode { get; set; }

        public string ShipToCountryCode { get; set; }

        public string ShipToCompany { get; set; }

        public string ShipToName { get; set; }

        public string ShipToAddressLine { get; set; }

        public string ShipToCity { get; set; }

        public string ShipToStateProvinceCode { get; set; }

        public string ShipFromAddressLine { get; set; }

        public string ShipFromCity { get; set; }

        public string ShipFromPostalCode { get; set; }

        public string ShipFromStateProvinceCode { get; set; }

        public string ShipFromCountryCode { get; set; }

        public string ShipFromName { get; set; }

        public string BillShipperAccountNumber { get; set; }

        public string PackagingTypeCode { get; set; }

        public string ShipmentChargeType { get; set; }

        public string FreightBillingOption { get; set; }

        #region Private
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void AddUpsSecurity(ShipService upsShipService)
        {
            var upss = new UPSSecurity();
            AddUpsServiceAccessToken(upss);
            AddUserNameToken(upss);
            upsShipService.UPSSecurityValue = upss;
        }

        private void AddUpsServiceAccessToken(UPSSecurity upss)
        {
            var upssSvcAccessToken = new UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = AccessLicenseNumber;
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private void AddUserNameToken(UPSSecurity upss)
        {
         
            var upssUsrNameToken = new UPSSecurityUsernameToken();
            upssUsrNameToken.Username = UserName;
            upssUsrNameToken.Password = Pasword;
            upss.UsernameToken = upssUsrNameToken;
        }

        private void AddShipper(ShipmentType shipment)
        {
            
           
            var shipper = new ShipperType();
            shipper.ShipperNumber = ShipperNumber;
            log.Debug("<<<Shipper information>>>");
            log.Debug("shipper Number: " + ShipperNumber);
            shipper.Name = ShipperName;
            log.Debug("shipper Name:" + ShipperName);
            AddShipperAddress(shipper);
            shipment.Shipper = shipper;
        }

        private void AddShipperAddress(ShipperType shipper)
        {
            var shipperAddress = new ShipAddressType();
            shipperAddress.AddressLine = new[] { ShipperAddressLine };
            log.Debug("shipper address: " + shipperAddress.AddressLine[0]);
            shipperAddress.City = ShipperCity;
            log.Debug("shipper city: " + ShipperCity);
            shipperAddress.PostalCode = ShipperPostalCode;
            log.Debug("shipper postal code: " + ShipperPostalCode);
            shipperAddress.StateProvinceCode = ShipperStateProvinceCode;
            log.Debug("shipper state province code: " + ShipperStateProvinceCode);
            shipperAddress.CountryCode = ShipperCountryCode;
            log.Debug("shipper country code: " + ShipperCountryCode);
            shipper.Name = ShipperName;
            log.Debug("shipper Name: " + ShipperName);
            shipper.Address = shipperAddress; 
        }

        private void AddShipToAddress(ShipmentType shipment)
        {
            var shipTo = new ShipToType();
            var shipToAddress = new ShipToAddressType();
          
            log.Debug("<<<ShipTo information>>>");

            shipToAddress.AddressLine = new[] { ShipToAddressLine };
            shipToAddress.City = ShipToCity;
            log.Debug("ShipTo City: " + ShipToCity);
            shipToAddress.PostalCode = ShipToPostalCode;
            log.Debug("ShipTo postal code: " + ShipToPostalCode);
            shipToAddress.StateProvinceCode = ShipToStateProvinceCode;
            log.Debug("ShipTo tate province code: " + ShipToStateProvinceCode);
            shipToAddress.CountryCode = ShipToCountryCode;
            log.Debug("ShipTo Country code: " + ShipToCountryCode);
            shipTo.Address = shipToAddress;
            log.Debug("ShipTo address: " + shipToAddress.AddressLine[0]);
            shipTo.Name = ShipToName;
            log.Debug("ShipTo name: " + ShipToName);
            shipTo.CompanyDisplayableName = ShipToCompany;
            log.Debug("ShipTo Company: " + ShipToCompany);
            shipment.ShipTo = shipTo;

        }

        private void AddShipFromAddress(ShipmentType shipment)
        {
            var shipFrom = new ShipFromType();
            var shipFromAddress = new ShipAddressType();
            shipFromAddress.AddressLine = new[] { ShipperAddressLine };
            shipFromAddress.City = ShipperCity;
            shipFromAddress.PostalCode = ShipperPostalCode;
            shipFromAddress.StateProvinceCode = ShipperStateProvinceCode;
            shipFromAddress.CountryCode = ShipperCountryCode;
            shipFrom.Address = shipFromAddress;
            shipFrom.Name = ShipperName;
            shipFrom.CompanyDisplayableName = ShipperCompany;
            shipment.ShipFrom = shipFrom;
        }

        private void AddBillShipperAccount(ShipmentType shipment)
        {
            var paymentInfo = new PaymentInfoType();
            var shpmentCharge = new ShipmentChargeType();
            var billShipper = new BillShipperType();
            log.Debug("<<<Billshipper payment information>>>");
            billShipper.AccountNumber = BillShipperAccountNumber;
            log.Debug("Bill shipper: " + BillShipperAccountNumber);
            shpmentCharge.BillShipper = billShipper;
          
            shpmentCharge.Type = ShipmentChargeType;
            log.Debug("Shipment charge type: " + ShipmentChargeType);
            ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
            paymentInfo.ShipmentCharge = shpmentChargeArray;
            shipment.PaymentInformation = paymentInfo;
        

        }

        private void AddPaymentInformation(ShipmentType shipment)
        {
            var paymentInfo = new PaymentInfoType();

            var paymentInfoType = new PaymentType();

            paymentInfoType.Code = "06";
            var shipper = new ShipperType();
            shipper.ShipperNumber = ShipperNumber;
            shipper.Name = ShipperName;
            AddShipperAddress(shipper);
            shipment.Shipper = shipper;
        }

        private void AddPackage(string boxNo, int boxWeight, int declaredVal, int boxHeight, int boxWidth, int boxLength, string packagingTypeCode, string currencyCode, PackageType[] pkgArray, int pos)
        {
            var package = new PackageType();
            var packageWeight = new PackageWeightType();
            log.Debug("<<<Package info for " + boxNo + ">>>");
            packageWeight.Weight = boxWeight.ToString(CultureInfo.InvariantCulture);
            log.Debug("Box weight: " + packageWeight.Weight);
            var uom = new ShipUnitOfMeasurementType();
            uom.Code = "LBS";
            log.Debug("Box weight unit: " + uom.Code);
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            log.Debug("Package unit of measurement: " + uom.Description);
            package.PackageWeight = packageWeight;

            var packageDimensions = new DimensionsType();
            packageDimensions.Height = boxHeight.ToString(CultureInfo.InvariantCulture);
            log.Debug("Package height: " + packageDimensions.Height);
            packageDimensions.Length = boxLength.ToString(CultureInfo.InvariantCulture);
            log.Debug("Package length: " + packageDimensions.Length);
            packageDimensions.Width = boxWidth.ToString(CultureInfo.InvariantCulture);
            log.Debug("Package width: " + packageDimensions.Width);
            var packDimType = new ShipUnitOfMeasurementType();
            packDimType.Code = "IN";

            packDimType.Description = "Inches";
            log.Debug("Package dimension type: " + packDimType.Description + " (" + packDimType.Code + ")");
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            var packageServiceOptions = new PackageServiceOptionsType();
            var declaredValue = new PackageDeclaredValueType();

            declaredValue.CurrencyCode = currencyCode;
            log.Debug("Package declared value: " + declaredValue.CurrencyCode);
            declaredValue.MonetaryValue = declaredVal.ToString(CultureInfo.InvariantCulture);
            log.Debug("Package monetary value: " + declaredValue.MonetaryValue);
            packageServiceOptions.DeclaredValue = declaredValue;
            package.PackageServiceOptions = packageServiceOptions;

            var packType = new PackagingType();
            packType.Description = boxNo;
            packType.Code = packagingTypeCode;
            package.Packaging = packType;
            pkgArray[pos] = package;
        }

        #endregion
        public ShipConfirmResponse CallUPSShipmentConfirmationRequest(string serviceCode, int shipmentID, ref string szError)
        {
            //var dbShipment = ShipmentModule.GetShipmentByID(ShipmentDetailID);
            try
            {
                var shipmentDetails = ShipmentModule.GetShipmentShipmentDetails(shipmentID);

                var shpSvc = new ShipService();
                var shipmentRequest = new ShipConfirmRequest();
                AddUpsSecurity(shpSvc);
                var request = new RequestType();
                string[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                var shipment = new ShipmentType();
                shipment.Description = "Ship webservice";
                AddShipper(shipment);
                AddShipFromAddress(shipment);
                AddShipToAddress(shipment);
                AddBillShipperAccount(shipment);
                //AddPaymentInformation(shipment);

                var service = new ServiceType();
                service.Code = serviceCode;
                shipment.Service = service;

                PackageType[] pkgArray;
                pkgArray = new PackageType[shipmentDetails.Count];
                var i = 0;
                foreach (var box in shipmentDetails)
                {
                    AddPackage(
                        box.BoxNo,
                        box.UnitWeight.Value,
                        (int)box.DeclaredValue,
                        box.DimensionH.Value,
                        box.DimensionD.Value,
                        box.DimensionL.Value,
                        PackagingTypeCode,
                        "USD",
                        pkgArray,
                        i);
                    i = i + 1;
                }
                shipment.Package = pkgArray;

                var labelSpec = new LabelSpecificationType();
                var labelStockSize = new LabelStockSizeType();
             
                log.Debug("<<<Package label info>>>");
                labelStockSize.Height = "3";
                log.Debug("Package label heigth: " + labelStockSize.Height);
                labelStockSize.Width = "2";
                log.Debug("Package label width: " + labelStockSize.Width);
                labelSpec.LabelStockSize = labelStockSize;
                var labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "GIF"; //"SPL";
                log.Debug("Package label image format: " + labelImageFormat.Code);
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;

                var shipmentResponse = shpSvc.ProcessShipConfirm(shipmentRequest);
                return shipmentResponse;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                string message = string.Empty;
                message = message + Environment.NewLine + "SoapException Message= " + ex.Message;
                message = message + Environment.NewLine + "SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText;
                message = message + Environment.NewLine + "SoapException XML String for all= " + ex.Detail.LastChild.OuterXml;
                message = message + Environment.NewLine + "SoapException StackTrace= " + ex.StackTrace;
                szError = string.Format("Error processing API Validate Address call (webservice error): {0} UPS API Error: {1}", ex.Message, ex.Detail.LastChild.InnerText);
                return null;
            }
        }

        public ShipmentResponse CallUPSShipmentRequest(string serviceCode, int shipmentID, ref string szError)
        {
            //var dbShipment = ShipmentModule.GetShipmentByID(ShipmentDetailID);
            try
            {
                var shipmentDetails = ShipmentModule.GetShipmentShipmentDetails(shipmentID);
               
                var shpSvc = new ShipService();
                var shipmentRequest = new ShipmentRequest();
                AddUpsSecurity(shpSvc);
                var request = new RequestType();
                string[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                var shipment = new ShipmentType();
                shipment.Description = "Ship webservice";
                AddShipper(shipment);
                AddShipFromAddress(shipment);
                AddShipToAddress(shipment);
                AddBillShipperAccount(shipment);
                //AddPaymentInformation(shipment);

                var service = new ServiceType();
                service.Code = serviceCode;
                shipment.Service = service;

                PackageType[] pkgArray;
                pkgArray = new PackageType[shipmentDetails.Count];
                var i = 0;
                foreach (var box in shipmentDetails)
                {
                    AddPackage(
                        box.BoxNo,
                        box.UnitWeight.Value,
                        (int)box.DeclaredValue,
                        box.DimensionH.Value,
                        box.DimensionD.Value,
                        box.DimensionL.Value,
                        PackagingTypeCode,
                        "USD",
                        pkgArray,
                        i);
                    i = i + 1;
                }
                shipment.Package = pkgArray;

                var labelSpec = new LabelSpecificationType();
                var labelStockSize = new LabelStockSizeType();
                labelStockSize.Height = "3";
                labelStockSize.Width = "2";
                labelSpec.LabelStockSize = labelStockSize;
                var labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "GIF"; //"SPL";
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;
              
                var shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);
                return shipmentResponse;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                string message = string.Empty;
                message = message + Environment.NewLine + "SoapException Message= " + ex.Message;
                message = message + Environment.NewLine + "SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText;
                message = message + Environment.NewLine + "SoapException XML String for all= " + ex.Detail.LastChild.OuterXml;
                message = message + Environment.NewLine + "SoapException StackTrace= " + ex.StackTrace;
                szError = string.Format("Error processing API Validate Address call (webservice error): {0} UPS API Error: {1}", ex.Message, ex.Detail.LastChild.InnerText);
                return null;
            }
        }
    }
}