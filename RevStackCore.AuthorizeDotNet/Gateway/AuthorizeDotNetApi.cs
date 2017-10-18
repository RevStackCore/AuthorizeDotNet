using System.Collections.Generic;

namespace RevStack.AuthorizeDotNet.Gateway
{
    internal class AuthorizeDotNetApi 
    {
        #region Api field names (constants)

        /// <summary>
        /// The merchant's unique API Login ID
        /// </summary>
        public const string ApiLogin = "x_login";

        /// <summary>
        /// The merchant's unique Transaction Key
        /// </summary>	
        public const string TransactionKey = "x_tran_key";

        /// <summary>
        /// True, False
        /// </summary>
        public const string AllowPartialAuth = "x_allow_partial_Auth";

        /// <summary>
        /// Whether to return the data in delimited fashion
        /// </summary>
        public const string DelimitData = "x_delim_data";

        /// <summary>
        /// If the return from AuthorizeNet is delimited - this is the character to use. Default is pipe
        /// </summary>
        public const string DelimitCharacter = "x_delim_char";

        /// <summary>
        /// The relay response - leave this set as TRUE
        /// </summary>
        public const string RelayResponse = "x_relay_response";

        /// <summary>
        /// Emails a sales receipt created by the Authorize.Net system to the client
        /// </summary>
        public const string EmailCustomer = "x_email_customer";

        /// <summary>
        /// Required - The merchant's transaction version
        /// </summary>
        public const string ApiVersion = "x_version";

        /// <summary>
        /// The type of transaction:
        /// AUTH_CAPTURE (default), AUTH_ONLY, CAPTURE_ONLY, CREDIT, PRIOR_AUTH_CAPTURE, VOID
        /// </summary>
        public const string TransactionType = "x_type";

        /// <summary>
        /// CC or ECHECK
        /// </summary>
        public const string Method = "x_method";

        /// <summary>
        /// The recurring billing status
        /// </summary>
        public const string RecurringBilling = "x_recurring_billing";

        /// <summary>
        /// The amount of the transaction
        /// </summary>
        public const string Amount = "x_amount";

        /// <summary>
        /// The credit card number - between 13 and 16 digits without spaces. When x_type=CREDIT, only the last four digits are required
        /// </summary>
        public const string CreditCardNumber = "x_card_num";

        /// <summary>
        /// The expiration date - MMYY, MM/YY, MM-YY, MMYYYY, MM/YYYY, MM-YYYY
        /// </summary>
        public const string CreditCardExpiration = "x_exp_date";

        /// <summary>
        /// The three- or four-digit number on the back of a credit card (on the front for American Express).
        /// </summary>
        public const string CreditCardCode = "x_card_code";

        /// <summary>
        /// The payment gateway assigned transaction ID of an original transaction - Required only for CREDIT, PRIOR_ AUTH_ CAPTURE, and VOID transactions
        /// </summary>
        public const string TransactionId = "x_trans_id";

        /// <summary>
        /// The payment gateway-assigned ID assigned when the original transaction includes  two or more partial payments. This is the identifier that is used to group transactions that are part of a split tender order.
        /// </summary>
        public const string SplitTender = "x_split_tender";

        /// <summary>
        /// The authorization code of an original transaction not authorized on the payment gateway
        /// </summary>
        public const string AuthorizationCode = "x_auth_code";

        /// <summary>
        /// The request to process test transactions
        /// </summary>
        public const string IsTestRequest = "x_test_request";

        /// <summary>
        /// The window of time after the submission of a transaction that a duplicate transaction can not be submitted
        /// </summary>
        public const string DuplicateWindowTime = "x_duplicate_window";

        /// <summary>
        /// The merchant assigned invoice number for the transaction
        /// </summary>
        public const string InvoiceNumber = "x_invoice_num";

        /// <summary>
        /// The transaction description
        /// </summary>
        public const string Description = "x_description";

        ///<summary>
        ///</summary>
        public const string FirstName = "x_first_name";

        ///<summary>
        ///</summary>
        public const string LastName = "x_last_name";

        ///<summary>
        ///</summary>
        public const string Company = "x_company";

        ///<summary>
        ///</summary>
        public const string Address = "x_address";

        ///<summary>
        ///</summary>
        public const string City = "x_city";

        ///<summary>
        ///</summary>
        public const string State = "x_state";

        ///<summary>
        ///</summary>
        public const string Zip = "x_zip";

        ///<summary>
        ///</summary>
        public const string Country = "x_country";

        ///<summary>
        ///</summary>
        public const string Phone = "x_phone";

        ///<summary>
        ///</summary>
        public const string Fax = "x_fax";

        ///<summary>
        ///</summary>
        public const string Email = "x_email";

        /// <summary>
        /// The ID of the Customer as relates to your application
        /// </summary>
        public const string CustomerId = "x_cust_id";

        ///<summary>
        ///</summary>
        public const string CustomerIpAddress = "x_cust_ip";

        ///<summary>
        ///</summary>
        public const string ShipFirstName = "x_ship_to_first_name";

        ///<summary>
        ///</summary>
        public const string ShipLastName = "x_ship_to_last_name";

        ///<summary>
        ///</summary>
        public const string ShipCompany = "x_ship_to_company";

        ///<summary>
        ///</summary>
        public const string ShipAddress = "x_ship_to_address";

        ///<summary>
        ///</summary>
        public const string ShipCity = "x_ship_to_city";

        ///<summary>
        ///</summary>
        public const string ShipState = "x_ship_to_state";

        ///<summary>
        ///</summary>
        public const string ShipZip = "x_ship_to_zip";

        ///<summary>
        ///</summary>
        public const string ShipCountry = "x_ship_to_country";

        ///<summary>
        ///</summary>
        public const string Tax = "x_tax";

        ///<summary>
        ///</summary>
        public const string Freight = "x_frieght";

        ///<summary>
        ///</summary>
        public const string Duty = "x_duty";

        ///<summary>
        ///</summary>
        public const string TaxExempt = "x_tax_exempt";

        ///<summary>
        ///</summary>
        public const string PoNumber = "x_po_num";



        //Subscription constants
        ///<summary>
        ///</summary>
        public const string BillingCycles = "x_billing_cycles";

        ///<summary>
        ///</summary>
        public const string BillingInterval = "x_billing_interval";

        ///<summary>
        ///</summary>
        public const string StartsOn = "x_starts_on";
        
        ///<summary>
        ///</summary>
        public const string SubscriptionID = "x_subscription_id";

        ///<summary>
        ///</summary>
        public const string SubscriptionName = "x_subscription_name";
        
        ///<summary>
        ///</summary>
        public const string TrialAmount = "x_trial_amount";
        
        ///<summary>
        ///</summary>
        public const string TrialBillingCycles = "x_trial_billing_cycles";

        ///<summary>
        ///</summary>
        public const string TotalOccurences = "x_total_occurences";

        ///<summary>
        ///</summary>
        public const string BatchId = "x_batch_id";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetApi"/> class.
        /// </summary>
        public AuthorizeDotNetApi()
        {
            ApiKeys = new List<string>
                          {
                              "x_login",
                              "x_tran_key",
                              "x_allow_partial_Auth",
                              "x_delim_data",
                              "x_delim_char",
                              "x_relay_response",
                              "x_version",
                              "x_type",
                              "x_method",
                              "x_recurring_billing",
                              "x_amount",
                              "x_card_num",
                              "x_exp_date",
                              "x_card_code",
                              "x_trans_id",
                              "x_split_tender",
                              "x_auth_code",
                              "x_test_request",
                              "x_duplicate_window",
                              "x_invoice_num",
                              "x_description",
                              "x_first_name",
                              "x_last_name",
                              "x_company",
                              "x_address",
                              "x_city",
                              "x_state",
                              "x_zip",
                              "x_country",
                              "x_phone",
                              "x_fax",
                              "x_email",
                              "x_cust_id",
                              "x_cust_ip",
                              "x_ship_to_first_name",
                              "x_ship_to_last_name",
                              "x_ship_to_company",
                              "x_ship_to_address",
                              "x_ship_to_city",
                              "x_ship_to_state",
                              "x_ship_to_zip",
                              "x_ship_to_country",
                              "x_tax",
                              "x_frieght",
                              "x_duty",
                              "x_tax_exempt",
                              "x_po_num",
                              "x_billing_cycles", //subscription
                              "x_billing_interval",
                              "x_description",
                              "x_starts_on",
                              "x_subscription_id",
                              "x_subscription_name",
                              "x_trial_amount",
                              "x_trial_billing_cycles",
                              "x_batch_id",
                              "x_total_occurences"
                          };
        }

        /// <summary>
        /// Gets or sets the API keys.
        /// </summary>
        /// <value>
        /// The API keys.
        /// </value>
        public IList<string> ApiKeys { get; set; }

        /// <summary>
        /// Finds out if the API contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool ApiContainsKey(string key)
        {
            return ApiKeys.Contains(key);
        }
    }
}