using Dominio;
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


namespace Presentacion
{
    public partial class FrmNuevoProducto : Form
    {
        private Catalogo catalogo = null;
        public FrmNuevoProducto()
        {
            InitializeComponent();
        }
        public FrmNuevoProducto(Catalogo catalogo)
        {
            InitializeComponent();
            this.catalogo = catalogo;
            Text = "Modicar Catalogo";
        }
        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            
            try
            {
                if (catalogo == null)
                    catalogo = new Catalogo();

                catalogo.Codigo = textBoxCodigo.Text;
                catalogo.Nombre = textBoxNombre.Text;
                catalogo.Descripcion = textBoxDescripcion.Text;
                catalogo.Marca = (Marcas)comboBoxMarca.SelectedItem;
                catalogo.Categoria = (Categorias)comboBoxCategoria.SelectedItem;
                catalogo.ImagenUrl = textBoxImagenUrl.Text;
                catalogo.Precio = decimal.Parse(textBoxPrecio.Text);

                if(catalogo.Id != 0)
                {
                    negocio.modificar(catalogo);
                    MessageBox.Show("Su producto se modifico exitosamente");
                }
                else
                {
                    negocio.agregar(catalogo);
                    MessageBox.Show("Su producto nuevo ya esta cargado");
                }
               
               
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Revisar campos vacios..","ATENCION",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
            
                
            
                

        }


        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmNuevoProducto_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                comboBoxMarca.DataSource = marcaNegocio.listar();
                comboBoxMarca.ValueMember = "Id";
                comboBoxMarca.DisplayMember = "Descripcion";
                comboBoxCategoria.DataSource = categoriaNegocio.listar();
                comboBoxCategoria.ValueMember = "Id";
                comboBoxCategoria.DisplayMember = "Descripcion";

                if(catalogo != null)
                {
                    textBoxCodigo.Text = catalogo.Codigo;
                    textBoxNombre.Text = catalogo.Nombre;
                    textBoxDescripcion.Text = catalogo.Descripcion;
                    comboBoxMarca.SelectedValue = catalogo.Marca.Id;
                    comboBoxCategoria.SelectedValue = catalogo.Categoria.Id;
                    textBoxImagenUrl.Text = catalogo.ImagenUrl;
                    cargarImagenNueva(catalogo.ImagenUrl);
                    textBoxPrecio.Text = catalogo.Precio.ToString();

                }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void textBoxImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagenNueva(textBoxImagenUrl.Text);

        }
        private void cargarImagenNueva(string imagen)
        {
            try
            {
                pictureBoxNuevoProducto.Load(imagen);
            }
            catch (Exception ex)
            {

                pictureBoxNuevoProducto.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }

        }

        ErrorProvider error = new ErrorProvider();

        private void textBoxCodigo_Leave(object sender, EventArgs e)
        {
            if (ValidacionTextBox.textVacios(textBoxCodigo))
            {
                error.SetError(textBoxCodigo, "Este campo no puede estar vacio");
            }
            else
                error.Clear();
        }

        private void textBoxNombre_Leave(object sender, EventArgs e)
        {
            if (ValidacionTextBox.textVacios(textBoxNombre))
            {
                error.SetError(textBoxNombre, "Este campo no puede estar vacio");
            }
            else
                error.Clear();
        }

        private void textBoxPrecio_Leave(object sender, EventArgs e)
        {
            if (ValidacionTextBox.textVacios(textBoxPrecio))
            {
                error.SetError(textBoxPrecio, "Este campo no puede estar vacio");
            }else
                error.Clear();
        }
    }
}
