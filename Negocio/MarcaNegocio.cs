﻿using Conexiones;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
	public class MarcaNegocio
	{
		public List<Marca> listar()
		{
			AccesoDatos datos = new AccesoDatos();
			List<Marca> lista = new List<Marca>();

			try
			{
				datos.setearConsulta("SELECT Id, Descripcion from MARCAS");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{
					Marca aux = new Marca();

					aux.Id = (int)datos.Lector["Id"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];

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
