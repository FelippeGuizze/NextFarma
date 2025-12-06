using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using NextFarma.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

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

                // Simple password strength: at least one uppercase and one digit
                if (string.IsNullOrEmpty(usuario.Senha) || !Regex.IsMatch(usuario.Senha, @"(?=.*[A-Z])(?=.*\d)"))
                {
                    ModelState.AddModelError("Senha", "A senha deve conter pelo menos uma letra maiúscula e um número.");
                    return View("Index", usuario);
                }

                var hasher = new PasswordHasher<Usuario>();
                usuario.Senha = hasher.HashPassword(usuario, usuario.Senha);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Após cadastrar, manda para o Login
                return RedirectToAction("Index", "Login");
            }
            return View("Index", usuario);
        }
    }
}