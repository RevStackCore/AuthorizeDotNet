using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetCaptureCreateTransactionResponse : ANetBaseCreateTransactionResponse
    {
        public ANetCaptureTransactionResponse TransactionResponse { get; set; }
    }
}
