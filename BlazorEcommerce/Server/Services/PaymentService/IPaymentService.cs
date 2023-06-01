using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSessionAsync();
        Task<ServiceResponse<bool>> FulfillOrderAsync(HttpRequest request);
    }
}
