using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetCaptureTransactionResponse : ANetBaseTransactionResponse
    {
        public IEnumerable<ANetSplitTenderPayment> SplitTenderPayments { get; set; }
        public IEnumerable<ANetUserField> UserFields { get; set; }
    }
}
