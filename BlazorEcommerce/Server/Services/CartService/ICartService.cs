namespace BlazorEcommerce.Server.Services.CartService
{
    public interface ICartService
    {
        /// <summary>
        /// Devuelve una ServiceResponse de todos los productos que están en el carrito
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProductsAsync(List<CartItem> cartItems);
    }
}
