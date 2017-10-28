using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetVoidTransactionRequestBody
    {
        public ANetMerchantAuthentication MerchantAuthentication { get; set; }
        public string RefId { get; set; }
        public ANetVoidTransactionRequest TransactionRequest { get; set; }
    }
}
