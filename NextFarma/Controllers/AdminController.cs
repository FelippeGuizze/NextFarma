using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextFarma.Data;
using NextFarma.Models;

namespace NextFarma.Controllers
{
    [Authorize(Roles = "Adm")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                Pacientes = await _context.Pacientes.ToListAsync(),
                Prontuarios = await _context.Prontuarios.Include(p => p.Medicamento).Include(p => p.Paciente).ToListAsync(),
                Medicamentos = await _context.Medicamentos.Include(m => m.Categoria).ToListAsync()
            };

            return View(model);
        }

        // PATIENTS
        public async Task<IActionResult> Patients(string q, int page = 1, int pageSize = 10)
        {
            var query = _context.Pacientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Nome.Contains(q) || p.RG.Contains(q) || p.Telefone.Contains(q));
            }

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var list = await query.OrderBy(p => p.Nome)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = q;

            return View(list);
        }

        public IActionResult CreatePatient()
        {
            return View(new Paciente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatient(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        public async Task<IActionResult> EditPatient(int id)
        {
            var p = await _context.Pacientes.FindAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(int id, Paciente paciente)
        {
            if (id != paciente.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _context.Update(paciente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente atualizado.";
                return RedirectToAction(nameof(Patients));
            }
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var p = await _context.Pacientes.FindAsync(id);
            if (p != null)
            {
                bool hasRecords = await _context.Prontuarios.AnyAsync(r => r.PacienteId == id);
                if (hasRecords)
                {
                    TempData["Error"] = "Não é possível excluir o paciente pois existem prontuários relacionados.";
                    return RedirectToAction(nameof(Patients));
                }

                _context.Pacientes.Remove(p);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente excluído.";
            }
            return RedirectToAction(nameof(Patients));
        }

        // MEDICINES
        public async Task<IActionResult> Medicines(string q, int page = 1, int pageSize = 10)
        {
            var query = _context.Medicamentos.Include(m => m.Categoria).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(m => m.Nome.Contains(q) || m.Concentracao.Contains(q) || m.Categoria.Lista.Contains(q));
            }

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var meds = await query.OrderBy(m => m.Nome)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = q;

            return View(meds);
        }

        public async Task<IActionResult> CreateMedicine()
        {
            // Redirect to the dedicated controller's create page
            return RedirectToAction("Create", "Medicamentos");
        }

        public async Task<IActionResult> EditMedicine(int id)
        {
            var m = await _context.Medicamentos.FindAsync(id);
            if (m == null) return NotFound();
            ViewBag.Categorias = await _context.CategoriasMedicamento.ToListAsync();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(int id, Medicamento model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _context.Medicamentos.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento atualizado.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = await _context.CategoriasMedicamento.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var m = await _context.Medicamentos.FindAsync(id);
            if (m != null)
            {
                bool hasRecords = await _context.Prontuarios.AnyAsync(r => r.MedicamentoId == id);
                if (hasRecords)
                {
                    TempData["Error"] = "Não é possível excluir o medicamento pois existem prontuários relacionados.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Medicamentos.Remove(m);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento excluído.";
            }
            return RedirectToAction(nameof(Index));
        }

        // RECORDS (PRONTUARIOS)
        public async Task<IActionResult> Records(string q, int page = 1, int pageSize = 10)
        {
            var query = _context.Prontuarios.Include(p => p.Paciente).Include(p => p.Medicamento).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(r => r.NumeroNotificacao.Contains(q) || r.CID.Contains(q) || r.Paciente.Nome.Contains(q));
            }

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var items = await query.OrderByDescending(p => p.DataEmissao)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = q;

            return View(items);
        }

        public async Task<IActionResult> CreateRecord()
        {
            // Redirect to the dedicated controller's create page
            return RedirectToAction("Create", "Prontuarios");
        }

        public async Task<IActionResult> EditRecord(int id)
        {
            var item = await _context.Prontuarios.FindAsync(id);
            if (item == null) return NotFound();
            ViewBag.Pacientes = await _context.Pacientes.ToListAsync();
            ViewBag.Medicamentos = await _context.Medicamentos.ToListAsync();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecord(int id, Prontuario model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _context.Prontuarios.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Prontuário atualizado.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Pacientes = await _context.Pacientes.ToListAsync();
            ViewBag.Medicamentos = await _context.Medicamentos.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var item = await _context.Prontuarios.FindAsync(id);
            if (item != null)
            {
                _context.Prontuarios.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Prontuário excluído.";
            }
            return RedirectToAction(nameof(Index));
        }

        // CATEGORIES
        public async Task<IActionResult> Categories()
        {
            var cats = await _context.CategoriasMedicamento.ToListAsync();
            return View(cats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var cat = await _context.CategoriasMedicamento.FindAsync(id);
            if (cat != null)
            {
                bool used = await _context.Medicamentos.AnyAsync(m => m.CategoriaId == id);
                if (used)
                {
                    TempData["Error"] = "Não é possível excluir a categoria pois existem medicamentos vinculados.";
                    return RedirectToAction(nameof(Categories));
                }

                _context.CategoriasMedicamento.Remove(cat);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoria excluída.";
            }
            return RedirectToAction(nameof(Categories));
        }
    }
}
