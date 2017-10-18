using RevStackCore.AuthorizeDotNet.Gateway;
using RevStackCore.Payment.Model;
using System;
using System.Collections.Generic;
using net.authorize.api;

namespace RevStackCore.AuthorizeDotNet.Model.Gateway
{
    public class Payment : IGatewayResponse, IPayment
    {
        public Payment(TransactionDetailsType transactionDetails)
        {
            this.Id = transactionDetails.transId;
            this.AuthorizationCode = transactionDetails.authCode;
            this.Amount = transactionDetails.settleAmount;
            this.ResponseCode = transactionDetails.responseCode.ToString();
            this.Code = transactionDetails.responseCode;
            this.AvsCode = transactionDetails.AVSResponse;
            this.Message = transactionDetails.responseReasonDescription;
            this.TransactionType = transactionDetails.transactionType;
            this.Message = transactionDetails.transactionStatus;

            if (transactionDetails.shipTo != null)
            {
                this.ShipFirstName = transactionDetails.shipTo.firstName;
                this.ShipLastName = transactionDetails.shipTo.lastName;
                this.ShipAddress = transactionDetails.shipTo.address;
                this.ShipCity = transactionDetails.shipTo.city;
                this.ShipState = transactionDetails.shipTo.state;
                this.ShipZipCode = transactionDetails.shipTo.zip;
                this.ShipCountry = transactionDetails.shipTo.country;
                this.ShipCompany = transactionDetails.shipTo.company;
            }

            if (transactionDetails.tax != null)
            {
                this.Tax = transactionDetails.tax.amount;
            }

            if (transactionDetails.billTo != null)
            {
                this.FirstName = transactionDetails.billTo.firstName;
                this.LastName = transactionDetails.billTo.lastName;
                this.Address = transactionDetails.billTo.address;
                this.City = transactionDetails.billTo.city;
                this.State = transactionDetails.billTo.state;
                this.ZipCode = transactionDetails.billTo.zip;
                this.Country = transactionDetails.billTo.country;
                this.Company = transactionDetails.billTo.company;
            }

            if (transactionDetails.order != null)
            {
                this.InvoiceNumber = transactionDetails.order.invoiceNumber;
            }
        }

        public Payment(TransactionSummaryType transaction)
        {
            this.Id = transaction.transId;
            this.Amount = transaction.settleAmount;
            this.FirstName = transaction.firstName;
            this.LastName = transaction.lastName;
            this.Message = transaction.transactionStatus;
            this.InvoiceNumber = transaction.invoiceNumber;
        }

        public string CcvCode { get; set; }

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
        }

        public string SubscriptionId { get; set; }
        public IList<string> SubscriptionResponse { get; set; }
        public int Code { get; set; }

        public int SubCode { get; set; }

        public string TransactionType { get; set; }

        public string AuthorizationCode { get; set; }

        public string Method { get; set; }

        public decimal Amount { get; set; }

        public decimal Tax { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public string InvoiceNumber { get; set; }

        public string Description { get; set; }

        public string ResponseCode { get; set; }

        public string CardNumber { get; set; }

        public string CardType { get; set; }

        public string AvsCode { get; set; }

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
        }

        #region Status

        public bool Approved
        {
            get { return Code == 1; }
        }

        public bool Declined
        {
            get { return Code == 2; }
        }

        public bool Error
        {
            get { return Code == 3; }
        }

        public bool HeldForReview
        {
            get { return Code == 4; }
        }

        #endregion

        #region Address

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        #endregion

        #region Shipping

        public string ShipFirstName { get; set; }

        public string ShipLastName { get; set; }

        public string ShipCompany { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipState { get; set; }

        public string ShipZipCode { get; set; }

        public string ShipCountry { get; set; }

        public string FullResponse { get; set; }

        #endregion
    }
}
