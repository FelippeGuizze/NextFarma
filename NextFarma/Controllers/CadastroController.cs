using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using NextFarma.Models;
using System.Linq;

namespace NextFarma.Controllers
{
    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Cadastro (O "Index" próprio dele)
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Cadastro/Cadastrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verifica email duplicado
                if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                {
                    ModelState.AddModelError("Email", "Email já cadastrado.");
                    return View("Index", usuario); // Retorna para a View Index com erros
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Após cadastrar, manda para o Login
                return RedirectToAction("Index", "Login");
            }
            return View("Index", usuario);
        }
    }
}