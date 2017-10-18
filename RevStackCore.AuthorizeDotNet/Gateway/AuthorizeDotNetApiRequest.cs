using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using net.authorize.api;
using RevStackCore.AuthorizeDotNet.Model.Gateway;

namespace RevStackCore.AuthorizeDotNet.Gateway
{
    public class AuthorizeDotNetApiRequest 
    {
        public IGatewayResponse Send(AuthorizeDotNetRequest request)
        {
            IGatewayResponse response = null;

            if (request.ApiAction == RequestAction.Authorize ||
                request.ApiAction == RequestAction.Credit || 
                request.ApiAction == RequestAction.Charge ||
                request.ApiAction == RequestAction.Capture || 
                request.ApiAction == RequestAction.Void) 
            {
                response = SendHttpChargeRequest(request);
            }
            
            if (request.ApiAction == RequestAction.GetTransactionDetails)
                response = SendGetTransactionDetailsRequest(request);

            if (request.ApiAction == RequestAction.Subscribe)
                response = SendCreateSubscriptionRequest(request);

            if (request.ApiAction == RequestAction.CancelSubscription)
                response = SendCancelSubscriptionRequest(request);

            if (request.ApiAction == RequestAction.UpdateSubscription)
                response = SendUpdateSubscriptionRequest(request);

            return response;
        }

        public IEnumerable<IGatewayResponse> GetTransactions(AuthorizeDotNetRequest request)
        {
            return SendGetTransactionsRequest(request);
        }

        private IGatewayResponse SendHttpChargeRequest(AuthorizeDotNetRequest request) 
        {
            //validate the inputs
            request.Validate();
            var postData = request.ToKeyValueString();

            //override the local cert policy
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            var serviceUrl = request.PostUrl;
            var webRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            webRequest.Method = "POST";
            webRequest.ContentLength = postData.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            // post data is sent as a stream
            var myWriter = new StreamWriter(webRequest.GetRequestStream());
            myWriter.Write(postData);
            myWriter.Close();

            // returned values are returned as a stream, then read into a string
            var response = (HttpWebResponse)webRequest.GetResponse();
            var rawResponseStream = response.GetResponseStream();

            var result = string.Empty;
            if (rawResponseStream != null)
                using (var responseStream = new StreamReader(rawResponseStream))
                {
                    result = responseStream.ReadToEnd();
                    responseStream.Close();
                }

            IGatewayResponse gatewayResponse = new GatewayResponse(result,
                                                              request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].
                                                                  ToCharArray()
                                                                  [0]);

            return gatewayResponse;
        }

        private IGatewayResponse SendUnsettledTransactionsRequest(AuthorizeDotNetRequest request)
        {
            var result = string.Empty;
            IGatewayResponse gatewayResponse;

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                GetUnsettledTransactionListRequestType listType = new GetUnsettledTransactionListRequestType();
                GetUnsettledTransactionListResponseType response = webService.GetUnsettledTransactionList(authentication, listType, null);
                
                char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];

                for (int i = 0; i < response.messages.Length; i++)
                {
                    result = response.messages[i].text + del;
                }

                result = result.TrimEnd(del);
                gatewayResponse = new GatewayResponse(result, del);
            }

            return gatewayResponse;
        }

        private IEnumerable<IGatewayResponse> SendGetTransactionsRequest(AuthorizeDotNetRequest request)
        {
            var result = string.Empty;
            //IGatewayResponse gatewayResponse;
            var transactions = new List<Model.Gateway.Payment>();

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            string id = request.KeyValues[AuthorizeDotNetApi.BatchId];

            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                GetTransactionListRequestType listType = new GetTransactionListRequestType();
                listType.batchId = id;
                GetTransactionListResponseType response = webService.GetTransactionList(authentication, listType, null);
                char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];

                if (response == null)
                {
                    return transactions;
                }

                foreach (var transaction in response.transactions)
                {
                    var gatewayResponse = new Model.Gateway.Payment(transaction);
                    transactions.Add(gatewayResponse);
                }
            }            

            return transactions;
        }

        private IGatewayResponse SendGetTransactionDetailsRequest(AuthorizeDotNetRequest request) 
        {
            var result = string.Empty;
            IGatewayResponse gatewayResponse;

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            string id = request.KeyValues[AuthorizeDotNetApi.TransactionId];

            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                GetTransactionDetailsResponseType response = webService.GetTransactionDetails(authentication, id, null);
                char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];
                gatewayResponse = new Model.Gateway.Payment(response.transaction);
            }

            return gatewayResponse;
        }

        private IGatewayResponse SendCancelSubscriptionRequest(AuthorizeDotNetRequest request)
        {
            var result = string.Empty;
            IGatewayResponse gatewayResponse;

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            string id = request.KeyValues[AuthorizeDotNetApi.SubscriptionID];
            
            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                ARBCancelSubscriptionResponseType response = webService.ARBCancelSubscription(authentication, long.Parse(id), null);
                char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];
                IList<string> list = new List<string>();

                for (int i = 0; i < response.messages.Length; i++)
                {
                    result = response.messages[i].text + del;
                    list.Add(response.messages[i].text);
                }

                result = result.TrimEnd(del);
                gatewayResponse = new GatewayResponse(result, del);
                gatewayResponse.SubscriptionResponse = list;
            }

            return gatewayResponse;
        }

        private IGatewayResponse SendCreateSubscriptionRequest(AuthorizeDotNetRequest request)
        {
            var result = string.Empty;
            IGatewayResponse gatewayResponse = null;
            
            //string id = request.KeyValues[AuthorizeDotNetApi.SubscriptionID];

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            //do required first
            ARBSubscriptionType subscription = new ARBSubscriptionType();
            subscription.amount = decimal.Parse(request.KeyValues[AuthorizeDotNetApi.Amount]);
            subscription.amountSpecified = true;
            subscription.name = request.KeyValues[AuthorizeDotNetApi.SubscriptionName];

            PaymentType payment = new PaymentType();
            var creditCard = new CreditCardType();
            creditCard.cardCode = request.KeyValues[AuthorizeDotNetApi.CreditCardCode];
            creditCard.cardNumber = request.KeyValues[AuthorizeDotNetApi.CreditCardNumber];
            creditCard.expirationDate = request.KeyValues[AuthorizeDotNetApi.CreditCardExpiration];
            payment.Item = creditCard;
            subscription.payment = payment;

            CustomerType customer = new CustomerType();
            customer.id = request.KeyValues[AuthorizeDotNetApi.CustomerId];
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Fax))
                customer.email = request.KeyValues[AuthorizeDotNetApi.Email];
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Fax))
                customer.faxNumber = request.KeyValues[AuthorizeDotNetApi.Fax];           
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Phone))
                customer.phoneNumber = request.KeyValues[AuthorizeDotNetApi.Phone];
            //customer.type = CustomerTypeEnum.individual;
            customer.typeSpecified = false;
            //customer.taxId = request.KeyValues[AuthorizeDotNetApi.t];
            //customer.driversLicense = request.KeyValues[AuthorizeDotNetApi.];
            subscription.customer = customer;

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Address))
            {
                NameAndAddressType customerBilling = new NameAndAddressType();
                customerBilling.address = request.KeyValues[AuthorizeDotNetApi.Address];
                customerBilling.city = request.KeyValues[AuthorizeDotNetApi.City];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Company))
                    customerBilling.company = request.KeyValues[AuthorizeDotNetApi.Company];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Country))
                    customerBilling.country = request.KeyValues[AuthorizeDotNetApi.Country];
                customerBilling.firstName = request.KeyValues[AuthorizeDotNetApi.FirstName];
                customerBilling.lastName = request.KeyValues[AuthorizeDotNetApi.LastName];
                customerBilling.state = request.KeyValues[AuthorizeDotNetApi.State];
                customerBilling.zip = request.KeyValues[AuthorizeDotNetApi.Zip];
                subscription.billTo = customerBilling;
            }

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipAddress))
            {
                NameAndAddressType shipping = new NameAndAddressType();
                shipping.address = request.KeyValues[AuthorizeDotNetApi.ShipAddress];
                shipping.city = request.KeyValues[AuthorizeDotNetApi.ShipCity];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipCompany))
                    shipping.company = request.KeyValues[AuthorizeDotNetApi.ShipCompany];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipCountry))
                    shipping.country = request.KeyValues[AuthorizeDotNetApi.ShipCountry];
                shipping.firstName = request.KeyValues[AuthorizeDotNetApi.ShipFirstName];
                shipping.lastName = request.KeyValues[AuthorizeDotNetApi.ShipLastName];
                shipping.state = request.KeyValues[AuthorizeDotNetApi.ShipState];
                shipping.zip = request.KeyValues[AuthorizeDotNetApi.ShipZip];
                subscription.shipTo = shipping;
            }
            
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.InvoiceNumber)) 
            {
                OrderType order = new OrderType();
                order.invoiceNumber = request.KeyValues[AuthorizeDotNetApi.InvoiceNumber];
                subscription.order = order;
            }

           
            PaymentScheduleType paymentSchedule = new PaymentScheduleType();
            PaymentScheduleTypeInterval paymentScheduleTypeInterval = new PaymentScheduleTypeInterval();
            paymentScheduleTypeInterval.length = short.Parse(request.KeyValues[AuthorizeDotNetApi.BillingCycles]);
            paymentScheduleTypeInterval.unit = (ARBSubscriptionUnitEnum)Enum.Parse(typeof(ARBSubscriptionUnitEnum), request.KeyValues[AuthorizeDotNetApi.BillingInterval], true);
            paymentSchedule.interval = paymentScheduleTypeInterval;
            paymentSchedule.startDate = DateTime.Parse(request.KeyValues[AuthorizeDotNetApi.StartsOn].ToString());
            paymentSchedule.startDateSpecified = true;
            paymentSchedule.totalOccurrencesSpecified = true;
            paymentSchedule.totalOccurrences = short.Parse(request.KeyValues[AuthorizeDotNetApi.TotalOccurences].ToString());

            subscription.trialAmountSpecified = false;
            paymentSchedule.trialOccurrencesSpecified = false;

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.TrialAmount))
            {
                subscription.trialAmount = decimal.Parse(request.KeyValues[AuthorizeDotNetApi.TrialAmount]);
                subscription.trialAmountSpecified = true;
                paymentSchedule.trialOccurrences = short.Parse(request.KeyValues[AuthorizeDotNetApi.TrialBillingCycles]);
                paymentSchedule.trialOccurrencesSpecified = true;
            }

            subscription.paymentSchedule = paymentSchedule;

            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                var response = webService.ARBCreateSubscription(authentication, subscription, null);

                if (response.resultCode != MessageTypeEnum.Ok)
                {
                    char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];
                    IList<string> list = new List<string>();

                    for (int i = 0; i < response.messages.Length; i++)
                    {
                        result += response.messages[i].text + del;
                        list.Add(response.messages[i].text);
                    }

                    result = result.TrimEnd(del);
                    gatewayResponse = new GatewayResponse(result, del);
                    gatewayResponse.SubscriptionResponse = list;
                }
                else 
                {
                    IList<string> list = new List<string>();

                    for (int i = 0; i < response.messages.Length; i++)
                        list.Add(response.messages[i].text);
                    
                    gatewayResponse = new GatewayResponse(response.subscriptionId.ToString());
                    gatewayResponse.SubscriptionResponse = list;
                }
            }

            return gatewayResponse;
        }

        private IGatewayResponse SendUpdateSubscriptionRequest(AuthorizeDotNetRequest request)
        {
            var result = string.Empty;
            IGatewayResponse gatewayResponse = null;

            long id = long.Parse(request.KeyValues[AuthorizeDotNetApi.SubscriptionID]);

            var authentication = new MerchantAuthenticationType();
            authentication.name = request.KeyValues[AuthorizeDotNetApi.ApiLogin];
            authentication.transactionKey = request.KeyValues[AuthorizeDotNetApi.TransactionKey];

            //do required first
            ARBSubscriptionType subscription = new ARBSubscriptionType();
            subscription.amount = decimal.Parse(request.KeyValues[AuthorizeDotNetApi.Amount]);
            subscription.amountSpecified = true;
            subscription.name = request.KeyValues[AuthorizeDotNetApi.SubscriptionName];

            PaymentType payment = new PaymentType();
            var creditCard = new CreditCardType();
            creditCard.cardCode = request.KeyValues[AuthorizeDotNetApi.CreditCardCode];
            creditCard.cardNumber = request.KeyValues[AuthorizeDotNetApi.CreditCardNumber];
            creditCard.expirationDate = request.KeyValues[AuthorizeDotNetApi.CreditCardExpiration];
            payment.Item = creditCard;
            subscription.payment = payment;

            CustomerType customer = new CustomerType();
            customer.id = request.KeyValues[AuthorizeDotNetApi.CustomerId];
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Fax))
                customer.email = request.KeyValues[AuthorizeDotNetApi.Email];
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Fax))
                customer.faxNumber = request.KeyValues[AuthorizeDotNetApi.Fax];
            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Phone))
                customer.phoneNumber = request.KeyValues[AuthorizeDotNetApi.Phone];
            //customer.type = CustomerTypeEnum.individual;
            customer.typeSpecified = false;
            //customer.taxId = request.KeyValues[AuthorizeDotNetApi.t];
            //customer.driversLicense = request.KeyValues[AuthorizeDotNetApi.];
            subscription.customer = customer;

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Address))
            {
                NameAndAddressType customerBilling = new NameAndAddressType();
                customerBilling.address = request.KeyValues[AuthorizeDotNetApi.Address];
                customerBilling.city = request.KeyValues[AuthorizeDotNetApi.City];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Company))
                    customerBilling.company = request.KeyValues[AuthorizeDotNetApi.Company];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.Country))
                    customerBilling.country = request.KeyValues[AuthorizeDotNetApi.Country];
                customerBilling.firstName = request.KeyValues[AuthorizeDotNetApi.FirstName];
                customerBilling.lastName = request.KeyValues[AuthorizeDotNetApi.LastName];
                customerBilling.state = request.KeyValues[AuthorizeDotNetApi.State];
                customerBilling.zip = request.KeyValues[AuthorizeDotNetApi.Zip];
                subscription.billTo = customerBilling;
            }

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipAddress))
            {
                NameAndAddressType shipping = new NameAndAddressType();
                shipping.address = request.KeyValues[AuthorizeDotNetApi.ShipAddress];
                shipping.city = request.KeyValues[AuthorizeDotNetApi.ShipCity];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipCompany))
                    shipping.company = request.KeyValues[AuthorizeDotNetApi.ShipCompany];
                if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.ShipCountry))
                    shipping.country = request.KeyValues[AuthorizeDotNetApi.ShipCountry];
                shipping.firstName = request.KeyValues[AuthorizeDotNetApi.ShipFirstName];
                shipping.lastName = request.KeyValues[AuthorizeDotNetApi.ShipLastName];
                shipping.state = request.KeyValues[AuthorizeDotNetApi.ShipState];
                shipping.zip = request.KeyValues[AuthorizeDotNetApi.ShipZip];
                subscription.shipTo = shipping;
            }

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.InvoiceNumber))
            {
                OrderType order = new OrderType();
                order.invoiceNumber = request.KeyValues[AuthorizeDotNetApi.InvoiceNumber];
                subscription.order = order;
            }


            //PaymentScheduleType paymentSchedule = new PaymentScheduleType();
            //PaymentScheduleTypeInterval paymentScheduleTypeInterval = new PaymentScheduleTypeInterval();
            //paymentScheduleTypeInterval.length = short.Parse(request.KeyValues[AuthorizeDotNetApi.BillingCycles]);
            //paymentScheduleTypeInterval.unit = (ARBSubscriptionUnitEnum)Enum.Parse(typeof(ARBSubscriptionUnitEnum), request.KeyValues[AuthorizeDotNetApi.BillingInterval], true);
            //paymentSchedule.interval = paymentScheduleTypeInterval;
            //paymentSchedule.startDate = DateTime.Parse(request.KeyValues[AuthorizeDotNetApi.StartsOn].ToString());
            //paymentSchedule.startDateSpecified = true;
            //paymentSchedule.totalOccurrencesSpecified = true;
            //paymentSchedule.totalOccurrences = short.Parse(request.KeyValues[AuthorizeDotNetApi.TotalOccurences].ToString());
            //paymentSchedule.trialOccurrencesSpecified = false;

            subscription.trialAmountSpecified = false;
            

            if (request.KeyValues.ContainsKey(AuthorizeDotNetApi.TrialAmount))
            {
                subscription.trialAmount = decimal.Parse(request.KeyValues[AuthorizeDotNetApi.TrialAmount]);
                subscription.trialAmountSpecified = true;
                //paymentSchedule.trialOccurrences = short.Parse(request.KeyValues[AuthorizeDotNetApi.TrialBillingCycles]);
                //paymentSchedule.trialOccurrencesSpecified = true;
            }

            //authorize does not allow us to update intervals...
            //subscription.paymentSchedule = paymentSchedule;

            using (var webService = new RevStack.AuthorizeDotNet.net.authorize.api.Service())
            {
                webService.Url = request.PostUrl;
                var response = webService.ARBUpdateSubscription(authentication, id, subscription, null);

                if (response.resultCode != MessageTypeEnum.Ok)
                {
                    char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];
                    IList<string> list = new List<string>();

                    for (int i = 0; i < response.messages.Length; i++)
                    {
                        result += response.messages[i].text + del;
                        list.Add(response.messages[i].text);
                    }

                    result = result.TrimEnd(del);
                    gatewayResponse = new GatewayResponse(result, del);
                    gatewayResponse.SubscriptionResponse = list;
                }
                else
                {
                    IList<string> list = new List<string>();

                    for (int i = 0; i < response.messages.Length; i++)
                        list.Add(response.messages[i].text);

                    gatewayResponse = new GatewayResponse(id.ToString());
                    gatewayResponse.SubscriptionResponse = list;
                }

                //if (response.resultCode == MessageTypeEnum.Ok)
                //{
                //    char del = request.KeyValues[AuthorizeDotNetApi.DelimitCharacter].ToCharArray()[0];

                //    for (int i = 0; i < response.messages.Length; i++)
                //    {
                //        result = response.messages[i].text + del;
                //    }

                //    result = result.TrimEnd(del);
                //    gatewayResponse = new AuthorizeDotNetResponse(result, del);
                //    gatewayResponse.SubscriptionId = id.ToString();
                //}
            }

            return gatewayResponse;
        }
    }
}
