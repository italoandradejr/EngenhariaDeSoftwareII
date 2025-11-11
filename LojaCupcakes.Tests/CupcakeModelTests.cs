using LojaCupcakes.Models; // Importa seu Model
using System.ComponentModel.DataAnnotations; // Necessário para a validação
using Xunit; // Pacote do xUnit

namespace LojaCupcakes.Tests
{
    public class CupcakeModelTests
    {
        // Função helper para validar o modelo
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }

        [Fact] // Define que este é um método de teste
        public void Cupcake_Preco_DeveSerMaiorQueZero()
        {
            // Arrange (Organizar)
            // Cria um cupcake que viola a regra de negócio
            var cupcake = new Cupcake
            {
                Nome = "Cupcake de Teste",
                Sabor = "Teste",
                Preco = 0.00m // Preço inválido
            };

            // Act (Agir)
            var validationResults = ValidateModel(cupcake);

            // Assert (Verificar)
            // Esperamos que haja 1 erro de validação
            Assert.Single(validationResults);
            // Esperamos que a mensagem de erro seja a que definimos no Model
            Assert.Equal("O preço deve ser maior que zero", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void Cupcake_Preco_Valido_NaoDeveGerarErro()
        {
            // Arrange (Organizar)
            // Cria um cupcake válido
            var cupcake = new Cupcake
            {
                Nome = "Cupcake de Teste Válido",
                Sabor = "Teste",
                Preco = 9.50m // Preço válido
            };

            // Act (Agir)
            var validationResults = ValidateModel(cupcake);

            // Assert (Verificar)
            // Esperamos que a lista de erros de validação esteja vazia
            Assert.Empty(validationResults);
        }
    }
}