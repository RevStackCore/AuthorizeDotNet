using System;
namespace RevStackCore.AuthorizeDotNet
{
    public enum ANetTransactionType
    {
        authCaptureTransaction,
        authOnlyTransaction,
        authCaptureContinueTransaction,
        captureOnlyTransaction,
        priorAuthCaptureTransaction,
        refundTransaction,
        voidTransaction
    }
}
