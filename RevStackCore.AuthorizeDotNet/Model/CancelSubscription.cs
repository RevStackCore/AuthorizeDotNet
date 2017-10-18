using RevStack.Payment.Model;

namespace RevStack.AuthorizeDotNet.Model
{
    public class CancelSubscription : ICancelSubscription
    {
        public string Id { get; set; }
    }
}
