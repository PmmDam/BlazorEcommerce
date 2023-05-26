﻿using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;
        private readonly ICartService _cartService;

        //Key para utilizar el webhook que nos comunicará si el pago de stripe ha ido bien
        const string secretWebhookKey = "whsec_fe41c9afa6c0823c2d901c8fdc16701fb0ba0a09b7b878e127a6a6ff80bd1ebf";

        public PaymentService(IOrderService orderService, IAuthService authService, ICartService cartService)
        {

            StripeConfiguration.ApiKey = "sk_test_51NBtmeJPujyepjLsyAGO81ij7HX9kl4zySOlIxLQcnUefl6YTxknXMVNqerEoOGzu9IJJEEKOdwUX66s7gRiSFdC006JBClxR5";
            _orderService = orderService;
            _authService = authService;
            _cartService = cartService;


        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;

            //Creamos las lineas que se verán en el checkout de stripe. Indicando el tipo de moneda, precio , nombre e imagenes
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            //Opciones de la sesión de stripe donde configurames el email del usuario autenticado, método de pago y las urls a las que navegar en caso de que todo vaya bien o no
            var options = new SessionCreateOptions
            {

                //Obtenemos el email de nuestro servicio de autenticación
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "ES", "FR","DK","DE"}
                },

                //Creamos una lista de strings por si añadiera más métodos de pago
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7173/order-success",
                CancelUrl = "https://localhost:7173/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], secretWebhookKey);

                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }
                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException ex)
            {
                return new ServiceResponse<bool> { Data = false,Success=false,Message=ex.Message };
            }
            finally
            {
                
            }
        }
    }
}