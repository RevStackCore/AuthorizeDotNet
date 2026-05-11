using System;
using System.Collections.Generic;
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
        private bool _testRequest { get; set; }

        public ANetProcessor(string loginId, string transactionKey, ANetProcessorModeType mode)
            : this(loginId, transactionKey, mode, false)
        {
        }

        /// <summary>
        /// Creates a processor with an explicit per-transaction test flag.
        /// When testRequest is true, every transaction sent through the
        /// convenience overloads (and any low-level overload whose caller
        /// hasn't already supplied TransactionSettings) is sent with
        /// settingName=testRequest, settingValue=true. The Authorize.NET
        /// gateway validates the request end-to-end without settling it.
        /// Orthogonal to the Sandbox/Live endpoint choice in <paramref name="mode"/>.
        /// </summary>
        public ANetProcessor(string loginId, string transactionKey, ANetProcessorModeType mode, bool testRequest)
        {
            _loginId = loginId;
            _transactionKey = transactionKey;
            _testRequest = testRequest;
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
            applyTestRequestIfNeeded(transaction);
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
            applyTestRequestIfNeeded(transaction);
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
            applyTestRequestIfNeeded(transaction);
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
            applyTestRequestIfNeeded(transaction);
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
            applyTestRequestIfNeeded(transaction);
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
            applyTestRequestIfNeeded(transaction);
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
        // Authorize.NET responses always include a top-level "messages" object with
        // a resultCode. When resultCode == "Error" the API-level call failed (auth,
        // schema, merchant config, etc.). In some of those cases the response also
        // contains a stub transactionResponse, so we cannot just key off
        // entity.TransactionResponse != null -- we must check the outer resultCode
        // first or we'll silently lose the error info.
        private bool tryGetApiError(string strResult, out IEnumerable<ANetMessage> messages)
        {
            messages = null;
            var outer = JSON.DeserializeObject<ANetCreateResponseMessages>(strResult);
            if (outer?.Messages != null &&
                string.Equals(outer.Messages.ResultCode, "Error", StringComparison.OrdinalIgnoreCase))
            {
                messages = outer.ToMessageEnumerable();
                return true;
            }
            return false;
        }

        private async Task<ANetChargeTransactionResponse> chargeAsync(ANetChargeCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request, true);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            if (tryGetApiError(strResult, out var apiErrors))
            {
                return new ANetChargeTransactionResponse
                {
                    ResponseCode = ANetResponseCodeType.Error,
                    Messages = apiErrors
                };
            }
            var entity = JSON.DeserializeObject<ANetChargeCreateTransactionResponse>(strResult);
            return entity.TransactionResponse ?? new ANetChargeTransactionResponse { ResponseCode = ANetResponseCodeType.Error };
        }

        private async Task<ANetAuthorizeTransactionResponse> authorizeAsync(ANetAuthorizeCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request, true);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            if (tryGetApiError(strResult, out var apiErrors))
            {
                return new ANetAuthorizeTransactionResponse
                {
                    ResponseCode = ANetResponseCodeType.Error,
                    Messages = apiErrors
                };
            }
            var entity = JSON.DeserializeObject<ANetAuthorizeCreateTransactionResponse>(strResult);
            return entity.TransactionResponse ?? new ANetAuthorizeTransactionResponse { ResponseCode = ANetResponseCodeType.Error };
        }

        private async Task<ANetCaptureTransactionResponse> captureAsync(ANetCaptureCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request, true);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            if (tryGetApiError(strResult, out var apiErrors))
            {
                return new ANetCaptureTransactionResponse
                {
                    ResponseCode = ANetResponseCodeType.Error,
                    Messages = apiErrors
                };
            }
            var entity = JSON.DeserializeObject<ANetCaptureCreateTransactionResponse>(strResult);
            return entity.TransactionResponse ?? new ANetCaptureTransactionResponse { ResponseCode = ANetResponseCodeType.Error };
        }

        private async Task<ANetRefundTransactionResponse> refundAsync(ANetRefundCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request, true);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            if (tryGetApiError(strResult, out var apiErrors))
            {
                return new ANetRefundTransactionResponse
                {
                    ResponseCode = ANetResponseCodeType.Error,
                    Messages = apiErrors
                };
            }
            var entity = JSON.DeserializeObject<ANetRefundCreateTransactionResponse>(strResult);
            return entity.TransactionResponse ?? new ANetRefundTransactionResponse { ResponseCode = ANetResponseCodeType.Error };
        }

        private async Task<ANetVoidTransactionResponse> voidAsync(ANetVoidCreateTransactionRequest request)
        {
            var json = JSON.SerializeObject(request, true);
            var strResult = await _endPoint.PostStringAsync(json).ReceiveString();
            if (tryGetApiError(strResult, out var apiErrors))
            {
                return new ANetVoidTransactionResponse
                {
                    ResponseCode = ANetResponseCodeType.Error,
                    Messages = apiErrors
                };
            }
            var entity = JSON.DeserializeObject<ANetVoidCreateTransactionResponse>(strResult);
            return entity.TransactionResponse ?? new ANetVoidTransactionResponse { ResponseCode = ANetResponseCodeType.Error };
        }
       
        private ANetMerchantAuthentication getMerchantAuthentication()
        {
            return new ANetMerchantAuthentication
            {
                Name = _loginId,
                TransactionKey = _transactionKey
            };
        }

        // Populates TransactionSettings with testRequest=true when the
        // processor was constructed with testRequest enabled. Skips if the
        // caller already supplied settings (don't clobber explicit values).
        private void applyTestRequestIfNeeded(ANetChargeTransactionRequest transaction)
        {
            if (!_testRequest) return;
            if (transaction.TransactionSettings != null) return;
            transaction.TransactionSettings = new ANetTransactionSettings
            {
                Setting = new List<ANetTransactionSetting>
                {
                    new ANetTransactionSetting { SettingName = "testRequest", SettingValue = "true" }
                }
            };
        }

        #endregion
    }
}
