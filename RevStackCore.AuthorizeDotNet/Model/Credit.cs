using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class Credit : ICredit
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
    }
}
