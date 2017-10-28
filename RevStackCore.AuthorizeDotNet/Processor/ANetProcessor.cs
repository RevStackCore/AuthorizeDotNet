using System;
using System.Threading.Tasks;
using Flurl.Http;
using JSON=RevStackCore.Serialization.Json;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetProcessor : IANetProcessor
    {
        const string sandboxEndPoint = "https://apitest.authorize.net/xml/v1/request.api";
        const string liveEndPoint = "https://api.authorize.net/xml/v1/request.api";
        private string _loginId { get; set; }
        private string _transactionKey { get; set; }
        private string _endPoint { get; set; }
        public ANetProcessor(string loginId, string transactionKey, ANetProcessorModeType mode) 
        {
            _loginId = loginId;
            _transactionKey = transactionKey;
            if(mode==ANetProcessorModeType.Sandbox)
            {
                _endPoint = sandboxEndPoint;
            }
            else
            {
                _endPoint = liveEndPoint;
            }
        }

        /// <summary>
        /// Charge Credit Card Async.
        /// </summary>
        /// <returns>ANetChargeCreateTransactionResponse instance</returns>
        /// <param name="creditCard">Credit card</param>
        /// <param name="refId">Reference identifier</param>
        /// <param name="amount">Amount</param>
        public async Task<ANetChargeTransactionResponse> ChargeAsync(ANetCreditCard creditCard,string refId, decimal amount)
        {
            var createRequest = new ANetChargeCreateTransactionRequest();
            var requestBody = new ANetChargeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            var transaction = new ANetChargeTransactionRequest();
            var payment = new ANetPayment
            {
                CreditCard = creditCard
            };
            transaction.Payment = payment;
            transaction.Amount = amount;
            transaction.TransactionType = ANetTransactionType.authCaptureTransaction;
            requestBody.TransactionRequest = transaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            createRequest.CreateTransactionRequest = requestBody;
            return await chargeAsync(createRequest);
        }

        /// <summary>
        /// Charge Credit Card Async.
        /// </summary>
        /// <returns>ANetChargeCreateTransactionResponse instance</returns>
        /// <param name="creditCard">Credit card.</param>
        /// <param name="refId">Reference identifier.</param>
        /// <param name="billing">Billing.</param>
        /// <param name="amount">Amount.</param>
        public async Task<ANetChargeTransactionResponse> ChargeAsync(ANetCreditCard creditCard, ANetBillTo billing,string refId, decimal amount)
        {
            var createRequest = new ANetChargeCreateTransactionRequest();
            var requestBody = new ANetChargeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            var transaction = new ANetChargeTransactionRequest();
            var payment = new ANetPayment
            {
                CreditCard = creditCard
            };
            transaction.Payment = payment;
            transaction.Amount = amount;
            transaction.TransactionType = ANetTransactionType.authCaptureTransaction;
            transaction.BillTo = billing;
            requestBody.TransactionRequest = transaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            createRequest.CreateTransactionRequest = requestBody;
            return await chargeAsync(createRequest);
        }

        /// <summary>
        /// Charge Credit Card Async.
        /// </summary>
        /// <returns>ANetChargeCreateTransactionResponse instance</returns>
        /// <param name="transaction">TransactionRequest.</param>
        /// <param name="refId">Reference identifier.</param>
        public async Task<ANetChargeTransactionResponse> ChargeAsync(ANetChargeTransactionRequest transaction, string refId)
        {
            var createRequest = new ANetChargeCreateTransactionRequest();
            var requestBody = new ANetChargeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            transaction.TransactionType = ANetTransactionType.authCaptureTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await chargeAsync(createRequest);
        }



        /// <summary>
        /// Authorize Credit Card Async.
        /// </summary>
        /// <returns>ANetAuthorizeCreateTransactionResponse instance</returns>
        /// <param name="creditCard">Credit card</param>
        /// <param name="refId">Reference identifier</param>
        /// <param name="amount">Amount</param>
        public async Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetCreditCard creditCard, string refId, decimal amount)
        {
            var createRequest = new ANetAuthorizeCreateTransactionRequest();
            var requestBody = new ANetAuthorizeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            var transaction = new ANetAuthorizeTransactionRequest();
            var payment = new ANetPayment
            {
                CreditCard = creditCard
            };
            transaction.Payment = payment;
            transaction.Amount = amount;
            transaction.TransactionType = ANetTransactionType.authOnlyTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await authorizeAsync(createRequest);
        }

        /// <summary>
        /// Authorize Credit Card Async.
        /// </summary>
        /// <returns>ANetAuthorizeCreateTransactionResponse instance</returns>
        /// <param name="creditCard">Credit card.</param>
        /// <param name="refId">Reference identifier.</param>
        /// <param name="billing">Billing.</param>
        /// <param name="amount">Amount.</param>
        public async Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetCreditCard creditCard,ANetBillTo billing, string refId,  decimal amount)
        {
            var createRequest = new ANetAuthorizeCreateTransactionRequest();
            var requestBody = new ANetAuthorizeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            var transaction = new ANetAuthorizeTransactionRequest();
            var payment = new ANetPayment
            {
                CreditCard = creditCard
            };
            transaction.Payment = payment;
            transaction.Amount = amount;
            transaction.TransactionType = ANetTransactionType.authOnlyTransaction;
            transaction.BillTo = billing;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await authorizeAsync(createRequest);
        }

        /// <summary>
        /// Authorize Credit Card Async.
        /// </summary>
        /// <returns>ANetAuthorizeCreateTransactionResponse instance</returns>
        /// <param name="transaction">TransactionRequest.</param>
        /// <param name="refId">Reference identifier.</param>
        public async Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetAuthorizeTransactionRequest transaction, string refId)
        {
            var createRequest = new ANetAuthorizeCreateTransactionRequest();
            var requestBody = new ANetAuthorizeTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            transaction.TransactionType = ANetTransactionType.authOnlyTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await authorizeAsync(createRequest);
        }

        /// <summary>
        /// Capture Credit Card Async.
        /// </summary>
        /// <returns>ANetCaptureCreateTransactionResponse instance</returns>
        /// <param name="transaction">TransactionRequest.</param>
        /// <param name="refId">Reference identifier.</param>
        public async Task<ANetCaptureTransactionResponse> CaptureAsync(ANetCaptureTransactionRequest transaction, string refId)
        {
            var createRequest = new ANetCaptureCreateTransactionRequest();
            var requestBody = new ANetCaptureTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            transaction.TransactionType = ANetTransactionType.priorAuthCaptureTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await captureAsync(createRequest);
        }

        /// <summary>
        /// Refund Credit Card Async.
        /// </summary>
        /// <returns>ANetRefundCreateTransactionResponse instance</returns>
        /// <param name="transaction">TransactionRequest.</param>
        /// <param name="refId">Reference identifier.</param>
        public async Task<ANetRefundTransactionResponse> RefundAsync(ANetRefundTransactionRequest transaction, string refId)
        {
            var createRequest = new ANetRefundCreateTransactionRequest();
            var requestBody = new ANetRefundTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            transaction.TransactionType = ANetTransactionType.refundTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await refundAsync(createRequest);
        }

        /// <summary>
        /// Void Credit Card Async.
        /// </summary>
        /// <returns>ANetVoidCreateTransactionResponse instance</returns>
        /// <param name="transaction">TransactionRequest.</param>
        /// <param name="refId">Reference identifier.</param>
        public async Task<ANetVoidTransactionResponse> VoidAsync(ANetVoidTransactionRequest transaction, string refId)
        {
            var createRequest = new ANetVoidCreateTransactionRequest();
            var requestBody = new ANetVoidTransactionRequestBody();
            var merchantAuthentication = getMerchantAuthentication();
            transaction.TransactionType = ANetTransactionType.voidTransaction;
            requestBody.MerchantAuthentication = merchantAuthentication;
            requestBody.RefId = refId;
            requestBody.TransactionRequest = transaction;
            createRequest.CreateTransactionRequest = requestBody;
            return await voidAsync(createRequest);
        }

        #region "Private"
        private async Task<ANetChargeTransactionResponse> chargeAsync(ANetChargeCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            var entity = JSON.DeserializeObject<ANetChargeCreateTransactionResponse>(strResult);
            if (entity.TransactionResponse != null)
            {
                return entity.TransactionResponse;
            }
            else
            {
                //return a new transactionResponse object with the message list
                var errorEntity = new ANetChargeTransactionResponse();
                var messages = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
                errorEntity.ResponseCode = ANetResponseCodeType.Error;
                errorEntity.Messages = messages.ToMessageEnumerable();
                return errorEntity;
            }
        }

        private async Task<ANetAuthorizeTransactionResponse> authorizeAsync(ANetAuthorizeCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            var entity = JSON.DeserializeObject<ANetAuthorizeCreateTransactionResponse>(strResult);
            if (entity.TransactionResponse != null)
            {
                return entity.TransactionResponse;
            }
            else
            {
                //return a new transactionResponse object with the message list
                var errorEntity = new ANetAuthorizeTransactionResponse();
                var messages = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
                errorEntity.ResponseCode = ANetResponseCodeType.Error;
                errorEntity.Messages = messages.ToMessageEnumerable();
                return errorEntity;
            }
        }

        private async Task<ANetCaptureTransactionResponse> captureAsync(ANetCaptureCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            var entity = JSON.DeserializeObject<ANetCaptureCreateTransactionResponse>(strResult);
            if (entity.TransactionResponse != null)
            {
                return entity.TransactionResponse;
            }
            else
            {
                //return a new transactionResponse object with the message list
                var errorEntity = new ANetCaptureTransactionResponse();
                var messages = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
                errorEntity.ResponseCode = ANetResponseCodeType.Error;
                errorEntity.Messages = messages.ToMessageEnumerable();
                return errorEntity;
            }
        }

        private async Task<ANetRefundTransactionResponse> refundAsync(ANetRefundCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            var entity = JSON.DeserializeObject<ANetRefundCreateTransactionResponse>(strResult);
            if (entity.TransactionResponse != null)
            {
                return entity.TransactionResponse;
            }
            else
            {
                //return a new transactionResponse object with the message list
                var errorEntity = new ANetRefundTransactionResponse();
                var messages = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
                errorEntity.ResponseCode = ANetResponseCodeType.Error;
                errorEntity.Messages = messages.ToMessageEnumerable();
                return errorEntity;
            }
        }

        private async Task<ANetVoidTransactionResponse> voidAsync(ANetVoidCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            var entity = JSON.DeserializeObject<ANetVoidCreateTransactionResponse>(strResult);
            if (entity.TransactionResponse != null)
            {
                return entity.TransactionResponse;
            }
            else
            {
                //return a new transactionResponse object with the message list
                var errorEntity = new ANetVoidTransactionResponse();
                var messages = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
                errorEntity.ResponseCode = ANetResponseCodeType.Error;
                errorEntity.Messages = messages.ToMessageEnumerable();
                return errorEntity;
            }
        }
       
        private ANetMerchantAuthentication getMerchantAuthentication()
        {
            return new ANetMerchantAuthentication
            {
                Name = _loginId,
                TransactionKey = _transactionKey
            };
        }

        #endregion
    }
}
