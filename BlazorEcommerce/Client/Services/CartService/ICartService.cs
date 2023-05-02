﻿namespace BlazorEcommerce.Client.Services.CartService
{
    public interface ICartService
    {
        /// <summary>
        /// Evento que se invocará cuando un producto cambie en el carrito de la compra
        /// </summary>
        event Action OnChange;

        Task AddToCart(CartItem cartItem);
        Task<List<CartItem>> GetCartItems();
    }
}