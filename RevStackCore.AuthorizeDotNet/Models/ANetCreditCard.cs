using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetCreditCard
    {
        public string CardNumber { get; set; }
        /// <summary>
        /// YYYY-MM
        /// </summary>
        /// <value>The expiration date.</value>
        public string ExpirationDate { get; set; }
        public string CardCode { get; set; }
    }
}
