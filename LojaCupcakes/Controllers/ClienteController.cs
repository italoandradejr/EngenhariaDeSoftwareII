using LojaCupcakes.Data;
using LojaCupcakes.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LojaCupcakes.Controllers
{
    public class ClienteController : Controller
    {
        private readonly LojaDbContext _context;

        public ClienteController(LojaDbContext context)
        {
            _context = context;
        }

        // --- CADASTRO (HU04) ---
        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro([Bind("Nome,Email,Senha")] Cliente cliente)
        {
            // RN#1 da HU04: O email não pode estar repetido
            if (await _context.Clientes.AnyAsync(c => c.Email == cliente.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está em uso.");
            }

            if (ModelState.IsValid)
            {
                // Criptografa a senha antes de salvar
                cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);

                _context.Add(cliente);
                await _context.SaveChangesAsync();

                // Redireciona para o Login
                return RedirectToAction(nameof(Login));
            }
            return View(cliente);
        }

        // --- LOGIN (HU04) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

            // Verifica se o cliente existe E se a senha criptografada bate
            if (cliente != null && BCrypt.Net.BCrypt.Verify(senha, cliente.Senha))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, cliente.Nome),
                    new Claim(ClaimTypes.Email, cliente.Email),
                    new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Cliente") 
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home"); // Volta para a vitrine
            }

            ViewData["Erro"] = "E-mail ou senha inválidos.";
            return View();
        }

        [Authorize] // Só pode deslogar quem está logado
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // HU06: Visualizar pedidos anteriores
        [Authorize(Roles = "Cliente")] // Só clientes logados podem ver
        [HttpGet]
        public async Task<IActionResult> MeusPedidos()
        {
            // 1. Pega o ID do cliente logado (CA#1)
            var clienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 2. Busca os pedidos no banco
            var pedidos = await _context.Pedidos
                // Filtra apenas os pedidos DO cliente logado
                .Where(p => p.ClienteId == clienteId)

                // Inclui os "ItensPedido" relacionados
                .Include(p => p.ItensPedido)

                // Dentro dos Itens, inclui o "Cupcake" (para sabermos o nome)
                .ThenInclude(item => item.Cupcake)

                // Ordena pelos mais recentes primeiro
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();

            // 3. Envia a lista de pedidos para a View
            return View(pedidos);
        }

        // Ação para a página de Acesso Negado
        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}