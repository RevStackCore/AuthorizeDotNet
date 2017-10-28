using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetCaptureTransactionRequest
    {
        public ANetTransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string TerminalNumber { get; set; }
        public string RefTransId { get; set; }
        public ANetOrder Order { get; set; }
    }
}
