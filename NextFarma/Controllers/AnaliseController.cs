using Microsoft.AspNetCore.Mvc;
using NextFarma.Services;
using NextFarma.Models;

namespace NextFarma.Controllers
{
    public class AnaliseController : Controller
    {
        private readonly PdfAnalyzerService _service;

        public AnaliseController()
        {
            _service = new PdfAnalyzerService();
        }

        // GET: Exibe a página de upload
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Analisar(IFormFile arquivoPdf, bool debug = false)
        {
            if (arquivoPdf == null || arquivoPdf.Length == 0)
                return RedirectToAction("Index");

            if (debug)
            {
                PdfDebugViewModel debugModel;
                using (var stream = arquivoPdf.OpenReadStream())
                {
                    debugModel = _service.ExtractWordsWithCoords(stream);
                }
                return View("Debug", debugModel);
            }

            AnaliseReceitaViewModel resultado;

            // Abre a stream do arquivo enviado
            using (var stream = arquivoPdf.OpenReadStream())
            {
                resultado = _service.AnalisarPdf(stream);
            }

            // Redireciona para a view de resultados
            return View("Resultado", resultado);
        }
    }
}
