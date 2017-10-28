using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetBankAccount
    {
        public ANetBankAccountType AccountType { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string NameOnAccount { get; set; }
        public ANetECheckType EcheckType { get; set; }
        public string BankName { get; set; }
        public string CheckNumber { get; set; }
    }
}
