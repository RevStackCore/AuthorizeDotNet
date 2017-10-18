using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class Transaction : ITransactionDetails
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
}
