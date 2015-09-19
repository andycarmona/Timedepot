using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;

namespace TimelyDepotMVC.UPSWrappers
{
    public static class ShipmentModule
    {        
        private static TimelyDepotContext ent = new TimelyDepotContext();
        public static void SaveChanges()
        {
            //ClearDummyShippingDetails();
            ent.SaveChanges();
        }

        private static void ClearDummyShippingDetails()
        {
            var dummy = ent.ShipmentDetails.Where(i => i.Sub_ItemID == "-");
            foreach (var item in dummy)
            {
                ent.ShipmentDetails.Remove(item);
            }
        }

        public static void Dispose()
        {
            ent.Dispose();
        }

        public static IEnumerable<int> GetInvoiceNumbers()
        {
            return ent.Invoices.Select(i => i.InvoiceId).DefaultIfEmpty().Distinct().OrderBy(i => i).ToList();
        }

        public static Invoice GetInvoice(int invs_num)
        {
            return ent.Invoices.FirstOrDefault(i => i.InvoiceId == invs_num);
        }

        public static Customers GetInvoiceCustomer(int invs_num)
        {
            var invoice = ent.Invoices.FirstOrDefault(i => i.InvoiceId== invs_num);
            return invoice == null ? null : ent.Customers.FirstOrDefault(i => i.CustomerNo == invoice.CustomerId.ToString());
        }

        //public static ins_data GetInsDataForInvoice(int invs_num)
        //{
        //    return ent.ins_data.FirstOrDefault(i => i.INVS_NUM == invs_num);
        //}

        //public static List<InvoiceDetail> GetInvtLogDataForInvoice(int invs_num)
        //{
        //    //ignore items with no weight and dimension information
        //    return
        //       (from i in ent.invt_log.Where(i => i.INVS_NUM == invs_num)
        //        join d in ent.inv_detl on i.PROD_CD equals d.PROD_CD
        //        where d.CASE_HI.HasValue && d.CASE_LEN.HasValue && d.CASE_WI.HasValue && d.CASE_WT.HasValue
        //        select new InvoiceDetail { ProductCode = i.PROD_CD, ProductQuantity = i.PROD_QTY, UnitPrice = i.UNIT_PRS }
        //        ).OrderBy(i => i.ProductCode).ToList();
        //}

        //public static inv GetInv(string productCode)
        //{
        //    return (from item in ent.invs
        //            where item.PROD_CD == productCode
        //            select item).FirstOrDefault();
        //}

        //public static inv_detl GetInvDetl(string productCode)
        //{
        //    return (from item in ent.inv_detl
        //            where item.PROD_CD == productCode
        //            select item).FirstOrDefault();
        //}

        //public static FromAddresses GetFromAddress(int id)
        //{
        //    return ent.FromAddresses.FirstOrDefault(i => i.FromAddressID == id);
        //}

        //public static List<FromAddresses> GetAllFromAddresses()
        //{
        //    return ent.FromAddresses.OrderBy(i => i.FromAddressID).ToList();
        //}

        //public static List<ListItem> GetAllFromAddressesForDdl()
        //{
        //    return ent.FromAddresses.OrderBy(i => i.FromAddressID).ToList().Select(i => new ListItem { Value = i.FromAddressID.ToString(), Text = i.CompanyName }).ToList();
        //}

        //public static void AddNewFromAddress(string companyName, string contact,
        //    string address1, string address2, string city, string countryCode,
        //    string postalCode, string stateCode, string telephone)
        //{
        //    var address = new FromAddresses();
        //    address.CompanyName = companyName;
        //    address.Contact = contact;
        //    address.Address1 = address1;
        //    address.Address2 = address2;
        //    address.City = city;
        //    address.CountryCode = countryCode;
        //    address.PostalCode = postalCode;
        //    address.StateCode = stateCode;
        //    address.Telephone = telephone;
        //    ent.FromAddresses.AddObject(address);
        //}

        //public static void AddNewFromAddress(FromAddresses newAddress)
        //{
        //    ent.FromAddresses.AddObject(newAddress);
        //}

        //public static FromAddresses AddNewFromAddress()
        //{
        //    var address = new FromAddresses();
        //    ent.FromAddresses.AddObject(address);
        //    return address;
        //}

        //public static void DeleteFromAddress(FromAddresses address)
        //{
        //    ent.FromAddresses.DeleteObject(address);
        //}

        public static void AddNewShipmentDetail(ShipmentDetails detail)
        {
            ent.ShipmentDetails.Add(detail);
        }

        //public static ShipmentDetails AddNewShipmentDetail(int invoiceNo)
        //{
        //    var detail = new ShipmentDetails();
        //    detail.ItemNo = "-";
        //    ent.ShipmentDetails.AddObject(detail);
        //    return detail;
        //}

        //public static List<ShipmentDetails> GetShipmentShipmentDetails(int shipmentID)
        //{
        //    var shipment = ent.Shipments.FirstOrDefault(i => i.ShipmentID == shipmentID && !i.IsCancelled);
        //    return
        //        shipment.ShipmentDetails
        //           .OrderBy(i => i.ItemNo)
        //           .ThenBy(i => i.BoxNumber)
        //           .ToList();
        //}

        public static void DeleteShipmentDetail(ShipmentDetails detail)
        {
            ent.ShipmentDetails.Remove(detail);
        }

        //public static bool HasShipment(int invoiceNum)
        //{
        //    return ent.Shipments.Any(i => i.InvoiceNum == invoiceNum && !i.IsCancelled && !i.IsShipped);
        //}

        //public static Shipments GetShipmentByID(int shipmentID)
        //{
        //    return ent.Shipments.FirstOrDefault(i => i.ShipmentID == shipmentID && !i.IsCancelled);
        //}

        //public static Shipments GetShipmentByInvoice(int invoiceNo)
        //{
        //    return ent.Shipments.FirstOrDefault(i => i.InvoiceNum == invoiceNo && !i.IsCancelled && !i.IsShipped);
        //}

        //public static Shipments AddNewShipment(int invoiceNo)
        //{
        //    var shipment = new Shipments();
        //    shipment.InvoiceNum = invoiceNo;
        //    shipment.LastModifiedDate = DateTime.Now;
        //    shipment.IsCancelled = false;
        //    ent.Shipments.AddObject(shipment);
        //    return shipment;
        //}

        //public static void AddNewShipment(Shipments shipment)
        //{
        //    ent.Shipments.AddObject(shipment);
        //}

        //public static ShipmentAddresses GetShipmentAddressFrom(int invoiceNum)
        //{
        //    var shipment = GetInvoiceNotProcessedShipment(invoiceNum);
        //    return shipment == null ? null : shipment.ShipmentAddresses.FirstOrDefault(i => i.ShipmentID == shipment.ShipmentID && i.ShipFrom);
        //}

        //public static ShipmentAddresses GetShipmentAddressTo(int invoiceNum)
        //{
        //    var shipment = GetInvoiceNotProcessedShipment(invoiceNum);
        //    return shipment == null ? null : shipment.ShipmentAddresses.FirstOrDefault(i => i.ShipmentID == shipment.ShipmentID && !i.ShipFrom);
        //}

        //public static Shipments GetInvoiceNotProcessedShipment(int invoiceNum)
        //{
        //    return
        //        ent.Shipments.FirstOrDefault(
        //            i => i.InvoiceNum == invoiceNum && !i.IsCancelled && !i.IsShipped);
        //}

        //public static ShipmentAddresses AddNewShipmentAddress(int shipmentID, bool shipFrom)
        //{
        //    var address = new ShipmentAddresses();
        //    address.ShipmentID = shipmentID;
        //    address.ShipFrom = shipFrom;
        //    ent.ShipmentAddresses.AddObject(address);
        //    return address;
        //}

        //public static List<Shipments> GetShipmentsByInvoice(int? invoiceNo)
        //{
        //    if (invoiceNo.HasValue)
        //        return ent.Shipments.Where(i => i.InvoiceNum == invoiceNo).OrderBy(i => i.ShipmentID).ToList();
        //    return ent.Shipments.OrderBy(i => i.InvoiceNum).ThenBy(i => i.ShipmentID).ToList();
        //}

        public static List<ShipmentDetails> GetShipmentShipmentDetails(int shipmentId)
        {
            var shipment = ent.ShipmentDetails.Where(i => i.ShipmentId == shipmentId && i.Shipped==false).ToList();
            return
                shipment.OrderBy(i => i.Sub_ItemID)
                   .ThenBy(i => i.BoxNo)
                   .ToList();
        }
    }

    public class InvoiceDetailWrapper
    {
        public string ProductCode { get; set; }
        public decimal? ProductQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class ProductBoxingInfo
    {
        public int BoxNumber { get; set; }
        public decimal Quantity { get; set; }
        public string ProductCode { get; set; }
        public decimal BoxWeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public decimal DeclaredValue { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
    }

    public class ListItem
    {
        public string Value;
        public string Text;

        public override string ToString()
        {
            return Text;
        }
    }
}