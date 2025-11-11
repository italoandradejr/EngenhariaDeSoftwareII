using LojaCupcakes.Data;
using LojaCupcakes.Models;
using LojaCupcakes.Models.ViewModels; // Importar o ViewModel
using Microsoft.AspNetCore.Authorization; // Importar
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Importar

namespace LojaCupcakes.Controllers
{
    public class PedidoController : Controller
    {
        private readonly LojaDbContext _context;

        public PedidoController(LojaDbContext context)
        {
            _context = context;
        }

        // --- PASSO 1: EXIBIR A PÁGINA DO CARRINHO (HU08) --- teste de commit
        [HttpGet]
        public IActionResult Carrinho()
        {
            // A view (Carrinho.cshtml) vai carregar
            // O JavaScript vai preencher os itens do localStorage
            return View();
        }

        // --- PASSO 2: FINALIZAR O PEDIDO (HU03) ---
        [HttpPost]
        [Authorize] // RN#1 da HU03: Só usuários logados podem finalizar
        public async Task<IActionResult> Finalizar([FromBody] List<CartItemViewModel> itensCarrinho)
        {
            if (itensCarrinho == null || !itensCarrinho.Any())
            {
                // RN#1 da HU08: Bloqueia finalização se carrinho estiver vazio
                return BadRequest("O carrinho está vazio.");
            }

            // Pega o ID do cliente logado
            var clienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // --- Verificação de Segurança: Busca os preços REAIS no banco ---
            decimal valorTotalPedido = 0;
            var listaItensPedido = new List<ItemPedido>();

            foreach (var itemVM in itensCarrinho)
            {
                var cupcakeDoBanco = await _context.Cupcakes.FindAsync(itemVM.Id);

                if (cupcakeDoBanco == null)
                {
                    return BadRequest("Produto não encontrado.");
                }

                var itemPedido = new ItemPedido
                {
                    CupcakeId = cupcakeDoBanco.Id,
                    Quantidade = itemVM.Quantidade,
                    PrecoUnitario = cupcakeDoBanco.Preco // Preço do banco, não do front-end!
                };

                listaItensPedido.Add(itemPedido);
                valorTotalPedido += (itemPedido.PrecoUnitario * itemPedido.Quantidade);
            }

            // Cria o pedido principal
            var novoPedido = new Pedido
            {
                ClienteId = clienteId,
                ValorTotal = valorTotalPedido,
                DataPedido = DateTime.Now,
                Status = "Processando",
                ItensPedido = listaItensPedido // Adiciona a lista de itens
            };

            // Salva tudo no banco
            _context.Pedidos.Add(novoPedido);
            await _context.SaveChangesAsync();

            // Retorna sucesso e a URL para redirecionar
            return Ok(new { redirectUrl = Url.Action("Confirmacao") });
        }

        // --- PASSO 3: PÁGINA DE CONFIRMAÇÃO ---
        [HttpGet]
        [Authorize]
        public IActionResult Confirmacao()
        {
            return View();
        }
    }
}