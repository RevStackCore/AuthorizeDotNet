using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetCustomer
    {
        public string Id { get; set; }
        public ANetCustomerType Type { get; set; }
        public string Email { get; set; }
    }
}
