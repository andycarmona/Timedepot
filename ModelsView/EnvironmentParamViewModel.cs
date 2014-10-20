

namespace TimelyDepotMVC.ModelsView
{
    using System.Collections;

    public class EnvironmentParamViewModel : IEnumerable
    {
        public int ParameterId { get; set; }
        
        public string GatewayId { get; set; }

        public string Password { get; set; }

        public string KeyValue { get; set; }

        public string KeyParameter { get; set; }

        public bool Active { get; set; }

        public string ServerUrl { get; set; }

        public string TransactionUri { get; set; }

        public string Description { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}