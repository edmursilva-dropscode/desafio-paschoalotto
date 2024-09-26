using Microsoft.EntityFrameworkCore;
using UsuariosRandomUserGenerator.Context;

var builder = WebApplication.CreateBuilder(args);

// Configurar a conex�o com o banco de dados PostgreSQL usando a string de conex�o definida no arquivo de configura��o
builder.Services.AddDbContext<Contexto>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar os servi�os necess�rios para suportar controladores e views (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
if (!app.Environment.IsDevelopment())
{    
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//Rdirecionamento, configura��o e habilita��o de recursos
app.UseHttpsRedirection();   

app.UseStaticFiles();  

app.UseRouting(); 

app.UseAuthorization(); 

// Mapeia as rotas para os controladores. Exemplo: /Home/Index/{id?}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia a aplica��o
app.Run(); 
