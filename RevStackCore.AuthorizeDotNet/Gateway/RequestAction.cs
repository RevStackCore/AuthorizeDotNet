
namespace RevStackCore.AuthorizeDotNet.Gateway
{
    public enum RequestAction
    {
        Authorize,
        Capture,
        Charge,
        Credit,
        Void,
        GetTransactions,
        GetTransactionDetails,
        Subscribe,
        UpdateSubscription,
        CancelSubscription,
        GetSubscriptionStatus
    }
}