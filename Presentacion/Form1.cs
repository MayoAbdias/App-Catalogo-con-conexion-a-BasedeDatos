using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Presentacion
{
    public partial class FormCatalogo : Form
    {
        private List<Catalogo> listaCatalogo;
        public FormCatalogo()
        {
            InitializeComponent();
        }

        private void FormCatalogo_Load(object sender, EventArgs e)
        {
            cargar();
            comboBoxCampo.Items.Add("Codigo");
            comboBoxCampo.Items.Add("Marca");
            comboBoxCampo.Items.Add("Categoria");


        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            Catalogo seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.ImagenUrl);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBoxCatalogo.Load(imagen);
            }
            catch (Exception ex)
            {

                pictureBoxCatalogo.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }

        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            FrmNuevoProducto nuevo = new FrmNuevoProducto();
            nuevo.ShowDialog();
            cargar();
        }
        private void cargar()
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            try
            {
                listaCatalogo = negocio.listar();
                dgvCatalogo.DataSource = listaCatalogo;
                dgvCatalogo.Columns["ImagenUrl"].Visible = false;
                dgvCatalogo.Columns["Id"].Visible = false;
                dgvCatalogo.Columns["Codigo"].Visible = false;

                dgvCatalogo.Columns["Precio"].DefaultCellStyle.Format = "0.00";

                cargarImagen(listaCatalogo[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCatalogo.CurrentRow != null)
                {
                    Catalogo seleccionado;
                    seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;

                    FrmNuevoProducto modificar = new FrmNuevoProducto(seleccionado);
                    modificar.ShowDialog();
                    cargar();
                }
                else
                {
                    MessageBox.Show("Debes seleccionar un articulo para poder modificarlo.", "ATENCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            Catalogo seleccionado;
            try
            {
                if (dgvCatalogo.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("¿Seguro queres eliminar este articulo?", "ELIMINANDO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                        cargar();
                    }
                }
                else
                {
                    MessageBox.Show("Debes seleccionar un articulo para poder eliminarlo..", "ATENCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBoxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = comboBoxCampo.SelectedItem.ToString();
            if (opcion == "Codigo" || opcion == "Marca" || opcion == "Categoria")
            {
                comboBoxCriterio.Items.Clear();
                comboBoxCriterio.Items.Add("Comienza con");
                comboBoxCriterio.Items.Add("Termina con");
                comboBoxCriterio.Items.Add("Contiene");
            }
           
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = comboBoxCampo.SelectedItem.ToString();
                string criterio = comboBoxCriterio.SelectedItem.ToString(); 
                string filtro = textBoxFiltro.Text;
                dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool validarFiltro()
        {
            if (comboBoxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un campo..");
                return true;
            }
            if (comboBoxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un criterio..");
                return true;
            }
            return false;
        }


    }
}
