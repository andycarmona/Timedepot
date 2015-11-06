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

        public string UpsCustomerTypeCode { get; set; }

        public string UpsCustomerTypeDescription { get; set; }


        public string accessLicenseNumber { get; set; }

        public string shipperNumber { get; set; }

        public string FromCompany { get; set; }

        public string FromName { get; set; }

        public string FromAddress1 { get; set; }

        public string FromCity { get; set; }

        public string FromZip { get; set; }

        public string Fromstate { get; set; }

        public string FromCountry { get; set; }

        public string FromTel { get; set; }

        public string ToCompany { get; set; }

        public string ToName { get; set; }

        public string ToAddress1 { get; set; }

        public string ToCity { get; set; }

        public string ToZip { get; set; }

        public string ToState { get; set; }

        public string ToCountry { get; set; }

        public string ToTel { get; set; }

        public string billShipperAccountNumber { get; set; }

        public string billShipperType { get; set; }


        public string BillerContact { get; set; }

        public string BillerAddress { get; set; }

        public string BillerPhone { get; set; }

        public string BillerCity { get; set; }

        public string BillerState { get; set; }

        public string BillerZipCode { get; set; }

        public string BillerCountry { get; set; }

        public string BillerAccountNumber { get; set; }

        public string packagingTypeCode { get; set; }

        public string shipmentChargeType { get; set; }

        public string shipmentServiceType { get; set; }
    }
}