using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NextFarma.Controllers
{
    public class ProntuariosController : Controller
    {
        private readonly AppDbContext _context;

        public ProntuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Prontuarios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Prontuarios.Include(p => p.Medicamento).Include(p => p.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Prontuarios/Create
        public IActionResult Create()
        {
            // Carrega as listas para o Dropdown
            ViewBag.Pacientes = _context.Pacientes.OrderBy(p => p.Nome).ToList();
            ViewBag.Medicamentos = _context.Medicamentos.OrderBy(m => m.Nome).ToList();
            return View();
        }

        // POST: Prontuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CID,NumeroNotificacao,DataEmissao,TipoDeUso,PacienteId,MedicamentoId")] Prontuario prontuario)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Medicamento");

            if (ModelState.IsValid)
            {
                _context.Add(prontuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Records", "Admin");
            }

            // Se der erro, recarrega as listas
            ViewBag.Pacientes = _context.Pacientes.OrderBy(p => p.Nome).ToList();
            ViewBag.Medicamentos = _context.Medicamentos.OrderBy(m => m.Nome).ToList();
            return View(prontuario);
        }

        // GET: Prontuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario == null)
            {
                return NotFound();
            }

            // CORREÇÃO: Carrega as listas no ViewBag exatamente como a View Edit.cshtml espera
            ViewBag.Pacientes = await _context.Pacientes.OrderBy(p => p.Nome).ToListAsync();
            ViewBag.Medicamentos = await _context.Medicamentos.OrderBy(m => m.Nome).ToListAsync();

            return View(prontuario);
        }

        // POST: Prontuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CID,NumeroNotificacao,DataEmissao,TipoDeUso,PacienteId,MedicamentoId")] Prontuario prontuario)
        {
            if (id != prontuario.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Paciente");
            ModelState.Remove("Medicamento");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prontuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProntuarioExists(prontuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Records", "Admin");
            }

            // Se der erro de validação, recarrega as listas para não quebrar a tela
            ViewBag.Pacientes = await _context.Pacientes.OrderBy(p => p.Nome).ToListAsync();
            ViewBag.Medicamentos = await _context.Medicamentos.OrderBy(m => m.Nome).ToListAsync();

            return View(prontuario);
        }

        // POST: Prontuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario != null)
            {
                _context.Prontuarios.Remove(prontuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Records", "Admin");
        }

        private bool ProntuarioExists(int id)
        {
            return _context.Prontuarios.Any(e => e.Id == id);
        }
    }
}