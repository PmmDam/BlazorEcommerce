using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
    public class ProductModel
    {
        //Propiedades
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        //Aunque parezca redundante, facilita mucho la vida a la hora de trabajar con EntityFramework
        //tener el objeto nullable y la clave foranea del mismo. Explicaré los casos de usos en la memoria
        public CategoryModel? Category { get; set; }
        public int CategoryId { get; set; }
    }
}
