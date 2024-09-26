using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosRandomUserGenerator.Context;
using UsuariosRandomUserGenerator.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text;
using Newtonsoft.Json.Linq;

// Controlador para gerenciar as operações relacionadas aos usuários.
namespace UsuariosRandomUserGenerator.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;
        private const int PageSize = 5; // Número de usuários por página

        // Construtor que injeta o contexto do banco de dados.
        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        // Ação GET para exibir a lista de usuários com paginação.
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var usuarios = await _context.Usuarios!
                .OrderBy(u => u.Nome) // Ordenar usuários pelo nome
                .Skip((pageNumber - 1) * PageSize) // Pular as páginas anteriores
                .Take(PageSize) // Pegar apenas o número da página atual
                .ToListAsync();

            // Calcula o número total de páginas
            ViewData["TotalPages"] = Math.Ceiling((double)_context.Usuarios!.Count() / PageSize);
            ViewData["CurrentPage"] = pageNumber;

            return View(usuarios);
        }

        // Ação POST para salvar um usuário (novo ou existente).
        [HttpPost]
        public async Task<JsonResult> Salvar(UsuarioModel usuario)
        {
            try
            {
                if (usuario.IdUsuario == 0) // Verifica se é um novo usuário
                {
                    await _context.AddAsync(usuario);
                }
                else // Edita um usuário existente
                {
                    _context.Update(usuario);
                }

                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Lida com exceções, se necessário, registra o erro
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Ação GET para obter os detalhes de um usuário por ID.
        [HttpGet]
        public async Task<JsonResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios!.FindAsync(id);
            if (usuario == null)
            {
                return Json(null);  // Retorna nulo se o usuário não for encontrado
            }
            return Json(usuario);  // Retorna o usuário encontrado
        }

        // Ação POST para excluir um usuário pelo ID.
        [HttpPost]
        public async Task<JsonResult> Excluir(int id)
        {
            var usuario = await _context.Usuarios!.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);  // Remove o usuário do contexto
                await _context.SaveChangesAsync();  // Salva as alterações
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Usuário não encontrado." });
        }

        // Ação GET para contar o total de usuários na base de dados.
        [HttpGet]
        public async Task<JsonResult> TotalUsuarios()
        {
            var total = await _context.Usuarios!.CountAsync();
            return Json(new { total });  // Retorna o total de usuários
        }

        // Ação POST para adicionar usuários aleatórios usando uma API externa.
        [HttpPost]
        public async Task<JsonResult> AddRandomUsers()
        {
            try
            {
                int usersToAdd = 5; // Número de usuários aleatórios a serem adicionados

                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://randomuser.me/api/?results={usersToAdd}");

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false });

                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);

                var randomUsers = json["results"]!.ToObject<List<JObject>>();

                // Valida se o email já está cadastrado
                foreach (var userJson in randomUsers!)
                {
                    var email = userJson["email"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(email) && !_context.Usuarios!.Any(u => u.Email == email))
                    {
                        var name = userJson["name"];
                        var firstName = name?["first"]?.ToString() ?? "";
                        var lastName = name?["last"]?.ToString() ?? "";

                        var user = new UsuarioModel
                        {
                            Nome = firstName,
                            Sobrenome = lastName,
                            Senha = SenhaAleatoria(),   // Gera uma senha aleatória
                            Email = email,
                            Telefone = userJson["phone"]?.ToString() ?? "",
                            Genero = ValidarGenero(userJson["gender"]?.ToString() ?? "")
                        };
                        _context.Usuarios!.Add(user);   // Adiciona o novo usuário ao contexto
                    }
                }

                await _context.SaveChangesAsync();   // Salva as alterações no banco de dados

            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                return Json(new { success = false });
            }

            return Json(new { success = true });  // Retorna sucesso ao final

        }

        // Ação GET para gerar um relatório em PDF dos usuários.
        [HttpGet]
        public IActionResult GerarRelatorioPDF()
        {
            var usuarios = _context.Usuarios!.OrderBy(x => x.Nome).ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                PdfDocument pdf = new PdfDocument();
                PdfPage page = pdf.AddPage();

                // Define a orientação da página para paisagem
                page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Define as fontes
                XFont titleFont = new XFont("Verdana", 12, XFontStyle.Bold);
                XFont dataFont = new XFont("Verdana", 9, XFontStyle.Regular);

                int yPos = 50;

                // Desenha o título do relatório
                gfx.DrawString("Relatório de Usuários", titleFont, XBrushes.Black, new XPoint(50, yPos));

                yPos += 20;

                // Definindo posições x para cada coluna
                double nomeX = 50;
                double sobrenomeX = 150;
                double senhaX = 250;
                double emailX = 380;
                double telefoneX = 590;
                double generoX = 700;

                // Desenha os cabeçalhos das colunas
                gfx.DrawString("Nome", dataFont, XBrushes.Black, new XPoint(nomeX, yPos));
                gfx.DrawString("Sobrenome", dataFont, XBrushes.Black, new XPoint(sobrenomeX, yPos));
                gfx.DrawString("Senha", dataFont, XBrushes.Black, new XPoint(senhaX, yPos));
                gfx.DrawString("Email", dataFont, XBrushes.Black, new XPoint(emailX, yPos));
                gfx.DrawString("Telefone", dataFont, XBrushes.Black, new XPoint(telefoneX, yPos));
                gfx.DrawString("Genero", dataFont, XBrushes.Black, new XPoint(generoX, yPos));

                yPos += 20;

                // Desenha os dados dos usuários
                foreach (var usuario in usuarios)
                {
                    gfx.DrawString(usuario.Nome, dataFont, XBrushes.Black, new XPoint(nomeX, yPos));
                    gfx.DrawString(usuario.Sobrenome, dataFont, XBrushes.Black, new XPoint(sobrenomeX, yPos));
                    gfx.DrawString(usuario.Senha, dataFont, XBrushes.Black, new XPoint(senhaX, yPos));
                    gfx.DrawString(usuario.Email, dataFont, XBrushes.Black, new XPoint(emailX, yPos));
                    gfx.DrawString(usuario.Telefone, dataFont, XBrushes.Black, new XPoint(telefoneX, yPos));
                    gfx.DrawString(usuario.Genero, dataFont, XBrushes.Black, new XPoint(generoX, yPos));

                    yPos += 20;  // Atualiza a posição y para a próxima linha
                }

                pdf.Save(stream, false);  // Salva o PDF no stream
                stream.Position = 0; 

                return File(stream.ToArray(), "application/pdf");  // Retorna o arquivo PDF
            }
        }

        // Função para gerar uma senha aleatória
        private string SenhaAleatoria()
        {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var senha = new StringBuilder();

            // Gerar uma senha com até 15 caracteres
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(caracteresPermitidos.Length);
                senha.Append(caracteresPermitidos[index]);
            }
            return senha.ToString();
        }

        // Função para validar o gênero
        private string ValidarGenero(string genero)
        {
            if (genero.Equals("male", StringComparison.OrdinalIgnoreCase))
            {
                return "Masculino";
            }
            else if (genero.Equals("female", StringComparison.OrdinalIgnoreCase))
            {
                return "Feminino";
            }
            else
            {
                return "Outro"; // Para outros gêneros
            }
        }
    }
}


