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

        /// <summary>
        /// Recuperamos de la base de datos el total de items en la cesta de la compra para un usuario
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<int>> GetCartItemsCount();

        /// <summary>
        /// Obtiene los productos de la cesta de la base de datos
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<CartProductResponseDTO>>> GetDbCartProducts();

        /// <summary>
        /// Añade los productos de la cesta a la base de datos
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        
        /// <summary>
        /// Actualiza la cantidad del item
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);

        /// <summary>
        ///  Borra un item de la cesta
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        Task<ServiceResponse<bool>> RemoveItemFromCart(int productId,int productTypeId);
    }
}
