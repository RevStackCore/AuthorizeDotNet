using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetChargeTransactionRequestBody 
    {
        public ANetMerchantAuthentication MerchantAuthentication { get; set; }
        public string RefId { get; set; }
        public ANetChargeTransactionRequest TransactionRequest { get; set; }
    }
}
