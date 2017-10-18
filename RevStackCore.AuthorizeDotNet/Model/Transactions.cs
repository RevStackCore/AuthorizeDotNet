using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class Transactions : ITransactions
    {
        public string Id { get; set; }
        public string BatchId { get; set; }
    }
}
