using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetVoidTransactionResponse : ANetBaseTransactionResponse
    {
        public IEnumerable<ANetSplitTenderPayment> SplitTenderPayments { get; set; }
    }
}
