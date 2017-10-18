using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class Transaction : ITransactionDetails
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
}
