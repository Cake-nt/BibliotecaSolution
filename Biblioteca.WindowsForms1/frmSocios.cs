using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmSocios : Form
    {
        private DataGridView dgvSocios;
        private Button btnNuevo, btnEditar, btnEliminar, btnCerrar;

        private readonly SocioService _socioService;
        private List<Socio> _socios;

        public frmSocios(SocioService socioService)
        {
            _socioService = socioService;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Socios";
            this.Size = new System.Drawing.Size(700, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            CrearControles();
            _ = CargarSociosAsync();
        }

        private void CrearControles()
        {
            // Panel superior
            var pnlSuperior = new Panel { Dock = DockStyle.Top, Height = 50 };
            btnNuevo = new Button { Text = "Nuevo Socio", Location = new System.Drawing.Point(20, 12), Width = 100 };
            btnNuevo.Click += btnNuevo_Click;
            pnlSuperior.Controls.Add(btnNuevo);

            // DataGridView
            dgvSocios = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            // Panel inferior
            var pnlInferior = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            btnEditar = new Button { Text = "Editar", Location = new System.Drawing.Point(20, 12), Width = 80 };
            btnEliminar = new Button { Text = "Eliminar", Location = new System.Drawing.Point(110, 12), Width = 80 };
            btnCerrar = new Button { Text = "Cerrar", Location = new System.Drawing.Point(200, 12), Width = 80 };

            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.Click += (s, e) => this.Close();

            pnlInferior.Controls.AddRange(new Control[] { btnEditar, btnEliminar, btnCerrar });

            this.Controls.AddRange(new Control[] { pnlSuperior, dgvSocios, pnlInferior });
        }

        private async Task CargarSociosAsync()
        {
            try
            {
                _socios = (await _socioService.GetAllSociosAsync()) as List<Socio>;
                dgvSocios.DataSource = _socios;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar socios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvSocios.AutoGenerateColumns = false;
            dgvSocios.Columns.Clear();

            dgvSocios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CodigoSocio", HeaderText = "Código", Width = 100 });
            dgvSocios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", Width = 150 });
            dgvSocios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Apellido", HeaderText = "Apellido", Width = 150 });
            dgvSocios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 200 });
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var frm = new frmSocioDetalle())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _socioService.CreateSocioAsync(frm.Socio);
                        await CargarSociosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear socio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvSocios.CurrentRow?.DataBoundItem is Socio socioSeleccionado)
            {
                using (var frm = new frmSocioDetalle(socioSeleccionado))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            await _socioService.UpdateSocioAsync(frm.Socio);
                            await CargarSociosAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvSocios.CurrentRow?.DataBoundItem is Socio socioSeleccionado)
            {
                var resultado = MessageBox.Show($"¿Eliminar a {socioSeleccionado.Nombre}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        await _socioService.DeleteSocioAsync(socioSeleccionado.Id);
                        await CargarSociosAsync();
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