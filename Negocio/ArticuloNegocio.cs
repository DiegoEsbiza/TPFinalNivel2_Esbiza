﻿using Conexiones;
using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;

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
                datos.setearConsulta("SELECT A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, imagenUrl, M.Descripcion Marca, C.Descripcion Categoria, Precio, M.Id, C.Id from MARCAS M, ARTICULOS A, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.imagen = (string)datos.Lector["ImagenUrl"];

                    aux.marca = new Marca();
                    aux.marca.Id = (int)datos.Lector["IdMarca"];
                    aux.marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];
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
        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, imagenUrl, Precio) values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                datos.setearParametro("@codigo", nuevo.codigo);
                datos.setearParametro("@nombre", nuevo.nombre);
                datos.setearParametro("@descripcion", nuevo.descripcion);
                datos.setearParametro("@idMarca", nuevo.marca.Id);
                datos.setearParametro("@idCategoria", nuevo.categoria.Id);
                datos.setearParametro("@imagenUrl", nuevo.imagen);
                datos.setearParametro("@precio", nuevo.precio);
                datos.ejecutarLectura();
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
        public void Modificar(Articulo modificar)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, imagenUrl = @imagenUrl, Precio = @precio Where Id = @Id ");
                datos.setearParametro("@codigo", modificar.codigo);
                datos.setearParametro("@nombre", modificar.nombre);
                datos.setearParametro("@descripcion", modificar.descripcion);
                datos.setearParametro("@idMarca", modificar.marca.Id);
                datos.setearParametro("@idCategoria", modificar.categoria.Id);
                datos.setearParametro("@imagenUrl", modificar.imagen);
                datos.setearParametro("@precio", modificar.precio);
                datos.setearParametro("@Id", modificar.id);

                datos.ejecutarAccion();
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
        public void Elimnar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE from ARTICULOS where Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, imagenUrl, M.Descripcion Marca, C.Descripcion Categoria, Precio, M.Id, C.Id from MARCAS M, ARTICULOS A, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria and ";
                switch (campo)
                {
                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Nombre like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "Nombre like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "Nombre like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    case "Marca":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "M.Descripcion like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "M.Descripcion like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "M.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    case "Categoria":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "C.Descripcion like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "C.Descripcion like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "C.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    default:
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Precio > " + filtro;
                                break;

                            case "Menor a":
                                consulta += "Precio < " + filtro;
                                break;

                            default:
                                consulta += "Precio = " + filtro;
                                break;
                        }
                        break;
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.imagen = (string)datos.Lector["ImagenUrl"];

                    aux.marca = new Marca();
                    aux.marca.Id = (int)datos.Lector["IdMarca"];
                    aux.marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
