using Microsoft.IdentityModel.Tokens;

namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve una lista con todos los productos y sus variants
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Include(p=>p.Variants).ToListAsync()
            };
            return response;
        }

        /// <summary>
        /// Devuelve un ServiceResponse con un producto en función del ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p=>p.Id == productId);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }
        /// <summary>
        /// Devuelve un ServiceResponse con una lista de productos en función de su categoría
        /// </summary>
        /// <param name="categoryUrl"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).Include(p => p.Variants).ToListAsync()
            };
            return response;
        }

        /// <summary>
        /// Devuelve una lista de productos que contienen el texto de busqueda en el título o en la descripción
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private async Task<List<Product>> FindProductBySearchTextAsync(string searchText)
        {
            return await _context.Products
                            .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower()))
                            .Include(P => P.Variants)
                            .ToListAsync();
        }
        /// <summary>
        /// Devuelve un ServiceResponse con una lista de productos en función del parámetro de búsqueda
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText)
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await FindProductBySearchTextAsync(searchText)
            };
            return response;
        }
   
        /// <summary>
        /// Devuelve un ServiceResponse con una lista de titulos de productos como sugerencias en función del parámetro de búsqueda.
        /// No solo sugiere una lista de titulos, tambien comprueba si la descripción contiene las palabras que estás introduciendo y te lo sugiere también.
        /// Es una lógica algo compleja y estaría bien refactorizarlo para dejarlo algo más limpio, pero bueno, de momento funciona
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products =  await FindProductBySearchTextAsync(searchText);
            
            List<string> result = new List<string>();
            
            foreach (var product in products)
            {
                //Comprobamps si el titulo del producto contiene el searchText para añadirlo a la lista de sugerencias
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }
                //Comprobamos si la descripción contiene las palabras que vamos introduciendo para añadirlo como sugerencia
                if (!product.Description.IsNullOrEmpty())
                {
                    var punctutation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();

                    var words = product.Description.Split().Select(s => s.Trim(punctutation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }
            
            return new ServiceResponse<List<string>> { Data = result};
        }
    }
}
