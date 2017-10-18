using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Model
{
    public class CancelSubscription : ICancelSubscription
    {
        public string Id { get; set; }
    }
}
