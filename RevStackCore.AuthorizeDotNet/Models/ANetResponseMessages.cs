using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetResponseMessages
    {
        public string ResultCode { get; set; }
        public IEnumerable<ANetResponseMessage> Message { get; set; }
    }
}
