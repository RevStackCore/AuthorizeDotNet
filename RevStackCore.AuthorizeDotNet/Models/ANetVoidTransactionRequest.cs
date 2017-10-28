using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetVoidTransactionRequest
    {
        public ANetTransactionType TransactionType { get; set; }
        public string TerminalNumber { get; set; }
        public string RefTransId { get; set; }
    }
}
