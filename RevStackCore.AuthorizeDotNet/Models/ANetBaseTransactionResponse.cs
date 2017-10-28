using System;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet
{
    public class ANetBaseTransactionResponse
    {
        public ANetResponseCodeType ResponseCode { get; set; }
        public string AuthCode { get; set; }
        public ANetAvsResultCodeType? AvsResultCode { get; set; }
        public ANetCvvResultCodeType? CvvResultCode { get; set; }
        public ANetCaavResultCodeType? CavvResultCode { get; set; }
        public string TransId { get; set; }
        public string RefTransId { get; set; }
        public string TransHash { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public IEnumerable<ANetMessage> Messages { get; set; }
        public IEnumerable<ANetError> Errors { get; set; }
    }
}
