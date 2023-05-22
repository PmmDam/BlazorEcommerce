using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        //Nos permite recuperar el userId
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
           var count = (await _context.CartItems.Where(ci=> ci.UserId == GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count};
        }

        public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetDbCartProducts()
        {
            return await GetCartProductsAsync(await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId= GetUserId();
            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(ci=>ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);
            if(sameItem == null) { 
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> {  Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == GetUserId());

            if(dbCartItem is null) 
            {
                return new ServiceResponse<bool> { Data=false,Success = false,Message="El producto de la cesta no existe"};
            }
            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true};
        }
    }
}
