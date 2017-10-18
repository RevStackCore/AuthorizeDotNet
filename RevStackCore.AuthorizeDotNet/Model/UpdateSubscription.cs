using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class UpdateSubscription : IUpdateSubscription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICustomer Customer { get; set; }
        public IShipping Shipping { get; set; }
        public ICreditCard CreditCard { get; set; }
        public decimal Amount { get; set; }
        public short TrialOccurrences { get; set; }
        public decimal TrialAmount { get; set; }
    }
}
