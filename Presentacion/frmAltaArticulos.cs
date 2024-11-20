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
            lblEncabezado.Text = "Agregar artículo";            
            lblCodigoRequerido.Visible = false;
            lblNombreRequerido.Visible = false;
            lblPrecioRequerido.Visible = false;
            pbxImagenAlta.Load("https://img.freepik.com/premium-vector/photo-icon-picture-icon-image-sign-symbol-vector-illustration_64749-4409.jpg");
        }
        public frmAltaArticulos(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar artículo";
            lblEncabezado.Text = "Modificar artículo";
            lblSubtitulo.Text = "Editar información";
            lblCodigoRequerido.Visible = false;
            lblNombreRequerido.Visible = false;
            lblPrecioRequerido.Visible = false;

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
                if(txtCodigo.Text == "" || txtNombre.Text == "" || txtPrecio.Text == "")
                {
                    if (txtCodigo.Text != "")
                        lblCodigoRequerido.Visible = false;
                    else
                        lblCodigoRequerido.Visible = true;

                    if (txtNombre.Text != "")
                        lblNombreRequerido.Visible = false;
                    else
                        lblNombreRequerido.Visible = true;

                    if (txtPrecio.Text != "")
                        lblPrecioRequerido.Visible = false;
                    else
                        lblPrecioRequerido.Visible = true;
                }
                else
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
                        DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea modificar la información?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                            negocio.Modificar(articulo);

                        MessageBox.Show("¡Artículo modificado exitosamente!");
                        Close();
                    }
                    else
                    {
                        negocio.Agregar(articulo);
                        MessageBox.Show("¡Artículo agregado exitosamente!");

                        DialogResult respuesta = MessageBox.Show("¿Desea agregar un nuevo artículo?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            MarcaNegocio marcaNegocio = new MarcaNegocio();
                            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

                            txtCodigo.Text = "";
                            txtNombre.Text = "";
                            txtDescripcion.Text = "";
                            txtPrecio.Text = "";
                            cboMarca.DataSource = marcaNegocio.listar();
                            cboCategoria.DataSource = categoriaNegocio.listar();
                            txtUrlImagen.Text = "";
                            pbxImagenAlta.Load("https://img.freepik.com/premium-vector/photo-icon-picture-icon-image-sign-symbol-vector-illustration_64749-4409.jpg");
                            lblCodigoRequerido.Visible = false;
                            lblNombreRequerido.Visible = false;
                            lblPrecioRequerido.Visible = false;
                        }
                        else
                        {
                            Close();
                        }
                    }
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
        private void cargarImagen(string imagen)
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

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
                lblCodigoRequerido.Visible = false;
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
                lblNombreRequerido.Visible = false;
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            if (txtPrecio.Text != "")
                lblPrecioRequerido.Visible = false;
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                lblPrecioRequerido.Text = "Ingrese solo números, por favor";
                lblPrecioRequerido.Visible = true;
            }
            else
            {
                lblPrecioRequerido.Visible = false;
            }
        }
    }
}
