namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        /// <summary>
        /// Evento que actualizará el renderizado de los productos cuando se haga una petición
        /// </summary>
        event Action ProductsChanged;
        
        /// <summary>
        /// Lista de productos en memoria
        /// </summary>
        List<Product> Products { get; set; }

        /// <summary>
        /// Obtiene todos los productos en función a la categoria. 
        /// Por defecto la categoria es un null así que si no se le especifica, 
        /// devolverá todos los productos
        /// </summary>
        /// <param name="categoryUrl"></param>
        /// <returns></returns>
        Task GetProductsAsync(string? categoryUrl = null);
        
        /// <summary>
        /// Obtiene un producto en función del ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ServiceResponse<Product>> GetProductAsync(int productId);



    }
}
