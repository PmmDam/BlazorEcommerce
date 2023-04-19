namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductModel>>> GetProductsAsync();
    }
}
