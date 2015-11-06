// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BillerContactData.cs" company="timessence">
//   
// </copyright>
// <summary>
//   Handles billers contact information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimelyDepotMVC.Models.Admin
{
    public class BillerContactData
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public string AccountNumber { get; set; }

    }
}