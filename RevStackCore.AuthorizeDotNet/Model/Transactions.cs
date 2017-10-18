using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class Transactions : ITransactions
    {
        public string Id { get; set; }
        public string BatchId { get; set; }
    }
}
