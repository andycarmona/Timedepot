using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace TimelyDepotMVC.UPSWrappers
{
    public class UPSUtility
    {
        #region Utilities
        private string CreateRequest(string AccessKey, string Username, string Password, string FromZip, string ToZip, string weight)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version='1.0'?>");
            sb.Append("<AccessRequest xml:lang='en-US'>");
            sb.AppendFormat("<AccessLicenseNumber>{0}</AccessLicenseNumber>", AccessKey);
            sb.AppendFormat("<UserId>{0}</UserId>", Username);
            sb.AppendFormat("<Password>{0}</Password>", Password);
            sb.Append("</AccessRequest>");
            sb.Append("<?xml version='1.0'?>");
            //sb.Append("<RatingServiceSelectionRequest xml:lang='en-US'>");
            sb.Append("<RatingServiceSelectionRequest>");
            sb.Append("<Request>");
            sb.Append("<TransactionReference>");
            sb.Append("<CustomerContext>Rate Request</CustomerContext>");
            sb.Append("<XpciVersion>1.0</XpciVersion>");
            sb.Append("</TransactionReference>");
            sb.Append("<RequestAction>Rate</RequestAction>");
            sb.Append("<RequestOption>Rate</RequestOption>");
            sb.Append("</Request>");
            if (String.Equals("US", "US", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                sb.Append("<PickupType>");
                sb.AppendFormat("<Code>{0}</Code>", "01");
                sb.Append("</PickupType>");
                sb.Append("<CustomerClassification>");
                sb.AppendFormat("<Code>{0}</Code>", "04"); //01 = wholesale, 03 = occasional, 04 = retail
                sb.Append("</CustomerClassification>");
            }
            sb.Append("<Shipment>");
            sb.Append("<Shipper>");
            sb.AppendFormat("<ShipperNumber>{0}</ShipperNumber>", "9X437V");
            sb.Append("<Address>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", FromZip);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", "US");
            sb.Append("</Address>");
            sb.Append("</Shipper>");
            sb.Append("<ShipTo>");
            sb.Append("<Address>");
            sb.Append("<ResidentialAddressIndicator/>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", ToZip);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", "US");
            sb.Append("</Address>");
            sb.Append("</ShipTo>");
            sb.Append("<ShipFrom>");
            sb.Append("<Address>");
            sb.AppendFormat("<PostalCode>{0}</PostalCode>", FromZip);
            sb.AppendFormat("<CountryCode>{0}</CountryCode>", "US");
            sb.Append("</Address>");
            sb.Append("</ShipFrom>");
            sb.Append("<Service>");
            sb.Append("<Code>03</Code>");
            sb.Append("</Service>");
            sb.Append("<Package>");
            sb.Append("<PackagingType>");
            sb.AppendFormat("<Code>{0}</Code>", "02");
            sb.Append("</PackagingType>");
            sb.Append("<PackageWeight>");
            sb.AppendFormat("<UnitOfMeasurement>");
            sb.AppendFormat("<code>{0}</code>", "LBS");
            sb.AppendFormat("</UnitOfMeasurement>");
            sb.AppendFormat("<Weight>{0}</Weight>", weight);
            sb.Append("</PackageWeight>");
            sb.Append("</Package>");
            sb.Append("<NegotiatedRatesIndicator />");
            sb.Append("</Shipment>");
            sb.Append("</RatingServiceSelectionRequest>");
            string requestString = sb.ToString();
            return requestString;
        }

        private string DoRequest(string URL, string RequestString)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(RequestString);
            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = request.GetResponse();
            string responseXML = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
                responseXML = reader.ReadToEnd();

            return responseXML;
        }

        private List<ShippingOption> ParseResponse(string response)
        {
            string error = "";
            var shippingOptions = new List<ShippingOption>();

            using (var sr = new StringReader(response))
            using (var tr = new XmlTextReader(sr))
                while (tr.Read())
                {
                    if ((tr.Name == "Error") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string errorText = "";
                        while (tr.Read())
                        {
                            if ((tr.Name == "ErrorCode") && (tr.NodeType == XmlNodeType.Element))
                            {
                                errorText += "UPS Rating Error, Error Code: " + tr.ReadString() + ", ";
                            }
                            if ((tr.Name == "ErrorDescription") && (tr.NodeType == XmlNodeType.Element))
                            {
                                errorText += "Error Desc: " + tr.ReadString();
                            }
                        }
                        error = "UPS Error returned: " + errorText;
                    }
                    if ((tr.Name == "RatedShipment") && (tr.NodeType == XmlNodeType.Element))
                    {
                        string serviceCode = "";
                        string monetaryValue = "";
                        while (tr.Read())
                        {
                            if ((tr.Name == "Service") && (tr.NodeType == XmlNodeType.Element))
                            {
                                while (tr.Read())
                                {
                                    if ((tr.Name == "Code") && (tr.NodeType == XmlNodeType.Element))
                                    {
                                        serviceCode = tr.ReadString();
                                        tr.ReadEndElement();
                                    }
                                    if ((tr.Name == "Service") && (tr.NodeType == XmlNodeType.EndElement))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (((tr.Name == "RatedShipment") && (tr.NodeType == XmlNodeType.EndElement)) || ((tr.Name == "RatedPackage") && (tr.NodeType == XmlNodeType.Element)))
                            {
                                break;
                            }
                            if ((tr.Name == "TotalCharges") && (tr.NodeType == XmlNodeType.Element))
                            {
                                while (tr.Read())
                                {
                                    if ((tr.Name == "MonetaryValue") && (tr.NodeType == XmlNodeType.Element))
                                    {
                                        monetaryValue = tr.ReadString();
                                        tr.ReadEndElement();
                                    }
                                    if ((tr.Name == "TotalCharges") && (tr.NodeType == XmlNodeType.EndElement))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        string service = "03";
                        string serviceId = String.Format("[{0}]", serviceCode);

                        //Weed out unwanted or unkown service rates
                        if (service.ToUpper() != "UNKNOWN")
                        {
                            var shippingOption = new ShippingOption();
                            shippingOption.Rate = Convert.ToDecimal(monetaryValue, new CultureInfo("en-US"));
                            shippingOption.Name = serviceCode;
                            shippingOptions.Add(shippingOption);
                        }

                    }
                }

            return shippingOptions;
        }

        #endregion

        #region Methods
        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="shipmentPackage">Shipment package</param>
        /// <param name="error">Error</param>
        /// <returns>Shipping options</returns>
        public string GetShippingRate(string AccessKey, string Username, string Password, string FromZip, string ToZip, string weight)
        {
            string rate = "";

            string requestString = CreateRequest(AccessKey, Username, Password, FromZip, ToZip, weight);
            string responseXML = DoRequest("https://www.ups.com/ups.app/xml/Rate", requestString);
            List<ShippingOption> shippingOptions = ParseResponse(responseXML);
            foreach (var shippingOption in shippingOptions)
            {
                rate = shippingOption.Rate.ToString();
                shippingOption.Name = string.Format("UPS {0}", shippingOption.Name);
            }

            return rate;
        }

        #endregion

        #region "classes"

        [Serializable]
        public class ShippingOption
        {
            private decimal rate;
            private string name;
            private string description;

            #region Properties
            /// <summary>
            /// Gets or sets a shipping rate
            /// </summary>
            public decimal Rate
            {
                get
                {
                    return rate;
                }
                set
                {
                    rate = value;
                }
            }
            /// <summary>
            /// Gets or sets a shipping option name
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }
            /// <summary>
            /// Gets or sets a shipping option description
            /// </summary>
            public string Description
            {
                get
                {
                    return description;
                }
                set
                {
                    description = value;
                }
            }
            #endregion
        }

        /// <summary>
        /// Represents a shipping option collection
        /// </summary>
        public class ShippingOptionCollection : List<ShippingOption>
        {

        }

        #endregion
    }
}