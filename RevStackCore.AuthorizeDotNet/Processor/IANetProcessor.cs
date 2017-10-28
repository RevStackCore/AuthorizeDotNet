using System;
using System.Threading.Tasks;

namespace RevStackCore.AuthorizeDotNet
{
    public interface IANetProcessor
    {
        Task<ANetChargeTransactionResponse> ChargeAsync(ANetCreditCard creditCard, string refId, decimal amount);
        Task<ANetChargeTransactionResponse> ChargeAsync(ANetCreditCard creditCard, ANetBillTo billing, string refId,  decimal amount);
        Task<ANetChargeTransactionResponse> ChargeAsync(ANetChargeTransactionRequest request, string refId);
        Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetCreditCard creditCard, string refId, decimal amount);
        Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetCreditCard creditCard, ANetBillTo billing, string refId, decimal amount);
        Task<ANetAuthorizeTransactionResponse> AuthorizeAsync(ANetAuthorizeTransactionRequest request, string refId);
        Task<ANetCaptureTransactionResponse> CaptureAsync(ANetCaptureTransactionRequest request, string refId);
        Task<ANetRefundTransactionResponse> RefundAsync(ANetRefundTransactionRequest request, string refId);
        Task<ANetVoidTransactionResponse> VoidAsync(ANetVoidTransactionRequest request, string refId);
    }
}
