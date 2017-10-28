using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetSplitTenderPayment
    {
        public string TransId { get; set; }
        public ANetResponseCodeType? ResponseCode { get; set; }
        public string ResponseToCustomer { get; set; }
        public string AuthCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal? RequestedAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? BalanceOnCard { get; set; }
        public IEnumerable<ANetUserField> UserFields { get; set; }

    }
}
