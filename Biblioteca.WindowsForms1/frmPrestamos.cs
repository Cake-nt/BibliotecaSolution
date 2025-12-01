using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmPrestamos : Form
    {
        private DataGridView dgvPrestamos;
        private ComboBox cmbFiltroEstado;
        private Button btnNuevo, btnDevolver, btnCerrar;
        private Label lblFiltro;

        private readonly PrestamoService _prestamoService;
        private readonly LibroService _libroService;
        private readonly SocioService _socioService;
        private List<Prestamo> _prestamos;

        public frmPrestamos(PrestamoService prestamoService, LibroService libroService, SocioService socioService)
        {
            _prestamoService = prestamoService;
            _libroService = libroService;
            _socioService = socioService;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Préstamos";
            this.Size = new System.Drawing.Size(900, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            CrearControles();
            _ = CargarPrestamosAsync();
        }

        private void CrearControles()
        {
            // Panel superior
            var pnlSuperior = new Panel { Dock = DockStyle.Top, Height = 50 };

            lblFiltro = new Label { Text = "Filtrar por:", Location = new System.Drawing.Point(20, 15), Width = 60 };
            cmbFiltroEstado = new ComboBox { Location = new System.Drawing.Point(90, 12), Width = 120 };
            cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Activos", "Devueltos", "Vencidos" });
            cmbFiltroEstado.SelectedIndex = 0;
            cmbFiltroEstado.SelectedIndexChanged += cmbFiltroEstado_SelectedIndexChanged;

            btnNuevo = new Button { Text = "Nuevo Préstamo", Location = new System.Drawing.Point(220, 12), Width = 120 };

            btnNuevo.Click += btnNuevoPrestamo_Click;

            pnlSuperior.Controls.AddRange(new Control[] { lblFiltro, cmbFiltroEstado, btnNuevo });

            // DataGridView
            dgvPrestamos = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            // Panel inferior
            var pnlInferior = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            btnDevolver = new Button { Text = "Devolver", Location = new System.Drawing.Point(20, 12), Width = 80 };
            btnCerrar = new Button { Text = "Cerrar", Location = new System.Drawing.Point(110, 12), Width = 80 };

            btnDevolver.Click += btnDevolver_Click;
            btnCerrar.Click += (s, e) => this.Close();

            pnlInferior.Controls.AddRange(new Control[] { btnDevolver, btnCerrar });

            this.Controls.AddRange(new Control[] { pnlSuperior, dgvPrestamos, pnlInferior });
        }

        private async Task CargarPrestamosAsync()
        {
            try
            {
                _prestamos = (await _prestamoService.GetAllPrestamosAsync()) as List<Prestamo>;
                dgvPrestamos.DataSource = _prestamos;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar préstamos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvPrestamos.AutoGenerateColumns = false;
            dgvPrestamos.Columns.Clear();

            dgvPrestamos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CodigoPrestamo", HeaderText = "Código", Width = 120 });
            dgvPrestamos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Libro.Titulo", HeaderText = "Libro", Width = 200 });
            dgvPrestamos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Socio.NombreCompleto", HeaderText = "Socio", Width = 150 });
            dgvPrestamos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaPrestamo", HeaderText = "Fecha Préstamo", Width = 120 });
            dgvPrestamos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 100 });
        }

        private async void btnNuevoPrestamo_Click(object sender, EventArgs e)
        {
            using (var frm = new frmPrestamoDetalle(_libroService, _socioService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _prestamoService.CreatePrestamoAsync(frm.Prestamo);
                        await CargarPrestamosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear préstamo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.CurrentRow?.DataBoundItem is Prestamo prestamoSeleccionado && prestamoSeleccionado.Estado == "Activo")
            {
                var resultado = MessageBox.Show($"¿Devolver '{prestamoSeleccionado.CodigoPrestamo}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        await _prestamoService.DevolverPrestamoAsync(prestamoSeleccionado.Id);
                        await CargarPrestamosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al devolver: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void cmbFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cmbFiltroEstado.SelectedItem.ToString())
                {
                    case "Activos":
                        dgvPrestamos.DataSource = await _prestamoService.GetPrestamosActivosAsync();
                        break;
                    case "Vencidos":
                        dgvPrestamos.DataSource = await _prestamoService.GetPrestamosVencidosAsync();
                        break;
                    case "Devueltos":
                        dgvPrestamos.DataSource = _prestamos?.FindAll(p => p.Estado == "Devuelto");
                        break;
                    default:
                        await CargarPrestamosAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}