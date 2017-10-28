using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetChargeCreateTransactionResponse : ANetBaseCreateTransactionResponse
    {
        public ANetChargeTransactionResponse TransactionResponse { get; set; }
    }
}
