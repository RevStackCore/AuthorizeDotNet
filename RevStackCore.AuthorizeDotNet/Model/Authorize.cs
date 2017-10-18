using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class Authorize : IAuthorize
    {
        public string Id { get; set; }
        public ICustomer Customer { get; set; }
        public IShipping Shipping { get; set; }
        public ICreditCard CreditCard { get; set; }
        public decimal Amount { get; set; }
    }
}
