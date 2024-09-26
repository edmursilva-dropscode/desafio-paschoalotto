using Microsoft.EntityFrameworkCore;
using UsuariosRandomUserGenerator.Context;

var builder = WebApplication.CreateBuilder(args);

// Configurar a conexão com o banco de dados PostgreSQL usando a string de conexão definida no arquivo de configuração
builder.Services.AddDbContext<Contexto>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar os serviços necessários para suportar controladores e views (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{    
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//Rdirecionamento, configuração e habilitação de recursos
app.UseHttpsRedirection();   

app.UseStaticFiles();  

app.UseRouting(); 

app.UseAuthorization(); 

// Mapeia as rotas para os controladores. Exemplo: /Home/Index/{id?}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia a aplicação
app.Run(); 
