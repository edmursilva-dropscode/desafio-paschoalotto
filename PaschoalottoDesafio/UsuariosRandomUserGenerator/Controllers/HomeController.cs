using Microsoft.AspNetCore.Mvc;
using UsuariosRandomUserGenerator.Models;
using System.Diagnostics;

// Controlador principal da aplicação, gerencia a página inicial e o tratamento de erros.
namespace UsuariosRandomUserGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Construtor que injeta um logger para registrar informações e erros.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Ação que retorna a visualização da página inicial.
        public IActionResult Index()
        {
            return View();
        }

        // Ação para exibir uma página de erro, incluindo o RequestId para rastreamento.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}