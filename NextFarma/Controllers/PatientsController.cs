using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using NextFarma.Models;
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

        public IActionResult Create()
        {
            return View(new Paciente());
        }

        // PatientsController.cs (Trecho relevante)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            // O ModelBinder preencherá Nome, Idade, RG, CEP, Endereco, Telefone e Sexo
            // baseando-se nos 'input names' do formulário acima.
            if (ModelState.IsValid)
            {
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Paciente criado com sucesso.";
                return RedirectToAction("Index", "Admin");
            }
            return View(paciente);
        }
    }

}
