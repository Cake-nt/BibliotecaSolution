using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class MainForm : Form
    {
        private MenuStrip menuPrincipal;

        public MainForm()
        {
            InicializarFormulario();
            CrearMenu();
        }

        private void InicializarFormulario()
        {
            this.Text = "Sistema de Gestión de Biblioteca";
            this.Size = new System.Drawing.Size(1000, 600);
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
        }

        private void CrearMenu()
        {
            menuPrincipal = new MenuStrip();

            // Menú Libros
            var menuLibros = new ToolStripMenuItem("&Libros");
            menuLibros.Click += (s, e) => MostrarFormulario<frmLibros>();
            menuPrincipal.Items.Add(menuLibros);

            // Menú Socios
            var menuSocios = new ToolStripMenuItem("&Socios");
            menuSocios.Click += (s, e) => MostrarFormulario<frmSocios>();
            menuPrincipal.Items.Add(menuSocios);

            // Menú Préstamos
            var menuPrestamos = new ToolStripMenuItem("&Préstamos");
            menuPrestamos.Click += (s, e) => MostrarFormulario<frmPrestamos>();
            menuPrincipal.Items.Add(menuPrestamos);

            // Menú Salir
            var menuSalir = new ToolStripMenuItem("&Salir");
            menuSalir.Click += (s, e) => Application.Exit();
            menuPrincipal.Items.Add(menuSalir);

            this.MainMenuStrip = menuPrincipal;
            this.Controls.Add(menuPrincipal);
        }

        private void MostrarFormulario<T>() where T : Form
        {
            try
            {
                var form = Program.ServiceProvider.GetService<T>();
                form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}