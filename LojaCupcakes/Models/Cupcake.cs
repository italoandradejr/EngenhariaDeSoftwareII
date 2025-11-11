using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaCupcakes.Models
{
    public class Cupcake
    {
        [Key] // Define que esta é a Chave Primária
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")] // Validação (HU15)
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string? Sabor { get; set; } // '?' permite que o sabor seja nulo

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(10, 2)")] // Garante o tipo correto (mesmo do SQL)
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")] // RN#1 da HU10
        public decimal Preco { get; set; }

        [StringLength(255)]
        [Display(Name = "Caminho da Imagem")]
        public string? Imagem { get; set; }
    }
}