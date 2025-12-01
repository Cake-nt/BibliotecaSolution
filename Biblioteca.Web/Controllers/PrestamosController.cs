using Microsoft.AspNetCore.Mvc;
using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoService _prestamoService;
        private readonly LibroService _libroService;
        private readonly SocioService _socioService;

        public PrestamosController(PrestamoService prestamoService, LibroService libroService, SocioService socioService)
        {
            _prestamoService = prestamoService;
            _libroService = libroService;
            _socioService = socioService;
        }

        public async Task<IActionResult> Index()
        {
            var prestamos = await _prestamoService.GetAllPrestamosAsync();
            return View(prestamos);
        }

        public async Task<IActionResult> Activos()
        {
            var prestamos = await _prestamoService.GetPrestamosActivosAsync();
            ViewData["Title"] = "Préstamos Activos";
            return View("Index", prestamos);
        }

        public async Task<IActionResult> Vencidos()
        {
            var prestamos = await _prestamoService.GetPrestamosVencidosAsync();
            ViewData["Title"] = "Préstamos Vencidos";
            return View("Index", prestamos);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Libros = await _libroService.GetLibrosDisponiblesAsync();
            ViewBag.Socios = await _socioService.GetSociosActivosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _prestamoService.CreatePrestamoAsync(prestamo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Libros = await _libroService.GetLibrosDisponiblesAsync();
            ViewBag.Socios = await _socioService.GetSociosActivosAsync();
            return View(prestamo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            try
            {
                await _prestamoService.DevolverPrestamoAsync(id);
                TempData["SuccessMessage"] = "Préstamo devuelto correctamente";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}