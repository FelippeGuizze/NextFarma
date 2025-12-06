using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NextFarma.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        // CORREÇÃO: Aceita nulo (string?) para evitar o aviso do compilador
        private string? ApenasNumeros(string? texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;
            return Regex.Replace(texto, @"\D", "");
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pacientes.ToListAsync());
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View(new Paciente());
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            // 1. Limpa os dados (Remove máscaras)
            paciente.RG = ApenasNumeros(paciente.RG);
            paciente.Telefone = ApenasNumeros(paciente.Telefone);
            paciente.CEP = ApenasNumeros(paciente.CEP);

            // 2. Remove erros antigos do ModelState (pois eles validaram o texto com pontos)
            ModelState.Remove("RG");
            ModelState.Remove("Telefone");
            ModelState.Remove("CEP");

            // 3. Revalida o objeto limpo
            if (TryValidateModel(paciente))
            {
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente criado com sucesso.";
                return RedirectToAction("Patients", "Admin");
            }

            // Se falhar, volta para a tela (os campos estarão limpos, sem formatação)
            return View(paciente);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            return View(paciente);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            if (id != paciente.Id) return NotFound();

            // 1. Limpa os dados
            paciente.RG = ApenasNumeros(paciente.RG);
            paciente.Telefone = ApenasNumeros(paciente.Telefone);
            paciente.CEP = ApenasNumeros(paciente.CEP);

            // 2. Remove erros antigos
            ModelState.Remove("RG");
            ModelState.Remove("Telefone");
            ModelState.Remove("CEP");

            // 3. Revalida
            if (TryValidateModel(paciente))
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id)) return NotFound();
                    else throw;
                }
                TempData["Success"] = "Paciente atualizado com sucesso.";
                return RedirectToAction("Patients", "Admin");
            }
            return View(paciente);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente excluído com sucesso.";
            }
            return RedirectToAction("Patients", "Admin");
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}