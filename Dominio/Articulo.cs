using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        /*Código de artículo.
        Nombre.
        Descripción.
        Marca(seleccionable de una lista desplegable).
        Categoría(seleccionable de una lista desplegable.
        Imagen.
        Precio.*/


        public string codigoDeArticulo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
        public string imagen { get; set; }
        public decimal precio { get; set;}

    }
}
