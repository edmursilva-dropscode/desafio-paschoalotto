using UsuariosRandomUserGenerator.Models;
using Microsoft.EntityFrameworkCore;

// Namespace que contém o contexto do Entity Framework para a aplicação
namespace UsuariosRandomUserGenerator.Context
{
    // Classe que representa o contexto do banco de dados
    public class Contexto : DbContext
    {
        // Construtor que recebe opções de configuração do DbContext
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        // Propriedade que representa a tabela de usuários no banco de dados
        public DbSet<UsuarioModel>? Usuarios { get; set; } = null;  // Conjunto de usuários

        // Configuração do modelo para alterar o nome da tabela no banco de dados
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeia a entidade UsuarioModel para a tabela "Usuario"
            modelBuilder.Entity<UsuarioModel>().ToTable("Usuario");
        }

    }
}
