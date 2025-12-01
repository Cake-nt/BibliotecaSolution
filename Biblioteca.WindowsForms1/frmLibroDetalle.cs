using Biblioteca.Core.Models;
using Biblioteca.Business.Services;
using System;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    public partial class frmLibroDetalle : Form
    {
        private TextBox txtTitulo, txtAutor, txtISBN, txtGenero, txtEditorial;
        private NumericUpDown numAnio, numTotal, numDisponible;
        private ComboBox cmbCategoria;
        private Button btnGuardar, btnCancelar;

        public Libro Libro { get; private set; }
        private bool _esNuevo;

        public frmLibroDetalle() : this(null) { }

        public frmLibroDetalle(Libro libro)
        {
            Libro = libro ?? new Libro();
            _esNuevo = libro == null;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Configurar formulario
            this.Text = _esNuevo ? "Nuevo Libro" : "Editar Libro";
            this.Size = new System.Drawing.Size(400, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CrearControles();
            if (!_esNuevo)
                CargarDatosExistente();
        }

        private void CrearControles()
        {
            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 200;

            // Título
            var lblTitulo = new Label { Text = "Título:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtTitulo = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblTitulo, txtTitulo });
            yPos += 35;

            // Autor
            var lblAutor = new Label { Text = "Autor:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtAutor = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblAutor, txtAutor });
            yPos += 35;

            // ISBN
            var lblISBN = new Label { Text = "ISBN:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtISBN = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblISBN, txtISBN });
            yPos += 35;

            // Género
            var lblGenero = new Label { Text = "Género:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtGenero = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblGenero, txtGenero });
            yPos += 35;

            // Año Publicación
            var lblAnio = new Label { Text = "Año Publicación:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            numAnio = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, yPos),
                Width = controlWidth,
                Minimum = 1000,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year
            };
            this.Controls.AddRange(new Control[] { lblAnio, numAnio });
            yPos += 35;

            // Editorial
            var lblEditorial = new Label { Text = "Editorial:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            txtEditorial = new TextBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            this.Controls.AddRange(new Control[] { lblEditorial, txtEditorial });
            yPos += 35;

            // Ejemplares Totales
            var lblTotal = new Label { Text = "Ejemplares Totales:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            numTotal = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, yPos),
                Width = controlWidth,
                Minimum = 1,
                Value = 1
            };
            this.Controls.AddRange(new Control[] { lblTotal, numTotal });
            yPos += 35;

            // Ejemplares Disponibles
            var lblDisponible = new Label { Text = "Ejemplares Disponibles:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            numDisponible = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, yPos),
                Width = controlWidth,
                Minimum = 0,
                Value = 1
            };
            this.Controls.AddRange(new Control[] { lblDisponible, numDisponible });
            yPos += 35;

            // Categoría
            var lblCategoria = new Label { Text = "Categoría:", Location = new System.Drawing.Point(20, yPos), Width = labelWidth };
            cmbCategoria = new ComboBox { Location = new System.Drawing.Point(140, yPos), Width = controlWidth };
            cmbCategoria.Items.AddRange(new object[] { "Ficción", "No Ficción", "Científico", "Educativo", "Biografía" });
            cmbCategoria.SelectedIndex = 0;
            this.Controls.AddRange(new Control[] { lblCategoria, cmbCategoria });
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
            txtTitulo.Text = Libro.Titulo;
            txtAutor.Text = Libro.Autor;
            txtISBN.Text = Libro.ISBN;
            txtGenero.Text = Libro.Genero;
            numAnio.Value = Libro.AnioPublicacion;
            txtEditorial.Text = Libro.Editorial;
            numTotal.Value = Libro.EjemplaresTotales;
            numDisponible.Value = Libro.EjemplaresDisponibles;

            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = Math.Max(0, Libro.CategoriaId - 1);
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
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("El autor es obligatorio", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (numDisponible.Value > numTotal.Value)
            {
                MessageBox.Show("Los ejemplares disponibles no pueden ser mayores a los totales",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void GuardarDatos()
        {
            Libro.Titulo = txtTitulo.Text.Trim();
            Libro.Autor = txtAutor.Text.Trim();
            Libro.ISBN = txtISBN.Text.Trim();
            Libro.Genero = txtGenero.Text.Trim();
            Libro.AnioPublicacion = (int)numAnio.Value;
            Libro.Editorial = txtEditorial.Text.Trim();
            Libro.EjemplaresTotales = (int)numTotal.Value;
            Libro.EjemplaresDisponibles = (int)numDisponible.Value;
            Libro.CategoriaId = cmbCategoria.SelectedIndex + 1;

            if (_esNuevo)
            {
                Libro.Activo = true;
            }
        }
    }
}