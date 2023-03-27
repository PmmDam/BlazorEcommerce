using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private static List<ProductModel> products = new List<ProductModel>
        {
            new ProductModel
            {
                Id = 1,
                Title="The Hitchhiker's Guide to the Galaxy",
                Description="The Hitchhiker's Guide to the Galaxy[note 1] (sometimes referred to as HG2G,[1] HHGTTG,[2] H2G2,[3] or tHGttG) is a comedy science fiction franchise created by Douglas Adams. Originally a 1978 radio comedy broadcast on BBC Radio 4, it was later adapted to other formats, including novels, stage shows, comic books, a 1981 TV series, a 1984 text-based computer game, and 2005 feature film.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/b/bd/H2G2_UK_front_cover.jpg",
                Price = 9.99m
            },
            new ProductModel
            {
                Id = 2,
                Title="Ready Player One",
                Description="Ready Player One es una novela de ciencia ficción escrita por el autor estadounidense Ernest Cline y cuya edición original en inglés fue publicada el 16 de agosto de 2011 por la editorial Crown Publishers (una filial de Random House).1​ La edición en español fue publicada por primera vez en 2011 por Ediciones B.2​ También se realizó una adaptación para el cine dirigida por Steven Spielberg, la cual se estrenó mundialmente el 30 de marzo de 2018. El libro fue un superventas de The New York Times.3​",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/df/Ready_Player_One_logo.png",
                Price = 19.99m
            },
            new ProductModel
            {
                Id = 3,
                Title="1984 ",
                Description="1984 (en su versión original en inglés: Nineteen Eighty-Four) es una novela política de ficción distópica, escrita por George Orwell entre 1947 y 1948 y publicada el 8 de junio de 1949. La novela popularizó los conceptos del omnipresente y vigilante Gran Hermano o Hermano Mayor, de la notoria habitación 101, de la ubicua policía del Pensamiento y de la neolengua, adaptación del idioma inglés en la que se reduce y se transforma el léxico con fines represivos, basándose en el principio de que lo que no forma parte de la lengua, no puede ser pensado.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/1984first.jpg/800px-1984first.jpg",
                Price = 29.99m
            }
        };

        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetProduct()
        {
            return Ok(products);
        }

        
    }
}
