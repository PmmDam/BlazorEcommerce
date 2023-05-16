namespace BlazorEcommerce.Server.Services.CartService
{
    public interface ICartService
    {
        /// <summary>
        /// Devuelve una ServiceResponse de todos los productos que están en el carrito
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProductsAsync(List<CartItem> cartItems);

        /// <summary>
        /// Guarda los productos de la cesta en la base de datos 
        /// </summary>
        /// <param name="cartItems"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems);

    }
}
