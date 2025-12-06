using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NextFarma.Controllers
{
    public class MedicamentosController : Controller
    {
        private readonly AppDbContext _context;

        public MedicamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Medicamentos
        public async Task<IActionResult> Index()
        {
            // Inclui a categoria para aparecer o nome dela na lista
            var appDbContext = _context.Medicamentos.Include(m => m.Categoria);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            // CORREÇÃO: Usando _context.CategoriasMedicamento
            ViewBag.Categorias = _context.CategoriasMedicamento.OrderBy(c => c.Descricao).ToList();
            return View();
        }

        // POST: Medicamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicamento medicamento)
        {
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento criado com sucesso.";
                return RedirectToAction("Medicines", "Admin");
            }

            // CORREÇÃO: Usando _context.CategoriasMedicamento
            ViewBag.Categorias = _context.CategoriasMedicamento.OrderBy(c => c.Descricao).ToList();
            return View(medicamento);
        }

        // ----------------------------------------------------
        // MÉTODOS DE EDIÇÃO (EDIT)
        // ----------------------------------------------------

        // GET: Medicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
            {
                return NotFound();
            }

            // CORREÇÃO: Usando _context.CategoriasMedicamento para carregar o dropdown
            ViewBag.Categorias = await _context.CategoriasMedicamento.OrderBy(c => c.Descricao).ToListAsync();

            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicamento medicamento)
        {
            if (id != medicamento.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Medicamento atualizado com sucesso.";
                return RedirectToAction("Medicines", "Admin");
            }

            // CORREÇÃO: Usando _context.CategoriasMedicamento
            ViewBag.Categorias = await _context.CategoriasMedicamento.OrderBy(c => c.Descricao).ToListAsync();
            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                _context.Medicamentos.Remove(medicamento);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento excluído com sucesso.";
            }
            return RedirectToAction("Medicines", "Admin");
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }
    }
}