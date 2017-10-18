using System;
using RevStackCore.Payment.Model;

namespace RevStackCore.AuthorizeDotNet.Gateway
{
    public abstract class GatewayRequest : IGatewayRequest
    {
        /// <summary>
        /// Gets or sets the API action.
        /// </summary>
        /// <value>
        /// The API action.
        /// </value>
        public abstract RequestAction ApiAction { get; set; }

        #region Fluent Methods

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
        public abstract void AddCustomer(ICustomer customer);

        /// <summary>
        /// Adds the shipping.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="address">The address.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public abstract void AddShipping(IShipping shipping);

        /// <summary>
        /// Adds the merchant value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        //public abstract void AddMerchantValue(string key, string value);

        /// <summary>
        /// Adds the invoice.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <returns></returns>
        public abstract void AddInvoice(IInvoice invoice);

        /// <summary>
        /// Adds the currency.
        /// </summary>
        /// <param name="invoiceNumber">The currency.</param>
        /// <returns></returns>
        public abstract void AddCurrency(string currency);

        #endregion

        #region Requests

        public abstract void Charge(ICharge charge);

        /// <summary>
        /// Settles the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="capture">The transaction id.</param>
        /// <returns></returns>
        public void Capture(string transactionId)
        {
            var capture = default(ICapture);
            capture.Id = transactionId;
            capture.Amount = 0m;
            Capture(capture);
        }

        /// <summary>
        /// Settles the transaction that matches the specified transaction id and amount.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public abstract void Capture(ICapture capture);

        /// <summary>
        /// Voids the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public abstract void Void(IVoid @void);

        /// <summary>
        /// Refunds the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <returns></returns>
        public abstract void Credit(ICredit credit);

        /// <summary>
        /// Authorizes a charge for the specified amount on the given credit card.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="cvv">The CVV.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public abstract void Authorize(IAuthorize authorize);

        /// <summary>
        /// Gets all transactions by type.
        /// </summary>
        /// <param name="request">Transaction Type</param>
        /// <returns></returns>
        public abstract void GetTransactions(ITransactions transactions);

        /// <summary>
        /// Gets transaction by id.
        /// </summary>
        /// <param name="request">Transaction Id</param>
        /// <returns></returns>
        public abstract void GetTransactionDetails(ITransactionDetails transactionDetails);

        /// <summary>
        /// Starts recurring billing charges for the given customer.
        /// </summary>
        /// <param name="request">The subscription request.</param>
        /// <returns></returns>
        public abstract void Subscribe(ISubscribe subscribe);

        /// <summary>
        /// Updates recurring billing charges for the given customer.
        /// </summary>
        /// <param name="request">The subscription request.</param>
        /// <returns></returns>
        public abstract void UpdateSubscription(IUpdateSubscription subscription);

        /// <summary>
        /// Stops the recurring billing charges that matches the specified subscription id.
        /// </summary>
        /// <param name="id">The subscription id.</param>
        /// <returns></returns>
        public abstract void CancelSubscription(ICancelSubscription subsciption);

        /// <summary>
        /// Gets the recurring billing status that matches the specified subscription id.
        /// </summary>
        /// <param name="id">The subscription id.</param>
        /// <returns></returns>
        //public abstract GatewayRequest GetSubscriptionStatus(string id);

        #endregion

        /// <summary>
        /// Validates this instance.
        /// </summary>
        //public abstract void Validate();

        ///// <summary>
        ///// Asserts the validation.
        ///// </summary>
        ///// <param name="keys">The keys.</param>
        //public void AssertValidation(params string[] keys)
        //{
        //    var sb = new StringBuilder();
        //    foreach (var item in keys)
        //    {
        //        if (!KeyValues.ContainsKey(item))
        //        {
        //            sb.AppendFormat("{0}, ", item);
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(KeyValues[item]))
        //                sb.AppendFormat("No value for '{0}', which is required. ", item);
        //        }
        //        var result = sb.ToString();
        //        if (result.Length > 0)
        //            throw new InvalidDataException("Can't submit to Gateway - missing these input fields: " +
        //                                           result.Trim().TrimEnd(','));
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the post.
        ///// </summary>
        ///// <value>
        ///// The post.
        ///// </value>
        //public Dictionary<string, string> KeyValues { get; set; }
        
        ///// <summary>
        ///// Queues the specified key.
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public bool Queue(string key, string value)
        //{
        //    if (KeyValues.ContainsKey(key))
        //        KeyValues.Remove(key);

        //    KeyValues.Add(key, value);
        //    return true;
        //}

        ///// <summary>
        ///// Converts the Queue to a string.
        ///// </summary>
        ///// <returns></returns>
        //public string ToKeyValueString()
        //{
        //    var sb = new StringBuilder();
        //    foreach (var key in KeyValues.Keys)
        //        sb.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(KeyValues[key]));

        //    var result = sb.ToString();
        //    return result.TrimEnd('&');
        //}
    }
}