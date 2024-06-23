using Dominio;
using Negocio;
using System;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmVerDetalles : Form
    {
        Articulo articulo;
        ArticuloNegocio negocio = new ArticuloNegocio();
        public frmVerDetalles(Articulo seleccionado)
        {
            InitializeComponent();
            this.articulo = seleccionado;
        }

        private void frmVerDetalles_Load(object sender, EventArgs e)
        {
            try
            {
                lblCodigoInformacion.Text = articulo.codigo;
                lblNombreINformacion.Text = articulo.nombre;
                lblDescripcionInformacion.Text = articulo.descripcion;
                lblPrecioInformacion.Text = articulo.precio.ToString();
                lblMarcaInformacion.Text = articulo.marca.Descripcion;
                lblCategoriaInformacion.Text = articulo.categoria.Descripcion;
                cargarImagen(articulo.imagen);
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagenDetalles.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxImagenDetalles.Load("https://img.freepik.com/premium-vector/photo-icon-picture-icon-image-sign-symbol-vector-illustration_64749-4409.jpg");
            }
        }
    }
}
