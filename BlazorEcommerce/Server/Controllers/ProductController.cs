using BlazorEcommerce.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _productService;

        public ProductController(DataContext context)
        {
            _productService = context;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductModel>>>> GetProduct()
        {
            var products = await _productService.Products.ToListAsync();
            var response = new ServiceResponse<List<ProductModel>>()
            {
                Data = products
            };
            return Ok(response);
        }

        
    }
}
