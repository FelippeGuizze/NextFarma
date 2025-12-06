using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NextFarma.Controllers
{
    // O nome do Controller deve terminar em "Controller"
    public class ValidadorController : Controller
    {
        // O método Index() é a ação padrão que responde à URL /Validador
        public IActionResult Index()
        {
            // Retorna a View localizada em Views/Validador/Index.cshtml
            return View();
        }

        // Se você precisar de lógica de processamento aqui (ex: Validação, Submissão), ela viria aqui:
        /*
        [HttpPost]
        public IActionResult ValidarPrescricao(SeuModeloDeDados modelo)
        {
            // Lógica de validação...
            return View("Index", modelo);
        }
        */
    }
}