using LojaCupcakes.Data;
using LojaCupcakes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore; // Necessário para o ToListAsync
using System.Diagnostics;

namespace LojaCupcakes.Controllers
{
    public class HomeController : Controller
    {
        private readonly LojaDbContext _context; 

        public HomeController(LojaDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            // Busca todos os cupcakes no banco de dados
            var cupcakes = await _context.Cupcakes.ToListAsync();

           
            return View(cupcakes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}