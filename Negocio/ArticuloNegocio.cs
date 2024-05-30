using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Conexiones;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar() 
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Codigo, Nombre, A.Descripcion, imagenUrl, M.Descripcion, C.Descripcion, Precio from MARCAS M, ARTICULOS A, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read()) 
                {
                    Articulo aux = new Articulo();
                    aux.codigoDeArticulo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    aux.imagen = (string)datos.Lector["ImagenUrl"];
                    aux.marca = new Marca();
                    //aux.marca = (Marca)datos.Lector["Marca"];
                    aux.categoria = new Categoria();
                    //aux.categoria = (Categoria)datos.Lector["Categoria"];
                    aux.precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        

    }
}
