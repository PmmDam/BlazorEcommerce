namespace BlazorEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            //Valor de retorno.
            var result = new ServiceResponse<List<CartProductResponseDTO>>
            {
                Data = new List<CartProductResponseDTO>()
            };

            //Recorremos la lista de cartItems 
            foreach(var item in cartItems)
            {
                //Recuperamos de la base de datos el item sobre el que estamos iterando
                var product = await _context.Products.Where(p=>p.Id == item.ProductId).FirstOrDefaultAsync();

                //Si existe el item, comprbamos y sus variants
                if(product != null)
                {
                    var productVariant = await _context.ProductVariants
                        .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                        .Include(v=> v.ProductType)
                        .FirstOrDefaultAsync();


                    if (productVariant != null)
                    {
                        var cartProduct = new CartProductResponseDTO
                        {
                            ProductId = product.Id,
                            Title = product.Title,
                            ImageUrl = product.ImageUrl,
                            Price = productVariant.Price,
                            ProductType = productVariant.ProductType.Name,
                            ProductTypeId = productVariant.ProductTypeId,
                            Quantity = item.Quantity
                        };
                        result.Data.Add(cartProduct);
                    }
               
                }
                
            }
            return result;
        }
    }
}
