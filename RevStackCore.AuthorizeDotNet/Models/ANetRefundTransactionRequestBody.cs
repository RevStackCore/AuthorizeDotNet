using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetRefundTransactionRequestBody
    {
        public ANetMerchantAuthentication MerchantAuthentication { get; set; }
        public string RefId { get; set; }
        public ANetRefundTransactionRequest TransactionRequest { get; set; }
    }
}
