﻿using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetAuthorizeTransactionResponse : ANetBaseTransactionResponse
    {
        public IEnumerable<ANetSplitTenderPayment> SplitTenderPayments { get; set; }
        public IEnumerable<ANetUserField> UserFields { get; set; }
    }
}
