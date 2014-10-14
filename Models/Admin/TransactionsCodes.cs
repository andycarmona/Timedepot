namespace TimelyDepotMVC.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class TransactionsCodes
    {
        [Key]
        public int TransactionCodeId { get; set; }

        public int TransactionCode { get; set; }

        public string CodeDescription { get; set; }
    }
    
}