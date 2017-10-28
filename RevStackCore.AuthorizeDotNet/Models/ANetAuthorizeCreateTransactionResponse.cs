using System;
namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetAuthorizeCreateTransactionResponse : ANetBaseCreateTransactionResponse
    {
        public ANetAuthorizeTransactionResponse TransactionResponse { get; set; }
    }
}
