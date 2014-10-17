namespace TimelyDepotMVC.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class EnvironmentParameters
    {
        [Key]
        public int ParameterId { get; set; }

        public string KeyValue { get; set; }

        public string KeyParameter { get; set; }

        public bool Active { get; set; }

        public string ServerUrl { get; set; }

        public string TransactionUri { get; set; }

        public string Description { get; set; }

    }
}