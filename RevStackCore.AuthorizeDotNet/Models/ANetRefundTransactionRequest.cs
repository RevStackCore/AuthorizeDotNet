using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetRefundTransactionRequest
    {
        public ANetTransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public ANetExtendedPayment Payment { get; set; }
        public string TerminalNumber { get; set; }
        public string RefTransId { get; set; }
        public ANetOrder Order { get; set; }
        public IEnumerable<ANetLineItem> LineItems { get; set; }
        public string PONumber { get; set; }
        public ANetCustomer Customer { get; set; }
        public ANetBillTo BillTo { get; set; }
        public ANetShipTo ShipTo { get; set; }
        public string EmployeeId { get; set; }
        public IEnumerable<ANetTransactionSetting> TransactionSettings { get; set; }
        public IEnumerable<ANetUserField> UserFields { get; set; }
        public string MerchantDescriptor { get; set; }
    }
}
