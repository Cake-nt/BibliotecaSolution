using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmLibros : Form
    {
        private DataGridView dgvLibros;
        private TextBox txtBuscar;
        private Button btnBuscar, btnNuevo, btnEditar, btnEliminar, btnCerrar;
        private Label lblBuscar;

        private readonly LibroService _libroService;
        private List<Libro> _libros;

        public frmLibros(LibroService libroService)
        {
            _libroService = libroService;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Libros";
            this.Size = new System.Drawing.Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            CrearControles();
            _ = CargarLibrosAsync();
        }

        private void CrearControles()
        {
            // Panel superior
            var pnlSuperior = new Panel { Dock = DockStyle.Top, Height = 50 };

            lblBuscar = new Label { Text = "Buscar:", Location = new System.Drawing.Point(20, 15), Width = 50 };
            txtBuscar = new TextBox { Location = new System.Drawing.Point(80, 12), Width = 200 };
            btnBuscar = new Button { Text = "Buscar", Location = new System.Drawing.Point(290, 12), Width = 80 };
            btnNuevo = new Button { Text = "Nuevo Libro", Location = new System.Drawing.Point(380, 12), Width = 100 };

            btnBuscar.Click += btnBuscar_Click;
            btnNuevo.Click += btnNuevo_Click;

            pnlSuperior.Controls.AddRange(new Control[] { lblBuscar, txtBuscar, btnBuscar, btnNuevo });

            // DataGridView
            dgvLibros = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            // Panel inferior
            var pnlInferior = new Panel { Dock = DockStyle.Bottom, Height = 50 };

            btnEditar = new Button { Text = "Editar", Location = new System.Drawing.Point(20, 12), Width = 80 };
            btnEliminar = new Button { Text = "Eliminar", Location = new System.Drawing.Point(110, 12), Width = 80 };
            btnCerrar = new Button { Text = "Cerrar", Location = new System.Drawing.Point(200, 12), Width = 80 };

            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.Click += (s, e) => this.Close();

            pnlInferior.Controls.AddRange(new Control[] { btnEditar, btnEliminar, btnCerrar });

            this.Controls.AddRange(new Control[] { pnlSuperior, dgvLibros, pnlInferior });
        }

        private async Task CargarLibrosAsync()
        {
            try
            {
                _libros = (await _libroService.GetAllLibrosAsync()) as List<Libro>;
                dgvLibros.DataSource = _libros;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar libros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvLibros.AutoGenerateColumns = false;
            dgvLibros.Columns.Clear();

            dgvLibros.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", HeaderText = "Título", Width = 200 });
            dgvLibros.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Autor", HeaderText = "Autor", Width = 150 });
            dgvLibros.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ISBN", HeaderText = "ISBN", Width = 100 });
            dgvLibros.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EjemplaresDisponibles", HeaderText = "Disponibles", Width = 80 });
            dgvLibros.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EjemplaresTotales", HeaderText = "Totales", Width = 80 });
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                try
                {
                    var resultados = await _libroService.SearchLibrosAsync(txtBuscar.Text);
                    dgvLibros.DataSource = resultados as List<Libro>;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                await CargarLibrosAsync();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var frm = new frmLibroDetalle())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _ = CargarLibrosAsync();
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.CurrentRow?.DataBoundItem is Libro libroSeleccionado)
            {
                using (var frm = new frmLibroDetalle(libroSeleccionado))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        await CargarLibrosAsync();
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un libro para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.CurrentRow?.DataBoundItem is Libro libroSeleccionado)
            {
                var resultado = MessageBox.Show($"¿Está seguro de eliminar '{libroSeleccionado.Titulo}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        await _libroService.DeleteLibroAsync(libroSeleccionado.Id);
                        await CargarLibrosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}