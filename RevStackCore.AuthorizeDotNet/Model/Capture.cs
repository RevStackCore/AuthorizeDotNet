using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class Capture : ICapture
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
}
