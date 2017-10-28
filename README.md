# AuthorizeDotNet
A .NET Core Authorize.Net Sdk

[![Build status](https://ci.appveyor.com/api/projects/status/py61ohca5g3ffoeh?svg=true)](https://ci.appveyor.com/project/tachyon1337/authorizedotnet-tuwwq)

A core Authorize.Net sdk for charge, authorize, refund and void transactions.

# Nuget Installation

``` bash
Install-Package RevStackCore.AuthorizeDotNet

```

# Payment Processor Interface Abstraction

The sdk payment processor implements the following interface for asynchronous processing of charge, authorize, refund and void transactions. The entities 
correspond to the Authorize.Net API reference https://developer.authorize.net/api/reference/index.html

```cs
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

```

# Payment Processor Constructor

The sdk payment processor constructor takes 3 arguments: the Authorize.Net loginId and transactionKey, and the processor mode: Live or Sandbox.

```cs
public ANetProcessor(string loginId, string transactionKey, ANetProcessorModeType mode)

```

# Sample Usage

```cs
using RevStackCore.AuthorizeDotNet;

var processor = new ANetProcessor(LOGIN_ID,TRANSACTION_KEY,ANetProcessorModeType.Sandbox)
var response = await processor.ChargeAsync(creditCard,REFERENCE_ID, AMOUNT);
if (response.ResponseCode == ANetResponseCodeType.Approved)
{
    //authCode=response.AuthCode;
    //transId=response.TransId;
    //do something
}
else
{
    if(response.Errors !=null && response.Errors.Any())
    {
        //errorText=response.Errors.FirstOrDefault().ErrorText;
        //errorCode = response.Errors.FirstOrDefault().ErrorCode;
    }
    else
    {
        //messageDescription=response.Messages.FirstOrDefault().Description;
        //messageCode=response.Messages.FirstOrDefault().Code;
    }
    
    //do seomthing

}

```