using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        /// <summary>
        /// Devuelve una lista de CartItems que se almacenan utilizando las funcionalidades del paquete NuGet Blarozed.LocalStorage
        /// para almacenar el carrito de la compra sin tener que estar logueado de manera local con blazor
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<List<CartItem>> InitializeLocalStorageAsync(string key)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>(key);
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            List<CartItem> cart = await InitializeLocalStorageAsync("cart");
            cart.Add(cartItem);

            await _localStorage.SetItemAsync("cart", cart);
            OnChange?.Invoke();
        }


        public async Task<List<CartItem>> GetCartItems()
        {
            List<CartItem> cart = await InitializeLocalStorageAsync("cart");
            return cart;
        }

        public async Task<List<CartProductResponseDTO>> GetCartProducts()
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>().ConfigureAwait(false);

            return cartProducts.Data;

        }
    }
}
