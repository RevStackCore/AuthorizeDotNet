using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetExtendedPayment : ANetPayment
    {
        public ANetBankAccount BankAccount { get; set; }
    }
}
