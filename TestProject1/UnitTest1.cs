using LojaCupcakes.Controllers;
using LojaCupcakes.Data;
using LojaCupcakes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

/* * ESTE ARQUIVO SIMULA TESTES UNITÁRIOS PARA O LOGIN (HU04)
 */

public class ClienteControllerTests
{
    private LojaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<LojaDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Banco em memória
            .Options;
        var context = new LojaDbContext(options);

        // Adiciona um cliente falso para o teste
        var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword("senha123");
        context.Clientes.Add(new Cliente
        {
            Id = 1,
            Nome = "Cliente Teste",
            Email = "teste@email.com",
            Senha = senhaCriptografada
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Login_Sucesso_RedirecionaParaHome()
    {
        // Arrange (Organizar)
        var context = GetInMemoryDbContext();
        var controller = new ClienteController(context);

        // Simula o HttpContext (necessário para o SignInAsync)
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.SignInAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                       .Returns(Task.CompletedTask);
        controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock.Object };

        // Act (Agir)
        var result = await controller.Login("teste@email.com", "senha123") as RedirectToActionResult;

        // Assert (Verificar)
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Home", result.ControllerName);
    }

    [Fact]
    public async Task Login_Falha_SenhaIncorreta()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ClienteController(context);

        // Act
        var result = await controller.Login("teste@email.com", "senha_errada") as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("E-mail ou senha inválidos.", result.ViewData["Erro"]);
    }

    [Fact]
    public async Task Login_Falha_UsuarioNaoExiste()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ClienteController(context);

        // Act
        var result = await controller.Login("naoexiste@email.com", "senha123") as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("E-mail ou senha inválidos.", result.ViewData["Erro"]);
    }
}