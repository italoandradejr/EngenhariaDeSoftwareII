using LojaCupcakes.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore; // para o UseMySql

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona a conexão com o Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<LojaDbContext>(options =>
    
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// 2. Adiciona os serviços do MVC
builder.Services.AddControllersWithViews();

// 3. Adiciona serviços de Autenticação (Cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cliente/Login";
        options.AccessDeniedPath = "/Cliente/AcessoNegado";
    });

// 4. Adiciona services de Sessão (para o carrinho)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Habilita a Sessão

// 5. Habilita Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();