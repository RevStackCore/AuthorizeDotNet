using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetRefundTransactionResponse : ANetBaseTransactionResponse
    {
        public IEnumerable<ANetUserField> UserFields { get; set; }
    }
}
