using Newtonsoft.Json;

// Classe que representa um modelo de mensagem, incluindo tipos de mensagens e métodos para serializar e desserializar mensagens entre páginas.
namespace UsuariosRandomUserGenerator.Models
{
    //Tipos de mensagens
    public enum TipoMensagem
    {
        Informacao,
        Erro
    }

    public class MensagemModel
    {
        //Propriedades
        public TipoMensagem Tipo { get; set; }
        public string Texto { get; set; }

        //Contrutor
        public MensagemModel(string mensagem, TipoMensagem tipo = TipoMensagem.Informacao)
        {
            Tipo = tipo;
            Texto = mensagem;
        }
        
        //Metodo para serializar/converter um objeto para string
        public static string Serializar(string mensagem, TipoMensagem tipo = TipoMensagem.Informacao)
        {
            var mensagemModel = new MensagemModel(mensagem, tipo);
            return JsonConvert.SerializeObject(mensagemModel);
        }

        //Metodo para deserializar/converter uma string para objeto
        public static MensagemModel? Desserializar(string mensagemString)
        {
            return JsonConvert.DeserializeObject<MensagemModel>(mensagemString);
        }

    }
}
