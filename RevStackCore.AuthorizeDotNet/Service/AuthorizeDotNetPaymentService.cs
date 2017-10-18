using RevStackCore.Payment.Model;
using RevStackCore.Payment.Service;
using RevStackCore.AuthorizeDotNet.Repository;
using System.Collections.Generic;

namespace RevStackCore.AuthorizeDotNet.Service
{
    public class AuthorizeDotNetPaymentService : IPaymentService
    {
        private readonly AuthorizeDotNetPaymentRepository _repository;

        public AuthorizeDotNetPaymentService(AuthorizeDotNetPaymentRepository repository)
        {
            _repository = repository;
        }

        public T Authorize<T>(IAuthorize authorize) where T : IPayment
        {
            return _repository.Authorize<T>(authorize);
        }

        public T Capture<T>(ICapture capture) where T : IPayment
        {
            return _repository.Capture<T>(capture);
        }

        public T Charge<T>(ICharge charge) where T : IPayment
        {
            return _repository.Charge<T>(charge);
        }

        public T Credit<T>(ICredit credit) where T : IPayment
        {
            return _repository.Credit<T>(credit);
        }

        public T Void<T>(IVoid @void) where T : IPayment
        {
            return _repository.Void<T>(@void);
        }

        public T Get<T>() where T : IEnumerable<IPayment>
        {
            return _repository.Get<T>();
        }

        public T Get<T>(ITransactions transactions) where T : IEnumerable<IPayment>
        {
            return _repository.Get<T>(transactions);
        }

        public T GetById<T>(ITransactionDetails transactionDetails) where T : IPayment
        {
            return _repository.GetById<T>(transactionDetails);
        }
    }
}
