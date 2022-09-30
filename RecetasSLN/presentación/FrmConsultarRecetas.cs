using RecetasSLN.dominio;
using RecetasSLN.Servicios;
using RecetasSLN.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecetasSLN.presentación
{
    public partial class FrmConsultarRecetas : Form
    {
        private Receta nueva;
        private IServicio service;
        public FrmConsultarRecetas()
        {
            InitializeComponent();
            nueva = new Receta();
            service = new ServicioFactoryImp().crearServicio();
        }

        private void FrmConsultarRecetas_Load(object sender, EventArgs e)
        {
            ProximaReceta();
            CargarCombo();
        }

        private void CargarCombo()
        {           
            cboIngrediente.DataSource = service.ObtenerIngredientes();
            cboIngrediente.ValueMember = "IdIngrediente";
            cboIngrediente.DisplayMember = "Nombre";
            cboIngrediente.SelectedIndex = -1;
        }

        private void ProximaReceta()
        {
            int next = service.ProximaReceta();
            if (next > 0)
            {
                lblNro.Text += next.ToString();
            }
            else
            {
                MessageBox.Show("Error De datos, no se pudo obtener la próxima receta.",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Valido()
        {
            if (cboIngrediente.SelectedIndex == -1)
            {
                MessageBox.Show("Debe Seleccionar un Ingrediente");
                return false;
            }
            if (NudCantidad.Value == 0)
            {
                MessageBox.Show("Debe Seleccionar una cantidad válida");
                return false;
            }
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["ingrediente"].Value.ToString().Equals(cboIngrediente.Text))
                {
                    MessageBox.Show("Ingrediente: " + cboIngrediente.Text + " ya se encuentra como detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            return true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Valido())
            {
                DetalleReceta det = new DetalleReceta();
                det.Cantidad = (int)NudCantidad.Value;
                det.Ingrediente = (Ingrediente)cboIngrediente.SelectedItem;
                nueva.AgregarDetalle(det);
                dgvDetalles.Rows.Add(new object[] { det.Ingrediente.IdIngrediente, det.Ingrediente.Nombre, det.Cantidad });
            }
            calcularTotal();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarAceptar())
            {
                nueva.NroReceta = service.ProximaReceta();
                nueva.Nombre = txtNombre.Text;
                nueva.Chef = txtCheff.Text;
                nueva.TipoReceta = Convert.ToInt32(cboIngrediente.SelectedIndex);
                if (service.ConfirmarReceta(nueva))
                {
                    MessageBox.Show("Receta agregada");
                    limpiar();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar la receta");
                }
                calcularTotal();
            }
        }

        private void limpiar()
        {
            txtNombre.Text = "";
            txtCheff.Text = "";
            cboIngrediente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;
            NudCantidad.Value = 0;
            txtNombre.Focus();
        }

        private bool ValidarAceptar()
        {
            if (txtNombre.Text == "")
            {
                MessageBox.Show("La receta debe tener un nombre");
                return false;
            }
            if (txtCheff.Text == "")
            {
                MessageBox.Show("La receta debe tener un cheff");
                return false;
            }
            if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("La receta debe tener un tipo");
                return false;
            }
            if (dgvDetalles.Rows.Count < 3)
            {
                MessageBox.Show("Ha olvidado Ingredientes?");
                return false;
            }
            return true;
        }

        private void calcularTotal()
        {
            lblTotalIngredientes.Text = "Total Ingredientes: " + dgvDetalles.Rows.Count;
        }
    }
}
