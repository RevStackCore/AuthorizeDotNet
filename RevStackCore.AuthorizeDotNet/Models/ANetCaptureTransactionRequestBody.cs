using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetCaptureTransactionRequestBody
    {
        public ANetMerchantAuthentication MerchantAuthentication { get; set; }
        public string RefId { get; set; }
        public ANetCaptureTransactionRequest TransactionRequest { get; set; }
    }
}
