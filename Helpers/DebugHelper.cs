using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.Helpers
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using TimelyDepotMVC.UPSShipService;
    using TimelyDepotMVC.XAVService;

    public class DebugHelper
    {
        public static string TransformShipmentRequestToXml(Type RequestType, ShipConfirmRequest request)
        {
            ShipConfirmRequest shipmentRequest;
            var xsSubmit = new XmlSerializer(RequestType);
            var xmlResult = string.Empty;

            using (var sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, request);
              xmlResult = sww.ToString(); // Your XML
      
            }

            return xmlResult;
        }

        public static string TransformXAVRequestToXml(Type RequestType, XAVRequest request)
        {
           
            var xsSubmit = new XmlSerializer(RequestType);
            var xmlResult = string.Empty;

            using (var sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, request);
                xmlResult = sww.ToString(); // Your XML

            }

            return xmlResult;
        }
    }
}