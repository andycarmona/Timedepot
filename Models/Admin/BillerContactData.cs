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
    using System.ComponentModel.DataAnnotations;

    public class BillerContactData
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        public string Country { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

       

    }
}