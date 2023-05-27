using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        //Flags
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;

        //Estas flags no son necesarias en la base de datos, son solo para un formulario
        //y es más fácil tenerlo aquí centralizado
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
