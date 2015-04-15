using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimelyDepotMVC.UPSTimeInTransit;

namespace TimelyDepotMVC.UPSWrappers
{
    public class UPSTimeInTransitWrapper
    {
        public string UserName { get; set; }
        public string Pasword { get; set; }
        public string AccessLicenseNumber { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeDescription { get; set; }

        public string ShipToPostalCode { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToName { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToStateProvinceCode { get; set; }

        public string ShipFromCity { get; set; }
        public string ShipFromPostalCode { get; set; }
        public string ShipFromStateProvinceCode { get; set; }
        public string ShipFromCountryCode { get; set; }
        public string ShipFromName { get; set; }

        public UPSTimeInTransitWrapper(string userName, string password, string accessLicenseNumber,
            string customerTypeCode, string customerTypeDescription,
            string shipToPostalCode, string shipToCountryCode, string shipToName, string shipToCity,
            string shipToStateProvinceCode, string shipFromCity, string shipFromPostalCode,
            string shipFromStateProvinceCode, string shipFromCountryCode,
            string shipFromName)
        {
            UserName = userName;
            Pasword = password;
            AccessLicenseNumber = accessLicenseNumber;
            CustomerTypeCode = customerTypeCode;
            CustomerTypeDescription = customerTypeDescription;
            ShipToPostalCode = shipToPostalCode;
            ShipToCountryCode = shipToCountryCode;
            ShipToName = shipToName;
            ShipToCity = shipToCity;
            ShipToStateProvinceCode = shipToStateProvinceCode;
            ShipFromCity = shipFromCity;
            ShipFromPostalCode = shipFromPostalCode;
            ShipFromStateProvinceCode = shipFromStateProvinceCode;
            ShipFromCountryCode = shipFromCountryCode;
            ShipFromName = shipFromName;
        }

        #region Private

        private void AddUpsSecurity(TimeInTransitService upsService)
        {
            var upss = new UPSTimeInTransit.UPSSecurity();
            AddUpsServiceAccessToken(upss);
            AddUserNameToken(upss);
            upsService.UPSSecurityValue = upss;
        }

        private void AddUpsServiceAccessToken(UPSTimeInTransit.UPSSecurity upss)
        {
            var upssSvcAccessToken = new UPSTimeInTransit.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = AccessLicenseNumber;
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private void AddUserNameToken(UPSTimeInTransit.UPSSecurity upss)
        {
            var upssUsrNameToken = new UPSTimeInTransit.UPSSecurityUsernameToken();
            upssUsrNameToken.Username = UserName;
            upssUsrNameToken.Password = Pasword;
            upss.UsernameToken = upssUsrNameToken;
        }

        private void AddShipToAddress(UPSTimeInTransit.TimeInTransitRequest request, bool isResidentialAddress)
        {
            var shipTo = new UPSTimeInTransit.RequestShipToType();
            var shipToAddress = new UPSTimeInTransit.RequestShipToAddressType();
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
            request.ShipTo = shipTo;
        }

        private void AddShipFromAddress(UPSTimeInTransit.TimeInTransitRequest request)
        {
            var shipFrom = new UPSTimeInTransit.RequestShipFromType();
            var shipFromAddress = new UPSTimeInTransit.RequestShipFromAddressType();
            if (!string.IsNullOrEmpty(ShipFromCity))
                shipFromAddress.City = ShipFromCity;
            if (!string.IsNullOrEmpty(ShipFromPostalCode))
                shipFromAddress.PostalCode = ShipFromPostalCode;
            if (!string.IsNullOrEmpty(ShipFromStateProvinceCode))
                shipFromAddress.StateProvinceCode = ShipFromStateProvinceCode;
            if (!string.IsNullOrEmpty(ShipFromCountryCode))
                shipFromAddress.CountryCode = ShipFromCountryCode;
            shipFrom.Address = shipFromAddress;
            request.ShipFrom = shipFrom;
        }

        private void AddInvoiceTotalType(int qty, decimal unitPrice, string currencyCode, UPSTimeInTransit.TimeInTransitRequest request)
        {
            var invoiceType = new UPSTimeInTransit.InvoiceLineTotalType();
            invoiceType.CurrencyCode = currencyCode;
            int total = (int)(qty * unitPrice);
            if (total % 100 > 0)
                total = total + (100 - total % 100);
            invoiceType.MonetaryValue = total.ToString();
            request.InvoiceLineTotal = invoiceType;
        }

        #endregion

        public UPSTimeInTransit.TimeInTransitResponse CallUPSTimeInTransitRequest(int qty, int nrBoxes, string fullBoxWeight, string partialBoxWeight, string currencyCode, decimal unitPrice, bool isResidentialAddress)
        {
            var upsService = new UPSTimeInTransit.TimeInTransitService();
            var response = new UPSTimeInTransit.TimeInTransitResponse();
            var request = new UPSTimeInTransit.TimeInTransitRequest();
            AddUpsSecurity(upsService);
            var requestType = new UPSTimeInTransit.RequestType();
            String[] requestOption = { "TNT" };
            requestType.RequestOption = requestOption;
            request.Request = requestType;
            AddShipFromAddress(request);
            AddShipToAddress(request, isResidentialAddress);
            PickupType pickup = new PickupType();
            pickup.Date = DateTime.Now.ToString("yyyyMMdd");
            request.Pickup = pickup;
            var shipmentWeight = new UPSTimeInTransit.ShipmentWeightType();
            shipmentWeight.Weight = (nrBoxes * decimal.Parse(fullBoxWeight) + (string.IsNullOrEmpty(partialBoxWeight) ? 0 : decimal.Parse(partialBoxWeight))).ToString();//"10";
            var unitOfMeasurement = new UPSTimeInTransit.CodeDescriptionType();
            unitOfMeasurement.Code = "LBS";
            unitOfMeasurement.Description = "pounds";
            shipmentWeight.UnitOfMeasurement = unitOfMeasurement;
            request.ShipmentWeight = shipmentWeight;

            request.TotalPackagesInShipment = nrBoxes.ToString();
            request.MaximumListSize = "1";
            AddInvoiceTotalType(qty, unitPrice, currencyCode, request);

            var tntResponse = upsService.ProcessTimeInTransit(request);
            return tntResponse;
        }
    }
}