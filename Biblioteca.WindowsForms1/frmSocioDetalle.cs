using Biblioteca.Core.Models;
using System;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmSocioDetalle : Form
    {
        private TextBox txtCodigo, txtNombre, txtApellido, txtEmail, txtTelefono, txtDireccion;
        private Button btnGuardar, btnCancelar;

        public Socio Socio { get; private set; }
        private bool _esNuevo;

        public frmSocioDetalle() : this(null) { }

        public frmSocioDetalle(Socio socio)
        {
            Socio = socio ?? new Socio();
            _esNuevo = socio == null;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            this.Text = _esNuevo ? "Nuevo Socio" : "Editar Socio";
            this.Size = new System.Drawing.Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            CrearControles();
            if (!_esNuevo)
                CargarDatosExistente();
            else
                txtCodigo.Text = GenerarCodigoSocio();
        }

        private void CrearControles()
        {
            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 200;

            // Código Socio
            var lblCodigo = new Label { Text = "Código Socio:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtCodigo = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth, ReadOnly = true };
            this.Controls.AddRange(new Control[] { lblCodigo, txtCodigo });
            yPos += 35;

            // Nombre
            var lblNombre = new Label { Text = "Nombre:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtNombre = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblNombre, txtNombre });
            yPos += 35;

            // Apellido
            var lblApellido = new Label { Text = "Apellido:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtApellido = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblApellido, txtApellido });
            yPos += 35;

            // Email
            var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtEmail = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblEmail, txtEmail });
            yPos += 35;

            // Teléfono
            var lblTelefono = new Label { Text = "Teléfono:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtTelefono = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblTelefono, txtTelefono });
            yPos += 35;

            // Dirección
            var lblDireccion = new Label { Text = "Dirección:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtDireccion = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblDireccion, txtDireccion });
            yPos += 45;

            // Botones
            btnGuardar = new Button { Text = "Guardar", Location = new System.Drawing.Point(140, yPos), Width = 80 };
            btnCancelar = new Button { Text = "Cancelar", Location = new System.Drawing.Point(230, yPos), Width = 80 };

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });
        }

        private void CargarDatosExistente()
        {
            txtCodigo.Text = Socio.CodigoSocio;
            txtNombre.Text = Socio.Nombre;
            txtApellido.Text = Socio.Apellido;
            txtEmail.Text = Socio.Email;
            txtTelefono.Text = Socio.Telefono;
            txtDireccion.Text = Socio.Direccion;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                GuardarDatos();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void GuardarDatos()
        {
            Socio.CodigoSocio = txtCodigo.Text.Trim();
            Socio.Nombre = txtNombre.Text.Trim();
            Socio.Apellido = txtApellido.Text.Trim();
            Socio.Email = txtEmail.Text.Trim();
            Socio.Telefono = txtTelefono.Text.Trim();
            Socio.Direccion = txtDireccion.Text.Trim();

            if (_esNuevo)
            {
                Socio.Activo = true;
                Socio.FechaRegistro = DateTime.Now;
            }
        }

        private string GenerarCodigoSocio()
        {
            return $"SOC-{DateTime.Now:yyyyMMdd-HHmmss}";
        }
    }
}