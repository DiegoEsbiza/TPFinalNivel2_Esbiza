using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;

namespace Presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulo> listaArticulo;
        public frmArticulos()
        {
            InitializeComponent();            
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");           
        }

        private void cargar() 
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;               
                ocultarColumnas();
                cargarImagen(listaArticulo[0].imagen);
                gboBusquedaAvanzada.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas() 
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Imagen"].Visible = false;
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null) 
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.imagen); 
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxImagen.Load("https://img.freepik.com/premium-vector/photo-icon-picture-icon-image-sign-symbol-vector-illustration_64749-4409.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulos alta = new frmAltaArticulos();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {            
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAltaArticulos modificar = new frmAltaArticulos(seleccionado);
                modificar.ShowDialog();
                cargar();  
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado = new Articulo();

            if(dgvArticulos.CurrentRow != null)
            {
                try
                {
                    DialogResult respuesta = MessageBox.Show("¿Esta seguro de que quiere eliminar este artículo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if(respuesta == DialogResult.Yes) 
                    {
                        seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        negocio.Elimnar(seleccionado.id);
                        MessageBox.Show("Articulo eliminado exitosamente!");
                        cargar();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private bool validarFiltro() 
        {
            if(cboCampo.SelectedIndex < 0) 
            {
                MessageBox.Show("Por favor, seleccione los campos para filtrar.");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)  
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio") 
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text)) 
                {
                    MessageBox.Show("Complete el campo para continuar");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese solo números, por favor");
                    return true;
                }
            }

            return false;
        }

        private bool soloNumeros(string cadena) 
        {
            foreach(char caracter in cadena) 
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }       
            return true;    
        }        

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }            
        }        

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));                
            }
            else
            {                
                listaFiltrada = listaArticulo;                
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            
            habilitarBotones();
            ocultarColumnas();
        }

        private void habilitarBotones()
        {
            if (dgvArticulos.CurrentRow == null)
            {
                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;
            }
            else
            {
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Precio") 
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void txtFiltro_MouseClick(object sender, EventArgs e)
        {
            txtFiltro.Text = "";            
        }

        private void lnkBusquedaAvanzada_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {           
            if (gboBusquedaAvanzada.Visible == false)
                gboBusquedaAvanzada.Visible = true;
            else
                gboBusquedaAvanzada.Visible = false;
        }

        private void txtFiltroAvanzado_MouseClick(object sender, MouseEventArgs e)
        {
            txtFiltroAvanzado.Text = "";
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmVerDetalles verDetalles = new frmVerDetalles(seleccionado);
                verDetalles.ShowDialog();
                cargar();
            }
        }
    }
}
