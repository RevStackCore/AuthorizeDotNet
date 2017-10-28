using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetVoidCreateTransactionResponse : ANetBaseCreateTransactionResponse
    {
        public ANetVoidTransactionResponse TransactionResponse { get; set; }
    }
}
