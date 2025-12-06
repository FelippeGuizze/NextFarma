using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using System.Linq;
using NextFarma.Models;
using Microsoft.AspNetCore.Identity;

namespace NextFarma.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logar(Usuario model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Senha))
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View("Index");
            }

            var email = model.Email.Trim().ToLower();
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == email);

            if (usuario != null)
            {
                var hasher = new PasswordHasher<Usuario>();
                var result = PasswordVerificationResult.Failed;

                // Try verify assuming stored value is a hash
                try
                {
                    result = hasher.VerifyHashedPassword(usuario, usuario.Senha, model.Senha);
                }
                catch
                {
                    // If VerifyHashedPassword throws (stored value not a valid hash), we'll fall back to plaintext check below
                    result = PasswordVerificationResult.Failed;
                }

                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        usuario.Senha = hasher.HashPassword(usuario, model.Senha);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Home");
                }

                // Fallback: if stored password is still plaintext and equals provided password, accept and rehash
                if (usuario.Senha == model.Senha)
                {
                    usuario.Senha = hasher.HashPassword(usuario, model.Senha);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View("Index");
        }
    }
}