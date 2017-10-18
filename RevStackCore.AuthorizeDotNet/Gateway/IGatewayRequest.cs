using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Gateway
{
    public interface IGatewayRequest
    { 
        RequestAction ApiAction { get; set; }
        void AddCustomer(ICustomer customer);
        void AddShipping(IShipping shipping);
        void AddInvoice(IInvoice invoice);
        void AddCurrency(string currency);
        void Charge(ICharge charge);
        void Capture(ICapture capture);
        void Void(IVoid @void);
        void Credit(ICredit credit);
        void Authorize(IAuthorize authorize);
        void GetTransactions(ITransactions transactions);
        void GetTransactionDetails(ITransactionDetails transactionDetails);
        void Subscribe(ISubscribe subscribe);
        void UpdateSubscription(IUpdateSubscription subscription);
        void CancelSubscription(ICancelSubscription subscription);
    }
}
