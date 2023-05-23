using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navManager;

        public OrderService(HttpClient http,AuthenticationStateProvider authStateProvider,NavigationManager navManager)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navManager = navManager;
        }

        public async Task PlaceOrder()
        {
            if(await IsUserAuthenticatedAsync())
            {
                await _http.PostAsync("api/order", null);
            }
            else
            {
                _navManager.NavigateTo("login");
            }
        }
        private async Task<bool> IsUserAuthenticatedAsync()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

    }
}
