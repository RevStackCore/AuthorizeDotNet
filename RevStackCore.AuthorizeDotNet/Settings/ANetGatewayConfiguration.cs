using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetGatewayConfiguration
    {
        public string ApiLogin { get; set; }
        public string ApiKey { get; set; }
        public ANetProcessorModeType ApiMode { get; set; }
    }
}
