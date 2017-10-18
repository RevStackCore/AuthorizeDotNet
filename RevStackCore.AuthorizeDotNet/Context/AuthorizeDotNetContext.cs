using RevStackCore.Payment;
using RevStackCore.Payment.Context;
using RevStackCore.AuthorizeDotNet.Gateway;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet.Context
{
    public class AuthorizeDotNetContext : PaymentGatewayContext
    {
        public string Currency { get; set; }

        private readonly string _apiLoginId;
        private readonly string _transactionKey;

        private AuthorizeDotNetContext() { }

        public AuthorizeDotNetContext(string apiLoginId, string transactionKey, ServiceMode serviceMode)
            : this(apiLoginId, transactionKey, "USD", serviceMode) { }

        public AuthorizeDotNetContext(string apiLoginId, string transactionKey, string currency, ServiceMode serviceMode)
        {
            _apiLoginId = apiLoginId;
            _transactionKey = transactionKey;
            ServiceMode = serviceMode;
            Currency = currency;
        }

        public IGatewayRequest Create()
        {
            return new AuthorizeDotNetRequest(_apiLoginId, _transactionKey, ServiceMode);
        }

        public IGatewayResponse Send(IGatewayRequest request)
        {
            var apiRequest = new AuthorizeDotNetApiRequest();
            return apiRequest.Send((AuthorizeDotNetRequest)request);
        }

        public IEnumerable<IGatewayResponse> GetTransactions(IGatewayRequest request)
        {
            var apiRequest = new AuthorizeDotNetApiRequest();
            return apiRequest.GetTransactions((AuthorizeDotNetRequest)request);
        }
    }
}
