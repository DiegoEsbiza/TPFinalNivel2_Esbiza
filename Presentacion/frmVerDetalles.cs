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
                pbxImagenDetalles.Load(articulo.imagen);
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
