using System;
using System.Collections.Generic;
using RevStackCore.Payment;
using RevStackCore.Payment.Model;
using System.Text;
using System.IO;
using RevStackCore.AuthorizeDotNet.Model;
using RevStackCore.AuthorizeDotNet.Gateway;
using RevStackCore.AuthorizeDotNet.Model;
using System.Net;

namespace RevStackCore.AuthorizeDotNet.Gateway
{
    public class AuthorizeDotNetRequest : GatewayRequest
    {
        #region private members & constructors

        private const string ChargeSandboxUrl = "https://test.authorize.net/gateway/transact.dll";
        private const string ChargeTestUrl = "https://secure.authorize.net/gateway/transact.dll";
        private const string ChargeLiveUrl = "https://secure.authorize.net/gateway/transact.dll";


        private const string SoapTestUrl = "https://apitest.authorize.net/soap/v1/Service.asmx";
        private const string SoapLiveUrl = "https://api.authorize.net/soap/v1/Service.asmx";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public AuthorizeDotNetRequest(string username, string password)
            : this(username, password, ServiceMode.Live)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isTestMode">if set to <c>true</c> [is test mode].</param>
        public AuthorizeDotNetRequest(string username, string password, ServiceMode serviceMode)
        {
            ServiceMode = serviceMode;

            KeyValues = new Dictionary<string, string>();
            Queue(AuthorizeDotNetApi.ApiLogin, username);
            Queue(AuthorizeDotNetApi.TransactionKey, password);
            if (serviceMode == ServiceMode.Test)
                Queue(AuthorizeDotNetApi.IsTestRequest, "TRUE");
            // default settings
            Queue(AuthorizeDotNetApi.DelimitData, "TRUE");
            Queue(AuthorizeDotNetApi.DelimitCharacter, "|");
            Queue(AuthorizeDotNetApi.RelayResponse, "TRUE");
            Queue(AuthorizeDotNetApi.EmailCustomer, "FALSE");
            Queue(AuthorizeDotNetApi.Method, "CC");
            Queue(AuthorizeDotNetApi.Country, "US");
            Queue(AuthorizeDotNetApi.ShipCountry, "US");
            Queue(AuthorizeDotNetApi.DuplicateWindowTime, "120");
        }
        
        /// <summary>
        /// Sets the API action.
        /// </summary>
        /// <param name="action">The action.</param>
        private void SetApiAction(RequestAction action)
        {
            var apiValue = "AUTH_CAPTURE";

            if (ServiceMode == ServiceMode.Live)
            {
                PostUrl = ChargeLiveUrl;
            }
            else if (ServiceMode == ServiceMode.Test)
            {
                PostUrl = ChargeTestUrl;
            }
            else
            {
                PostUrl = ChargeSandboxUrl;
            }

            ApiAction = action;
            switch (action)
            {
                case RequestAction.Charge:
                    apiValue = "AUTH_CAPTURE";
                    break;
                case RequestAction.Authorize:
                    apiValue = "AUTH_ONLY";
                    break;
                case RequestAction.Capture:
                    apiValue = "PRIOR_AUTH_CAPTURE";
                    break;
                case RequestAction.Credit:
                    apiValue = "CREDIT";
                    break;
                case RequestAction.Void:
                    apiValue = "VOID";
                    break;
                case RequestAction.GetTransactions:
                    apiValue = "GET_TRANS";
                    PostUrl = (ServiceMode == ServiceMode.Test || ServiceMode == ServiceMode.Sandbox) ? SoapTestUrl : SoapLiveUrl;
                    break;
                case RequestAction.GetTransactionDetails:
                    apiValue = "GET_TRANS_DETAILS";
                    PostUrl = (ServiceMode == ServiceMode.Test || ServiceMode == ServiceMode.Sandbox) ? SoapTestUrl : SoapLiveUrl;
                    break;
                case RequestAction.Subscribe:
                    apiValue = "CREATE_SUBSCRIPTION";
                    PostUrl = ServiceMode == ServiceMode.Test ? SoapTestUrl : SoapLiveUrl;
                    break;
                case RequestAction.UpdateSubscription:
                    apiValue = "UPDATE_SUBSCRIPTION";
                    PostUrl = ServiceMode == ServiceMode.Test ? SoapTestUrl : SoapLiveUrl;
                    break;
                case RequestAction.CancelSubscription:
                    apiValue = "CANCEL_SUBSCRIPTION";
                    PostUrl = ServiceMode == ServiceMode.Test ? SoapTestUrl : SoapLiveUrl;
                    break;
                case RequestAction.GetSubscriptionStatus:
                    apiValue = "GET_SUBSCRIPTION_STATUS";
                    PostUrl = ServiceMode == ServiceMode.Test ? SoapTestUrl : SoapLiveUrl;
                    break;
            }

            Queue(AuthorizeDotNetApi.TransactionType, apiValue);
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test mode]; otherwise, <c>false</c>.
        /// </value>
        public ServiceMode ServiceMode { get; set; }

        /// <summary>
        /// Gets or sets the post URL.
        /// </summary>
        /// <value>
        /// The post URL.
        /// </value>
        public string PostUrl { get; set; }

        /// <summary>
        /// Gets or sets the API action.
        /// </summary>
        /// <value>
        /// The API action.
        /// </value>
        public override RequestAction ApiAction { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            //make sure we have all the fields we need
            //starting with the login/key pair
            AssertValidation(AuthorizeDotNetApi.ApiLogin, AuthorizeDotNetApi.TransactionKey);

            //each call has its own requirements... check each
            switch (ApiAction)
            {
                case RequestAction.Charge:
                case RequestAction.Authorize:
                    AssertValidation(AuthorizeDotNetApi.CreditCardNumber, AuthorizeDotNetApi.CreditCardExpiration,
                                     AuthorizeDotNetApi.Amount);
                    break;
                case RequestAction.Capture:
                    AssertValidation(AuthorizeDotNetApi.TransactionId);//, AuthorizeDotNetApi.AuthorizationCode);
                    break;
                case RequestAction.Credit:
                    AssertValidation(AuthorizeDotNetApi.TransactionId, AuthorizeDotNetApi.Amount,
                                     AuthorizeDotNetApi.CreditCardNumber);
                    break;
                case RequestAction.Void:
                    AssertValidation(AuthorizeDotNetApi.TransactionId);
                    break;
            }
        }

        #region Fluent 

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="address">The address.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public override void AddCustomer(ICustomer customer)
        {
            Queue(AuthorizeDotNetApi.FirstName, customer.FirstName);
            Queue(AuthorizeDotNetApi.LastName, customer.LastName);
            Queue(AuthorizeDotNetApi.Address, customer.Address);
            Queue(AuthorizeDotNetApi.City, customer.City);
            Queue(AuthorizeDotNetApi.State, customer.StateOrProvince);
            Queue(AuthorizeDotNetApi.Zip, customer.Zipcode);
            Queue(AuthorizeDotNetApi.CustomerId, customer.Id);
        }

        /// <summary>
        /// Adds the shipping.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="address">The address.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public override void AddShipping(IShipping shipping)
        {
            Queue(AuthorizeDotNetApi.ShipFirstName, shipping.FirstName);
            Queue(AuthorizeDotNetApi.ShipLastName, shipping.LastName);
            Queue(AuthorizeDotNetApi.ShipAddress, shipping.Address);
            Queue(AuthorizeDotNetApi.ShipCity, shipping.City);
            Queue(AuthorizeDotNetApi.ShipState, shipping.StateOrProvince);
            Queue(AuthorizeDotNetApi.ShipZip, shipping.Zipcode);
        }

        /// <summary>
        /// Adds the merchant value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        //public override void AddMerchantValue(string key, string value)
        //{
        //    Queue(key, value);
        //}

        /// <summary>
        /// Adds the invoice.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <returns></returns>
        public override void AddInvoice(IInvoice invoice)
        {
            Queue(AuthorizeDotNetApi.InvoiceNumber, invoice.InvoiceNumber);
        }

        /// <summary>
        /// Adds the currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        public override void AddCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
                currency = "USD";
            //Queue(AuthorizeDotNetApi.Currency, currency);
        }
        #endregion

        #region Requests

        /// <summary>
        /// Authorizes a charge for the specified amount on the given credit card.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="cvv">The CVV.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override void Authorize(IAuthorize authorize)
        {
            SetApiAction(RequestAction.Authorize);
            Queue(AuthorizeDotNetApi.CreditCardNumber, authorize.CreditCard.CardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, authorize.CreditCard.ExpirationMonth + authorize.CreditCard.ExpirationYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, authorize.CreditCard.CVV);
            Queue(AuthorizeDotNetApi.Amount, authorize.Amount.ToString());
        }

        /// <summary>
        /// Sales the specified card number.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="cvv">The CVV.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override void Charge(ICharge charge)
        {
            SetApiAction(RequestAction.Charge);
            Queue(AuthorizeDotNetApi.CreditCardNumber, charge.CreditCard.CardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, charge.CreditCard.ExpirationMonth + charge.CreditCard.ExpirationYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, charge.CreditCard.CVV);
            Queue(AuthorizeDotNetApi.Amount, charge.Amount.ToString());
        }

        /// <summary>
        /// Settles the transaction that matches the specified transaction id and amount.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override void Capture(ICapture capture)
        {
            SetApiAction(RequestAction.Capture);
            Queue(AuthorizeDotNetApi.TransactionId, capture.Id);
            if (capture.Amount > 0) Queue(AuthorizeDotNetApi.Amount, capture.Amount.ToString());
        }

        /// <summary>
        /// Voids the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public override void Void(IVoid @void)
        {
            SetApiAction(RequestAction.Void);
            Queue(AuthorizeDotNetApi.TransactionId, @void.Id);
        }

        /// <summary>
        /// Refunds the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <returns></returns>
        public override void Credit(ICredit credit)
        {
            var c = (Credit)credit;
            SetApiAction(RequestAction.Credit);
            Queue(AuthorizeDotNetApi.TransactionId, c.Id);
            Queue(AuthorizeDotNetApi.CreditCardNumber, c.CardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, c.ExpirationDate);
            Queue(AuthorizeDotNetApi.Amount, c.Amount.ToString());
        }

        public override void GetTransactions(ITransactions transactions)
        {
            SetApiAction(RequestAction.GetTransactions);
            Queue(AuthorizeDotNetApi.BatchId, transactions.BatchId);
        }

        public override void GetTransactionDetails(ITransactionDetails transactionDetails)
        {
            SetApiAction(RequestAction.GetTransactionDetails);
            Queue(AuthorizeDotNetApi.TransactionId, transactionDetails.Id);
        }

        /// <summary>
        /// Starts recurring billing charges for the subscriber.
        /// </summary>
        /// <param name="request">The subscription request.</param>
        /// <returns></returns>
        public override void Subscribe(ISubscribe subscribe)
        {
            SetApiAction(RequestAction.Subscribe);
            Queue(AuthorizeDotNetApi.Amount, subscribe.Amount.ToString());
            Queue(AuthorizeDotNetApi.BillingCycles, subscribe.BillingCycles.ToString());
            Queue(AuthorizeDotNetApi.BillingInterval, Enum.GetName(typeof(BillingInterval), subscribe.BillingInterval));
            Queue(AuthorizeDotNetApi.CreditCardNumber, subscribe.CreditCard.CardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, subscribe.CreditCard.ExpirationMonth + subscribe.CreditCard.ExpirationYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, subscribe.CreditCard.CVV);
            //Queue(AuthorizeDotNetApi.Email, request.CustomerEmail);
            Queue(AuthorizeDotNetApi.TotalOccurences, subscribe.TotalOccurrences.ToString());
            Queue(AuthorizeDotNetApi.Description, subscribe.Description);
            Queue(AuthorizeDotNetApi.StartsOn, subscribe.StartsOn.ToString());
            Queue(AuthorizeDotNetApi.SubscriptionName, subscribe.Name);
            Queue(AuthorizeDotNetApi.TrialAmount, subscribe.TrialAmount.ToString());
            Queue(AuthorizeDotNetApi.TrialBillingCycles, subscribe.TrialOccurrences.ToString());
        }

        /// <summary>
        /// Updates recurring billing charges for the subscriber.
        /// </summary>
        /// <param name="request">The subscription request.</param>
        /// <returns></returns>
        public override void UpdateSubscription(IUpdateSubscription subscription)
        {
            SetApiAction(RequestAction.UpdateSubscription);
            Queue(AuthorizeDotNetApi.Amount, subscription.Amount.ToString());
            //Queue(AuthorizeDotNetApi.BillingCycles, billingCycles.ToString());
            //Queue(AuthorizeDotNetApi.BillingInterval, billingInterval.ToString());
            Queue(AuthorizeDotNetApi.CreditCardNumber, subscription.CreditCard.CardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, subscription.CreditCard.ExpirationMonth + subscription.CreditCard.ExpirationYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, subscription.CreditCard.CVV);
            //Queue(AuthorizeDotNetApi.Email, request.CustomerEmail);
            //Queue(AuthorizeDotNetApi.TotalOccurences, totalOccurrences.ToString());
            Queue(AuthorizeDotNetApi.Description, subscription.Description);
            //Queue(AuthorizeDotNetApi.StartsOn, startsOn.ToString());
            Queue(AuthorizeDotNetApi.SubscriptionID, subscription.Id);
            Queue(AuthorizeDotNetApi.SubscriptionName, subscription.Name);
            Queue(AuthorizeDotNetApi.TrialAmount, subscription.TrialAmount.ToString());
            Queue(AuthorizeDotNetApi.TrialBillingCycles, subscription.TrialOccurrences.ToString());
        }

        /// <summary>
        /// Stops the recurring billing charges that matches the specified subscription id.
        /// </summary>
        /// <param name="id">The subscription id.</param>
        /// <returns></returns>
        public override void CancelSubscription(ICancelSubscription subscription) 
        {
            SetApiAction(RequestAction.CancelSubscription);
            Queue(AuthorizeDotNetApi.SubscriptionID, subscription.Id);
        }

        #endregion


        /// <summary>
        /// Asserts the validation.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void AssertValidation(params string[] keys)
        {
            var sb = new StringBuilder();
            foreach (var item in keys)
            {
                if (!KeyValues.ContainsKey(item))
                {
                    sb.AppendFormat("{0}, ", item);
                }
                else
                {
                    if (string.IsNullOrEmpty(KeyValues[item]))
                        sb.AppendFormat("No value for '{0}', which is required. ", item);
                }
                var result = sb.ToString();
                if (result.Length > 0)
                    throw new InvalidDataException("Can't submit to Gateway - missing these input fields: " +
                                                   result.Trim().TrimEnd(','));
            }
        }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public Dictionary<string, string> KeyValues { get; set; }

        /// <summary>
        /// Queues the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Queue(string key, string value)
        {
            if (KeyValues.ContainsKey(key))
                KeyValues.Remove(key);

            KeyValues.Add(key, value);
            return true;
        }

        /// <summary>
        /// Converts the Queue to a string.
        /// </summary>
        /// <returns></returns>
        public string ToKeyValueString()
        {
            var sb = new StringBuilder();
            foreach (var key in KeyValues.Keys)
                sb.AppendFormat("{0}={1}&", key, WebUtility.UrlEncode(KeyValues[key]));

            var result = sb.ToString();
            return result.TrimEnd('&');
        }
    }
}