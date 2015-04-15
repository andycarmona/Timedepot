using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimelyDepotMVC.Properties;

namespace TimelyDepotMVC.UPSWrappers
{
    public static class UPSConstants
    {
        public static string UpsUserName
        {
            get { return Settings.Default.UPSUserName; }
        }

        public static string UpsPasword
        {
            get { return Settings.Default.UPSPassword; }
        }

        public static string UpsAccessLicenseNumber
        {
            get { return Settings.Default.UPSApiKey; }
        }

        public static string UpsShipperNumber
        {
            get { return "A3024V"; }
        }

        public static string UpsShipperName
        {
            get { return "Shipper Name"; }
        }

        public static string UpsCustomerTypeCode
        {
            get { return "53"; }
        }

        public static string UpsCustomerTypeDescription { get { return ""; } }

        public static string UpsShipperAddressLine
        {
            get { return "2508 Merced Ave"; }
        }

        public static string UpsShipperCity
        {
            get { return "South El Monte"; }
        }

        public static string UpsShipperPostalCode
        {
            get { return "91733"; }
        }

        public static string UpsShipperStateProvinceCode
        {
            get { return "CA"; }
        }

        public static string UpsShipperCountryCode
        {
            get
            {
                return "US";

            }
        }

        public static string UpsShipFromAddressLine
        {
            get { return "2508 Merced Ave"; }
        }

        public static string UpsShipFromCity
        {
            get { return "South El Monte"; }
        }

        public static string UpsShipFromPostalCode
        {
            get { return "91733"; }
        }

        public static string UpsShipFromStateProvinceCode
        {
            get { return "CA"; }
        }

        public static string UpsShipFromCountryCode
        {
            get { return "US"; }
        }

        public static string UpsShipFromName
        {
            get { return "Ship from name"; }
        }

        public static string UpsShipmentChargeType
        {
            get { return "01"; }
        }

        public static string UpsPackagingType
        {
            get
            {
                return "02";
            }
        }
    }
}