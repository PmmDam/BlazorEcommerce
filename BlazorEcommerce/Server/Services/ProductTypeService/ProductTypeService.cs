namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync()
        {
            //Obtenemos todos los tipos de producto de la base de datos
            var productTypes = await _context.ProductTypes.ToListAsync();

            //Devolvemos un ServiceResponse con los productos
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }
    }
}
