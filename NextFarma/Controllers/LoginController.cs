using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using System.Linq;
using NextFarma.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> Logar(Usuario model)
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
                    result = PasswordVerificationResult.Failed;
                }

                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        usuario.Senha = hasher.HashPassword(usuario, model.Senha);
                        _context.SaveChanges();
                    }

                    // Create claims and sign-in cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.Role, usuario.Type.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }

                // Fallback: if stored password is still plaintext and equals provided password, accept and rehash
                if (usuario.Senha == model.Senha)
                {
                    usuario.Senha = hasher.HashPassword(usuario, model.Senha);
                    _context.SaveChanges();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.Role, usuario.Type.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View("Index");
        }
    }
}