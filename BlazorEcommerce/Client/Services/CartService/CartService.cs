using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient http,AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
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

        /// <summary>
        /// Añade un producto al carrito, si el usuario está autenticado lo cogerá de la base de datos, si no, del LocalStorage
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        public async Task AddToCart(CartItem cartItem)
        {
            if((await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
            {
                await Console.Out.WriteLineAsync("The user is authenticated");
            }
            else
            {
                await Console.Out.WriteLineAsync("The user is NOT authenticated");
            }
            List<CartItem> cart = await InitializeLocalStorageAsync("cart");
            if(cart == null)
            {
                cart = new List<CartItem> {};
            }

            //Comprobamos si el item que queremos añadir está ya en la cesta
            var sameItem = cart.Find(x=>x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);

            //Si sameItem no está en la cesta, será igual a null
            if(sameItem == null)
            {
                cart.Add(cartItem);
            }
            //Si está en la cesta, incrementamos la cantidad
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

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



        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if(cart != null)
            {
                var cartItem = cart.Find(x=>x.ProductId == productId && x.ProductTypeId == productTypeId);
                
                if(cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                    OnChange?.Invoke();
                } 
            }
        }


        public async Task UpdateQuantity(CartProductResponseDTO product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart != null)
            {
                var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);

                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                    
                }
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart)
        {
           var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            
            //Clausula guarda para evitar niveles de indentación y que el código quede lo más claro posible
            if(localCart == null)
            {
                return;
            }

            await _http.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }

        }
    }
}
