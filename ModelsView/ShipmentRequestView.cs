// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentRequestView.cs" company="Timedepot">
//   
// </copyright>
// <summary>
//   Data to be handle by shipment request
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimelyDepotMVC.ModelsView
{
    public class ShipmentRequestView
    {
        public string userName { get; set; }

        public string password { get; set; }

        public string accessLicenseNumber { get; set; }

        public string shipperNumber { get; set; }

        public string shipperName { get; set; }

        public string shipperAddressLine { get; set; }

        public string shipperCity { get; set; }

        public string shipperPostalCode { get; set; }

        public string shipperStateProvinceCode { get; set; }

        public string shipperCountryCode { get; set; }

        public string shipToPostalCode { get; set; }

        public string shipToCountryCode { get; set; }

        public string shipToName { get; set; }

        public string shipToAddressLine { get; set; }

        public string shipToCity { get; set; }

        public string shipToStateProvinceCode { get; set; }

        public string shipFromAddressLine { get; set; }

        public string shipFromCity { get; set; }

        public string shipFromPostalCode { get; set; }

        public string shipFromStateProvinceCode { get; set; }

        public string shipFromCountryCode { get; set; }

        public string shipFromName { get; set; }

        public string billShipperAccountNumber { get; set; }

        public string packagingTypeCode { get; set; }

        public string shipmentChargeType { get; set; }
    }
}