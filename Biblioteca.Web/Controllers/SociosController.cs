using Microsoft.AspNetCore.Mvc;
using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class SociosController : Controller
    {
        private readonly SocioService _socioService;

        public SociosController(SocioService socioService)
        {
            _socioService = socioService;
        }

        public async Task<IActionResult> Index()
        {
            var socios = await _socioService.GetAllSociosAsync();
            return View(socios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var socio = await _socioService.GetSocioByIdAsync(id);
            if (socio == null)
            {
                return NotFound();
            }
            return View(socio);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (ModelState.IsValid)
            {
                await _socioService.CreateSocioAsync(socio);
                return RedirectToAction(nameof(Index));
            }
            return View(socio);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var socio = await _socioService.GetSocioByIdAsync(id);
            if (socio == null)
            {
                return NotFound();
            }
            return View(socio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Socio socio)
        {
            if (id != socio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _socioService.UpdateSocioAsync(socio);
                return RedirectToAction(nameof(Index));
            }
            return View(socio);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var socio = await _socioService.GetSocioByIdAsync(id);
            if (socio == null)
            {
                return NotFound();
            }
            return View(socio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _socioService.DeleteSocioAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}