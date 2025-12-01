using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ConfigureMenu();
        }

        private void ConfigureMenu()
        {
            // Configurar menú principal
            Text = "Sistema de Gestión de Biblioteca";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Cargar controles del menú
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            MenuStrip menuStrip = new MenuStrip();

            // Menú Libros
            ToolStripMenuItem menuLibros = new ToolStripMenuItem("&Libros");
            menuLibros.Click += (s, e) => ShowForm<frmLibros>();
            menuStrip.Items.Add(menuLibros);

            // Menú Socios
            ToolStripMenuItem menuSocios = new ToolStripMenuItem("&Socios");
            menuSocios.Click += (s, e) => ShowForm<frmSocios>();
            menuStrip.Items.Add(menuSocios);

            // Menú Préstamos
            ToolStripMenuItem menuPrestamos = new ToolStripMenuItem("&Préstamos");
            menuPrestamos.Click += (s, e) => ShowForm<frmPrestamos>();
            menuStrip.Items.Add(menuPrestamos);

            // Menú Salir
            ToolStripMenuItem menuSalir = new ToolStripMenuItem("&Salir");
            menuSalir.Click += (s, e) => Application.Exit();
            menuStrip.Items.Add(menuSalir);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void ShowForm<T>() where T : Form
        {
            var form = Program.ServiceProvider.GetService<T>();
            form.MdiParent = this;
            form.Show();
        }
    }
}