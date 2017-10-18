using RevStackCore.Payment.Model;
using RevStackCore.Payment.Repository;
using RevStackCore.AuthorizeDotNet.Context;
using RevStackCore.AuthorizeDotNet.Gateway;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet.Repository
{
    public class AuthorizeDotNetPaymentRepository : IPaymentRepository
    {
        private readonly AuthorizeDotNetContext _context;

        public AuthorizeDotNetPaymentRepository(AuthorizeDotNetContext context)
        {
            _context = context;
        }

        public T Authorize<T>(IAuthorize authorize) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Authorize;
            request.AddCustomer(authorize.Customer);
            request.AddShipping(authorize.Shipping);
            request.AddCurrency(_context.Currency);
            request.Authorize(authorize);
            return (T)_context.Send(request);
        }

        public T Capture<T>(ICapture capture) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Capture;
            request.Capture(capture);
            return (T)_context.Send(request);
        }

        public T Charge<T>(ICharge charge) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Charge;
            request.AddCustomer(charge.Customer);
            request.AddShipping(charge.Shipping);
            request.AddCurrency(_context.Currency);
            request.Charge(charge);
            return (T)_context.Send(request);
        }

        public T Credit<T>(ICredit credit) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Credit;
            request.Credit(credit);
            return (T)_context.Send(request);
        }

        public T Void<T>(IVoid @void) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Void;
            request.Void(@void);
            return (T)_context.Send(request);
        }

        public T Get<T>() where T : IEnumerable<IPayment>
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.GetTransactions;
            request.GetTransactions(null);
            return (T)_context.Send(request);
        }

        public T Get<T>(ITransactions transactions) where T : IEnumerable<IPayment>
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.GetTransactions;
            request.GetTransactions(transactions);
            return (T)_context.GetTransactions(request);            
        }

        public T GetById<T>(ITransactionDetails transactionDetails) where T : IPayment
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.GetTransactionDetails;
            request.GetTransactionDetails(transactionDetails);
            return (T)_context.Send(request);
        }
    }
}
