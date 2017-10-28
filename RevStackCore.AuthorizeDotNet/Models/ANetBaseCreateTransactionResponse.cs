using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    internal class ANetBaseCreateTransactionResponse
    {
        public string RefId { get; set; }
        public ANetCreateResponseMessages Messages { get; set; }
    }
}
