using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Presentacion
{
    public partial class frmAltaArticulos : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAltaArticulos()
        {
            InitializeComponent();
            Text = "Agregar artículo";
        }
        public frmAltaArticulos(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar artículo";
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.codigo = txtCodigo.Text;
                articulo.nombre = txtNombre.Text;
                articulo.descripcion = txtDescripcion.Text;
                articulo.marca = (Marca)cboMarca.SelectedItem;
                articulo.categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.imagen = txtUrlImagen.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);

                if (articulo.id != 0) 
                { 
                    negocio.Modificar(articulo);
                    MessageBox.Show("Artículo modificado exitosamente!");
                    Close();
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Artículo agregado exitosamente!");
                    Close();
                }

                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void frmAltaArticulos_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if(articulo != null) 
                {
                    txtCodigo.Text = articulo.codigo;
                    txtNombre.Text = articulo.nombre;
                    txtDescripcion.Text = articulo.descripcion;
                    txtUrlImagen.Text = articulo.imagen;
                    cargarImagen(articulo.imagen);
                    cboMarca.SelectedValue = articulo.marca.Id;
                    cboCategoria.SelectedValue = articulo.categoria.Id;
                    txtPrecio.Text = articulo.precio.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        private void cargarImagen(String imagen)
        {
            try
            {
                pbxImagenAlta.Load(imagen);
            }
            catch (Exception)
            {
                pbxImagenAlta.Load("https://img.freepik.com/premium-vector/photo-icon-picture-icon-image-sign-symbol-vector-illustration_64749-4409.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK) 
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }

        }
    }
}
