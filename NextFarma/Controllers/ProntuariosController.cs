using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;

namespace NextFarma.Controllers
{
    public class ProntuariosController : Controller
    {
        private readonly AppDbContext _context;

        public ProntuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Prontuarios/Create
        public IActionResult Create()
        {
            // Carrega Pacientes e Medicamentos para os selects
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome");
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nome");
            return View();
        }

        // Em Controllers/ProntuariosController.cs

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CID,NumeroNotificacao,DataEmissao,TipoDeUso,PacienteId,MedicamentoId")] Prontuario prontuario)
        {
            // CORREÇÃO: Remove validação das propriedades de navegação
            ModelState.Remove("Paciente");
            ModelState.Remove("Medicamento");

            if (ModelState.IsValid)
            {
                _context.Add(prontuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Records", "Admin");
            }

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", prontuario.PacienteId);
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nome", prontuario.MedicamentoId);

            return View(prontuario);
        }

        // GET: Prontuarios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Prontuarios.Include(p => p.Medicamento).Include(p => p.Paciente);
            return View(await appDbContext.ToListAsync());
        }
    }
}