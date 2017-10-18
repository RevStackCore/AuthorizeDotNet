using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class Charge : ICharge
    {
        public string Id { get; set; }
        public ICustomer Customer { get; set; }
        public IShipping Shipping { get; set; }
        public ICreditCard CreditCard { get; set; }
        public decimal Amount { get; set; }
    }
}
