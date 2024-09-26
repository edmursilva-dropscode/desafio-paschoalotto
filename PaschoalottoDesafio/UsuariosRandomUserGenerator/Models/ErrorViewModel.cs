// Classe que representa o modelo de erro, contendo um ID de requisi��o opcional e uma propriedade para determinar se o ID deve ser exibido.
namespace UsuariosRandomUserGenerator.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
