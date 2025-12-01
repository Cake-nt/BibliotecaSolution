using Biblioteca.Core.Models;
using Biblioteca.Business.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmPrestamoDetalle : Form
    {
        private ComboBox cmbLibro, cmbSocio;
        private DateTimePicker dtpDevolucion;
        private Button btnGuardar, btnCancelar;

        private readonly LibroService _libroService;
        private readonly SocioService _socioService;
        public Prestamo Prestamo { get; private set; }

        public frmPrestamoDetalle(LibroService libroService, SocioService socioService)
        {
            _libroService = libroService;
            _socioService = socioService;
            Prestamo = new Prestamo();
            InicializarFormulario();
        }

        private async void InicializarFormulario()
        {
            this.Text = "Nuevo Préstamo";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            CrearControles();
            await CargarDatosAsync();
        }

        private void CrearControles()
        {
            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 200;

            // Libro
            var lblLibro = new Label { Text = "Libro:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            cmbLibro = new ComboBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.AddRange(new Control[] { lblLibro, cmbLibro });
            yPos += 35;

            // Socio
            var lblSocio = new Label { Text = "Socio:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            cmbSocio = new ComboBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.AddRange(new Control[] { lblSocio, cmbSocio });
            yPos += 35;

            // Fecha Devolución
            var lblDevolucion = new Label { Text = "Devolución:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            dtpDevolucion = new DateTimePicker
            {
                Location = new System.Drawing.Point(140, yPos),
                Width = controlWidth,
                Value = DateTime.Now.AddDays(15)
            };
            this.Controls.AddRange(new Control[] { lblDevolucion, dtpDevolucion });
            yPos += 45;

            // Botones
            btnGuardar = new Button { Text = "Guardar", Location = new System.Drawing.Point(140, yPos), Width = 80 };
            btnCancelar = new Button { Text = "Cancelar", Location = new System.Drawing.Point(230, yPos), Width = 80 };

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var libros = await _libroService.GetLibrosDisponiblesAsync();
                cmbLibro.DataSource = libros.ToList();
                cmbLibro.DisplayMember = "Titulo";
                cmbLibro.ValueMember = "Id";

                var socios = await _socioService.GetSociosActivosAsync();
                cmbSocio.DataSource = socios.ToList();
                cmbSocio.DisplayMember = "NombreCompleto";
                cmbSocio.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (cmbLibro.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un libro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbSocio.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un socio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dtpDevolucion.Value <= DateTime.Now)
            {
                MessageBox.Show("La fecha de devolución debe ser futura", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void GuardarDatos()
        {
            Prestamo.LibroId = (int)cmbLibro.SelectedValue;
            Prestamo.SocioId = (int)cmbSocio.SelectedValue;
            Prestamo.FechaDevolucionPrevista = dtpDevolucion.Value;
            Prestamo.CodigoPrestamo = $"PRE-{DateTime.Now:yyyyMMdd-HHmmss}";
        }
    }
}