using Microsoft.AspNetCore.Mvc;
using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibroService _libroService;

        public LibrosController(LibroService libroService)
        {
            _libroService = libroService;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = await _libroService.GetAllLibrosAsync();
            return View(libros);
        }

        // GET: Libros/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Capturar los datos del formulario manualmente
                var titulo = Request.Form["Titulo"];
                var autor = Request.Form["Autor"];
                var isbn = Request.Form["ISBN"];
                var genero = Request.Form["Genero"];
                var anioPublicacion = int.Parse(Request.Form["AnioPublicacion"]);
                var editorial = Request.Form["Editorial"];
                var categoriaId = int.Parse(Request.Form["CategoriaId"]);
                var ejemplaresTotales = int.Parse(Request.Form["EjemplaresTotales"]);
                var ejemplaresDisponibles = int.Parse(Request.Form["EjemplaresDisponibles"]);

                var libro = new Libro
                {
                    Titulo = titulo,
                    Autor = autor,
                    ISBN = isbn,
                    Genero = genero,
                    AnioPublicacion = anioPublicacion,
                    Editorial = editorial,
                    CategoriaId = categoriaId,
                    EjemplaresTotales = ejemplaresTotales,
                    EjemplaresDisponibles = ejemplaresDisponibles,
                    Activo = true
                };

                await _libroService.CreateLibroAsync(libro);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Mostrar error en la vista
                ViewBag.Error = $"Error: {ex.Message}";
                return View();
            }
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                await _libroService.CreateLibroAsync(libro);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _libroService.UpdateLibroAsync(libro);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _libroService.DeleteLibroAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // GET: Libros/Search
        public async Task<IActionResult> Search(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                return RedirectToAction(nameof(Index));
            }

            var resultados = await _libroService.SearchLibrosAsync(criterio);
            return View("Index", resultados);
        }
    }
}