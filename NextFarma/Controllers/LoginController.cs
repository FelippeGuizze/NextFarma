using Microsoft.AspNetCore.Mvc;
using NextFarma.Data;
using System.Linq;

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
        public IActionResult Logar(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                // -------------------------------------------------------------------------
                // COMENTÁRIO: É AQUI QUE VOCÊ MUDA A PÁGINA DE DESTINO APÓS O LOGIN
                // O primeiro parâmetro é a Action ("Index")
                // O segundo parâmetro é o Controller ("Home")
                // Se quiser mandar para Dashboard, mudaria para: RedirectToAction("Index", "Dashboard");
                // -------------------------------------------------------------------------
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Se errar a senha, cria a mensagem de erro
                ViewBag.Erro = "Usuário ou senha inválidos!";

                // Retorna para a mesma tela de Login ("Index") para mostrar o erro
                return View("Index");
            }
        }
    }
}