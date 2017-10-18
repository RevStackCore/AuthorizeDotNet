using RevStackCore.Payment.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using RevStackCore.AuthorizeDotNet.Gateway;

namespace RevStackCore.AuthorizeDotNet.Model.Gateway
{
    public class GatewayResponse : IGatewayResponse, IPayment, ISubscription
    {
        public string[] RawResponse { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="delimitCharacter">The delimit character.</param>
        public GatewayResponse(string result, char delimitCharacter)
        {
            var rawResponse = result.Split(delimitCharacter);
            //if (rawResponse.Length == 1)
            //    throw new InvalidDataException(
            //        string.Format(
            //            "There was an error returned from AuthorizeNet: {0}; " +
            //            "this usually means your data sent along was incorrect. " +
            //            "Please recheck that all dates and amounts are formatted correctly",
            //            rawResponse[0]));
            RawResponse = rawResponse;
        }

        

        public GatewayResponse(string subscriptionId)
        {
            this.SubscriptionId = subscriptionId;
        }

        public string MD5Hash
        {
            get { return ParseResponse(37); }
        }

        public string CcvCode
        {
            get { return ParseResponse(38); }

        }

        public string CcvResponse
        {
            get
            {
                var code = CcvCode;

                switch (code)
                {
                    case "M":
                        return "Successful Match";
                    case "N":
                        return "The Card Code does not match";
                    case "P":
                        return "The Card Code was not processed";
                    case "S":
                        return "The Card Code should be on card, but is not indicated";
                    case "U":
                        return "Card Code is not supported by the card issuer";
                }
                return "";
            }
            private set { }
        }

        public string SubscriptionId { get; set; }
        public IList<string> SubscriptionResponse { get; set; }
        public int Code
        {
            get { return ParseInt(0); }
        }

        public int SubCode
        {
            get { return ParseInt(1); }
        }

        public string TransactionType
        {
            get { return ParseResponse(11); }
            private set { }
        }

        public string AuthorizationCode
        {
            get { return ParseResponse(4); }
            private set { }
        }

        public string Method
        {
            get { return ParseResponse(10); }
            private set { }
        }

        public decimal Amount
        {
            get { return ParseDecimal(9); }
            private set { }
        }

        public decimal Tax
        {
            get { return ParseDecimal(32); }
            private set { }
        }

        public string Id
        {
            get {
                return ParseResponse(6);
            }
            set { }
        }

        public string Message
        {
            get { return ParseResponse(3); }
            private set { }
        }

        public string FullResponse
        {
            get { return RawResponse.ToString(); }
        }

        public string InvoiceNumber
        {
            get { return ParseResponse(7); }
            private set { }
        }

        public string Description
        {
            get { return ParseResponse(8); }
            private set { }
        }

        public string ResponseCode
        {
            get { return ParseResponse(0); }
            private set { }
        }

        public string CardNumber
        {
            get { return ParseResponse(40); }
            private set { }
        }

        public string CardType
        {
            get { return ParseResponse(51); }
            private set { }
        }

        public string AvsCode
        {
            get { return ParseResponse(5); }
        }

        public string AvsResponse
        {
            get
            {
                var code = AvsCode;

                switch (code)
                {
                    case "A":
                        return "Address (Street) matches, ZIP does not";
                    case "B":
                        return "Address information not provided for AVS check";
                    case "E":
                        return "AVS error";
                    case "G":
                        return "Non-U.S. Card Issuing Bank";
                    case "N":
                        return "No Match on Address (Street) or ZIP";
                    case "P":
                        return "AVS not applicable for this transaction";
                    case "R":
                        return "Retry — System unavailable or timed out";
                    case "S":
                        return "Service not supported by issuer";
                    case "U":
                        return "Address information is unavailable";
                    case "W":
                        return "Nine digit ZIP matches, Address (Street) does not";
                    case "X":
                        return "Address (Street) and nine digit ZIP match";
                    case "Y":
                        return "Address (Street) and five digit ZIP match";
                    case "Z":
                        return "Five digit ZIP matches, Address (Street) does not";
                }
                return "";
            }
            private set { }
        }

        #region Status

        public bool Approved
        {
            get { return Code == 1; }
            private set { }
        }

        public bool Declined
        {
            get { return Code == 2; }
            private set { }
        }

        public bool Error
        {
            get { return Code == 3; }
            private set { }
        }

        public bool HeldForReview
        {
            get { return Code == 4; }
            private set { }
        }

        #endregion

        #region Address

        public string FirstName
        {
            get { return ParseResponse(13); }
            private set { }
        }

        public string LastName
        {
            get { return ParseResponse(14); }
            private set { }
        }

        public string Email
        {
            get { return ParseResponse(23); }
            private set { }
        }

        public string Company
        {
            get { return ParseResponse(15); }
            private set { }
        }

        public string Address
        {
            get { return ParseResponse(16); }
            private set { }
        }

        public string City
        {
            get { return ParseResponse(17); }
            private set { }
        }

        public string State
        {
            get { return ParseResponse(18); }
            private set { }
        }

        public string ZipCode
        {
            get { return ParseResponse(19); }
            private set { }
        }

        public string Country
        {
            get { return ParseResponse(20); }
            private set { }
        }

        #endregion

        #region Shipping

        public string ShipFirstName
        {
            get { return ParseResponse(24); }
            private set { }
        }

        public string ShipLastName
        {
            get { return ParseResponse(25); }
            private set { }
        }

        public string ShipCompany
        {
            get { return ParseResponse(26); }
            private set { }
        }

        public string ShipAddress
        {
            get { return ParseResponse(27); }
            private set { }
        }

        public string ShipCity
        {
            get { return ParseResponse(28); }
            private set { }
        }

        public string ShipState
        {
            get { return ParseResponse(29); }
            private set { }
        }

        public string ShipZipCode
        {
            get { return ParseResponse(30); }
            private set { }
        }

        public string ShipCountry
        {
            get { return ParseResponse(31); }
            private set { }
        }

        #endregion

        /// <summary>
        /// Parses the response to int.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private int ParseInt(int index)
        {
            var result = 0;
            if (RawResponse.Length > index)
                int.TryParse(RawResponse[index].ToString(CultureInfo.InvariantCulture), out result);
            return result;
        }

        /// <summary>
        /// Parses the response to decimal.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private decimal ParseDecimal(int index)
        {
            decimal result = 0;
            if (RawResponse.Length > index)
                decimal.TryParse(RawResponse[index].ToString(CultureInfo.InvariantCulture), out result);
            return result;
        }

        /// <summary>
        /// Parses the response.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string ParseResponse(int index)
        {
            var result = string.Empty;
            if (RawResponse.Length > index)
                result = RawResponse[index].ToString(CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// All of the possible keys returned from the API
        /// </summary>
        public Dictionary<int, string> ApiReponseKeys
        {
            get
            {
                var result = new Dictionary<int, string>
                                 {
                                     {1, "Response Code"},
                                     {2, "Response Subcode"},
                                     {3, "Response Reason Code"},
                                     {4, "Response Reason Text"},
                                     {5, "Authorization Code"},
                                     {6, "AVS Response"},
                                     {7, "Transaction ID"},
                                     {8, "Invoice Number"},
                                     {9, "Description"},
                                     {10, "Amount"},
                                     {11, "Method"},
                                     {12, "Transaction Type"},
                                     {13, "Customer ID"},
                                     {14, "First Name"},
                                     {15, "Last Name"},
                                     {16, "Company"},
                                     {17, "Address"},
                                     {18, "City"},
                                     {19, "State"},
                                     {20, "ZIP Code"},
                                     {21, "Country"},
                                     {22, "Phone"},
                                     {23, "Fax"},
                                     {24, "Email Address"},
                                     {25, "Ship To First Name"},
                                     {26, "Ship To Last Name"},
                                     {27, "Ship To Company"},
                                     {28, "Ship To Address"},
                                     {29, "Ship To City"},
                                     {30, "Ship To State"},
                                     {31, "Ship To ZIP Code"},
                                     {32, "Ship To Country"},
                                     {33, "Tax"},
                                     {34, "Duty"},
                                     {35, "Freight"},
                                     {36, "Tax Exempt"},
                                     {37, "Purchase Order Number"},
                                     {38, "MD5 Hash"},
                                     {39, "Card Code Response"},
                                     {40, "Cardholder Authentication Verification Response"},
                                     {41, "Account Number"},
                                     {42, "Card Type"},
                                     {43, "Split Tender ID"},
                                     {44, "Requested Amount"},
                                     {45, "Balance On Card"}
                                 };
                return result;
            }
        }

        /// <summary>
        /// Finds the Response element by value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public int FindByValue(string val)
        {
            var result = 0;
            for (var i = 0; i < RawResponse.Length; i++)
            {
                if (RawResponse[i].ToString(CultureInfo.InvariantCulture) != val)
                    continue;
                result = i;
                break;
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            var index = 0;
            foreach (var key in ApiReponseKeys.Keys)
            {
                sb.AppendFormat("{0} = {1}\n", ApiReponseKeys[key], ParseResponse(index));
                index++;
            }
            return sb.ToString();
        }
    }
}