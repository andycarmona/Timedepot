namespace TimelyDepotMVC.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class EnvironmentParameters
    {
        [Key]
        public int ParameterId { get; set; }

        [Required]
        public string GatewayId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, StringLength(20)]
        public string KeyValue { get; set; }

        [Required, StringLength(50)]
        public string KeyParameter { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required, StringLength(50)]
        public string ServerUrl { get; set; }

        [Required, StringLength(20)]
        public string TransactionUri { get; set; }

        [Required, StringLength(20)]
        public string Description { get; set; }

    }
}