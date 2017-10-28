using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetAuthorizeTransactionRequestBody
    {
        public ANetMerchantAuthentication MerchantAuthentication { get; set; }
        public string RefId { get; set; }
        public ANetAuthorizeTransactionRequest TransactionRequest { get; set; }
    }
}
