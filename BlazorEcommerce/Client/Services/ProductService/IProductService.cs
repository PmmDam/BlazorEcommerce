namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        List<ProductModel> Products { get; set; }
        Task GetProductsAsync();



    }
}
