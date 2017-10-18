using RevStackCore.Payment;
using RevStackCore.Payment.Model;
using RevStackCore.Payment.Repository;
using RevStackCore.AuthorizeDotNet.Context;
using RevStackCore.AuthorizeDotNet.Gateway;

namespace RevStackCore.AuthorizeDotNet.Repository
{
    public class AuthorizeDotNetSubscriptionRepository : ISubscriptionRepository
    {
        private readonly AuthorizeDotNetContext _context;

        public AuthorizeDotNetSubscriptionRepository(AuthorizeDotNetContext context)
        {
            _context = context;
        }

        public T Subscribe<T>(ISubscribe subscribe) where T : ISubscription
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.Subscribe;
            request.AddCurrency(_context.Currency);
            if (subscribe.Customer != null)
            {
                request.AddCustomer(subscribe.Customer);
            }
            if (subscribe.Shipping != null)
            {
                request.AddShipping(subscribe.Shipping);
            }
            request.Subscribe(subscribe);
            return (T)_context.Send(request);
        }

        public T Update<T>(IUpdateSubscription subscription) where T : ISubscription
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.UpdateSubscription;
            request.AddCurrency(_context.Currency);
            if (subscription.Customer != null)
            {
                request.AddCustomer(subscription.Customer);
            }
            if (subscription.Shipping != null)
            {
                request.AddShipping(subscription.Shipping);
            }
            request.UpdateSubscription(subscription);
            return (T)_context.Send(request);
        }

        public T Cancel<T>(ICancelSubscription subscription) where T : ISubscription
        {
            var request = _context.Create();
            request.ApiAction = RequestAction.CancelSubscription;
            request.CancelSubscription(subscription);
            return (T)_context.Send(request);
        }
    }
}
