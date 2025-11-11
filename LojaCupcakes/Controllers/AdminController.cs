using LojaCupcakes.Data;
using LojaCupcakes.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // Importar
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LojaCupcakes.Controllers
{
    public class AdminController : Controller
    {
        private readonly LojaDbContext _context;

        public AdminController(LojaDbContext context)
        {
            _context = context;
        }

        // --- LOGIN DO ADMIN (HU05) ---

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            // RN da HU05: Login fixo
            if (email == "admin@cupcake.com" && senha == "admin123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Administrador"),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Admin") // Define a "Role" (papel)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index"); // Redireciona para o painel
            }

            ViewData["Erro"] = "Credenciais de administrador inválidas.";
            return View();
        }

        // --- PAINEL DO ADMIN (Dashboard) ---
        [Authorize(Roles = "Admin")] // Só acessa se for Admin
        public async Task<IActionResult> Index()
        {
            // Lista os cupcakes e pedidos (HU12)
            var cupcakes = await _context.Cupcakes.ToListAsync();
            // (Futuramente, adicione a lista de pedidos aqui)
            return View(cupcakes);
        }

        // --- CADASTRAR CUPCAKE (HU10) ---

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Mostra o formulário de cadastro
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Sabor,Preco,Imagem")] Cupcake cupcake)
        {
            // RN da HU10: Preço deve ser > 0 (o Model já faz isso com [Range])
            if (ModelState.IsValid)
            {
                _context.Add(cupcake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Volta ao painel
            }
            // Se houver erro de validação (ex: preço 0), continua na página
            return View(cupcake);
        }

        // --- REMOVER CUPCAKE (HU11) ---
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cupcake = await _context.Cupcakes.FindAsync(id);
            if (cupcake != null)
            {
                _context.Cupcakes.Remove(cupcake);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        // ... (Dentro da classe AdminController) ...

        // --- GERENCIAR PEDIDOS (HU12) ---

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GerenciarPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente) // Inclui o Cliente para pegar o nome
                .Include(p => p.ItensPedido) // Inclui os itens
                    .ThenInclude(item => item.Cupcake) // Inclui os detalhes do Cupcake
                .OrderByDescending(p => p.DataPedido) // Mais recentes primeiro
                .ToListAsync();

            return View(pedidos);
        }

        // --- ATUALIZAR STATUS DO PEDIDO ---

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarStatusPedido(int pedidoId, string status)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido != null)
            {
                pedido.Status = status;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }

            // Volta para a lista de pedidos
            return RedirectToAction(nameof(GerenciarPedidos));
        }
    }
}