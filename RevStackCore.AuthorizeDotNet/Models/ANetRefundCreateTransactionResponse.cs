using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetRefundCreateTransactionResponse : ANetBaseCreateTransactionResponse
    {
        public ANetRefundTransactionResponse TransactionResponse { get; set; }
    }
}
