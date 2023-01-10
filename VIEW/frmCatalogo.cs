using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MODEL;
using CONTROLLER;

namespace VIEW
{
    public partial class Catalogo : Form
    {
        private List<Articulo> articulos;
        public Catalogo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");

            cboOrdenar.Items.Add("Precio");
            cboOrdenar.Items.Add("Nombre");
            cboOrdenar.Items.Add("Marca");
            cboOrdenar.Items.Add("Categoría");

        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                articulos = negocio.listar();
                dgvArticulos.DataSource = articulos;
                ocultarColumnas();
                cargarImagen(articulos[0].Imagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            //dgvArticulos.Columns["Codigo"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            //dgvArticulos.Columns["Categoria"].Visible = false;
            //dgvArticulos.Columns["Marca"].Visible = false;
            dgvArticulos.Columns["Imagen"].Visible = false;
            //dgvArticulos.Columns["Precio"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxArticulo.Load("https://www.webempresa.com/foro/wp-content/uploads/wpforo/attachments/3200/318277=80538-Sin_imagen_disponible.jpg");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.Imagen);
                lblDescripcion.Text = "Descripción: " + seleccionado.Descripcion;
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
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulos modificar = new frmAltaArticulos(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void eliminar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad quieres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminarLogico(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void cbxCampo_SelectedIndexChanged_1(object sender, EventArgs e)
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                List<Articulo> seleccionActual = new List<Articulo>();
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                seleccionActual = (List<Articulo>)(dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro));

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private bool validarFiltro()
        {
            string valor = txtFiltro.Text;
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(valor))
                {
                    MessageBox.Show("Si el campo es numero el filtro no puede estar vacio...");
                    return true;
                }
                if (!(soloNumeros(valor)))
                {
                    MessageBox.Show("Solo numeros para un campo numerico");
                    return true;
                }

            }

            return false;
        }
        private bool validarOrdenar()
        {
            string valor = txtFiltro.Text;
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(valor))
                {
                    MessageBox.Show("Si el campo es numero el filtro no puede estar vacio...");
                    return true;
                }
                if (!(soloNumeros(valor)))
                {
                    MessageBox.Show("Solo numeros para un campo numerico");
                    return true;
                }

            }

            return false;
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {

                string campo = cboOrdenar.SelectedItem.ToString();
                string criterio = cboOrdenarCriterio.SelectedItem.ToString();
                dgvArticulos.DataSource = negocio.ordenar(campo, criterio);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboOrdenar.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboOrdenarCriterio.Items.Clear();
                cboOrdenarCriterio.Items.Add("Mayor a menor");
                cboOrdenarCriterio.Items.Add("Menor a mayor");
            }
            else
            {
                cboOrdenarCriterio.Items.Clear();
                cboOrdenarCriterio.Items.Add("A - Z");
                cboOrdenarCriterio.Items.Add("Z - A");
            }
        }
    }
}
