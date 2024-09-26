using Microsoft.AspNetCore.Mvc;
using UsuariosRandomUserGenerator.Models;
using System.Diagnostics;

// Controlador principal da aplica��o, gerencia a p�gina inicial e o tratamento de erros.
namespace UsuariosRandomUserGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Construtor que injeta um logger para registrar informa��es e erros.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // A��o que retorna a visualiza��o da p�gina inicial.
        public IActionResult Index()
        {
            return View();
        }

        // A��o para exibir uma p�gina de erro, incluindo o RequestId para rastreamento.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}