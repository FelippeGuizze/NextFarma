using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;

namespace NextFarma.Controllers
{
    public class MedicamentosController : Controller
    {
        private readonly AppDbContext _context;

        public MedicamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            // ISSO É CRUCIAL: Carrega a lista de categorias para o Dropdown
            ViewData["CategoriaId"] = new SelectList(_context.CategoriasMedicamento, "Id", "Descricao");
            return View();
        }

        // Em Controllers/MedicamentosController.cs

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Concentracao,CategoriaId")] Medicamento medicamento)
        {
            // CORREÇÃO: Removemos a validação do objeto Categoria, pois só temos o ID
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                return RedirectToAction("Medicines", "Admin");
            }

            ViewData["CategoriaId"] = new SelectList(_context.CategoriasMedicamento, "Id", "Descricao", medicamento.CategoriaId);
            return View(medicamento);
        }

        // GET: Medicamentos (Listagem)
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Medicamentos.Include(m => m.Categoria);
            return View(await appDbContext.ToListAsync());
        }
    }
}