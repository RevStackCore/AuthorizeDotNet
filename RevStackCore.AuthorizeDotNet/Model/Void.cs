using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class Void : IVoid
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
}
