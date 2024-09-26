using Microsoft.AspNetCore.Mvc;

// Controlador responsável pela lógica do Dashboard, gerenciando a exibição da página inicial do Dashboard.
namespace UsuariosRandomUserGenerator.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Retorna a visualização associada ao Dashboard.

        }
    }
}
